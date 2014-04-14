using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class StoredProceduresTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; private set; }

        public StoredProceduresTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, string nodeDisplayText = "Procedures")
            : base(nodeDisplayText, databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading stored procedures ...");
            //Schema.StoredProcedures.Clear();
            IList<StoredProcedure> storedProcedures;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                var databaseInstanceName = Schema.Parent == null ? null : Schema.Parent.Name;
                storedProcedures = infoProvider.GetStoredProcedures(connection, Schema.Name, databaseInstanceName);
                foreach (var storedProcedure in storedProcedures)
                {
                    storedProcedure.Parent = Schema;
                }
            }
            //Schema.StoredProcedures.AddRange(storedProcedures);
            _log.DebugFormat("Loaded {0} stored procedure(s).", storedProcedures.Count);

            var nodes = storedProcedures.Select(table => new StoredProcedureTreeNode(table, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}