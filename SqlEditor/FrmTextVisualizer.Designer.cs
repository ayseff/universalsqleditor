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
            this.FrmShowSql_Fill_Panel = new System.Windows.Forms.Panel();
            this._editor = new ICSharpCode.TextEditor.TextEditorControl();
            this._FrmShowSql_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._utm = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
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
            this.FrmShowSql_Fill_Panel.Location = new System.Drawing.Point(4, 27);
            this.FrmShowSql_Fill_Panel.Name = "FrmShowSql_Fill_Panel";
            this.FrmShowSql_Fill_Panel.Size = new System.Drawing.Size(616, 314);
            this.FrmShowSql_Fill_Panel.TabIndex = 0;
            // 
            // _editor
            // 
            this._editor.CausesValidation = false;
            this._editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this._editor.IsReadOnly = false;
            this._editor.Location = new System.Drawing.Point(0, 0);
            this._editor.Name = "_editor";
            this._editor.ShowLineNumbers = false;
            this._editor.Size = new System.Drawing.Size(616, 314);
            this._editor.TabIndex = 0;
            // 
            // _FrmShowSql_Toolbars_Dock_Area_Left
            // 
            this._FrmShowSql_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmShowSql_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmShowSql_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._FrmShowSql_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmShowSql_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._FrmShowSql_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._FrmShowSql_Toolbars_Dock_Area_Left.Name = "_FrmShowSql_Toolbars_Dock_Area_Left";
            this._FrmShowSql_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 314);
            this._FrmShowSql_Toolbars_Dock_Area_Left.ToolbarsManager = this._utm;
            // 
            // _utm
            // 
            this._utm.DesignerFlags = 1;
            this._utm.DockWithinContainer = this;
            this._utm.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this._utm.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedSizable;
            // 
            // _FrmShowSql_Toolbars_Dock_Area_Right
            // 
            this._FrmShowSql_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmShowSql_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmShowSql_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._FrmShowSql_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmShowSql_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._FrmShowSql_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(620, 27);
            this._FrmShowSql_Toolbars_Dock_Area_Right.Name = "_FrmShowSql_Toolbars_Dock_Area_Right";
            this._FrmShowSql_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 314);
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
            this._FrmShowSql_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(624, 27);
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
    }
}