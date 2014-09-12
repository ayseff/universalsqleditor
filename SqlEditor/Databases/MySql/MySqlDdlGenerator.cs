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
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            // Get full DDL
            using (var connection = databaseConnection.CreateNewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SHOW CREATE TABLE {0}.{1}", schema, tableName);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return dr.GetString(1);
                        }
                        throw new Exception("MySQL SHOW CREATE TABLE statement did not return any rows");
                    }
                }
            }
        }

        public override string GenerateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            using (var connection = databaseConnection.CreateNewConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = string.Format("SHOW CREATE TABLE {0}.{1}", schema, tableName);
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return dr.GetString(1);
                        }
                        throw new Exception("MySQL SHOW CREATE TABLE statement did not return any rows");
                    }
                }
            }
            throw new NotImplementedException();
        }
    }
}