using System;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases.MySql
{
    public class MySqlDdlGenerator : DdlGenerator
    {
        public override string GenerateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            return RunShowCreateStatement(databaseConnection, schema, tableName, "TABLE");
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            return GenerateTableDdl(databaseConnection, database, schema, tableName);
        }

        public override string GenerateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            return RunShowCreateStatement(databaseConnection, schema, viewName, "VIEW");
        }

        public override string GenerateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            return GenerateViewDdl(databaseConnection, database, schema, viewName);
        }

        private static string RunShowCreateStatement(DatabaseConnection databaseConnection, string schema, string tableName,
            [NotNull] string objectType)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");
            if (objectType == null) throw new ArgumentNullException("objectType");

            // Get full DDL
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SHOW CREATE {0} {1}.{2}", objectType, schema, tableName);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return dr.GetString(1);
                        }
                        throw new Exception("MySQL SHOW CREATE " + objectType + " statement did not return any rows");
                    }
                }
            }
        }
    }
}