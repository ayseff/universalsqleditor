#region

using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    /// <summary>
    ///   Entity that can have table hints
    /// </summary>
    public interface ITableHints
    {
        List<TableHint> TableHints { get; set; }
    }
}