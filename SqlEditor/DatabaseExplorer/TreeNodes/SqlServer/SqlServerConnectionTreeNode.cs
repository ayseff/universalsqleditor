using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.SqlServer
{
    public sealed class SqlServerConnectionTreeNode : ConnectionTreeNode
    {
        protected readonly List<DatabaseInstance> DatabaseInstances = new List<DatabaseInstance>();

        public SqlServerConnectionTreeNode(DatabaseConnection connection)
            : base(connection)
        {
            DatabaseInstances = new List<DatabaseInstance>();
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
                        _log.Debug("Loading schemas ...");
                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                        DatabaseInstances.Clear();
                        DatabaseInstances.AddRange(infoProvider.GetDatabaseInstances(connection));
                        _log.DebugFormat("Loaded {0} databases(s).", DatabaseInstances.Count);

                        _log.Debug("Creating nodes ...");
                        var schemNodes =
                            DatabaseInstances.Select(schema => new SqlServerDatabaseInstanceTreeNode(schema, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
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