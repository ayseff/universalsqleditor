#region

using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Databases;
using Utilities.Collections;

#endregion

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class ViewsTreeNode : FolderContainerTreeNode
    {
        public Schema Schema { get; private set; }

        public ViewsTreeNode([NotNull] Schema schema, DatabaseConnection databaseConnection)
            : base("Views", databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {

            _log.Debug("Loading views ...");
            Schema.Views.Clear();
            IList<View> views;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                views = infoProvider.GetViews(connection, Schema.Name);
            }
            Schema.Views.AddRange(views);
            _log.DebugFormat("Loaded {0} view(s).", views.Count);

            _log.Debug("Loading nodes ...");
            Nodes.Clear();
            var viewNodes =
                views.Select(view => new ViewTreeNode(view, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            return viewNodes;
        }
    }
}