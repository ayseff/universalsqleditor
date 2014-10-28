using System;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerExplainPlanGenerator : ExplainPlanGenerator
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

                    command.CommandText = "SET SHOWPLAN_ALL ON";
                    command.ExecuteNonQuery();

                    command.CommandText = sql;
                    using (var dr = command.ExecuteReader())
                    {
                        table.Load(dr);
                    }
                }
            }

            table.Columns["StmtText"].ReadOnly = false;
            foreach (DataRow row in table.Rows)
            {
                var value = ((string)row["StmtText"]).Trim();
                while (value.StartsWith("|") ||
                       value.StartsWith("-"))
                {
                    value = value.Substring(1).Trim();
                }
                row.SetField("StmtText", value);
            }

            // Parse table
            var explainPlanData = new SqlServerExplainPlanData();
            var rootRow = table.Rows.Cast<DataRow>().Where(x => ((int)x["Parent"]) == 0).ToList();
            if (rootRow.Count == 0)
            {
                throw new Exception("Could not find a starting point in the execution plan.");
            }
            var explainPlanRow = new ExplainPlanRow(rootRow.First());
            explainPlanData.Rows.Add(explainPlanRow);
            ParseExplainPlan(explainPlanRow, table, "NodeId", "Parent");
            return explainPlanData;
        }
    }
}