using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using SqlEditor.Databases;
using SqlEditor.SqlParser;
using SqlEditor.SqlParser.Interfaces;
using Utilities.Text;
using log4net;

namespace SqlEditor.Intellisense
{
    public enum StatementContext
    {
        None = 0,
        Column = 1,
        Object = 2,
        Schema = 4
    }

    public class CompleteWordCompletionProvider : ICompletionDataProvider
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static Regex _validFullyQualifiedIdentifier, _validIdentifierCharacters;
        private readonly IntelisenseData _intellisenseData;
        private readonly int _tableIconIndex;
        private readonly int _viewIconIndex;
        private readonly int _mviewIconIndex;
        private readonly int _columnIconIndex;
        private readonly ImageList _imageList;
        private readonly string[] _queryDelimiters = { "/", ";", "\r\n\r\n" };
        private readonly Regex _columnContextRegexBeginning;
        private readonly Regex _tableContextRegexBeginning;
        private readonly Regex _columnContextRegex;
        private readonly Regex _tableContextRegex;

        public CompleteWordCompletionProvider(ImageList imageList, IntelisenseData intellisenseData, int tableIconIndex, int viewIconIndex, int mviewIconIndex, int columnIconIndex)
            : this()
        {
            _imageList = imageList;
            _intellisenseData = intellisenseData;
            _tableIconIndex = tableIconIndex;
            _viewIconIndex = viewIconIndex;
            _mviewIconIndex = mviewIconIndex;
            _columnIconIndex = columnIconIndex;
        }

