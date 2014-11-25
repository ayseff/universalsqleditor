using System;
using System.Collections.ObjectModel;
using System.Reflection;
using SqlEditor.Databases.Db2;
using SqlEditor.Databases.MsAccess;
using SqlEditor.Databases.MySql;
using SqlEditor.Databases.Oracle;
using SqlEditor.Databases.PostgreSql;
using SqlEditor.Databases.SqlCe;
using SqlEditor.Databases.SqlServer;
using SqlEditor.Databases.Sqlite;
using log4net;

namespace SqlEditor.Databases
{
    public class DatabaseServerFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static DatabaseServerFactory _instance;

        private ReadOnlyCollection<DatabaseServer> _list;

        private DatabaseServerFactory()
        {
        }

        public static DatabaseServerFactory Instance
        {
            get { return _instance ?? (_instance = new DatabaseServerFactory()); }
        }

        public ReadOnlyCollection<DatabaseServer> SupportedServers
        {
            get
            {
                if (_list == null)
                {
                    _list = new ReadOnlyCollection<DatabaseServer>(new DatabaseServer[]
                                                                       {
                                                                           new SqlServerDatabaseServer(),
                                                                           new OracleDatabaseServer(),
                                                                           new Db2DatabaseServer(),
                                                                           new MySqlDatabaseServer(),
                                                                           new PostgreSqlDatabaseServer(), 
                                                                           new MsAccess2003DatabaseServer(),
                                                                           new MsAccess2007DatabaseServer(),
                                                                           new SqliteDatabaseServer(),
                                                                           new SqlCeDatabaseServer()
                                                                       });
                }
                return _list;
            }
        }

        public DatabaseServer GetDatabaseServer(string serverType)
        {
            serverType = serverType.Trim().ToUpper();
            if (serverType == "ORACLE")
            {
                return new OracleDatabaseServer();
            }
            else if (string.Equals(serverType, "DB2", StringComparison.InvariantCultureIgnoreCase))
            {
                return new Db2DatabaseServer();
            }
            else if (string.Equals(serverType, "SQL SERVER", StringComparison.InvariantCultureIgnoreCase) 
                || string .Equals(serverType, "SQLSERVER", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SqlServerDatabaseServer();
            }
            else if (string.Equals(serverType, "SQL SERVER COMPACT", StringComparison.InvariantCultureIgnoreCase) 
                || string.Equals(serverType, "SQL CE", StringComparison.InvariantCultureIgnoreCase) 
                || string.Equals(serverType, "SQLCE", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SqlCeDatabaseServer();
            }
            else if (string.Equals(serverType, "MYSQL", StringComparison.InvariantCultureIgnoreCase) ||
                     string.Equals(serverType, "MY SQL", StringComparison.InvariantCultureIgnoreCase))
            {
                return new MySqlDatabaseServer();
            }
            else if (string.Equals(serverType, "PostgreSQL", StringComparison.InvariantCultureIgnoreCase))
            {
                return new PostgreSqlDatabaseServer();
            }
            else if (serverType == "MICROSOFT ACCESS 2003" || serverType == "MS ACCESS 2003" ||
                     serverType == "MSACCESS2003" || serverType == "ACCESS32003")
            {
                return new MsAccess2003DatabaseServer();
            }
            else if (serverType == "MICROSOFT ACCESS 2007" || serverType == "MS ACCESS 2007" ||
                     serverType == "MSACCESS2007" || serverType == "ACCESS32007")
            {
                return new MsAccess2007DatabaseServer();
            }
            else if (string.Equals(serverType, "SQLITE", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SqliteDatabaseServer();
            }
            var message = string.Format("Unrecognized server type {0}.", serverType);
            _log.Error(message);
            throw new Exception(message);
        }

        public string GetConnectionOpenImage(DatabaseServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }
            // TODO: Assign actual images
            if (server is OracleDatabaseServer)
            {
                return "oracle.png";
            }
            else if (server is SqlCeDatabaseServer)
            {
                return "sql_server_ce.png";
            }
            else if (server is SqlServerDatabaseServer)
            {
                return "sql_server.png";
            }
            else if (server is Db2DatabaseServer)
            {
                return "db2.png";
            }
            else if (server is SqliteDatabaseServer)
            {
                return "sqlite.png";
            }
            else if (server is MySqlDatabaseServer)
            {
                return "mysql.png";
            }
            else if (server is PostgreSqlDatabaseServer)
            {
                return "postgresql.png";
            }
            else if (server is MsAccess2003DatabaseServer)
            {
                return "access.png";
            }
            string message = string.Format("Unrecognized server type {0}.", server.Name);
            _log.Error(message);
            throw new Exception(message);
        }

        public string GetConnectionClosedImage(DatabaseServer server)
        {
            // TODO: Assign actual images
            if (server is OracleDatabaseServer)
            {
                return "oracle.png";
            }
            else if (server is SqlCeDatabaseServer)
            {
                return "sql_server_ce.png";
            }
            else if (server is SqlServerDatabaseServer)
            {
                return "sql_server.png";
            }
            else if (server is Db2DatabaseServer)
            {
                return "db2.png";
            }
            else if (server is SqliteDatabaseServer)
            {
                return "sqlite.png";
            }
            else if (server is MySqlDatabaseServer)
            {
                return "mysql.png";
            }
            else if (server is PostgreSqlDatabaseServer)
            {
                return "postgresql.png";
            }
            else if (server is MsAccess2003DatabaseServer)
            {
                return "access.png";
            }
            string message = string.Format("Unrecognized server type {0}.", server.Name);
            _log.Error(message);
            throw new Exception(message);
        }

        public string GetSchemaImage(DatabaseServer server)
        {
            if (server is OracleDatabaseServer)
            {
                return "user.png";
            }
            else if (server is SqlCeDatabaseServer)
            {
                return "database.png";
            }
            else if (server is SqlServerDatabaseServer)
            {
                return "user.png";
            }
            else if (server is Db2DatabaseServer)
            {
                return "user.png";
            }
            else if (server is SqliteDatabaseServer)
            {
                return "database.png";
            }
            else if (server is MySqlDatabaseServer)
            {
                return "database_yellow.png";
            }
            else if (server is PostgreSqlDatabaseServer)
            {
                return "user.png";
            }
            else if (server is MsAccess2003DatabaseServer)
            {
                return "database.png";
            }
            var message = string.Format("Unrecognized server type {0}.", server.Name);
            _log.Error(message);
            throw new Exception(message);
        }

        public string GetDatabaseInstanceImage(DatabaseServer server)
        {
            return "database_yellow.png";
        }
    }
}