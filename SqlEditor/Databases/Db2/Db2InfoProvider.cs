﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using log4net;

namespace SqlEditor.Databases.Db2
{
    public class Db2InfoProvider : DbInfoProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection)
        {
            throw new NotSupportedException();
        }

        public override IList<Schema> GetSchemas(IDbConnection connection, DatabaseInstance databaseInstance = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            return GetSchemasBase(connection, "SELECT schemaname FROM syscat.schemata ORDER BY schemaname WITH ur");
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTablesBase(connection, schemaName,
                                 "SELECT tabname FROM syscat.tables WHERE TRIM(UPPER(tabschema)) = @1 AND type IN ('G', 'H', 'L', 'T', 'U') ORDER BY tabname WITH ur",
                                 schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTableColumnsBase(connection, schemaName, tableName,
                                       "SELECT c.colname, c.typename, c.length, c.length as precision, c.scale, c.nulls, c.colno FROM syscat.columns c INNER JOIN syscat.tables t on (c.tabschema = t.tabschema AND c.tabname = t.tabname) WHERE TRIM(UPPER(c.tabname)) = @1 AND TRIM(UPPER(c.tabschema)) = @2 ORDER BY c.colno WITH ur",
                                       tableName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTableColumnsBase(connection, schemaName, tableName,
                                       "SELECT c.colname, c.typename, c.length, c.length as precision, c.scale, c.nulls, c.colno FROM SYSCAT.KEYCOLUSE A, SYSCAT.TABCONST B, SYSCAT.COLUMNS C WHERE A.CONSTNAME=B.CONSTNAME AND B.TYPE = 'P' AND UPPER(A.TABNAME) = @1 AND UPPER(A.TABSCHEMA) = @2 and C.TABNAME = A.TABNAME AND C.TABSCHEMA = A.TABSCHEMA AND C.COLNAME = A.COLNAME order by A.COLSEQ",
                                       tableName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            return GetTablePartitionsBase(connection, schemaName, tableName,
                                       "SELECT datapartitionname FROM syscat.datapartitions WHERE TRIM(UPPER(tabschema)) = @1 AND TRIM(UPPER(tabname)) = @2 ORDER BY datapartitionname WITH ur",
                                       schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            return GetViewsBase(connection, schemaName,
                                "SELECT viewname FROM syscat.views WHERE TRIM(UPPER(viewschema)) = @1 ORDER BY viewname",
                                schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (viewName == null) throw new ArgumentNullException("viewName");
            return GetViewColumnsBase(connection, schemaName, viewName,
                                      "SELECT c.colname, c.typename, c.length, c.length, c.scale, c.nulls, c.colno FROM syscat.columns c INNER JOIN syscat.views t on (c.tabschema = t.viewschema AND c.tabname = t.viewname) WHERE TRIM(UPPER(c.tabname)) = @1 AND TRIM(UPPER(c.tabschema)) = @2 ORDER BY colname WITH ur",
                                      viewName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            return GetMaterializedViewsBase(connection, schemaName,
                                            "SELECT tabname FROM syscat.tables WHERE TRIM(UPPER(tabschema)) = @1 AND type IN ('S') ORDER BY tabname WITH ur",
                                            schemaName.Trim().ToUpper());
        }

        public override IList<Column> GetMaterializedViewColumns([NotNull] IDbConnection connection,
                                                                 [NotNull] string schemaName,
                                                                 [NotNull] string materializedViewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (materializedViewName == null) throw new ArgumentNullException("materializedViewName");
            return GetMaterializedViewColumnsBase(connection, schemaName, materializedViewName,
                                                  "SELECT c.colname, c.typename, c.length, c.length as precision, c.scale, c.nulls, c.colno FROM syscat.columns c INNER JOIN syscat.tables t on (c.tabschema = t.tabschema AND c.tabname = t.tabname) WHERE TRIM(UPPER(c.tabname)) = @1 AND TRIM(UPPER(c.tabschema)) = @2 ORDER BY c.colno WITH ur",
                                                  materializedViewName.Trim().ToUpper(), schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexes(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT indschema, INDNAME, UNIQUERULE FROM syscat.indexes WHERE TRIM(UPPER(indschema)) = @1 ORDER BY indname WITH ur",
                                  schemaName.Trim().ToUpper());
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");
            return GetIndexesBase(connection, schemaName,
                                  "SELECT indschema, indname, UNIQUERULE FROM syscat.indexes WHERE TRIM(UPPER(tabschema)) = @1 AND TRIM(UPPER(tabname)) = @2 ORDER BY indname WITH ur",
                                  schemaName.Trim().ToUpper(), tableName.Trim().ToUpper());
        }

        public override IList<Column> GetIndexColumns(IDbConnection connection, string schemaName, string indexName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (indexName == null) throw new ArgumentNullException("indexName");
            
            _log.DebugFormat("Getting columns for schema {0} and index {1} ...", schemaName, indexName);
            List<string> columnNames = null;
            string tableName = null, tableSchemaName = null;
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT colnames, tabname, tabschema FROM syscat.indexes WHERE TRIM(UPPER(indname)) = @1 AND TRIM(UPPER(indschema)) = @2 FETCH FIRST ROW ONLY WITH ur";
                var param = command.CreateParameter();
                param.ParameterName = "@1";
                param.Value = indexName.Trim().ToUpper();
                command.Parameters.Add(param);
                param = command.CreateParameter();
                param.ParameterName = "@2";
                param.Value = schemaName.Trim().ToUpper();
                command.Parameters.Add(param);
                using (var dr = command.ExecuteReader())
                {
                    while (dr != null && dr.Read())
                    {
                        columnNames = dr.GetString(0).Trim().ToUpper().Split(new[] { '+' },
                                                                                      StringSplitOptions.
                                                                                          RemoveEmptyEntries).ToList();
                        tableName = dr.GetString(1);
                        tableSchemaName = dr.GetString(2);
                    }
                }
            }

            if (tableName == null || tableSchemaName == null) return new List<Column>();
            var parameters = new List<string> { tableName.Trim().ToUpper(), tableSchemaName.Trim().ToUpper() };
            var sql = "SELECT c.colname, c.typename, c.length, c.length as precision, c.scale, c.nulls, c.colno FROM syscat.columns c INNER JOIN syscat.tables t on (c.tabschema = t.tabschema AND c.tabname = t.tabname) WHERE TRIM(UPPER(c.tabname)) = @1 AND TRIM(UPPER(c.tabschema)) = @2 AND TRIM(UPPER(c.colname)) IN (";
            var separator = string.Empty;
            for (var i = 0; i < columnNames.Count; i++)
            {
                sql += separator + "@" + (i + 3);
                separator = ", ";
                parameters.Add(columnNames[i].Trim().ToUpper());
            }
            sql += ")  ORDER BY c.colno WITH ur";

            return GetTableColumnsBase(connection, tableSchemaName, tableName, sql, parameters.Cast<object>().ToArray());
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSequencesBase(connection, schemaName,
                                    "SELECT seqname, minvalue, maxvalue, increment, nextcachefirstvalue FROM syscat.sequences WHERE TRIM(UPPER(seqschema)) = @1 ORDER BY seqname WITH ur",
                                    schemaName.Trim().ToUpper());
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTriggersBase(connection, schemaName,
                                   "SELECT trigname FROM syscat.triggers WHERE TRIM(UPPER(trigschema)) = @1 ORDER BY trigname WITH ur",
                                   schemaName.Trim().ToUpper());
        }

        public override IList<Synonym> GetPublicSynonyms(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSynonymsBase(connection, schemaName,
                                   "SELECT tabname FROM syscat.tables WHERE TRIM(UPPER(tabschema)) = 'SYSPUBLIC' AND type = 'A' ORDER BY tabname WITH ur");
        }

        public override IList<Synonym> GetSynonyms([NotNull] IDbConnection connection, [NotNull] string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetSynonymsBase(connection, schemaName,
                                   "SELECT tabname FROM syscat.tables WHERE TRIM(UPPER(tabschema)) = @1 AND type = 'A' ORDER BY tabname WITH ur",
                                   schemaName.ToUpper());
        }

        public override IList<StoredProcedure> GetStoredProcedures(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetStoredProceduresBase(connection, schemaName,
                                  "SELECT r.specificname, r.routinename, r.text FROM syscat.routines r WHERE UPPER(r.routineschema) = @1 AND r.routinetype = 'P' ORDER BY r.routinename",
                                  schemaName.ToUpper());
        }

        public override IList<Function> GetFunctions(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetFunctionsBase(connection, schemaName,
                                  "SELECT r.specificname, r.routinename, r.text FROM syscat.routines r WHERE UPPER(r.routineschema) = @1 AND r.routinetype = 'F' ORDER BY r.routinename",
                                  schemaName.ToUpper());
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters([NotNull] IDbConnection connection,
                                                                            [NotNull] StoredProcedure storedProcedure)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

            const string sql = "SELECT NVL(parmname, '') as parmname, typename,  length, length as precision, scale, nulls, ordinal, parm_mode FROM syscat.procparms WHERE UPPER(procschema) = @1 AND UPPER(procname) = @2 AND specificname = @3 ORDER BY ordinal";
            return GetStoredProcedureParametersBase(connection, storedProcedure, sql,
                                                    storedProcedure.Parent.Name.ToUpper(),
                                                    storedProcedure.Name.ToUpper(), storedProcedure.ObjectId);

        }

        public override IList<ColumnParameter> GetFunctionParameters(IDbConnection connection, Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");

            const string sql = "SELECT NVL(parmname, '') as parmname, typename,  length, length as precision, scale, 'Y' AS nulls, ordinal, 'IN' AS parm_mode FROM syscat.FUNCPARMS WHERE UPPER(funcschema) = @1 AND UPPER(funcname) = @2 AND specificname = @3 AND rowtype = 'P' ORDER BY ordinal";
            return GetStoredProcedureParametersBase(connection, function, sql,
                                                    function.Parent.Name.ToUpper(),
                                                    function.Name.ToUpper(), function.ObjectId);
        }

        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (function == null) throw new ArgumentNullException("function");

            const string sql = "SELECT parmname, typename,  length, length as precision, scale, 'Y' AS nulls, ordinal, 'OUT' AS parm_mode FROM syscat.FUNCPARMS WHERE UPPER(funcschema) = @1 AND UPPER(funcname) = @2 AND specificname = @3 AND rowtype = 'R' ORDER BY ordinal";
            return GetStoredProcedureParametersBase(connection, function, sql,
                                                    function.Parent.Name.ToUpper(),
                                                    function.Name.ToUpper(), function.ObjectId);
        }

        public override IntelisenseData GetIntelisenseData(IDbConnection connection, string currentSchemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (currentSchemaName == null) throw new ArgumentNullException("currentSchemaName");

            const string tablesSql =
                "SELECT c.tabschema, c.tabname, c.colname FROM syscat.columns c INNER JOIN syscat.TABLES t on (t.tabschema = c.tabschema AND t.tabname = c.tabname) WHERE t.type = 'T' ORDER BY tabschema, tabname, colname WITH ur";
            const string viewsSql =
                "SELECT v.viewschema, v.viewname, c.colname FROM syscat.VIEWS v INNER JOIN syscat.columns c on v.viewname = c.tabname AND v.viewschema =c.tabschema  ORDER BY tabschema, tabname, colname WITH ur";

            return GetIntellisenseDataHelper(connection, currentSchemaName, tablesSql, viewsSql, null);
        }
    }
}