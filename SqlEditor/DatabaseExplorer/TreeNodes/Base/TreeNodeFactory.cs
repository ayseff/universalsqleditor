using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using log4net;
using SqlEditor.DatabaseExplorer.TreeNodes.Db2;
using SqlEditor.DatabaseExplorer.TreeNodes.MsAccess;
using SqlEditor.DatabaseExplorer.TreeNodes.MySql;
using SqlEditor.DatabaseExplorer.TreeNodes.Oracle;
using SqlEditor.DatabaseExplorer.TreeNodes.SqlCe;
using SqlEditor.DatabaseExplorer.TreeNodes.Sqlite;
using SqlEditor.DatabaseExplorer.TreeNodes.SqlServer;
using SqlEditor.Databases;
using SqlEditor.Databases.Db2;
using SqlEditor.Databases.MsAccess;
using SqlEditor.Databases.MySql;
using SqlEditor.Databases.Oracle;
using SqlEditor.Databases.PostgreSql;
using SqlEditor.Databases.SqlCe;
using SqlEditor.Databases.Sqlite;
using SqlEditor.Databases.SqlServer;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Base
{
    public static class TreeNodeFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static ConnectionTreeNode GetConnectionTreeNode(DatabaseConnection databaseConnection)
        {
            if (databaseConnection.DatabaseServer is Db2DatabaseServer)
            {
                return new Db2ConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is OracleDatabaseServer)
            {
                return new OracleConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is MySqlDatabaseServer)
            {
                return new MySqlConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is PostgreSqlDatabaseServer)
            {
                return new PostgreSqlConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is SqlServerDatabaseServer)
            {
                return new SqlServerConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is SqlCeDatabaseServer)
            {
                return new SqlCeConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is MsAccess2003DatabaseServer)
            {
                return new MsAccessConnectionTreeNode(databaseConnection);
            }
            else if (databaseConnection.DatabaseServer is SqliteDatabaseServer)
            {
                return new SqliteConnectionTreeNode(databaseConnection);
            }
            else
            {
                throw new Exception("Unrecognized database server " + databaseConnection.GetType());
            }
        }

        public static IList<TreeNodeBase> GetSchemaNodes(DatabaseConnection databaseConnection)
        {
            try
            {
                var actionName = string.Format("Getting schemas for {0} ...", databaseConnection.Name);
                _log.Debug(actionName);
                using (new WaitActionStatus(actionName))
                {
                    _log.DebugFormat("Creating connection for {0} ...", databaseConnection.Name);
                    using (var connection = databaseConnection.CreateNewConnection())
                    {
                        connection.OpenIfRequired();
                        var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
                        var schemas = infoProvider.GetSchemas(connection);
                        _log.DebugFormat("Loaded {0} schema(s).", schemas.Count);
                        var schemaNodes = GetSchemaNodesForServer(schemas, databaseConnection);
                        return schemaNodes;
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

        private static IList<TreeNodeBase> GetSchemaNodesForServer([NotNull] IEnumerable<Schema> schemas,
            [NotNull] DatabaseConnection databaseConnection)
        {
            if (schemas == null) throw new ArgumentNullException("schemas");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            if (databaseConnection.DatabaseServer is Db2DatabaseServer)
            {
                return schemas.Select(schema => new Db2SchemaTreeNode(schema, databaseConnection)).Cast<TreeNodeBase>().ToList();
            }
                // TODO: Add other servers here
            else
            {
                throw new Exception("Unrecognized database server " + databaseConnection.GetType());
            }
        }
    }
}
