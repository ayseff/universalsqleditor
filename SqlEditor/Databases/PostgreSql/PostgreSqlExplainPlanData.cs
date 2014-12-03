using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.PostgreSql
{
    public class PostgreSqlExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("Operation", "Operation"));
            list.Add(new ExplainPlanColumn("Startup_Cost", "Startup Cost"));
            list.Add(new ExplainPlanColumn("Total_Cost", "Total Cost"));
            list.Add(new ExplainPlanColumn("Row_Count", "Row Count"));
            list.Add(new ExplainPlanColumn("Width", "Width"));
            return list;
        }
    }
}