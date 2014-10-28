using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.Oracle
{
    public class OracleExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("OPERATION", "Operation"));
            list.Add(new ExplainPlanColumn("OBJECT_OWNER", "Object Owner"));
            list.Add(new ExplainPlanColumn("OBJECT_NAME", "Object Name"));
            list.Add(new ExplainPlanColumn("ACCESS_PREDICATES", "Access Predicates"));
            list.Add(new ExplainPlanColumn("FILTER_PREDICATES", "Filter Predicates"));
            list.Add(new ExplainPlanColumn("CARDINALITY", "Number of Rows"));
            list.Add(new ExplainPlanColumn("BYTES", "Bytes"));
            list.Add(new ExplainPlanColumn("COST", "Cost"));
            list.Add(new ExplainPlanColumn("IO_COST", "IO Cost"));
            list.Add(new ExplainPlanColumn("CPU_COST", "CPU Cost"));


            
            return list;
        }
    }
}