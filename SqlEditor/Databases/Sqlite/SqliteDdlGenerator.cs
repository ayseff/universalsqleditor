using System;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteDdlGenerator : DdlGenerator
    {
        public override string GenerateCreateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            return RunSqlLiteMasterQuery(databaseConnection, tableName, "table");
        }

        public override string GenerateCreateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(GenerateCreateTableDdl(databaseConnection, database, schema, tableName));
            stringBuilder.AppendLine(Environment.NewLine);

            var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var indexes = infoProvider.GetIndexesForTable(connection, string.Empty, tableName);
                foreach (var index in indexes)
                {
                    var columns = infoProvider.GetIndexColumns(connection, schema, tableName, string.Empty, index.Name);
                    stringBuilder.AppendFormat("CREATE {0}INDEX {1} ON {2}({3})",
                        index.IsUnique ? "UNIQUE " : string.Empty, index.Name, tableName,
                        string.Join(", ", columns.Select(x => x.Name).ToList()));
                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine(databaseConnection.DatabaseServer.SqlTerminators.FirstOrDefault());
                    stringBuilder.AppendLine(Environment.NewLine);
                }
            }
            return stringBuilder.ToString();
        }

        public override string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            return RunSqlLiteMasterQuery(databaseConnection, viewName, "view");
        }

        public override string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            return GenerateCreateViewDdl(databaseConnection, database, schema, viewName);
        }

        public override string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, string indexSchema, string indexName, object indexId)
        {
            return RunSqlLiteMasterQuery(databaseConnection, indexName, "index");
        }

        public override string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema,
            string packageName)
        {
            throw new NotImplementedException();
        }

        private static string RunSqlLiteMasterQuery(DatabaseConnection databaseConnection, string objectName,
            [NotNull] string objectType)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (objectName == null) throw new ArgumentNullException("objectName");
            if (objectType == null) throw new ArgumentNullException("objectType");

            using (var connection = databaseConnection.CreateNewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                        string.Format("SELECT sql FROM sqlite_master WHERE lower(type) = '{0}' AND upper(name) = upper(@1)", objectType.ToLower());
                    var param = command.CreateParameter();
                    param.ParameterName = "@1";
                    param.Value = objectName;
                    command.Parameters.Add(param);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return (dr.IsDBNull(0) ? string.Empty : dr.GetString(0)) + databaseConnection.DatabaseServer.SqlTerminators.FirstOrDefault();
                        }
                        throw new Exception("SQLite sqlite_master " + objectType + " does not have an entry for " + objectName);
                    }
                }
            }
        }
    }
}