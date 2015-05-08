using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using Infragistics.Win.UltraWinToolbars;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.QueryResults;
using SqlEditor.SqlHelpers;
using SqlEditor.SqlTextEditor;
using Utilities.Forms.Dialogs;
using log4net;

namespace SqlEditor.ScriptResults
{
    public partial class UcScriptResults : UserControl, INotifyPropertyChanged
    {
        #region Fields
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly LabelTool _timingLabelTool;
        private readonly StateButtonTool _flagTool;
        private readonly ControlContainerTool _activityIndicatorTool;
        private readonly ButtonTool _stopTool;
        private readonly ButtonTool _commitTool;
        private readonly ButtonTool _rollbackTool;
        private readonly ButtonTool _copyTool;
        private readonly ButtonTool _saveTool;
        private readonly ButtonTool _clearTool;
        
        private readonly DatabaseConnection _databaseConnection;
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IDbCommand _command;
        private readonly Stopwatch _stopwatch;
        private int _maxResults;
        private bool _isPinned;
        private bool _isBusy;
        private string _sql;
        private IList<string> _sqlStatements;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Dictionary<TextEditorControl, HighlightGroup> _errorHighlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();
        private readonly Dictionary<TextEditorControl, HighlightGroup> _queryHighlightGroups = new Dictionary<TextEditorControl, HighlightGroup>();
        private readonly List<TextRange> _errorRanges = new List<TextRange>();
        private readonly List<TextRange> _queryRanges = new List<TextRange>();

        #endregion


        #region Properties
        public int MaxResults
        {
            get { return _maxResults; }
            set
            {
                if (value == _maxResults) return;
                _maxResults = value;
                OnPropertyChanged("MaxResults");
            }
        }

        public bool IsPinned
        {
            get { return _isPinned; }
            protected set
            {
                if (value.Equals(_isPinned)) return;
                _isPinned = value;
                OnPropertyChanged("IsPinned");
                _flagTool.Checked = IsPinned;
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value.Equals(_isBusy)) return;
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public string Sql
        {
            get { return _sql; }
            set
            {
                if (value == _sql) return;
                _sql = value;
                OnPropertyChanged("Sql");
            }
        }
        #endregion

        
        public UcScriptResults()
        {
            InitializeComponent();
            _stopwatch = new Stopwatch();
            _isPinned = false;
            _sql = string.Empty;
            _isBusy = false;
            _maxResults = 100;

            _teScriptResults.IsReadOnly = true;
            
            _timingLabelTool = (LabelTool)_utm.Tools["Timing"];            
            _flagTool = (StateButtonTool)_utm.Tools["Flag"];
            _activityIndicatorTool = (ControlContainerTool)_utm.Tools["Activity Indicator"];
            _stopTool = (ButtonTool)_utm.Tools["Stop"];
            _commitTool = (ButtonTool)_utm.Tools["Commit"];
            _rollbackTool = (ButtonTool)_utm.Tools["Rollback"];
            _copyTool = (ButtonTool)_utm.Tools["Copy"];
            _saveTool = (ButtonTool)_utm.Tools["Save"];
            _clearTool = (ButtonTool)_utm.Tools["Clear"];

            RefreshUserInterface();
        }

        public UcScriptResults(DatabaseConnection databaseConnection)
            : this()
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            _databaseConnection = databaseConnection;
            _databaseConnection.PropertyChanged += (sender, args) => RefreshUserInterface();
        }

        public async void RunQueryAsync(IList<string> sqlStatements, int maxResults)
        {
            if (sqlStatements == null)
            {
                throw new ArgumentNullException("sqlStatements");
            }
            else if (IsBusy)
            {
                throw new InvalidOperationException("Previous operation is still in progress");
            }
            else if (_databaseConnection == null || !_databaseConnection.IsConnected)
            {
                throw new InvalidOperationException("Database connection is closed");
            }
            else if (maxResults <= 0)
            {
                throw new ArgumentException("Max results cannot be less or equal to 0", "maxResults");
            }

            _sqlStatements = sqlStatements;
            Sql = string.Join(Environment.NewLine + Environment.NewLine, _sqlStatements);
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("Starting to run SQL:");
                _log.Debug(Sql);
            }

