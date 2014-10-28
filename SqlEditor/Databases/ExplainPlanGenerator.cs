using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SqlEditor.DatabaseExplorer;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases
{
    public abstract class ExplainPlanGenerator
    {
        public abstract ExplainPlanData GetExplainPlan(DatabaseConnection databaseConnection, string sql);

        public Task<ExplainPlanData> GetExplainPlanAsync(DatabaseConnection databaseConnection, string sql, CancellationToken token)
        {
            var task = Task.Run(() => GetExplainPlan(databaseConnection, sql), token);
            return task;
        }

        protected static void ParseExplainPlan(ExplainPlanRow parentRow, DataTable table, string idColumnName, string parentIdColumnName)
        {
            var parentOperatorId = int.Parse(parentRow.Row[idColumnName].ToString());
            var childRows = table.Rows.Cast<DataRow>().Where(x => !x.IsNull(parentIdColumnName) && ((int.Parse(x[parentIdColumnName].ToString()))) == parentOperatorId).ToList();
            if (childRows.Count == 0) return;
            foreach (var childRow in childRows)
            {
                var childExplainRow = new ExplainPlanRow(childRow);
                parentRow.ChildRows.Add(childExplainRow);
                ParseExplainPlan(childExplainRow, table, idColumnName, parentIdColumnName);
            }
        }
    }
}