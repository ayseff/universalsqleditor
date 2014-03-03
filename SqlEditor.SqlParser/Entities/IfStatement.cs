#region

using SqlEditor.SqlParser.Expressions;
using SqlEditor.SqlParser.Interfaces;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class IfStatement : CustomStatement
    {
        public IStatement If { get; set; }
        public Expression Condition { get; set; }
        public IStatement Else { get; set; }
    }
}