using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IBM.Data.DB2;
using Oracle.ManagedDataAccess.Client;
using SqlEditor.Database;
using SqlEditor.Databases;
using SqlEditor.Databases.Db2;
using SqlEditor.Databases.MySql;
using SqlEditor.Databases.Oracle;
using Utilities.IO;
using log4net;

namespace SqlEditor.DatabaseExplorer.ConnectionsImport
{
    public class AquaDataStudioConnectionImporter
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string BASE_DIR = @"\.datastudio\connections";
        public static List<DatabaseConnection> ImportConnections(string directoryOrFile)
        {
            if (directoryOrFile == null) throw new ArgumentNullException("directoryOrFile");
            if (!Directory.Exists(directoryOrFile) &&
                !File.Exists(directoryOrFile))
            {
                throw new ArgumentException("Directory of file " + directoryOrFile + " does not exist");
            }

            try
            {
                _log.Debug("Getting files ...");
                var files = new List<string>();
                if (Directory.Exists(directoryOrFile))
                {
                    files.AddRange(Directory.GetFiles(directoryOrFile, "*.conn", SearchOption.AllDirectories));
                }
                else
                {
                    files.Add(directoryOrFile);
                }
                _log.DebugFormat("Found {0} file(s).", files.Count);

                _log.DebugFormat("Getting base path ...");
                var index = directoryOrFile.IndexOf(BASE_DIR, StringComparison.Ordinal);
                if (index < 0)
                {
                    throw new Exception("Could not find base path " + BASE_DIR + " in specified path " + directoryOrFile);
                }
                var basePath = directoryOrFile.Substring(0, index) + BASE_DIR;
                _log.DebugFormat("Base path is {0}.", basePath);

                _log.Debug("Parsing connections ...");
                var connectionList = new List<DatabaseConnection>();
                foreach (var file in files)
                {
                    try
                    {
                        _log.DebugFormat("Parsing connection file {0} ...", file);
                        var connection = GetConnection(file, basePath);
                        if (connection != null)
                        {
                            connectionList.Add(connection);
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Error parsing connection file " + file);
                        _log.Error(ex.Message, ex);
                        throw;
                    }
                }

                _log.DebugFormat("Loaded {0} connection(s).", connectionList.Count);
                return connectionList;
            }
            catch (Exception ex)
            {
                _log.Error("Error loading connections.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        private static DatabaseConnection GetConnection(string aquaStudioConnectionFile, string basePath)
        {
            var lines =
                File.ReadAllLines(aquaStudioConnectionFile)
                    .Where(x => x.Trim().ToLower().StartsWith("connection."))
                    .ToList();
            var connection = new DatabaseConnection();
            connection.DatabaseServer = GetDatabaseServer(lines);
            connection.Folder = @"Connections\" +
                                Path.GetDirectoryName(aquaStudioConnectionFile).GetRelativePath(basePath);
            connection.MaxResults = 100;
            connection.Name = Path.GetFileNameWithoutExtension(aquaStudioConnectionFile);
            if (connection.DatabaseServer is Db2DatabaseServer)
            {
                var sb = (DB2ConnectionStringBuilder) connection.DatabaseServer.GetConnectionStringBuilder();
                sb.Server = GetPropertyValue(lines, "connection.host") + ":" + GetPropertyValue(lines, "connection.port");
                sb.UserID = GetPropertyValue(lines, "connection.username");
                sb.Password = string.Empty;
                sb.Database = GetPropertyValue(lines, "connection.database");
                connection.ConnectionString = sb.ConnectionString;
                return connection;
            }
            else if (connection.DatabaseServer is OracleDatabaseServer)
            {
                var sb = (OracleConnectionStringBuilder)connection.DatabaseServer.GetConnectionStringBuilder();
                sb.DataSource = GetPropertyValue(lines, "connection.sid").Replace("SERVICE:", string.Empty);
                sb.UserID = GetPropertyValue(lines, "connection.username");
                sb.Password = string.Empty;
                connection.ConnectionString = sb.ConnectionString;
                return connection;
            }
            return null;
        }

        private static DatabaseServer GetDatabaseServer(IEnumerable<string> lines)
        {
            var serverType = GetPropertyValue(lines, "connection.rdbms").ToLower();
            if (string.IsNullOrEmpty(serverType))
            {
                throw new Exception("Could not determine database server type");
            }
            
            if (serverType.Contains("db2"))
            {
                return  new Db2DatabaseServer();
            }
            else if (serverType.Contains("oracle"))
            {
                return new OracleDatabaseServer();
            }
            else if (serverType.Contains("mssqlserver"))
            {
                return new OracleDatabaseServer();
            }
            else if (serverType.Contains("mysql"))
            {
                return new MySqlDatabaseServer();
            }
            throw new Exception("Unrecognized database server type " + serverType);
        }

        private static string GetPropertyValue(IEnumerable<string> lines, string propertyName)
        {
            var line = lines.FirstOrDefault(x => x.Trim().ToLower().StartsWith(propertyName.ToLower() + "="));
            if (line == null)
            {
                return string.Empty;
            }
            var index = line.IndexOf("=", StringComparison.Ordinal);
            if (index == line.Length)
            {
                return string.Empty;
            }
            return line.Substring(index + 1);
        }
    }
}