using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes.MySql
{
    public sealed class MySqlConnectionTreeNode : ConnectionTreeNode
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

        public MySqlConnectionTreeNode(DatabaseConnection connection)
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
                        var csb = new MySqlConnectionStringBuilder(DatabaseConnection.ConnectionString);
                        foreach (var databaseInstance in DatabaseInstances)
                        {
                            csb.Database = databaseInstance.Name;
                            var dbConnection = (DatabaseConnection)DatabaseConnection.Clone();
                            dbConnection.ConnectionString = csb.ConnectionString;
                            dbConnection.IsConnected = false;
                            var node = new MySqlDatabaseInstanceTreeNode(databaseInstance, dbConnection);
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
//            try
//            {
//                var actionName = string.Format("Attempting to connect to {0} ...", DatabaseConnection.Name);
//                _log.Debug(actionName);
//                using (new WaitActionStatus(actionName))
//                {
//                    _log.DebugFormat("Creating connection for {0} ...", DatabaseConnection.Name);
//                    using (var connection = DatabaseConnection.CreateNewConnection())
//                    {
//                        connection.OpenIfRequired();
//                        DatabaseConnection.Connect();
//                        _log.Debug("Connection is successful.");
//                        _log.Debug("Loading databases ...");
//                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
//                        Schemas.Clear();
//                        Schemas.AddRange(infoProvider.GetSchemas(connection));
//                        _log.DebugFormat("Loaded {0} database(s).", Schemas.Count);
//
//                        _log.Debug("Creating nodes ...");
//                        var schemNodes =
//                            Schemas.Select(databaseInstance => new MySqlSchemaTreeNode(null, DatabaseConnection, DatabaseInstance)).Cast<TreeNodeBase>().ToList();
//                        _log.Debug("Loading nodes finished.");
//                        return schemNodes;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                DatabaseConnection.Disconnect();
//                _log.ErrorFormat("Error opening connection and loading data.");
//                _log.Error(ex.Message, ex);
//                throw;
//            }
        }
    }
}