using System.Collections.Generic;
using System.Data;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class TablePrimaryKeyColumnsTreeNode : ColumnsTreeNode
    {
        public TablePrimaryKeyColumnsTreeNode(Table table, DatabaseConnection databaseConnection)
            : base(table, databaseConnection, "Primary Key Columns")
        {
        }

        protected override void Clear()
        {
            DatabaseObject.PrimaryKeyColumns.Clear();
        }

        protected override void AddColumns(IList<Column> columns)
        {
            DatabaseObject.PrimaryKeyColumns.AddRange(columns);
        }

        protected override IList<Column> GetColumns(DbInfoProvider infoProvider, IDbConnection connection)
        {
            return infoProvider.GetTablePrimaryKeyColumns(connection, DatabaseObject.Parent.Name, DatabaseObject.Name);
        }
    }
}