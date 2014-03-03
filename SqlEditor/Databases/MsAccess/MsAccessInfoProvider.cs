using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using Utilities.Collections;
using log4net;

namespace SqlEditor.Databases.MsAccess
{
    public class MsAccessInfoProvider : DbInfoProvider
    {
        private const string DEFAULT_SCHEMA = "MAIN";
        private static readonly Schema _defaultSchema = new Schema(string.Empty, DEFAULT_SCHEMA);
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override IList<Schema> GetSchemas(IDbConnection connection)
        {
            return new List<Schema>(new[] {_defaultSchema});
        }

        public override IList<Table> GetTables(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            _log.DebugFormat("Getting tables for schema {0} ...", schemaName);
            var tblrestrictions = new[] {null, null, null, "TABLE"};
            var tables = new List<Table>();
            var olecon = (OleDbConnection) connection;
            DataTable dataTable = olecon.GetSchema("tables", tblrestrictions);
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var tableName = (string) dataRow["TABLE_NAME"];
                tables.Add(new Table(tableName, MsAccess2003DatabaseServer.DefaultSchema));
            }
            _log.DebugFormat("Retrieved {0}  table(s).", tables.Count.ToString("#,0"));
            return tables.OrderBy(x => x.Name).ToList();
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (schemaName == null) throw new ArgumentNullException("schemaName");
            if (tableName == null) throw new ArgumentNullException("tableName");

            _log.DebugFormat("Getting column for schema {0} and table {1} ...", schemaName, tableName);
            var t = new Table(tableName, _defaultSchema);
            var columns = GetTableColumnsDefault<OleDbType>(connection, t);
            _log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            return columns.OrderBy(x => x.Name).ToList();
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName)
        {
            return new List<Column>();
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName)
        {
            return new List<Partition>();
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            var conn = connection as OleDbConnection;
            if (conn == null)
            {
                throw new Exception("Microsoft Access database requires OleDbConnection");
            }
            var views = new List<View>();
            DataTable queries = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Views, null);
            if (queries != null)
            {
                foreach (DataRow row in queries.Rows)
                {
                    string viewName = row["TABLE_NAME"].ToString();
                    //var viewSql = row["VIEW_DEFINITION"].ToString();
                    var view = new View(viewName, _defaultSchema);
                    views.Add(view);
                }
            }
            return views.OrderBy(x => x.Name).ToList();
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (viewName == null) throw new ArgumentNullException("viewName");

            _log.DebugFormat("Getting columns for schema {0} and view {1} ...", schemaName, viewName);
            var t = new View(viewName, _defaultSchema);
            var columns = GetTableColumnsDefault<OleDbType>(connection, t);
            _log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            return columns.OrderBy(x => x.Name).ToList();
        }

        public override IList<MaterializedView> GetMaterializedViews(IDbConnection connection, string schemaName)
        {
            return new List<MaterializedView>();
        }

        public override IList<Column> GetMaterializedViewColumns(IDbConnection connection, string schemaName,
                                                                 string materializedViewName)
        {
            return new List<Column>();
        }

        public override IList<Index> GetIndexes(IDbConnection connection, string schemaName)
        {
            return new List<Index>();
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName)
        {
            return new List<Index>();
        }

        public override IList<Column> GetIndexColumns(IDbConnection connection, string schemaName, string indexName)
        {
            return new List<Column>();
        }

        public override IList<Sequence> GetSequences(IDbConnection connection, string schemaName)
        {
            return new List<Sequence>();
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName)
        {
            return new List<Trigger>();
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
            throw new NotImplementedException();
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
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
            IList<Table> tables = GetTables(connection, currentSchemaName);
            _defaultSchema.Tables.Clear();
            _defaultSchema.Tables.AddRange(tables);
            intellisenseData.AllObjects.AddRange(tables);

            // Load table columns
            foreach (Table table in tables)
            {
                table.Columns.Clear();
                table.Columns.AddRange(GetTableColumns(connection, currentSchemaName, table.Name));
                intellisenseData.AllColumns.AddRange(table.Columns);
            }

            // Load views
            IList<View> views = GetViews(connection, currentSchemaName);
            _defaultSchema.Views.Clear();
            _defaultSchema.Views.AddRange(views);
            intellisenseData.AllObjects.AddRange(views);

            // Load view columns
            foreach (View view in views)
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