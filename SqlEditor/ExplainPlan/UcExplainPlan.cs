using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinTree;
using log4net;
using SqlEditor.Annotations;
using SqlEditor.DatabaseExplorer;
using Utilities.Forms.Dialogs;
using Utilities.Text;

namespace SqlEditor.ExplainPlan
{
    public partial class UcExplainPlan : UserControl, INotifyPropertyChanged
    {
        #region Fields
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly LabelTool _timingLabelTool;
        private readonly StateButtonTool _flagTool;
        private readonly ControlContainerTool _activityIndicatorTool;
        private readonly ButtonTool _stopButton;
        private readonly ButtonTool _refreshQueryResultsTool;
        private readonly ButtonTool _copyTool;

        private readonly DatabaseConnection _databaseConnection;
        private readonly Stopwatch _stopwatch;
        private bool _isPinned;
        private bool _isBusy;
        private string _sql;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        #endregion


        #region Properties
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
            protected set
            {
                if (value == _sql) return;
                _sql = value;
                OnPropertyChanged("Sql");
            }
        }

        #endregion
        
        public UcExplainPlan()
        {
            InitializeComponent();
            _stopwatch = new Stopwatch();
            _isPinned = false;
            _isBusy = false;
            
            _timingLabelTool = (LabelTool)_utm.Tools["Timing"];            
            _flagTool = (StateButtonTool)_utm.Tools["Flag"];
            _activityIndicatorTool = (ControlContainerTool)_utm.Tools["Activity Indicator"];
            _stopButton = (ButtonTool)_utm.Tools["Stop"];
            _refreshQueryResultsTool = (ButtonTool)_utm.Tools["Refresh"];
            _copyTool = (ButtonTool)_utm.Tools["Copy"];
            PopupMenuTool gridMenuTool = (PopupMenuTool)_utm.Tools["GridPopupMenu"];
            gridMenuTool.BeforeToolDropdown += (sender, args) => RefreshUserInterface();

            RefreshUserInterface();
        }

