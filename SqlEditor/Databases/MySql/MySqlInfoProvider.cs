using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using log4net;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;

namespace SqlEditor.Databases.MySql
{
    public class MySqlInfoProvider : DbInfoProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return GetDatabaseInstancesBase(connection, "SHOW DATABASES");
        }

        public override IList<Schema> GetSchemas(IDbConnection connection, string databaseInstance = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            try
            {
                return GetSchemasBase(connection, "SHOW DATABASES").OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error getting databases using SHOW DATABASES command. Will attempt using information schema.");
                _log.Error(ex.Message, ex);
            }

            return GetSchemasBase(connection, "SELECT schema_name FROM information_schema.schemata ORDER BY schema_name");
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return GetTablesBase(connection, schemaName, "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND UPPER(table_schema) = @1", schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTableColumns([NotNull] IDbConnection connection, [NotNull] string schemaName, [NotNull] string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName, "SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position  FROM information_schema.columns WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 ORDER BY ordinal_position", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName, "SELECT c.column_name, c.data_type, c.character_maximum_length, c.numeric_precision, c.numeric_scale, c.is_nullable, c.ordinal_position   FROM information_schema.columns c INNER JOIN information_schema.key_column_usage k 	USING(table_schema,table_name,column_name) INNER JOIN information_schema.table_constraints t 	USING(table_schema,table_name,constraint_name) WHERE UPPER(c.table_schema) = @1 AND UPPER(c.table_name) = @2  AND t.constraint_type='PRIMARY KEY'", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTablePartitionsBase(connection, schemaName, tableName, "SELECT partition_name  FROM information_schema.partitions WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 and partition_name is not null ORDER BY partition_ordinal_position", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<View> GetViews([NotNull] IDbConnection connection, [NotNull] string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            return GetViewsBase(connection, schemaName, "SELECT table_name FROM information_schema.views WHERE UPPER(table_schema) = @1", schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");
            return GetTableColumnsBase(connection, schemaName, viewName, "SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position FROM information_schema.columns WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 ORDER BY ordinal_position", schemaName.Trim().ToUpper(), viewName.Trim().ToUpper());
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<MaterializedView>();
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName, string materializedViewName, string databaseInstanceName = null)
        {
            return new List<Column>();
        }

        public override IList<Index> GetIndexes([NotNull] IDbConnection connection, [NotNull] string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT null, table_schema, table_name, index_schema,  index_name, CASE WHEN non_unique = 0 THEN 1 ELSE 0 END as is_unique FROM  information_schema.STATISTICS WHERE UPPER(index_schema) = @1 ORDER BY index_name",
                                  schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT null, table_schema, table_name, index_schema, index_name, CASE WHEN non_unique = 0 THEN 1 ELSE 0 END as is_unique FROM  information_schema.STATISTICS WHERE UPPER(table_schema) = @1 AND UPPER(table_name) = @2 ORDER BY index_name",
                                  schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns([NotNull] IDbConnection connection, string tableSchemaName, string tableName, [NotNull] string indexSchemaName, [NotNull] string indexName, object indexId = null, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (indexSchemaName == null) throw new ArgumentNullException("indexSchemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");

            // Find the table index belongs

            //string tableName = null, tableSchemaName = null;
            //using (var command = connection.CreateCommand())
            //{
            //    BuildSqlCommand(command, "select s.TABLE_SCHEMA, s.TABLE_NAME, from information_schema.STATISTICS s where UPPER(s.INDEX_NAME) = @1 and UPPER(s.INDEX_SCHEMA) = @2 LIMIT 1", new object[] { indexName.Trim().ToUpper(), indexSchemaName.Trim().ToUpper() });
            //    using (var dr = command.ExecuteReader())
            //    {
            //        if (dr.Read())
            //        {
            //            tableSchemaName = dr.GetString(0);
            //            tableName = dr.GetString(1);
            //        }
            //        else
            //        {
            //            throw new Exception("Index " + indexSchemaName + "." + indexName + " does not exist in the database");
            //        }
            //    }
            //}
            
            return  GetIndexColumnsBase(connection, indexSchemaName, indexName,
                                       "SELECT c.column_name, c.data_type, c.character_maximum_length, c.numeric_precision, c.numeric_scale, c.is_nullable, c.ordinal_position FROM information_schema.STATISTICS s INNER JOIN information_schema.COLUMNS c ON c.TABLE_SCHEMA = s.TABLE_SCHEMA AND c.TABLE_NAME = s.TABLE_NAME AND c.COLUMN_NAME = s.COLUMN_NAME WHERE UPPER(s.INDEX_NAME) = @1 and UPPER(s.INDEX_SCHEMA) = @2 AND UPPER(s.TABLE_NAME) = @3 and UPPER(s.TABLE_SCHEMA) = @4 ORDER BY seq_in_index",
                                       indexName.Trim().ToUpper(), indexSchemaName.Trim().ToUpper(), tableName.Trim().ToUpper(), tableSchemaName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexIncludedColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, string indexName, object indexId = null, string databaseInstanceName = null)
        {
            return new List<Column>();
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<Sequence>();
        }

        public override IList<Constraint> GetConstraints(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetConstraintsBase(connection, schemaName, "select TABLE_SCHEMA, CONSTRAINT_NAME, 'Y', CONSTRAINT_TYPE from information_schema.TABLE_CONSTRAINTS where UPPER(TABLE_SCHEMA) = @1 order by CONSTRAINT_NAME",
                                       schemaName.Trim().ToUpper());
            
        }

        public override IList<Constraint> GetConstraintsForTable(IDbConnection connection, string schemaName, string tableName,
            string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetConstraintsBase(connection, schemaName, "select TABLE_SCHEMA, CONSTRAINT_NAME, 'Y', CONSTRAINT_TYPE from information_schema.TABLE_CONSTRAINTS where UPPER(TABLE_SCHEMA) = @1 and UPPER(TABLE_NAME) = @2 order by CONSTRAINT_NAME",
                                       schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Trigger> GetTriggers([NotNull] IDbConnection connection, [NotNull] string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTriggersBase(connection, schemaName,
                                   "SELECT trigger_name FROM information_schema.triggers WHERE UPPER(trigger_schema) = @1 ORDER BY trigger_name",
                                   schemaName.Trim().ToUpper());
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<Synonym>();
        }

        public override IList<Synonym> GetSynonyms(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<Synonym>();
        }

        public override IList<StoredProcedure> GetStoredProcedures(IDbConnection connection, string schemaName, string databaseInstanceName = null)
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
                        if (dr.Read())
                        {
                            storedProcedure.Definition = dr.IsDBNull(2) ? null : dr.GetString(2);
                        }
                    }

                }
            }
            return procedures;
        }

        public override IList<Function> GetFunctions(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            var functions = GetFunctionsBase(connection, schemaName,
                                  "SELECT r.specific_name, r.routine_name, '' as routine_definition FROM information_schema.routines r WHERE UPPER(r.routine_schema) = @1 AND r.routine_type = 'FUNCTION' ORDER BY r.routine_name",
                                  schemaName.ToUpper());
            using (var command = connection.CreateCommand())
            {
                foreach (var storedProcedure in functions)
                {
                    command.CommandText = "SHOW CREATE FUNCTION " + storedProcedure.FullyQualifiedName;
                    using (var dr = command.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            storedProcedure.Definition = dr.IsDBNull(2) ? null : dr.GetString(2);
                        }
                    }

                }
            }
            return functions;
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
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");

            const string sql =
                "SELECT parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 'YES' as is_nullable, ordinal_position, parameter_mode  FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2 AND parameter_mode = 'IN' ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, function, sql,
                                                        function.Parent.Name.ToUpper(),
                                                        function.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                throw new Exception("Showing function parameters requires MySQL version 5.5 or greater", ex);
            }
        }

        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");

            const string sql =
                "SELECT COALESCE(parameter_name, 'OUT') as parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 'YES' as is_nullable, ordinal_position, parameter_mode  FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2 AND parameter_mode IS NULL ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, function, sql,
                                                        function.Parent.Name.ToUpper(),
                                                        function.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                throw new Exception("Showing function parameters requires MySQL version 5.5 or greater", ex);
            }
        }

        public override IList<Package> GetPackages(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            throw new NotImplementedException();
        }

        public override IList<PackageProcedure> GetPackageProcedures(IDbConnection connection, string schemaName, string packageName, string databaseInstanceName = null)
        {
            throw new NotImplementedException();
        }

        public override IList<Login> GetLogins(IDbConnection connection, string databaseInstanceName = null)
        {
            throw new NotImplementedException();
        }

        public override IntelisenseData GetIntelisenseData([NotNull] IDbConnection connection,
                                                           [NotNull] string currentSchemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (currentSchemaName == null) throw new ArgumentNullException("currentSchemaName");
            const string tablesSql =
                "SELECT c.table_schema, c.table_name, c.column_name FROM information_schema.columns c ORDER BY c.table_schema, c.table_name, c.column_name";
            return GetIntellisenseDataHelper(connection, currentSchemaName, tablesSql, null, null);
        }
    }
}
