using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.MySql
{
    public sealed class MySqlConnectionTreeNode : ConnectionTreeNode
    {
        protected readonly List<Schema> Schemas = new List<Schema>();

        public MySqlConnectionTreeNode(DatabaseConnection connection)
            : base(connection)
        {
            Schemas = new List<Schema>();
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                var actionName = string.Format("Attempting to connect to {0} ...", DatabaseConnection.Name);
                _log.Debug(actionName);
                using (new WaitActionStatus(actionName))
                {
                    _log.DebugFormat("Creating connection for {0} ...", DatabaseConnection.Name);
                    using (var connection = DatabaseConnection.CreateNewConnection())
                    {
                        connection.OpenIfRequired();
                        DatabaseConnection.Connect();
                        _log.Debug("Connection is successful.");
                        _log.Debug("Loading databases ...");
                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                        Schemas.Clear();
                        Schemas.AddRange(infoProvider.GetSchemas(connection));
                        _log.DebugFormat("Loaded {0} database(s).", Schemas.Count);

                        _log.Debug("Creating nodes ...");
                        var schemNodes =
                            Schemas.Select(databaseInstance => new MySqlSchemaTreeNode(databaseInstance, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
                        _log.Debug("Loading nodes finished.");
                        return schemNodes;
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseConnection.Disconnect();
                _log.ErrorFormat("Error opening connection and loading data.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}