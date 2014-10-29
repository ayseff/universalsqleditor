using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("PhysicalOp", "Physical Operator"));
            list.Add(new ExplainPlanColumn("LogicalOp", "Logical Operator"));
            list.Add(new ExplainPlanColumn("ObjectName", "Object Name"));
            list.Add(new ExplainPlanColumn("EstimateRows", "Estimate Rows"));
            list.Add(new ExplainPlanColumn("EstimateIO", "Estimate IO"));
            list.Add(new ExplainPlanColumn("EstimateCPU", "Estimate CPU"));
            list.Add(new ExplainPlanColumn("AvgRowSize", "Average Row Size"));
            list.Add(new ExplainPlanColumn("EstimatedTotalSubtreeCost", "Cost"));
            return list;
        }
    }
}