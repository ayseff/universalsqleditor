#region

using SqlEditor.SqlParser.Entities;

#endregion

namespace SqlEditor.SqlParser.Expressions
{
    public class SelectExpression : Expression
    {
        public SelectExpression() : base(null)
        {
            Statement = new SelectStatement();
        }

        public SelectStatement Statement { get; set; }

        public override string Value
        {
            get { return Statement.ToString(); }
        }

        public override bool CanInline
        {
            get { return Statement.CanInLine(); }
        }
    }
}