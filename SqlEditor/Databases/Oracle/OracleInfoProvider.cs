using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using log4net;

namespace SqlEditor.Databases.Oracle
{
    public class OracleInfoProvider : DbInfoProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection)
        {
            throw new NotSupportedException();
        }

        public override IList<Schema> GetSchemas(IDbConnection connection, DatabaseInstance databaseInstance = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return GetSchemasBase(connection, "SELECT username FROM all_users ORDER BY username");
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTablesBase(connection, schemaName,
                                 "SELECT table_name FROM all_tables WHERE UPPER(owner) = :1 ORDER BY table_name",
                                 schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName,
                                       "SELECT column_name, data_type, data_length, data_precision, data_scale, nullable, column_id FROM all_tab_columns WHERE UPPER(table_name) = :1 AND UPPER(owner) = :2 ORDER BY column_id",
                                       tableName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTableColumnsBase(connection, schemaName, tableName,
                                       "SELECT atc.column_name, atc.data_type, atc.data_length, atc.data_precision, atc.data_scale, atc.nullable, atc.column_id  FROM all_tab_columns atc INNER JOIN all_cons_columns acc ON acc.column_name = atc.column_name AND acc.owner = atc.owner AND acc.table_name = atc.table_name INNER JOIN all_constraints ac ON ac.constraint_name = acc.constraint_name WHERE UPPER(atc.table_name) = :1 AND UPPER(atc.owner) = :2 AND ac.constraint_type = 'P' ORDER BY atc.column_id",
                                       tableName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions([NotNull] IDbConnection connection,
                                                            [NotNull] string schemaName, [NotNull] string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetTablePartitionsBase(connection, schemaName, tableName,
                                          "select PARTITION_NAME from ALL_TAB_PARTITIONS where UPPER(TABLE_OWNER) = @1 AND UPPER(TABLE_NAME) = @2 ORDER BY PARTITION_POSITION",
                                          schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName)
        {
            return GetViewsBase(connection, schemaName,
                                "SELECT VIEW_NAME FROM ALL_VIEWS WHERE UPPER(owner) = :1 ORDER BY VIEW_NAME",
                                schemaName.Trim().ToUpper());

            //var schema = new Schema(schemaName);
            //var views = new List<View>();
            //var command = connection.CreateCommand();
            //command.CommandText = "SELECT VIEW_NAME FROM ALL_VIEWS WHERE UPPER(owner) = :1 ORDER BY VIEW_NAME";
            //command.Parameters.Add(new OracleParameter(":1", schemaName.Trim().ToUpper()));
            //using (var dr = command.ExecuteReader())
            //{
            //    while (dr.Read())
            //    {
            //        var view = new View(dr.GetString(0).ToUpper(), schema);
            //        views.Add(view);
            //    }
            //}
            //return views;
        }

        public override IList<Column> GetViewColumns([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                     [NotNull] string viewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");

            return GetViewColumnsBase(connection, schemaName, viewName,
                                      "SELECT column_name, c.DATA_TYPE, c.DATA_LENGTH, C.DATA_PRECISION, C.DATA_SCALE, C.NULLABLE, C.COLUMN_ID FROM cols c, all_views o WHERE c.TABLE_NAME = o.view_name AND UPPER(o.owner) = :1 AND UPPER(c.TABLE_NAME) = :2 ORDER BY c.COLUMN_ID",
                                      schemaName.Trim().ToUpper(), viewName.Trim().ToUpper());
            //var schema = new Schema(schemaName);
            //var view = new View(viewName, schema);
            //var views = new List<Column>();
            //var command = connection.CreateCommand();
            //command.CommandText =
            //    "SELECT column_name, c.DATA_TYPE, c.DATA_LENGTH, C.DATA_PRECISION, C.NULLABLE, C.COLUMN_ID FROM cols c, all_views o WHERE c.TABLE_NAME = o.view_name AND UPPER(o.owner) = :1 AND UPPER(c.TABLE_NAME) = :2 ORDER BY c.COLUMN_ID";
            //command.Parameters.Add(new OracleParameter(":1", schemaName.Trim().ToUpper()));
            //command.Parameters.Add(new OracleParameter(":2", viewName.Trim().ToUpper()));
            //using (var dr = command.ExecuteReader())
            //{
            //    while (dr.Read())
            //    {
            //        var columnName = dr.GetString(0).Trim().ToUpper();
            //        var col = new Column(columnName, view);
            //        col.Name = columnName;
            //        col.DataType = dr.GetString(1);
            //        col.CharacterLength = (int)dr.GetDecimal(2);
            //        col.DataPrecision = 0;
            //        if (!dr.IsDBNull(3))
            //        {
            //            col.DataPrecision = (int)dr.GetDecimal(3);
            //        }
            //        col.Nullable = true;
            //        if (!dr.IsDBNull(4))
            //        {
            //            string value = dr.GetString(4);
            //            col.Nullable = value.ToUpper().Trim() == "Y";
            //        }
            //        views.Add(col);
            //        view.Columns.Add(col);
            //    }
            //}
            //return views;
        }

        public override IList<MaterializedView> GetMaterializedViews([NotNull] IDbConnection connection,
                                                                     [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetMaterializedViewsBase(connection, schemaName,
                                            "SELECT mview_name FROM all_mviews WHERE UPPER(owner) = @1 ORDER by mview_name",
                                            schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName,
                                                                 string materializedViewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (materializedViewName == null) throw new ArgumentNullException("materializedViewName");

            return GetViewColumnsBase(connection, schemaName, materializedViewName,
                                      "SELECT column_name, c.DATA_TYPE, c.DATA_LENGTH, C.DATA_PRECISION, C.DATA_SCALE, C.NULLABLE, C.COLUMN_ID FROM cols c, all_mviews o WHERE c.TABLE_NAME = o.view_name AND UPPER(o.owner) = :1 AND UPPER(c.TABLE_NAME) = :2 ORDER BY c.COLUMN_ID",
                                      schemaName.Trim().ToUpper(), materializedViewName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexes([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT OWNER, INDEX_NAME, CASE WHEN uniqueness = 'UNIQUE' THEN 1 ELSE 0 END AS is_unique FROM ALL_INDEXES WHERE UPPER(owner) = @1 ORDER BY INDEX_NAME",
                                  schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexesForTable([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                        [NotNull] string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT OWNER, INDEX_NAME, CASE WHEN uniqueness = 'UNIQUE' THEN 1 ELSE 0 END AS is_unique FROM ALL_INDEXES WHERE UPPER(owner) = @1 AND UPPER(TABLE_NAME) = @2 ORDER BY INDEX_NAME",
                                  schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns([NotNull] IDbConnection connection, [NotNull] string schemaName,
                                                      [NotNull] string indexName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            return GetIndexColumnsBase(connection, schemaName, indexName,
                                       "SELECT atc.column_name, atc.data_type, atc.data_length, atc.data_precision, atc.data_scale, atc.nullable, atc.column_id FROM all_tab_columns atc INNER JOIN all_ind_columns aic ON aic.table_owner = atc.owner AND aic.table_name = atc.table_name AND aic.column_name = atc.column_name WHERE UPPER(atc.owner) = @1 AND UPPER(aic.index_name) = @2 ORDER BY aic.column_position",
                                       schemaName.Trim().ToUpper(), indexName.Trim().ToUpper());
        }

        public override IList<Sequence> GetSequences([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSequencesBase(connection, schemaName,
                                    "SELECT SEQUENCE_NAME, MIN_VALUE, MAX_VALUE, INCREMENT_BY, LAST_NUMBER FROM ALL_SEQUENCES WHERE UPPER(SEQUENCE_OWNER) = @1 ORDER BY SEQUENCE_NAME",
                                    schemaName.Trim().ToUpper());
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTriggersBase(connection, schemaName,
                                   "SELECT TRIGGER_NAME FROM ALL_TRIGGERS WHERE UPPER(OWNER) = @1 ORDER BY TRIGGER_NAME",
                                   schemaName.Trim().ToUpper());
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSynonymsBase(connection, schemaName,
                                   "SELECT synonym_name, table_name FROM all_synonyms WHERE UPPER(owner) = 'PUBLIC' ORDER BY synonym_name",
                                   schemaName.Trim().ToUpper());
        }

        public override IList<Synonym> GetSynonyms([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSynonymsBase(connection, schemaName,
                                   "SELECT synonym_name, table_name FROM user_synonyms ORDER BY synonym_name");
        }

        public override IList<StoredProcedure> GetStoredProcedures(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            var procs = GetStoredProceduresBase(connection, schemaName,
                                  "SELECT p.object_id, p.object_name, s.text FROM all_procedures p INNER JOIN all_source s ON s.owner = p.owner AND s.name = p.object_name WHERE p.object_type ='PROCEDURE' AND s.type ='PROCEDURE' AND UPPER(p.owner) = @1 ORDER BY p.object_id, p.OBJECT_NAME, s.line",
                                  schemaName.ToUpper());

            var groupedProcedures = procs.GroupBy(x => x.Name);
            return groupedProcedures.Select(grouping => new StoredProcedure(grouping.Key, grouping.First().Parent)
                                                            {
                                                                ObjectId = grouping.First().ObjectId,
                                                                Definition = "CREATE " + string.Join(string.Empty, grouping.Select(x => x.Definition))
                                                            }).ToList();
        }

        public override IList<Function> GetFunctions(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            var procs = GetStoredProceduresBase(connection, schemaName,
                                  "SELECT p.object_id, p.object_name, s.text FROM all_procedures p INNER JOIN all_source s ON s.owner = p.owner AND s.name = p.object_name WHERE p.object_type ='FUNCTION' AND s.type ='FUNCTION' AND UPPER(p.owner) = @1 ORDER BY p.object_id, p.OBJECT_NAME, s.line",
                                  schemaName.ToUpper());

            var groupedProcedures = procs.GroupBy(x => x.Name);
            return groupedProcedures.Select(grouping => new Function(grouping.Key, grouping.First().Parent)
                                                            {
                                                                ObjectId = grouping.First().ObjectId,
                                                                Definition = "CREATE " + string.Join(string.Empty, grouping.Select(x => x.Definition))
                                                            }).ToList();
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters([NotNull] IDbConnection connection,
                                                                            [NotNull] StoredProcedure storedProcedure)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");
            const string sql = "SELECT argument_name, data_type, data_length, data_precision, data_scale, 'Y' as nullable, position, in_out FROM all_arguments WHERE UPPER(owner) = :1 AND UPPER(object_name) = :2 AND OBJECT_ID = :3 ORDER BY position";
            return GetStoredProcedureParametersBase(connection, storedProcedure, sql,
                                                    storedProcedure.Parent.Name.ToUpper(),
                                                    storedProcedure.Name.ToUpper(), int.Parse(storedProcedure.ObjectId));
        }

        public override IList<ColumnParameter> GetFunctionParameters(IDbConnection connection, Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");
            const string sql = "SELECT argument_name, data_type, data_length, data_precision, data_scale, 'Y' as nullable, position, in_out FROM all_arguments WHERE UPPER(owner) = :1 AND UPPER(object_name) = :2 AND OBJECT_ID = :3 AND in_out = 'IN' ORDER BY position";
            return GetStoredProcedureParametersBase(connection, function, sql,
                                                    function.Parent.Name.ToUpper(),
                                                    function.Name.ToUpper(), int.Parse(function.ObjectId));
        }

        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");
            const string sql = "SELECT NVL(argument_name, ' ') AS argument_name, data_type, data_length, data_precision, data_scale, 'Y' as nullable, position, in_out FROM all_arguments WHERE UPPER(owner) = :1 AND UPPER(object_name) = :2 AND OBJECT_ID = :3 AND in_out = 'OUT' ORDER BY position";
            return GetStoredProcedureParametersBase(connection, function, sql,
                                                    function.Parent.Name.ToUpper(),
                                                    function.Name.ToUpper(), int.Parse(function.ObjectId));
        }

        public override IntelisenseData GetIntelisenseData(IDbConnection connection, string currentSchemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (currentSchemaName == null) throw new ArgumentNullException("currentSchemaName");

            const string tablesSql =
                "select owner, table_name as object_name, column_name from all_tab_columns where owner not in ('XS$NULL', 'APEX_040000', 'APEX_PUBLIC_USER', 'FLOWS_FILES', 'MDSYS', 'ANONYMOUS', 'XDB', 'CTXSYS', 'OUTLN', 'SYSTEM', 'SYS') order by owner, table_name, column_name";
            const string viewsSql =
                "SELECT o.owner, o.view_name as object_name, column_name FROM cols c, all_views o WHERE c.TABLE_NAME = o.view_name and owner not in ('XS$NULL', 'APEX_040000', 'APEX_PUBLIC_USER', 'FLOWS_FILES', 'MDSYS', 'ANONYMOUS', 'XDB', 'CTXSYS', 'OUTLN', 'SYSTEM', 'SYS') order by owner, view_name, column_name";
            const string mviewsSql =
                "SELECT o.owner, o.mview_name as object_name, column_name FROM cols c, all_mviews o WHERE c.TABLE_NAME = o.mview_name and owner not in ('XS$NULL', 'APEX_040000', 'APEX_PUBLIC_USER', 'FLOWS_FILES', 'MDSYS', 'ANONYMOUS', 'XDB', 'CTXSYS', 'OUTLN', 'SYSTEM', 'SYS')  order by owner, mview_name, column_name";

            return GetIntellisenseDataHelper(connection, currentSchemaName, tablesSql, viewsSql, mviewsSql);
        }
    }
}