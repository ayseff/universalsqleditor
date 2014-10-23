using System;
using ErikEJ.SqlCeScripting;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeDdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var repo = new DB4Repository(databaseConnection.ConnectionString);

            var generator = new Generator4(repo);
            generator.GenerateTableCreate(tableName);
            return generator.GeneratedScript;
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
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

        public override string GenerateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            throw new NotSupportedException("This database does not support views");
        }

        public override string GenerateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            throw new NotSupportedException("This database does not support views");
        }
    }
}