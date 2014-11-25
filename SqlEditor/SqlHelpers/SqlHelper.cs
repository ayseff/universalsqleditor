using System;
using System.Data;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using SqlEditor.QueryResults;
using Utilities.Text;

namespace SqlEditor.SqlHelpers
{
    public static class SqlHelper
    {
        public static string TrimLeadingComments(string sql, params Regex[] commentRegexes)
        {
            if (sql == null) throw new ArgumentNullException("sql");
            if (commentRegexes == null) throw new ArgumentNullException("commentRegexes");
            foreach (var commentRegex in commentRegexes)
            {
                sql = commentRegex.Replace(sql, String.Empty);
            }
            return sql;
        }

        public static string GetFirstKeyword(string sql, params Regex[] commentRegexes)
        {
            if (sql == null) throw new ArgumentNullException("sql");
            sql = TrimLeadingComments(sql, commentRegexes).Trim();
            var splits = Regex.Split(sql, @"\s+", RegexOptions.Multiline);
            if (splits.Length == 0)
            {
                return null;
            }
            return splits[0].ToUpper();
        }

        public static SqlType GetSqlType(string sql, string sqlFirstKeyword = null)
        {
            if (sql.IsNullEmptyOrWhitespace()) throw new ArgumentNullException("sql");
            if (sqlFirstKeyword.IsNullEmptyOrWhitespace())
            {
                sqlFirstKeyword = GetFirstKeyword(sql);
            }
            if (sqlFirstKeyword != null && 
                (sqlFirstKeyword.Trim().ToLower().StartsWith("select") ||
                 sqlFirstKeyword.Trim().ToLower().StartsWith("with") || 
                 sqlFirstKeyword.Trim().ToLower().StartsWith("call") || 
                 sqlFirstKeyword.Trim().ToLower().StartsWith("exec") || 
                 sqlFirstKeyword.Trim().ToLower().StartsWith("execute") ||
                 sqlFirstKeyword.Trim().ToLower().StartsWith("show") ||
                 sqlFirstKeyword.Trim().ToLower().StartsWith("explain")
                 ))
            {
                return SqlType.Query;
            }
            else if (sqlFirstKeyword != null && (sqlFirstKeyword.ToLower().Trim().StartsWith("insert") ||
                                                 sqlFirstKeyword.ToLower().Trim().StartsWith("update") ||
                                                 sqlFirstKeyword.ToLower().Trim().StartsWith("delete") ||
                                                 sqlFirstKeyword.ToLower().Trim().StartsWith("merge") ||
                                                 sqlFirstKeyword.ToLower().Trim().StartsWith("call")))
            {
                return SqlType.Dml;
            }
            else
            {
                return SqlType.Ddl;
            }
        }

        public static DataTable CreateTable(this IDataRecord dataReader)
        {
            var resultsTable = new DataTable();
            for (var i = 0; i < dataReader.FieldCount; ++i)
            {
                var columnName = dataReader.GetName(i);
                var distinctColumnName = columnName;
                var index = 1;
                while (resultsTable.Columns.Contains(distinctColumnName))
                {
                    distinctColumnName = columnName + "_" + index;
                    ++index;
                }
                resultsTable.Columns.Add(distinctColumnName, dataReader.GetFieldType(i));
            }
            return resultsTable;
        }

        public static DataTable FetchDataTable(this IDataReader reader, int rowCount)
        {
            if (reader == null) throw new ArgumentNullException("reader");
            var tmpTable = reader.CreateTable();
            var rowValues = new object[reader.FieldCount];
            var rowNumber = 0;
            while (rowNumber < rowCount)
            {
                if (reader.Read())
                {
                    var newRow = tmpTable.NewRow();
                    reader.GetValues(rowValues);
                    newRow.ItemArray = rowValues;
                    tmpTable.Rows.Add(newRow);
                    ++rowNumber;
                }
                else
                {
                    break;
                }
            }
            return tmpTable;
        }

        public static void BuildSqlCommand([NotNull] this IDbCommand command, [NotNull] string sql, string parameterSymbol, params object[] parameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (sql == null) throw new ArgumentNullException("sql");

            command.CommandText = sql; 
            if (parameters != null && parameters.Length > 0)
            {
                for (var i = 0; i < parameters.Length; i++)
                {
                    var param = command.CreateParameter();
                    param.ParameterName = parameterSymbol + (i + 1);
                    param.Value = parameters[i];
                    command.Parameters.Add(param);
                }
            }
        }
    }
}
