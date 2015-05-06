using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes.SqlServer
{
    public class SqlServerConnectionTreeNode : ConnectionTreeNode
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

        public SqlServerConnectionTreeNode(DatabaseConnection connection)
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
                        _log.Debug("Loading database instances ...");
                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                        DatabaseInstances.Clear();
                        DatabaseInstances.AddRange(infoProvider.GetDatabaseInstances(connection));
                        _log.DebugFormat("Loaded {0} databases instance(s).", DatabaseInstances.Count);

                        _log.Debug("Creating nodes ...");
                        var databaseInstanceNodes = new List<TreeNodeBase>();
                        var csb = new SqlConnectionStringBuilder(DatabaseConnection.ConnectionString);
                        foreach (var databaseInstance in DatabaseInstances)
                        {
                            csb.InitialCatalog = databaseInstance.Name;
                            var dbConnection = (DatabaseConnection)DatabaseConnection.Clone();
                            dbConnection.ConnectionString = csb.ConnectionString;
                            dbConnection.IsConnected = false;
                            var node = new SqlServerDatabaseInstanceTreeNode(databaseInstance, dbConnection);
                            databaseInstanceNodes.Add(node);
                        }
                        _log.Debug("Loading nodes finished.");
                        return databaseInstanceNodes;

//                        _log.Debug("Creating nodes ...");
//                        var nodes =
//                            DatabaseInstances.Select(schema => new SqlServerDatabaseInstanceTreeNode(schema, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
//                        _log.Debug("Loading nodes finished.");
//                        return nodes;
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