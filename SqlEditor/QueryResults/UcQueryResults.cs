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
using SqlEditor.SqlHelpers;
using Utilities.Forms.Dialogs;
using Utilities.InfragisticsUtilities.UltraGridUtilities;
using Utilities.Text;
using log4net;
using ColumnHeader = Infragistics.Win.UltraWinGrid.ColumnHeader;
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
        
        private readonly DatabaseConnection _databaseConnection;
        private IDbConnection _connection;
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
        private Infragistics.Win.Appearance _appearance = new Infragistics.Win.Appearance { BackColor = Color.LightYellow };
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

            Task<SqlQueryResult> queryTask = null;
            try
            {
                InitalizeOperationStart();
                InitializeQueryStart(sql, maxResults);
                UpdateQueryElapsedTime();

                // Create a new connection
                _connection = await GetConnectionAsync().WithCancellation(_cancellationTokenSource.Token);

                // Run task
                if (_sqlType == SqlType.Query)
                {
                    // Execute SELECT or DDL query
                    queryTask = _connection.ExecuteQueryAsync(Sql, maxResults, _cancellationTokenSource.Token).WithCancellation(_cancellationTokenSource.Token);
                    var results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                    _log.Debug("Query complete.");
                    HasMoreRows = results.HasMoreRows;
                    SetResults(results.Result, PerformAutoSizeType.AllRowsInBand, true);
                    _log.Debug("Results bound.");
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
                        _log.Debug("Non-query will use a transaction.");
                        queryTask = _connection.ExecuteNonQueryTransactionAsync(Sql, _cancellationTokenSource.Token);
                        results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                        _transaction = results.Transaction;
                    }
                    else
                    {
                        queryTask = _connection.ExecuteNonQueryAsync(Sql, _cancellationTokenSource.Token);
                        results = await queryTask.WithCancellation(_cancellationTokenSource.Token);
                        await CloseCurrentConnectionAsync().WithCancellation(_cancellationTokenSource.Token);
                    }
                    _log.Debug("Non-query complete.");
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
                    _log.Debug("Results bound.");
                    StopTimer();

                    // Update row counts label
                    _rowCountLabelTool.SharedProps.Visible = true;
                    _rowCountLabelTool.SharedProps.Caption = "Operation successful";
                }
            }
            catch (OperationCanceledException)
            {
                _log.Info("Operation cancelled.");
                CleanupAfterQueryAbort("Operation cancelled", queryTask);
                
                // If a forcibly cancelled task is still running, create a continuation task to close the database connection after the query is done
                if (queryTask != null && !queryTask.IsCompleted)
                {
                    var c = _connection;
                    var t = _transaction;
// ReSharper disable CSharpWarnings::CS4014
                    queryTask.ContinueWith(task => CloseConnectionAsync(c, t));
// ReSharper restore CSharpWarnings::CS4014
                }
            }
            catch (Exception ex)
            {
                _log.Info("Operation failed.");
                CleanupAfterQueryAbort("Operation failed", queryTask);
                SetResults(GetErrorMessageTable(ex), PerformAutoSizeType.AllRowsInBand);
                _ugGrid.DisplayLayout.PerformAutoResizeColumns(false, PerformAutoSizeType.AllRowsInBand);

                _log.ErrorFormat("Error executing query {0}.", Sql);
                _log.Error(ex.Message, ex);
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

        private void CleanupAfterQueryAbort(string message, Task queryTask)
        {
            StopTimer();
            SetQueryElpasedTime();
            HasMoreRows = false;
            _rowCountLabelTool.SharedProps.Visible = true;
            _rowCountLabelTool.SharedProps.Caption = message;
            
            // If a forcibly cancelled task is still running, create a continuation task to close the database connection after the query is done
            if (queryTask != null && !queryTask.IsCompleted)
            {
                var c = _connection;
                _connection = null;
                var t = _transaction;
                _transaction = null;
                queryTask.ContinueWith(task => CloseConnectionAsync(c, t));
            }
            else
            {
                CloseCurrentConnectionAsync();
            }
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
            await CloseCurrentConnectionAsync();

            _log.DebugFormat("Creating new connection for {0} ...", _databaseConnection.Name);
            _connection = await _databaseConnection.CreateNewConnectionAsync();
            _log.Debug("Connection created.");
            return _connection;
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
            return CloseConnectionAsync(dbConnectionClosure, dbTransactionClosure);
        }

        private Task CloseConnectionAsync(IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            var dbConnectionClosure = dbConnection;
            var dbTransactionClosure = dbTransaction;
            var task = Task.Run(() => CloseConnection(dbConnectionClosure, dbTransactionClosure));
            return task;
        }

        private void CloseConnection(IDbConnection dbConnection, IDbTransaction dbTransaction)
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
                _copyForSql.SharedProps.Enabled = !IsBusy;

            _activityIndicatorTool.SharedProps.Visible = 
                _uaiActivity.AnimationEnabled = 
                _stopButton.SharedProps.Visible = IsBusy;
            
            _stopButton.SharedProps.Enabled = IsBusy && !_cancellationTokenSource.IsCancellationRequested;

            _commitTool.SharedProps.Enabled =
                _rollbackTool.SharedProps.Enabled = !IsBusy && _databaseConnection != null && _databaseConnection.IsConnected && !_databaseConnection.AutoCommit && _transaction != null && _connection != null;

            _refreshQueryResultsTool.SharedProps.Enabled =
                !IsBusy && _databaseConnection != null && _databaseConnection.IsConnected && _transaction == null;

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
                for (int i = 0; i < _ugGrid.Rows.Count; i+=2)
                {
                    _ugGrid.Rows[i].Appearance = _appearance;
                }
            }
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
                        _ugGrid.ExportToExcel();
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
                        _ugGrid.ExportToCsv();
                        break;

                    case "Export to Text":
                        _ugGrid.ExportToDelimitedFile("\t");
                        break;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error performing operation.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, "Error performing operation.", ex.Message);
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

            Task rollbackTask = null;
            try
            {
                InitalizeOperationStart();
                rollbackTask = Task.Run(() => _transaction.Rollback()).WithCancellation(_cancellationTokenSource.Token);
                await rollbackTask;
                _log.Debug("Rollback done.");
                StopTimer();
                _transaction = null;
                SetResults(GetNonQueryResultsTable("Rollback complete."), PerformAutoSizeType.AllRowsInBand);
                await CloseCurrentConnectionAsync();
            }
            catch (OperationCanceledException)
            {
                CleanupAfterQueryAbort("Operation cancelled", rollbackTask);
                _log.Debug("Rollback aborted.");
            }
            catch (Exception ex)
            {
                CleanupAfterQueryAbort("Operation failed", rollbackTask);
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

            Task commitTask = null;
            try
            {
                InitalizeOperationStart();
                commitTask = Task.Run(() => _transaction.Commit()).WithCancellation(_cancellationTokenSource.Token);
                await commitTask;
                _log.Debug("Commit done.");
                StopTimer();
                _transaction = null;
                SetResults(GetNonQueryResultsTable("Commit complete."), PerformAutoSizeType.AllRowsInBand);
                await CloseCurrentConnectionAsync();
            }
            catch (OperationCanceledException)
            {
                CleanupAfterQueryAbort("Operation cancelled", commitTask);
                _log.Debug("Commit aborted.");
            }
            catch (Exception ex)
            {
                CleanupAfterQueryAbort("Operation failed", commitTask);
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
                Dialog.ShowErrorDialog(Application.ProductName, "Error stopping the operation.", ex.Message);
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
            CloseConnection(_connection, _transaction);
        }

        private void UgGrid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            foreach (var column in e.Layout.Bands[0].Columns)
            {
                column.Editor.DataFilter = _nullColumnDataFilter;
                column.CellActivation = Activation.ActivateOnly;
            }
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

        private void UgGrid_MouseUp(object sender, MouseEventArgs e)
        {
            var grid = (UltraGrid)sender;

            if (e.Button == MouseButtons.Right)
            {
                var element = grid.DisplayLayout.UIElement.LastElementEntered;

                var ugCell = element.GetContext(typeof(UltraGridCell)) as UltraGridCell;
                if (ugCell == null) return;
                var ugRow = element.GetContext(typeof(UltraGridRow)) as UltraGridRow;
                if (ugRow == null) return;
                var columnHeader = element.GetContext(typeof(ColumnHeader)) as ColumnHeader;
                if (columnHeader == null) return;

                

                if (columnHeader != null)
                {
                    MessageBox.Show(columnHeader.Caption);
                }
            }
        }
    }
}

