using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class StoredProceduresTreeNode : FolderContainerTreeNode
    {
        public Schema Schema { get; protected set; }

        public StoredProceduresTreeNode(Schema schema, DatabaseConnection databaseConnection)
            : base("Stored Procedures", databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading stored procedures ...");
            Schema.StoredProcedures.Clear();
            IList<StoredProcedure> storedProcedures;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                storedProcedures = infoProvider.GetStoredProcedures(connection, Schema.Name);
            }
            Schema.StoredProcedures.AddRange(storedProcedures);
            _log.DebugFormat("Loaded {0} stored procedure(s).", storedProcedures.Count);

            var nodes = storedProcedures.Select(table => new StoredProcedureTreeNode(table, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}