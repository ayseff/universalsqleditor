using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TriggersTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; private set; }

        public TriggersTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base("Triggers", databaseConnection, databaseInstance)
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
                var databaseInstanceName = DatabaseInstance == null ? null : DatabaseInstance.Name;
                triggers = infoProvider.GetTriggers(connection, Schema.Name, databaseInstanceName);
                foreach (var trigger in triggers)
                {
                    trigger.Parent = Schema;
                }
            }
            //Schema.Triggers.AddRange(triggers);
            _log.DebugFormat("Loaded {0} trigger(s).", triggers.Count);

            var nodes = triggers.Select(x => new TriggerTreeNode(x, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}