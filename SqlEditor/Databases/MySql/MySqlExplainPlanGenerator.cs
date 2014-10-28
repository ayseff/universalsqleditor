using System;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.Databases.Db2;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.MySql
{
    public class MySqlExplainPlanGenerator : ExplainPlanGenerator
    {
        public override ExplainPlanData GetExplainPlan([NotNull] DatabaseConnection databaseConnection,
            [NotNull] string sql)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (sql == null) throw new ArgumentNullException("sql");

            var table = new DataTable();
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
            var explainPlanData = new MySqlExplainPlanData();
            var rootRow = table.Rows.Cast<DataRow>().ToList();
            if (rootRow.Count == 0)
            {
                throw new Exception("Could not find a starting point in the execution plan.");
            }
            explainPlanData.Rows.AddRange(table.Rows.Cast<DataRow>().Select(x => new ExplainPlanRow(x)));
            return explainPlanData;
        }
    }
}