using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.DatabaseExplorer;
using SqlEditor.QueryResults;
using SqlEditor.ScriptResults;
using SqlEditor.SqlHelpers;
using SqlEditor.SqlTextEditor;
using Utilities.Collections;
using Utilities.Forms.Dialogs;
using log4net;

namespace SqlEditor.RunMultipleFiles
{
    public partial class FrmRunMultipleFiles : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BindingList<SqlFileDetails> _sqlFiles = new BindingList<SqlFileDetails>();
        private readonly DatabaseConnection _databaseConnection;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly Dictionary<TextEditorControl, HighlightGroup> _errorHighlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();
        private readonly Dictionary<TextEditorControl, HighlightGroup> _queryHighlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();
        private readonly List<TextRange> _errorRanges = new List<TextRange>();
        private readonly List<TextRange> _queryRanges = new List<TextRange>();

        public FrmRunMultipleFiles(DatabaseConnection databaseConnection)
        {
            InitializeComponent();

            _databaseConnection = databaseConnection;
            _databaseConnection.PropertyChanged += DatabaseConnectionOnPropertyChanged;
            _bsSqlFiles.DataSource = _sqlFiles;
        }

        private void DatabaseConnectionOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            _utm.Tools["Run"].SharedProps.Enabled = _databaseConnection.IsConnected;
        }

        private async void Utm_ToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "Add Files":
                        AddFiles();
                        break;

                    case "Remove Files":
                        RemoveFiles();
                        break;

                    case "Move Up":
                        MoveFiles(1);
                        break;

                    case "Move Down":
                        MoveFiles(-1);
                        break;

                    case "Run":
                        await RunSqlFilesAsync();
                        break;

                    case "Stop":
                        _cancellationTokenSource.Cancel();
                        _utm.Tools["Stop"].SharedProps.Enabled = false;
                        break;

                    case "Clear":
                        ClearResults();
                        break;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error occured.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, "Error occured during last action", ex.Message);
            }
        }

        private async Task RunSqlFilesAsync()
        {
            // Get files to run
            var sqlFiles = _ugFiles.Rows.Select(x => (SqlFileDetails)x.ListObject).ToList();
            if (sqlFiles.Count == 0) throw new Exception("No files found to run");

            // Get options
            var useTransaction = ((StateButtonTool)_utm.Tools["Use Database Transaction"]).Checked;
            var continueOnError = ((StateButtonTool)_utm.Tools["Continue on Error"]).Checked;
            var runEntireFile = ((StateButtonTool)_utm.Tools["Run Each File as Single Statement"]).Checked;

            // Disable buttons
            foreach (var tool in _utm.Tools.Cast<ToolBase>().Where(x => x.Key != "Stop"))
            {
                tool.SharedProps.Enabled = false;
            }
            _utm.Tools["Stop"].SharedProps.Enabled = true;

            // Clear output
            _teResults.DoEditAction(new SelectWholeDocument());
            _teResults.DoEditAction(new Delete());
            
            // Remove highlight ranges
            _errorRanges.Clear();
            _queryRanges.Clear();

            

            // Reset all files
            foreach (var sqlFile in sqlFiles)
            {
                sqlFile.Status = "Pending";
                sqlFile.ElapsedTime = TimeSpan.Zero;
            }

            // Setup cancellation
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                using (var connection =
                        await
                        _databaseConnection.CreateNewConnectionAsync().WithCancellation(_cancellationTokenSource.Token))
                {
                    connection.OpenIfRequired();
                    IDbTransaction transaction = null;
                    if (useTransaction)
                    {
                        transaction = connection.BeginTransaction();
                    }

                    var stopwatch = new Stopwatch();
                    foreach (var sqlFile in sqlFiles)
                    {
                        // Start the timer
                        stopwatch.Restart();

                        // Read all text for file
                        var sqlFileText = File.ReadAllText(sqlFile.FileName);

                        // Split file text into individual SQL statements
                        List<string> sqlStatements = new List<string>();
                        if (runEntireFile)
                        {
                            sqlStatements.Add(sqlFileText);
                        }
                        else
                        {
                            var sqlSplitter = new SqlTextExtractor(_databaseConnection.DatabaseServer.SqlTerminators);
                            sqlStatements.AddRange(sqlSplitter.SplitSqlStatements(sqlFileText));
                        }

                        // Run each statement
                        sqlFile.Status = "Running";
                        foreach (var sqlStatement in sqlStatements)
                        {
                            await Task.Run(() => Thread.Sleep(3000));

                            try
                            {
                                _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                                var sqlFirstKeyword = SqlHelper.GetFirstKeyword(sqlStatement,
                                                                                _databaseConnection.DatabaseServer
                                                                                                   .BlockCommentRegex,
                                                                                _databaseConnection.DatabaseServer
                                                                                                   .LineCommentRegex);
                                var sqlType = SqlHelper.GetSqlType(sqlStatement, sqlFirstKeyword);

                                Task<SqlQueryResult> queryTask;
                                if (sqlType == SqlType.Query)
                                {
                                    // Execute SELECT query
                                    _log.Debug("Running query ...");
                                    queryTask = connection.ExecuteQueryKeepAliveAsync(transaction, sqlStatement, 100,
                                                                                      _cancellationTokenSource.Token);
                                    var results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                                    _log.Debug("Query complete.");
                                    SetResults(results.Result, sqlStatement);
                                    _log.Debug("Results bound.");
                                }
                                else
                                {
                                    // Execute DML or DDL query
                                    _log.Debug("Running non-query - it will use a transaction ...");
                                    queryTask = connection.ExecuteNonQueryTransactionAsync(transaction, sqlStatement,
                                                                                           _cancellationTokenSource
                                                                                               .Token);
                                    var results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                                    _log.Debug("Non-query complete.");
                                    var resultsTable = new DataTable();
                                    resultsTable.Columns.Add("Results", typeof(string));
                                    if (sqlType == SqlType.Dml && results.RowsAffected >= 0)
                                    {
                                        // Ex: Inserted x rows
                                        resultsTable.Rows.Add(string.Format("{0}{1}D {2} row{3}",
                                                                            sqlFirstKeyword.Trim().ToUpper(),
                                                                            sqlFirstKeyword.ToUpper()
                                                                                           .Trim()
                                                                                           .EndsWith("E")
                                                                                ? string.Empty
                                                                                : "E",
                                                                            results.RowsAffected.ToString("#,0"),
                                                                            results.RowsAffected != 1
                                                                                ? "s"
                                                                                : string.Empty));
                                    }
                                    else if (sqlType == SqlType.Dml)
                                    {
                                        resultsTable.Columns.Add("Results", typeof(int));
                                        resultsTable.Rows.Add(string.Format("{0} affected {1} row{2}",
                                                                            sqlFirstKeyword.Trim().ToUpper(),
                                                                            results.RowsAffected.ToString("#,0"),
                                                                            results.RowsAffected > 1
                                                                                ? "s"
                                                                                : string.Empty));
                                    }
                                    else
                                    {
                                        resultsTable.Rows.Add(string.Format("{0} successful",
                                                                            sqlFirstKeyword.Trim().ToUpper()));
                                    }
                                    SetResults(resultsTable, sqlStatement);
                                    _log.Debug("Results bound.");
                                }

                                // Set status
                                sqlFile.Status = "Success";
                            }
                            catch (OperationCanceledException)
                            {
                                sqlFile.Status = "Cancelled";
                                throw;
                            }
                            catch (Exception ex)
                            {
                                _log.Error("Error running query:");
                                _log.Error(sqlStatement);
                                _log.Error(ex.Message, ex);
                                SetResults(GetErrorMessageTable(ex), sqlStatement, true);
                                sqlFile.Status = "Failed";
                                // If we're not goingto continue, throw OperationCanceledException s it gets caught in the outside scope and doesn't report errors
                                if (!continueOnError) throw new OperationCanceledException();
                            }
                        } // end foreach sql stamenet

                        // Stop the timer
                        stopwatch.Stop();

                        // Set elapsed time
                        sqlFile.ElapsedTime = stopwatch.Elapsed;
                    }  // end foreach sql file

                    if (transaction != null)
                    {
                        transaction.Commit();
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _log.Debug("Operation cancelled.");
            }
            finally
            {
                MarkScriptMessages(_queryHighlightGroups, _queryRanges, Color.LightSkyBlue);
                MarkScriptMessages(_errorHighlightGroups, _errorRanges, Color.LightCoral);

                // Enable buttons
                foreach (var tool in _utm.Tools.Cast<ToolBase>().Where(x => x.Key != "Stop"))
                {
                    tool.SharedProps.Enabled = true;
                }
                _utm.Tools["Stop"].SharedProps.Enabled = false;
                _utm.Tools["Run"].SharedProps.Enabled = _databaseConnection.IsConnected;
            }
        }

        private void MarkScriptMessages(Dictionary<TextEditorControl, HighlightGroup> highlightGroups, IEnumerable<TextRange> highlightRanges, Color color)
        {
            try
            {
                if (!highlightGroups.ContainsKey(_teResults))
                    highlightGroups[_teResults] = new HighlightGroup(_teResults);
                var group = highlightGroups[_teResults];
                foreach (var textRange in highlightRanges)
                {
                    var m = new TextMarker(textRange.Offset, textRange.Length,
                                           TextMarkerType.SolidBlock, color, Color.Black);
                    @group.AddMarker(m);
                }
                _teResults.Refresh();
            }
            catch (Exception ex)
            {
                _log.Error("Error marking script messages");
                _log.Error(ex.Message, ex);
            }
        }


        private void SetResults(DataTable table, string sqlStatement, bool isError = false)
        {
            if (table != null && table.Columns.Count == 0)
            {
                table.Columns.Add("Results", typeof(string));
                table.Rows.Add("Command executed successfully");
                SetResults(table, sqlStatement, isError);
                return;
            }

            _log.Debug("Preparing results ...");
            var startOffset = _teResults.Document.TextLength;
            StringBuilder sb = new StringBuilder();
            sb.Append(sqlStatement);
            _queryRanges.Add(new TextRange(startOffset, sb.Length));
            sb.Append(Environment.NewLine);
            sb.Append(table == null ? string.Empty : table.AsFormattedString());
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            _teResults.AppendTextAtEnd(sb.ToString());

            if (isError)
            {
                _errorRanges.Add(new TextRange(startOffset, sb.Length));
            }
            _teResults.Refresh();
        }

        private static DataTable GetErrorMessageTable(Exception e)
        {
            var errorsTable = new DataTable();
            errorsTable.Columns.Add("Error Message", typeof(string));
            var errorText = e.Message;
            errorsTable.Rows.Add(new object[] { errorText });
            return errorsTable;
        }

        private void MoveFiles(int offset)
        {
            var selectedRows = _ugFiles.Selected.Rows.Cast<UltraGridRow>().ToList();
            if (selectedRows.Count == 0) throw new Exception("Nothing is selected");

            try
            {
                _ugFiles.BeginUpdate();
                
                if (offset > 0 && selectedRows.Select(x => x.Index).Min() == 0)
                {
                    throw new Exception("One of the selected files is already at the top");
                }
                else if (offset < 0 && selectedRows.Select(x => x.Index).Max() == _sqlFiles.Count - 1)
                {
                    throw new Exception("One of the selected files is already at the bottom");
                }
            
                foreach (var selectedRow in selectedRows)
                {
                    _ugFiles.Rows.Move(selectedRow, selectedRow.Index - offset);
                }
            }
            finally
            {
                _ugFiles.EndUpdate(true);
            }
        }

        private void RemoveFiles()
        {
            var selectedRowsCount = _ugFiles.Selected.Rows.Count;
            if (selectedRowsCount == 0)
            {
                throw new Exception("Noting selected to delete");
            }
            var dialogResult = Dialog.ShowYesNoDialog(Application.ProductName,
                                                      "Are you sure you want to remove " +
                                                      selectedRowsCount.ToString("#,0") + " file" +
                                                      (selectedRowsCount > 1 ? "s" : string.Empty) + "?", string.Empty, TaskDialogStandardIcon.Information);
            if (dialogResult != TaskDialogResult.Yes)
            {
                return;
            }
            _ugFiles.DeleteSelectedRows(false);
        }

        private void AddFiles()
        {
            string[] files;
            if (
                Dialog.ShowOpenFilesDialog("Select SQL files", out files,
                                           new[]
                                               {
                                                   new CommonFileDialogFilter("SQL file", "*.sql"),
                                                   new CommonFileDialogFilter("All files", "*")
                                               }) !=
                CommonFileDialogResult.OK)
            {
                return;
            }

            _sqlFiles.AddRange(files.Where(File.Exists).Select(x => new SqlFileDetails {FileName = x}));
        }

        private void ClearResults()
        {
            _teResults.Text = string.Empty;
            if (_errorHighlightGroups.ContainsKey(_teResults))
            {
                var group = _errorHighlightGroups[_teResults];
                group.ClearMarkers();
            }
            _errorRanges.Clear();

            if (_queryHighlightGroups.ContainsKey(_teResults))
            {
                var group = _queryHighlightGroups[_teResults];
                group.ClearMarkers();
            }
            _queryRanges.Clear();
            _teResults.Refresh();
        }
    }
}
