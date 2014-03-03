#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace SqlEditor.SqlParser.Expressions
{
    public class IdentifierExpression : Expression
    {
        public IdentifierExpression(string value, Expression parent) : base(parent)
        {
            Parts = value.Split(new[] {Constants.Dot}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public List<string> Parts { get; set; }

        public override string Value
        {
            get { return String.Join(Constants.Dot, Parts.ToArray()); }
        }

        public override bool CanInline
        {
            get { return true; }
        }
    }
}