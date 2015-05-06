using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.SqlServer
{
    public class SqlServerDatabaseInstanceTreeNode : DatabaseInstanceTreeNode
    {
        public SqlServerDatabaseInstanceTreeNode(DatabaseInstance databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseInstance, databaseConnection)
        {
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var schemasTreeNode = new SchemasTreeNode(DatabaseConnection, DatabaseInstance);
            nodes.Add(schemasTreeNode);
            return nodes;
//            try
//            {
//                var actionName = string.Format("Fetching databases for {0} ...", DatabaseConnection.Name);
//                using (new WaitActionStatus(actionName))
//                {
//                    using (var connection = DatabaseConnection.CreateNewConnection())
//                    {
//                        connection.OpenIfRequired();
//                        DatabaseConnection.Connect();
//                        _log.Debug("Connection is successful.");
//                        _log.Debug("Loading database instances ...");
//                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
//                        var schemas = infoProvider.GetSchemas(connection, DatabaseInstance.Name);
//                        foreach (var schema in schemas)
//                        {
//                            schema.Parent = DatabaseInstance;
//                        }
//                        _log.DebugFormat("Loaded {0} database instance(s).", schemas.Count);
//
//                        _log.Debug("Creating nodes ...");
//                        var schemNodes =
//                            schemas.Select(schema => new SqlServerSchemaTreeNode(schema, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
//                        _log.Debug("Loading nodes finished.");
//                        return schemNodes;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                DatabaseConnection.Disconnect();
//                _log.ErrorFormat("Error fetching databases for {0}.", DatabaseConnection.Name);
//                _log.Error(ex.Message, ex);
//                throw;
//            }
        }
    }
}