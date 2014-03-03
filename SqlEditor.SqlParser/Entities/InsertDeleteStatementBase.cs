#region

using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class InsertDeleteStatementBase : CustomStatement, ITableHints
    {
        internal InsertDeleteStatementBase()
        {
            TableHints = new List<TableHint>();
        }

        #region ITableHints Members

        public List<TableHint> TableHints { get; set; }

        #endregion
    }
}