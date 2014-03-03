using System.Collections.Generic;
using System.Data;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class TableColumnsTreeNode : ColumnsTreeNode
    {
        public TableColumnsTreeNode(Table table, DatabaseConnection databaseConnection) 
            : base(table, databaseConnection)
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
            return infoProvider.GetTableColumns(connection, DatabaseObject.Parent.Name, DatabaseObject.Name);
        }
    }
}