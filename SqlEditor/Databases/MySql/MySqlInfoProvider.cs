using System;
using System.Collections.Generic;
using System.Data;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;

namespace SqlEditor.Databases.MySql
{
    public class MySqlInfoProvider : DbInfoProvider
    {
        public override IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection)
        {
            throw new NotImplementedException();
        }

        public override IList<Schema> GetSchemas(IDbConnection connection, DatabaseInstance databaseInstance = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            try
            {
                return GetSchemasBase(connection, "SHOW DATABASES");
            }
            catch
            { }

            return GetSchemasBase(connection, "SELECT schema_name FROM information_schema.schemata ORDER BY schema_name");
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return GetTablesBase(connection, schemaName, "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND UPPER(table_schema) = @1", schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTableColumns([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                      [NotNull] string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName, "SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position  FROM information_schema.columns WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 ORDER BY ordinal_position", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName, "SELECT c.column_name, c.data_type, c.character_maximum_length, c.numeric_precision, c.numeric_scale, c.is_nullable, c.ordinal_position   FROM information_schema.columns c INNER JOIN information_schema.key_column_usage k 	USING(table_schema,table_name,column_name) INNER JOIN information_schema.table_constraints t 	USING(table_schema,table_name,constraint_name) WHERE UPPER(c.table_schema) = @1 AND UPPER(c.table_name) = @2  AND t.constraint_type='PRIMARY KEY'", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTablePartitionsBase(connection, schemaName, tableName, "SELECT partition_name  FROM information_schema.partitions WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 and partition_name is not null ORDER BY partition_ordinal_position", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<View> GetViews([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            return GetViewsBase(connection, schemaName, "SELECT table_name FROM information_schema.views WHERE UPPER(table_schema) = @1", schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");
            return GetTableColumnsBase(connection, schemaName, viewName, "SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position FROM information_schema.columns WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 ORDER BY ordinal_position", schemaName.Trim().ToUpper(), viewName.Trim().ToUpper());
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName)
        {
            return new List<MaterializedView>();
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName, string materializedViewName)
        {
            return new List<Column>();
        }

        public override IList<Index> GetIndexes([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT index_schema,  index_name, CASE WHEN non_unique = 0 THEN 1 ELSE 0 END as is_unique FROM  information_schema.STATISTICS WHERE UPPER(table_schema) = @1 ORDER BY index_name",
                                  schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT index_schema, index_name, CASE WHEN non_unique = 0 THEN 1 ELSE 0 END as is_unique FROM  information_schema.STATISTICS WHERE UPPER(table_schema) = @1 AND table_name = @2 ORDER BY index_name",
                                  schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                      [NotNull] string indexName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            return GetIndexColumnsBase(connection, schemaName, indexName,
                                       "SELECT c.column_name, c.data_type, c.character_maximum_length, c.numeric_precision, c.numeric_scale, c.is_nullable, c.ordinal_position FROM information_schema.STATISTICS s INNER JOIN information_schema.COLUMNS c ON c.TABLE_SCHEMA = s.TABLE_SCHEMA AND c.TABLE_NAME = s.TABLE_NAME AND c.COLUMN_NAME = s.COLUMN_NAME WHERE UPPER(s.INDEX_NAME) = @1 and UPPER(s.TABLE_SCHEMA) = @2 ORDER BY seq_in_index",
                                       indexName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName)
        {
            return new List<Sequence>();
        }

        public override IList<Trigger> GetTriggers([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTriggersBase(connection, schemaName,
                                   "SELECT trigger_name FROM information_schema.triggers WHERE UPPER(trigger_schema) = @1 ORDER BY trigger_name",
                                   schemaName.Trim().ToUpper());
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName)
        {
            return new List<Synonym>();
        }

        public override IList<Synonym> GetSynonyms(IDbConnection connection, string schemaName)
        {
            return new List<Synonym>();
        }

        public override IList<StoredProcedure> GetStoredProcedures(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            var procedures = GetStoredProceduresBase(connection, schemaName,
                                  "SELECT r.specific_name, r.routine_name, '' as routine_definition FROM information_schema.routines r WHERE UPPER(r.routine_schema) = @1 AND r.routine_type = 'PROCEDURE' ORDER BY r.routine_name",
                                  schemaName.ToUpper());
            using (var command = connection.CreateCommand())
            {
                foreach (var storedProcedure in procedures)
                {
                    command.CommandText = "SHOW CREATE PROCEDURE " + storedProcedure.FullyQualifiedName;
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr != null && dr.Read())
                        {
                            storedProcedure.Definition = dr.IsDBNull(2) ? null : dr.GetString(2);
                        }
                    }

                }
            }
            return procedures;
        }

        public override IList<Function> GetFunctions(IDbConnection connection, string schemaName)
        {
            throw new NotImplementedException();
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

            const string sql =
                "SELECT parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 'YES' as is_nullable, ordinal_position, parameter_mode  FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2 ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, storedProcedure, sql,
                                                        storedProcedure.Parent.Name.ToUpper(),
                                                        storedProcedure.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                throw new Exception("Showing procedure parameters requires MySQL version 5.5 or greater", ex);
            }
        }

        public override IList<ColumnParameter> GetFunctionParameters(IDbConnection connection, Function function)
        {
            throw new NotImplementedException();
        }

        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            throw new NotImplementedException();
        }

        public override IntelisenseData GetIntelisenseData([NotNull] IDbConnection connection,
                                                           [NotNull] string currentSchemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (currentSchemaName == null) throw new ArgumentNullException("currentSchemaName");
            const string tablesSql =
                "SELECT table_schema, table_name, column_name FROM information_schema.columns ORDER BY table_schema, table_name, column_name";
            //const string viewsSql =
            //    "SELECT v.viewschema, v.viewname, c.colname FROM syscat.VIEWS v INNER JOIN syscat.columns c on v.viewname = c.tabname AND v.viewschema =c.tabschema  ORDER BY tabschema, tabname, colname WITH ur";
            return GetIntellisenseDataHelper(connection, currentSchemaName, tablesSql, null, null);
        }
    }
}
