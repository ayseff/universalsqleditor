﻿using System;
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
    public class ObjectScripter
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<string> GenerateViewSelectStatement([JetBrains.Annotations.NotNull] View view,
            [JetBrains.Annotations.NotNull] DatabaseConnection databaseConnection)
        {
            return await GenerateSelectStatement(view, databaseConnection);
        }

        public static async Task<string> GenerateTableSelectStatement([JetBrains.Annotations.NotNull] Table table,
            [JetBrains.Annotations.NotNull] DatabaseConnection databaseConnection)
        {
            return await GenerateSelectStatement(table, databaseConnection);
        }

        public static async Task<string> GenerateSelectStatement([JetBrains.Annotations.NotNull] DatabaseObjectWithColumns objectWithColumns,
            [JetBrains.Annotations.NotNull] DatabaseConnection databaseConnection)
        {
            if (objectWithColumns == null) throw new ArgumentNullException("objectWithColumns");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating SELECT statement for view {0} ...", objectWithColumns.FullyQualifiedName);
            await LoadColumns(objectWithColumns, databaseConnection);

            var sb = new StringBuilder();
            sb.AppendLine("SELECT");
            sb.AppendLine("\t" + string.Join("," + Environment.NewLine + "\t", objectWithColumns.Columns.Select(x => x.Name)));
            sb.AppendLine("FROM");
            sb.AppendLine("\t" + objectWithColumns.FullyQualifiedName);
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static async Task<string> GenerateTableInsertStatement([NotNull] Table table,
                                                     [NotNull] DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating INSERT statement for table {0} ...", table.FullyQualifiedName);
            await LoadColumns(table, databaseConnection);

            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO " + table.FullyQualifiedName + " (");
            sb.AppendLine("\t" + string.Join("," + Environment.NewLine + "\t", table.Columns.Select(x => x.Name)) + ")");
            sb.AppendLine("VALUES (");
            var separator = string.Empty;
            foreach (var column in table.Columns)
            {
                sb.Append(separator);
                if (databaseConnection.DatabaseServer.NumericDataTypes.Contains(column.DataType.ToUpper()))
                {
                    sb.Append("\t0");
                }
                else if (databaseConnection.DatabaseServer.DateTimeDataTypes.Contains(column.DataType.ToUpper()))
                {
                    var date = DateTime.Now;
                    sb.AppendFormat("\t{0}{1}{0}", "'", date.ToString("yyyy-MM-dd"));
                }
                else
                {
                    sb.Append("\t''");
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
            await LoadColumns(table, databaseConnection);

            var sb = new StringBuilder();
            sb.AppendLine("UPDATE " + table.FullyQualifiedName);
            sb.AppendLine("SET");
            AppendColumns(databaseConnection, sb, table.Columns.Where(x => table.PrimaryKeyColumns.All(y => y != x)), ",", " ");
            sb.AppendLine(Environment.NewLine + "WHERE");
            AppendColumns(databaseConnection, sb, table.PrimaryKeyColumns, "AND ", "\t");
            _log.DebugFormat("Generating complete.");
            return sb.ToString();
        }

        public static async Task<string> GenerateTableDeleteStatement(Table table, DatabaseConnection databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Generating DELETE statement for table {0} ...", table.FullyQualifiedName);
            await LoadColumns(table, databaseConnection);

            var sb = new StringBuilder();
            sb.AppendLine("DELETE FROM " + table.FullyQualifiedName);
            sb.AppendLine("WHERE");
            AppendColumns(databaseConnection, sb, table.PrimaryKeyColumns, "AND ", "\t");
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

        private static void AppendColumns(DatabaseConnection databaseConnection, StringBuilder sb, IEnumerable<Column> columns, string columnSeparator, string firstColumnSeparator)
        {
            var separator = firstColumnSeparator;
            foreach (var column in columns)
            {
                sb.Append("\t" + separator);
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
                separator = Environment.NewLine + "\t" + columnSeparator;
            }
        }

        private static async Task LoadColumns(DatabaseObjectWithColumns table, DatabaseConnection databaseConnection)
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
            table.Columns.AddRange(columns);
            table.PrimaryKeyColumns.AddRange(primaryKeyColumns);
        }


        
    }
}
