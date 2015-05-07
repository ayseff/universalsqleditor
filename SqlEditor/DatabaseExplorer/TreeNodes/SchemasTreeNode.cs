using System;
using System.Collections.Generic;
using SqlEditor.DatabaseExplorer.TreeNodes.Base;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SchemasTreeNode : FolderContainerTreeNode
    {
        public List<DatabaseObject> Schemas { get; private set; }

        public SchemasTreeNode(DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance, "Schemas", "folder-horizontal-user.png", "folder-horizontal-open-user.png")
        {
            Schemas = new List<DatabaseObject>();
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading schemas ...");
            Schemas.Clear();
            try
            {
                var actionName = string.Format("Getting schemas for {0} ...", DatabaseConnection.Name);
                _log.Debug(actionName);
                using (new WaitActionStatus(actionName))
                {
                    _log.DebugFormat("Creating connection for {0} ...", DatabaseConnection.Name);
                    using (var connection = DatabaseConnection.CreateNewConnection())
                    {
                        connection.OpenIfRequired();
                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                        var schemas = infoProvider.GetSchemas(connection);
                        _log.DebugFormat("Loaded {0} schema(s).", schemas.Count);
                        Schemas.AddRange(schemas);
                        return TreeNodeFactory.GetSchemaNodes(schemas, DatabaseConnection, DatabaseInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error opening connection and loading schemas.");
                _log.Error(ex.Message);
                throw new Exception("Error opening connection and loading schemas.", ex);
            }
        }
    }
}