        public CompleteWordCompletionProvider()
        {
            if (_validFullyQualifiedIdentifier == null)
            {
                _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*\$\#]+[\r\n\f]*$", RegexOptions.Compiled | RegexOptions.Multiline);
            }

            if (_validIdentifierCharacters == null)
            {
                _validIdentifierCharacters = new Regex(@"[a-zA-Z_0-9\$\#]+$", RegexOptions.Compiled | RegexOptions.Multiline);
            }

            if (_columnContextRegexBeginning == null)
            {
                _columnContextRegexBeginning = new Regex(@"^(SELECT|WHERE|BY|SET|,)\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            }

            if (_tableContextRegexBeginning == null)
            {
                _tableContextRegexBeginning = new Regex(@"^(FROM|MERGE|JOIN|UPDATE|DELETE|INTO)\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);
            }

            if (_columnContextRegex == null)
            {
                _columnContextRegex = new Regex(@"(SELECT|WHERE|ORDER\s+BY|GROUP\s+BY|SET)\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.RightToLeft);
            }

            if (_tableContextRegex == null)
            {
                _tableContextRegex = new Regex(@"(FROM|MERGE|JOIN|UPDATE|DELETE|INTO)\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.RightToLeft);
            } 
        }

        public ImageList ImageList
        {
            get
            {
                return _imageList;
            }
        }

        public string PreSelection
        {
            get
            {
                return null;
            }
        }

        public int DefaultIndex
        {
            get
            {
                return -1;
            }
        }

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }
            return CompletionDataProviderKeyResult.InsertionKey;
        }

        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            if (textArea.Document.TextLength == 0)
            {
                return false;
            }

            var startOffset = Math.Max(0, textArea.Document.PositionToOffset(textArea.Caret.Position) - 1);
            while (startOffset >= 0 && _validIdentifierCharacters.IsMatch(textArea.Document.GetText(startOffset, 1)))
            {
                --startOffset;                
            }
            ++startOffset;

            var endOffset = textArea.Document.PositionToOffset(textArea.Caret.Position);
            while (endOffset < textArea.Document.TextLength && _validIdentifierCharacters.IsMatch(textArea.Document.GetText(endOffset, 1)))
            {    
                ++endOffset;
            }
                        
            var posn = textArea.Document.OffsetToPosition(startOffset);
            textArea.Document.UndoStack.StartUndoGroup();
            if (endOffset - startOffset > 0)
            {
                textArea.Document.Remove(startOffset, endOffset - startOffset);
            }
            textArea.Caret.Position = posn; // new TextLocation(pos.Column, pos.Line);
            bool returnValue = data.InsertAction(textArea, key);
            textArea.Document.UndoStack.EndUndoGroup();

            return returnValue;

        }

        //public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        //{
        //    string query = GetQueryText(textArea);
        //    string alias = GetPreviousWord(textArea, query);
        //    var st = new SQLStatement(query, textArea.Caret.Offset, SQLStatement.SearchOrder.asc);
        //    string objectName = st.GetAliasTableName(alias);
            
        //    var completionList = new List<DefaultCompletionData>();
            
        //    var schemaName = _intellisenseData.AllSchemas.Where(s => s.Name == objectName).FirstOrDefault();
        //    if (schemaName != null)
        //    {
        //        completionList.AddRange(schemaName.Tables.OrderBy(t => t.Name).Select(t => new DefaultCompletionData(t.Name, string.Empty, 15)).ToList());
        //        return completionList.ToArray();
        //    }

        //    var tableName = _intellisenseData.AllObjects.Where(o => o.Name == objectName).FirstOrDefault();
        //    if (tableName != null)
        //    {
        //        completionList.AddRange(tableName.Columns.OrderBy(c => c.Name).Select(c => new DefaultCompletionData(c.Name, string.Empty, 14)).ToList());
        //        return completionList.ToArray();
        //    }

        //    return null;
        //}

        private StatementContext GetStatementContext(string query, int position)
        {
            //string[] words = Regex.Split(query, @"\s");            
            //Array.Reverse(words);
            //var reverseQuery = string.Join(" ", words);
            //int newPosition = query.Length - position;

            //int maxColumnContextMatch = -1;
            //var match = _columnContextRegex.Match(query, 0, position);
            //while (match.Success)
            //{
            //    maxColumnContextMatch = match.Index;
            //    int startPosition = match.Index + 1;
            //    if (startPosition > position)
            //    {
            //        break;
            //    }
            //    match = _columnContextRegex.Match(query, startPosition, position - startPosition);
            //}

            var maxColumnContextMatch = -1;
            var match = _columnContextRegex.Match(query);
            if (match.Success)
            {
                maxColumnContextMatch = match.Index;
            }

            //int maxTableContextMatch = -1;
            //match = _tableContextRegex.Match(query, 0, position);
            //while (match.Success)
            //{
            //    maxTableContextMatch = match.Index;
            //    int startPosition = match.Index + 1;
            //    if (startPosition > position)
            //    {
            //        break;
            //    }
            //    match = _tableContextRegex.Match(query, startPosition, position - startPosition);
            //}

            var maxTableContextMatch = -1;
            match = _tableContextRegex.Match(query, 0, position);
            if (match.Success)
            {
                maxTableContextMatch = match.Index;
            }

            if (maxColumnContextMatch > maxTableContextMatch)
            {
                return StatementContext.Column;
            }
            return StatementContext.Object;            
        }


        private string GetPreviousWord(TextArea textArea, int queryStartPosition)
        {
            string queryUpToCursor = textArea.Document.TextContent.Substring(queryStartPosition, textArea.Caret.Offset - queryStartPosition);
            var q = Regex.Replace(queryUpToCursor, @"\s+", " ");
            var match = _validFullyQualifiedIdentifier.Match(q);
            if (match.Success)
            {
                return match.Value;
            }
            return string.Empty;

            //StringBuilder sb = new StringBuilder(queryUpToCursor.Length);
            //for (int i = queryUpToCursor.Length - 1; i >= 0; i--)
            //{
            //    if (_splitSqlStatementRegex.IsMatch(queryUpToCursor[i].ToString()))
            //    {
            //        sb.Append(queryUpToCursor[i]);
            //    }
            //}
            //return StringHelper.Reverse(sb.ToString());
            //string[] words = _splitSqlStatementRegex.Split(queryUpToCursor);
            //var nonEmptyWoprds = words.Where(w => w != string.Empty).ToList();
            //string lastWord = string.Empty;
            //if (nonEmptyWoprds.Count > 0)
            //{
            //    lastWord = nonEmptyWoprds[nonEmptyWoprds.Count - 1];
            //}
            //return lastWord;
        }

        private string GetQueryText(TextArea textArea)
        {
            string selectedText = textArea.SelectionManager.SelectedText.Trim();
            if (!selectedText.IsNullEmptyOrWhitespace())
            {
                return selectedText;
            }
            
            var pos = textArea.Caret.Position;
            int position = textArea.Document.PositionToOffset(pos);
            int end = StringHelper.IndexOfNextNonQualifiedCharacter(textArea.Document.TextContent, position,
                                                              _queryDelimiters, "'", "\"");

            int start = StringHelper.IndexOfPreviousNonQualifiedCharacter(textArea.Document.TextContent, position,
                                                                          _queryDelimiters, "'", "\"");


            var query = textArea.Document.GetText(start, end - start);
            return query;
        }

        private int GetQueryStartPosition(TextArea textArea)
        {
            string selectedText = textArea.SelectionManager.SelectedText.Trim();
            if (!string.IsNullOrEmpty(selectedText))
            {
                var startLocation = textArea.SelectionManager.SelectionCollection[0].StartPosition;
                int startPosition = textArea.Document.PositionToOffset(startLocation);
                return startPosition;
            }

            var pos = textArea.Caret.Position;
            int position = textArea.Document.PositionToOffset(pos);
            int start = StringHelper.IndexOfPreviousNonQualifiedCharacter(textArea.Document.TextContent, position,
                                                                          _queryDelimiters, "'", "\"");
            return start;
        }
        
        public ICompletionData[] GenerateCompletionData(string fileName, TextArea qcTextEditor, char charTyped)
        {
            var queryStart = -1;
            string query = null, previousWord = null;
            var columns = new List<string>();
            var tables = new List<string>();
            var views = new List<string>();
            var mviews = new List<string>();
            try
            {
                var completionList = new List<DefaultCompletionData>();
                queryStart = GetQueryStartPosition(qcTextEditor);
                if (IsInsideQuotedString(qcTextEditor, queryStart)) return null;
                query = GetQueryText(qcTextEditor);
                previousWord = GetPreviousWord(qcTextEditor, queryStart).TrimStart().ToUpper();
                
                IStatement statement;
                try
                {
                    var statements = ParserFactory.Execute(query);
                    if (statements != null && statements.Count > 0 && statements[0] != null)
                    {
                        statement = statements[0];
                    }
                    else
                    {                        
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _log.DebugFormat("Error parsing SQL statement: {0}.", query);
                    _log.Debug(ex.Message, ex);
                    return null;
                }

                // Context: List of columns
                if (_columnContextRegexBeginning.IsMatch(previousWord) && statement.Tables.Count > 0)
                {
                    // Show column list
                    foreach (var table in statement.Tables)
                    {
                        var objectName = table.Name;
                        var schema = GetSchemaOrDefault(table.Schema);
                        var schemaName = schema.Name;
                        var cols = _intellisenseData.AllColumns.Where(
                                c => c.Parent != null &&
                                String.Equals(c.Parent.Name, objectName, StringComparison.CurrentCultureIgnoreCase) &&
                                c.Parent.Parent != null &&
                                String.Equals(c.Parent.Parent.Name, schemaName, StringComparison.CurrentCultureIgnoreCase)
                           ).Select(c => c.Name).ToList();
                        columns.AddRange(cols);
                    }
                }
                // Context: List of objects
                else if (_tableContextRegexBeginning.IsMatch(previousWord))
                {
                    // Show table list from a current schema                    
                    tables = _intellisenseData.CurrentSchema.Tables.Select(x => x.Name).ToList();
                    views = _intellisenseData.CurrentSchema.Views.Select(x => x.Name).ToList();
                    mviews = _intellisenseData.CurrentSchema.MaterializedViews.Select(x => x.Name).ToList();
                }
                // Context: any; 
                else
                {
                    var dotSplits = previousWord.Split('.');
                    if (dotSplits.Length == 3)
                    {
                        // Format: Schema.Object.ColumnList
                        var schemaName = dotSplits[0].TrimStart().ToUpper();
                        var tableName = dotSplits[1].ToUpper();
                        var columnName = dotSplits[2].TrimEnd().ToUpper();

                        columns = _intellisenseData.AllColumns.Where(
                                c => c.Name.StartsWith(columnName, StringComparison.InvariantCultureIgnoreCase) &&
                                c.Parent != null &&
                                c.Parent.Name.ToUpper() == tableName &&
                                c.Parent.Parent != null &&
                                c.Parent.Parent.Name.ToUpper() == schemaName
                           ).Select(c => c.Name).ToList();
                    }
                    else if (dotSplits.Length == 2)
                    {
                        // Format: alias.ColumnList 
                        var schemaName = _intellisenseData.CurrentSchema.Name.Trim().ToUpper();
                        var aliasName = dotSplits[0].TrimStart().ToUpper();
                        var columnName = dotSplits[1].TrimEnd().ToUpper();
                        var table = statement.Tables.FirstOrDefault(t => t.Alias != null && t.Alias.Name != null && t.Alias.Name.Trim().ToUpper() == aliasName);
                        if (table != null)
                        {
                            // Format: alias.ColumnList 
                            table.Schema = string.IsNullOrEmpty(table.Schema) ? schemaName : table.Schema;
                            columns = _intellisenseData.AllColumns.Where(
                                    c => c.Name.StartsWith(columnName, StringComparison.InvariantCultureIgnoreCase) &&
                                    c.Parent != null &&
                                    String.Equals(c.Parent.Name, table.Name, StringComparison.CurrentCultureIgnoreCase) &&
                                    c.Parent.Parent != null &&
                                    String.Equals(c.Parent.Parent.Name, table.Schema, StringComparison.CurrentCultureIgnoreCase)).Select(c => c.Name).OrderBy(c => c).ToList();
                        }
                        else
                        {
                            // Format: Schema.ObjectName                            
                            schemaName = dotSplits[0].TrimStart().ToUpper();
                            var schema = GetSchema(schemaName);
                            if (schema != null)
                            {
                                var tableName = dotSplits[1].TrimEnd().ToUpper();
                                tables =
                                    schema.Tables.Where(
                                        x => x.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.Name)
                                        .ToList();
                                views =
                                    schema.Views.Where(
                                        x => x.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.Name)
                                        .ToList();
                                mviews =
                                    schema.MaterializedViews.Where(
                                        x => x.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.Name)
                                        .ToList();
                            }

                        }
                    }
                    else
                    {
                        // Format: Column
                        var context = GetStatementContext(query, Math.Min(query.Length, qcTextEditor.Caret.Offset - queryStart));
                        if (context == StatementContext.Column && statement.Tables.Count > 0)
                        {
                            // Show column list from a single table
                            var table = statement.Tables[0];

                            if (table.Schema == null)
                            {
                                table.Schema = _intellisenseData.AllObjects
                                    .Where(x => string.Equals(x.Name, table.Name, StringComparison.InvariantCultureIgnoreCase))
                                    .Select(x => x.Parent.Name)
                                    .FirstOrDefault();
                            }
                            var schema = GetSchema(table.Schema);
                            if (schema != null)
                            {
                                var tbl =
                                    schema.Tables.FirstOrDefault(x => x.Name.ToUpper() == table.Name.Trim().ToUpper());
                                if (tbl != null)
                                {
                                    columns =
                                        tbl.Columns.Where(
                                            c =>
                                                c.Name.StartsWith(previousWord.Trim().ToUpper(),
                                                    StringComparison.InvariantCultureIgnoreCase))
                                            .Select(x => x.Name)
                                            .ToList();
                                }
                                else
                                {
                                    var view =
                                        schema.Views.FirstOrDefault(x => x.Name.ToUpper() == table.Name.Trim().ToUpper());
                                    if (view != null)
                                    {
                                        columns =
                                            view.Columns.Where(
                                                c =>
                                                    c.Name.StartsWith(previousWord.Trim().ToUpper(),
                                                        StringComparison.InvariantCultureIgnoreCase))
                                                .Select(x => x.Name)
                                                .ToList();
                                    }
                                    else
                                    {
                                        var mview =
                                            schema.MaterializedViews.FirstOrDefault(
                                                x => x.Name.ToUpper() == table.Name.Trim().ToUpper());
                                        if (mview != null)
                                        {
                                            columns =
                                                mview.Columns.Where(
                                                    c =>
                                                        c.Name.StartsWith(previousWord.Trim().ToUpper(),
                                                            StringComparison.InvariantCultureIgnoreCase))
                                                    .Select(x => x.Name)
                                                    .ToList();
                                        }
                                    }
                                }
                            }
                        }
                        // Format: table/object
                        else
                        {
                            var schemaName = _intellisenseData.CurrentSchema.Name.Trim().ToUpper();
                            var schema = GetSchema(schemaName);
                            if (schema != null)
                            {
                                var tableName = previousWord.Trim().ToUpper();
                                tables =
                                    schema.Tables.Where(
                                        x => x.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.Name)
                                        .ToList();
                                views =
                                    schema.Views.Where(
                                        x => x.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.Name)
                                        .ToList();
                                mviews =
                                    schema.MaterializedViews.Where(
                                        x => x.Name.StartsWith(tableName, StringComparison.InvariantCultureIgnoreCase))
                                        .Select(x => x.Name)
                                        .ToList();
                            }
                        }
                    }
                }

                completionList.AddRange(columns.Select(c => new DefaultCompletionData(c, string.Empty, _columnIconIndex)));
                completionList.AddRange(tables.Select(c => new DefaultCompletionData(c, string.Empty, _tableIconIndex)));
                completionList.AddRange(views.Select(c => new DefaultCompletionData(c, string.Empty, _viewIconIndex)));
                completionList.AddRange(mviews.Select(c => new DefaultCompletionData(c, string.Empty, _mviewIconIndex)));
                
                return completionList.OrderBy(x => x.Text).Cast<ICompletionData>().ToArray();
            }
            catch (Exception ex)
            {
                _log.Error("Error completing word.");
                _log.ErrorFormat("Query: {0}", query ?? "NULL");
                _log.ErrorFormat("Query Start: {0}", queryStart);
                _log.ErrorFormat("Previous Word: {0}", previousWord ?? "NULL");                
                _log.Error(ex.Message, ex);
                return  null;
            }
        }

        private static bool IsInsideQuotedString(TextArea qcTextEditor, int queryStart)
        {
            var isQuoteOpen = false;
            for (var i = queryStart; i < qcTextEditor.Caret.Offset; i++)
            {
                var character = qcTextEditor.Document.GetText(i, 1);
                if (character == "'" || character == "\"" || character == "`")
                {
                    isQuoteOpen = !isQuoteOpen;
                }
            }
            return isQuoteOpen;
        }

        private Schema GetSchemaOrDefault(string schemaName)
        {
            var schema = _intellisenseData.CurrentSchema;
            if (!string.IsNullOrEmpty(schemaName))
            {
                schema = _intellisenseData.AllSchemas.FirstOrDefault(x => x.Name.ToUpper() == schemaName.Trim().ToUpper());
            }

            if (schema == null && _intellisenseData.CurrentSchema == null)
            {
                throw new Exception("Default schema in null.");
            }
            else if (schema == null)
            {
                schema = _intellisenseData.CurrentSchema;
            }
            return schema;
        }

        private Schema GetSchema(string schemaName)
        {
            return _intellisenseData.AllSchemas.FirstOrDefault(x => x.Name.ToUpper() == schemaName.Trim().ToUpper());
        }
    }
}
