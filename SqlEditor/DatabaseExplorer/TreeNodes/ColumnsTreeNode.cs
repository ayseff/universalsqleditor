using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class ColumnsTreeNode : FolderContainerTreeNode
    {
        public DatabaseObjectWithColumns DatabaseObject { get; set; }

        protected ColumnsTreeNode([NotNull] DatabaseObjectWithColumns databaseObject, DatabaseConnection connection, DatabaseInstance databaseInstance, string nodeDisplayText = "Columns")
            : base(nodeDisplayText, connection, databaseInstance)
        {
            if (databaseObject == null) throw new ArgumentNullException("databaseObject");
            if (connection == null) throw new ArgumentNullException("connection");
            
            DatabaseObject = databaseObject;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                _log.DebugFormat("Loading columns for object {0} ...", DatabaseObject.FullyQualifiedName);
                Clear();
                IList<Column> columns;
                using (var connection = DatabaseConnection.CreateNewConnection())
                {
                    connection.OpenIfRequired();
                    var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                    columns = GetColumns(infoProvider, connection);
                }
                AddColumns(columns);
                _log.DebugFormat("Loaded {0} column(s).", columns.Count);

                var columnNodes =
                    columns.Select(x => new ColumnTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
                return columnNodes;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading columns for object {0}.", DatabaseObject.FullyQualifiedName);
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        protected abstract void Clear();

        protected abstract void AddColumns(IList<Column> columns);

        protected abstract IList<Column> GetColumns(DbInfoProvider infoProvider, IDbConnection connection);
    }
}