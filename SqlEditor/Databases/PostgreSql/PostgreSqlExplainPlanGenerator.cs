using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.PostgreSql
{
    public class PostgreSqlExplainPlanGenerator : ExplainPlanGenerator
    {
        private static readonly Regex _leadingSpaceRegex = new Regex(@"^\s+", RegexOptions.Compiled);
        private static readonly Regex _costRowRegex = new Regex(@"^(?<OperationName>.+?)\(\s*cost\s*=\s*(?<InitialCost>[\d\.]+)\s*\.\.\s*(?<TotalCost>[\d\.]+)\s*rows\s*=\s*(?<Rows>[\d\.]+)\s*width\s*=\s*(?<Width>[\d\.]+)\s*\)\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override ExplainPlanData GetExplainPlan([NotNull] DatabaseConnection databaseConnection,
            [NotNull] string sql)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (sql == null) throw new ArgumentNullException("sql");

            DataTable table;
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXPLAIN " + sql;
                    using (var dr = command.ExecuteReader())
                    {
                        var result = dr.FetchDataTable(int.MaxValue);
                        table = result.Result;
                    }
                }
            }

            // Parse table
            var resultTable = new DataTable();
            resultTable.Columns.Add("Operation", typeof (string));
            resultTable.Columns.Add("Startup_Cost", typeof(double));
            resultTable.Columns.Add("Total_Cost", typeof(double));
            resultTable.Columns.Add("Row_Count", typeof(long));
            resultTable.Columns.Add("Width", typeof(long));
            var explainPlanData = new PostgreSqlExplainPlanData();
            var dataRows = table.Rows.Cast<DataRow>().ToList();
            var rootRow = dataRows.FirstOrDefault();
            if (rootRow == null)
            {
                throw new Exception("Could not find a starting point in the execution plan.");
            }
            var rootExplainPlanRow = ParseSingleExplainPlanRow(rootRow, resultTable.NewRow());
            explainPlanData.Rows.Add(rootExplainPlanRow);
            var levelMap = new Dictionary<int, ExplainPlanRow>();
            var parent = rootExplainPlanRow;
            for (var i = 1; i < dataRows.Count; i++)
            {
                var data = ((string) dataRows[i][0]).Replace("->", "  ");
                var level = 0;
                var match = _leadingSpaceRegex.Match(data);
                if (match.Success)
                {
                    level = match.Value.Length;
                }
                // Remove non relevant subtree
                levelMap = levelMap.Where(x => x.Key <= level).ToDictionary(x => x.Key, y => y.Value);

                var row = ParseSingleExplainPlanRow(dataRows[i], resultTable.NewRow());
                if (!levelMap.ContainsKey(level))
                {
                    levelMap.Add(level, parent);
                    parent.ChildRows.Add(row);
                }
                else
                {
                    levelMap[level].ChildRows.Add(row);
                }
                parent = row;
            }
            return explainPlanData;
        }

        private static ExplainPlanRow ParseSingleExplainPlanRow(DataRow rootRow, DataRow newRow)
        {
            var data = (string)rootRow[0];
            data = data.Replace("->", string.Empty).Trim();
            var match = _costRowRegex.Match(data);
            if (match.Success)
            {
                var name = match.Groups["OperationName"].Value.Trim();
                var initialCost = double.Parse(match.Groups["InitialCost"].Value.Trim());
                var totalCost = double.Parse(match.Groups["TotalCost"].Value.Trim());
                var rows = long.Parse(match.Groups["Rows"].Value.Trim());
                var width = long.Parse(match.Groups["Width"].Value.Trim());
                newRow.ItemArray = new object[] { name, initialCost, totalCost, rows, width };
            }
            else
            {
                newRow.ItemArray = new object[] { data, null, null, null, null };   
            }
            return new ExplainPlanRow(newRow);
        }
    }
}
