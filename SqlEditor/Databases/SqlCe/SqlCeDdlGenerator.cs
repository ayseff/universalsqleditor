using System;
using ErikEJ.SqlCeScripting;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.SqlHelpers;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeDdlGenerator : DdlGenerator
    {
        public override string GenerateCreateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var repo = new DB4Repository(databaseConnection.ConnectionString);

            var generator = new Generator4(repo);
            generator.GenerateTableCreate(tableName);
            return generator.GeneratedScript;
        }

        public override string GenerateCreateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var repo = new DB4Repository(databaseConnection.ConnectionString);

            var generator = new Generator4(repo);
            generator.GenerateTableCreate(tableName);

            var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                var indexes = infoProvider.GetIndexesForTable(connection, schema, tableName);
                foreach (var index in indexes)
                {
                    generator.GenerateIndexScript(tableName, index.Name);
                }
            }

            return generator.GeneratedScript;
        }

        public override string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            throw new NotSupportedException("This database does not support views");
        }

        public override string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            throw new NotSupportedException("This database does not support views");
        }

        public override string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, string indexSchema, string indexName, object indexId)
        {
            var repo = new DB4Repository(databaseConnection.ConnectionString);

            string tableName;
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.BuildSqlCommand(
                        "SELECT TOP 1 TABLE_NAME from information_schema.indexes WHERE UPPER(INDEX_NAME) = @1", "@",
                        indexName.Trim().ToUpper());
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tableName = dr.GetString(0).Trim().ToUpper();
                        }
                        else
                        {
                            throw new Exception("Index " + indexName + " does not exist in the database");
                        }
                    }
                }
            }

            var generator = new Generator4(repo);
            generator.GenerateIndexScript(tableName, indexName);
            return generator.GeneratedScript;
        }

        public override string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema,
            string packageName)
        {
            throw new NotImplementedException();
        }
    }
}