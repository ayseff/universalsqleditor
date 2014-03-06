using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TriggersTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; protected set; }

        public TriggersTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection)
            : base("Triggers", databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading triggers ...");
            //Schema.Tables.Clear();
            IList<Trigger> triggers;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                triggers = infoProvider.GetTriggers(connection, Schema.Name);
            }
            //Schema.Triggers.AddRange(triggers);
            _log.DebugFormat("Loaded {0} trigger(s).", triggers.Count);

            var nodes = triggers.Select(x => new TriggerTreeNode(x, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}