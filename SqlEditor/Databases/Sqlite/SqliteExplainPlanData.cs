using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("selectid", "Select Id"));
            list.Add(new ExplainPlanColumn("order", "Order"));
            list.Add(new ExplainPlanColumn("from", "From"));
            list.Add(new ExplainPlanColumn("detail", "Detail"));
            return list;
        }
    }
}