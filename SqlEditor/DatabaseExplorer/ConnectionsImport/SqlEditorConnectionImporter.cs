using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using SqlEditor.Databases;
using log4net;

namespace SqlEditor.DatabaseExplorer.ConnectionsImport
{
    public class SqlEditorConnectionImporter
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof (Connections));

        public static List<DatabaseConnection> ImportConnections(string connectionsXmlFileName, string encryptionKey)
        {
            if (connectionsXmlFileName == null) throw new ArgumentNullException("connectionsXmlFileName");
            if (encryptionKey == null) throw new ArgumentNullException("encryptionKey");

            var connectionList = new List<DatabaseConnection>();
            try
            {
                _log.Debug("Loading connections ...");
                if (!File.Exists(connectionsXmlFileName))
                {
                    string message = string.Format(
                        "Could not load connections from file {0}. The file does not exist!", connectionsXmlFileName);
                    _log.Error(message);
                    throw new FileNotFoundException(connectionsXmlFileName);
                }

                Connections connections;
                using (var stream = new StreamReader(connectionsXmlFileName, Encoding.Unicode))
                {
                    using (var reader = new XmlTextReader(stream))
                    {
                        connections = (Connections) _serializer.Deserialize(reader);
                    }
                }

                foreach (var connection in connections.Items)
                {
                    try
                    {
                        var dbConnection = new DatabaseConnection
                                               {
                                                   Name = connection.Name,
                                                   DatabaseServer =
                                                       DatabaseServerFactory.Instance.GetDatabaseServer(connection.Server),
                                                   ConnectionString = connection.ConnectionString,
                                                   Folder = connection.Folder,
                                                   MaxResults = connection.MaxResults,
                                                   AutoCommit = connection.AutoCommit
                                               };
                        connectionList.Add(dbConnection);
                        
                        // Set connection string by using the encrypted password
                        if (!string.IsNullOrEmpty(connection.Password))
                        {
                            dbConnection.ConnectionString =
                                dbConnection.DatabaseServer.SetPassword(connection.ConnectionString,
                                                                        Crypto.DecryptStringAES(connection.Password,
                                                                            EncryptionInfo.EncryptionKey));
                        }

                    }
                    catch (Exception ex)
                    {
                        _log.ErrorFormat("Error parsing connection with connection string {0}.", connection.ConnectionString);
                        _log.Error(ex.Message, ex);
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

        public static void ExportConnections(IList<DatabaseConnection> connectionList, string connectionsXmlFileName, string encryptionKey)
        {
            if (connectionList == null) throw new ArgumentNullException("connectionList");
            if (connectionsXmlFileName == null) throw new ArgumentNullException("connectionsXmlFileName");
            try
            {
                _log.Debug("Exporting connections ...");
                var xmlConnections = new Connections {Items = new ConnectionsConnection[connectionList.Count]};
                for (var i = 0; i < connectionList.Count; i++)
                {
                    DatabaseConnection connection = connectionList[i];
                    var xmlConnection = new ConnectionsConnection
                                            {
                                                ConnectionString = connection.DatabaseServer.SetPassword(connection.ConnectionString, null),
                                                Folder = connection.Folder,
                                                Name = connection.Name,
                                                Server = connection.DatabaseServer.Name,
                                                MaxResults = connection.MaxResults,
                                                Password = string.IsNullOrEmpty(connection.DatabaseServer.GetPassword(connection.ConnectionString)) ? string.Empty : Crypto.EncryptStringAES(connection.DatabaseServer.GetPassword(connection.ConnectionString), encryptionKey),
                                                AutoCommit = connection.AutoCommit
                                            };
                    xmlConnections.Items[i] = xmlConnection;
                }

                using (var stream = new FileStream(connectionsXmlFileName, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream, Encoding.Unicode))
                    {
                        _serializer.Serialize(writer, xmlConnections);
                    }
                }
                _log.DebugFormat("Exported {0} connection(s).", connectionList.Count.ToString("#,0"));
            }
            catch (Exception ex)
            {
                _log.Error("Error exporting connections.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}