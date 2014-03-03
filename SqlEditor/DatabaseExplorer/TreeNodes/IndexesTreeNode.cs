#region
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlEditor.Database;
using SqlEditor.Databases;
using Utilities;
using Utilities.Collections;

#endregion

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class IndexesTreeNode : FolderContainerTreeNode
    {
        public Schema Schema { get; private set; }

        public IndexesTreeNode(Schema schema, DatabaseConnection databaseConnection)
            : base("Indexes", databaseConnection)
        {
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading indexes ...");
            Schema.Indexes.Clear();
            IList<Index> indexes;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                indexes = infoProvider.GetIndexes(connection, Schema.Name);
            }
            Schema.Indexes.AddRange(indexes);
            _log.DebugFormat("Loaded {0} index(es).", indexes.Count);

            var indexNodes =
                indexes.Select(view => new IndexTreeNode(view, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            return indexNodes;
        }
    }
}