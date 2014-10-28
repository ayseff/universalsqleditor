using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("StmtText", "Statement Text"));
            list.Add(new ExplainPlanColumn("LogicalOp", "Logical Operation"));
            list.Add(new ExplainPlanColumn("PhysicalOp", "Physical Operation"));
            list.Add(new ExplainPlanColumn("Argument", "Argument"));
            list.Add(new ExplainPlanColumn("DefinedValues", "Defined Values"));
            list.Add(new ExplainPlanColumn("EstimateRows", "Estimate Rows"));
            list.Add(new ExplainPlanColumn("EstimateIO", "Estimate IO"));
            list.Add(new ExplainPlanColumn("EstimateCPU", "Estimate CPU"));
            list.Add(new ExplainPlanColumn("AvgRowSize", "Average Row Size"));
            list.Add(new ExplainPlanColumn("TotalSubtreeCost", "Cost"));
            list.Add(new ExplainPlanColumn("Parallel", "Parallel"));
            list.Add(new ExplainPlanColumn("EstimateExecutions", "Estimate Executions"));
            return list;
        }
    }
}