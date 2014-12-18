using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.SqlHelpers;

namespace SqlEditor.Databases.MySql
{
    public class MySqlDdlGenerator : DdlGenerator
    {
        public override string GenerateCreateTableDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema,
            [NotNull] string tableName)
        {
            return RunShowCreateStatement(databaseConnection, schema, tableName, "TABLE");
        }

        public override string GenerateCreateTableFullDdl([NotNull] DatabaseConnection databaseConnection, string database, string schema, [NotNull] string tableName)
        {
            return GenerateCreateTableDdl(databaseConnection, database, schema, tableName);
        }

        public override string GenerateCreateViewDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            return RunShowCreateStatement(databaseConnection, schema, viewName, "VIEW");
        }

        public override string GenerateCreateViewFullDdl(DatabaseConnection databaseConnection, string database, string schema, string viewName)
        {
            return GenerateCreateViewDdl(databaseConnection, database, schema, viewName);
        }

        public override string GenerateCreateIndexDdl(DatabaseConnection databaseConnection, string database, string indexSchema, string indexName, object indexId)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (indexSchema == null) throw new ArgumentNullException("indexSchema");
            if (indexName == null) throw new ArgumentNullException("indexName");

            // Find the table index belongs
            string tableName = null, tableSchema = null, indexType = null;
            var nonUnique = 0;
            var columns = new List<string>();

            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.BuildSqlCommand("select s.TABLE_SCHEMA, s.TABLE_NAME, s.INDEX_TYPE, s.NON_UNIQUE, s.COLUMN_NAME from information_schema.STATISTICS s where UPPER(s.INDEX_NAME) = @1 and UPPER(s.INDEX_SCHEMA) = @2", "@", indexName.Trim().ToUpper(), indexSchema.Trim().ToUpper());
                    using (var dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            if (tableSchema == null)
                            {
                                tableSchema = dr.GetString(0);
                                tableName = dr.GetString(1);
                                indexType = dr.GetString(2);
                                nonUnique = dr.GetInt32(3);
                            }
                            columns.Add(dr.GetString(4));
                        }


                        if (tableName == null)
                        {
                            throw new Exception("Index " + indexSchema + "." + indexName +
                                                " does not exist in the database");
                        }
                    }
                }
            }
            var indexTypeValue = string.Empty;
            if (nonUnique == 0)
            {
                indexTypeValue = "UNIQUE ";
            }
            else if (indexType.Trim().ToUpper() != "BTREE")
            {
                indexTypeValue = indexType.Trim().ToUpper() + " ";
            }

            var sql = string.Format("CREATE {0}INDEX {1}.{2} ON {3}.{4} ({5})", indexTypeValue, indexSchema, indexName, tableSchema, tableName, string.Join(", ", columns));
            return sql;
        }

        public override string GenerateDropIndexDdl([NotNull] DatabaseConnection databaseConnection, string database,
            [NotNull] string schema, [NotNull] string indexName)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            if (schema == null) throw new ArgumentNullException("schema");
            if (indexName == null) throw new ArgumentNullException("indexName");

            // Find the table index belongs
            string tableName, tableSchema;
            using (var connection = databaseConnection.CreateNewConnection())
            {
                connection.OpenIfRequired();
                using (var command = connection.CreateCommand())
                {
                    command.BuildSqlCommand("select s.TABLE_SCHEMA, s.TABLE_NAME from information_schema.STATISTICS s where UPPER(s.INDEX_NAME) = @1 and UPPER(s.INDEX_SCHEMA) = @2 LIMIT 1", "@", indexName.Trim().ToUpper(), schema.Trim().ToUpper());
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tableSchema = dr.GetString(0);
                            tableName = dr.GetString(1);
                        }
                        else
                        {
                            throw new Exception("Index " + schema + "." + indexName +
                                                " does not exist in the database");
                        }
                    }
                }
            }

            return "DROP INDEX " + GetFullyQualifiedName(database, schema, indexName) + " ON " +
                   GetFullyQualifiedName(database, tableSchema, tableName) +
                   databaseConnection.DatabaseServer.SqlTerminators.First();

        }

        public override string GenerateCreatePackageDdl(DatabaseConnection databaseConnection, string database, string schema,
            string packageName)
        {
            throw new NotImplementedException();
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