using System.Collections.Generic;

namespace SqlEditor.ExplainPlan
{
    public abstract class ExplainPlanData
    {
        public abstract IList<ExplainPlanColumn> GetColumns();

        public List<ExplainPlanRow> Rows { get; set; }

        protected ExplainPlanData()
        {
            Rows = new List<ExplainPlanRow>();
        }
    }
}
