using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class FunctionsTreeNode : FolderContainerTreeNode
    {
        public DatabaseObject Schema { get; protected set; }

        public FunctionsTreeNode(DatabaseObject schema, DatabaseConnection databaseConnection, string nodeDisplayText = "Functions")
            : base(nodeDisplayText, databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading functions ...");
            //Schema.StoredProcedures.Clear();
            IList<Function> functions;
            using (var connection = DatabaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                functions = infoProvider.GetFunctions(connection, Schema.Name);
            }
            //Schema.StoredProcedures.AddRange(storedProcedures);
            _log.DebugFormat("Loaded {0} function(s).", functions.Count);

            var nodes = functions.Select(function => new FunctionTreeNode(function, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            _log.Debug("Loading tree finished.");
            return nodes;
        }
    }
}