using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ICSharpCode.TextEditor;
using JetBrains.Annotations;
using SqlEditor.Databases;
using Utilities;
using Utilities.Collections;

namespace SqlEditor.SqlHelpers
{
    public class SqlTextExtractor
    {
        private readonly DatabaseServer _databaseServer;
        private static readonly Regex _spaceRegex = new Regex(@"\s+", RegexOptions.Compiled);
        private static readonly Regex _trimStartRegex = new Regex(@"^[\s\r\n\f]+", RegexOptions.Multiline | RegexOptions.Compiled);
        private static readonly Regex _trimEndRegex = new Regex(@"[\s\r\n\f]+$", RegexOptions.Multiline | RegexOptions.Compiled);
        public IList<string> SqlTerminators { get; set; }
        public IList<string> LiteralQualifiers { get; set; }

        public SqlTextExtractor()
        {
            LiteralQualifiers = new List<string> {"\"", "'", "`"};
        }

        public SqlTextExtractor([NotNull] IEnumerable<string> sqlTerminators, [NotNull] DatabaseServer databaseServer)
            : this()
        {
            if (sqlTerminators == null) throw new ArgumentNullException("sqlTerminators");
            if (databaseServer == null) throw new ArgumentNullException("databaseServer");
            _databaseServer = databaseServer;
            SqlTerminators = new List<string>();
            SqlTerminators.AddRange(sqlTerminators);
        }

        public string GetSqlText(string text, int currentCaretPosition)
        {
            var sqlTerminatorsArray = SqlTerminators.ToArray();
            var end = StringHelper.IndexOfNextNonQualifiedCharacter(text, currentCaretPosition,
                                                                    sqlTerminatorsArray, "'", "\"");

            var start = StringHelper.IndexOfPreviousNonQualifiedCharacter(text, currentCaretPosition,
                                                                          sqlTerminatorsArray, "'", "\"");


            var query = text.Substring(start, end - start);
            return query.Trim();
        }

        public int GetQueryStartPosition(TextArea textArea, int currentCaretPosition)
        {
            string selectedText = textArea.SelectionManager.SelectedText.Trim();
            if (!string.IsNullOrEmpty(selectedText))
            {
                var startLocation = textArea.SelectionManager.SelectionCollection[0].StartPosition;
                int startPosition = textArea.Document.PositionToOffset(startLocation);
                return startPosition;
            }

            var start = StringHelper.IndexOfPreviousNonQualifiedCharacter(textArea.Document.TextContent,
                                                                          currentCaretPosition,
                                                                          SqlTerminators.ToArray(), "'", "\"");
            return start;
        }

        public string GetPreviousWord(string text, int queryStartPosition, int currentCursorPosition,
                                         Regex validFullyQualifiedIdentifier)
        {
            var queryUpToCursor = text.Substring(queryStartPosition, currentCursorPosition - queryStartPosition);
            var q = _spaceRegex.Replace(queryUpToCursor, " ");
            var match = validFullyQualifiedIdentifier.Match(q);
            return match.Success ? match.Value : string.Empty;
        }

        public IList<string> SplitSqlStatements(string text)
        {
            var list = new List<string>();
            if (text == null) throw new ArgumentNullException("text");
            if (string.IsNullOrWhiteSpace(text) || text.Length == 0)
            {
                return list;
            }

            text = _databaseServer.InlineCommentRegex.Replace(text, string.Empty);
            text = _databaseServer.BlockCommentRegex.Replace(text, string.Empty);

            var sqlTerminators = SqlTerminators.ToArray();
            var literalQualifiers = LiteralQualifiers.ToArray();
            string sql = null;
            int start = 0, end = text.Length;

            while (start >= 0 && start < text.Length && end != -1)
            {
                end = StringHelper.IndexOfNextNonQualifiedCharacter(text, start, sqlTerminators, literalQualifiers);
                if (end != -1)
                {
                    sql = text.Substring(start, end - start).Trim();
                    sql = _trimStartRegex.Replace(sql, string.Empty);
                    sql = _trimEndRegex.Replace(sql, string.Empty);
                    if (sql.Length > 0)
                    {
                        list.Add(sql);
                    }
                    start = end + 1;
                }
            }

            if (start >= 0 && start < text.Length)
            {
                end = text.Length;
                sql = _trimStartRegex.Replace(text.Substring(start, end - start).Trim(), string.Empty);
                sql = _trimEndRegex.Replace(sql, string.Empty);
                if (sql.Length > 0)
                {
                    list.Add(sql);
                }
            }
            return list;
        }
    }
}
