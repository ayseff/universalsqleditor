#region

using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public abstract class SetStatement : Statement
    {
        public Expression Assignment { get; set; }
    }
}