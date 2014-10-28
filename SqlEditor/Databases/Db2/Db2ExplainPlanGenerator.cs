using System;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.Db2
{
    public class Db2ExplainPlanGenerator : ExplainPlanGenerator
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
                    var user = databaseConnection.DatabaseServer.GetUserId(databaseConnection.ConnectionString);
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;

                        try
                        {
                            command.CommandText = "SELECT * FROM " + user + ".EXPLAIN_INSTANCE";
                            using (var dr = command.ExecuteReader(CommandBehavior.SchemaOnly))
                            {
                                dr.Read();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Could not SELECT from " + user + ".EXPLAIN_INSTANCE table. " + ex.Message);
                        }

                        command.CommandText = "DELETE FROM " + user + ".EXPLAIN_INSTANCE";
                        command.ExecuteNonQuery();

                        command.CommandText = "EXPLAIN PLAN FOR " + sql;
                        command.ExecuteNonQuery();

                        command.CommandText = "	 SELECT O.OPERATOR_ID, S2.TARGET_ID AS PARENT_OPERATOR_ID, S2.SOURCE_ID, O.OPERATOR_TYPE,	" + Environment.NewLine;
                        command.CommandText += "	        S.OBJECT_NAME, S.OBJECT_SCHEMA, S2.COLUMN_NAMES, O.IO_COST, O.CPU_COST, O.FIRST_ROW_COST, O.BUFFERS, O.TOTAL_COST COST	" + Environment.NewLine;
                        command.CommandText += "	 FROM " + user + ".EXPLAIN_OPERATOR O	" + Environment.NewLine;
                        command.CommandText += "	      LEFT OUTER JOIN " + user + ".EXPLAIN_STREAM S2	" + Environment.NewLine;
                        command.CommandText += "	                      ON O.Operator_ID=S2.Source_ID	" + Environment.NewLine;
                        command.CommandText += "	      LEFT OUTER JOIN " + user + ".EXPLAIN_STREAM S	" + Environment.NewLine;
                        command.CommandText += "	                      ON O.Operator_ID = S.Target_ID	" + Environment.NewLine;
                        command.CommandText += "	                     AND O.Explain_Time = S.Explain_Time	" + Environment.NewLine;
                        command.CommandText += "	                     AND S.Object_Name IS NOT NULL	" + Environment.NewLine;
                        command.CommandText += "	 ORDER BY O.Explain_Time ASC, Operator_ID ASC;	" + Environment.NewLine;
                        
                        using (var dr = command.ExecuteReader())
                        {
                            table.Load(dr);
                        }
                    }
                    transaction.Rollback();
                }
            }

            // Parse table
            var explainPlanData = new Db2ExplainPlanData();
            var rootRow = table.Rows.Cast<DataRow>().Where(x => x.IsNull("PARENT_OPERATOR_ID")).ToList();
            if (rootRow.Count == 0)
            {
                throw new Exception("Could not find a starting point in the execution plan.");
            }
            var explainPlanRow = new ExplainPlanRow(rootRow.First());
            explainPlanData.Rows.Add(explainPlanRow);
            ParseExplainPlan(explainPlanRow, table, "OPERATOR_ID", "PARENT_OPERATOR_ID");
            return explainPlanData;

        }

        //private static void ParseExplainPlan(ExplainPlanRow parentRow, DataTable table)
        //{
        //    ParseExplainPlan(parentRow, table, "OPERATOR_ID", "PARENT_OPERATOR_ID");

        //    //var parentOperatorId = (int) parentRow.Row["OPERATOR_ID"];
        //    //var childRows = table.Rows.Cast<DataRow>().Where(x => !x.IsNull("PARENT_OPERATOR_ID") && ((int)x["PARENT_OPERATOR_ID"]) == parentOperatorId).ToList();
        //    //if (childRows.Count == 0) return;
        //    //foreach (var childRow in childRows)
        //    //{
        //    //    var childExplainRow = new ExplainPlanRow(childRow);
        //    //    parentRow.ChildRows.Add(childExplainRow);
        //    //    ParseExplainPlan(childExplainRow, table);
        //    //}
        //}
    }
}