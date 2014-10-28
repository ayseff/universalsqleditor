using System.Windows.Forms;
using SqlEditor.QueryResults;

namespace SqlEditor.ExplainPlan
{
    partial class UcExplainPlan
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.UltraWinTree.UltraTreeColumnSet ultraTreeColumnSet1 = new Infragistics.Win.UltraWinTree.UltraTreeColumnSet();
            Infragistics.Win.UltraWinTree.Override _override1 = new Infragistics.Win.UltraWinTree.Override();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("QueryToolbar");
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool2 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Flag", "");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Show SQL");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Refresh");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool2 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("Activity Indicator");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Stop");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool2 = new Infragistics.Win.UltraWinToolbars.LabelTool("Timing");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Show SQL");
            Infragistics.Win.UltraWinToolbars.ControlContainerTool controlContainerTool1 = new Infragistics.Win.UltraWinToolbars.ControlContainerTool("Activity Indicator");
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Timing");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Stop");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.StateButtonTool stateButtonTool1 = new Infragistics.Win.UltraWinToolbars.StateButtonTool("Flag", "");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("GridPopupMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Refresh");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Refresh");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcExplainPlan));
            this.QueryResultsControl2_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this._utExplain = new Infragistics.Win.UltraWinTree.UltraTree();
            this._uaiActivity = new Infragistics.Win.UltraActivityIndicator.UltraActivityIndicator();
            this._QueryResultsControl2_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._utm = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._iml32 = new System.Windows.Forms.ImageList(this.components);
            this._iml16 = new System.Windows.Forms.ImageList(this.components);
            this._QueryResultsControl2_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._QueryResultsControl2_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._tmQueryTimer = new System.Windows.Forms.Timer(this.components);
            this.QueryResultsControl2_Fill_Panel.ClientArea.SuspendLayout();
            this.QueryResultsControl2_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utExplain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).BeginInit();
            this.SuspendLayout();
            // 
            // QueryResultsControl2_Fill_Panel
            // 
            // 
            // QueryResultsControl2_Fill_Panel.ClientArea
            // 
            this.QueryResultsControl2_Fill_Panel.ClientArea.Controls.Add(this._utExplain);
            this.QueryResultsControl2_Fill_Panel.ClientArea.Controls.Add(this._uaiActivity);
            this.QueryResultsControl2_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.QueryResultsControl2_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QueryResultsControl2_Fill_Panel.Location = new System.Drawing.Point(0, 27);
            this.QueryResultsControl2_Fill_Panel.Name = "QueryResultsControl2_Fill_Panel";
            this.QueryResultsControl2_Fill_Panel.Size = new System.Drawing.Size(786, 379);
            this.QueryResultsControl2_Fill_Panel.TabIndex = 0;
            // 
            // _utExplain
            // 
            ultraTreeColumnSet1.Key = "Set1";
            this._utExplain.ColumnSettings.RootColumnSet = ultraTreeColumnSet1;
            this._utExplain.ColumnSettings.ShowBandNodes = Infragistics.Win.UltraWinTree.ShowBandNodes.Always;
            this._utm.SetContextMenuUltra(this._utExplain, "GridPopupMenu");
            this._utExplain.DisplayStyle = Infragistics.Win.UltraWinTree.UltraTreeDisplayStyle.WindowsVista;
            this._utExplain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._utExplain.Location = new System.Drawing.Point(0, 0);
            this._utExplain.Name = "_utExplain";
            _override1.SelectionType = Infragistics.Win.UltraWinTree.SelectType.Extended;
            this._utExplain.Override = _override1;
            this._utExplain.Size = new System.Drawing.Size(786, 379);
            this._utExplain.TabIndex = 1;
            this._utExplain.ViewStyle = Infragistics.Win.UltraWinTree.ViewStyle.OutlookExpress;
            this._utExplain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UtExplain_KeyDown);
            this._utExplain.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UtExplain_KeyUp);
            // 
            // _uaiActivity
            // 
            this._uaiActivity.AnimationSpeed = 40;
            this._uaiActivity.CausesValidation = true;
            this._uaiActivity.Location = new System.Drawing.Point(336, 205);
            this._uaiActivity.Name = "_uaiActivity";
            this._uaiActivity.Size = new System.Drawing.Size(188, 19);
            this._uaiActivity.TabIndex = 0;
            this._uaiActivity.TabStop = true;
            this._uaiActivity.ViewStyle = Infragistics.Win.UltraActivityIndicator.ActivityIndicatorViewStyle.Aero;
            // 
            // _QueryResultsControl2_Toolbars_Dock_Area_Left
            // 
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.Name = "_QueryResultsControl2_Toolbars_Dock_Area_Left";
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 379);
            this._QueryResultsControl2_Toolbars_Dock_Area_Left.ToolbarsManager = this._utm;
            // 
            // _utm
            // 
            this._utm.DesignerFlags = 1;
            this._utm.DockWithinContainer = this;
            this._utm.ImageListLarge = this._iml32;
            this._utm.ImageListSmall = this._iml16;
            this._utm.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            controlContainerTool2.ControlName = "_uaiActivity";
            controlContainerTool2.InstanceProps.IsFirstInGroup = true;
            labelTool2.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            stateButtonTool2,
            buttonTool7,
            buttonTool17,
            controlContainerTool2,
            buttonTool10,
            labelTool2});
            ultraToolbar1.Text = "QueryToolbar";
            this._utm.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool2.SharedPropsInternal.Caption = "SQL";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.TextOnlyAlways;
            buttonTool2.SharedPropsInternal.ToolTipText = "Show SQL that was executed.";
            buttonTool2.SharedPropsInternal.ToolTipTitle = "Show SQL";
            controlContainerTool1.ControlName = "_uaiActivity";
            controlContainerTool1.SharedPropsInternal.Caption = "Activity Indicator";
            appearance1.TextHAlignAsString = "Left";
            appearance1.TextVAlignAsString = "Middle";
            labelTool1.SharedPropsInternal.AppearancesSmall.Appearance = appearance1;
            labelTool1.SharedPropsInternal.Caption = "Timing";
            appearance2.Image = 12;
            buttonTool5.SharedPropsInternal.AppearancesLarge.Appearance = appearance2;
            appearance3.Image = 6;
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool5.SharedPropsInternal.Caption = "Stop";
            buttonTool5.SharedPropsInternal.ToolTipText = "Stop currently running query.";
            buttonTool5.SharedPropsInternal.ToolTipTitle = "Stop";
            appearance4.Image = 16;
            stateButtonTool1.SharedPropsInternal.AppearancesLarge.Appearance = appearance4;
            appearance5.Image = 16;
            stateButtonTool1.SharedPropsInternal.AppearancesSmall.Appearance = appearance5;
            appearance6.Image = "MapMarker_PushPin2__Pink.png";
            stateButtonTool1.SharedPropsInternal.AppearancesSmall.PressedAppearance = appearance6;
            stateButtonTool1.SharedPropsInternal.Caption = "Pin";
            stateButtonTool1.SharedPropsInternal.ToolTipText = "Pin the results so the widnow is not reused.";
            stateButtonTool1.SharedPropsInternal.ToolTipTitle = "Pin";
            popupMenuTool1.SharedPropsInternal.Caption = "GridPopupMenu";
            buttonTool22.InstanceProps.IsFirstInGroup = true;
            popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool12,
            buttonTool22});
            appearance7.Image = 8;
            buttonTool11.SharedPropsInternal.AppearancesLarge.Appearance = appearance7;
            appearance8.Image = 3;
            buttonTool11.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
            buttonTool11.SharedPropsInternal.Caption = "Copy";
            buttonTool11.SharedPropsInternal.ToolTipText = "Copy selected cells to clipboard";
            buttonTool11.SharedPropsInternal.ToolTipTitle = "Copy";
            appearance9.Image = 10;
            buttonTool16.SharedPropsInternal.AppearancesLarge.Appearance = appearance9;
            appearance10.Image = 5;
            buttonTool16.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            buttonTool16.SharedPropsInternal.Caption = "Refresh";
            buttonTool16.SharedPropsInternal.ToolTipText = "Refresh the data by re-running the query.";
            buttonTool16.SharedPropsInternal.ToolTipTitle = "Refresh Results";
            this._utm.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            controlContainerTool1,
            labelTool1,
            buttonTool5,
            stateButtonTool1,
            popupMenuTool1,
            buttonTool11,
            buttonTool16});
            this._utm.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.UtmToolClick);
            // 
            // _iml32
            // 
            this._iml32.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_iml32.ImageStream")));
            this._iml32.TransparentColor = System.Drawing.Color.Transparent;
            this._iml32.Images.SetKeyName(0, "flag_red.png");
            this._iml32.Images.SetKeyName(1, "document_inspector.png");
            this._iml32.Images.SetKeyName(2, "filter.png");
            this._iml32.Images.SetKeyName(3, "control_stop_blue.png");
            this._iml32.Images.SetKeyName(4, "database_go.png");
            this._iml32.Images.SetKeyName(5, "draw_eraser.png");
            this._iml32.Images.SetKeyName(6, "database_delete.png");
            this._iml32.Images.SetKeyName(7, "database_save.png");
            this._iml32.Images.SetKeyName(8, "page_copy.png");
            this._iml32.Images.SetKeyName(9, "download.png");
            this._iml32.Images.SetKeyName(10, "arrow_refresh.png");
            this._iml32.Images.SetKeyName(11, "database_undo.png");
            this._iml32.Images.SetKeyName(12, "control_stop_red.png");
            this._iml32.Images.SetKeyName(13, "download_red.png");
            this._iml32.Images.SetKeyName(14, "export_excel.png");
            this._iml32.Images.SetKeyName(15, "MapMarker_PushPin2__Pink.png");
            this._iml32.Images.SetKeyName(16, "MapMarker_PushPin2_Left_Chartreuse.png");
            this._iml32.Images.SetKeyName(17, "data_chooser.png");
            // 
            // _iml16
            // 
            this._iml16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_iml16.ImageStream")));
            this._iml16.TransparentColor = System.Drawing.Color.Transparent;
            this._iml16.Images.SetKeyName(0, "flag_red.png");
            this._iml16.Images.SetKeyName(1, "filter.png");
            this._iml16.Images.SetKeyName(2, "control_stop_blue.png");
            this._iml16.Images.SetKeyName(3, "page_copy.png");
            this._iml16.Images.SetKeyName(4, "download.png");
            this._iml16.Images.SetKeyName(5, "arrow_refresh.png");
            this._iml16.Images.SetKeyName(6, "control_stop_red.png");
            this._iml16.Images.SetKeyName(7, "download_red.png");
            this._iml16.Images.SetKeyName(8, "page_copy_database.png");
            this._iml16.Images.SetKeyName(9, "page_copy_header.png");
            this._iml16.Images.SetKeyName(10, "file_extension_rtf.png");
            this._iml16.Images.SetKeyName(11, "file_extension_txt.png");
            this._iml16.Images.SetKeyName(12, "file_extension_xls.png");
            this._iml16.Images.SetKeyName(13, "database_check.png");
            this._iml16.Images.SetKeyName(14, "database_left.png");
            this._iml16.Images.SetKeyName(15, "MapMarker_PushPin2__Pink.png");
            this._iml16.Images.SetKeyName(16, "MapMarker_PushPin2_Left_Chartreuse.png");
            this._iml16.Images.SetKeyName(17, "data_chooser.png");
            // 
            // _QueryResultsControl2_Toolbars_Dock_Area_Right
            // 
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(786, 27);
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.Name = "_QueryResultsControl2_Toolbars_Dock_Area_Right";
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 379);
            this._QueryResultsControl2_Toolbars_Dock_Area_Right.ToolbarsManager = this._utm;
            // 
            // _QueryResultsControl2_Toolbars_Dock_Area_Top
            // 
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.Name = "_QueryResultsControl2_Toolbars_Dock_Area_Top";
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(786, 27);
            this._QueryResultsControl2_Toolbars_Dock_Area_Top.ToolbarsManager = this._utm;
            // 
            // _QueryResultsControl2_Toolbars_Dock_Area_Bottom
            // 
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 406);
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.Name = "_QueryResultsControl2_Toolbars_Dock_Area_Bottom";
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(786, 0);
            this._QueryResultsControl2_Toolbars_Dock_Area_Bottom.ToolbarsManager = this._utm;
            // 
            // _tmQueryTimer
            // 
            this._tmQueryTimer.Enabled = true;
            this._tmQueryTimer.Tick += new System.EventHandler(this.TmQueryTimerTick);
            // 
            // UcExplainPlan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.QueryResultsControl2_Fill_Panel);
            this.Controls.Add(this._QueryResultsControl2_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._QueryResultsControl2_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._QueryResultsControl2_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._QueryResultsControl2_Toolbars_Dock_Area_Top);
            this.Name = "UcExplainPlan";
            this.Size = new System.Drawing.Size(786, 406);
            this.QueryResultsControl2_Fill_Panel.ClientArea.ResumeLayout(false);
            this.QueryResultsControl2_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utExplain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager _utm;
        private Infragistics.Win.Misc.UltraPanel QueryResultsControl2_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _QueryResultsControl2_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _QueryResultsControl2_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _QueryResultsControl2_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _QueryResultsControl2_Toolbars_Dock_Area_Bottom;
        private Infragistics.Win.UltraActivityIndicator.UltraActivityIndicator _uaiActivity;
        private System.Windows.Forms.ImageList _iml32;
        private System.Windows.Forms.ImageList _iml16;
        private Timer _tmQueryTimer;
        private Infragistics.Win.UltraWinTree.UltraTree _utExplain;
    }
}
