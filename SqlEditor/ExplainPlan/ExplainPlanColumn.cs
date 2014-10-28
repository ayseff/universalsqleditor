namespace SqlEditor.ExplainPlan
{
    public class ExplainPlanColumn
    {
        public string ColumnName { get; set; }
        public string ColumnDisplayName { get; set; }

        public ExplainPlanColumn()
        {
        }

        public ExplainPlanColumn(string columnName, string columnDisplayName)
        {
            ColumnName = columnName;
            ColumnDisplayName = columnDisplayName;
        }
    }
}