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
            "SELECT t.name FROM sys.{0} t INNER JOIN sys.objects o ON o.object_id = t.object_id INNER JOIN sys.schemas s on o.schema_id = s.schema_id  WHERE UPPER(s.name) = @1 AND  t.is_ms_shipped = 0 ORDER BY t.name";

        private const string GET_OBJECT_COLUMNS_SQL =
            "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, c.is_nullable, c.column_id FROM sys.columns c INNER JOIN sys.{0} ob ON c.object_id = ob.object_id INNER JOIN sys.objects o ON ob.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(ob.name) = @2  AND t.name <> 'sysname' ORDER BY c.column_id";
        
        public override IList<Schema> GetSchemas(IDbConnection connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return GetSchemasBase(connection,
                                      //"SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') ORDER BY name");
                                      "SELECT name FROM sys.schemas ORDER BY name");
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTablesBase(connection, schemaName,
                                 //"SELECT name FROM sys.tables WHERE is_ms_shipped = 0 ORDER BY name");
                                 string.Format(GET_OBJECTS_SQL, "tables"),
                                 //"SELECT t.name FROM sys.tables t INNER JOIN sys.objects o ON o.object_id = t.object_id INNER JOIN sys.schemas s on o.schema_id = s.schema_id  WHERE UPPER(s.name) = @1 AND  t.is_ms_shipped = 0 ORDER BY t.name",
                                 schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTableColumnsBase(connection, schemaName, tableName,
                                       //"SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position FROM information_schema.columns WHERE UPPER(table_name) = @1 ORDER BY ordinal_position",
                                       // "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, c.is_nullable, c.column_id FROM sys.columns c INNER JOIN sys.objects o ON c.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(o.name) = @2 ORDER BY c.column_id",
                                       string.Format(GET_OBJECT_COLUMNS_SQL, "tables"),
                                       schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns([NotNull] IDbConnection connection,
                                                                [NotNull] string schemaName, [NotNull] string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName, 
                "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, c.is_nullable, c.column_id, t.* FROM  sys.columns c INNER JOIN sys.tables ob ON c.object_id = ob.object_id  INNER JOIN sys.objects o ON ob.object_id = o.object_id  INNER JOIN sys.schemas s ON s.schema_id = o.schema_id  INNER JOIN sys.types t ON c.system_type_id = t.system_type_id  INNER JOIN sys.index_columns ic ON c.object_id = ic.object_id and COL_NAME(ic.OBJECT_ID,ic.column_id) = c.name where UPPER(s.name) = @1 AND UPPER(ob.name) = @2 AND t.name <> 'sysname' ORDER BY c.column_id",
                                       schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTablePartitionsBase(connection, schemaName, tableName,
                                          "SELECT DISTINCT t.name FROM sys.partitions p INNER JOIN sys.tables t ON p.object_id = t.object_id  INNER JOIN sys.objects o ON t.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_is = t.schema_id WHERE p.partition_number <> 1 AND o.is_ms_shipped = 0 AND UPPER(s.name) = @1 AND UPPER(t.name) = @2",
                                          schemaName.Trim().ToUpper(),
                                          tableName.Trim().ToUpper());
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            return GetViewsBase(connection, schemaName,
                                string.Format(GET_OBJECTS_SQL, "views"), schemaName.Trim().ToUpper());
                                //"SELECT table_name FROM Information_schema.views ORDER BY table_name");
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");

            return GetViewColumnsBase(connection, schemaName, viewName,
                                      string.Format(GET_OBJECT_COLUMNS_SQL, "views"),
                                      //"SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, is_nullable, ordinal_position FROM information_schema.columns WHERE UPPER(table_name) = @1 ORDER BY ordinal_position",
                                      schemaName.Trim().ToUpper(),
                                      viewName.Trim().ToUpper());
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetMaterializedViewsBase(connection, schemaName,
                                            "SELECT o.name FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id WHERE o.xtype = 'V' AND o.is_ms_shipped = 0 ORDER BY o.name");
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName, string materializedViewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (materializedViewName == null) throw new ArgumentNullException("materializedViewName");
            
            return GetMaterializedViewColumnsBase(connection, schemaName, materializedViewName,
                                                  "SELECT c.name, c.max_length, c.precision as numeric_precision, c.scale, c.is_nullable, column_id FROM sysobjects o INNER JOIN sysindexes i ON o.id = i.id INNER JOIN sys.columns c ON c.object_id = o.id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id WHERE o.xtype = 'V' AND o.is_ms_shipped = 0 AND UPPER(o.name) = @1 ORDER BY column_id",
                                                  materializedViewName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexes(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT DISTINCT '" + schemaName + "' as schema_name, a.name, a.is_unique FROM sys.indexes a INNER JOIN sys.objects o ON a.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id WHERE o.is_ms_shipped = 0 AND a.name IS NOT NULL AND UPPER(s.name) = @1 ORDER BY a.name",
                                  schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT DISTINCT s.name, i.name, i.is_unique FROM sys.indexes AS i INNER JOIN sys.data_spaces AS ds ON i.data_space_id = ds.data_space_id 	INNER JOIN sys.objects AS o ON o.object_id = i.object_id 	INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id WHERE is_hypothetical = 0 AND i.index_id <> 0 AND UPPER(s.name) = @1 AND UPPER(o.name) = UPPER(@2) ORDER BY i.name", schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                      [NotNull] string indexName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            return GetIndexColumnsBase(connection, schemaName, indexName,
                                        string.Format(GET_OBJECT_COLUMNS_SQL, "indexes"),
                                       //"SELECT column_name, data_type, character_maximum_length, numeric_precision, numeric_scale, c.is_nullable, ordinal_position from sys.objects t inner join sys.schemas s on t.schema_id = s.schema_id inner join sys.indexes i on i.object_id = t.object_id inner join sys.index_columns ic on ic.object_id = t.object_id inner join sys.columns c on c.object_id = t.object_id and ic.column_id = c.column_id INNER JOIN INFORMATION_SCHEMA.COLUMNS cl ON cl.TABLE_NAME = t.name AND cl.COLUMN_NAME = c.name where i.index_id > 0    and i.is_disabled = 0 and i.is_hypothetical = 0 and ic.key_ordinal > 0 AND UPPER(i.name) = @1",
                                       schemaName.Trim().ToUpper(),
                                       indexName.Trim().ToUpper());
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSequencesBase(connection, schemaName,
                                    "SELECT iss.sequence_name, iss.minimum_value, iss.maximum_value, iss.increment, sq.current_value FROM INFORMATION_SCHEMA.SEQUENCES iss INNER JOIN sys.sequences sq ON iss.SEQUENCE_NAME = sq.name WHERE UPPER(SEQUENCE_CATALOG) = @1 ORDER BY iss.sequence_name",
                                    schemaName.Trim().ToUpper());
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTriggersBase(connection, schemaName, "SELECT name FROM sys.triggers ORDER BY name");
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName)
        {
            return new List<Synonym>();
        }

        public override IList<Synonym> GetSynonyms(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            return GetSynonymsBase(connection, schemaName,
                                   "SELECT name FROM sys.synonyms ORDER BY name");
        }

        public override IList<StoredProcedure> GetStoredProcedures([NotNull] IDbConnection connection,
                                                                   [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            
            const string sql2 =
                "SELECT specific_name, routine_name, routine_definition FROM information_schema.routines WHERE routine_type = 'PROCEDURE' AND UPPER(routine_schema) = @1 ORDER BY routine_name";
            try
            {
                return GetStoredProceduresBase(connection, schemaName, sql2, schemaName.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting stored procedures using INFORMATION_SCHEMA. Retry using sys catalog with be made.");
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

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

            const string sql = "SELECT parameter_name, data_type, character_maximum_length, numeric_precision, numeric_scale, 1 as is_nullable, ordinal_position, parameter_mode FROM information_schema.parameters WHERE UPPER(specific_schema) = @1 AND UPPER(specific_name) = @2 ORDER BY ordinal_position";
            try
            {
                return GetStoredProcedureParametersBase(connection, storedProcedure, sql,
                                                        storedProcedure.Parent.Name.ToUpper(),
                                                        storedProcedure.ObjectId.ToUpper());
            }
            catch (Exception ex)
            {
                _log.Error("Error getting stored procedure parameters using INFORMATION_SCHEMA. Retry using sys catalog with be made.");
                _log.Error(ex.Message);
            }

            const string sql2 =
                "SELECT c.name AS column_name, t.name AS data_type, c.max_length, c.precision, c.scale, 1 as is_nullable, c.parameter_id, CASE WHEN is_output = 0 THEN 'IN' ELSE 'OUT' END as parameter_direction FROM sys.parameters c INNER JOIN sys.procedures ob ON c.object_id = ob.object_id INNER JOIN sys.objects o ON ob.object_id = o.object_id INNER JOIN sys.schemas s ON s.schema_id = o.schema_id INNER JOIN sys.types t ON c.system_type_id = t.system_type_id where UPPER(s.name) = @1 AND UPPER(ob.name) = @2  AND t.name <> 'sysname' ORDER BY c.parameter_id";
            return GetStoredProcedureParametersBase(connection, storedProcedure, sql2,
                                                        storedProcedure.Parent.Name.ToUpper(),
                                                        storedProcedure.Name.ToUpper());

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
        
        private static void UseDatabase(IDbConnection connection, string schemaName)
        {
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT DB_NAME() AS DataBaseName";
            var currentDatabaseName = (string)command.ExecuteScalar();
            if (currentDatabaseName.ToUpper() != schemaName.Trim().ToUpper())
            {
                command.CommandText = "USE " + schemaName;
                command.ExecuteNonQuery();
            }
        }
    }
}