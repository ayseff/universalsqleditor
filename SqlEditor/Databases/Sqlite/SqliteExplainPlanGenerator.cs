using System;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.Databases.Oracle;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteExplainPlanGenerator : ExplainPlanGenerator
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
                    command.CommandText = "EXPLAIN QUERY PLAN " + sql;
                    using (var dr = command.ExecuteReader())
                    {
                        table.Load(dr);
                    }
                }
            }

            // Parse table
            var explainPlanData = new SqliteExplainPlanData();
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