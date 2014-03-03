#region

using System.Diagnostics;
using SqlEditor.SqlParser.Expressions;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    [DebuggerDisplay("{Expression.Value} ({Alias})")]
    public class Field : AliasedEntity
    {
        public Expression Expression { get; set; }
    }
}