            if (_transaction != null)
            {
                var dialogResult = Dialog.ShowYesNoDialog(Application.ProductName,
                                                          "Previous transaction is still in progress. Are you sure you want to continue?",
                                                          "Selecting Yes will rollback previous transaction.");
                if (dialogResult != TaskDialogResult.Yes) return;
            }
            
            var needsTransaction = true;
            try
            {
                InitalizeOperationStart();
                MaxResults = maxResults;
                UpdateQueryElapsedTime();

                // Create a new connection
                _connection = await GetConnectionAsync().WithCancellation(_cancellationTokenSource.Token);
                await _connection.OpenIfRequiredAsync();

                // If any SQL not a SELECT query, open transaction
                needsTransaction = !_databaseConnection.AutoCommit && sqlStatements.Any(x => SqlHelper.GetSqlType(Sql,
                                                                               SqlHelper
                                                                                   .GetFirstKeyword(
                                                                                       x,
                                                                                       _databaseConnection
                                                                                           .DatabaseServer
                                                                                           .BlockCommentRegex,
                                                                                       _databaseConnection
                                                                                           .DatabaseServer
                                                                                           .LineCommentRegex)) != SqlType.Query);
                if (needsTransaction)
                {
                    _log.Debug("Opening transaction ...");
                    _connection.OpenIfRequired();
                    _transaction = _connection.BeginTransaction();
                }

                var individualSqlStopwatch = new Stopwatch();
                foreach (var sqlStatement in sqlStatements)
                {
                    // Create new command
                    _command = _connection.CreateCommand();

                    try
                    {
                        var sqlFirstKeyword = SqlHelper.GetFirstKeyword(sqlStatement, _databaseConnection.DatabaseServer.BlockCommentRegex,
                                                                        _databaseConnection.DatabaseServer.LineCommentRegex);
                        var sqlType = SqlHelper.GetSqlType(Sql, sqlFirstKeyword);
                        Task<SqlQueryResult> queryTask;
                        if (sqlType == SqlType.Query)
                        {
                            // Execute SELECT query
                            _log.Debug("Running query ...");
                            //queryTask = _connection.ExecuteQueryKeepAliveAsync(sqlStatement, maxResults, _cancellationTokenSource.Token);
                            individualSqlStopwatch.Restart();
                            queryTask = _command.ExecuteQueryKeepAliveAsync(sqlStatement, maxResults, _cancellationTokenSource.Token);
                            var results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                            individualSqlStopwatch.Stop();
                            _log.Debug("Query complete.");
                            SetResults(results.Result, sqlStatement, individualSqlStopwatch.Elapsed);
                            _log.Debug("Results bound.");
                        }
                        else
                        {
                            // Execute DML or DDL query
                            _log.Debug("Running non-query - it will use a transaction ...");
                            //queryTask = _connection.ExecuteNonQueryTransactionAsync(_transaction, sqlStatement, _cancellationTokenSource.Token);
                            individualSqlStopwatch.Restart();
                            queryTask = _command.ExecuteNonQueryTransactionAsync(_transaction, sqlStatement, _cancellationTokenSource.Token);
                            var results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                            individualSqlStopwatch.Stop();
                            _log.Debug("Non-query complete.");
                            var resultsTable = new DataTable();
                            resultsTable.Columns.Add("Results (" + ((int)individualSqlStopwatch.ElapsedMilliseconds).ToString("#,0") + " ms)", typeof(string));
                            if (sqlType == SqlType.Dml && results.RowsAffected >= 0)
                            {
                                // Ex: Inserted x rows
                                resultsTable.Rows.Add(string.Format("{0}{1}D {2} row{3}", sqlFirstKeyword.Trim().ToUpper(), sqlFirstKeyword.ToUpper().Trim().EndsWith("E") ? string.Empty : "E", results.RowsAffected.ToString("#,0"), results.RowsAffected != 1 ? "s" : string.Empty));
                            }
                            else if (sqlType == SqlType.Dml)
                            {
                                //resultsTable.Columns.Add("Results", typeof(int));
                                resultsTable.Rows.Add(string.Format("{0} affected {1} row{2}", sqlFirstKeyword.Trim().ToUpper(), results.RowsAffected.ToString("#,0"), results.RowsAffected > 1 ? "s" : string.Empty));
                            }
                            else
                            {
                                resultsTable.Rows.Add(string.Format("{0} successful", sqlFirstKeyword.Trim().ToUpper()));
                            }
                            SetResults(resultsTable, sqlStatement, individualSqlStopwatch.Elapsed, false, false);
                            _log.Debug("Results bound.");
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        individualSqlStopwatch.Stop();
                        _log.Error("Error running query:");
                        _log.Error(sqlStatement);
                        _log.Error(ex.Message, ex);
                        SetResults(GetErrorMessageTable(ex), sqlStatement, individualSqlStopwatch.Elapsed, true);
                    }
                }

                if (_transaction == null)
                {
                    await CloseCurrentConnectionAsync();
                }
            }
            catch (OperationCanceledException)
            {
                _log.Info("Operation canceled.");
                CleanupAfterQueryAbort("Operation canceled");

                //// If a forcibly canceled task is still running, create a continuation task to close the database connection after the query is done
                //if (queryTask != null && !queryTask.IsCompleted)
                //{
                //    var connection = _connection;
                //    var transaction = _transaction;
                //    // ReSharper disable CSharpWarnings::CS4014
                //    queryTask.ContinueWith(task => CloseConnectionAsync(connection, transaction));
                //    // ReSharper restore CSharpWarnings::CS4014
                //}
            }
            catch (Exception ex)
            {
                CleanupAfterQueryAbort("Operation failed");
                SetResults(GetErrorMessageTable(ex), string.Empty, TimeSpan.Zero, true);

                _log.ErrorFormat("Error executing query {0}.", Sql);
                _log.Error(ex.Message, ex);
            }
            finally
            {
                IsBusy = false;
                StopTimer();
                UpdateQueryElapsedTime();
                MarkScriptMessages(_queryHighlightGroups, _queryRanges, Color.LightSkyBlue);
                MarkScriptMessages(_errorHighlightGroups, _errorRanges, Color.LightCoral);
                RefreshUserInterface();
            }

            // Close connection if transaction was not needed
            try
            {
                if (!needsTransaction) await CloseCurrentConnectionAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Error closing connection.");
                _log.Error(ex.Message, ex);
            }
        }

