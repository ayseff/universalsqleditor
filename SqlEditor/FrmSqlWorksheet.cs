using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinToolbars;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.Annotations;
using SqlEditor.DatabaseExplorer;
using SqlEditor.Intellisense;
using SqlEditor.Properties;
using SqlEditor.RunMultipleFiles;
using SqlEditor.ScriptResults;
using SqlEditor.SqlTextEditor;
using Utilities.Forms.Dialogs;
using Utilities.Text;
using log4net;
using System.Reflection;
using CodeCompletionWindow = SqlEditor.Intellisense.CodeCompletionWindow;
using UcQueryResults = SqlEditor.QueryResults.UcQueryResults;

namespace SqlEditor
{
    public delegate void RunQueryEventHandler(object sender, RunQueryEventArgs e);
    public delegate void RunScriptEventHandler(object sender, RunScriptEventArgs e);


    public enum OnCloseSaveAction
    {
        Save = 0,
        Discard = 1,
        DiscardAll = 2
    }

    public sealed partial class FrmSqlWorksheet : Form
    {
        #region Fields
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int _nextTabKey;
        private bool _isModified;
        private string _title;
        private string _worksheetFile;
        private IntelisenseData _intellisenseData;
        private readonly FrmFindReplace _findForm = new FrmFindReplace();
        private readonly StateButtonTool _autoCommitTool;
        private bool _skipToolClickEvents;
        #endregion


