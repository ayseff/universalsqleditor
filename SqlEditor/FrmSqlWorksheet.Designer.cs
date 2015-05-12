using System.Windows.Forms;
using SqlEditor.SqlTextEditor;

namespace SqlEditor
{
    sealed partial class FrmSqlWorksheet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save As");
            Infragistics.Win.UltraWinToolbars.RibbonTab ribbonTab1 = new Infragistics.Win.UltraWinToolbars.RibbonTab("SQL");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup1 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Run");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run Script");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool53 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run from Files");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool54 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Show Explain Plan");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup2 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Connection");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Auto Commit", "");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool3 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("Max Results");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup3 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("File");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save As");
            Infragistics.Win.UltraWinToolbars.RibbonGroup ribbonGroup4 = new Infragistics.Win.UltraWinToolbars.RibbonGroup("Edit");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cut");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clear");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Select All");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Toggle Comment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Upper Case");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Lower Case");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Undo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Redo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool50 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool51 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Replace");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Undo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Redo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run Script");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clear");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save As");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cut");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("Max Results");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Open");
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Upper Case");
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Lower Case");
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Camel Case");
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Toggle Comment");
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Select All");
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Undo");
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Redo");
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Sql Editor Menu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Cut");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Paste");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Toggle Comment");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Upper Case");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Lower Case");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Select All");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool49 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Replace");
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Auto Commit", "");
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool52 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Run from Files");
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Show Explain Plan");
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSqlWorksheet));
            this.Worksheet2_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._sqlEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this._utcTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._Worksheet2_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._utm = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._img32 = new System.Windows.Forms.ImageList(this.components);
            this._img16 = new System.Windows.Forms.ImageList(this.components);
            this._Worksheet2_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Worksheet2_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._Worksheet2_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ucQueryResults1 = new SqlEditor.QueryResults.UcQueryResults();
            this.Worksheet2_Fill_Panel.ClientArea.SuspendLayout();
            this.Worksheet2_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).BeginInit();
            this._utcTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).BeginInit();
            this.SuspendLayout();
            // 
            // Worksheet2_Fill_Panel
            // 
            // 
            // Worksheet2_Fill_Panel.ClientArea
            // 
            this.Worksheet2_Fill_Panel.ClientArea.Controls.Add(this._sqlEditor);
            this.Worksheet2_Fill_Panel.ClientArea.Controls.Add(this.ultraSplitter1);
            this.Worksheet2_Fill_Panel.ClientArea.Controls.Add(this._utcTabs);
            this.Worksheet2_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.Worksheet2_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Worksheet2_Fill_Panel.Location = new System.Drawing.Point(4, 146);
            this.Worksheet2_Fill_Panel.Name = "Worksheet2_Fill_Panel";
            this.Worksheet2_Fill_Panel.Size = new System.Drawing.Size(1191, 499);
            this.Worksheet2_Fill_Panel.TabIndex = 0;
            // 
            // _sqlEditor
            // 
            this._sqlEditor.AllowDrop = true;
            this._utm.SetContextMenuUltra(this._sqlEditor, "Sql Editor Menu");
            this._sqlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._sqlEditor.IsReadOnly = false;
            this._sqlEditor.Location = new System.Drawing.Point(0, 0);
            this._sqlEditor.Name = "_sqlEditor";
            this._sqlEditor.Size = new System.Drawing.Size(1191, 223);
            this._sqlEditor.TabIndex = 4;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.SystemColors.Control;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 223);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 154;
            this.ultraSplitter1.Size = new System.Drawing.Size(1191, 6);
            this.ultraSplitter1.TabIndex = 3;
            // 
            // _utcTabs
            // 
            this._utcTabs.CloseButtonLocation = Infragistics.Win.UltraWinTabs.TabCloseButtonLocation.Tab;
            this._utcTabs.Controls.Add(this.ultraTabSharedControlsPage1);
            this._utcTabs.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._utcTabs.Location = new System.Drawing.Point(0, 229);
            this._utcTabs.Name = "_utcTabs";
            this._utcTabs.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this._utcTabs.Size = new System.Drawing.Size(1191, 270);
            this._utcTabs.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.Excel;
            this._utcTabs.TabCloseButtonAlignment = Infragistics.Win.UltraWinTabs.TabCloseButtonAlignment.AfterContent;
            this._utcTabs.TabCloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.WhenSelected;
            this._utcTabs.TabIndex = 0;
            this._utcTabs.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            this._utcTabs.TabClosing += new Infragistics.Win.UltraWinTabControl.TabClosingEventHandler(this.UtcTabs_TabClosing);
            this._utcTabs.TabClosed += new Infragistics.Win.UltraWinTabControl.TabClosedEventHandler(this.UtcTabs_TabClosed);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1189, 249);
            // 
            // _Worksheet2_Toolbars_Dock_Area_Left
            // 
            this._Worksheet2_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Worksheet2_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Worksheet2_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._Worksheet2_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Worksheet2_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._Worksheet2_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 146);
            this._Worksheet2_Toolbars_Dock_Area_Left.Name = "_Worksheet2_Toolbars_Dock_Area_Left";
            this._Worksheet2_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 499);
            this._Worksheet2_Toolbars_Dock_Area_Left.ToolbarsManager = this._utm;
            // 
            // _utm
            // 
            this._utm.DesignerFlags = 1;
            this._utm.DockWithinContainer = this;
            this._utm.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this._utm.ImageListLarge = this._img32;
            this._utm.ImageListSmall = this._img16;
            this._utm.Office2007UICompatibility = false;
            this._utm.Ribbon.ApplicationMenu.ToolAreaLeft.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool21,
            buttonTool29,
            buttonTool30});
            ribbonTab1.Caption = "SQL";
            ribbonGroup1.Caption = "Run";
            buttonTool18.InstanceProps.PreferredSizeOnRibbon = Infragistics.Win.UltraWinToolbars.RibbonToolSize.Large;
            ribbonGroup1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool18,
            buttonTool19,
            buttonTool53,
            buttonTool54});
            ribbonGroup2.Caption = "Connection";
            ribbonGroup2.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool2,
            textBoxTool3});
            ribbonGroup3.Caption = "File";
            ribbonGroup3.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool45,
            buttonTool46,
            buttonTool47});
            ribbonGroup4.Caption = "Edit";
            buttonTool26.InstanceProps.IsFirstInGroup = true;
            buttonTool33.InstanceProps.IsFirstInGroup = true;
            buttonTool22.InstanceProps.IsFirstInGroup = true;
            buttonTool50.InstanceProps.IsFirstInGroup = true;
            ribbonGroup4.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool26,
            buttonTool25,
            buttonTool27,
            buttonTool37,
            buttonTool38,
            buttonTool33,
            buttonTool35,
            buttonTool36,
            buttonTool22,
            buttonTool23,
            buttonTool50,
            buttonTool51});
            ribbonTab1.Groups.AddRange(new Infragistics.Win.UltraWinToolbars.RibbonGroup[] {
            ribbonGroup1,
            ribbonGroup2,
            ribbonGroup3,
            ribbonGroup4});
            this._utm.Ribbon.NonInheritedRibbonTabs.AddRange(new Infragistics.Win.UltraWinToolbars.RibbonTab[] {
            ribbonTab1});
            buttonTool16.InstanceProps.IsFirstInGroup = true;
            this._utm.Ribbon.QuickAccessToolbar.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool24,
            buttonTool20,
            buttonTool16,
            buttonTool17});
            this._utm.Ribbon.Visible = true;
            this._utm.ShowFullMenusDelay = 500;
            this._utm.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.Office2010;
            appearance1.Image = 3;
            buttonTool1.SharedPropsInternal.AppearancesLarge.Appearance = appearance1;
            appearance2.Image = 3;
            buttonTool1.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool1.SharedPropsInternal.Caption = "Run";
            buttonTool1.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.F9;
            buttonTool1.SharedPropsInternal.ToolTipText = "Runs selected SQL statement.";
            buttonTool1.SharedPropsInternal.ToolTipTitle = "Run";
            appearance3.Image = 4;
            buttonTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance3;
            appearance4.Image = 4;
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool2.SharedPropsInternal.Caption = "Run as Script";
            buttonTool2.SharedPropsInternal.ToolTipText = "Runs selected SQL statements as script.";
            buttonTool2.SharedPropsInternal.ToolTipTitle = "Run as Script";
            appearance5.Image = 5;
            buttonTool3.SharedPropsInternal.AppearancesLarge.Appearance = appearance5;
            appearance6.Image = 5;
            buttonTool3.SharedPropsInternal.AppearancesSmall.Appearance = appearance6;
            buttonTool3.SharedPropsInternal.Caption = "Clear";
            buttonTool3.SharedPropsInternal.ToolTipText = "Clears entire SQL worksheet.";
            buttonTool3.SharedPropsInternal.ToolTipTitle = "Clear";
            appearance7.Image = global::SqlEditor.Properties.Resources.disk;
            buttonTool4.SharedPropsInternal.AppearancesLarge.Appearance = appearance7;
            appearance8.Image = 12;
            buttonTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
            buttonTool4.SharedPropsInternal.Caption = "Save";
            buttonTool4.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            buttonTool4.SharedPropsInternal.ToolTipText = "Saves curent worksheet to a file.";
            buttonTool4.SharedPropsInternal.ToolTipTitle = "Save";
            appearance9.Image = global::SqlEditor.Properties.Resources.save_as;
            buttonTool5.SharedPropsInternal.AppearancesLarge.Appearance = appearance9;
            appearance10.Image = 11;
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            buttonTool5.SharedPropsInternal.Caption = "Save As";
            buttonTool5.SharedPropsInternal.ToolTipText = "Saves current worksheet to a different file.";
            buttonTool5.SharedPropsInternal.ToolTipTitle = "Save As";
            appearance11.Image = 9;
            buttonTool6.SharedPropsInternal.AppearancesLarge.Appearance = appearance11;
            appearance12.Image = 6;
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            buttonTool6.SharedPropsInternal.Caption = "Cut";
            buttonTool6.SharedPropsInternal.ToolTipText = "Cuts selected text from a SQL worksheet.";
            buttonTool6.SharedPropsInternal.ToolTipTitle = "Cut";
            appearance13.Image = 0;
            buttonTool7.SharedPropsInternal.AppearancesLarge.Appearance = appearance13;
            appearance14.Image = 0;
            buttonTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance14;
            buttonTool7.SharedPropsInternal.Caption = "Copy";
            buttonTool7.SharedPropsInternal.ToolTipText = "Copies selected text in the SQL worksheet.";
            buttonTool7.SharedPropsInternal.ToolTipTitle = "Copy";
            appearance15.Image = 1;
            buttonTool8.SharedPropsInternal.AppearancesLarge.Appearance = appearance15;
            appearance16.Image = 1;
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance16;
            buttonTool8.SharedPropsInternal.Caption = "Paste";
            buttonTool8.SharedPropsInternal.ToolTipText = "Pastes clipboard text into the SQL worksheet.";
            buttonTool8.SharedPropsInternal.ToolTipTitle = "Paste";
            textBoxTool1.SharedPropsInternal.Caption = "Max Results";
            textBoxTool1.SharedPropsInternal.ToolTipText = "Specifies the maximum number of results to return when running SQL queries.";
            textBoxTool1.SharedPropsInternal.ToolTipTitle = "Max Results";
            textBoxTool1.SharedPropsInternal.Width = 130;
            appearance17.Image = global::SqlEditor.Properties.Resources.folder;
            buttonTool9.SharedPropsInternal.AppearancesLarge.Appearance = appearance17;
            appearance18.Image = 13;
            buttonTool9.SharedPropsInternal.AppearancesSmall.Appearance = appearance18;
            appearance19.Image = "folder.png";
            buttonTool9.SharedPropsInternal.AppearancesSmall.AppearanceOnMenu = appearance19;
            appearance20.Image = "folder.png";
            buttonTool9.SharedPropsInternal.AppearancesSmall.AppearanceOnRibbonGroup = appearance20;
            buttonTool9.SharedPropsInternal.Caption = "Open";
            buttonTool9.SharedPropsInternal.ToolTipText = "Open a file into the SQL worksheet.";
            buttonTool9.SharedPropsInternal.ToolTipTitle = "Open";
            appearance21.Image = 8;
            buttonTool10.SharedPropsInternal.AppearancesLarge.Appearance = appearance21;
            appearance22.Image = 7;
            buttonTool10.SharedPropsInternal.AppearancesSmall.Appearance = appearance22;
            buttonTool10.SharedPropsInternal.Caption = "Upper Case";
            buttonTool10.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlU;
            buttonTool10.SharedPropsInternal.ToolTipText = "Converts selected text into upper case.";
            buttonTool10.SharedPropsInternal.ToolTipTitle = "Upper Case";
            appearance23.Image = 6;
            buttonTool11.SharedPropsInternal.AppearancesLarge.Appearance = appearance23;
            appearance24.Image = 8;
            buttonTool11.SharedPropsInternal.AppearancesSmall.Appearance = appearance24;
            buttonTool11.SharedPropsInternal.Caption = "Lower Case";
            buttonTool11.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            buttonTool11.SharedPropsInternal.ToolTipText = "Converts selected text into lower case.";
            buttonTool11.SharedPropsInternal.ToolTipTitle = "Lower Case";
            appearance25.Image = 7;
            buttonTool12.SharedPropsInternal.AppearancesLarge.Appearance = appearance25;
            appearance26.Image = 9;
            buttonTool12.SharedPropsInternal.AppearancesSmall.Appearance = appearance26;
            buttonTool12.SharedPropsInternal.Caption = "Camel Case";
            buttonTool12.SharedPropsInternal.ToolTipText = "Converts selected text into camel case.";
            buttonTool12.SharedPropsInternal.ToolTipTitle = "Camel Case";
            appearance27.Image = 10;
            buttonTool13.SharedPropsInternal.AppearancesLarge.Appearance = appearance27;
            appearance28.Image = 10;
            buttonTool13.SharedPropsInternal.AppearancesSmall.Appearance = appearance28;
            buttonTool13.SharedPropsInternal.Caption = "Toggle Comment";
            buttonTool13.SharedPropsInternal.ToolTipText = "Toggles comments on and off.";
            buttonTool13.SharedPropsInternal.ToolTipTitle = "Toggle Comment";
            appearance29.Image = 2;
            buttonTool28.SharedPropsInternal.AppearancesLarge.Appearance = appearance29;
            appearance30.Image = 2;
            buttonTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance30;
            buttonTool28.SharedPropsInternal.Caption = "Select All";
            buttonTool28.SharedPropsInternal.ToolTipText = "Selects all text in a SQL workesheet.";
            buttonTool28.SharedPropsInternal.ToolTipTitle = "Select All";
            appearance31.Image = 14;
            buttonTool14.SharedPropsInternal.AppearancesLarge.Appearance = appearance31;
            appearance32.Image = 14;
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance32;
            buttonTool14.SharedPropsInternal.Caption = "Undo";
            buttonTool14.SharedPropsInternal.ToolTipText = "Undo typing.";
            buttonTool14.SharedPropsInternal.ToolTipTitle = "Undo";
            appearance33.Image = 15;
            buttonTool15.SharedPropsInternal.AppearancesLarge.Appearance = appearance33;
            appearance34.Image = 15;
            buttonTool15.SharedPropsInternal.AppearancesSmall.Appearance = appearance34;
            buttonTool15.SharedPropsInternal.Caption = "Redo";
            buttonTool15.SharedPropsInternal.ToolTipText = "Redo typing.";
            buttonTool15.SharedPropsInternal.ToolTipTitle = "Redo";
            popupMenuTool1.SharedPropsInternal.Caption = "Sql Editor Menu";
            buttonTool41.InstanceProps.IsFirstInGroup = true;
            buttonTool44.InstanceProps.IsFirstInGroup = true;
            buttonTool42.InstanceProps.IsFirstInGroup = true;
            popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool43,
            buttonTool41,
            buttonTool31,
            buttonTool34,
            buttonTool44,
            buttonTool39,
            buttonTool40,
            buttonTool42});
            appearance35.Image = 17;
            buttonTool48.SharedPropsInternal.AppearancesLarge.Appearance = appearance35;
            appearance36.Image = 16;
            buttonTool48.SharedPropsInternal.AppearancesSmall.Appearance = appearance36;
            buttonTool48.SharedPropsInternal.Caption = "Find";
            buttonTool48.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            buttonTool48.SharedPropsInternal.ToolTipText = "Find text in a SQL worksheet.";
            buttonTool48.SharedPropsInternal.ToolTipTitle = "Find";
            appearance37.Image = 16;
            buttonTool49.SharedPropsInternal.AppearancesLarge.Appearance = appearance37;
            appearance38.Image = 17;
            buttonTool49.SharedPropsInternal.AppearancesSmall.Appearance = appearance38;
            buttonTool49.SharedPropsInternal.Caption = "Replace";
            buttonTool49.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            buttonTool49.SharedPropsInternal.ToolTipText = "Replace text in a SQL worksheet.";
            buttonTool49.SharedPropsInternal.ToolTipTitle = "Replace";
            appearance39.Image = 18;
            stateButtonTool1.SharedPropsInternal.AppearancesLarge.Appearance = appearance39;
            appearance40.Image = 21;
            stateButtonTool1.SharedPropsInternal.AppearancesSmall.Appearance = appearance40;
            stateButtonTool1.SharedPropsInternal.Caption = "Auto Commit";
            stateButtonTool1.SharedPropsInternal.ToolTipText = "Toggle auto commit on and off. When auto comit is on, DML operations are autmatic" +
    "ally commited when complete. When auto commit if off, DML operations have to be " +
    "explicitly committed.";
            stateButtonTool1.SharedPropsInternal.ToolTipTitle = "Auto Commit";
            appearance41.Image = 19;
            buttonTool52.SharedPropsInternal.AppearancesLarge.Appearance = appearance41;
            appearance42.Image = 22;
            buttonTool52.SharedPropsInternal.AppearancesSmall.Appearance = appearance42;
            buttonTool52.SharedPropsInternal.Caption = "Run from Files";
            buttonTool52.SharedPropsInternal.ToolTipText = "Show run from files dialog.";
            buttonTool52.SharedPropsInternal.ToolTipTitle = "Run from Files";
            appearance43.Image = 20;
            buttonTool32.SharedPropsInternal.AppearancesLarge.Appearance = appearance43;
            appearance44.Image = 23;
            buttonTool32.SharedPropsInternal.AppearancesSmall.Appearance = appearance44;
            buttonTool32.SharedPropsInternal.Caption = "Show Explain Plan";
            this._utm.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            textBoxTool1,
            buttonTool9,
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13,
            buttonTool28,
            buttonTool14,
            buttonTool15,
            popupMenuTool1,
            buttonTool48,
            buttonTool49,
            stateButtonTool1,
            buttonTool52,
            buttonTool32});
            this._utm.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.Utm_ToolClick);
            // 
            // _img32
            // 
            this._img32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_img32.ImageStream")));
            this._img32.TransparentColor = System.Drawing.Color.Transparent;
            this._img32.Images.SetKeyName(0, "page_copy.png");
            this._img32.Images.SetKeyName(1, "page_paste.png");
            this._img32.Images.SetKeyName(2, "select_restangular.png");
            this._img32.Images.SetKeyName(3, "control_play_blue.png");
            this._img32.Images.SetKeyName(4, "script_play.png");
            this._img32.Images.SetKeyName(5, "draw_eraser.png");
            this._img32.Images.SetKeyName(6, "text_lowercase.png");
            this._img32.Images.SetKeyName(7, "text_replace.png");
            this._img32.Images.SetKeyName(8, "text_uppercase.png");
            this._img32.Images.SetKeyName(9, "cut.png");
            this._img32.Images.SetKeyName(10, "document_comment_below.png");
            this._img32.Images.SetKeyName(11, "disk.png");
            this._img32.Images.SetKeyName(12, "save_as.png");
            this._img32.Images.SetKeyName(13, "folder.png");
            this._img32.Images.SetKeyName(14, "Undo_32x32.png");
            this._img32.Images.SetKeyName(15, "Redo_32x32.png");
            this._img32.Images.SetKeyName(16, "text_replace.png");
            this._img32.Images.SetKeyName(17, "find.png");
            this._img32.Images.SetKeyName(18, "database_check.png");
            this._img32.Images.SetKeyName(19, "folder_vertical_document_play.png");
            this._img32.Images.SetKeyName(20, "node-tree.png");
            // 
            // _img16
            // 
            this._img16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_img16.ImageStream")));
            this._img16.TransparentColor = System.Drawing.Color.Transparent;
            this._img16.Images.SetKeyName(0, "page_copy.png");
            this._img16.Images.SetKeyName(1, "page_paste.png");
            this._img16.Images.SetKeyName(2, "select_restangular.png");
            this._img16.Images.SetKeyName(3, "control_play_blue.png");
            this._img16.Images.SetKeyName(4, "script_play.png");
            this._img16.Images.SetKeyName(5, "draw_eraser.png");
            this._img16.Images.SetKeyName(6, "cut.png");
            this._img16.Images.SetKeyName(7, "text_uppercase.png");
            this._img16.Images.SetKeyName(8, "text_lowercase.png");
            this._img16.Images.SetKeyName(9, "text_replace.png");
            this._img16.Images.SetKeyName(10, "document_comment_below.png");
            this._img16.Images.SetKeyName(11, "save_as.png");
            this._img16.Images.SetKeyName(12, "disk.png");
            this._img16.Images.SetKeyName(13, "folder.png");
            this._img16.Images.SetKeyName(14, "arrow_undo_blue.png");
            this._img16.Images.SetKeyName(15, "arrow_redo_blue.png");
            this._img16.Images.SetKeyName(16, "find.png");
            this._img16.Images.SetKeyName(17, "text_replace.png");
            this._img16.Images.SetKeyName(18, "table (2).png");
            this._img16.Images.SetKeyName(19, "3d_glasses.png");
            this._img16.Images.SetKeyName(20, "column_single.png");
            this._img16.Images.SetKeyName(21, "database_check.png");
            this._img16.Images.SetKeyName(22, "folder_vertical_document_play.png");
            this._img16.Images.SetKeyName(23, "node-tree.png");
            // 
            // _Worksheet2_Toolbars_Dock_Area_Right
            // 
            this._Worksheet2_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Worksheet2_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Worksheet2_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._Worksheet2_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Worksheet2_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._Worksheet2_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(1195, 146);
            this._Worksheet2_Toolbars_Dock_Area_Right.Name = "_Worksheet2_Toolbars_Dock_Area_Right";
            this._Worksheet2_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 499);
            this._Worksheet2_Toolbars_Dock_Area_Right.ToolbarsManager = this._utm;
            // 
            // _Worksheet2_Toolbars_Dock_Area_Top
            // 
            this._Worksheet2_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Worksheet2_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Worksheet2_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._Worksheet2_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Worksheet2_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._Worksheet2_Toolbars_Dock_Area_Top.Name = "_Worksheet2_Toolbars_Dock_Area_Top";
            this._Worksheet2_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(1199, 146);
            this._Worksheet2_Toolbars_Dock_Area_Top.ToolbarsManager = this._utm;
            // 
            // _Worksheet2_Toolbars_Dock_Area_Bottom
            // 
            this._Worksheet2_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._Worksheet2_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._Worksheet2_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._Worksheet2_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._Worksheet2_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._Worksheet2_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 645);
            this._Worksheet2_Toolbars_Dock_Area_Bottom.Name = "_Worksheet2_Toolbars_Dock_Area_Bottom";
            this._Worksheet2_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(1199, 4);
            this._Worksheet2_Toolbars_Dock_Area_Bottom.ToolbarsManager = this._utm;
            // 
            // ucQueryResults1
            // 
            this.ucQueryResults1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucQueryResults1.HasMoreRows = false;
            this.ucQueryResults1.IsBusy = false;
            this.ucQueryResults1.Location = new System.Drawing.Point(0, 0);
            this.ucQueryResults1.MaxResults = 100;
            this.ucQueryResults1.Name = "ucQueryResults1";
            this.ucQueryResults1.Size = new System.Drawing.Size(891, 248);
            this.ucQueryResults1.Sql = "";
            this.ucQueryResults1.TabIndex = 0;
            // 
            // FrmSqlWorksheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1199, 649);
            this.Controls.Add(this.Worksheet2_Fill_Panel);
            this.Controls.Add(this._Worksheet2_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._Worksheet2_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._Worksheet2_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._Worksheet2_Toolbars_Dock_Area_Top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSqlWorksheet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSqlWorksheet_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSqlWorksheet_FormClosed);
            this.Worksheet2_Fill_Panel.ClientArea.ResumeLayout(false);
            this.Worksheet2_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).EndInit();
            this._utcTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager _utm;
        private Infragistics.Win.Misc.UltraPanel Worksheet2_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Worksheet2_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Worksheet2_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Worksheet2_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _Worksheet2_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl _utcTabs;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private ImageList _img32;
        private ImageList _img16;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private ICSharpCode.TextEditor.TextEditorControl _sqlEditor;
        private QueryResults.UcQueryResults ucQueryResults1;
    }
}