        private void MarkScriptMessages(Dictionary<TextEditorControl, HighlightGroup> highlightGroups, IEnumerable<TextRange> highlightRanges, Color color)
        {
            try
            {
                if (!highlightGroups.ContainsKey(_teScriptResults))
                    highlightGroups[_teScriptResults] = new HighlightGroup(_teScriptResults);
                var group = highlightGroups[_teScriptResults];
                foreach (var textRange in highlightRanges)
                {
                    var m = new TextMarker(textRange.Offset, textRange.Length,
                                           TextMarkerType.SolidBlock, color, Color.Black);
                    @group.AddMarker(m);
                }
                _teScriptResults.Refresh();
            }
            catch (Exception ex)
            {
                _log.Error("Error marking script messages");
                _log.Error(ex.Message, ex);
            }
        }

        private void CleanupAfterQueryAbort(string message)
        {
            StopTimer();
            SetQueryElpasedTime();
            CloseCurrentConnectionAsync();
            _teScriptResults.AppendText(message);
            
            //// If a forcibly canceled task is still running, create a continuation task to close the database connection after the query is done
            //if (queryTask != null && !queryTask.IsCompleted)
            //{
            //    var c = _connection;
            //    _connection = null;
            //    var t = _transaction;
            //    _transaction = null;
            //    queryTask.ContinueWith(task => CloseConnectionAsync(c, t));
            //}
            //else
            //{
            //    CloseCurrentConnectionAsync();
            //}
        }

