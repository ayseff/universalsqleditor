using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using Utilities.Collections;
using Utilities.Text;
using log4net;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteInfoProvider : DbInfoProvider
    {
        private const string DEFAULT_SCHEMA = "MAIN";
        private static readonly Schema _defaultSchema = new Schema(string.Empty, DEFAULT_SCHEMA);
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override IList<DatabaseInstance> GetDatabaseInstances(IDbConnection connection)
        {
            throw new NotSupportedException();
        }

        public override IList<Schema> GetSchemas(IDbConnection connection, string databaseInstance = null)
        {
            throw new NotSupportedException();
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTablesBase(connection, schemaName,
                                 "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name");
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            _log.DebugFormat("Getting columns for schema {0} and table {1} ...", schemaName, tableName);
            var table = new Table(tableName, _defaultSchema);
            var columns = new List<Column>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("pragma table_info({0})", tableName);
                using (var dr = command.ExecuteReader())
                {
                    while (dr != null && dr.Read())
                    {
                        var column = ParseColumn(dr, table);
                        columns.Add(column);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            return columns.OrderBy(x => x.Name).ToList();
        }

        private static Column ParseColumn(IDataReader dr, Table table)
        {
            var column = new Column(dr.GetString(1).Trim().ToUpper(), table);
            if (!dr.IsDBNull(2))
            {
                var dataType = dr.GetString(2);
                if (!dataType.IsNullEmptyOrWhitespace())
                {
                    string[] splits = dataType.Split(new[] {'(', ')'}, StringSplitOptions.RemoveEmptyEntries);
                    column.DataType = splits[0];
                    if (splits.Length == 2)
                    {
                        int val;
                        if (int.TryParse(splits[1], out val))
                        {
                            column.CharacterLength = val;
                        }
                    }
                }
            }
            column.Nullable = dr.GetInt32(3) == 0;
            return column;
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            _log.DebugFormat("Getting primary key columns for schema {0} and table {1} ...", schemaName, tableName);
            var table = new Table(tableName, _defaultSchema);
            var columns = new List<Column>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("pragma table_info({0})", tableName);
                using (var dr = command.ExecuteReader())
                {
                    while (dr != null && dr.Read())
                    {
                        if (dr.GetInt32(5) != 1) continue;
                        var column = ParseColumn(dr, table);
                        columns.Add(column);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            return columns.OrderBy(x => x.Name).ToList();
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            return new List<Partition>();
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetViewsBase(connection, schemaName, "SELECT name FROM sqlite_master WHERE type='view' ORDER BY name");
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (viewName == null) throw new ArgumentNullException("viewName");
            return GetTableColumns(connection, schemaName, viewName);
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<MaterializedView>();
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName, string materializedViewName, string databaseInstanceName = null)
        {
            return new List<Column>();
        }

        public override IList<Index> GetIndexes(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");

            _log.DebugFormat("Getting indexes for schema {0} ...", schemaName);
            var schema = new Schema(schemaName);
            var indices = new List<Index>();
            var map = new Dictionary<Index, string>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT name, tbl_name FROM sqlite_master WHERE type='index' ORDER BY name";
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var index = new Index(dr.GetString(0).Trim().ToUpper(), schema) { Table = new Table(dr.GetString(1), new Schema(DEFAULT_SCHEMA))};
                        map.Add(index, dr.GetString(1));
                        indices.Add(index);
                    }
                }

                foreach (var index in indices)
                {
                    var tableName = map[index];
                    var tableIndex = GetIndexesForTable(connection, schemaName, tableName).First(x => String.Equals(x.Name, index.Name, StringComparison.CurrentCultureIgnoreCase));
                    index.IsUnique = tableIndex.IsUnique;
                }
            }
            _log.DebugFormat("Retrieved {0} index(es).", indices.Count.ToString("#,0"));
            return indices;
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, [NotNull] string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            _log.DebugFormat("Getting indexes for schema {0} and table {1} ...", schemaName, tableName);
            var schema = new Schema(schemaName);
            var table = new Table(tableName, schema);
            var indices = new List<Index>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA index_list('" + tableName + "')";
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var index = new Index(dr.GetString(1), schema) { Table = new Table(tableName, schema)};
                        index.IsUnique = dr[2].ToString() == "1";
                        index.Table = table;
                        indices.Add(index);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} index(es).", indices.Count.ToString("#,0"));
            return indices;
        }

        public override IList<Column> GetIndexColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, [NotNull] string indexName, object indexId = null, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (indexName == null) throw new ArgumentNullException("indexName");
            if (indexSchemaName == null) throw new ArgumentNullException("indexSchemaName");

            _log.DebugFormat("Getting indexed columns for schema {0} and index {1} ...", indexSchemaName, indexName);
            var schema = new Schema(indexSchemaName);
            var index = new Index(indexName, schema);
            var columns = new List<Column>();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA index_info('" + indexName + "')";
                using (var dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var column = new Column(dr.GetString(2), index);
                        columns.Add(column);
                    }
                }
            }
            _log.DebugFormat("Retrieved {0} indexed columns.", columns.Count.ToString("#,0"));
            return columns;
        }

        public override IList<Column> GetIndexIncludedColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, string indexName, object indexId = null, string databaseInstanceName = null)
        {
            return new List<Column>();
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            return GetSequencesBase(connection, schemaName, "SELECT name, 1, " + int.MaxValue + ", 1, seq FROM sqlite_sequence ORDER BY name");
        }

        public override IList<Constraint> GetConstraints(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<Constraint>();
        }

        public override IList<Constraint> GetConstraintsForTable(IDbConnection connection, string schemaName, string tableName,
            string databaseInstanceName = null)
        {
            return new List<Constraint>();
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            return GetTriggersBase(connection, _defaultSchema.Name,
                                   "SELECT name FROM sqlite_master WHERE type='trigger' ORDER BY name");
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
            return new List<StoredProcedure>();
        }

        public override IList<Function> GetFunctions(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            throw new NotImplementedException();
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
        {
            return new List<ColumnParameter>();
        }

        public override IList<ColumnParameter> GetFunctionParameters(IDbConnection connection, Function function)
        {
            throw new NotImplementedException();
        }


        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            throw new NotImplementedException();
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

            // Load schemas
            var intellisenseData = new IntelisenseData();
            intellisenseData.AllSchemas.Add(_defaultSchema);

            // Load tables
            var tables = GetTables(connection, currentSchemaName);
            _defaultSchema.Tables.Clear();
            _defaultSchema.Tables.AddRange(tables);
            intellisenseData.AllObjects.AddRange(tables);

            // Load columns
            foreach (var table in tables)
            {
                table.Columns.Clear();
                table.Columns.AddRange(GetTableColumns(connection, currentSchemaName, table.Name));
                intellisenseData.AllColumns.AddRange(table.Columns);
            }

            // Load views
            var views = GetViews(connection, currentSchemaName);
            _defaultSchema.Views.Clear();
            _defaultSchema.Views.AddRange(views);
            intellisenseData.AllObjects.AddRange(views);

            // Load view columns
            foreach (var view in views)
            {
                view.Columns.Clear();
                view.Columns.AddRange(GetViewColumns(connection, currentSchemaName, view.Name));
                intellisenseData.AllColumns.AddRange(view.Columns);
            }

            // Set current schema
            intellisenseData.CurrentSchema = _defaultSchema;
            return intellisenseData;
        }
    }
}