        public UcExplainPlan(DatabaseConnection databaseConnection)
            : this()
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            _databaseConnection = databaseConnection;
            _databaseConnection.PropertyChanged += (sender, args) => RefreshUserInterface();
        }

        public async void RunExplainAsync(string sql)
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

            try
            {
                Sql = sql;
                InitalizeOperationStart();
                UpdateQueryElapsedTime();

                var explainGenerator = _databaseConnection.DatabaseServer.GetExplainPlanGenerator();
                var explainData = await explainGenerator.GetExplainPlanAsync(_databaseConnection, sql, _cancellationTokenSource.Token).WithCancellation(_cancellationTokenSource.Token);
                StopTimer();
                SetResults(explainData);
            }
            catch (OperationCanceledException)
            {
                //_log.Info("Operation canceled.");
                CleanupAfterQueryAbort();
            }
            catch (Exception ex)
            {
                //_log.Info("Operation failed.");
                CleanupAfterQueryAbort();
                SetErrorResults(ex);
            }
            finally
            {
                IsBusy = false;
                UpdateQueryElapsedTime();
                RefreshUserInterface();
            }
        }

        private void CleanupAfterQueryAbort()
        {
            StopTimer();
            SetQueryElpasedTime();
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

        

        private void RefreshUserInterface()
        {
                _copyTool.SharedProps.Enabled =
                    !IsBusy && _databaseConnection != null;

            _activityIndicatorTool.SharedProps.Visible = 
                _uaiActivity.AnimationEnabled = 
                _stopButton.SharedProps.Visible = IsBusy;
            
            _stopButton.SharedProps.Enabled = IsBusy && !_cancellationTokenSource.IsCancellationRequested;

            _refreshQueryResultsTool.SharedProps.Enabled =
                !IsBusy && _databaseConnection != null && _databaseConnection.IsConnected;
        }

        private void SetResults(ExplainPlanData explainData)
        {
            SetResultsInternal(explainData.GetColumns(), explainData.Rows);
        }

        private void SetResultsInternal(IList<ExplainPlanColumn> explainPlanColumns, IList<ExplainPlanRow> rootExplainPlanRow)
        {
            // Clear any existing results
            ClearResults();

            // Add columns
            var columnSet = new UltraTreeColumnSet {Key = "Default"};
            foreach (var explainPlanColumn in explainPlanColumns)
            {
                var column = new UltraTreeNodeColumn();
                column.Key = explainPlanColumn.ColumnDisplayName;
                column.ButtonDisplayStyle = ButtonDisplayStyle.Always;
                column.CanShowExpansionIndicator = Infragistics.Win.DefaultableBoolean.True;
                columnSet.Columns.Add(column);
            }
            _utExplain.ColumnSettings.ColumnSets.Add(columnSet);
            _utExplain.ColumnSettings.RootColumnSetIndex = 0;
            _utExplain.ColumnSettings.ShowBandNodes = ShowBandNodes.Always;

            PopulateNodes(rootExplainPlanRow, _utExplain.Nodes, explainPlanColumns);

            _utExplain.ExpandAll(ExpandAllType.Always);

            // Auto size
            foreach (var column in columnSet.Columns)
            {
                column.PerformAutoResize(ColumnAutoSizeMode.AllNodes);
            }

        }

        private void ClearResults()
        {
            _utExplain.Nodes.Clear();
            _utExplain.ColumnSettings.ColumnSets.Clear();
        }

        private static void PopulateNodes(IList<ExplainPlanRow> rows, TreeNodesCollection nodes, IList<ExplainPlanColumn> explainPlanColumns)
        {
            foreach (var row in rows)
            {
                var node = nodes.Add();
                foreach (var explainPlanColumn in explainPlanColumns)
                {
                    var value = row.Row.IsNull(explainPlanColumn.ColumnName)
                        ? null
                        : row.Row[explainPlanColumn.ColumnName];
                    node.Cells[explainPlanColumn.ColumnDisplayName].Value = value;
                }

                foreach (var childRow in row.ChildRows)
                {
                    PopulateNodes(new[] { childRow } , node.Nodes, explainPlanColumns);
                }
            }
        }

        private void SetErrorResults(Exception ex)
        {
            var messageText = ex.Message.Trim();
            var exc = ex.InnerException;
            while (exc != null && !exc.Message.IsNullEmptyOrWhitespace())
            {
                if (!messageText.EndsWith("."))
                {
                    messageText += ". ";
                }
                messageText += exc.Message.Trim();
            }
            var table = new DataTable();
            table.Columns.Add("ERROR_MESSAGE", typeof (string));
            var row = table.NewRow();
            row.ItemArray = new object[] {messageText};


            var explainPlanColumns = new List<ExplainPlanColumn>();
            var explainPlanColumn = new ExplainPlanColumn("ERROR_MESSAGE", "Error Message");
            explainPlanColumns.Add(explainPlanColumn);
            var explainPlanRow = new [] {new ExplainPlanRow {Row = row}};
            SetResultsInternal(explainPlanColumns, explainPlanRow);
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
                    
                    case "Copy":
                        Copy();
                        break;

                    case "Refresh":
                        RunExplainAsync(Sql);
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

        private void Copy()
        {
            var selectedNodes = _utExplain.SelectedNodes.Cast<UltraTreeNode>().ToList();
            if (selectedNodes.Count == 0)
            {
                return;
            }

            var sb = new StringBuilder();
            foreach (var node in selectedNodes)
            {
                var separator = string.Empty;
                var indent = string.Join(string.Empty, Enumerable.Repeat("\t", node.Level));
                sb.Append(indent);
                foreach (var column in _utExplain.ColumnSettings.ColumnSets[0].Columns)
                {
                    separator = "\t";
                    sb.Append(separator);
                    sb.Append(node.Cells[column].Value);
                }
                sb.AppendLine();
            }

            Clipboard.SetText(sb.ToString());
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
            ClearResults();

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

        public event PropertyChangedEventHandler PropertyChanged;        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UtExplain_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control &&
                    e.KeyCode == Keys.C)
                {
                    Copy();
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error copying.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, "Error performing copy.", ex.Message, ex.StackTrace);
            }
        }

        private void UtExplain_KeyDown(object sender, KeyEventArgs e)
        {
            UtExplain_KeyUp(sender, e);
        }

        public void FreeResources()
        {
            IsPinned = false;
        }
    }
}

