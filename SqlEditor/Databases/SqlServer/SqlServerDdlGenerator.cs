using System;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using SqlEditor.DatabaseExplorer;
using Microsoft.SqlServer.Management.Smo;


namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerDdlGenerator : DdlGenerator
    {
        public override string GenerateCreateTableDdl([NotNull] DatabaseConnection databaseConnection,
            [NotNull] string database, [NotNull] string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var options = new ScriptingOptions();
            options.ClusteredIndexes = true;
            options.DriAll = true;
            options.FullTextIndexes = true;
            options.IncludeDatabaseContext = true;
            options.IncludeHeaders = true;
            options.Indexes = true;
            options.SchemaQualify = true;
            options.Triggers = true;
            options.XmlIndexes = true;
            options.ScriptBatchTerminator = true;
            options.BatchSize = 1;

            return GenerateTableDdlInternal(databaseConnection, database, schema, tableName, options);
        }

        public override string GenerateCreateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (schema == null) throw new ArgumentNullException("schema");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var options = new ScriptingOptions();
            options.WithDependencies = true;
            options.ClusteredIndexes = true;
            options.DriAll = true;
            options.FullTextIndexes = true;
            options.IncludeDatabaseContext = true;
            options.IncludeHeaders = true;
            options.Indexes = true;
            options.SchemaQualify = true;
            options.Triggers = true;
            options.XmlIndexes = true;
            options.ScriptBatchTerminator = true;
            options.BatchSize = 1;

            return GenerateTableDdlInternal(databaseConnection, database, schema, tableName, options);
        }

        public override string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (schema == null) throw new ArgumentNullException("schema");
            if (viewName == null) throw new ArgumentNullException("viewName");

            var options = new ScriptingOptions();
            options.ClusteredIndexes = true;
            options.DriAll = true;
            options.FullTextIndexes = true;
            options.IncludeDatabaseContext = true;
            options.IncludeHeaders = true;
            options.Indexes = true;
            options.SchemaQualify = true;
            options.Triggers = true;
            options.XmlIndexes = true;
            options.ScriptBatchTerminator = true;
            options.BatchSize = 1;

            return GenerateViewDdlInternal(databaseConnection, database, schema, viewName, options);
        }

        public override string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (schema == null) throw new ArgumentNullException("schema");
            if (viewName == null) throw new ArgumentNullException("viewName");

            var options = new ScriptingOptions();
            options.WithDependencies = true;
            options.ClusteredIndexes = true;
            options.DriAll = true;
            options.FullTextIndexes = true;
            options.IncludeDatabaseContext = true;
            options.IncludeHeaders = true;
            options.Indexes = true;
            options.SchemaQualify = true;
            options.Triggers = true;
            options.XmlIndexes = true;
            options.ScriptBatchTerminator = true;
            options.BatchSize = 1;

            return GenerateViewDdlInternal(databaseConnection, database, schema, viewName, options);
        }

        public override string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, string indexSchema, string indexName, object indexId)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (indexSchema == null) throw new ArgumentNullException("indexSchema");
            if (indexName == null) throw new ArgumentNullException("indexName");

            var options = new ScriptingOptions();
            options.ClusteredIndexes = true;
            options.DriAll = true;
            options.FullTextIndexes = true;
            options.IncludeDatabaseContext = true;
            options.IncludeHeaders = true;
            options.Indexes = true;
            options.SchemaQualify = true;
            options.Triggers = true;
            options.XmlIndexes = true;
            options.ScriptBatchTerminator = true;
            options.BatchSize = 1;

            var cb = new SqlConnectionStringBuilder(databaseConnection.ConnectionString)
            {
                MultipleActiveResultSets = false
            };

            // Connect to the local, default instance of SQL Server. 
            using (var dbConnection = new SqlConnection(cb.ConnectionString))
            {
                dbConnection.Open();
                var connection = new ServerConnection(dbConnection);
                var srv = new Server(connection);

                // Reference the database
                if (srv.Databases.Cast<Microsoft.SqlServer.Management.Smo.Database>().All(x => !string.Equals(x.Name, database, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new Exception("Database '" + database + "' does not exist");
                }
                var db = srv.Databases[database];

                // Define a Scripter object and set the required scripting options. 
                var scrp = new Scripter(srv) { Options = options };
                var tables = db.Tables.Cast<Microsoft.SqlServer.Management.Smo.Table>().ToList();
                var index = tables.SelectMany(x => x.Indexes.Cast<Microsoft.SqlServer.Management.Smo.Index>())
                    .FirstOrDefault(x => x.ID == (int) indexId);
                if (index == null)
                {
                    throw new Exception("Could not find index " + indexSchema + "." + indexName);
                }
                else if (index.IsSystemObject)
                {
                    throw new Exception("Index " + indexSchema + "." + indexName + " is a system object");
                }

                return ScriptObject(databaseConnection, scrp, index.Urn);
            }
        }

        public override string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema,
            string packageName)
        {
            throw new NotImplementedException();
        }

        private static string GenerateTableDdlInternal([NotNull] DatabaseConnection databaseConnection, [NotNull] string database, string schema, string tableName,
            [NotNull] ScriptingOptions options)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (options == null) throw new ArgumentNullException("options");

            var cb = new SqlConnectionStringBuilder(databaseConnection.ConnectionString)
            {
                MultipleActiveResultSets = false
            };

            // Connect to the local, default instance of SQL Server. 
            using (var dbConnection = new SqlConnection(cb.ConnectionString))
            {
                dbConnection.Open();
                var connection = new ServerConnection(dbConnection);
                var srv = new Server(connection);

                // Reference the database
                if (srv.Databases.Cast<Microsoft.SqlServer.Management.Smo.Database>().All(x => !string.Equals(x.Name, database, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new Exception("Database '" + database + "' does not exist");
                }
                var db = srv.Databases[database];

                // Define a Scripter object and set the required scripting options. 
                var scrp = new Scripter(srv) {Options = options};
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
                
                return ScriptObject(databaseConnection, scrp, table.Urn);
            }
        }

        private static string GenerateViewDdlInternal([NotNull] DatabaseConnection databaseConnection, [NotNull] string database, string schema, string viewName,
            [NotNull] ScriptingOptions options)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (database == null) throw new ArgumentNullException("database");
            if (options == null) throw new ArgumentNullException("options");

            var cb = new SqlConnectionStringBuilder(databaseConnection.ConnectionString)
            {
                MultipleActiveResultSets = false
            };

            // Connect to the local, default instance of SQL Server. 
            using (var dbConnection = new SqlConnection(cb.ConnectionString))
            {
                dbConnection.Open();
                var connection = new ServerConnection(dbConnection);
                var srv = new Server(connection);

                // Reference the database
                if (srv.Databases.Cast<Microsoft.SqlServer.Management.Smo.Database>().All(x => !string.Equals(x.Name, database, StringComparison.InvariantCultureIgnoreCase)))
                {
                    throw new Exception("Database '" + database + "' does not exist");
                }
                var db = srv.Databases[database];

                // Define a Scripter object and set the required scripting options. 
                var scrp = new Scripter(srv) { Options = options };
                var view = db.Views.Cast<Microsoft.SqlServer.Management.Smo.View>().FirstOrDefault(x => string.Equals(x.Schema, schema, StringComparison.InvariantCultureIgnoreCase)
                                                                                                           && string.Equals(x.Name, viewName, StringComparison.InvariantCultureIgnoreCase));
                if (view == null)
                {
                    throw new Exception("Could not find view " + schema + "." + viewName);
                }
                else if (view.IsSystemObject)
                {
                    throw new Exception("View " + schema + "." + viewName + " is a system object");
                }

                return ScriptObject(databaseConnection, scrp, view.Urn);
            }
        }

        private static string ScriptObject(DatabaseConnection databaseConnection, Scripter scrp, Urn urn)
        {
            var sb = new StringBuilder();
            var stringCollection = scrp.Script(new[] {urn});
            foreach (var line in stringCollection)
            {
                sb.Append(line);
                sb.AppendLine(databaseConnection.DatabaseServer.SqlTerminators.FirstOrDefault());
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}