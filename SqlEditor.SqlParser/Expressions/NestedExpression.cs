namespace SqlEditor.SqlParser.Expressions
{
    public class NestedExpression : Expression
    {
        /// <summary>
        ///   Initializes a new instance of the NestedExpression class.
        /// </summary>
        public NestedExpression(Expression parent) : base(parent)
        {
        }

        public Expression Expression { get; set; }

        public override string Value
        {
            get { return Constants.OpenBracket + Expression.Value + Constants.CloseBracket; }
        }

        public override bool CanInline
        {
            get { return Expression.CanInline; }
        }
    }
}