using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SqlEditor.Annotations;
using SqlEditor.Database;
using SqlEditor.DatabaseExplorer;
using Utilities.Collections;
using log4net;

namespace SqlEditor.Databases
{
    public static class ObjectScripter
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<string> GenerateViewSelectStatement([JetBrains.Annotations.NotNull] View view,
            [JetBrains.Annotations.NotNull] DatabaseConnection databaseConnection)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            try
            {
                _log.DebugFormat("Generating SELECT statement for view {0} ...", view.FullyQualifiedName);
                await LoadViewColumns(view, databaseConnection);

                var sb = new StringBuilder();
                sb.AppendLine("SELECT");
                sb.AppendLine("    " + string.Join("," + Environment.NewLine + "    ", view.Columns.Select(x => x.Name)));
                sb.AppendLine("FROM");
                sb.AppendLine("    " + view.FullyQualifiedName);
                _log.DebugFormat("Generating complete.");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                _log.Error("Error getting a list of columns.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        public static async Task<string> GenerateTableSelectStatement([JetBrains.Annotations.NotNull] Table table,
            [JetBrains.Annotations.NotNull] DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            try
            {
                _log.DebugFormat("Generating SELECT statement for view {0} ...", table.FullyQualifiedName);
                await LoadTableColumns(table, databaseConnection);

                var sb = new StringBuilder();
                sb.AppendLine("SELECT");
                sb.AppendLine("    " + string.Join("," + Environment.NewLine + "    ", table.Columns.Select(x => x.Name)));
                sb.AppendLine("FROM");
                sb.AppendLine("    " + table.FullyQualifiedName);
                _log.DebugFormat("Generating complete.");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                _log.Error("Error getting a list of columns.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        public static async Task<string> GenerateTableInsertStatement([NotNull] Table table,
                                                     [NotNull] DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating INSERT statement for table {0} ...", table.FullyQualifiedName);
            try
            {
                await LoadTableColumns(table, databaseConnection);
            }
            catch (Exception ex)
            {
                _log.Error("Error getting a list of columns.");
                _log.Error(ex.Message, ex);
                throw;
            }

            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO " + table.FullyQualifiedName + " (");
            sb.AppendLine("    " + string.Join("," + Environment.NewLine + "    ", table.Columns.Select(x => x.Name)) + ")");
            sb.AppendLine("VALUES (");
            var separator = string.Empty;
            foreach (var column in table.Columns)
            {
                sb.Append(separator);
                if (databaseConnection.DatabaseServer.NumericDataTypes.Contains(column.DataType.ToUpper()))
                {
                    sb.Append("    0");
                }
                else if (databaseConnection.DatabaseServer.DateTimeDataTypes.Contains(column.DataType.ToUpper()))
                {
                    var date = DateTime.Now;
                    sb.AppendFormat("    {0}{1}{0}", "'", date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    sb.Append("    ''");
                }
                separator = "," + Environment.NewLine;
            }
            sb.AppendLine(")");
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static async Task<string> GenerateTableUpdateStatement(Table table, DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating UPDATE statement for table {0} ...", table.FullyQualifiedName);
            try
            {
                await LoadTableColumns(table, databaseConnection);
            }
            catch (Exception ex)
            {
                _log.Error("Error getting a list of columns.");
                _log.Error(ex.Message, ex);
                throw;
            }

            var sb = new StringBuilder();
            sb.AppendLine("UPDATE " + table.FullyQualifiedName);
            sb.AppendLine("SET");
            AppendColumns(databaseConnection, sb, table.Columns.Where(x => table.PrimaryKeyColumns.All(y => y != x)), ",", " ");
            sb.AppendLine(Environment.NewLine + "WHERE");
            AppendColumns(databaseConnection, sb, table.PrimaryKeyColumns, "AND ", "    ");
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static async Task<string> GenerateTableDeleteStatement(Table table, DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DELETE statement for table {0} ...", table.FullyQualifiedName);
            try
            {
                await LoadTableColumns(table, databaseConnection);
            }
            catch (Exception ex)
            {
                _log.Error("Error getting a list of columns.");
                _log.Error(ex.Message, ex);
                throw;
            }

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM " + table.FullyQualifiedName);
            sb.AppendLine("WHERE");
            AppendColumns(databaseConnection, sb, table.PrimaryKeyColumns, "AND ", "    ");
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static string GenerateTableDropStatement(Table table, DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DROP statement for table {0} ...", table.FullyQualifiedName);

            var sb = new StringBuilder();
            sb.AppendLine("DROP TABLE " + table.FullyQualifiedName);
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static string GenerateViewDropStatement(View view, DatabaseConnection databaseConnection)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DROP statement for view {0} ...", view.FullyQualifiedName);

            var sb = new StringBuilder();
            sb.AppendLine("DROP VIEW " + view.FullyQualifiedName);
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static string GenerateStoredProcedureDropStatement(StoredProcedure storedProcedure, DatabaseConnection databaseConnection)
        {
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DROP statement for stored procedure {0} ...", storedProcedure.FullyQualifiedName);

            var sb = new StringBuilder();
            sb.AppendLine("DROP PROCEDURE " + storedProcedure.FullyQualifiedName);
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static string GenerateFunctionDropStatement(Function function, DatabaseConnection databaseConnection)
        {
            if (function == null) throw new ArgumentNullException("function");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DROP statement for function {0} ...", function.FullyQualifiedName);

            var sb = new StringBuilder();
            sb.AppendLine("DROP FUNCTION " + function.FullyQualifiedName);
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static string GeneratePackageDropStatement(Package package, DatabaseConnection databaseConnection)
        {
            if (package == null) throw new ArgumentNullException("package");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DROP statement for package {0} ...", package.FullyQualifiedName);

            var sb = new StringBuilder();
            sb.AppendLine("DROP PACKAGE " + package.FullyQualifiedName);
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        private static void AppendColumns(DatabaseConnection databaseConnection, StringBuilder sb, IEnumerable<Column> columns, string columnSeparator, string firstColumnSeparator)
        {
            var separator = firstColumnSeparator;
            foreach (var column in columns)
            {
                sb.Append("    " + separator);
                sb.Append(column.Name + " = ");
                if (databaseConnection.DatabaseServer.NumericDataTypes.Contains(column.DataType.ToUpper()))
                {
                    sb.Append("0");
                }
                else if (databaseConnection.DatabaseServer.DateTimeDataTypes.Contains(column.DataType.ToUpper()))
                {
                    var date = DateTime.Now;
                    sb.AppendFormat("{0}{1}{0}", "'", date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    sb.Append("''");
                }
                separator = Environment.NewLine + "    " + columnSeparator;
            }
        }

        private static async Task LoadTableColumns(Table table, DatabaseConnection databaseConnection)
        {
            if (table.Columns.Count != 0 &&
                table.PrimaryKeyColumns.Count != 0)
            {
                return;
            }

            table.Columns.Clear();
            table.PrimaryKeyColumns.Clear();

            _log.DebugFormat("Fetching columns for table {0} ...", table.FullyQualifiedName);
            var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
            IList<Column> columns, primaryKeyColumns;
            try
            {
                using (var connection = await databaseConnection.CreateNewConnectionAsync())
                {
                    await connection.OpenIfRequiredAsync();
                    var databaseInstanceName = table.Parent.Parent == null ? null : table.Parent.Parent.Name;
                    columns = await infoProvider.GetTableColumnsAsync(connection, table.Parent.Name, table.Name, databaseInstanceName);
                    primaryKeyColumns =
                        await infoProvider.GetTablePrimaryKeyColumnsAsync(connection, table.Parent.Name, table.Name, databaseInstanceName);
                    primaryKeyColumns =
                        columns.Where(
                            x => primaryKeyColumns.Any(y => String.Equals(y.Name.Trim(), x.Name.Trim(), StringComparison.CurrentCultureIgnoreCase)))
                            .ToList();
                    _log.DebugFormat("Fetching complete.");
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error getting a list of columns for table {0}.", table.FullyQualifiedName);
                _log.Error(ex.Message, ex);
                throw;
            }
            table.Columns.AddRange(columns);
            table.PrimaryKeyColumns.AddRange(primaryKeyColumns);
        }

        private static async Task LoadViewColumns(View view, DatabaseConnection databaseConnection)
        {
            if (view.Columns.Count != 0 &&
                view.PrimaryKeyColumns.Count != 0)
            {
                return;
            }

            view.Columns.Clear();
            view.PrimaryKeyColumns.Clear();

            _log.DebugFormat("Fetching columns for table {0} ...", view.FullyQualifiedName);
            var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
            IList<Column> columns;
            try
            {
                using (var connection = await databaseConnection.CreateNewConnectionAsync())
                {
                    await connection.OpenIfRequiredAsync();
                    var databaseInstanceName = view.Parent.Parent == null ? null : view.Parent.Parent.Name;
                    columns = await infoProvider.GetViewColumnsAsync(connection, view.Parent.Name, view.Name, databaseInstanceName);
                    _log.DebugFormat("Fetching complete.");
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error getting a list of columns for view {0}.", view.FullyQualifiedName);
                _log.Error(ex.Message, ex);
                throw;
            }
            view.Columns.AddRange(columns);
        }
    }
}
