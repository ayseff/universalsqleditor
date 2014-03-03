#region

using System;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class Top
    {
        public Top(Expression value, bool brackets)
        {
            Brackets = brackets;
            Expression = value;
        }

        public bool Brackets { get; private set; }
        public Expression Expression { get; set; }
        public bool Percent { get; set; }

        public override string ToString()
        {
            string format = Brackets ? " TOP ({0}){1}" : " TOP {0}{1}";
            return String.Format(format, Expression.Value, Percent ? " PERCENT " : "");
        }
    }
}