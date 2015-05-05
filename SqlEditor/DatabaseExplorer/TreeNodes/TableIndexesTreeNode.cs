using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TableIndexesTreeNode : FolderContainerTreeNode
    {
        public Table Table { get; set; }

        public TableIndexesTreeNode([NotNull] Table table, DatabaseConnection connection, DatabaseInstance databaseInstance)
            : base("Indexes", connection, databaseInstance)
        {
            if (table == null) throw new ArgumentNullException("table");
            
            Table = table;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                _log.DebugFormat("Loading indexes for table {0} ...", Table.FullyQualifiedName);
                Table.Indexes.Clear();
                IList<Index> indexes;
                using (var connection = DatabaseConnection.CreateNewConnection())
                {
                    connection.OpenIfRequired();
                    var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                    var databaseInstanceName = Table.Parent.Parent == null ? null : Table.Parent.Parent.Name;
                    indexes = infoProvider.GetIndexesForTable(connection, Table.Parent.Name, Table.Name, databaseInstanceName);
                    //foreach (var idx in indexes)
                    //{
                    //    idx.Parent = Table.Parent;
                    //}
                }
                Table.Indexes.AddRange(indexes);
                _log.DebugFormat("Loaded {0} index(es).", indexes.Count);

                var indexNodes =
                    indexes.Select(x => new IndexTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
                return indexNodes;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading indexes for table {0}.", Table.Name);
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}