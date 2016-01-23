using Infragistics.Win.UltraWinGrid;

namespace SqlEditor
{
    sealed partial class FrmTableDetails
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Export to Excel");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("GridMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopyText");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Export to Excel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("CopyText");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTableDetails));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._ugColumns = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._ugData = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._ugIndexes = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraTabPageControl4 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._ugPartitions = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.FrmTableDetails_Fill_Panel = new System.Windows.Forms.Panel();
            this._utcTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._FrmTableDetails_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._utm = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._img32 = new System.Windows.Forms.ImageList(this.components);
            this._img16 = new System.Windows.Forms.ImageList(this.components);
            this._FrmTableDetails_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmTableDetails_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ugColumns)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ugData)).BeginInit();
            this.ultraTabPageControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ugIndexes)).BeginInit();
            this.ultraTabPageControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ugPartitions)).BeginInit();
            this.FrmTableDetails_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).BeginInit();
            this._utcTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this._ugColumns);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(602, 472);
            // 
            // _ugColumns
            // 
            this._utm.SetContextMenuUltra(this._ugColumns, "GridMenu");
            this._ugColumns.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this._ugColumns.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._ugColumns.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this._ugColumns.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._ugColumns.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this._ugColumns.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._ugColumns.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains;
            this._ugColumns.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            this._ugColumns.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._ugColumns.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this._ugColumns.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._ugColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ugColumns.Location = new System.Drawing.Point(0, 0);
            this._ugColumns.Name = "_ugColumns";
            this._ugColumns.Size = new System.Drawing.Size(602, 472);
            this._ugColumns.TabIndex = 0;
            this._ugColumns.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.Grid_InitializeLayout);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this._ugData);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(602, 472);
            // 
            // _ugData
            // 
            this._utm.SetContextMenuUltra(this._ugData, "GridMenu");
            this._ugData.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this._ugData.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._ugData.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this._ugData.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._ugData.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this._ugData.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._ugData.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains;
            this._ugData.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            this._ugData.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._ugData.DisplayLayout.Override.RowSelectorHeaderStyle = Infragistics.Win.UltraWinGrid.RowSelectorHeaderStyle.ColumnChooserButton;
            this._ugData.DisplayLayout.Override.RowSelectorNumberStyle = Infragistics.Win.UltraWinGrid.RowSelectorNumberStyle.RowIndex;
            this._ugData.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.True;
            this._ugData.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._ugData.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ugData.Location = new System.Drawing.Point(0, 0);
            this._ugData.Name = "_ugData";
            this._ugData.Size = new System.Drawing.Size(602, 472);
            this._ugData.TabIndex = 1;
            this._ugData.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.Grid_InitializeLayout);
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Controls.Add(this._ugIndexes);
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(602, 472);
            // 
            // _ugIndexes
            // 
            this._utm.SetContextMenuUltra(this._ugIndexes, "GridMenu");
            this._ugIndexes.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this._ugIndexes.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._ugIndexes.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this._ugIndexes.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._ugIndexes.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this._ugIndexes.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._ugIndexes.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains;
            this._ugIndexes.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            this._ugIndexes.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._ugIndexes.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this._ugIndexes.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._ugIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ugIndexes.Location = new System.Drawing.Point(0, 0);
            this._ugIndexes.Name = "_ugIndexes";
            this._ugIndexes.Size = new System.Drawing.Size(602, 472);
            this._ugIndexes.TabIndex = 1;
            this._ugIndexes.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.Grid_InitializeLayout);
            // 
            // ultraTabPageControl4
            // 
            this.ultraTabPageControl4.Controls.Add(this._ugPartitions);
            this.ultraTabPageControl4.Location = new System.Drawing.Point(1, 22);
            this.ultraTabPageControl4.Name = "ultraTabPageControl4";
            this.ultraTabPageControl4.Size = new System.Drawing.Size(602, 472);
            // 
            // _ugPartitions
            // 
            this._utm.SetContextMenuUltra(this._ugPartitions, "GridMenu");
            this._ugPartitions.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns;
            this._ugPartitions.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No;
            this._ugPartitions.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup;
            this._ugPartitions.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            this._ugPartitions.DisplayLayout.Override.AllowRowFiltering = Infragistics.Win.DefaultableBoolean.True;
            this._ugPartitions.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            this._ugPartitions.DisplayLayout.Override.FilterOperatorDefaultValue = Infragistics.Win.UltraWinGrid.FilterOperatorDefaultValue.Contains;
            this._ugPartitions.DisplayLayout.Override.FilterUIType = Infragistics.Win.UltraWinGrid.FilterUIType.FilterRow;
            this._ugPartitions.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this._ugPartitions.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            this._ugPartitions.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this._ugPartitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ugPartitions.Location = new System.Drawing.Point(0, 0);
            this._ugPartitions.Name = "_ugPartitions";
            this._ugPartitions.Size = new System.Drawing.Size(602, 472);
            this._ugPartitions.TabIndex = 2;
            this._ugPartitions.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.Grid_InitializeLayout);
            // 
            // FrmTableDetails_Fill_Panel
            // 
            this.FrmTableDetails_Fill_Panel.Controls.Add(this._utcTabs);
            this.FrmTableDetails_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FrmTableDetails_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrmTableDetails_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.FrmTableDetails_Fill_Panel.Name = "FrmTableDetails_Fill_Panel";
            this.FrmTableDetails_Fill_Panel.Size = new System.Drawing.Size(604, 495);
            this.FrmTableDetails_Fill_Panel.TabIndex = 0;
            // 
            // _utcTabs
            // 
            this._utcTabs.CloseButtonLocation = Infragistics.Win.UltraWinTabs.TabCloseButtonLocation.None;
            this._utcTabs.Controls.Add(this.ultraTabSharedControlsPage1);
            this._utcTabs.Controls.Add(this.ultraTabPageControl1);
            this._utcTabs.Controls.Add(this.ultraTabPageControl2);
            this._utcTabs.Controls.Add(this.ultraTabPageControl3);
            this._utcTabs.Controls.Add(this.ultraTabPageControl4);
            this._utcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._utcTabs.Location = new System.Drawing.Point(0, 0);
            this._utcTabs.Name = "_utcTabs";
            this._utcTabs.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this._utcTabs.Size = new System.Drawing.Size(604, 495);
            this._utcTabs.TabIndex = 0;
            ultraTab1.AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            ultraTab1.Key = "Columns";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Columns";
            ultraTab2.AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            ultraTab2.Key = "Data";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Data";
            ultraTab3.AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            ultraTab3.Key = "Indexes";
            ultraTab3.TabPage = this.ultraTabPageControl3;
            ultraTab3.Text = "Indexes";
            ultraTab4.Key = "Partitions";
            ultraTab4.TabPage = this.ultraTabPageControl4;
            ultraTab4.Text = "Partitions";
            ultraTab4.ToolTipText = "Partitions";
            this._utcTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2,
            ultraTab3,
            ultraTab4});
            this._utcTabs.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            this._utcTabs.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.UtcTabsSelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(602, 472);
            // 
            // _FrmTableDetails_Toolbars_Dock_Area_Left
            // 
            this._FrmTableDetails_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmTableDetails_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._FrmTableDetails_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._FrmTableDetails_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmTableDetails_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._FrmTableDetails_Toolbars_Dock_Area_Left.Name = "_FrmTableDetails_Toolbars_Dock_Area_Left";
            this._FrmTableDetails_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 495);
            this._FrmTableDetails_Toolbars_Dock_Area_Left.ToolbarsManager = this._utm;
            // 
            // _utm
            // 
            this._utm.DesignerFlags = 1;
            this._utm.DockWithinContainer = this;
            this._utm.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this._utm.ImageListLarge = this._img32;
            this._utm.ImageListSmall = this._img16;
            this._utm.ShowFullMenusDelay = 500;
            appearance1.Image = 2;
            buttonTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance1;
            appearance2.Image = 1;
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool2.SharedPropsInternal.Caption = "Export to Excel";
            popupMenuTool1.SharedPropsInternal.Caption = "GridMenu";
            buttonTool14.InstanceProps.IsFirstInGroup = true;
            popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool14});
            appearance3.Image = 3;
            buttonTool1.SharedPropsInternal.AppearancesLarge.Appearance = appearance3;
            appearance4.Image = 3;
            buttonTool1.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool1.SharedPropsInternal.Caption = "Copy";
            this._utm.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            popupMenuTool1,
            buttonTool1});
            this._utm.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.Utm_ToolClick);
            // 
            // _img32
            // 
            this._img32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_img32.ImageStream")));
            this._img32.TransparentColor = System.Drawing.Color.Transparent;
            this._img32.Images.SetKeyName(0, "database_delete.png");
            this._img32.Images.SetKeyName(1, "database_save.png");
            this._img32.Images.SetKeyName(2, "database_go.png");
            this._img32.Images.SetKeyName(3, "page_copy.png");
            this._img32.Images.SetKeyName(4, "page_paste.png");
            this._img32.Images.SetKeyName(5, "select_by_adding_to_selection.png");
            this._img32.Images.SetKeyName(6, "control_play_blue.png");
            this._img32.Images.SetKeyName(7, "control_stop_blue.png");
            this._img32.Images.SetKeyName(8, "script_play.png");
            this._img32.Images.SetKeyName(9, "draw_eraser.png");
            this._img32.Images.SetKeyName(10, "text_lowercase.png");
            this._img32.Images.SetKeyName(11, "text_replace.png");
            this._img32.Images.SetKeyName(12, "text_uppercase.png");
            this._img32.Images.SetKeyName(13, "cut.png");
            this._img32.Images.SetKeyName(14, "arrow_refresh.png");
            // 
            // _img16
            // 
            this._img16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_img16.ImageStream")));
            this._img16.TransparentColor = System.Drawing.Color.Transparent;
            this._img16.Images.SetKeyName(0, "page_copy.png");
            this._img16.Images.SetKeyName(1, "export_excel.png");
            // 
            // _FrmTableDetails_Toolbars_Dock_Area_Right
            // 
            this._FrmTableDetails_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmTableDetails_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._FrmTableDetails_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._FrmTableDetails_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmTableDetails_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(604, 0);
            this._FrmTableDetails_Toolbars_Dock_Area_Right.Name = "_FrmTableDetails_Toolbars_Dock_Area_Right";
            this._FrmTableDetails_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 495);
            this._FrmTableDetails_Toolbars_Dock_Area_Right.ToolbarsManager = this._utm;
            // 
            // _FrmTableDetails_Toolbars_Dock_Area_Top
            // 
            this._FrmTableDetails_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmTableDetails_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._FrmTableDetails_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._FrmTableDetails_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmTableDetails_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._FrmTableDetails_Toolbars_Dock_Area_Top.Name = "_FrmTableDetails_Toolbars_Dock_Area_Top";
            this._FrmTableDetails_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(604, 0);
            this._FrmTableDetails_Toolbars_Dock_Area_Top.ToolbarsManager = this._utm;
            // 
            // _FrmTableDetails_Toolbars_Dock_Area_Bottom
            // 
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 495);
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.Name = "_FrmTableDetails_Toolbars_Dock_Area_Bottom";
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(604, 0);
            this._FrmTableDetails_Toolbars_Dock_Area_Bottom.ToolbarsManager = this._utm;
            // 
            // FrmTableDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 495);
            this.Controls.Add(this.FrmTableDetails_Fill_Panel);
            this.Controls.Add(this._FrmTableDetails_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._FrmTableDetails_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._FrmTableDetails_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._FrmTableDetails_Toolbars_Dock_Area_Top);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmTableDetails";
            this.Text = "TableDetails";
            this.Load += new System.EventHandler(this.FrmTableDetailsLoad);
            this.ultraTabPageControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ugColumns)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ugData)).EndInit();
            this.ultraTabPageControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ugIndexes)).EndInit();
            this.ultraTabPageControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ugPartitions)).EndInit();
            this.FrmTableDetails_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).EndInit();
            this._utcTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager _utm;
        private System.Windows.Forms.Panel FrmTableDetails_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmTableDetails_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmTableDetails_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmTableDetails_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmTableDetails_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl _utcTabs;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private System.Windows.Forms.ImageList _img32;
        private System.Windows.Forms.ImageList _img16;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl4;
        private UltraGrid _ugColumns;
        private UltraGrid _ugData;
        private UltraGrid _ugIndexes;
        private UltraGrid _ugPartitions;
    }
}