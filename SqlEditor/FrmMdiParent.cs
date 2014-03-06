﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.AppStyling;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinEditors;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinTree;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.DatabaseExplorer;
using SqlEditor.DatabaseExplorer.ConnectionsImport;
using SqlEditor.DatabaseExplorer.TreeNodes;
using SqlEditor.DatabaseExplorer.TreeNodes.Base;
using SqlEditor.Databases;
using SqlEditor.Properties;
using Utilities.Forms;
using Utilities.Forms.Dialogs;
using Utilities.IO;
using Utilities.InfragisticsUtilities.UltraGridUtilities;
using Utilities.Text;
using log4net;
using SqlEditor.SqlHistory;
using log4net.Core;
using log4net.Repository.Hierarchy;
using Resources = SqlEditor.Properties.Resources;

namespace SqlEditor
{
    public sealed partial class FrmMdiParent : Form
    {
        private static FrmMdiParent _instance;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private bool _connectionsChanged;
        private bool _skipToolClickEvents;
        private readonly ToolBase _addFolderButtonTool;
        private readonly ToolBase _connectButtonTool;
        private readonly ToolBase _copyButtonTool;
        private readonly ToolBase _cloneButtonTool;
        private readonly ToolBase _deleteConnectionButtonTool;
        private readonly ToolBase _deleteFolderButtonTool;
        private readonly ToolBase _disconnectButtonTool;
        private readonly ToolBase _editConnectionButtonTool;
        private readonly ToolBase _editFolderButtonTool;
        private readonly ToolBase _exportConnectionButtonTool;
        private readonly ToolBase _importButtonTool;
        private readonly ToolBase _newConnectionButtonTool;
        private readonly ToolBase _newWorksheetButtonTool;
        private readonly ToolBase _refreshButtonTool;
        private readonly ToolBase _sortButtonTool;
        private readonly ToolBase _collapseAllButtonTool;
        private readonly ToolBase _expandAllButtonTool;

        // Stored procedure buttons
        private readonly ToolBase _storedProcedureEditButtonTool;
        private readonly ToolBase _storedProcedureScriptAsCreateButtonTool;
        private readonly ToolBase _storedProcedureScriptAsDropButtonTool;
        private readonly PopupMenuTool _storedProcedureScriptPopupMenu;

        // Table buttons
        private readonly ToolBase _tableDetailsButtonTool;
        private readonly ToolBase _tableScriptAsDropButtonTool;
        private readonly ToolBase _tableScriptAsCreateButtonTool;
        private readonly ToolBase _tableScriptAsInsertButtonTool;
        private readonly ToolBase _tableScriptAsUpdateButtonTool;
        private readonly ToolBase _tableScriptAsDeleteButtonTool;
        private readonly PopupMenuTool _tableScriptPopupMenu;

        private readonly StateButtonTool _showConnectionsPaneStateButtonTool;
        private readonly StateButtonTool _showSqlHistoryStateButtonTool;
        private readonly StateButtonTool _showLogStateButtonTool;
        private readonly StateButtonTool _debugLogLevelStateButtonTool;
        private readonly StateButtonTool _infoLogLevelStateButtonTool;
        private readonly StateButtonTool _warningLogLevelStateButtonTool;
        private readonly StateButtonTool _errorLogLevelStateButtonTool;
        private readonly PopupMenuTool _connectionsPopupMenu;

        // Command lists
        private readonly List<ToolBase> _tableCommands = new List<ToolBase>();
        private readonly List<ToolBase> _storedProcedureCommands = new List<ToolBase>();

        private readonly UltraTreeDropHightLightDrawFilter _treeDrawFilter = new UltraTreeDropHightLightDrawFilter();
        private readonly SqlHistoryList _sqlHistoryList = new SqlHistoryList();
        private readonly Task<List<TreeNodeBase>> _getConnectionNodesTask;
        

        public List<DatabaseConnection> Connections
        {
            get
            {
                var nodes =
                    ConnectionsTreeNode.Instance.Nodes.Cast<UltraTreeNode>().Flatten(x => x.Nodes.Cast<UltraTreeNode>())
                        .Where(x => x is ConnectionTreeNode).Cast<ConnectionTreeNode>().ToList();
                foreach (var node in nodes)
                {
                    node.DatabaseConnection.Folder = node.Parent.FullPath;
                }
                return nodes.Select(x => x.DatabaseConnection).ToList();
            }
        }

