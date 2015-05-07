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
using SqlEditor.DatabaseExplorer.TreeNodes.PostgreSql;
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

//        public static IList<TreeNodeBase> GetSchemaNodes(IList<Schema> schemas, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
//        {
//            return GetSchemaNodes((IEnumerable<Schema>) schemas, databaseConnection, databaseInstance);
////            try
////            {
////                var actionName = string.Format("Getting schemas for {0} ...", databaseConnection.Name);
////                _log.Debug(actionName);
////                using (new WaitActionStatus(actionName))
////                {
////                    _log.DebugFormat("Creating connection for {0} ...", databaseConnection.Name);
////                    using (var connection = databaseConnection.CreateNewConnection())
////                    {
////                        connection.OpenIfRequired();
//////                        var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
//////                        var schemas = infoProvider.GetSchemas(connection);
//////                        _log.DebugFormat("Loaded {0} schema(s).", schemas.Count);
////                        var schemaNodes = GetSchemaNodesForServer(schemas, databaseConnection, databaseInstance);
////                        return schemaNodes;
////                    }
////                }
////            }
////            catch (Exception ex)
////            {
////                _log.ErrorFormat("Error opening connection and loading schemas.");
////                _log.Error(ex.Message);
////                throw new Exception("Error opening connection and loading schemas.", ex);
////            }
//        }

        public static IList<TreeNodeBase> GetSchemaNodes([NotNull] IEnumerable<Schema> schemas, [NotNull] DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
        {
            if (schemas == null) throw new ArgumentNullException("schemas");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            if (databaseConnection.DatabaseServer is Db2DatabaseServer)
            {
                return schemas.Select(schema => new Db2SchemaTreeNode(schema, databaseConnection, databaseInstance)).Cast<TreeNodeBase>().ToList();
            }
            else if (databaseConnection.DatabaseServer is PostgreSqlDatabaseServer)
            {
                return schemas.Select(schema => new PostgreSqlSchemaTreeNode(schema, databaseConnection, databaseInstance)).Cast<TreeNodeBase>().ToList();
            }
            else if (databaseConnection.DatabaseServer is MySqlDatabaseServer)
            {
                return schemas.Select(schema => new MySqlSchemaTreeNode(schema, databaseConnection, databaseInstance)).Cast<TreeNodeBase>().ToList();
            }
            else if (databaseConnection.DatabaseServer is SqlServerDatabaseServer)
            {
                return schemas.Select(schema => new SqlServerSchemaTreeNode(schema, databaseConnection, databaseInstance)).Cast<TreeNodeBase>().ToList();
            }
            // TODO: Add other servers here
            else
            {
                throw new Exception("Unrecognized database server " + databaseConnection.GetType());
            }
        }

        public static IList<TreeNodeBase> GetLoginNodes([NotNull] IEnumerable<Login> logins, [NotNull] DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
        {
            return logins.Select(login => new LoginTreeNode(login, databaseConnection, databaseInstance)).Cast<TreeNodeBase>().ToList();
        }
    }
}
