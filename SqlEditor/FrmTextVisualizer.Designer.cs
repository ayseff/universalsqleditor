namespace SqlEditor
{
    partial class FrmTextVisualizer
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainToolbar");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Undo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Redo");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Replace");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Save");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Find");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Replace");
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("PopupMenu");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Copy");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Undo");
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Redo");
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTextVisualizer));
            this.FrmShowSql_Fill_Panel = new System.Windows.Forms.Panel();
            this._editor = new ICSharpCode.TextEditor.TextEditorControl();
            this._FrmShowSql_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._utm = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._img32 = new System.Windows.Forms.ImageList(this.components);
            this._img16 = new System.Windows.Forms.ImageList(this.components);
            this._FrmShowSql_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmShowSql_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmShowSql_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.FrmShowSql_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).BeginInit();
            this.SuspendLayout();
            // 
            // FrmShowSql_Fill_Panel
            // 
            this.FrmShowSql_Fill_Panel.Controls.Add(this._editor);
            this.FrmShowSql_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FrmShowSql_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrmShowSql_Fill_Panel.Location = new System.Drawing.Point(4, 55);
            this.FrmShowSql_Fill_Panel.Name = "FrmShowSql_Fill_Panel";
            this.FrmShowSql_Fill_Panel.Size = new System.Drawing.Size(616, 286);
            this.FrmShowSql_Fill_Panel.TabIndex = 0;
            // 
            // _editor
            // 
            this._editor.CausesValidation = false;
            this._utm.SetContextMenuUltra(this._editor, "PopupMenu");
            this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.IsReadOnly = false;
            this._editor.Location = new System.Drawing.Point(0, 0);
            this._editor.Name = "_editor";
            this._editor.ShowLineNumbers = false;
            this._editor.Size = new System.Drawing.Size(616, 286);
            this._editor.TabIndex = 0;
            // 
            // _FrmShowSql_Toolbars_Dock_Area_Left
            // 
            this._FrmShowSql_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmShowSql_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmShowSql_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._FrmShowSql_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmShowSql_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._FrmShowSql_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 55);
            this._FrmShowSql_Toolbars_Dock_Area_Left.Name = "_FrmShowSql_Toolbars_Dock_Area_Left";
            this._FrmShowSql_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 286);
            this._FrmShowSql_Toolbars_Dock_Area_Left.ToolbarsManager = this._utm;
            // 
            // _utm
            // 
            this._utm.DesignerFlags = 1;
            this._utm.DockWithinContainer = this;
            this._utm.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this._utm.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            this._utm.ImageListLarge = this._img32;
            this._utm.ImageListSmall = this._img16;
            this._utm.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            buttonTool16.InstanceProps.IsFirstInGroup = true;
            buttonTool7.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            buttonTool6,
            buttonTool16,
            buttonTool17,
            buttonTool7,
            buttonTool8});
            ultraToolbar1.Text = "MainToolbar";
            this._utm.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance1.Image = 11;
            buttonTool1.SharedPropsInternal.AppearancesLarge.Appearance = appearance1;
            appearance2.Image = 12;
            buttonTool1.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool1.SharedPropsInternal.Caption = "Save";
            buttonTool1.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            appearance3.Image = 0;
            buttonTool2.SharedPropsInternal.AppearancesLarge.Appearance = appearance3;
            appearance4.Image = 0;
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool2.SharedPropsInternal.Caption = "Copy";
            appearance5.Image = 17;
            buttonTool3.SharedPropsInternal.AppearancesLarge.Appearance = appearance5;
            appearance6.Image = 16;
            buttonTool3.SharedPropsInternal.AppearancesSmall.Appearance = appearance6;
            buttonTool3.SharedPropsInternal.Caption = "Find";
            buttonTool3.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
            appearance7.Image = 16;
            buttonTool4.SharedPropsInternal.AppearancesLarge.Appearance = appearance7;
            appearance8.Image = 17;
            buttonTool4.SharedPropsInternal.AppearancesSmall.Appearance = appearance8;
            buttonTool4.SharedPropsInternal.Caption = "Replace";
            buttonTool4.SharedPropsInternal.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
            popupMenuTool1.SharedPropsInternal.Caption = "PopupMenu";
            popupMenuTool1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool10});
            appearance9.Image = 14;
            buttonTool14.SharedPropsInternal.AppearancesLarge.Appearance = appearance9;
            appearance10.Image = 14;
            buttonTool14.SharedPropsInternal.AppearancesSmall.Appearance = appearance10;
            buttonTool14.SharedPropsInternal.Caption = "Undo";
            appearance11.Image = 15;
            buttonTool15.SharedPropsInternal.AppearancesLarge.Appearance = appearance11;
            appearance12.Image = 15;
            buttonTool15.SharedPropsInternal.AppearancesSmall.Appearance = appearance12;
            buttonTool15.SharedPropsInternal.Caption = "Redo";
            this._utm.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4,
            popupMenuTool1,
            buttonTool14,
            buttonTool15});
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
            // 
            // _FrmShowSql_Toolbars_Dock_Area_Right
            // 
            this._FrmShowSql_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmShowSql_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmShowSql_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._FrmShowSql_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmShowSql_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._FrmShowSql_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(620, 55);
            this._FrmShowSql_Toolbars_Dock_Area_Right.Name = "_FrmShowSql_Toolbars_Dock_Area_Right";
            this._FrmShowSql_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 286);
            this._FrmShowSql_Toolbars_Dock_Area_Right.ToolbarsManager = this._utm;
            // 
            // _FrmShowSql_Toolbars_Dock_Area_Top
            // 
            this._FrmShowSql_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmShowSql_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmShowSql_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._FrmShowSql_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmShowSql_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._FrmShowSql_Toolbars_Dock_Area_Top.Name = "_FrmShowSql_Toolbars_Dock_Area_Top";
            this._FrmShowSql_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(624, 55);
            this._FrmShowSql_Toolbars_Dock_Area_Top.ToolbarsManager = this._utm;
            // 
            // _FrmShowSql_Toolbars_Dock_Area_Bottom
            // 
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 341);
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.Name = "_FrmShowSql_Toolbars_Dock_Area_Bottom";
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(624, 4);
            this._FrmShowSql_Toolbars_Dock_Area_Bottom.ToolbarsManager = this._utm;
            // 
            // FrmTextVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 345);
            this.Controls.Add(this.FrmShowSql_Fill_Panel);
            this.Controls.Add(this._FrmShowSql_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._FrmShowSql_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._FrmShowSql_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._FrmShowSql_Toolbars_Dock_Area_Top);
            this.Name = "FrmTextVisualizer";
            this.ShowIcon = false;
            this.Text = "Text Visualizer";
            this.FrmShowSql_Fill_Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager _utm;
        private System.Windows.Forms.Panel FrmShowSql_Fill_Panel;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmShowSql_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmShowSql_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmShowSql_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmShowSql_Toolbars_Dock_Area_Bottom;
        private ICSharpCode.TextEditor.TextEditorControl _editor;
        private System.Windows.Forms.ImageList _img32;
        private System.Windows.Forms.ImageList _img16;
    }
}