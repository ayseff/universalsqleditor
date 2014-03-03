#region

using System.Collections.Generic;

#endregion

namespace SqlEditor.SqlParser.Entities
{
    public class UpdateStatement : ProjectionStatement, ITableHints
    {
        public UpdateStatement()
        {
            TableHints = new List<TableHint>();
        }

        public Top Top { get; set; }
        public string TableName { get; set; }

        #region ITableHints Members

        public List<TableHint> TableHints { get; set; }

        #endregion
    }
}