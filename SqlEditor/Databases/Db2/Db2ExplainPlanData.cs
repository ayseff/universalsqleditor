using System.Collections.Generic;
using SqlEditor.ExplainPlan;

namespace SqlEditor.Databases.Db2
{
    public class Db2ExplainPlanData : ExplainPlanData
    {
        public override IList<ExplainPlanColumn> GetColumns()
        {
            var list = new List<ExplainPlanColumn>();
            list.Add(new ExplainPlanColumn("OPERATOR_TYPE", "Operation"));
            list.Add(new ExplainPlanColumn("COST", "Cost"));
            list.Add(new ExplainPlanColumn("IO_COST", "IO Cost"));
            list.Add(new ExplainPlanColumn("CPU_COST", "CPU Cost"));
            list.Add(new ExplainPlanColumn("OBJECT_SCHEMA", "Object Schema"));
            list.Add(new ExplainPlanColumn("OBJECT_NAME", "Object Name"));
            list.Add(new ExplainPlanColumn("COLUMN_NAMES", "Column Names"));
            return list;
        }
    }
}