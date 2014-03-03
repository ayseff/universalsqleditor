namespace SqlEditor
{
    partial class FrmPassword
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.FrmPassword_Fill_Panel = new System.Windows.Forms.Panel();
            this._ubCancel = new Infragistics.Win.Misc.UltraButton();
            this._ubOk = new Infragistics.Win.Misc.UltraButton();
            this._utePassword = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this._uteUserId = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this._FrmPassword_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._utm = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._FrmPassword_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmPassword_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._FrmPassword_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.FrmPassword_Fill_Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utePassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._uteUserId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).BeginInit();
            this.SuspendLayout();
            // 
            // FrmPassword_Fill_Panel
            // 
            this.FrmPassword_Fill_Panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.FrmPassword_Fill_Panel.Controls.Add(this._ubCancel);
            this.FrmPassword_Fill_Panel.Controls.Add(this._ubOk);
            this.FrmPassword_Fill_Panel.Controls.Add(this._utePassword);
            this.FrmPassword_Fill_Panel.Controls.Add(this.ultraLabel2);
            this.FrmPassword_Fill_Panel.Controls.Add(this.ultraLabel1);
            this.FrmPassword_Fill_Panel.Controls.Add(this._uteUserId);
            this.FrmPassword_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FrmPassword_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrmPassword_Fill_Panel.Location = new System.Drawing.Point(4, 26);
            this.FrmPassword_Fill_Panel.Name = "FrmPassword_Fill_Panel";
            this.FrmPassword_Fill_Panel.Size = new System.Drawing.Size(398, 118);
            this.FrmPassword_Fill_Panel.TabIndex = 0;
            // 
            // _ubCancel
            // 
            this._ubCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ubCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._ubCancel.Location = new System.Drawing.Point(202, 87);
            this._ubCancel.Name = "_ubCancel";
            this._ubCancel.Size = new System.Drawing.Size(75, 23);
            this._ubCancel.TabIndex = 3;
            this._ubCancel.Text = "Cancel";
            // 
            // _ubOk
            // 
            this._ubOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this._ubOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ubOk.Location = new System.Drawing.Point(121, 87);
            this._ubOk.Name = "_ubOk";
            this._ubOk.Size = new System.Drawing.Size(75, 23);
            this._ubOk.TabIndex = 2;
            this._ubOk.Text = "OK";
            // 
            // _utePassword
            // 
            this._utePassword.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this._utePassword.Location = new System.Drawing.Point(69, 50);
            this._utePassword.Name = "_utePassword";
            this._utePassword.PasswordChar = '*';
            this._utePassword.Size = new System.Drawing.Size(305, 21);
            this._utePassword.TabIndex = 1;
            // 
            // ultraLabel2
            // 
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance1;
            this.ultraLabel2.Location = new System.Drawing.Point(3, 54);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(67, 17);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "Password:";
            // 
            // ultraLabel1
            // 
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance2;
            this.ultraLabel1.Location = new System.Drawing.Point(1, 19);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(70, 21);
            this.ultraLabel1.TabIndex = 1;
            this.ultraLabel1.Text = "User Id:";
            // 
            // _uteUserId
            // 
            this._uteUserId.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this._uteUserId.Location = new System.Drawing.Point(69, 19);
            this._uteUserId.Name = "_uteUserId";
            this._uteUserId.ReadOnly = true;
            this._uteUserId.Size = new System.Drawing.Size(305, 21);
            this._uteUserId.TabIndex = 2;
            this._uteUserId.TabStop = false;
            // 
            // _FrmPassword_Toolbars_Dock_Area_Left
            // 
            this._FrmPassword_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmPassword_Toolbars_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmPassword_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._FrmPassword_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmPassword_Toolbars_Dock_Area_Left.InitialResizeAreaExtent = 4;
            this._FrmPassword_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 26);
            this._FrmPassword_Toolbars_Dock_Area_Left.Name = "_FrmPassword_Toolbars_Dock_Area_Left";
            this._FrmPassword_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(4, 118);
            this._FrmPassword_Toolbars_Dock_Area_Left.ToolbarsManager = this._utm;
            // 
            // _utm
            // 
            this._utm.DesignerFlags = 1;
            this._utm.DockWithinContainer = this;
            this._utm.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this._utm.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.RoundedFixed;
            // 
            // _FrmPassword_Toolbars_Dock_Area_Right
            // 
            this._FrmPassword_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmPassword_Toolbars_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmPassword_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._FrmPassword_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmPassword_Toolbars_Dock_Area_Right.InitialResizeAreaExtent = 4;
            this._FrmPassword_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(402, 26);
            this._FrmPassword_Toolbars_Dock_Area_Right.Name = "_FrmPassword_Toolbars_Dock_Area_Right";
            this._FrmPassword_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(4, 118);
            this._FrmPassword_Toolbars_Dock_Area_Right.ToolbarsManager = this._utm;
            // 
            // _FrmPassword_Toolbars_Dock_Area_Top
            // 
            this._FrmPassword_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmPassword_Toolbars_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmPassword_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._FrmPassword_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmPassword_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._FrmPassword_Toolbars_Dock_Area_Top.Name = "_FrmPassword_Toolbars_Dock_Area_Top";
            this._FrmPassword_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(406, 26);
            this._FrmPassword_Toolbars_Dock_Area_Top.ToolbarsManager = this._utm;
            // 
            // _FrmPassword_Toolbars_Dock_Area_Bottom
            // 
            this._FrmPassword_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._FrmPassword_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this._FrmPassword_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._FrmPassword_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._FrmPassword_Toolbars_Dock_Area_Bottom.InitialResizeAreaExtent = 4;
            this._FrmPassword_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 144);
            this._FrmPassword_Toolbars_Dock_Area_Bottom.Name = "_FrmPassword_Toolbars_Dock_Area_Bottom";
            this._FrmPassword_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(406, 4);
            this._FrmPassword_Toolbars_Dock_Area_Bottom.ToolbarsManager = this._utm;
            // 
            // FrmPassword
            // 
            this.AcceptButton = this._ubOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._ubCancel;
            this.ClientSize = new System.Drawing.Size(406, 148);
            this.ControlBox = false;
            this.Controls.Add(this.FrmPassword_Fill_Panel);
            this.Controls.Add(this._FrmPassword_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._FrmPassword_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._FrmPassword_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._FrmPassword_Toolbars_Dock_Area_Top);
            this.Name = "FrmPassword";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Password";
            this.FrmPassword_Fill_Panel.ResumeLayout(false);
            this.FrmPassword_Fill_Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utePassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._uteUserId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._utm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager _utm;
        private System.Windows.Forms.Panel FrmPassword_Fill_Panel;
        private Infragistics.Win.Misc.UltraButton _ubCancel;
        private Infragistics.Win.Misc.UltraButton _ubOk;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor _utePassword;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor _uteUserId;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmPassword_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmPassword_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmPassword_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _FrmPassword_Toolbars_Dock_Area_Bottom;
    }
}