using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SqlEditor.ScriptResults
{
    public static class FormattingHelpers
    {
        public static string AsFormattedString(this IEnumerable<DataRow> rows, IEnumerable<string> columnNames)
        {
            if (rows == null) return null;
            var dataRows = rows as IList<DataRow> ?? rows.ToList();
            if (dataRows.Count == 0) return string.Empty;
            var table = dataRows.First().Table;

            if (dataRows.Any(dataRow => dataRow.Table != table))
            {
                throw new Exception("Row collection does not belong to the same data table.");
            }


            var columns =
                columnNames.Where(x => table.Columns.Contains(x)).Select(x => x).ToList();
            var dataLengths = GetMaxDataLengths(table, columns, dataRows);
            var sb = new StringBuilder();
            PrintHeader(columns, sb, dataLengths);
            PrintRows(dataRows, columns, sb, dataLengths);
            return sb.ToString();
        }

        private static void PrintHeader(List<string> columns, StringBuilder sb, IDictionary<string, int> dataLengths)
        {
            // Columns
            foreach (var column in columns)
            {
                sb.Append(column.PadRight(dataLengths[column] + 1));
            }
            sb.AppendLine();

            // Separator
            foreach (var column in columns)
            {
                sb.Append(string.Empty.PadRight(dataLengths[column], '-'));
                sb.Append(" ");
            }
            sb.AppendLine();
        }

        private static void PrintRows(IEnumerable<DataRow> dataRows, List<string> columns, StringBuilder sb, Dictionary<string, int> dataLengths)
        {
            // Data
            foreach (var row in dataRows)
            {
                foreach (var column in columns)
                {
                    var dataValueString = "NULL";
                    if (!row.IsNull(column))
                    {
                        dataValueString = row[column].ToString();
                    }
                    sb.Append(dataValueString.PadRight(dataLengths[column] + 1));
                }
                sb.AppendLine();
            }
        }

        private static Dictionary<string, int> GetMaxDataLengths(DataTable table, IEnumerable<string> columns, IList<DataRow> dataRows)
        {
            // Get max data lengths for padding
            var dataLengths = table.Columns.Cast<DataColumn>()
                                   .ToDictionary(column => column.ColumnName, column => column.ColumnName.Length);
            foreach (var column in columns)
            {
                foreach (var row in dataRows)
                {
                    var dataValueString = "NULL";
                    if (!row.IsNull(column))
                    {
                        dataValueString = row[column].ToString();
                    }
                    if (dataLengths[column] < dataValueString.Length)
                    {
                        dataLengths[column] = dataValueString.Length;
                    }
                }
            }
            return dataLengths;
        }

        public static string AsFormattedString(this DataTable expectedDataTable)
        {
            if (expectedDataTable == null) return null;
            var dataLengths = expectedDataTable.Columns.Cast<DataColumn>().ToDictionary(column => column.ColumnName, column => column.ColumnName.Length);

            // Get max data lengths for padding
            var columns = expectedDataTable.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();
            foreach (var column in columns)
            {
                foreach (DataRow row in expectedDataTable.Rows)
                {
                    var dataValueString = "NULL";
                    if (!row.IsNull(column))
                    {
                        dataValueString = row[column].ToString();
                    }
                    var currentMaxValue = dataLengths[column];
                    var dataLength = dataValueString.Length;
                    if (currentMaxValue < dataLength)
                    {
                        dataLengths[column] = dataLength;
                    }
                }
            }

            var sb = new StringBuilder();

            // Header
            foreach (var columnName in columns)
            {
                sb.Append(columnName.PadRight(dataLengths[columnName] + 1));
            }
            sb.AppendLine();

            // Separator
            foreach (var column in columns)
            {
                sb.Append(string.Empty.PadRight(dataLengths[column], '-'));
                sb.Append(" ");
            }
            sb.AppendLine();

            // Data
            foreach (DataRow row in expectedDataTable.Rows)
            {
                foreach (var column in columns)
                {
                    var dataValueString = "NULL";
                    if (!row.IsNull(column))
                    {
                        dataValueString = row[column].ToString();
                    }
                    sb.Append(dataValueString.PadRight(dataLengths[column] + 1));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}