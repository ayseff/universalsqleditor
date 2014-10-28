using System;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.Oracle
{
    public class OracleExplainPlanGenerator : ExplainPlanGenerator
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
                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        try
                        {
                            command.CommandText = "SELECT * FROM PLAN_TABLE";
                            command.ExecuteReader(CommandBehavior.SchemaOnly);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Could not SELECT from PLAN_TABLE. " + ex.Message);
                        }

                        command.CommandText = "DELETE FROM PLAN_TABLE";
                        command.ExecuteNonQuery();
                        
                        command.CommandText = "EXPLAIN PLAN SET STATEMENT_ID = 'stats' FOR " + sql;
                        command.ExecuteNonQuery();

                        command.CommandText = "	SELECT 		" + Environment.NewLine;
                        command.CommandText += "		ID,	" + Environment.NewLine;
                        command.CommandText += "		PARENT_ID,	" + Environment.NewLine;
                        command.CommandText += "		OPERATION || ' ' ||  NVL(OPTIONS, '') as OPERATION,	" + Environment.NewLine;
                        command.CommandText += "		OBJECT_OWNER,	" + Environment.NewLine;
                        command.CommandText += "		OBJECT_NAME,	" + Environment.NewLine;
                        command.CommandText += "		ACCESS_PREDICATES,	" + Environment.NewLine;
                        command.CommandText += "		FILTER_PREDICATES,	" + Environment.NewLine;
                        command.CommandText += "		CARDINALITY,	" + Environment.NewLine;
                        command.CommandText += "		BYTES,	" + Environment.NewLine;
                        command.CommandText += "		COST,	" + Environment.NewLine;
                        command.CommandText += "		IO_COST,	" + Environment.NewLine;
                        command.CommandText += "		CPU_COST	" + Environment.NewLine;
                        command.CommandText += "	FROM 		" + Environment.NewLine;
                        command.CommandText += "		PLAN_TABLE	" + Environment.NewLine;
                        command.CommandText += "	ORDER BY		" + Environment.NewLine;
                        command.CommandText += "		ID	" + Environment.NewLine;

                        
                        using (var dr = command.ExecuteReader())
                        {
                            table.Load(dr);
                        }
                    }
                    transaction.Rollback();
                }
            }

            // Parse table
            var explainPlanData = new OracleExplainPlanData();
            var rootRow = table.Rows.Cast<DataRow>().Where(x => x.IsNull("PARENT_ID")).ToList();
            if (rootRow.Count == 0)
            {
                throw new Exception("Could not find a starting point in the execution plan.");
            }
            var explainPlanRow = new ExplainPlanRow(rootRow.First());
            explainPlanData.Rows.Add(explainPlanRow);
            ParseExplainPlan(explainPlanRow, table, "ID", "PARENT_ID");
            return explainPlanData;
        }
    }
}