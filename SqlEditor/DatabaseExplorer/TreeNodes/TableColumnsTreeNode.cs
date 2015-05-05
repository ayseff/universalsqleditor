using System.Collections.Generic;
using System.Data;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class TableColumnsTreeNode : ColumnsTreeNode
    {
        public TableColumnsTreeNode(Table table, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance) 
            : base(table, databaseConnection, databaseInstance)
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
            var databaseInstanceName = DatabaseObject.Parent.Parent == null ? null : DatabaseObject.Parent.Parent.Name;
            var columns = infoProvider.GetTableColumns(connection, DatabaseObject.Parent.Name, DatabaseObject.Name, databaseInstanceName);
            foreach (var column in columns)
            {
                column.Parent = DatabaseObject;
            }
            return columns;
        }
    }
}