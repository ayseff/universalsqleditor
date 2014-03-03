namespace SqlEditor.SqlParser.Expressions
{
    public class StringExpression : Expression
    {
        public StringExpression(string value, Expression parent) : base(parent)
        {
            Value = value;
        }

        public override bool CanInline
        {
            get { return true; }
        }
    }
}