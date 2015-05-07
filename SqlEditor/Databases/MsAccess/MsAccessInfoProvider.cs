using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
using ADOX;
using SqlEditor.Database;
using SqlEditor.Intellisense;
using Utilities.Collections;
using log4net;
using View = SqlEditor.Database.View;

namespace SqlEditor.Databases.MsAccess
{
    public class MsAccessInfoProvider : DbInfoProvider
    {
        private const string DEFAULT_SCHEMA = "MAIN";
        private static readonly Schema _defaultSchema = new Schema(string.Empty, DEFAULT_SCHEMA);
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<string> _systemTablesList = new List<string> { "MSYSACCESSSTORAGE", "MSYSACES", "MSYSCOMPLEXCOLUMNS", "MSYSNAMEMAP", "MSYSNAMEMAP", "MSYSNAVPANEGROUPCATEGORIES", "MSYSNAVPANEGROUPS", "MSYSNAVPANEGROUPTOOBJECTS", "MSYSNAVPANEOBJECTIDS", "MSYSOBJECTS", "MSYSQUERIES", "MSYSRELATIONSHIPS", "MSYSRESOURCES" };
        private readonly List<string> _systemViewsList = new List<string>(); // { "MSYSACCESSSTORAGE", "MSYSACES", "MSYSCOMPLEXCOLUMNS", "MSYSNAMEMAP", "MSYSNAMEMAP", "MSYSNAVPANEGROUPCATEGORIES", "MSYSNAVPANEGROUPS", "MSYSNAVPANEGROUPTOOBJECTS", "MSYSNAVPANEOBJECTIDS", "MSYSOBJECTS", "MSYSQUERIES", "MSYSRELATIONSHIPS", "MSYSRESOURCES" }; 

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
            var cnn = new ADODB.Connection();
            try
            {
                var tables = new List<Table>();
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                var viewNames = (catalog.Views.Cast<ADOX.View>().Select(view => view.Name)).ToList();

                foreach (ADOX.Table table in catalog.Tables)
                {
                    if (_systemTablesList.Any(
                            x => string.Equals(table.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    if (viewNames.Any(
                            x => string.Equals(table.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    tables.Add(new Table(table.Name, MsAccess2003DatabaseServer.DefaultSchema));
                }
                return tables;
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }

            //var tblrestrictions = new[] {null, null, null, "TABLE"};
            //var tables = new List<Table>();
            //var olecon = (OleDbConnection) connection;
            //var dataTable = olecon.GetSchema("tables", tblrestrictions);
            //foreach (DataRow dataRow in dataTable.Rows)
            //{
            //    var tableName = (string) dataRow["TABLE_NAME"];
            //    tables.Add(new Table(tableName, MsAccess2003DatabaseServer.DefaultSchema));
            //}
            //_log.DebugFormat("Retrieved {0}  table(s).", tables.Count.ToString("#,0"));
            //return tables.OrderBy(x => x.Name).ToList();
        }

        public override IList<Column> GetTableColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var columns = new List<Column>();
            var cnn = new ADODB.Connection();
            try
            {
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                foreach (ADOX.Table table in catalog.Tables)
                {
                    if (_systemTablesList.Any(
                            x => string.Equals(table.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    if (!string.Equals(table.Name, tableName))
                    {
                        continue;
                    }

                    var tab = new Table(table.Name, MsAccess2003DatabaseServer.DefaultSchema);
                    foreach (ADOX.Column column in table.Columns)
                    {
                        var col = GetColumn(column, tab);
                        columns.Add(col);
                    }
                    return columns;
                }
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
            return columns;

            //_log.DebugFormat("Getting column for schema {0} and table {1} ...", schemaName, tableName);
            //var t = new Table(tableName, _defaultSchema);
            //var columns = GetTableColumnsDefault<OleDbType>(connection, t);
            //_log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            //return columns.OrderBy(x => x.Name).ToList();
        }

        public override IList<Column> GetTablePrimaryKeyColumns(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (tableName == null) throw new ArgumentNullException("tableName");

            var columns = new List<Column>();
            var cnn = new ADODB.Connection();
            try
            {
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                foreach (ADOX.Table table in catalog.Tables)
                {
                    if (_systemTablesList.Any(
                            x => string.Equals(table.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    if (!string.Equals(table.Name, tableName))
                    {
                        continue;
                    }

                    var tab = new Table(table.Name, MsAccess2003DatabaseServer.DefaultSchema);
                    foreach (ADOX.Key key in table.Keys)
                    {
                        if (key.Type != KeyTypeEnum.adKeyPrimary)
                        {
                            continue;
                        }

                        foreach (ADOX.Column column in key.Columns)
                        {
                            var col = GetColumn(column, tab);
                            columns.Add(col);
                        }
                        return columns;
                    }
                }
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
            return columns;
        }

        public override IList<Partition> GetTablePartitions(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            return new List<Partition>();
        }

        public override IList<View> GetViews(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            var cnn = new ADODB.Connection();
            try
            {
                var views = new List<View>();
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                foreach (ADOX.View view in catalog.Views)
                {
                    if (_systemViewsList.Any(
                            x => string.Equals(view.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }
                    views.Add(new View(view.Name, MsAccess2003DatabaseServer.DefaultSchema));
                }
                return views;
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }


            //if (connection == null) throw new ArgumentNullException("connection");
            //var conn = connection as OleDbConnection;
            //if (conn == null)
            //{
            //    throw new Exception("Microsoft Access database requires OleDbConnection");
            //}
            //var views = new List<View>();
            //DataTable queries = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Views, null);
            //if (queries != null)
            //{
            //    foreach (DataRow row in queries.Rows)
            //    {
            //        var viewName = row["TABLE_NAME"].ToString();
            //        //var viewSql = row["VIEW_DEFINITION"].ToString();
            //        var view = new View(viewName, _defaultSchema);
            //        views.Add(view);
            //    }
            //}
            //return views.OrderBy(x => x.Name).ToList();
        }

        public override IList<Column> GetViewColumns(IDbConnection connection, string schemaName, string viewName, string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (viewName == null) throw new ArgumentNullException("viewName");

            _log.DebugFormat("Getting columns for schema {0} and view {1} ...", schemaName, viewName);
            var t = new View(viewName, _defaultSchema);
            var columns = GetTableColumnsDefault<OleDbType>(connection, t);
            _log.DebugFormat("Retrieved {0}  column(s).", columns.Count.ToString("#,0"));
            return columns.OrderBy(x => x.Name).ToList();
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
            var cnn = new ADODB.Connection();
            try
            {
                var schema = new Schema(DEFAULT_SCHEMA);
                var indexes = new List<Index>();
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                for (var i = 0; i < catalog.Tables.Count; i++)
                {
                    var table = catalog.Tables[i];
                    var tableObj = new Table(table.Name, MsAccess2003DatabaseServer.DefaultSchema);
                    for (var j = 0; j < table.Indexes.Count; j++)
                    {
                        indexes.Add(new Index(table.Indexes[j].Name, schema) { Table = tableObj});
                    }
                }
                return indexes;
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
        }

        public override IList<Index> GetIndexesForTable(IDbConnection connection, string schemaName, string tableName, string databaseInstanceName = null)
        {
            var indexes = new List<Index>();
            var cnn = new ADODB.Connection();
            try
            {
                var schema = new Schema(DEFAULT_SCHEMA);
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                for (var i = 0; i < catalog.Tables.Count; i++)
                {
                    var table = catalog.Tables[i];
                    var tableObj = new Table(table.Name, MsAccess2003DatabaseServer.DefaultSchema);
                    if (!string.Equals(table.Name, tableName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    for (var j = 0; j < table.Indexes.Count; j++)
                    {
                        var index = new Index(table.Indexes[j].Name, schema) { Table = tableObj };
                        index.IsUnique = table.Indexes[j].Unique;
                        indexes.Add(index);
                    }
                    return indexes;
                }
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
            return indexes;
        }

        public override IList<Column> GetIndexColumns(IDbConnection connection, string tableSchemaName, string tableName, string indexSchemaName, string indexName, object indexId = null, string databaseInstanceName = null)
        {
            var columns = new List<Column>();
            var cnn = new ADODB.Connection();
            try
            {
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                for (var i = 0; i < catalog.Tables.Count; i++)
                {
                    var table = catalog.Tables[i];
                    if (!string.Equals(table.Name, tableName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    for (var j = 0; j < table.Indexes.Count; j++)
                    {
                        if (!string.Equals(table.Indexes[j].Name, indexName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        foreach (ADOX.Column column in table.Indexes[j].Columns)
                        {
                            var col = GetColumn(column, null);
                            columns.Add(col);
                        }
                        return columns;
                    }
                }
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
            return columns;
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

            var constraints = new List<Constraint>();
            var cnn = new ADODB.Connection();
            try
            {
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                foreach (ADOX.Table table in catalog.Tables)
                {
                    if (_systemTablesList.Any(
                            x => string.Equals(table.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }
                    
                    foreach (ADOX.Key key in table.Keys)
                    {
                        var constraint = new Constraint(key.Name, MsAccess2003DatabaseServer.DefaultSchema);
                        constraints.Add(constraint);
                    }
                }
                return constraints;
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
            return constraints;
        }

        public override IList<Constraint> GetConstraintsForTable(IDbConnection connection, string schemaName, string tableName,
            string databaseInstanceName = null)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            var constraints = new List<Constraint>();
            var cnn = new ADODB.Connection();
            try
            {
                cnn.Open(connection.ConnectionString);
                var catalog = new Catalog();
                catalog.ActiveConnection = cnn;

                foreach (ADOX.Table table in catalog.Tables)
                {
                    if (_systemTablesList.Any(
                            x => string.Equals(table.Name, x, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        continue;
                    }

                    if (!string.Equals(table.Name, tableName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    foreach (ADOX.Key key in table.Keys)
                    {
                        var constraint = new Constraint(key.Name, MsAccess2003DatabaseServer.DefaultSchema);
                        constraints.Add(constraint);
                        return constraints;
                    }
                }
            }
            finally
            {
                try
                {
                    cnn.Close();
                }
                catch
                {
                }
            }
            return constraints;
        }

        public override IList<Trigger> GetTriggers(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            return new List<Trigger>();
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
            throw new NotSupportedException();
        }

        public override IList<Function> GetFunctions(IDbConnection connection, string schemaName, string databaseInstanceName = null)
        {
            throw new NotSupportedException();
        }

        public override IList<ColumnParameter> GetStoredProcedureParameters(IDbConnection connection, StoredProcedure storedProcedure)
        {
            throw new NotSupportedException();
        }

        public override IList<ColumnParameter> GetFunctionParameters(IDbConnection connection, Function function)
        {
            throw new NotSupportedException();
        }

        public override IList<ColumnParameter> GetFunctionReturnValue(IDbConnection connection, Function function)
        {
            throw new NotSupportedException();
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

            // Load table columns
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

        private Column GetColumn(ADOX.Column column, Table tab)
        {
            var col = new Column(column.Name, tab);
            try
            {
                col.CharacterLength = column.DefinedSize;
            }
            catch
            {}
            try
            {
                col.DataPrecision = column.Precision;
            }
            catch
            { }
            try
            {
                col.DataScale = column.NumericScale;
            }
            catch
            { }
            try
            {
                col.DataType = GetDataType(column.Type);
            }
            catch
            { }
            return col;
        }

        private string GetDataType(DataTypeEnum type)
        {
            if (type == DataTypeEnum.adBSTR)
            {
                return "BSTR";
            }
            else if (type == DataTypeEnum.adBigInt)
            {
                return "BIGINT";
            }
            else if (type == DataTypeEnum.adBinary)
            {
                return "BINARY";
            }
            else if (type == DataTypeEnum.adBoolean)
            {
                return "BOOLEAN";
            }
            else if (type == DataTypeEnum.adChapter)
            {
                return "CHAPTER";
            }
            else if (type == DataTypeEnum.adChar)
            {
                return "CHAR";
            }
            else if (type == DataTypeEnum.adCurrency)
            {
                return "CURRENCY";
            }
            else if (type == DataTypeEnum.adDBDate)
            {
                return "DATE";
            }
            else if (type == DataTypeEnum.adDBTime)
            {
                return "TIME";
            }
            else if (type == DataTypeEnum.adDBTimeStamp)
            {
                return "TIMESTAMP";
            }
            else if (type == DataTypeEnum.adDate)
            {
                return "DATE";
            }
            else if (type == DataTypeEnum.adDecimal)
            {
                return "DECIMAL";
            }
            else if (type == DataTypeEnum.adDouble)
            {
                return "DOUBLE";
            }
            else if (type == DataTypeEnum.adEmpty)
            {
                return "EMPTY";
            }
            else if (type == DataTypeEnum.adError)
            {
                return "ERROR";
            }
            else if (type == DataTypeEnum.adFileTime)
            {
                return "FILETIME";
            }
            else if (type == DataTypeEnum.adGUID)
            {
                return "GUID";
            }
            else if (type == DataTypeEnum.adIDispatch)
            {
                return "DISPATCH";
            }
            else if (type == DataTypeEnum.adIUnknown)
            {
                return "UNKNOWN";
            }
            else if (type == DataTypeEnum.adInteger)
            {
                return "INTEGER";
            }
            else if (type == DataTypeEnum.adLongVarBinary)
            {
                return "LONGVARBINARY";
            }
            else if (type == DataTypeEnum.adLongVarChar)
            {
                return "LONGVARCHAR";
            }
            else if (type == DataTypeEnum.adLongVarWChar)
            {
                return "LONGVARWCHAR";
            }
            else if (type == DataTypeEnum.adNumeric)
            {
                return "NUMERIC";
            }
            else if (type == DataTypeEnum.adPropVariant)
            {
                return "PROPVARIANT";
            }
            else if (type == DataTypeEnum.adSingle)
            {
                return "SINGLE";
            }
            else if (type == DataTypeEnum.adSmallInt)
            {
                return "SMALLINT";
            }
            else if (type == DataTypeEnum.adTinyInt)
            {
                return "TINYINT";
            }
            else if (type == DataTypeEnum.adUnsignedBigInt)
            {
                return "UNSIGNEDBIGINT";
            }
            else if (type == DataTypeEnum.adUnsignedInt)
            {
                return "UNSIGNEDINT";
            }
            else if (type == DataTypeEnum.adUnsignedSmallInt)
            {
                return "UNSIGNEDSMALLINT";
            }
            else if (type == DataTypeEnum.adUnsignedTinyInt)
            {
                return "UNSIGNEDTINYINT";
            }
            else if (type == DataTypeEnum.adUserDefined)
            {
                return "USERDEFINED";
            }
            else if (type == DataTypeEnum.adVarBinary)
            {
                return "VARBINARY";
            }
            else if (type == DataTypeEnum.adVarChar)
            {
                return "VARCHAR";
            }
            else if (type == DataTypeEnum.adVarNumeric)
            {
                return "VARNUMERIC";
            }
            else if (type == DataTypeEnum.adVarWChar)
            {
                return "VARWCHAR";
            }
            else if (type == DataTypeEnum.adVariant)
            {
                return "VARINAT";
            }
            else if (type == DataTypeEnum.adWChar)
            {
                return "WCHAR";
            }
            else
            {
                return "UNKNOWN";
            }
        }
    }
}