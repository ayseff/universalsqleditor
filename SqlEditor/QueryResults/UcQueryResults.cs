using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinMaskedEdit;
using Infragistics.Win.UltraWinToolbars;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.Properties;
using SqlEditor.SqlHelpers;
using Utilities.Forms;
using Utilities.Forms.Dialogs;
using Utilities.InfragisticsUtilities.UltraGridUtilities;
using Utilities.Text;
using log4net;
using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;
using PropertyChangedEventHandler = System.ComponentModel.PropertyChangedEventHandler;

namespace SqlEditor.QueryResults
{
    public partial class UcQueryResults : UserControl, INotifyPropertyChanged
    {
        #region Fields
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly UltraGridNullValueDataFilter _nullColumnDataFilter = new UltraGridNullValueDataFilter();
        
        private readonly LabelTool _timingLabelTool;
        private readonly LabelTool _rowCountLabelTool;
        private readonly StateButtonTool _flagTool;
        private readonly StateButtonTool _showFilterTool;
        private readonly ControlContainerTool _activityIndicatorTool;
        private readonly ButtonTool _stopButton;
        private readonly ButtonTool _exportToExcelTool;
        private readonly ButtonTool _exportToCsvTool;
        private readonly ButtonTool _exportToTextTool;
        private readonly ButtonTool _commitTool;
        private readonly ButtonTool _rollbackTool;
        private readonly ButtonTool _fetchMoreRowsTool;
        private readonly ButtonTool _fetchAllRowsTool;
        private readonly ButtonTool _refreshQueryResultsTool;
        private readonly ButtonTool _copyTool;
        private readonly ButtonTool _copyWithHeaders;
        private readonly ButtonTool _copyForSql;
        private readonly ButtonTool _visualizedataTool;
        private readonly PopupMenuTool _gridMenuTool;
        
        private readonly DatabaseConnection _databaseConnection;
        private IDbConnection _connection;
        private IDbCommand _command;
        private IDbTransaction _transaction;
        private readonly Stopwatch _stopwatch;
        private int _maxResults;
        private SqlType _sqlType;
        private string _sqlFirstKeyword;
        private bool _hasMoreRows;
        private bool _isPinned;
        private bool _isBusy;
        private string _sql;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Infragistics.Win.Appearance _alternateRowAppearance = new Infragistics.Win.Appearance { BackColor = Color.LightYellow };
        private readonly Infragistics.Win.Appearance _normalRowAppearance = new Infragistics.Win.Appearance { BackColor = Color.White };
        #endregion


        #region Properties
        public bool HasMoreRows
        {
            get { return _hasMoreRows; }
            set
            {
                if (value.Equals(_hasMoreRows)) return;
                _hasMoreRows = value;
                OnPropertyChanged("HasMoreRows");
            }
        }

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
        
        public UcQueryResults()
        {
            InitializeComponent();
            _stopwatch = new Stopwatch();
            _isPinned = false;
            _sql = string.Empty;
            _isBusy = false;
            _maxResults = 100;
            
            _timingLabelTool = (LabelTool)_utm.Tools["Timing"];            
            _rowCountLabelTool = (LabelTool)_utm.Tools["Row Counts"];
            _flagTool = (StateButtonTool)_utm.Tools["Flag"];
            _showFilterTool = (StateButtonTool)_utm.Tools["Show Filter"];
            _activityIndicatorTool = (ControlContainerTool)_utm.Tools["Activity Indicator"];
            _stopButton = (ButtonTool)_utm.Tools["Stop"];
            _exportToExcelTool = (ButtonTool)_utm.Tools["Export to Excel"];
            _exportToCsvTool = (ButtonTool)_utm.Tools["Export to CSV"];
            _exportToTextTool = (ButtonTool)_utm.Tools["Export to Text"];
            _commitTool = (ButtonTool)_utm.Tools["Commit"];
            _rollbackTool = (ButtonTool)_utm.Tools["Rollback"];
            _fetchAllRowsTool = (ButtonTool)_utm.Tools["Fetch All Rows"];
            _fetchMoreRowsTool = (ButtonTool)_utm.Tools["Fetch More Rows"];
            _refreshQueryResultsTool = (ButtonTool)_utm.Tools["Refresh"];
            _copyTool = (ButtonTool)_utm.Tools["Copy"];
            _copyWithHeaders = (ButtonTool)_utm.Tools["Copy w/ Headers"];
            _copyForSql = (ButtonTool)_utm.Tools["Copy for SQL IN Clause"];
            _visualizedataTool = (ButtonTool)_utm.Tools["Visualize Data"];
            _gridMenuTool = (PopupMenuTool)_utm.Tools["GridPopupMenu"];
            _gridMenuTool.BeforeToolDropdown += (sender, args) => RefreshUserInterface();

            RefreshUserInterface();
        }

