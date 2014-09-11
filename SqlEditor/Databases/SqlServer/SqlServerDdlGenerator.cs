using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using IBM.Data.DB2;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Common;
using SqlEditor.DatabaseExplorer;
using Utilities.Process;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk.Sfc;


namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerDdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");
            
            var cb = new SqlConnectionStringBuilder(databaseConnection.ConnectionString);
            var dbName = cb.InitialCatalog;

            // Connect to the local, default instance of SQL Server. 
            using (var dbConnection = (SqlConnection) databaseConnection.CreateNewConnection())
            {
                var connection = new ServerConnection(dbConnection);
                var srv = new Server(connection);

                // Reference the database.  
                var db = srv.Databases[dbName];

                // Define a Scripter object and set the required scripting options. 
                var scrp = new Scripter(srv);
                //scrp.Options.ScriptDrops = false;
                //scrp.Options.WithDependencies = true;
                //scrp.Options.Indexes = true; // To include indexes
                scrp.Options.DriAllConstraints = true; // to include referential constraints in the script

                var table = db.Tables.Cast<Microsoft.SqlServer.Management.Smo.Table>().FirstOrDefault(x => string.Equals(x.Schema, schema, StringComparison.InvariantCultureIgnoreCase)
                                                                                                           && string.Equals(x.Name, tableName, StringComparison.InvariantCultureIgnoreCase));
                if (table == null)
                {
                    throw new Exception("Could not find table " + schema + "." + tableName);
                }
                else if (table.IsSystemObject)
                {
                    throw new Exception("Table " + schema + "." + tableName + " is a system object");
                }

                var sb = new StringBuilder();
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { table.Urn });
                foreach (string st in sc)
                {
                    sb.AppendLine(st);
                }
                return sb.ToString();
            }
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            var cb = new SqlConnectionStringBuilder(databaseConnection.ConnectionString);
            var dbName = cb.InitialCatalog;

            // Connect to the local, default instance of SQL Server. 
            using (var dbConnection = (SqlConnection)databaseConnection.CreateNewConnection())
            {
                var connection = new ServerConnection(dbConnection);
                var srv = new Server(connection);

                // Reference the database.  
                var db = srv.Databases[dbName];

                // Define a Scripter object and set the required scripting options. 
                var scrp = new Scripter(srv);
                scrp.Options.WithDependencies = true;
                scrp.Options.Indexes = true; // To include indexes
                scrp.Options.IncludeDatabaseContext = true;
                scrp.Options.DriAll = true; // to include referential constraints in the script
                scrp.Options.IncludeHeaders = true;
                scrp.Options.SchemaQualify = true;

                var table = db.Tables.Cast<Microsoft.SqlServer.Management.Smo.Table>().FirstOrDefault(x => string.Equals(x.Schema, schema, StringComparison.InvariantCultureIgnoreCase)
                                                                                                           && string.Equals(x.Name, tableName, StringComparison.InvariantCultureIgnoreCase));
                if (table == null)
                {
                    throw new Exception("Could not find table " + schema + "." + tableName);
                }
                else if (table.IsSystemObject)
                {
                    throw new Exception("Table " + schema + "." + tableName + " is a system object");
                }

                var sb = new StringBuilder();
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { table.Urn });
                foreach (string st in sc)
                {
                    sb.AppendLine(st);
                }
                return sb.ToString();
            }
        }

        private string GenerateTableDdl(string connectionString, string schema, string tableName)
        {
            var cb = new SqlConnectionStringBuilder(connectionString);
            cb.MultipleActiveResultSets = false;
            var dbName = cb.InitialCatalog;

            // Connect to the local, default instance of SQL Server. 
            using (var dbConnection = (SqlConnection)databaseConnection.CreateNewConnection())
            {
                var connection = new ServerConnection(dbConnection);
                var srv = new Server(connection);

                // Reference the database.  
                var db = srv.Databases[dbName];

                // Define a Scripter object and set the required scripting options. 
                var scrp = new Scripter(srv);
                scrp.Options.WithDependencies = true;
                scrp.Options.Indexes = true; // To include indexes
                scrp.Options.IncludeDatabaseContext = true;
                scrp.Options.DriAll = true; // to include referential constraints in the script
                scrp.Options.IncludeHeaders = true;
                scrp.Options.SchemaQualify = true;

                var table = db.Tables.Cast<Microsoft.SqlServer.Management.Smo.Table>().FirstOrDefault(x => string.Equals(x.Schema, schema, StringComparison.InvariantCultureIgnoreCase)
                                                                                                           && string.Equals(x.Name, tableName, StringComparison.InvariantCultureIgnoreCase));
                if (table == null)
                {
                    throw new Exception("Could not find table " + schema + "." + tableName);
                }
                else if (table.IsSystemObject)
                {
                    throw new Exception("Table " + schema + "." + tableName + " is a system object");
                }

                var sb = new StringBuilder();
                System.Collections.Specialized.StringCollection sc = scrp.Script(new Urn[] { table.Urn });
                foreach (string st in sc)
                {
                    sb.AppendLine(st);
                }
                return sb.ToString();
            }
        }
    }
}