        private void SetQueryElpasedTime()
        {
            _timingLabelTool.SharedProps.Visible = true;
            _timingLabelTool.SharedProps.Caption = string.Format("Elapsed: {0}:{1}:{2}.{3}",
                                                                 _stopwatch.Elapsed.Hours.ToString("00"),
                                                                 _stopwatch.Elapsed.Minutes.ToString("00"),
                                                                 _stopwatch.Elapsed.Seconds.ToString("00"),
                                                                 _stopwatch.Elapsed.Milliseconds.ToString("000"));
        }

        private async Task<IDbConnection> GetConnectionAsync()
        {
            try
            {
                await CloseCurrentConnectionAsync();

                _log.DebugFormat("Creating new connection for {0} ...", _databaseConnection.Name);
                _connection = await _databaseConnection.CreateNewConnectionAsync();
                _log.Debug("Connection created.");
                return _connection;
            }
            catch (Exception ex)
            {
                _log.Error("Error obtaining a connection.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        private Task CloseCurrentConnectionAsync()
        {
            var dbTransactionClosure = _transaction;
            _transaction = null;
            var dbConnectionClosure = _connection;
            _connection = null;
            var dbCommandClosure = _command;
            _command = null;
            return CloseConnectionAsync(dbConnectionClosure, dbTransactionClosure, dbCommandClosure);
        }

        private Task CloseConnectionAsync(IDbConnection dbConnection, IDbTransaction dbTransaction, IDbCommand dbCommand)
        {
            var dbConnectionClosure = dbConnection;
            var dbTransactionClosure = dbTransaction;
            var dbCommandClosure = dbCommand;
            var task = Task.Run(() => CloseConnection(dbConnectionClosure, dbTransactionClosure, dbCommandClosure));
            return task;
        }

        private void CloseConnection(IDbConnection dbConnection, IDbTransaction dbTransaction, IDbCommand dbCommand)
        {
            try
            {
                if (dbTransaction != null)
                {
                    _log.Debug("Rolling back existing transaction ...");
                    dbTransaction.Rollback();
                    _log.Debug("Rollback complete.");
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error rolling back transaction for connection {0}.", _databaseConnection.Name);
                _log.Error(ex.Message, ex);
            }

            try
            {
                if (dbCommand != null)
                {
                    _log.Debug("Canceling command ...");
                    dbCommand.Cancel();
                    _log.Debug("Cancel complete.");
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error canceling command for connection {0}.", _databaseConnection.Name);
                _log.Error(ex.Message, ex);
            }

            try
            {
                if (dbConnection != null)
                {
                    _log.DebugFormat("[{0}] Closing connection ...", _databaseConnection.Name);
                    dbConnection.Close();
                    _log.DebugFormat("[{0}] Connection closed.", _databaseConnection.Name);
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error closing connection {0}.", _databaseConnection.Name);
                _log.Error(ex.Message, ex);
            }

            _connection = null;
            _transaction = null;
            _command = null;
        }

        private void RefreshUserInterface()
        {
            _copyTool.SharedProps.Enabled = !IsBusy;

            _activityIndicatorTool.SharedProps.Visible = 
                _uaiActivity.AnimationEnabled = 
                _stopTool.SharedProps.Visible = IsBusy;

            _saveTool.SharedProps.Enabled =
                _clearTool.SharedProps.Enabled = !IsBusy;
            
            _stopTool.SharedProps.Enabled = IsBusy && !_cancellationTokenSource.IsCancellationRequested;

            _commitTool.SharedProps.Enabled =
                _rollbackTool.SharedProps.Enabled = !IsBusy && _databaseConnection != null && _databaseConnection.IsConnected && !_databaseConnection.AutoCommit && _transaction != null && _connection != null;
        }

        private static DataTable GetNonQueryResultsTable(string message)
        {
            var table = new DataTable();
            table.Columns.Add("Results", typeof(string));
            table.Rows.Add(new object[] {message});
            return table;
        }

        private static DataTable GetErrorMessageTable(Exception e)
        {
            var errorsTable = new DataTable();
            errorsTable.Columns.Add("Error Message", typeof(string));
            var errorText = e.Message;
            errorsTable.Rows.Add(new object[] { errorText });
            return errorsTable;
        }

        private void SetResults(DataTable table, string sqlStatement, TimeSpan elapsedTime, bool isError = false, bool showTiming = true)
        {
            if (table != null && table.Columns.Count == 0)
            {
                table.Columns.Add("Results (" + ((int)elapsedTime.TotalMilliseconds).ToString("#,0") + " ms)", typeof(string));
                table.Rows.Add("Command executed successfully");
                SetResults(table, sqlStatement, elapsedTime, isError, false);
                return;
            }

            _log.Debug("Preparing results ...");
            var startOffset = _teScriptResults.Document.TextLength;
            var sb = new StringBuilder();
            sb.Append(sqlStatement);
            _queryRanges.Add(new TextRange(startOffset, sb.Length));
            sb.Append(Environment.NewLine);
            if (showTiming)
            {
                sb.AppendLine("Results (" + ((int) elapsedTime.TotalMilliseconds).ToString("#,0") + " ms)");
            }
            sb.Append(table == null ? string.Empty : table.AsFormattedString());
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            
            _teScriptResults.AppendTextAtEnd(sb.ToString());

            if (isError)
            {
                _errorRanges.Add(new TextRange(startOffset, sb.Length));
            }
            _teScriptResults.Refresh();
        }

        private void UtmToolClick(object sender, ToolClickEventArgs e)
        {
            try
            {
                switch (e.Tool.Key)
                {
                    case "Flag":
                        IsPinned = _flagTool.Checked;
                        break;

                    case "Show SQL":
                        ShowSql();
                        break;

                    case "Stop":
                        Stop();
                        break;

                    case "Commit":
                        CommitAsync();
                        break;

                    case "Rollback":
                        RollbackAsync();
                        break;
                    
                    case "Copy":
                        Copy();
                        break;

                    case "Save":
                        Save();
                        break;

                    case "Clear":
                        ClearResults();
                        break;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error performing operation.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, "Error performing operation.", ex.Message, ex.StackTrace);
            }
            finally
            {
                RefreshUserInterface();
            }
        }

        private void ClearResults()
        {
            // Clear grid
            _teScriptResults.Text = string.Empty;
            if (_errorHighlightGroups.ContainsKey(_teScriptResults))
            {
                var group = _errorHighlightGroups[_teScriptResults];
                group.ClearMarkers();
            }
            _errorRanges.Clear();

            if (_queryHighlightGroups.ContainsKey(_teScriptResults))
            {
                var group = _queryHighlightGroups[_teScriptResults];
                group.ClearMarkers();
            }
            _queryRanges.Clear();
            _teScriptResults.Refresh();
        }

        private void Save()
        {
            string selectedFile;
            var dialogResult = Dialog.ShowSaveFileDialog("Select a file", out selectedFile,
                                                 new[]
                                                     {
                                                         new CommonFileDialogFilter("Text files", ".txt"),
                                                         new CommonFileDialogFilter("All files", "*.*")
                                                     });
            if (dialogResult != CommonFileDialogResult.Ok) return;
            
            _teScriptResults.SaveFile(selectedFile);
        }

        private void Copy()
        {
            _teScriptResults.DoEditAction(new Copy());
        }

        private async void RollbackAsync()
        {
            _log.DebugFormat("Rollback started ...");
            if (IsBusy)
            {
                throw new InvalidOperationException("Previous operation is still in progress");
            }
            else if (_connection == null || _transaction == null)
            {
                throw new InvalidOperationException("Transaction is not currently active");
            }

            try
            {
                InitalizeOperationStart(false);
                var rollbackTask = Task.Run(() => _transaction.Rollback()).WithCancellation(_cancellationTokenSource.Token);
                await rollbackTask;
                _log.Debug("Rollback done.");
                StopTimer();
                _transaction = null;
                SetResults(GetNonQueryResultsTable("Rollback complete."), string.Empty, _stopwatch.Elapsed);
                await CloseCurrentConnectionAsync();
            }
            catch (OperationCanceledException)
            {
                CleanupAfterQueryAbort("Operation canceled");
                _log.Debug("Rollback aborted.");
            }
            catch (Exception ex)
            {
                CleanupAfterQueryAbort("Operation failed");
                SetResults(GetErrorMessageTable(ex), string.Empty, TimeSpan.Zero, true);

                _log.Error("Error rolling back transaction.");
                _log.Error(ex.Message, ex);
            }
            finally
            {
                IsBusy = false;
                RefreshUserInterface();
            }
        }

        private async void CommitAsync()
        {
            _log.DebugFormat("Commit started ...");
            if (IsBusy)
            {
                throw new InvalidOperationException("Previous operation is still in progress");
            }
            else if (_connection == null || _transaction == null)
            {
                throw new InvalidOperationException("Transaction is not currently active");
            }

            try
            {
                InitalizeOperationStart(false);
                var commitTask = Task.Run(() => _transaction.Commit()).WithCancellation(_cancellationTokenSource.Token);
                await commitTask;
                _log.Debug("Commit done.");
                StopTimer();
                _transaction = null;
                SetResults(GetNonQueryResultsTable("Commit complete."), string.Empty, _stopwatch.Elapsed);
                await CloseCurrentConnectionAsync();
            }
            catch (OperationCanceledException)
            {
                CleanupAfterQueryAbort("Operation canceled");
                _log.Debug("Commit aborted.");
            }
            catch (Exception ex)
            {
                CleanupAfterQueryAbort("Operation failed");
                SetResults(GetErrorMessageTable(ex), string.Empty, TimeSpan.Zero, true);

                _log.Error("Error committing transaction.");
                _log.Error(ex.Message, ex);
            }
            finally
            {
                IsBusy = false;
                RefreshUserInterface();
            }
        }

        private void StopTimer()
        {
            _stopwatch.Stop();
            _tmQueryTimer.Stop();
        }

        private void InitalizeOperationStart(bool clearExistingResults = true)
        {
            // Start measuring query elapsed time
            _stopwatch.Restart();
            _tmQueryTimer.Start();

            // Initialize CancellationSource
            _cancellationTokenSource = new CancellationTokenSource();

            // Indicate we're busy processing
            IsBusy = true;

            // Clear grid
            if (clearExistingResults)
            {
                ClearResults();
            }

            // Remove ranges
            _errorRanges.Clear();
            _queryRanges.Clear();

            // Refresh user interface to reflect the status
            RefreshUserInterface();
        }

        private void Stop()
        {
            try
            {
                _cancellationTokenSource.Cancel();
            }
            catch (Exception ex)
            {
                _log.Error("Error stopping the operation.");
                _log.Error(ex.Message);
                Dialog.ShowErrorDialog(Application.ProductName, "Error stopping the operation.", ex.Message, ex.StackTrace);
            }
        }

        private void ShowSql()
        {
            var frm = new FrmShowSql(Sql, _databaseConnection.DatabaseServer.HighlightingDefinitionsFile);
            frm.ShowDialog();
        }

        private void TmQueryTimerTick(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => TmQueryTimerTick(sender, e)));
                return;
            }

            UpdateQueryElapsedTime();
        }

        private void UpdateQueryElapsedTime()
        {
            _timingLabelTool.SharedProps.Visible = true;
            _timingLabelTool.SharedProps.Caption = string.Format("Elapsed: {0}:{1}:{2}.{3}",
                                                                 _stopwatch.Elapsed.Hours.ToString("00"),
                                                                 _stopwatch.Elapsed.Minutes.ToString("00"),
                                                                 _stopwatch.Elapsed.Seconds.ToString("00"),
                                                                 _stopwatch.Elapsed.Milliseconds.ToString("000"));
        }

        public void FreeResources()
        {
            CloseConnection(_connection, _transaction, _command);
            IsPinned = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