        public UcQueryResults(DatabaseConnection databaseConnection)
            : this()
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            _databaseConnection = databaseConnection;
            _databaseConnection.PropertyChanged += (sender, args) => RefreshUserInterface();
        }

        public async void RunQueryAsync(string sql, int maxResults)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }
            else if (sql.IsNullEmptyOrWhitespace())
            {
                throw new InvalidOperationException("SQL is not specified");
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

            //if (_log.IsDebugEnabled)
            //{
            //    _log.DebugFormat("Starting to run SQL:");
            //    _log.Debug(Sql);
            //}

            if (_transaction != null)
            {
                var dialogResult = Dialog.ShowYesNoDialog(Application.ProductName,
                                                          "Previous transaction is still in progress. Are you sure you want to continue?",
                                                          "Selecting Yes will rollback previous transaction.");
                if (dialogResult != TaskDialogResult.Yes) return;
            }

            try
            {
                InitalizeOperationStart();
                InitializeQueryStart(sql, maxResults);
                UpdateQueryElapsedTime();

                // Create a new connection
                _connection = await GetConnectionAsync().WithCancellation(_cancellationTokenSource.Token);

                // Create a new connection
                await _connection.OpenIfRequiredAsync().WithCancellation(_cancellationTokenSource.Token);

                // Create command
                _command = _connection.CreateCommand();
                _command.CommandTimeout = 0;

                // Run task
                Task<SqlQueryResult> queryTask;
                if (_sqlType == SqlType.Query)
                {
                    // Execute SELECT or DDL query
                    //queryTask = _connection.ExecuteQueryAsync(Sql, maxResults, _cancellationTokenSource.Token).WithCancellation(_cancellationTokenSource.Token);
                    queryTask = _command.ExecuteQueryAsync(Sql, maxResults, _cancellationTokenSource.Token);
                    var results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                    //_log.Debug("Query complete.");
                    HasMoreRows = results.HasMoreRows;
                    SetResults(results.Result, PerformAutoSizeType.AllRowsInBand, true);
                    //_log.Debug("Results bound.");
                    StopTimer();
                    await CloseCurrentConnectionAsync().WithCancellation(_cancellationTokenSource.Token);

                    // Update row counts label
                    _rowCountLabelTool.SharedProps.Visible = true;
                    var rowCount = results.Result.Rows.Count;
                    _rowCountLabelTool.SharedProps.Caption = "Showing " + (HasMoreRows ? "first " : " all ") + rowCount.ToString("#,0") + " row" + (rowCount != 1 ? "s" : string.Empty);
                }
                else
                {
                    // Execute DML query
                    SqlQueryResult results;
                    if (_sqlType == SqlType.Dml && !_databaseConnection.AutoCommit)
                    {
                        //_log.Debug("Non-query will use a transaction.");
                        //queryTask = _connection.ExecuteNonQueryTransactionAsync(Sql, _cancellationTokenSource.Token);
                        queryTask = _command.ExecuteNonQueryTransactionAsync(Sql, _cancellationTokenSource.Token);
                        results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                        _transaction = results.Transaction;
                    }
                    else
                    {
                        //queryTask = _connection.ExecuteNonQueryAsync(Sql, _cancellationTokenSource.Token);
                        queryTask = _command.ExecuteNonQueryAsync(Sql, _cancellationTokenSource.Token);
                        results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                        await CloseCurrentConnectionAsync().WithCancellation(_cancellationTokenSource.Token);
                    }
                    //_log.Debug("Non-query complete.");
                    HasMoreRows = false;
                    var resultsTable = new DataTable();
                    resultsTable.Columns.Add("Results", typeof(string));
                    if (_sqlType == SqlType.Dml && results.RowsAffected >= 0)
                    {
                        // Ex: Inserted x rows
                        resultsTable.Rows.Add(string.Format("{0}{1}D {2} row{3}", _sqlFirstKeyword.Trim().ToUpper(), _sqlFirstKeyword.ToUpper().Trim().EndsWith("E") ? string.Empty : "E", results.RowsAffected.ToString("#,0"), results.RowsAffected != 1 ? "s" : string.Empty));
                    }
                    else
                    {
                        resultsTable.Rows.Add(string.Format("{0} successful", _sqlFirstKeyword.Trim().ToUpper()));
                    }
                    SetResults(resultsTable, PerformAutoSizeType.AllRowsInBand);
                    //_log.Debug("Results bound.");
                    StopTimer();

                    // Update row counts label
                    _rowCountLabelTool.SharedProps.Visible = true;
                    _rowCountLabelTool.SharedProps.Caption = "Operation successful";
                }
            }
            catch (OperationCanceledException)
            {
                //_log.Info("Operation canceled.");
                CleanupAfterQueryAbort("Operation canceled");
                
                //// If a forcibly canceled task is still running, create a continuation task to close the database connection after the query is done
                //if (queryTask != null && !queryTask.IsCompleted)
                //{
                //    var connection = _connection;
                //    var transaction = _transaction;
                //    var command = _command;
                //    CloseConnectionAsync(connection, transaction, command);
                //    // ReSharper disable CSharpWarnings::CS4014
                //    queryTask.ContinueWith(task => CloseConnectionAsync(connection, transaction, command));
                //    // ReSharper restore CSharpWarnings::CS4014
                //}
            }
            catch (Exception ex)
            {
                //_log.Info("Operation failed.");
                CleanupAfterQueryAbort("Operation failed");
                SetResults(GetErrorMessageTable(ex), PerformAutoSizeType.AllRowsInBand);
                _ugGrid.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand);

                //_log.ErrorFormat("Error executing query {0}.", Sql);
                //_log.Error(ex.Message, ex);
            }
            finally
            {
                IsBusy = false;
                UpdateQueryElapsedTime();
                RefreshUserInterface();
            }
        }

        private void InitializeQueryStart(string sql, int maxResults)
        {
            MaxResults = maxResults;
            Sql = sql;
            _sqlFirstKeyword = SqlHelper.GetFirstKeyword(Sql, _databaseConnection.DatabaseServer.BlockCommentRegex,
                                                         _databaseConnection.DatabaseServer.LineCommentRegex);
            _sqlType = SqlHelper.GetSqlType(Sql, _sqlFirstKeyword);
            _rowCountLabelTool.SharedProps.Visible = false;
        }

        private void CleanupAfterQueryAbort(string message)
        {
            StopTimer();
            SetQueryElpasedTime();
            HasMoreRows = false;
            _rowCountLabelTool.SharedProps.Visible = true;
            _rowCountLabelTool.SharedProps.Caption = message;

            var connection = _connection;
            _connection = null;
            var transaction = _transaction;
            _transaction = null;
            var command = _command;
            _command = null;
            CloseConnectionAsync(connection, transaction, command);
            
            //// If a forcibly canceled task is still running, create a continuation task to close the database connection after the query is done
            //if (queryTask != null && !queryTask.IsCompleted)
            //{
            //    var connection = _connection;
            //    _connection = null;
            //    var transaction = _transaction;
            //    _transaction = null;
            //    var command = _command;
            //    _command = null;
            //    CloseConnectionAsync(connection, transaction, command);
            //    //queryTask.ContinueWith(task => CloseConnectionAsync(connection, transaction, command));
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

        //private IDbConnection GetConnection()
        //{
        //    CloseCurrentConnection();
        //    _log.DebugFormat("Creating new connection for {0} ...", _databaseConnection.Name);
        //    _connection = _databaseConnection.CreateNewConnection();
        //    _log.Debug("Connection created.");
        //    return _connection;
        //}

        private Task CloseCurrentConnectionAsync()
        {
            var dbTransactionClosure = _transaction;
            _transaction = null;
            var dbConnectionClosure = _connection;
            _connection = null;
            var dbCommand = _command;
            _command = null;
            return CloseConnectionAsync(dbConnectionClosure, dbTransactionClosure, dbCommand);
        }

        //private Task CloseConnectionAsync(IDbConnection dbConnection, IDbTransaction dbTransaction)
        //{
        //    var dbConnectionClosure = dbConnection;
        //    var dbTransactionClosure = dbTransaction;
        //    var task = Task.Run(() => CloseConnection(dbConnectionClosure, dbTransactionClosure));
        //    return task;
        //}

        private Task CloseConnectionAsync(IDbConnection dbConnection, IDbTransaction dbTransaction, IDbCommand dbCommand)
        {
            var dbConnectionClosure = dbConnection;
            var dbTransactionClosure = dbTransaction;
            var dbCommandClosure = dbCommand;
            var task = Task.Run(() => CloseConnection(dbConnectionClosure, dbTransactionClosure, dbCommandClosure));
            return task;
        }

        //private void CloseConnection(IDbConnection dbConnection, IDbTransaction dbTransaction)
        //{
        //    try
        //    {
        //        if (dbTransaction != null)
        //        {
        //            _log.Debug("Rolling back existing transaction ...");
        //            dbTransaction.Rollback();
        //            _log.Debug("Rollback complete.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.ErrorFormat("Error rolling back transaction for connection {0}.", _databaseConnection.Name);
        //        _log.Error(ex.Message, ex);
        //    }

        //    try
        //    {
        //        if (dbConnection != null)
        //        {
        //            _log.DebugFormat("[{0}] Closing connection ...", _databaseConnection.Name);
        //            dbConnection.Close();
        //            _log.DebugFormat("[{0}] Connection closed.", _databaseConnection.Name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.ErrorFormat("Error closing connection {0}.", _databaseConnection.Name);
        //        _log.Error(ex.Message, ex);
        //    }
        //}

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
                    _log.Debug("Canceling existing command ...");
                    dbCommand.Cancel();
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error canceling existing command for connection {0}.", _databaseConnection.Name);
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
        }

        private void RefreshUserInterface()
        {
            _exportToExcelTool.SharedProps.Enabled =
                _exportToCsvTool.SharedProps.Enabled =
                _exportToTextTool.SharedProps.Enabled =
                _showFilterTool.SharedProps.Enabled =
                _copyTool.SharedProps.Enabled =
                _copyWithHeaders.SharedProps.Enabled =
                _copyForSql.SharedProps.Enabled = !IsBusy && !string.IsNullOrEmpty(Sql) && _databaseConnection != null;

            _visualizedataTool.SharedProps.Enabled = _ugGrid.Selected.Cells.Count > 0;

            _activityIndicatorTool.SharedProps.Visible = 
                _uaiActivity.AnimationEnabled = 
                _stopButton.SharedProps.Visible = IsBusy;
            
            _stopButton.SharedProps.Enabled = IsBusy && !_cancellationTokenSource.IsCancellationRequested;

            _commitTool.SharedProps.Enabled =
                _rollbackTool.SharedProps.Enabled = !IsBusy && _databaseConnection != null && _databaseConnection.IsConnected 
                && !_databaseConnection.AutoCommit && _transaction != null && _connection != null;

            _refreshQueryResultsTool.SharedProps.Enabled =
                !IsBusy && !string.IsNullOrEmpty(Sql) && _databaseConnection != null && _databaseConnection.IsConnected && _transaction == null;

            _fetchAllRowsTool.SharedProps.Enabled =
                _fetchMoreRowsTool.SharedProps.Enabled =
                !IsBusy && _databaseConnection != null && _databaseConnection.IsConnected && HasMoreRows &&
                _transaction == null;
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
        private void SetResults(DataTable table, PerformAutoSizeType performAutoSizeType = PerformAutoSizeType.VisibleRows, bool showAlternateAppearance = false)
        {
            if (table == null || table.Columns.Count > 0)
            {
                _log.Debug("Binding grid ...");
                _ugGrid.DataSource = table;
                _ugGrid.SetColumnFormat(typeof (DateTime), "yyyy-MM-dd HH:mm:ss.fff");
                _ugGrid.DisplayLayout.Override.RowSelectorHeaderStyle = table != null
                                                                            ? RowSelectorHeaderStyle.ColumnChooserButton
                                                                            : RowSelectorHeaderStyle.Default;
                _log.Debug("Performing auto resize ...");
                _ugGrid.ResizeColumnsToFit(performAutoSizeType);
                _log.Debug("Binding grid complete.");
            }
            else
            {
                table.Columns.Add("Results", typeof(string));
                table.Rows.Add("Command executed successfully");
                SetResults(table, performAutoSizeType);
            }

            if (showAlternateAppearance)
            {
                SetAlternateRowAppearance();
            }
        }

        private void SetAlternateRowAppearance()
        {
            for (var i = 0; i < _ugGrid.Rows.Count; ++i)
            {
                if (_ugGrid.Rows[i].VisibleIndex%2 == 1)
                {
                    _ugGrid.Rows[i].Appearance = _normalRowAppearance;
                }
                else
                {
                    _ugGrid.Rows[i].Appearance = _alternateRowAppearance;
                }
            }
        }

        private async void UtmToolClick(object sender, ToolClickEventArgs e)
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

                    case "Show Filter":
                        ToggleShowFilterRow(_showFilterTool.Checked);
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

                    case "Copy w/ Headers":
                        CopyWithHeaders();
                        break;

                    case "Copy for SQL IN Clause":
                        CopyForSqlInClause();
                        break;

                    case "Export to Excel":
                        await _ugGrid.ExportToExcelAsync();
                        break;
                    
                    case "Fetch All Rows":
                        FetchAllRows();
                        break;

                    case "Fetch More Rows":
                        FetchMoreRows();
                        break;

                    case "Refresh":
                        RunQueryAsync(Sql, _maxResults);
                        break;

                    case "Export to CSV":
                        await _ugGrid.ExportToDelimitedFileAsync(",", ".csv");
                        break;

                    case "Export to Text":
                        await _ugGrid.ExportToDelimitedFileAsync("\t");
                        break;
                    
                    case "Visualize Data":
                        VisualizeData();
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

        private void FetchAllRows()
        {
            RunQueryAsync(Sql, int.MaxValue);
        }

        private void FetchMoreRows()
        {
            RunQueryAsync(Sql, _ugGrid.Rows.Count + _maxResults);
        }

        private void Copy()
        {
            _ugGrid.PerformAction(UltraGridAction.Copy);
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
                InitalizeOperationStart();
                var rollbackTask = Task.Run(() => _transaction.Rollback()).WithCancellation(_cancellationTokenSource.Token);
                await rollbackTask;
                _log.Debug("Rollback done.");
                StopTimer();
                _transaction = null;
                SetResults(GetNonQueryResultsTable("Rollback complete."), PerformAutoSizeType.AllRowsInBand);
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
                SetResults(GetErrorMessageTable(ex), PerformAutoSizeType.AllRowsInBand);

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
                InitalizeOperationStart();
                var commitTask = Task.Run(() => _transaction.Commit()).WithCancellation(_cancellationTokenSource.Token);
                await commitTask;
                _log.Debug("Commit done.");
                StopTimer();
                _transaction = null;
                SetResults(GetNonQueryResultsTable("Commit complete."), PerformAutoSizeType.AllRowsInBand);
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
                SetResults(GetErrorMessageTable(ex), PerformAutoSizeType.AllRowsInBand);

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

        private void InitalizeOperationStart()
        {
            // Start measuring query elapsed time
            _stopwatch.Restart();
            _tmQueryTimer.Start();

            // Initialize CancellationSource
            _cancellationTokenSource = new CancellationTokenSource();

            // Indicate we're busy processing
            IsBusy = true;

            // Clear grid
            SetResults(null);

            // Refresh user interface to reflect the status
            RefreshUserInterface();
        }

        private void Stop()
        {
            try
            {
                _cancellationTokenSource.Cancel(true);
            }
            catch (Exception ex)
            {
                _log.Error("Error stopping the operation.");
                _log.Error(ex.Message);
                Dialog.ShowErrorDialog(Application.ProductName, "Error stopping the operation.", ex.Message, ex.StackTrace);
            }

            try
            {
                CloseCurrentConnectionAsync();
            }
            catch (Exception ex)
            {
                _log.Error("Error closing connection.");
                _log.Error(ex.Message);
            }
        }

        private void ToggleShowFilterRow(bool show)
        {
             _ugGrid.DisplayLayout.Override.FilterUIType = show ?  FilterUIType.FilterRow : FilterUIType.Default;
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

        private void UgGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            foreach (var column in e.Layout.Bands[0].Columns)
            {
                column.Editor.DataFilter = _nullColumnDataFilter;
                column.CellActivation = Activation.ActivateOnly;
            }
            _ugGrid.DisplayLayout.Override.RowAlternateAppearance.BackColor = Color.LightYellow;
        }

        private void CopyWithHeaders()
        {
            var ops = _ugGrid.DisplayLayout.Override.AllowMultiCellOperations;
            _ugGrid.DisplayLayout.Override.AllowMultiCellOperations = AllowMultiCellOperation.CopyWithHeaders;
            _ugGrid.PerformAction(UltraGridAction.Copy);
            _ugGrid.DisplayLayout.Override.AllowMultiCellOperations = ops;
        }

        private void CopyForSqlInClause()
        {
            var selectedCells = _ugGrid.Selected.Cells.Cast<UltraGridCell>().ToList();
            if (selectedCells.Count == 0)
            {
                throw new Exception("Nothing is selected");
            }
            var columnCount = selectedCells.Select(x => x.Column).Distinct().Count();
            if (columnCount != 1)
            {
                throw new Exception("You can only select one column when using this option");
            }

            var column = selectedCells.Select(x => x.Column).First();
            var needsQuotes = column.DataType == typeof (string) || column.DataType == typeof (DateTime) || column.DataType == typeof (TimeSpan);
            var quote = needsQuotes ? "'" : string.Empty;
            var text = string.Join(", ", selectedCells.Select(x => x.Value == null || x.Value == DBNull.Value ? "NULL" : quote + x.GetText(MaskMode.IncludeBoth) + quote));
            Clipboard.Clear();
            Clipboard.SetText(text);
        }

        public event PropertyChangedEventHandler PropertyChanged;        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UgGrid_DoubleClickCell(object sender, DoubleClickCellEventArgs e)
        {
            try
            {
                VisualizeData(e.Cell.Column.DataType, e.Cell.GetText(MaskMode.IncludeLiterals));
            }
            catch (Exception ex)
            {
                _log.Error("Error visualizing data");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, "Error visualizing data,", ex.Message, ex.StackTrace);
            }
        }

        private void VisualizeData()
        {
            var selectedCell = _ugGrid.Selected.Cells.Cast<UltraGridCell>().FirstOrDefault();
            if (selectedCell == null)
            {
                Dialog.ShowErrorDialog(Application.ProductName, "No cells are selected to be visualized", string.Empty, null);
                return;
            }
            VisualizeData(selectedCell.Column.DataType, selectedCell.GetText(MaskMode.IncludeLiterals));
        }

        private static void VisualizeData(Type dataType, string textValue)
        {
            var textType = TextType.Text;
            if (dataType == typeof (string) &&
                textValue.TrimStart().StartsWith("<"))
            {
                // try XML
                try
                {
                    var xmlText = textValue.PrettyPrintXml();
                    textValue = xmlText;
                    textType = TextType.Xml;
                }
                catch (Exception ex)
                {
                    _log.DebugFormat("Could not format text as XML");
                    _log.Debug(ex.Message, ex);
                }
            }

            // Create form
            var form = new FrmTextVisualizer(textValue.Replace("\r", string.Empty), textType) {StartPosition = FormStartPosition.CenterParent};

            // Restore geometry
            try
            {
                if (!string.IsNullOrEmpty(Settings.Default.FrmVisualizeData_Geometry))
                {
                    RestoreFormPosition.GeometryFromString(Settings.Default.FrmVisualizeData_Geometry, form);
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error restoring data visualization form geometry.");
                _log.Error(ex.Message, ex);
            }

            // Show form
            form.ShowDialog();

            // Save geometry
            try
            {
                Settings.Default.FrmVisualizeData_Geometry = RestoreFormPosition.GeometryToString(form);
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error saving data visualization form geometry.");
                _log.Error(ex.Message, ex);
            }

        }

        private void UgGrid_AfterSortChange(object sender, BandEventArgs e)
        {
            try
            {
                SetAlternateRowAppearance();
            }
            catch (Exception ex)
            {
                _log.ErrorFormat(ex.Message, ex);
            }
        }
    }
}

