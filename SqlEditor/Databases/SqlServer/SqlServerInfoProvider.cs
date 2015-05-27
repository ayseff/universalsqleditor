using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using log4net;

namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerInfoProvider : DbInfoProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string GET_OBJECTS_SQL =
            "SELECT t.name, o.object_id FROM sys.{0} t INNER JOIN sys.objects o ON o.object_id = t.object_id INNER JOIN sys.schemas s on o.schema_id = s.schema_id  WHERE UPPER(s.name) = @1 AND  t.is_ms_shipped = 0 ORDER BY t.name";

        private const string GET_OBJECT_COLUMNS_SQL =
            "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, c.is_nullable, c.column_id FROM sys.columns c INNER JOIN sys.{0} ob ON c.object_id = ob.object_id INNER JOIN sys.objects o ON ob.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(ob.name) = @2  AND t.name <> 'sysname' ORDER BY c.column_id";

        public override IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            try
            {
                return GetDatabaseInstancesBase(connection, "SELECT name FROM sys.databases WHERE LOWER(name) NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name");
            }
            catch
            { }

            return GetDatabaseInstancesBase(connection, "SELECT name FROM sysdatabases WHERE LOWER(name) NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name");


            //return GetDatabaseInstancesBase(connection, "SELECT	DISTINCT catalog_name FROM	information_schema.schemata WHERE LOWER(catalog_name) NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY catalog_name");
        }

        public override IList<Schema> GetSchemas(IDbConnection connection, string databaseInstance = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            UseDatabase(connection, databaseInstance);
            try
            {
                return GetSchemasBase(connection, "SELECT schema_name FROM information_schema.schemata ORDER BY schema_name");
            }
            catch
            {}
            return GetSchemasBase(connection, "SELECT name FROM sys.schemas ORDER BY name");
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetTablesBase(connection, schemaName,
                                 string.Format(GET_OBJECTS_SQL, "tables"),
                                 schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");
            
            UseDatabase(connection, databaseInstanceName);
            return GetTableColumnsBase(connection, schemaName, tableName,
                                       string.Format(GET_OBJECT_COLUMNS_SQL, "tables"),
                                       schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns([NotNull] IDbConnection connection, [NotNull] string schemaName, [NotNull] string tableName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetTableColumnsBase(connection, schemaName, tableName, 
                "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, c.is_nullable, c.column_id, t.* FROM  sys.columns c INNER JOIN sys.tables ob ON c.object_id = ob.object_id  INNER JOIN sys.objects o ON ob.object_id = o.object_id  INNER JOIN sys.schemas s ON s.schema_id = o.schema_id  INNER JOIN sys.types t ON c.system_type_id = t.system_type_id  INNER JOIN sys.index_columns ic ON c.object_id = ic.object_id and COL_NAME(ic.OBJECT_ID,ic.column_id) = c.name where UPPER(s.name) = @1 AND UPPER(ob.name) = @2 AND t.name <> 'sysname' ORDER BY c.column_id",
                                       schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetTablePartitionsBase(connection, schemaName, tableName,
                                          "SELECT DISTINCT t.name FROM sys.partitions p INNER JOIN sys.tables t ON p.object_id = t.object_id  INNER JOIN sys.objects o ON t.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = t.schema_id WHERE p.partition_number <> 1 AND o.is_ms_shipped = 0 AND UPPER(s.name) = @1 AND UPPER(t.name) = @2",
                                          schemaName.Trim().ToUpper(),
                                          tableName.Trim().ToUpper());
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetViewsBase(connection, schemaName,
                                string.Format(GET_OBJECTS_SQL, "views"), schemaName.Trim().ToUpper());
                                //"SELECT table_name FROM Information_schema.views ORDER BY table_name");
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetViewColumnsBase(connection, schemaName, viewName,
                                      string.Format(GET_OBJECT_COLUMNS_SQL, "views"),
                                      //"SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position FROM information_schema.columns WHERE UPPER(table_name) = @1 ORDER BY ordinal_position",
                                      schemaName.Trim().ToUpper(),
                                      viewName.Trim().ToUpper());
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetMaterializedViewsBase(connection, schemaName,
                                            "SELECT o.name FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id WHERE o.xtype = 'V' AND o.is_ms_shipped = 0 ORDER BY o.name");
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName, string materializedViewName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (materializedViewName == null) throw new ArgumentNullException("materializedViewName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetMaterializedViewColumnsBase(connection, schemaName, materializedViewName,
                                                  "SELECT c.name, c.max_length, c.precision as numeric_precision, c.scale, c.is_nullable, column_id FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id INNER JOIN sys.columns c ON c.object_id = o.id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id WHERE o.xtype = 'V' AND o.is_ms_shipped = 0 AND UPPER(o.name) = @1 ORDER BY column_id",
                                                  materializedViewName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexes(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetIndexesBase(connection, schemaName,
                                  "SELECT DISTINCT '" + databaseInstanceName + "', s.name, o.name,  s.name, a.name, a.is_unique, a.index_id FROM sys.indexes a INNER JOIN sys.objects o ON a.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id WHERE o.is_ms_shipped = 0 AND a.name IS NOT NULL AND UPPER(s.name) = @1 ORDER BY a.name",
                                  schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetIndexesBase(connection, schemaName,
                                  "SELECT DISTINCT '" + databaseInstanceName + "', s.name, o.name,  s.name, i.name, i.is_unique, i.index_id FROM sys.indexes AS i INNER JOIN sys.data_spaces AS ds ON i.data_space_id = ds.data_space_id 	INNER JOIN sys.objects AS o ON o.object_id = i.object_id 	INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id WHERE is_hypothetical = 0 AND i.index_id <> 0 AND UPPER(s.name) = @1 AND UPPER(o.name) = UPPER(@2) ORDER BY i.name", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns([NotNull] IDbConnection connection, string tableSchemaName, string tableName, [NotNull] string indexSchemaName, [NotNull] string indexName, [JetBrains.Annotations.NotNull] object indexId = null, [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (indexSchemaName == null) throw new ArgumentNullException("indexSchemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            if (indexId == null) throw new ArgumentNullException("indexId");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            var indexIdInt = (int) indexId;
            UseDatabase(connection, databaseInstanceName);
            return GetIndexColumnsBase(connection, indexSchemaName, indexName,
                                        "SELECT col.name AS column_name, t.name AS data_type, col.max_length, col.precision, col.scale, col.is_nullable, col.column_id FROM 	sys.indexes ind 	INNER JOIN sys.index_columns ic 		ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 	INNER JOIN sys.columns col 		ON ic.object_id = col.object_id and ic.column_id = col.column_id 	 	INNER JOIN sys.objects o ON ind.object_id = o.object_id 	INNER JOIN sys.schemas s ON s.schema_id = o.schema_id 	INNER JOIN sys.types t ON col.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(ind.name) = @2  AND t.name <> 'sysname' AND ind.index_id = @3 AND ic.is_included_column = 0 ORDER BY ic.index_column_id",
                                       indexSchemaName.Trim().ToUpper(),
                                       indexName.Trim().ToUpper(),
                                       indexIdInt);
        }

        public override IList<Column> GetIndexIncludedColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, string indexName, object indexId = null, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (indexSchemaName == null) throw new ArgumentNullException("indexSchemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            if (indexId == null) throw new ArgumentNullException("indexId");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            var indexIdInt = (int)indexId;
            UseDatabase(connection, databaseInstanceName);
            return GetIndexColumnsBase(connection, indexSchemaName, indexName,
                                        "SELECT col.name AS column_name, t.name AS data_type, col.max_length, col.precision, col.scale, col.is_nullable, col.column_id FROM 	sys.indexes ind 	INNER JOIN sys.index_columns ic 		ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id 	INNER JOIN sys.columns col 		ON ic.object_id = col.object_id and ic.column_id = col.column_id 	 	INNER JOIN sys.objects o ON ind.object_id = o.object_id 	INNER JOIN sys.schemas s ON s.schema_id = o.schema_id 	INNER JOIN sys.types t ON col.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(ind.name) = @2  AND t.name <> 'sysname' AND ind.index_id = @3 AND ic.is_included_column = 1 ORDER BY ic.index_column_id",
                                       indexSchemaName.Trim().ToUpper(),
                                       indexName.Trim().ToUpper(),
                                       indexIdInt);
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetSequencesBase(connection, schemaName,
                                    "SELECT iss.sequence_name, iss.minimum_value, iss.maximum_value, iss.increment, sq.current_value FROM INFORMATION_SCHEMA.SEQUENCES iss INNER JOIN sys.sequences sq ON iss.SEQUENCE_NAME = sq.name WHERE UPPER(SEQUENCE_CATALOG) = @1 ORDER BY iss.sequence_name",
                                    schemaName.Trim().ToUpper());
        }

        public override IList<Constraint> GetConstraints(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetConstraintsBase(connection, schemaName,
                                    "SELECT CONSTRAINT_SCHEMA, CONSTRAINT_NAME, CASE INITIALLY_DEFERRED WHEN 'NO' THEN 'Y' ELSE 'N' END, CONSTRAINT_TYPE FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE UPPER(CONSTRAINT_CATALOG) = @1 AND UPPER(CONSTRAINT_SCHEMA) = @2 ORDER BY CONSTRAINT_NAME",
                                    databaseInstanceName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Constraint> GetConstraintsForTable(
            [JetBrains.Annotations.NotNull] IDbConnection connection, [JetBrains.Annotations.NotNull] string schemaName,
            [JetBrains.Annotations.NotNull] string tableName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");
            UseDatabase(connection, databaseInstanceName);
            return GetConstraintsBase(connection, schemaName,
                                    "SELECT CONSTRAINT_SCHEMA, CONSTRAINT_NAME, CASE INITIALLY_DEFERRED WHEN 'NO' THEN 'Y' ELSE 'N' END, CONSTRAINT_TYPE FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE UPPER(CONSTRAINT_CATALOG) = @1 AND UPPER(CONSTRAINT_SCHEMA) = @2 AND UPPER(TABLE_NAME) = @3 ORDER BY CONSTRAINT_NAME",
                                    databaseInstanceName.Trim().ToUpper(), schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetTriggersBase(connection, schemaName, "SELECT t.* FROM sys.triggers t INNER JOIN sys.objects o ON t.object_id = o.object_id INNER JOIN sys.objects o1 ON o1.object_id = t.parent_id INNER JOIN sys.schemas s ON s.schema_id = o1.schema_id  WHERE UPPER(s.NAME) = @1 ORDER BY t.name", schemaName.ToUpper());
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            throw new NotSupportedException();
        }

        public override IList<Synonym> GetSynonyms(IDbConnection connection, string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            return GetSynonymsBase(connection, schemaName,
                                   "SELECT t.name, t.base_object_name FROM sys.synonyms t INNER JOIN sys.objects o ON t.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = t.schema_id WHERE UPPER(s.NAME) = @1 ORDER BY t.name", schemaName.ToUpper());
        }

        public override IList<StoredProcedure> GetStoredProcedures([NotNull] IDbConnection connection, [NotNull] string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            const string sql2 =
                "SELECT specific_name, routine_name, routine_definition FROM information_schema.routines WHERE routine_type = 'PROCEDURE' AND UPPER(routine_schema) = @1 ORDER BY routine_name";
            try
            {
                return GetStoredProceduresBase(connection, schemaName, sql2, schemaName.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting stored procedures using INFORMATION_SCHEMA. Retry using sys catalog will be made.");
                _log.Error(ex.Message);
            }

            const string sql =
                "SELECT p.object_id, p.name, '' as definition FROM sys.procedures p INNER JOIN sys.schemas s ON s.schema_id = p.schema_id WHERE p.is_ms_shipped = 0 AND p.type = 'P' AND UPPER(s.name) = @1";
            var procedures = GetStoredProceduresBase(connection, schemaName, sql, schemaName.ToUpper());
            using (var command = connection.CreateCommand())
            {
                var sb = new StringBuilder();
                foreach (var storedProcedure in procedures)
                {
                    command.CommandText = string.Format("sp_helptext '{0}'; ", storedProcedure.FullyQualifiedName);
                    using (var dr = command.ExecuteReader())
                    {
                        sb.Clear();
                        while (dr != null && dr.Read())
                        {
                            if (sb.Length != 0)
                            {
                                sb.Append(Environment.NewLine);
                            }
                            sb.Append(dr.GetString(0));
                        }
                        storedProcedure.Definition = sb.ToString();
                    }
                }
            }
            return procedures;
        }

        public override IList<Function> GetFunctions([JetBrains.Annotations.NotNull] IDbConnection connection,
            [JetBrains.Annotations.NotNull] string schemaName,
            [JetBrains.Annotations.NotNull] string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (databaseInstanceName == null) throw new ArgumentNullException("databaseInstanceName");

            UseDatabase(connection, databaseInstanceName);
            const string sql2 =
                "SELECT specific_name, routine_name, routine_definition FROM information_schema.routines WHERE routine_type = 'FUNCTION' AND UPPER(routine_schema) = @1 ORDER BY routine_name";
            try
            {
                return GetFunctionsBase(connection, schemaName, sql2, schemaName.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting function using INFORMATION_SCHEMA. Retry using sys catalog will be made.");
                _log.Error(ex.Message);
            }

            const string sql =
                "SELECT m.object_id, o.name, m.definition FROM sys.sql_modules AS m INNER JOIN sys.objects AS o ON m.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id WHERE type IN ('FN', 'IF', 'TF') AND UPPER(s.name) = @1 ORDER BY o.name";
            return GetFunctionsBase(connection, schemaName, sql, schemaName.ToUpper());
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

            var databaseInstanceName = storedProcedure.Parent.Parent == null ? null : storedProcedure.Parent.Parent.Name;
            UseDatabase(connection, databaseInstanceName);
            const string sql = "SELECT parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 1 as is_nullable, ordinal_position, parameter_mode FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2 ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, storedProcedure, sql,
                                                        storedProcedure.Parent.Name.ToUpper(),
                                                        storedProcedure.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting stored procedure parameters using INFORMATION_SCHEMA. Retry using sys catalog will be made.");
                _log.Error(ex.Message);
            }

            const string sql2 =
                "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, 1 as is_nullable, c.parameter_id, CASE WHEN is_output = 0 THEN 'IN' ELSE 'OUT' END as parameter_direction FROM sys.parameters c INNER JOIN sys.procedures ob ON c.object_id = ob.object_id INNER JOIN sys.objects o ON ob.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(ob.name) = @2  AND t.name <> 'sysname' ORDER BY c.parameter_id";
            return GetStoredProcedureParametersBase(connection, storedProcedure, sql2,
                                                        storedProcedure.Parent.Name.ToUpper(),
                                                        storedProcedure.Name.ToUpper());

        }

        public override IList<ColumnParameter> GetFunctionParameters(
            [JetBrains.Annotations.NotNull] IDbConnection connection, [JetBrains.Annotations.NotNull] Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");

            var databaseInstanceName = function.Parent.Parent == null ? null : function.Parent.Parent.Name;
            UseDatabase(connection, databaseInstanceName);
            const string sql = "SELECT parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 1 as is_nullable, ordinal_position, parameter_mode FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2  AND parameter_mode = 'IN' ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, function, sql,
                                                        function.Parent.Name.ToUpper(),
                                                        function.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting function parameters using INFORMATION_SCHEMA. Retry using sys catalog will be made.");
                _log.Error(ex.Message);
            }

            const string sql2 =
                "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, 1 as is_nullable, c.parameter_id, CASE WHEN is_output = 0 THEN 'IN' ELSE 'OUT' END as parameter_direction FROM sys.parameters c INNER JOIN sys.sql_modules ob ON c.object_id = ob.object_id INNER JOIN sys.objects o ON ob.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id WHERE UPPER(s.name) = @1 AND UPPER(o.name) = @2  AND t.name <> 'sysname' AND  is_output = 0 ORDER BY c.parameter_id";
            return GetStoredProcedureParametersBase(connection, function, sql2,
                                                        function.Parent.Name.ToUpper(),
                                                        function.Name.ToUpper());
        }

        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");

            var databaseInstanceName = function.Parent.Parent == null ? null : function.Parent.Parent.Name;
            UseDatabase(connection, databaseInstanceName);
            const string sql = "SELECT parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 1 as is_nullable, ordinal_position, parameter_mode FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2 AND parameter_mode = 'OUT' ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, function, sql,
                                                        function.Parent.Name.ToUpper(),
                                                        function.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting function return value using INFORMATION_SCHEMA. Retry using sys catalog will be made.");
                _log.Error(ex.Message);
            }

            const string sql2 =
                "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, 1 as is_nullable, c.parameter_id, CASE WHEN is_output = 0 THEN 'IN' ELSE 'OUT' END as parameter_direction FROM sys.parameters c INNER JOIN sys.sql_modules ob ON c.object_id = ob.object_id INNER JOIN sys.objects o ON ob.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id WHERE UPPER(s.name) = @1 AND UPPER(o.name) = @2  AND t.name <> 'sysname' AND  is_output = 0 ORDER BY c.parameter_id";
            return GetStoredProcedureParametersBase(connection, function, sql2,
                                                        function.Parent.Name.ToUpper(),
                                                        function.Name.ToUpper());
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

        public override IntelisenseData GetIntelisenseData(IDbConnection connection, string currentSchemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (currentSchemaName == null) throw new ArgumentNullException("currentSchemaName");

            const string tablesSql =
                "SELECT s.name, t.name, c.name FROM sys.columns c INNER JOIN sys.tables t ON c.object_id = t.object_id INNER JOIN sys.objects o ON t.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id order by s.name, t.name, c.name";
                //"SELECT c.table_catalog, c.table_name, c.column_name FROM information_schema.columns c INNER JOIN information_schema.tables t on (t.table_catalog = c.table_catalog AND t.table_name = c.table_name AND t.TABLE_SCHEMA = c.TABLE_SCHEMA) ORDER BY t.TABLE_CATALOG, t.TABLE_NAME, c.COLUMN_NAME";
            const string viewsSql =
                "SELECT s.name, t.name, c.name FROM sys.columns c INNER JOIN sys.views t ON c.object_id = t.object_id INNER JOIN sys.objects o ON t.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id order by s.name, t.name, c.name";

            return GetIntellisenseDataHelper(connection, currentSchemaName, tablesSql, viewsSql, null);
        }
        
        private static void UseDatabase(IDbConnection connection, string databaseInstanceName)
        {
            if (databaseInstanceName == null) return;

            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT DB_NAME() AS DataBaseName";
            var currentDatabaseName = (string)command.ExecuteScalar();
            if (!String.Equals(currentDatabaseName, databaseInstanceName.Trim(), StringComparison.CurrentCultureIgnoreCase))
            {
                command.CommandText = "USE " + databaseInstanceName;
                command.ExecuteNonQuery();
            }
        }
    }
}