        #region Properties
        public DatabaseConnection DatabaseConnection { get; set; }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                UpdateTitle();
            }
        }

        public bool IsModified
        {
            get { return _isModified; }
            private set
            {
                _isModified = value;
                UpdateTitle();
            }
        }

        public string WorksheetFile
        {
            get { return _worksheetFile; }
            private set
            {
                _worksheetFile = value;
                if (_worksheetFile.IsNullEmptyOrWhitespace()) return;
                Title = Path.GetFileName(_worksheetFile);
            }
        }

        private int NextTabKey
        {
            get
            {
                ++_nextTabKey;
                return _nextTabKey;
            }
        }
        #endregion


        public event RunQueryEventHandler RunQuery;
        private void OnRunQuery(RunQueryEventArgs e)
        {
            if (RunQuery != null)
            {
                RunQuery(this, e);
            }
        }

        public event RunScriptEventHandler RunScript;
        private void OnRunScript(RunScriptEventArgs e)
        {
            if (RunScript != null)
            {
                RunScript(this, e);
            }
        }

        private void UpdateTitle()
        {
            Text = DatabaseConnection.Name + " [" + _title + (_isModified ? "*" : string.Empty) + "]";
        }

        public FrmSqlWorksheet([NotNull] DatabaseConnection connection, [NotNull] string title)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (title == null) throw new ArgumentNullException("title");

            InitializeComponent();

            _skipToolClickEvents = true;

            DatabaseConnection = connection;
            DatabaseConnection.PropertyChanged += (sender, args) => RefreshUserInterface();
            
            var maxResultsTextBoxTool = ((TextBoxTool) _utm.Tools["Max Results"]);
            maxResultsTextBoxTool.Text = connection.MaxResults.ToString(CultureInfo.InvariantCulture);
            maxResultsTextBoxTool.ToolKeyPress += (sender, e) =>
                                                       {
                                                           if (!char.IsDigit(e.KeyChar)) e.Handled = true;
                                                       };
            maxResultsTextBoxTool.ToolValueChanged += (sender, args) =>
                                                           {
                                                               int count;
                                                               connection.MaxResults = !int.TryParse(maxResultsTextBoxTool.Text, out count) ? connection.MaxResults : count;
                                                           };
            _autoCommitTool = (StateButtonTool) _utm.Tools["Auto Commit"];
            _autoCommitTool.Checked = connection.AutoCommit;
            
            _sqlEditor.Document.DocumentChanged += (sender, args) => IsModified = true;
            _sqlEditor.ActiveTextAreaControl.TextArea.KeyUp += SqlEditor_KeyUp;
            _sqlEditor.ActiveTextAreaControl.TextArea.KeyDown += SqlEditor_KeyDown;

            IntellisenseManager.Instance.LoadIntellisenseDataAsync(connection);

            Title = title;

            LoadHighlightingDefinitions();

            FindUsableResultsTab();

            LoadSettings();

            _skipToolClickEvents = false;
        }

        public void AppendText(string text)
        {
            _sqlEditor.Document.Insert(_sqlEditor.ActiveTextAreaControl.Caret.Offset, text);
        }

        private void RefreshUserInterface()
        {
            try
            {
                _skipToolClickEvents = true;

                _utm.Tools["Run"].SharedProps.Enabled =
                    _utm.Tools["Run Script"].SharedProps.Enabled =
                    DatabaseConnection != null && DatabaseConnection.IsConnected;
                _autoCommitTool.Checked = DatabaseConnection != null && DatabaseConnection.AutoCommit;
            }
            catch (Exception ex)
            {
                _log.Error("Error refreshing user interface.");
                _log.Error(ex.Message, ex);
            }
            finally
            {
                _skipToolClickEvents = false;
            }
        }

        private void LoadSettings()
        {
            if (Settings.Default.FrmWorksheet_SplitterDistance > 0)
            {
                _utcTabs.Height = Settings.Default.FrmWorksheet_SplitterDistance;
            }
        }

        private void LoadHighlightingDefinitions()
        {
            try
            {
                var dir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Resources");
                if (Directory.Exists(dir))
                {
                    var fsmProvider = new FileSyntaxModeProvider(dir); // Provider
                    HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.
                    _sqlEditor.SetHighlighting(DatabaseConnection.DatabaseServer.HighlightingDefinitionsFile);
                }
                else
                {
                    _log.ErrorFormat("Cannot find directory {0} to load highlighting definitions.", dir);
                    throw new Exception("Missing highlighting definitions.");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error opening highlighting definitions.");
                _log.Error(ex.Message);
            }
        }


        private UcQueryResults FindUsableResultsTab(string sql = null)
        {
            sql = sql ?? string.Empty;
            foreach (var tab in _utcTabs.Tabs)
            {
                foreach (var containedControl in tab.TabPage.Controls)
                {
                    var resultsControl = containedControl as UcQueryResults;
                    if (resultsControl != null && (!resultsControl.IsBusy && (!resultsControl.IsPinned || !tab.Visible)))
                    {
                        tab.Visible = true;
                        tab.EnsureTabInView();
                        tab.Active = true;
                        tab.Selected = true;
                        tab.ToolTipText = sql;
                        return resultsControl;
                    }
                }
            }

            var tabText = "Query Results " + NextTabKey;
            var newTab = _utcTabs.Tabs.Add(tabText, tabText);
            newTab.ToolTipText = sql;
            var qr = new UcQueryResults(DatabaseConnection);
            newTab.TabPage.SuspendLayout();
            newTab.TabPage.Controls.Add(qr);
            qr.Dock = DockStyle.Fill;
            newTab.Selected = true;
            newTab.TabPage.ResumeLayout();
            return qr;
        }

        private UcScriptResults FindUsableScriptResultsTab()
        {
            foreach (var tab in _utcTabs.Tabs)
            {
                foreach (var containedControl in tab.TabPage.Controls)
                {
                    var resultsControl = containedControl as UcScriptResults;
                    if (resultsControl != null && (!resultsControl.IsBusy && (!resultsControl.IsPinned || !tab.Visible)))
                    {
                        tab.Visible = true;
                        tab.EnsureTabInView();
                        tab.Active = true;
                        tab.Selected = true;
                        return resultsControl;
                    }
                }
            }

            var tabText = "Script Output " + NextTabKey;
            var newTab = _utcTabs.Tabs.Add(tabText, tabText);
            var qr = new UcScriptResults(DatabaseConnection);
            newTab.TabPage.SuspendLayout();
            newTab.TabPage.Controls.Add(qr);
            qr.Dock = DockStyle.Fill;
            newTab.Selected = true;
            newTab.TabPage.ResumeLayout();
            return qr;
        }

        //void QueryCompleted(object sender, QueryCompletedEventArgs queryCompletedEventArgs)
        //{
        //    // HACK: Force the tab control to redraw itself so it resizes the individual tab controls
        //    _utcTabs.Height += 1;
        //    _utcTabs.Height -= 1;            
        //}
        
        private void FrmSqlWorksheet_FormClosed(object sender, FormClosedEventArgs e)
        {
            FreeAllResources();
            SaveSettings();
        }

        private void SaveSettings()
        {
            try
            {
                Settings.Default.FrmWorksheet_SplitterDistance = _utcTabs.Height;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                _log.Error("Error saving splitter distance");
                _log.Error(ex.Message, ex);
            }
        }

        private void FreeAllResources()
        {
            try
            {
                // Free all resources
                foreach (var tab in _utcTabs.Tabs)
                {
                    FreeTabResources(tab);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error freeing resources.");
                _log.Error(ex.Message, ex);
            }
        }

        private void UtcTabs_TabClosed(object sender, TabClosedEventArgs e)
        {
            try
            {
                FreeTabResources(e.Tab);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error releasing resources from {0}.", e.Tab.Text);
                _log.Error(ex.Message, ex);
            }
        }

        private static void FreeTabResources(UltraTab tab)
        {
            if (tab == null) throw new ArgumentNullException("tab");
            _log.DebugFormat("Releasing resources from  tab {0} ...", tab.Text);
            foreach (var resultsControl in tab.TabPage.Controls.OfType<UcQueryResults>())
            {
                try
                {
                    resultsControl.FreeResources();
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error releasing resources from {0}.", resultsControl.Name);
                    _log.Error(ex.Message, ex);
                }
            }
            foreach (var resultsControl in tab.TabPage.Controls.OfType<UcScriptResults>())
            {
                try
                {
                    resultsControl.FreeResources();
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error releasing resources from {0}.", resultsControl.Name);
                    _log.Error(ex.Message, ex);
                }
            }
            _log.DebugFormat("Releasing resources from  tab {0} finished.", tab.Text);
        }

        private void Utm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (_skipToolClickEvents) return;

            try
            {
                switch (e.Tool.Key)
                {
                    case "Run":
                        RunSingleQuery();
                        break;

                    case "Run Script":
                        RunScriptQuery();
                        break;
                    
                    case "Run from Files":
                        RunFromFiles();
                        break;

                    case "Clear":
                        _sqlEditor.DoEditAction(new SelectWholeDocument());
                        _sqlEditor.DoEditAction(new Delete());
                        break;

                    case "Save":
                        Save();
                        break;

                    case "Save As":
                        SaveAs();
                        break;

                    case "Cut":
                        _sqlEditor.DoEditAction(new Cut());
                        break;

                    case "Copy":
                        _sqlEditor.DoEditAction(new Copy());
                        break;

                    case "Paste":
                        _sqlEditor.DoEditAction(new Paste());
                        break;

                    case "Open":
                        OpenFile();
                        break;

                    case "Upper Case":
                        _sqlEditor.ToUpperCase();
                        break;

                    case "Lower Case":
                        _sqlEditor.ToLowerCase();
                        break;

                    case "Camel Case":
                        _sqlEditor.ToProperCase();
                        break;

                    case "Toggle Comment":
                        _sqlEditor.DoEditAction(new ToggleLineComment());
                        break;

                    case "Select All":
                        _sqlEditor.DoEditAction(new SelectWholeDocument());
                        break;

                    case "Undo":
                        _sqlEditor.DoEditAction(new Undo());
                        break;

                    case "Redo":
                        _sqlEditor.DoEditAction(new Redo());
                        break;
                    
                    case "Find":
                        _findForm.ShowFor(_sqlEditor, false);
                        break;

                    case "Replace":
                        _findForm.ShowFor(_sqlEditor, true);
                        break;

                    case "Auto Commit":
                        DatabaseConnection.AutoCommit = ((StateButtonTool) e.Tool).Checked;
                        break;
                }
            }
            catch (Exception ex)
            {
                const string message = "Error performing operation.";
                _log.Error(message);
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, message, ex.Message, ex.StackTrace);
            }
        }

        private void RunFromFiles()
        {
            var form = new FrmRunMultipleFiles(DatabaseConnection);
            form.ShowDialog();
        }

        private void OpenFile()
        {
            string selectedFile;
            var dialogResult = Dialog.ShowOpenFileDialog("Select a file", out selectedFile,
                                                 new[]
                                                     {
                                                         new CommonFileDialogFilter("SQL files", ".sql"),
                                                         new CommonFileDialogFilter("Text files", ".txt"),
                                                         new CommonFileDialogFilter("All files", ".*")
                                                     }, ".sql");
            if (dialogResult != CommonFileDialogResult.OK) return;
            
            if (_sqlEditor.Document.TextLength > 0)
            {
                var dialogresult = Dialog.ShowYesNoDialog("Confirm contents replacement?",
                                                          "Are you sure you want to open a file and replace the contents of the editor with the contents of the file?",
                                                          string.Empty, TaskDialogStandardIcon.None);
                if (dialogresult != TaskDialogResult.Yes) return;
            }
            _sqlEditor.LoadFile(selectedFile);
            WorksheetFile = selectedFile;
            IsModified = false;
        }

        private bool SaveAs()
        {
            string selectedFile;
            var dialogResult = Dialog.ShowSaveFileDialog("Select a file", out selectedFile,
                                                 new[]
                                                     {
                                                         new CommonFileDialogFilter("SQL files", ".sql"),
                                                         new CommonFileDialogFilter("Text files", ".txt"),
                                                         new CommonFileDialogFilter("All files", "*.*")
                                                     }, ".sql");
            if (dialogResult != CommonFileDialogResult.OK) return false;

            WorksheetFile = selectedFile;
            return Save();
        }

        private bool Save()
        {
            if (WorksheetFile.IsNullEmptyOrWhitespace())
            {
                return SaveAs();
            }
            else
            {
                _sqlEditor.SaveFile(WorksheetFile);
                IsModified = false;
                return true;
            }
        }

        private void RunScriptQuery()
        {
            var sql = _sqlEditor.GetQueryText(DatabaseConnection.DatabaseServer.SqlTerminators);
            if (sql.IsNullEmptyOrWhitespace())
            {
                throw new Exception("No query selected to run");
            }
            var sqlSplitter = new SqlHelpers.SqlTextExtractor(DatabaseConnection.DatabaseServer.SqlTerminators);
            var sqlStatements = sqlSplitter.SplitSqlStatements(sql);
            var control = FindUsableScriptResultsTab();
            control.RunQueryAsync(sqlStatements, DatabaseConnection.MaxResults);
            OnRunScript(new RunScriptEventArgs(sqlStatements, DatabaseConnection));
        }

        private void RunSingleQuery()
        {
            var sql = _sqlEditor.GetQueryText(DatabaseConnection.DatabaseServer.SqlTerminators);
            if (sql.IsNullEmptyOrWhitespace())
            {
                throw new Exception("No query selected to run");
            }
            var control = FindUsableResultsTab(sql);
            control.RunQueryAsync(sql, DatabaseConnection.MaxResults);
            OnRunQuery(new RunQueryEventArgs(sql, DatabaseConnection.Name, DatabaseConnection.MaxResults));
        }

        private void SqlEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Space)
            {
                e.SuppressKeyPress = true;
                try
                {
                    ShowIntellisense((char)e.KeyValue);
                }
                catch (Exception ex)
                {
                    _log.Error("Error showing intellisense.");
                    _log.Error(ex.Message, ex);
                    Dialog.ShowErrorDialog(Application.ProductName, "Error showing intellisense.", ex.Message,
                                           ex.StackTrace);
                }
            }
            else if (e.Control && e.KeyCode == Keys.F)
            {
                _findForm.ShowFor(_sqlEditor, false);
            }
            else if (e.Control && e.KeyCode == Keys.H)
            {
                _findForm.ShowFor(_sqlEditor, true);
            }
        }

        private void ShowIntellisense(char value)
        {
            _intellisenseData = IntellisenseManager.Instance.GetIntellisenseData(DatabaseConnection);
            if (_intellisenseData == null)
            {
                _log.Debug("Intellisense data is still generating.");
                return;
            }

            var startOffset = Math.Min(_sqlEditor.ActiveTextAreaControl.Caret.Offset, _sqlEditor.ActiveTextAreaControl.Document.TextLength - 1);
            var endOffset = _sqlEditor.ActiveTextAreaControl.Caret.Offset;
            var text = _sqlEditor.ActiveTextAreaControl.Document.GetText(startOffset, 1);
            //Debug.WriteLine(text);
            while (startOffset > 0 && !text.IsNullEmptyOrWhitespace() && DatabaseConnection.DatabaseServer.ValidIdentifierRegex.IsMatch(text))
            {
                --startOffset;
                text = _sqlEditor.ActiveTextAreaControl.Document.GetText(startOffset, 1);
            }
            //Debug.WriteLine("StartOffset (before code completion window): {0}.", startOffset);
            if (startOffset == _sqlEditor.ActiveTextAreaControl.Document.TextLength - 1)
            {
                ++startOffset;
            }
            //++startOffset;

            ICompletionDataProvider completionDataProvider = new CompleteWordCompletionProvider(_img16,
                                                                                                _intellisenseData, 18, 19, 19, 20);

            CodeCompletionWindow.ShowCompletionWindow(
                ParentForm, // The parent window for the completion window
                _sqlEditor, // The text editor to show the window for
                string.Empty, // Filename - will be passed back to the provider
                completionDataProvider, // Provider to get the list of possible completions
                value, // Key pressed - will be passed to the provider
                true,
                false,
                startOffset,
                endOffset
                );
        }

        private void SqlEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemPeriod)
            {
                try
                {
                    ShowIntellisense((char)e.KeyValue);
                }
                catch (Exception ex)
                {
                    _log.Error("Error showing intellisense.");
                    _log.Error(ex.Message, ex);
                    Dialog.ShowErrorDialog(Application.ProductName, "Error showing intellisense.", ex.Message,
                                           ex.StackTrace);
                }
            }   
        }

        private void FrmSqlWorksheet_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If there are no modifications to the worksheet, exit
            if (!IsModified || CloseAction.Instance.OnCloseSaveAction == OnCloseSaveAction.DiscardAll) return;

            // Prompt to save
            var taskdlg = new TaskDialog
            {
                Icon = TaskDialogStandardIcon.Information,
                Caption = Application.ProductName,
                InstructionText = "Do you want to save changes you made to the SQL worksheet " + Title + " ?"
            };

            var commandLinkSave = new TaskDialogCommandLink("buttonSave", "Save", "Save changes made to the worksheet.");
            commandLinkSave.Click += (o, args) =>
            {
                try
                {
                    if (!Save())
                    {
                        taskdlg.Close(TaskDialogResult.Cancel);
                    }
                    else
                    {
                        taskdlg.Close(TaskDialogResult.Ok);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error("Error saving worksheet.");
                    _log.Error(ex.Message, ex);
                    Dialog.ShowErrorDialog(Application.ProductName, "Error occurred saving the document. ", ex.Message, ex.StackTrace);
                    taskdlg.Close(TaskDialogResult.Cancel);
                }
            };
            taskdlg.Controls.Add(commandLinkSave);

            var commandLinkDiscard = new TaskDialogCommandLink("buttonDiscard", "Discard", "Discard changes made to this worksheet.");
            commandLinkDiscard.Click += (o, args) => taskdlg.Close(TaskDialogResult.No);
            taskdlg.Controls.Add(commandLinkDiscard);

            if (e.CloseReason != CloseReason.UserClosing)
            {
                var commandLinkDiscardAll = new TaskDialogCommandLink("buttonDiscardAll", "Discard All",
                                                                      "Discard changes on all worksheets.");
                commandLinkDiscardAll.Click += (o, args) =>
                                                   {
                                                       CloseAction.Instance.OnCloseSaveAction =
                                                           OnCloseSaveAction.DiscardAll;
                                                       taskdlg.Close(TaskDialogResult.No);
                                                   };
                taskdlg.Controls.Add(commandLinkDiscardAll);
            }
            var commandLinkCancel = new TaskDialogCommandLink("buttonCancel", "Cancel", "Cancel and return to the worksheet.");
            commandLinkCancel.Click += (o, args) => taskdlg.Close(TaskDialogResult.Cancel);
            taskdlg.Controls.Add(commandLinkCancel);

            var dialogResult = taskdlg.Show();
            if (dialogResult == TaskDialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
