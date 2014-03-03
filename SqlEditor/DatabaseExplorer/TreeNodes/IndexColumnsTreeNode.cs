using System.Collections.Generic;
using System.Data;
using SqlEditor.Annotations;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class IndexColumnsTreeNode : ColumnsTreeNode
    {
        public IndexColumnsTreeNode([NotNull] Index index, DatabaseConnection connection)
            : base(index, connection)
        {
        }

        protected override void Clear()
        {
            DatabaseObject.Columns.Clear();
        }

        protected override void AddColumns(IList<Column> columns)
        {
            DatabaseObject.Columns.AddRange(columns);
        }

        protected override IList<Column> GetColumns(DbInfoProvider infoProvider, IDbConnection connection)
        {
            return infoProvider.GetIndexColumns(connection, DatabaseObject.Parent.Name, DatabaseObject.Name);
        }
    }
}