using System;
using System.Collections.Generic;
using System.Data;
using SqlEditor.Annotations;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class IndexColumnsTreeNode : ColumnsTreeNode
    {
        public IndexColumnsTreeNode([NotNull] Index index, DatabaseConnection connection, DatabaseInstance databaseInstance)
            : base(index, connection, databaseInstance)
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
            var index = DatabaseObject as Index;
            if (index == null) throw new Exception("Index is null");
            var columns = infoProvider.GetIndexColumns(connection, index.Table.Parent.Name, index.Table.Name, DatabaseObject.Parent.Name, DatabaseObject.Name, indexId: this.DatabaseObject.Id, databaseInstanceName: databaseInstanceName);
            foreach (var column in columns)
            {
                column.Parent = DatabaseObject;
            }
            return columns;
        }
    }
}