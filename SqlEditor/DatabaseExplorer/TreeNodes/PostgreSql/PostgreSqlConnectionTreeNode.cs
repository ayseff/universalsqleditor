using System;
using System.Collections.Generic;
using Npgsql;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes.PostgreSql
{
    public sealed class PostgreSqlConnectionTreeNode : ConnectionTreeNode
    {
        private readonly List<DatabaseInstance> _databaseInstances = new List<DatabaseInstance>();

        public IList<DatabaseInstance> DatabaseInstances
        {
            get { return _databaseInstances; }
        }

        public override bool OpensWorksheet
        {
            get { return false; }
        }

        public PostgreSqlConnectionTreeNode(DatabaseConnection connection)
            : base(connection)
        {
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
                        var databaseInstanceNodes = new List<TreeNodeBase>();
                        var csb = new NpgsqlConnectionStringBuilder(DatabaseConnection.ConnectionString);
                        foreach (var databaseInstance in DatabaseInstances)
                        {
                            csb.Database = databaseInstance.Name;
                            var dbConnection = (DatabaseConnection)DatabaseConnection.Clone();
                            dbConnection.ConnectionString = csb.ConnectionString;
                            dbConnection.IsConnected = false;
                            var node = new PostgreSqlDatabaseInstanceTreeNode(databaseInstance, dbConnection);
                            databaseInstanceNodes.Add(node);
                        }
                        _log.Debug("Loading nodes finished.");
                        return databaseInstanceNodes;
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