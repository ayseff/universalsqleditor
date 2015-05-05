using System.Collections.Generic;
using System.Data;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class TablePrimaryKeyColumnsTreeNode : ColumnsTreeNode
    {
        public TablePrimaryKeyColumnsTreeNode(Table table, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(table, databaseConnection, databaseInstance, "Primary Key Columns")
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
            var databaseInstanceName = DatabaseObject.Parent.Parent == null ? null : DatabaseObject.Parent.Parent.Name;
            var columns = infoProvider.GetTablePrimaryKeyColumns(connection, DatabaseObject.Parent.Name, DatabaseObject.Name, databaseInstanceName);
            foreach (var column in columns)
            {
                column.Parent = DatabaseObject;
            }
            return columns;
        }
    }
}