using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.MySql
{
    public class MySqlExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("id", "Id"));
            list.Add(new ExplainPlanColumn("select_type", "Select Type"));
            list.Add(new ExplainPlanColumn("table", "Table"));
            list.Add(new ExplainPlanColumn("possible_keys", "Possible Keys"));
            list.Add(new ExplainPlanColumn("key", "Key"));
            list.Add(new ExplainPlanColumn("ref", "Ref"));
            list.Add(new ExplainPlanColumn("ref", "Rows"));
            list.Add(new ExplainPlanColumn("Extra", "Extra"));
            return list;
        }
    }
}