        private static string SqlHistoryXmlFileName
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "SqlHistory.xml");
            }
        }

        private static string ConnectionsXmlFileName
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Connections.xml");
            }
        }

        public static FrmMdiParent Instance
        {
            get { return _instance ?? (_instance = new FrmMdiParent()); }
        }

        public string StatusBarText
        {
            get
            {
                return _usbStatus.Text;                
            }
            set
            {
                _usbStatus.InvokeIfRequired(() =>
                                                {
                                                    _usbStatus.Text = value;
                                                });
            }
        }

        public FrmMdiParent()
        {
            // Form default style
            LoadDefaultStyle();

            // Run splash screen
            Task.Run(() => Application.Run(FrmSplash.Instance));

            // Initialize
            InitializeComponent();

            // Load connections
            _getConnectionNodesTask = RunGetConnectionNodesTask();

            // Load SQL history
            LoadSqlHistoryAsync();

            // Assign button names
            _connectionsPopupMenu = (PopupMenuTool)_utm.Tools["Connection - Context Menu"];
            _newWorksheetButtonTool = _utm.Tools["New SQL Worksheet"];
            _newConnectionButtonTool = _utm.Tools["New Connection"];
            _editConnectionButtonTool = _utm.Tools["Edit Connection"];
            _deleteConnectionButtonTool = _utm.Tools["Delete Connection"];
            _disconnectButtonTool = _utm.Tools["Disconnect"];
            _connectButtonTool = _utm.Tools["Connect"];
            _editConnectionButtonTool = _utm.Tools["Edit Connection"];
            _refreshButtonTool = _utm.Tools["Refresh"];
            _copyButtonTool = _utm.Tools["Copy Text"];
            _cloneButtonTool = _utm.Tools["Clone Connection"];
            _exportConnectionButtonTool = _utm.Tools["Export"];
            _sortButtonTool = _utm.Tools["Sort"];
            _importButtonTool = _utm.Tools["Import"];
            _addFolderButtonTool = _utm.Tools["New Folder"];
            _editFolderButtonTool = _utm.Tools["Edit Folder"];
            _deleteFolderButtonTool = _utm.Tools["Delete Folder"];
            _collapseAllButtonTool = _utm.Tools["Collapse All"];
            _expandAllButtonTool = _utm.Tools["Expand All"];
            _showConnectionsPaneStateButtonTool = (StateButtonTool)_utm.Tools["Windows - Connections"];
            _showSqlHistoryStateButtonTool = (StateButtonTool)_utm.Tools["Windows - SQL History"];
            _showLogStateButtonTool = (StateButtonTool)_utm.Tools["Windows - Log"];
            _debugLogLevelStateButtonTool = (StateButtonTool)_utm.Tools["Logging Tools - Debug"];
            _infoLogLevelStateButtonTool = (StateButtonTool)_utm.Tools["Logging Tools - Info"];
            _warningLogLevelStateButtonTool = (StateButtonTool)_utm.Tools["Logging Tools - Warning"];
            _errorLogLevelStateButtonTool = (StateButtonTool)_utm.Tools["Logging Tools - Error"];

            // Setup table commands
            _tableDetailsButtonTool = _utm.Tools["Tables - Details"];
            _tableScriptAsDropButtonTool = _utm.Tools["Tables - Script as - Drop"];
            _tableScriptAsCreateButtonTool = _utm.Tools["Tables - Script as - Create"];
            _tableScriptAsInsertButtonTool = _utm.Tools["Tables - Script as - Insert"];
            _tableScriptAsUpdateButtonTool = _utm.Tools["Tables - Script as - Update"];
            _tableScriptAsDeleteButtonTool = _utm.Tools["Tables - Script as - Delete"];
            _tableScriptPopupMenu = (PopupMenuTool)_utm.Tools["Script Table As - Context Menu"];
            _tableScriptPopupMenu.Tools.Clear();
            _tableScriptPopupMenu.Tools.Add(_tableScriptAsDropButtonTool);
            _tableScriptPopupMenu.Tools.Add(_tableScriptAsCreateButtonTool);
            _tableScriptPopupMenu.Tools.Add(_tableScriptAsInsertButtonTool);
            _tableScriptPopupMenu.Tools.Add(_tableScriptAsUpdateButtonTool);
            _tableScriptPopupMenu.Tools.Add(_tableScriptAsDeleteButtonTool);
            _tableCommands.Add(_tableDetailsButtonTool);
            _tableCommands.Add(_tableScriptPopupMenu);
            _tableCommands.Add(null);
            _tableCommands.Add(_copyButtonTool);
            _tableCommands.Add(null);
            _tableCommands.Add(_collapseAllButtonTool);
            _tableCommands.Add(_expandAllButtonTool);

            // Setup stored procedures commands
            _storedProcedureEditButtonTool = _utm.Tools["Stored Procedures - Edit"];
            _storedProcedureScriptAsCreateButtonTool = _utm.Tools["Stored Procedures - Script As - Create"];
            _storedProcedureScriptAsDropButtonTool = _utm.Tools["Stored Procedures - Script As - Drop"];
            _storedProcedureScriptPopupMenu = (PopupMenuTool)_utm.Tools["Stored Procedures - Script As"];
            _storedProcedureScriptPopupMenu.Tools.Clear();
            _storedProcedureScriptPopupMenu.Tools.Add(_storedProcedureScriptAsDropButtonTool);
            _storedProcedureScriptPopupMenu.Tools.Add(_storedProcedureScriptAsCreateButtonTool);
            _storedProcedureCommands.Add(_storedProcedureEditButtonTool);
            _storedProcedureCommands.Add(_storedProcedureScriptPopupMenu);
            _storedProcedureCommands.Add(null);
            _storedProcedureCommands.Add(_copyButtonTool);
            _storedProcedureCommands.Add(null);
            _storedProcedureCommands.Add(_collapseAllButtonTool);
            _storedProcedureCommands.Add(_expandAllButtonTool);

            // Set image list for nodes
            DatabaseExplorerImageList.Instance.ImageList = _iml16;

            // Load settings
            LoadSettings();
            
            // Set application title
            Text = Application.ProductName + " " + Application.ProductVersion;
            
            // Load main connections  node
            _utConnections.Nodes.Add(ConnectionsTreeNode.Instance);

            // Hook Event Handlers
            _connectionsPopupMenu.BeforeToolDropdown += Connections_PopupMenuBeforeToolDropdown;
            _treeDrawFilter.Invalidate += (sender, args) => _utConnections.Invalidate();
            _treeDrawFilter.QueryStateAllowedForNode += ConnectionsDrawFilterQueryStateAllowedForNode;
            ((EditorButton) _uteConnectionsSearch.ButtonsRight["Clear"]).Click += (sender, args) => _uteConnectionsSearch.Clear();
            _uteConnectionsSearch.TextChanged += Connections_SearchTextChanged;

            ToggleVisibleToolRibbonTab(false, "Logging Tools");
            _rtbLog.GotFocus += (sender, args) => ToggleVisibleToolRibbonTab(true, "Logging Tools");
            _rtbLog.LostFocus += (sender, args) => ToggleVisibleToolRibbonTab(false, "Logging Tools");

            ToggleVisibleToolRibbonTab(false, "Database Explorer Tools");
            _utConnections.GotFocus += (sender, args) => ToggleVisibleToolRibbonTab(true, "Database Explorer Tools");
            _utConnections.LostFocus += (sender, args) => ToggleVisibleToolRibbonTab(false, "Database Explorer Tools");

            ToggleVisibleToolRibbonTab(false, "SQL History Tools");
            _ugGrid.GotFocus += (sender, args) => ToggleVisibleToolRibbonTab(true, "SQL History Tools");
            _ugGrid.LostFocus += (sender, args) => ToggleVisibleToolRibbonTab(false, "SQL History Tools");
        }

        private void ToggleVisibleToolRibbonTab(bool isVisible, string tabKey)
        {
            var tab = _utm.Ribbon.ContextualTabGroups[tabKey];
            tab.Visible = isVisible;
            if (isVisible)
            {
                _utm.Ribbon.SelectedTab = tab.Tabs[0];
            }
        }

        private static void LoadDefaultStyle()
        {
            var styleName = string.IsNullOrEmpty(Settings.Default.FrmMdiParent_Style) ? "Office 2010 Blue" : Settings.Default.FrmMdiParent_Style;
            _log.DebugFormat("Loading style {0} ...", styleName);
            try
            {
                var styleData = GetStyleData(styleName);
                using (var memStream = new MemoryStream(styleData))
                {
                    if (memStream.Length > 0)
                    {
                        StyleManager.Load(memStream);
                        _log.DebugFormat("Style {0} loaded successfully.", styleName);
                    }
                    else
                    {
                        _log.ErrorFormat("Style data is empty.");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error loading style {0}.", styleName);
                _log.Error(ex.Message, ex);
            }
        }

        private void Connections_SearchTextChanged(object sender, EventArgs eventArgs)
        {
            Connections_Search(_uteConnectionsSearch.Text);
        }

        private void Connections_Search(string filterText)
        {
            try
            {
                _log.DebugFormat("Searching connections for text {0} ...", filterText);
                _utConnections.BeginUpdate();
                if (filterText.IsNullEmptyOrWhitespace())
                {
                    var nodes = ConnectionsTreeNode.Instance.Nodes.Cast<UltraTreeNode>().Flatten(x => x.Nodes.Cast<UltraTreeNode>()).ToList();
                    foreach (var node in nodes)
                    {
                        node.Visible = true;
                    }
                }
                else
                {
                    Connections_SearchHelper(ConnectionsTreeNode.Instance, filterText);
                    foreach (var node in ConnectionsTreeNode.Instance.Nodes)
                    {
                        Connections_SearchHelper(node, filterText);
                    }
                }
                _log.DebugFormat("Searching connections for text {0} complete.", filterText);
            }
            finally
            {
                _utConnections.EndUpdate();
            }
        }

        private static bool Connections_SearchHelper(UltraTreeNode node, string filterText)
        {
            if (node is DummyTreeNode)
            {
                node.Visible = true;
                return false;
            }

            var matches = node.Text.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
            foreach (var childNode in node.Nodes)
            {
                var childmatches = Connections_SearchHelper(childNode, filterText);
                matches = matches || childmatches;
            }
            node.Visible = matches;
            return matches;
        }

        private void LoadSettings()
        {
            try
            {
                _log.Debug("Loading form settings ...");
                _skipToolClickEvents = true;

                // Form position
                _log.Debug("Loading form geometry ...");
                if (!string.IsNullOrEmpty(Settings.Default.FrmMdiParent_Geometry))
                {
                    try
                    {
                        RestoreFormPosition.GeometryFromString(Settings.Default.FrmMdiParent_Geometry, this);
                        _log.Debug("Loading form geometry finished.");
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorFormat("Error restoring form geometry from string {0}.",
                                         Settings.Default.FrmMdiParent_Geometry);
                        _log.Error(ex.Message, ex);
                    }
                }
                else
                {
                    _log.Warn("Form geometry not set. Nothing will be loaded");
                }

                // Dock geometry
                _log.Debug("Loading dock geometry ...");
                if (!string.IsNullOrEmpty(Settings.Default.FrmMdiParent_Docks))
                {
                    try
                    {
                        using (var ms = new MemoryStream(Encoding.Default.GetBytes(Settings.Default.FrmMdiParent_Docks))
                            )
                        {
                            _udm.LoadFromXML(ms);
                        }
                        _log.Debug("Loading dock geometry finished.");
                    }
                    catch (Exception ex)
                    {
                        _log.ErrorFormat("Error restoring dock geometry from string {0}.",
                                         Settings.Default.FrmMdiParent_Geometry);
                        _log.Error(ex.Message, ex);
                    }
                }
                else
                {
                    _log.Warn("Dock geometry not set. Nothing will be loaded");
                }

                // Style
                var tool =
                    _utm.GetCategoryTools("Styles")
                        .OfType<StateButtonTool>()
                        .FirstOrDefault(x => x.Key == (Settings.Default.FrmMdiParent_Style ?? string.Empty));
                if (tool != null)
                {
                    tool.Checked = true;
                }

                // Log Level
                var level = GetLoggingLevelFromString(Settings.Default.LoggingLevel) ?? Level.Info;
                SetLoggingLevel(level);
            }
            catch (Exception ex)
            {
                _log.Error("Error loading settings.");
                _log.Error(ex.Message, ex);
            }
            finally
            {
                _skipToolClickEvents = false;
            }
        }

        private void SaveSettings()
        {
            try
            {
                _log.Debug("Saving form settings.");

                // Save form position
                Settings.Default.FrmMdiParent_Geometry = RestoreFormPosition.GeometryToString(this);
                Settings.Default.Save();
                
                // Save docks settings
                using (var ms = new MemoryStream())
                {
                    _udm.SaveAsXML(ms);
                    
                    ms.Position = 0;
                    using (var sReader = new StreamReader(ms))
                    {
                        var formattedXml = sReader.ReadToEnd();
                        Settings.Default.FrmMdiParent_Docks = formattedXml;
                    }
                    ms.Close();
                }
                Settings.Default.Save();

                // Save style
                var checkedButton = _utm.GetCategoryTools("Styles").OfType<StateButtonTool>().FirstOrDefault(x => x.Checked);
                if (checkedButton != null)
                {
                    Settings.Default.FrmMdiParent_Style = checkedButton.Key;
                    Settings.Default.Save();
                }

                // Save logging level
                Settings.Default.LoggingLevel = ((Logger) _log.Logger).Level.ToString();
                Settings.Default.Save();
                
                _log.Debug("Saving form settings finished.");
            }
            catch (Exception ex)
            {
                _log.Error("Error saving form geometry.");
                _log.Error(ex.Message, ex);
            }
        }

        private void Connections_ShowDetails(DatabaseObject databaseObject, DatabaseConnection databaseConnection)
        {
            var table = databaseObject as Table;
            if (table != null)
            {
                var frm = new FrmTableDetails(databaseConnection, table) { MdiParent = this };
                frm.Show();
            }
        }

        //private void SqlHistory_UseSql(UseSqlEventArgs e)
        //{
        //    var activeWorksheet = GetActiveWorksheet();
        //    if (activeWorksheet == null)
        //    {
        //        throw new Exception("There are no active SQL worksheets available");
        //    }
        //    activeWorksheet.AppendText(e.Sql);
        //}

        private void LoadStyle(string buttonName)
        {
            if (_skipToolClickEvents) return;

            _log.Debug("Loading style ....");
            var styleData = GetStyleData(buttonName);
            _skipToolClickEvents = true;
            try
            {
                using (var memStream = new MemoryStream(styleData))
                {
                    if (memStream.Length > 0)
                    {
                        StyleManager.Load(memStream);
                        UpdateCheckedStyle(buttonName);
                        _log.Debug("Style loaded successfully.");
                    }
                    else
                    {
                        _log.ErrorFormat("Style data is empty.");
                    }
                }
            }
            finally
            {
                _skipToolClickEvents = false;
            }
        }

        private static byte[] GetStyleData(string buttonName)
        {
            _log.DebugFormat("Getting style data for {0} ....", buttonName);
            switch (buttonName)
            {
                case "Office 2007 Blue":
                    return Resources.Office2007Blue;
                case "Office 2007 Silver":
                    return Resources.Office2007Silver;
                case "Office 2007 Black":
                    return Resources.Office2007Black;
                case "Office 2010 Blue":
                    return Resources.Office2010Blue;
                case "Office 2010 Silver":
                    return Resources.Office2010Silver;
                case "Office 2010 Black":
                    return Resources.Office2010Black;
                case "Metro":
                    return Resources.Metro;
                case "Aero":
                    return Resources.Aero;
                case "Windows7":
                    return Resources.Windows7;
                default:
                    throw new Exception("Could not find style data for " + buttonName);
            }
        }

        private FrmSqlWorksheet NewWorksheet(DatabaseConnection connection)
        {
            _log.DebugFormat("Opening new worksheet for connection {0} ...", connection.Name);
            FrmSqlWorksheet worksheet;
            using (new WaitActionStatus("Opening new worksheet ..."))
            {
                var title = "Untitled";
                var count = MdiChildren.OfType<FrmSqlWorksheet>().Count(x => x.DatabaseConnection == connection) + 1;
                title += count.ToString(CultureInfo.InvariantCulture);
                worksheet = new FrmSqlWorksheet(connection, title) { MdiParent = this };
                worksheet.RunQuery += WorksheetRunQuery;
                worksheet.RunScript += WorksheetRunScript;
                worksheet.Show();
            }
            SelectSqlTab();
            return worksheet;
        }

        private void WorksheetRunScript(object sender, RunScriptEventArgs e)
        {
            foreach (var statement in e.Statements)
            {
                var existingStatement = _sqlHistoryList.FirstOrDefault(s => s.SqlStatement == statement);
                if (existingStatement != null)
                {
                    existingStatement.RunDateTime = DateTime.Now;
                }
                else
                {
                    _sqlHistoryList.AddFirst(statement, e.DatabaseConnection.Name, DateTime.Now);
                }
            }
        }

        private void WorksheetRunQuery(object sender, RunQueryEventArgs e)
        {
            var statement =  _sqlHistoryList.FirstOrDefault(s => s.SqlStatement == e.Sql);
            if (statement != null)
            {
                statement.RunDateTime = DateTime.Now;
            }
            else
            {
                _sqlHistoryList.AddFirst(e.Sql, e.ConnectionName, DateTime.Now);
            }
        }

        
        private void FrmMdiParentShown(object sender, EventArgs e)
        {
            RichTextBoxAppender.SetRichTextBox(_rtbLog, "RichTextBoxAppender");

            try
            {
                _getConnectionNodesTask.Wait(3000);
                ConnectionsTreeNode.Instance.Load(_getConnectionNodesTask.Result.ToArray());
                ConnectionsTreeNode.Instance.Expanded = true;
                CloseSplashForm();
            }
            catch (AggregateException ex1)
            {
                CloseSplashForm();
                var ex = ex1.InnerException ?? ex1;
                _log.Error("Error loading connections.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog("Error", "Error loading connections.", ex.Message, ex.StackTrace);
            }
            catch (Exception ex)
            {
                CloseSplashForm();
                _log.Error("Error loading connections.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog("Error", "Error loading connections.", ex.Message, ex.StackTrace);
            }
            
            
            BringToFront();
            TopMost = true;
            Application.DoEvents();
            TopMost = false;
        }

        private static void CloseSplashForm()
        {
            try
            {
                FrmSplash.Instance.CloseForm();
            }
            catch (Exception ex)
            {
                _log.Error("Error closing splash form.");
                _log.Error(ex.Message, ex);
            }
        }

        private Task<List<TreeNodeBase>> RunGetConnectionNodesTask()
        {
            var task = Task.Run(() =>
                {
                    try
                    {
                        _log.Debug("Loading connections ...");
                        if (File.Exists(ConnectionsXmlFileName))
                        {
                            var list = SqlEditorConnectionImporter.ImportConnections(ConnectionsXmlFileName, EncryptionInfo.EncryptionKey);
                            var nodes = Connections_CreateNodes(list);
                            _log.Debug("Loading connections finished.");
                            return nodes;
                        }
                        else
                        {
                            _log.WarnFormat("Connections file {0} could not be found.", ConnectionsXmlFileName);
                            return new List<TreeNodeBase>();
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Error loading connections.");
                        _log.Error(ex.Message, ex);
                        throw new Exception("Error loading connections", ex);
                    }
                });
            return task;
        }     

        private void LoadSqlHistoryAsync()
        {
            Task.Run(() =>
                {
                    try
                    {
                        _log.Debug("Loading SQL history ...");
                        var executedSqlStatements = SqlHistoryImportExport.LoadExecutedSqlStatements(SqlHistoryXmlFileName);
                        _sqlHistoryList.Initialize(executedSqlStatements);
                        _ugGrid.InvokeIfRequired(() => _ugGrid.DataSource =  _sqlHistoryList);
                        _log.Info("SQL history loaded.");
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Error loading sql history.");
                        _log.Error(ex.Message, ex);
                    }
                });
        }

        private FrmSqlWorksheet GetActiveWorksheet()
        {
           return _utmdi.ActiveTab.Form as FrmSqlWorksheet;            
        }

        private void FrmMdiParentFormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
            
            try
            {
                Connections_Close();
                if (_connectionsChanged)
                {
                    _log.Debug("Connections have changed. Saving  changes ...");
                    SqlEditorConnectionImporter.ExportConnections(Connections, ConnectionsXmlFileName, EncryptionInfo.EncryptionKey);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error saving connections.");
                _log.Error(ex.Message, ex);
            }

            try
            {
                if (_sqlHistoryList.HasChanged)
                {
                    _log.Debug("SQL history has changed. Saving  changes ...");
                    var executedSqlStatements =
                        _sqlHistoryList.OrderByDescending(s => s.RunDateTime).Take(50).ToList();
                    SqlHistoryImportExport.SaveExecutedSqlStatements(executedSqlStatements, SqlHistoryXmlFileName);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error saving SQL history.");
                _log.Error(ex.Message, ex);
            }
        }

        private void Utm_ToolClick(object sender, ToolClickEventArgs e)
        {
            if (_skipToolClickEvents) return;
            try
            {
                switch (e.Tool.Key)
                {
                    case "About ...":
                        var frm = new FrmAbout();
                        frm.ShowDialog();
                        break;

                    case "Exit":
                        Close();
                        break;

                    case "Options": // ButtonTool
                        // Place code here
                        break;

                    case "Office 2007 Blue":
                    case "Office 2007 Silver":
                    case "Office 2007 Black":
                    case "Office 2010 Blue":
                    case "Office 2010 Silver":
                    case "Office 2010 Black":
                    case "Metro":
                    case "Aero":
                    case "Windows7":
                        LoadStyle(e.Tool.Key);
                        break;

                    case "New Connection":
                        Connections_NewConnection();
                        break;

                    case "Connect":
                        Connections_Connect();
                        break;

                    case "Delete Connection":
                        Connections_DeleteSelectedNodes();
                        break;

                    case "Rename":
                        Connections_RenameNode();
                        break;

                    case "Refresh":
                        Connections_Refresh();
                        break;

                    case "New Folder":
                        Connections_NewFolder();
                        break;

                    case "Delete Folder":
                        Connections_DeleteSelectedNodes();
                        break;

                    case "Sort":
                        Connections_Sort();
                        break;

                    case "Disconnect":
                        Connections_Disconnect();
                        break;

                    case "Export":
                        Connections_Export();
                        break;

                    case "Edit Connection":
                        Connections_Edit();
                        break;

                    case "Edit Folder":
                        Connections_RenameNode();
                        break;

                    case "New SQL Worksheet":
                        Connections_NewSqlWorksheet();
                        break;

                    case "Copy Text":
                        Connections_CopySelectedNodeText();
                        break;

                    case "Tables - Details":
                        Connections_ShowObjectDetails();
                        break;

                    case "Import Sql Editor Connections":
                        Connections_ImportFromSqlEditor();
                        break;

                    case "Import Aqua Data Studio Connections":
                        Connections_ImportFromAquaDataStudio();
                        break;

                    case "Collapse All":
                        ToggleExpandColapse(false, ConnectionsTreeNode.Instance.Nodes);
                        break;

                    case "Expand All":
                        ToggleExpandColapse(true, ConnectionsTreeNode.Instance.Nodes);
                        break;

                    case "Clone Connection":
                        Connections_Clone();
                        break;

                    case "Windows - SQL History":
                        ShowHideSqlHistoryWindow();
                        break;

                    case "Windows - Connections":
                        ShowHideConnectionsWindow();
                        break;

                    case "Windows - Log":
                        ShowHideLogWindow();
                        break;

                    case "Logging Tools - Copy":
                        _rtbLog.Copy();
                        break;

                    case "Logging Tools - ClearLog":
                        _rtbLog.Clear();
                        break;

                    case "Logging Tools - Save":
                        Logging_Save();
                        break;

                    case "SQL History - Use SQL":
                        AppendSqlToWorksheet();
                        break;

                    case "SQL History - Delete":
                        DeleteSqlFromHistory();
                        break;

                    case "SQL History - Copy":
                        _ugGrid.PerformAction(UltraGridAction.Copy);
                        break;

                    case "SQL History - Export to Excel":
                        _ugGrid.ExportToExcel();
                        break;

                    case "Logging Tools - Debug": 
                        SetLoggingLevel(Level.Debug);
                        break;

                    case "Logging Tools - Info":
                        SetLoggingLevel(Level.Info);
                        break;

                    case "Logging Tools - Warning":
                        SetLoggingLevel(Level.Warn);
                        break;

                    case "Logging Tools - Error":
                        SetLoggingLevel(Level.Error);
                        break;

                    case "Tables - Script as - Insert":
                        Connections_ScriptTableAsInsert();
                        break;

                    case "Tables - Script as - Update":
                        Connections_ScriptTableAsUpdate();
                        break;

                    case "Tables - Script as - Delete":
                        Connections_ScriptTableAsDelete();
                        break;

                    case "Tables - Script as - Drop":
                        Connections_ScriptTableAsDrop();
                        break;

                    case "Tables - Script as - Create": 
                        throw new NotImplementedException();
                        break;

                    case "Stored Procedures - Edit":
                    case "Stored Procedures - Script As - Create":
                        Connections_StoredProcedureEdit();
                        break;

                    case "Stored Procedures - Script As - Drop":
                        Connections_StoredProcedureDrop();
                        break;
                        
                }
            }
            catch (Exception ex)
            {
                const string message = "Error occurred during last operation.";
                _log.Error(message);
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, message, ex.Message, ex.StackTrace);
            }
            finally
            {
                RefreshUserInterface();
            }
        }

        private void Connections_StoredProcedureEdit()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }

            var selectedNode = _utConnections.SelectedNodes[0] as StoredProcedureTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("Stored procedure not selected.");
            }


            var databaseConnection = selectedNode.DatabaseConnection;
            var worksheet = NewWorksheet(databaseConnection);
            worksheet.Title = selectedNode.StoredProcedure.Name;
            var insertSql = selectedNode.StoredProcedure.Definition;
            worksheet.AppendText(insertSql);
        }

        private void Connections_StoredProcedureDrop()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }

            var selectedNode = _utConnections.SelectedNodes[0] as StoredProcedureTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("Stored procedure not selected.");
            }


            var databaseConnection = selectedNode.DatabaseConnection;
            var worksheet = NewWorksheet(databaseConnection);
            var insertSql = ObjectScripter.GenerateStoredProcedureDropStatement(selectedNode.StoredProcedure, databaseConnection);
            worksheet.AppendText(insertSql);
        }

        private async void Connections_ScriptTableAsDelete()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }

            var selectedNode = _utConnections.SelectedNodes[0] as TableTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("No table selected for delete");
            }

            var databaseConnection = selectedNode.DatabaseConnection;
            var worksheet = NewWorksheet(databaseConnection);
            var insertSql = await ObjectScripter.GenerateTableDeleteStatement(selectedNode.Table, databaseConnection);
            worksheet.AppendText(insertSql);
        }

        private void Connections_ScriptTableAsDrop()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }

            var selectedNode = _utConnections.SelectedNodes[0] as TableTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("No table selected for drop");
            }

            var databaseConnection = selectedNode.DatabaseConnection;
            var worksheet = NewWorksheet(databaseConnection);
            var insertSql = ObjectScripter.GenerateTableDropStatement(selectedNode.Table, databaseConnection);
            worksheet.AppendText(insertSql);
        }

        private async void Connections_ScriptTableAsInsert()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }

            var selectedNode = _utConnections.SelectedNodes[0] as TableTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("No table selected for insert");
            }

            var databaseConnection = selectedNode.DatabaseConnection;
            var worksheet = NewWorksheet(databaseConnection);
            var insertSql = await ObjectScripter.GenerateTableInsertStatement(selectedNode.Table, databaseConnection);
            worksheet.AppendText(insertSql);
        }

        private async void Connections_ScriptTableAsUpdate()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }

            var selectedNode = _utConnections.SelectedNodes[0] as TableTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("No table selected for update");
            }

            var databaseConnection = selectedNode.DatabaseConnection;
            var worksheet = NewWorksheet(databaseConnection);
            var insertSql = await ObjectScripter.GenerateTableUpdateStatement(selectedNode.Table, databaseConnection);
            worksheet.AppendText(insertSql);
        }

        private void Connections_Clone()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                throw new Exception("No node selected");
            }
            
            var selectedNode = _utConnections.SelectedNodes[0] as ConnectionTreeNode;
            if (selectedNode == null)
            {
                throw new Exception("Only connections can be cloned");
            }

            var connectionDetails = selectedNode.DatabaseConnection;
            var dbConnection = new DatabaseConnection
                {
                    AutoCommit = connectionDetails.AutoCommit,
                    ConnectionString = connectionDetails.ConnectionString,
                    DatabaseServer = connectionDetails.DatabaseServer,
                    Folder = connectionDetails.Folder,
                    IsConnected = false,
                    MaxResults = connectionDetails.MaxResults,
                    Name = connectionDetails.Name + " Copy"
                };
            var node = TreeNodeFactory.GetConnectionTreeNode(dbConnection);
            selectedNode.Parent.Nodes.Add(node);
        }

        private void DeleteSqlFromHistory()
        {
            var executedSqlStatements = _ugGrid.GetSelectedObjectList<ExecutedSqlStatement>();
            if (executedSqlStatements.Count == 0)
            {
                throw new Exception("No SQL statements are selected");
            }
            var dialogResult = Dialog.ShowYesNoDialog(Application.ProductName,
                                                      "Are you sure you want to delete selected SQL statements from history?",
                                                      executedSqlStatements.Count.ToString("#,0") +
                                                      " statement" +
                                                      (executedSqlStatements.Count > 1 ? "s" : string.Empty) +
                                                      " will be deleted.");
            if (dialogResult != TaskDialogResult.Yes) return;
            foreach (var executedSqlStatement in executedSqlStatements)
            {
                _sqlHistoryList.Remove(executedSqlStatement);
            }
        }

        private void AppendSqlToWorksheet()
        {
            var executedSqlStatements = _ugGrid.GetSelectedObjectList<ExecutedSqlStatement>();
            if (executedSqlStatements.Count == 0)
            {
                throw new Exception("No SQL statements are selected");
            }
            var sqlText = string.Join(Environment.NewLine, executedSqlStatements.Select(x => x.SqlStatement));
            var activeWorksheet = GetActiveWorksheet();
            if (activeWorksheet == null)
            {
                throw new Exception("There are no active SQL worksheets available");
            }
            activeWorksheet.AppendText(sqlText);
        }

        private void Logging_Save()
        {
            string file= null;
            try
            {
                var dialogresult = Dialog.ShowSaveFileDialog(Application.ProductName, out file,
                                                             new[]
                                                                 {
                                                                     new CommonFileDialogFilter("Log files", "*.log"),
                                                                     new CommonFileDialogFilter("All files", "*.*")
                                                                 });
                if (dialogresult != CommonFileDialogResult.OK) return;
                _rtbLog.SaveFile(file, RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error saving log to file {0}.", (file ?? "NULL"));
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        private void ShowHideLogWindow()
        {
            var pane = GetDockablePane(_rtbLog);
            if (_showLogStateButtonTool.Checked)
            {
                if (pane != null)
                {
                    pane.Show();
                }
            }
            else if (pane != null)
            {
                pane.Close();
            }
        }

        private void RefreshUserInterface()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(RefreshUserInterface));
                return;
            }

            try
            {
                _skipToolClickEvents = true;

                Connections_RefreshUserInterface();
            
                var connectionsPane = GetDockablePane(_tlpConnections);
                _showConnectionsPaneStateButtonTool.Checked = connectionsPane != null && connectionsPane.IsVisible;
                var logPane = GetDockablePane(_rtbLog);
                _showLogStateButtonTool.Checked = logPane != null && logPane.IsVisible;
                var sqlHistoryPane = GetDockablePane(_ugGrid);
                _showSqlHistoryStateButtonTool.Checked = sqlHistoryPane != null && sqlHistoryPane.IsVisible;
            }
            finally
            {
                _skipToolClickEvents = false;
            }
        }

        private void ShowHideConnectionsWindow()
        {
            var pane = GetDockablePane(_tlpConnections);
            if (_showConnectionsPaneStateButtonTool.Checked)
            {
                if (pane != null)
                {
                    pane.Show();
                }
            }
            else if (pane != null)
            {
                pane.Close();
            }
        }

        private DockableControlPane GetDockablePane(Control control)
        {
            return _udm.ControlPanes.Cast<DockableControlPane>().FirstOrDefault(x => x.Control == control);
        }

        private void ShowHideSqlHistoryWindow()
        {
            var pane = GetDockablePane(_ugGrid);
            if (_showSqlHistoryStateButtonTool.Checked)
            {
                if (pane != null)
                {
                    pane.Show();
                }
            }
            else if (pane != null)
            {
                pane.Close();
            }
        }

        private void ToggleExpandColapse(bool expand, TreeNodesCollection nodes)
        {
            try
            {
                _utConnections.BeginUpdate();
                ConnectionsTreeNode.Instance.Expanded = expand;
                foreach (var node in nodes)
                {
                    if (expand && !(node is ConnectionTreeNode))
                    {
                        node.Expanded = true;
                        ToggleExpandColapse(true, node.Nodes);
                    }
                    else if (!expand)
                    {
                        node.Expanded = false;
                        ToggleExpandColapse(false, node.Nodes);
                    }
                }
            }
            finally
            {
                _utConnections.EndUpdate();
            }
        }

        private void UpdateCheckedStyle(string buttonName)
        {
            try
            {
                var styleTools = _utm.GetCategoryTools("Styles").ToList();
                if (styleTools.Count == 0) throw new Exception("No Style tools are available");
                foreach (var tool in styleTools)
                {
                    ((StateButtonTool) tool).Checked = false;
                }
                ((StateButtonTool)_utm.Tools[buttonName]).Checked = true;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error updating checked style for button {0}.", buttonName);
                _log.Error(ex.Message, ex);
                throw;
            }
        }

        private void FrmMdiParent_Load(object sender, EventArgs e)
        {
            try
            {
                _utConnections.DrawFilter = _treeDrawFilter;

                CueProvider.SetCueText(_uteConnectionsSearch.Controls[0], "Search ...");

                RefreshUserInterface();
                _log.Info("ParentForm Shown event finished.");
            }
            catch (Exception ex)
            {
                _log.Error("Error in form loading event");
                _log.Error(ex.Message, ex);
                throw;
            }
        }


        #region Connection
        private void ConnectionsDrawFilterQueryStateAllowedForNode(object sender,
                                                                   UltraTreeDropHightLightDrawFilter.
                                                                       QueryStateAllowedForNodeEventArgs e)
        {
            //	Don't let folder nodes be dropped onto other connection nodes
            var folderNode = e.Node as FolderTreeNode;
            var connectionNode = e.Node as ConnectionTreeNode;
            var isFolderNode = folderNode != null;
            var isConnectionNode = connectionNode != null;

            if (e.Node == ConnectionsTreeNode.Instance)
            {
                e.StatesAllowed = DropLinePositionEnum.OnNode;
                _treeDrawFilter.EdgeSensitivity = e.Node.Bounds.Height/3;
            }
            else if (isFolderNode)
            {
                e.StatesAllowed = DropLinePositionEnum.All;
                _treeDrawFilter.EdgeSensitivity = e.Node.Bounds.Height/3;
            }
            else if (isConnectionNode && !e.Node.Selected)
            {
                e.StatesAllowed = DropLinePositionEnum.AboveNode | DropLinePositionEnum.BelowNode;
                _treeDrawFilter.EdgeSensitivity = e.Node.Bounds.Height/2;
            }
            else
            {
                e.StatesAllowed = DropLinePositionEnum.None;
            }
        }

        private void Connections_PopupMenuBeforeToolDropdown(object sender, BeforeToolDropdownEventArgs e)
        {
            Connections_RefreshUserInterface(true);
            if (_connectionsPopupMenu.Tools.Count == 0)
            {
                e.Cancel = true;
            }
        }

        private void Connections_RefreshUserInterface(bool refreshPopupMenu = false)
        {
            try
            {
                var selectedNode = _utConnections.SelectedNodes.Count > 0 ? _utConnections.SelectedNodes[0] : null;
                var isNodeSelected = _utConnections.SelectedNodes.Count > 0;
                var isRootNodeSelected = isNodeSelected &&
                                          selectedNode == ConnectionsTreeNode.Instance;
                var isConnectionNodeSelected = isNodeSelected && selectedNode is ConnectionTreeNode;
                var isFolderContainerNodeSelected = isNodeSelected && (selectedNode as FolderContainerTreeNode) != null;
                var isFolderNodeSelected = isFolderContainerNodeSelected && selectedNode is FolderTreeNode;
                var isTableNodeSelected = isNodeSelected && selectedNode is TableTreeNode;
                var isStoredProcedureNodeSelected = isNodeSelected && selectedNode is StoredProcedureTreeNode;
                var isViewNodeSelected = isNodeSelected && selectedNode is ViewTreeNode;
                var isMaterializedViewNodeSelected = isNodeSelected && selectedNode is ViewTreeNode;
                var isColumnNodeSelected = isNodeSelected && selectedNode is ColumnTreeNode;
                var isSequenceNodeSelected = isNodeSelected && selectedNode is SequenceTreeNode;
                // TODO: Fix to point to node
                // TODO: Add other node types
                //bool isIndexNodeSelected = isNodeSelected && selectedNode is IndexTreeNode;

                _newConnectionButtonTool.SharedProps.Enabled = isRootNodeSelected || isFolderNodeSelected;
                _editConnectionButtonTool.SharedProps.Enabled = isConnectionNodeSelected &&
                                                                !(((ConnectionTreeNode) selectedNode).DatabaseConnection.
                                                                                                      IsConnected);
                _deleteConnectionButtonTool.SharedProps.Enabled = isConnectionNodeSelected &&
                                                                  !(((ConnectionTreeNode) selectedNode).DatabaseConnection.
                                                                                                        IsConnected);
                _newWorksheetButtonTool.SharedProps.Enabled = isConnectionNodeSelected &&
                                                              ((ConnectionTreeNode) selectedNode).DatabaseConnection.
                                                                                                  IsConnected;
                _disconnectButtonTool.SharedProps.Enabled = isConnectionNodeSelected &&
                                                            ((ConnectionTreeNode) selectedNode).DatabaseConnection.
                                                                                                IsConnected;
                _connectButtonTool.SharedProps.Enabled = isConnectionNodeSelected &&
                                                         !(((ConnectionTreeNode) selectedNode).DatabaseConnection.
                                                                                               IsConnected);

                _refreshButtonTool.SharedProps.Enabled = isNodeSelected && !isColumnNodeSelected && !isSequenceNodeSelected;
                _exportConnectionButtonTool.SharedProps.Enabled = isRootNodeSelected;
                _importButtonTool.SharedProps.Enabled = isRootNodeSelected;
                _sortButtonTool.SharedProps.Enabled = isFolderContainerNodeSelected || isRootNodeSelected;

                _addFolderButtonTool.SharedProps.Enabled = isRootNodeSelected || isFolderNodeSelected;
                _editFolderButtonTool.SharedProps.Enabled = isFolderNodeSelected;
                _deleteFolderButtonTool.SharedProps.Enabled = isFolderNodeSelected;

                _copyButtonTool.SharedProps.Enabled = isNodeSelected;
                _cloneButtonTool.SharedProps.Enabled = isConnectionNodeSelected;
                _tableDetailsButtonTool.SharedProps.Enabled = isTableNodeSelected || isViewNodeSelected ||
                                                         isMaterializedViewNodeSelected; // TODO: Addothers to this list

                // Fix popup menu
                if (refreshPopupMenu)
                {
                    _connectionsPopupMenu.Tools.Clear();
                    if (isTableNodeSelected)
                    {
                        AddTools(_tableCommands, _connectionsPopupMenu);
                    }
                    else if (isStoredProcedureNodeSelected)
                    {
                        AddTools(_storedProcedureCommands, _connectionsPopupMenu);
                    }
                    else
                    {
                        string previousCategory = null;
                        var tools = _utm.GetCategoryTools("Connections").Where(x => x.SharedProps.Enabled && !(x is PopupMenuTool)).OrderBy(x => x.SharedProps.Priority).ThenBy(x => x.Tag ?? string.Empty).ToList();
                        foreach (var tool in tools)
                        {
                            _connectionsPopupMenu.Tools.Add(tool);
                            var currentCategory = (string) (tool.Tag ?? string.Empty);
                            if (currentCategory != previousCategory)
                            {
                                if (_connectionsPopupMenu.Tools.Count > 0)
                                {
                                    _connectionsPopupMenu.Tools[tool.Key].InstanceProps.IsFirstInGroup = true;
                                }
                                previousCategory = currentCategory;
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error refreshing user interface state.");
                _log.Error(ex.Message, ex);
            }
        }

        private static void AddTools(IEnumerable<ToolBase> toolCommands, PopupMenuTool popupMenu)
        {
            var isFirstInGroup = false;
            foreach (var tool in toolCommands)
            {
                if (tool == null)
                {
                    isFirstInGroup = true;
                    continue;
                }
                popupMenu.Tools.Add(tool);
                popupMenu.Tools[tool.Key].InstanceProps.IsFirstInGroup = isFirstInGroup;
                isFirstInGroup = false;
            }
        }

        private List<TreeNodeBase> Connections_CreateNodes(IList<DatabaseConnection> connections)
        {
            if (connections == null) throw new ArgumentNullException("connections");
            
            _log.Debug("Creating connections ...");
            var nodes = new List<TreeNodeBase>();

            _log.Debug("Creating folder nodes ...");
            var separator = _utConnections.PathSeparator;
            foreach (var dbConnection in connections)
            {
                dbConnection.PropertyChanged += (sender, args) => _connectionsChanged = true;
                var folderPaths = dbConnection.Folder.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                UltraTreeNode parentNode = null;
                for (var i = 1; i < folderPaths.Length; i++)
                {
                    var fullPath = string.Join(separator, folderPaths.Skip(1).Take(i).ToList());
                    var folderNode = nodes.Flatten(x => x.Nodes.Cast<TreeNodeBase>()).FirstOrDefault(x => x.FullPath == fullPath);
                    if (folderNode == null)
                    {
                        _log.DebugFormat("Creating new folder node {0} ...", folderPaths[i]);
                        folderNode = new FolderTreeNode(folderPaths[i]);
                        if (parentNode == null)
                        {
                            nodes.Add(folderNode);
                        }
                        else
                        {
                            parentNode.Nodes.Add(folderNode);
                        }
                        _log.DebugFormat("Created new folder node {0}.", folderNode.FullPath);
                    }
                    parentNode = folderNode;
                }
            }
            _log.Debug("Creating folders finished.");

            // Create connection nodes
            _log.Debug("Creating connection nodes ...");
            var folderNodes = nodes.Flatten(x => x.Nodes.Cast<TreeNodeBase>()).ToList();
            foreach (var dbConnection in connections)
            {
                //var connectionNode = new ConnectionTreeNode(dbConnection);
                var connectionNode = TreeNodeFactory.GetConnectionTreeNode(dbConnection);
                var firstSeparatorIndex = dbConnection.Folder.IndexOf(separator, StringComparison.Ordinal);
                if (firstSeparatorIndex >= 0)
                {
                    var fullPath = dbConnection.Folder.Substring(firstSeparatorIndex + 1);
                    var parentNode = folderNodes.FirstOrDefault(x => x.FullPath == fullPath);
                    if (parentNode == null) throw new Exception("Folder node " + fullPath + " was not created for connection " + dbConnection + " whose folder is " + dbConnection.Folder);
                    parentNode.Nodes.Add(connectionNode);
                }
                else
                {
                    nodes.Add(connectionNode);
                }
            }
            _log.Debug("Creating connection nodes finished.");
            return nodes;
        }

        public void Connections_LoadConnections(IList<DatabaseConnection> connections)
        {
            if (connections == null) throw new ArgumentNullException("connections");

            if (_utConnections.InvokeRequired)
            {
                _utConnections.Invoke(new MethodInvoker(() => Connections_LoadConnections(connections)));
                return;
            }


            _log.Debug("Loading connections ...");
            using (new WaitActionStatus("Loading connections ..."))
            {
                try
                {
                    _log.Debug("Loading tree control ...");
                    _utConnections.BeginUpdate();
                    ConnectionsTreeNode.Instance.Nodes.Clear();

                    // Create folders
                    _log.Debug("Creating folders ...");
                    var separator = _utConnections.PathSeparator;
                    foreach (var dbConnection in connections)
                    {
                        var folderPaths = dbConnection.Folder.Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
                        UltraTreeNode parentNode = null; // ConnectionsTreeNode.Instance;
                        for (var i = 1; i < folderPaths.Length; i++)
                        {
                            var fullPath = string.Join(separator, folderPaths.Take(i + 1).ToList());
                            var folderNode =
                                _utConnections.Nodes.Cast<UltraTreeNode>().Flatten<UltraTreeNode>(
                                    x => x.Nodes.Cast<UltraTreeNode>()).FirstOrDefault(x => x.FullPath == fullPath);
                            if (folderNode == null)
                            {
                                _log.DebugFormat("Creating new folder node {0} ...", folderPaths[i]);
                                folderNode = new FolderTreeNode(folderPaths[i]);
                                if (parentNode == null) throw new Exception("Parent node for connection " + dbConnection.Name + " could not be found");
                                parentNode.Nodes.Add(folderNode);
                                _log.DebugFormat("Created new folder node {0}.", parentNode.FullPath);
                            }
                            parentNode = folderNode;
                        }
                    }
                    _log.Debug("Creating folders finished.");

                    // Add connections
                    _log.Debug("Adding connections ...");
                    var folderNodes =
                        _utConnections.Nodes.Cast<UltraTreeNode>().Flatten<UltraTreeNode>(
                            x => x.Nodes.Cast<UltraTreeNode>()).ToList();
                    foreach (var dbConnection in connections)
                    {
                        dbConnection.PropertyChanged += (sender, args) => _connectionsChanged = true;
                        var parentNode = folderNodes.First(x => x.FullPath == dbConnection.Folder);
                        if (parentNode == null) throw new Exception("Parent node for connection " + dbConnection.Name + " could not be found");
                        var connectionNode = TreeNodeFactory.GetConnectionTreeNode(dbConnection);
                        parentNode.Nodes.Add(connectionNode);
                    }
                    _log.Debug("Adding connections finished.");
                    ConnectionsTreeNode.Instance.Expanded = true;
                }
                finally
                {
                    _utConnections.EndUpdate();
                }
            }
        }

        private void Connections_Sort()
        {
            if (_utConnections.SelectedNodes.Count == 0 ||
                (_utConnections.SelectedNodes[0] != ConnectionsTreeNode.Instance &&
                 !(_utConnections.SelectedNodes[0] is FolderContainerTreeNode)))
            {
                return;
            }

            _log.Info("Sorting connections ...");
            using (new WaitActionStatus("Sorting connections ..."))
            {
                try
                {
                    Connections_SortHelper(_utConnections.SelectedNodes[0]);
                }
                finally
                {
                    _utConnections.EndUpdate();
                    _connectionsChanged = true;
                }
            }
            _log.Info("Connections sorted.");
        }

        private static void Connections_SortHelper(UltraTreeNode node)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (node.Nodes.Count <= 1 || (node != ConnectionsTreeNode.Instance && !(node is FolderTreeNode)))
            {
                return;
            }

            List<UltraTreeNode> nodes = node.Nodes.Cast<UltraTreeNode>().ToList();
            node.Nodes.Clear();
            node.Nodes.AddRange(nodes.Where(x => x is FolderTreeNode).OrderBy(x => x.Text).ToArray());
            node.Nodes.AddRange(nodes.Where(x => !(x is FolderTreeNode)).OrderBy(x => x.Text).ToArray());
            foreach (UltraTreeNode treeNode in nodes)
            {
                Connections_SortHelper(treeNode);
            }
        }

        private void Connections_CopySelectedNodeText()
        {
            if (_utConnections.SelectedNodes.Count > 0)
            {
                var node = _utConnections.SelectedNodes[0];
                var text = node.Text;
                var treeNode = node as TreeNodeBase;
                if (treeNode != null)
                {
                    text = treeNode.GetClipboardText();
                }
                Clipboard.SetText(text);
            }
            else
            {
                throw new Exception("No nodes are selected");
            }
        }

        private void Connections_DeleteSelectedNodes()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                Dialog.ShowDialog("Invalid Action", "Nothing selected.", "You must select at least one node to delete.",
                                  TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Warning);
                return;
            }
            else if (
                _utConnections.SelectedNodes.Cast<UltraTreeNode>().Any(
                    x => !(x is ConnectionTreeNode) && !(x is FolderTreeNode)))
            {
                Dialog.ShowDialog("Invalid Action", "Cannot delete selected node(s).",
                                  "You can only delete connections and folders.", TaskDialogStandardButtons.Ok,
                                  TaskDialogStandardIcon.Warning);
                return;
            }

            var dialogResult = Dialog.ShowYesNoDialog("Confirm delete", "Confirm delete?",
                                                                   "Are you sure you want to delete selected connection(s) and/or folder(s)?");
            if (dialogResult == TaskDialogResult.Yes)
            {
                foreach (var selectedNode in _utConnections.SelectedNodes)
                {
                    var parent = selectedNode.Parent;
                    if (parent != null)
                    {
                        parent.Nodes.Remove(selectedNode);
                    }
                    //Connections_DeleteSelectedNodeHelper(selectedNode);
                }
                _connectionsChanged = true;
            }
        }

        private void Connections_Connect()
        {
            if (_utConnections.SelectedNodes.Count > 0)
            {
                var connectionNode = _utConnections.SelectedNodes[0] as ConnectionTreeNode;
                if (connectionNode != null)
                {
                    connectionNode.Expanded = true;
                }
            }
        }

        private void Connections_RenameNode()
        {
            if (_utConnections.SelectedNodes.Count > 0)
            {
                if (!(_utConnections.SelectedNodes[0] is ConnectionTreeNode) &&
                    !(_utConnections.SelectedNodes[0] is FolderTreeNode))
                {
                    Dialog.ShowDialog("Invalid Action", "Cannot rename selected node(s).",
                                      "You can only rename connections and folders.", TaskDialogStandardButtons.Ok,
                                      TaskDialogStandardIcon.Warning);
                    return;
                }

                UltraTreeNode selectedNode = _utConnections.SelectedNodes[0];
                if (selectedNode is ConnectionTreeNode || selectedNode is FolderTreeNode)
                {
                    selectedNode.BeginEdit();
                }
            }
        }

        private void Connections_NewConnection()
        {
            _log.Info("Creating a new connection ...");
            UltraTreeNode parentNode = ConnectionsTreeNode.Instance;
            var cn = new DatabaseConnection {Folder = parentNode.FullPath};
            if (_utConnections.SelectedNodes.Count > 0 && _utConnections.SelectedNodes[0] is FolderTreeNode)
            {
                parentNode = _utConnections.SelectedNodes[0] as FolderTreeNode;
                cn.Folder = parentNode.FullPath;
            }

            var frm = new FrmConnectionDetails(cn);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                cn.PropertyChanged += (sender, args) => _connectionsChanged = true;
                var connectionTreeNode = TreeNodeFactory.GetConnectionTreeNode(cn);
                parentNode.Nodes.Add(connectionTreeNode);
                _log.Info("Connection created.");
                _connectionsChanged = true;
            }
        }

        private void Connections_Disconnect()
        {
            using (new WaitActionStatus("Disconnecting ..."))
            {
                if (_utConnections.SelectedNodes.Count > 0)
                {
                    var connectionTreeNode = _utConnections.SelectedNodes[0] as ConnectionTreeNode;
                    if (connectionTreeNode != null)
                    {
                        _log.InfoFormat("Disconnecting from {0} ...", connectionTreeNode.DatabaseConnection.Name);
                        connectionTreeNode.Disconnect();
                        _log.InfoFormat("Disconnect successful.");
                    }
                }
            }
        }

        private async void Connections_AfterExpand(object sender, NodeEventArgs e)
        {
            _log.Debug("Handling node expanded event ...");   
            var expandedNode = e.TreeNode as TreeNodeBase;
            if (expandedNode == null)
            {
                _log.Warn("Expanded node is null.");
                return;
            }
            else if (expandedNode.IsLoaded)
            {
                _log.Debug("Expanded node is already loaded.");
                return;
            }
            else if (expandedNode.IsLoading)
            {
                _log.Debug("Expanded node is loading ...");
                return;
            }

            try
            {
                _log.Debug("Loading data ...");
                using (new WaitActionStatus("Loading data ..."))
                {
                    var connectionNode = expandedNode as ConnectionTreeNode;
                    var openWorksheet = connectionNode != null && !connectionNode.DatabaseConnection.IsConnected;
                    expandedNode.SubObjectPropChanged += change => RefreshUserInterface();
                    var nodes = await expandedNode.GetNodesAsync();
                    expandedNode.Nodes.Clear();
                    expandedNode.Nodes.AddRange(nodes.Cast<UltraTreeNode>().ToArray());
                    if (openWorksheet)
                    {
                        _log.Debug("Opening new worksheet ...");
                        connectionNode.DatabaseConnection.Connect();
                        NewWorksheet(connectionNode.DatabaseConnection);
                    }
                }
                _log.Debug("Data loaded successfully.");
            }
            catch (OperationCanceledException)
            {
                _log.Info("Operation cancelled.");
                expandedNode.Reset();
            }
            catch (Exception ex)
            {
                _log.Error("Error handling expand tree expand node event.");
                _log.Error(ex.Message, ex);
                expandedNode.Reset();
                Dialog.ShowErrorDialog("Error", "Error loading data.", ex.Message, ex.StackTrace);
            }
            finally
            {
                RefreshUserInterface();
            }
        }

        private void Connections_Refresh()
        {
            using (new WaitActionStatus("Reloading ..."))
            {
                if (_utConnections.SelectedNodes.Count > 0)
                {
                    var selectedNode = _utConnections.SelectedNodes[0];
                    var node = selectedNode as TreeNodeBase;
                    if (node != null)
                    {
                        node.ReloadAsync();
                    }
                }
            }
        }

        private void Connections_AfterSelect(object sender, SelectEventArgs e)
        {
            Connections_RefreshUserInterface();
        }

        private void Connections_AfterLabelEdit(object sender, NodeEventArgs e)
        {
            _connectionsChanged = true;
        }

        private void Connections_Close()
        {
            _log.Info("Closing any open connections ...");
            foreach (var databaseConnection in Connections)
            {
                databaseConnection.Disconnect();
            }
            _log.Info("Closed all open connections.");
        }

        private void Connections_SelectionDragStart(object sender, EventArgs e)
        {
            foreach (var node in _utConnections.SelectedNodes)
            {
                var connectionNode = node as ConnectionTreeNode;
                if (connectionNode == null)
                {
                    _treeDrawFilter.ClearDropHighlight();
                    return;
                }
            }
            _utConnections.DoDragDrop(_utConnections.SelectedNodes, DragDropEffects.Move);
        }

        private void Connections_DragDrop(object sender, DragEventArgs e)
        {
            //Set the DropNode
            UltraTreeNode dropNode = _treeDrawFilter.DropHightLightNode;

            //Get the Data and put it into a SelectedNodes collection,
            //then clone it and work with the clone
            //These are the nodes that are being dragged and dropped
            var selectedNodes = (SelectedNodesCollection) e.Data.GetData(typeof (SelectedNodesCollection));
            selectedNodes = selectedNodes.Clone() as SelectedNodesCollection;

            //Sort the selected nodes into their visible position. 
            //This is done so that they stay in the same order when
            //they are repositioned. 
            if (selectedNodes != null)
            {
                selectedNodes.SortByPosition();

                //Determine where we are dropping based on the current
                //DropLinePosition of the DrawFilter
                UltraTreeNode aNode;
                int i;
                switch (_treeDrawFilter.DropLinePosition)
                {
                    case DropLinePositionEnum.OnNode: //Drop ON the node
                        {
                            //Loop through the SelectedNodes and reposition
                            //them to the node that was dropped on.
                            //Note that the DrawFilter keeps track of what
                            //node the mouse is over, so we can just use
                            //DropHighLightNode as the drop target. 
                            for (i = 0; i <= (selectedNodes.Count - 1); i++)
                            {
                                aNode = selectedNodes[i];
                                aNode.Reposition(dropNode.Nodes);
                            }
                            break;
                        }
                    case DropLinePositionEnum.BelowNode: //Drop Below the node
                        {
                            for (i = 0; i <= (selectedNodes.Count - 1); i++)
                            {
                                aNode = selectedNodes[i];
                                aNode.Reposition(dropNode, NodePosition.Next);
                                //Change the DropNode to the node that was
                                //just repositioned so that the next 
                                //added node goes below it. 
                                dropNode = aNode;
                            }
                            break;
                        }
                    case DropLinePositionEnum.AboveNode: //New Index should be the same as the Drop
                        {
                            for (i = 0; i <= (selectedNodes.Count - 1); i++)
                            {
                                aNode = selectedNodes[i];
                                aNode.Reposition(dropNode, NodePosition.Previous);
                            }
                            break;
                        }
                }
                _connectionsChanged = true;
            }

            //After the drop is complete, erase the current drop
            //highlight. 
            _treeDrawFilter.ClearDropHighlight();
        }

        private void Connections_DragOver(object sender, DragEventArgs e)
        {
            //Get the position of the mouse in the tree, as opposed
            //to form coords
            Point pointInTree = _utConnections.PointToClient(new Point(e.X, e.Y));

            //Get the node the mouse is over.
            UltraTreeNode aNode = _utConnections.GetNodeFromPoint(pointInTree);

            //Make sure the mouse is over a node
            if (aNode == null)
            {
                //The Mouse is not over a node
                //Do not allow dropping here
                e.Effect = DragDropEffects.None;
                //Erase any DropHighlight
                _treeDrawFilter.ClearDropHighlight();
                //Exit stage left
                return;
            }


            //	Don't let continent droping on anything but folder nodes
            var folderNode = aNode as FolderTreeNode;
            var connectionNode = aNode as ConnectionTreeNode;
            if (folderNode == null && connectionNode == null && aNode != ConnectionsTreeNode.Instance)
            {
                if (pointInTree.Y > (aNode.Bounds.Top + 2) &&
                    pointInTree.Y < (aNode.Bounds.Bottom - 2))
                {
                    e.Effect = DragDropEffects.None;
                    _treeDrawFilter.ClearDropHighlight();
                    return;
                }
            }

            // Don't let dropping on parent nodes
            if (_utConnections.SelectedNodes.Count == 0)
            {
                e.Effect = DragDropEffects.None;
                _treeDrawFilter.ClearDropHighlight();
                return;
            }


            //var selectedNode = _utConnections.SelectedNodes[0];
            //var parent = selectedNode.Parent;
            //while (parent != null)
            //{
            //    if (parent == aNode)
            //    {
            //        e.Effect = DragDropEffects.None;
            //        _treeDrawFilter.ClearDropHighlight();
            //        return;
            //    }
            //}            

            //If we//ve reached this point, it//s okay to drop on this node
            //Tell the DrawFilter where we are by calling SetDropHighlightNode
            _treeDrawFilter.SetDropHighlightNode(aNode, pointInTree);
            //Allow Dropping here. 
            e.Effect = DragDropEffects.Move;
        }

        private void Connections_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            //Did the user press escape? 
            if (e.EscapePressed)
            {
                //User pressed escape
                //Cancel the Drag
                e.Action = DragAction.Cancel;
                //Clear the Drop highlight, since we are no longer
                //dragging
                _treeDrawFilter.ClearDropHighlight();
            }
        }

        private void Connections_ImportFromAquaDataStudio()
        {
            using (new WaitActionStatus("Importing connections ..."))
            {
                string selectedDirectory;
                var dialogResult = Dialog.ShowFolderDialog("Select connections directory", out selectedDirectory);
                if (dialogResult != CommonFileDialogResult.OK) return;

                _log.DebugFormat("Importing connections from {0} ...", selectedDirectory);
                var connections = AquaDataStudioConnectionImporter.ImportConnections(selectedDirectory);
                _log.DebugFormat("Loaded {0} connection(s).", connections.Count.ToString("#,0"));
                var dialogResult2 = Dialog.ShowYesNoDialog(Application.ProductName,
                                                           "Confirm connection import?",
                                                           "Are you sure you want to delete all existing connections and import " +
                                                           connections.Count.ToString("#,0") +
                                                           " connection(s)?");
                if (dialogResult2 != TaskDialogResult.Yes) return;
                Connections_LoadConnections(connections);
                _log.Debug("Importing connections finished.");
            }
        }

        private void Connections_ImportFromSqlEditor()
        {
            using (new WaitActionStatus("Importing connections ..."))
            {
                string selectedFile;
                var dialogResult = Dialog.ShowOpenFileDialog("Select connections file", out selectedFile,
                                                                            new[]
                                                                                {
                                                                                    new CommonFileDialogFilter(
                                                                                        "XML files", ".xml")
                                                                                }, ".xml");
                if (dialogResult == CommonFileDialogResult.OK)
                {
                    _log.DebugFormat("Importing connections from {0} ...", selectedFile);
                    var connections = SqlEditorConnectionImporter.ImportConnections(selectedFile, EncryptionInfo.EncryptionKey);
                    _log.DebugFormat("Loaded {0} connection(s).", connections.Count.ToString("#,0"));
                    var dialogResult2 = Dialog.ShowYesNoDialog(Application.ProductName,
                                                                            "Confirm connection import?",
                                                                            "Are you sure you want to delete all existing connections and import " +
                                                                            connections.Count.ToString("#,0") +
                                                                            " connection(s)?");
                    if (dialogResult2 != TaskDialogResult.Yes) return;
                    Connections_LoadConnections(connections);
                    _log.Debug("Importing connections finished.");
                }
            }
        }

        private void Connections_ShowObjectDetails()
        {
            if (_utConnections.SelectedNodes.Count > 0)
            {
                var selectedNode = (TreeNodeBase) _utConnections.SelectedNodes[0];
                var tableNode = selectedNode as TableTreeNode;
                if (tableNode != null)
                {
                    Connections_ShowDetails(tableNode.Table, tableNode.DatabaseConnection);
                    //OnShowDetails(tableNode.Table, tableNode.DatabaseConnection);
                    return;
                }
                var indexNode = selectedNode as IndexTreeNode;
                if (indexNode != null)
                {
                    Connections_ShowDetails(indexNode.IndexObject, indexNode.DatabaseConnection);
                    //OnShowDetails(indexNode.TableIndex, indexNode.DatabaseConnection);
                    return;
                }
                var viewNode = selectedNode as ViewTreeNode;
                if (viewNode != null)
                {
                    Connections_ShowDetails(viewNode.View, viewNode.DatabaseConnection);
                    // OnShowDetails(viewNode.View, viewNode.DatabaseConnection);
                    return;
                }
                //var mviewNode = selectedNode as MaterializedViewTreeNode;
                //if (mviewNode != null)
                //{
                //    OnShowDetails(mviewNode.View);
                //    return;
                //}
            }
        }

        private void Connections_Edit()
        {
            if (_utConnections.SelectedNodes.Count == 0)
            {
                return;
            }
            var connectionNode = _utConnections.SelectedNodes[0] as ConnectionTreeNode;
            if (connectionNode == null)
            {
                return;
            }
            _log.Info("Editing connection ...");
            var frm = new FrmConnectionDetails(connectionNode.DatabaseConnection);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                _connectionsChanged = true;
                _log.Info("Connection edited.");
            }
        }

        private void Connections_NewSqlWorksheet()
        {
            if (_utConnections.SelectedNodes.Count > 0)
            {
                var selectedNode = _utConnections.SelectedNodes[0];
                var node = selectedNode as ConnectionTreeNode;
                if (node != null)
                {
                    _log.Debug("Opening new SQL worksheet ...");
                    NewWorksheet(node.DatabaseConnection);
                }
            }
        }

        private void Connections_Export()
        {
            using (new WaitActionStatus("Exporting connections ..."))
            {
                string selectedFile;
                var dialogResult = Dialog.ShowOpenFileDialog("Select connections file", out selectedFile,
                                                                            new[]
                                                                                {
                                                                                    new CommonFileDialogFilter(
                                                                                        "XML files", ".xml")
                                                                                }, ".xml");
                if (dialogResult == CommonFileDialogResult.OK)
                {
                    _log.DebugFormat("Exporting connections to {0} ...", selectedFile);
                    SqlEditorConnectionImporter.ExportConnections(Connections, selectedFile, EncryptionInfo.EncryptionKey);
                    _log.Debug("Export finished.");
                }
            }
        }

        private void Connections_NewFolder()
        {
            if (_utConnections.SelectedNodes.Count > 0)
            {
                UltraTreeNode selectedNode = _utConnections.SelectedNodes[0];
                if (selectedNode is ConnectionsTreeNode || selectedNode is FolderTreeNode)
                {
                    var childNode = new FolderTreeNode("Untitled");
                    selectedNode.Nodes.Add(childNode);
                    childNode.BeginEdit();
                }
            }
        }

        private void Connections_KeyDown(object sender, KeyEventArgs e)
        {
            if (_utConnections.SelectedNodes.Count > 0 && e.KeyCode == Keys.C && e.Control)
            {
                Connections_CopySelectedNodeText();
            }
        }
        #endregion

        private void Udm_PaneHidden(object sender, PaneHiddenEventArgs e)
        {
            RefreshUserInterface();
        }

        private void Udm_AfterDockChange(object sender, PaneEventArgs e)
        {
            RefreshUserInterface();
        }

        private void Udm_AfterHideFlyout(object sender, ControlPaneEventArgs e)
        {
            RefreshUserInterface();
        }

        private void UgSqlHistory_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            _ugGrid.SetColumnFormat(typeof(DateTime), "yyyy-MM-dd HH:mm:ss");
            foreach (var band in e.Layout.Bands)
            {
                foreach (var column in band.Columns)
                {
                    column.CellActivation = Activation.ActivateOnly;
                }
            }
        }

        private void SetLoggingLevel(Level level)
        {
            try
            {
                _skipToolClickEvents = true;
                _log.InfoFormat("Setting log level to {0}.", level);
                
                _debugLogLevelStateButtonTool.Checked = level == Level.Debug;
                _infoLogLevelStateButtonTool.Checked = level == Level.Info;
                _warningLogLevelStateButtonTool.Checked = level == Level.Warn;
                _errorLogLevelStateButtonTool.Checked = level == Level.Error;

                Utilities.Logging.Log4NetHelper.SetLogLevel(level);
                
                _log.InfoFormat("Log level set to {0}.", level);
            }
            catch (Exception ex)
            {
                const string message = "Error setting log level";
                _log.ErrorFormat(message);
                _log.Error(ex.Message, ex);
                throw new Exception(message + ". " + ex.Message);
            }
            finally
            {
                _skipToolClickEvents = false;
            }
        }

        private Level GetLoggingLevelFromString(string logLevel)
        {
            if (logLevel.ToUpper() == "DEBUG")
            {
                return Level.Debug;
            }
            else if (logLevel.ToUpper() == "INFO")
            {
                return Level.Info;
            }
            else if (logLevel.ToUpper() == "WARNING" || logLevel.ToUpper() == "WARN")
            {
                return Level.Warn;
            }
            else if (logLevel.ToUpper() == "ERROR")
            {
                return Level.Error;
            }
            return null;
        }

        private void Connections_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var nodeAtPoint = _utConnections.GetNodeFromPoint(e.Location);
                if (nodeAtPoint != null && !nodeAtPoint.Selected)
                {
                    _utConnections.SelectedNodes.Clear();
                    nodeAtPoint.Selected = true;
                    _utConnections.ActiveNode = nodeAtPoint;
                }
            }
        }

        private void Utmdi_TabActivated(object sender, Infragistics.Win.UltraWinTabbedMdi.MdiTabEventArgs e)
        {
            SelectSqlTab();
        }

        private void SelectSqlTab()
        {
            try
            {
                var ribbonTab = _utm.Ribbon.Tabs.Cast<RibbonTab>().FirstOrDefault(x => x.Key == "_mdichild.merge.ribbontab.key__sql");
                if (ribbonTab != null) _utm.Ribbon.SelectedTab = ribbonTab;
            }
            catch (Exception ex)
            {
                _log.Error("Error setting selected ribbon tab to SQL tab");
                _log.Error(ex.Message, ex);
            }
        }
    }
}
