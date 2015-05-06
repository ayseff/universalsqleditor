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
        public DatabaseObject Schema { get; private set; }

        public ViewsTreeNode([NotNull] DatabaseObject schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base("Views", databaseConnection, databaseInstance)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {

            _log.Debug("Loading views ...");
            //Schema.Views.Clear();
            IList<View> views;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                var databaseInstanceName = DatabaseInstance == null ? null : DatabaseInstance.Name;
                views = infoProvider.GetViews(connection, Schema.Name, databaseInstanceName);
                foreach (var view in views)
                {
                    view.Parent = Schema;
                }
            }
            //Schema.Views.AddRange(views);
            _log.DebugFormat("Loaded {0} view(s).", views.Count);

            _log.Debug("Loading nodes ...");
            Nodes.Clear();
            var viewNodes =
                views.Select(view => new ViewTreeNode(view, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
            return viewNodes;
        }
    }
}