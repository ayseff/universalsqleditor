﻿using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.SqlServer
{
    public class SqlServerDatabaseInstanceTreeNode : DatabaseInstanceTreeNode
    {
        public List<Schema> Schemas = new List<Schema>();

        public SqlServerDatabaseInstanceTreeNode(DatabaseInstance databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseInstance, databaseConnection)
        { }

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
                        Schemas.Clear();
                        Schemas.AddRange(infoProvider.GetSchemas(connection));
                        _log.DebugFormat("Loaded {0} schema(s).", Schemas.Count);

                        _log.Debug("Creating nodes ...");
                        var schemNodes =
                            Schemas.Select(schema => new SqlServerSchemaTreeNode(schema, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
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