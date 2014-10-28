using System.Collections.Generic;
using System.Data;

namespace SqlEditor.ExplainPlan
{
    public class ExplainPlanRow
    {
        public DataRow Row { get; set; }
        public IList<ExplainPlanRow> ChildRows { get; set; }

        public ExplainPlanRow()
        {
            ChildRows = new List<ExplainPlanRow>();
        }

        public ExplainPlanRow(DataRow row) 
            : this()
        {
            Row = row;
        }

        public override string ToString()
        {
            return Row == null ? "NULL" : Row.ToString();
        }
    }
}