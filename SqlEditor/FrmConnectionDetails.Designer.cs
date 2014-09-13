namespace SqlEditor
{
    partial class FrmConnectionDetails
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
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo1 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Tests connection using supplied informtion.", Infragistics.Win.ToolTipImage.Default, "Test Connection", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo2 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Whether queries changing database records should be commited automatically. Unche" +
        "cking this option will allow you to manually commit or rollback your transaction" +
        ".", Infragistics.Win.ToolTipImage.Default, "Auto Commit", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo3 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Whether queries changing database records should be commited automatically. Unche" +
        "cking this option will allow you to manually commit or rollback your transaction" +
        ".", Infragistics.Win.ToolTipImage.Default, "Auto Commit", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo4 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Maximum number of records to return for SELECT queries. Please keep this to a rea" +
        "sonable number to avoid performance penalties.", Infragistics.Win.ToolTipImage.Default, "Maximum Results", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo5 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Maximum number of records to return for SELECT queries. Please keep this to a rea" +
        "sonable number to avoid performance penalties.", Infragistics.Win.ToolTipImage.Default, "Maximum Results", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo6 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("", Infragistics.Win.ToolTipImage.Default, "Connection Name", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinToolTip.UltraToolTipInfo ultraToolTipInfo7 = new Infragistics.Win.UltraWinToolTip.UltraToolTipInfo("Types of database whihc you are connecting to (i.e. ORACLE, DB2, etc.)", Infragistics.Win.ToolTipImage.Default, "Database Type", Infragistics.Win.DefaultableBoolean.Default);
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmConnectionDetails));
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._pgConnectionSimple = new System.Windows.Forms.PropertyGrid();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._pgConnectionAdvanced = new System.Windows.Forms.PropertyGrid();
            this.FrmConnectionEdit_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._ubCancel = new Infragistics.Win.Misc.UltraButton();
            this._ubOk = new Infragistics.Win.Misc.UltraButton();
            this._ubTestConnection = new Infragistics.Win.Misc.UltraButton();
            this._ugbDatabaseType = new Infragistics.Win.Misc.UltraGroupBox();
            this.ultraLabel4 = new Infragistics.Win.Misc.UltraLabel();
            this._uceAutoCommit = new Infragistics.Win.UltraWinEditors.UltraCheckEditor();
            this._uneMaxiumumResults = new Infragistics.Win.UltraWinEditors.UltraNumericEditor();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this._uteConnectionName = new Infragistics.Win.UltraWinEditors.UltraTextEditor();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this._uceDatabaseType = new Infragistics.Win.UltraWinEditors.UltraComboEditor();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this._utcTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this._utt = new Infragistics.Win.UltraWinToolTip.UltraToolTipManager(this.components);
            this.ultraTabPageControl1.SuspendLayout();
            this.ultraTabPageControl2.SuspendLayout();
            this.FrmConnectionEdit_Fill_Panel.ClientArea.SuspendLayout();
            this.FrmConnectionEdit_Fill_Panel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ugbDatabaseType)).BeginInit();
            this._ugbDatabaseType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._uceAutoCommit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._uneMaxiumumResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._uteConnectionName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._uceDatabaseType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).BeginInit();
            this._utcTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this._pgConnectionSimple);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 22);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(422, 288);
            // 
            // _pgConnectionSimple
            // 
            this._pgConnectionSimple.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pgConnectionSimple.Location = new System.Drawing.Point(0, 0);
            this._pgConnectionSimple.Name = "_pgConnectionSimple";
            this._pgConnectionSimple.Size = new System.Drawing.Size(422, 288);
            this._pgConnectionSimple.TabIndex = 2;
            this._pgConnectionSimple.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PgConnectionPropertyValueChanged);
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this._pgConnectionAdvanced);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(422, 288);
            // 
            // _pgConnectionAdvanced
            // 
            this._pgConnectionAdvanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this._pgConnectionAdvanced.Location = new System.Drawing.Point(0, 0);
            this._pgConnectionAdvanced.Name = "_pgConnectionAdvanced";
            this._pgConnectionAdvanced.Size = new System.Drawing.Size(422, 288);
            this._pgConnectionAdvanced.TabIndex = 1;
            this._pgConnectionAdvanced.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PgConnectionPropertyValueChanged);
            // 
            // FrmConnectionEdit_Fill_Panel
            // 
            // 
            // FrmConnectionEdit_Fill_Panel.ClientArea
            // 
            this.FrmConnectionEdit_Fill_Panel.ClientArea.Controls.Add(this.tableLayoutPanel1);
            this.FrmConnectionEdit_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.FrmConnectionEdit_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FrmConnectionEdit_Fill_Panel.Location = new System.Drawing.Point(0, 0);
            this.FrmConnectionEdit_Fill_Panel.Name = "FrmConnectionEdit_Fill_Panel";
            this.FrmConnectionEdit_Fill_Panel.Size = new System.Drawing.Size(436, 518);
            this.FrmConnectionEdit_Fill_Panel.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._ubCancel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this._ubOk, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this._ubTestConnection, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this._ugbDatabaseType, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraGroupBox2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(436, 518);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _ubCancel
            // 
            this._ubCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._ubCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._ubCancel.Location = new System.Drawing.Point(281, 487);
            this._ubCancel.Name = "_ubCancel";
            this._ubCancel.Size = new System.Drawing.Size(107, 23);
            this._ubCancel.TabIndex = 3;
            this._ubCancel.Text = "Cancel";
            this._ubCancel.Click += new System.EventHandler(this.UbCancelClick);
            // 
            // _ubOk
            // 
            this._ubOk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this._ubOk.Location = new System.Drawing.Point(164, 487);
            this._ubOk.Name = "_ubOk";
            this._ubOk.Size = new System.Drawing.Size(107, 23);
            this._ubOk.TabIndex = 2;
            this._ubOk.Text = "OK";
            this._ubOk.Click += new System.EventHandler(this.UbOkClick);
            // 
            // _ubTestConnection
            // 
            this._ubTestConnection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._ubTestConnection.Location = new System.Drawing.Point(48, 487);
            this._ubTestConnection.Name = "_ubTestConnection";
            this._ubTestConnection.Size = new System.Drawing.Size(107, 23);
            this._ubTestConnection.TabIndex = 1;
            this._ubTestConnection.Text = "Test Connection";
            ultraToolTipInfo1.ToolTipText = "Tests connection using supplied informtion.";
            ultraToolTipInfo1.ToolTipTitle = "Test Connection";
            this._utt.SetUltraToolTip(this._ubTestConnection, ultraToolTipInfo1);
            this._ubTestConnection.Click += new System.EventHandler(this.UbTestConnectionClick);
            // 
            // _ugbDatabaseType
            // 
            this.tableLayoutPanel1.SetColumnSpan(this._ugbDatabaseType, 3);
            this._ugbDatabaseType.Controls.Add(this.ultraLabel4);
            this._ugbDatabaseType.Controls.Add(this._uceAutoCommit);
            this._ugbDatabaseType.Controls.Add(this._uneMaxiumumResults);
            this._ugbDatabaseType.Controls.Add(this.ultraLabel3);
            this._ugbDatabaseType.Controls.Add(this._uteConnectionName);
            this._ugbDatabaseType.Controls.Add(this.ultraLabel2);
            this._ugbDatabaseType.Controls.Add(this._uceDatabaseType);
            this._ugbDatabaseType.Controls.Add(this.ultraLabel1);
            this._ugbDatabaseType.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ugbDatabaseType.Location = new System.Drawing.Point(3, 3);
            this._ugbDatabaseType.Name = "_ugbDatabaseType";
            this._ugbDatabaseType.Size = new System.Drawing.Size(430, 137);
            this._ugbDatabaseType.TabIndex = 4;
            this._ugbDatabaseType.Text = "Connection Details";
            this._ugbDatabaseType.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // ultraLabel4
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            this.ultraLabel4.Appearance = appearance1;
            this.ultraLabel4.AutoSize = true;
            this.ultraLabel4.Location = new System.Drawing.Point(6, 108);
            this.ultraLabel4.Name = "ultraLabel4";
            this.ultraLabel4.Size = new System.Drawing.Size(70, 14);
            this.ultraLabel4.TabIndex = 7;
            this.ultraLabel4.Text = "Auto Commit";
            ultraToolTipInfo2.ToolTipText = "Whether queries changing database records should be commited automatically. Unche" +
    "cking this option will allow you to manually commit or rollback your transaction" +
    ".";
            ultraToolTipInfo2.ToolTipTitle = "Auto Commit";
            this._utt.SetUltraToolTip(this.ultraLabel4, ultraToolTipInfo2);
            // 
            // _uceAutoCommit
            // 
            this._uceAutoCommit.Location = new System.Drawing.Point(112, 108);
            this._uceAutoCommit.Name = "_uceAutoCommit";
            this._uceAutoCommit.Size = new System.Drawing.Size(120, 20);
            this._uceAutoCommit.TabIndex = 6;
            ultraToolTipInfo3.ToolTipText = "Whether queries changing database records should be commited automatically. Unche" +
    "cking this option will allow you to manually commit or rollback your transaction" +
    ".";
            ultraToolTipInfo3.ToolTipTitle = "Auto Commit";
            this._utt.SetUltraToolTip(this._uceAutoCommit, ultraToolTipInfo3);
            // 
            // _uneMaxiumumResults
            // 
            this._uneMaxiumumResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._uneMaxiumumResults.Location = new System.Drawing.Point(112, 81);
            this._uneMaxiumumResults.MinValue = 1;
            this._uneMaxiumumResults.Name = "_uneMaxiumumResults";
            this._uneMaxiumumResults.PromptChar = ' ';
            this._uneMaxiumumResults.Size = new System.Drawing.Size(306, 21);
            this._uneMaxiumumResults.TabIndex = 5;
            ultraToolTipInfo4.ToolTipText = "Maximum number of records to return for SELECT queries. Please keep this to a rea" +
    "sonable number to avoid performance penalties.";
            ultraToolTipInfo4.ToolTipTitle = "Maximum Results";
            this._utt.SetUltraToolTip(this._uneMaxiumumResults, ultraToolTipInfo4);
            // 
            // ultraLabel3
            // 
            this.ultraLabel3.AutoSize = true;
            this.ultraLabel3.Location = new System.Drawing.Point(6, 85);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(94, 14);
            this.ultraLabel3.TabIndex = 4;
            this.ultraLabel3.Text = "Maximum Results";
            ultraToolTipInfo5.ToolTipText = "Maximum number of records to return for SELECT queries. Please keep this to a rea" +
    "sonable number to avoid performance penalties.";
            ultraToolTipInfo5.ToolTipTitle = "Maximum Results";
            this._utt.SetUltraToolTip(this.ultraLabel3, ultraToolTipInfo5);
            // 
            // _uteConnectionName
            // 
            this._uteConnectionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._uteConnectionName.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this._uteConnectionName.Location = new System.Drawing.Point(112, 27);
            this._uteConnectionName.Name = "_uteConnectionName";
            this._uteConnectionName.Size = new System.Drawing.Size(306, 21);
            this._uteConnectionName.TabIndex = 3;
            // 
            // ultraLabel2
            // 
            this.ultraLabel2.AutoSize = true;
            this.ultraLabel2.Location = new System.Drawing.Point(6, 31);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(95, 14);
            this.ultraLabel2.TabIndex = 2;
            this.ultraLabel2.Text = "Connection Name";
            ultraToolTipInfo6.ToolTipTitle = "Connection Name";
            this._utt.SetUltraToolTip(this.ultraLabel2, ultraToolTipInfo6);
            // 
            // _uceDatabaseType
            // 
            this._uceDatabaseType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._uceDatabaseType.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2007;
            this._uceDatabaseType.DropDownStyle = Infragistics.Win.DropDownStyle.DropDownList;
            this._uceDatabaseType.Location = new System.Drawing.Point(112, 54);
            this._uceDatabaseType.Name = "_uceDatabaseType";
            this._uceDatabaseType.Size = new System.Drawing.Size(306, 21);
            this._uceDatabaseType.TabIndex = 1;
            ultraToolTipInfo7.ToolTipText = "Types of database whihc you are connecting to (i.e. ORACLE, DB2, etc.)";
            ultraToolTipInfo7.ToolTipTitle = "Database Type";
            this._utt.SetUltraToolTip(this._uceDatabaseType, ultraToolTipInfo7);
            this._uceDatabaseType.SelectionChangeCommitted += new System.EventHandler(this.UceDatabaseTypeSelectionChangeCommitted);
            // 
            // ultraLabel1
            // 
            this.ultraLabel1.AutoSize = true;
            this.ultraLabel1.Location = new System.Drawing.Point(6, 58);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(81, 14);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Database Type";
            // 
            // ultraGroupBox2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ultraGroupBox2, 3);
            this.ultraGroupBox2.Controls.Add(this._utcTabs);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(3, 146);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(430, 330);
            this.ultraGroupBox2.TabIndex = 5;
            this.ultraGroupBox2.Text = "Connection String Details";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // _utcTabs
            // 
            this._utcTabs.CloseButtonLocation = Infragistics.Win.UltraWinTabs.TabCloseButtonLocation.None;
            this._utcTabs.Controls.Add(this.ultraTabSharedControlsPage1);
            this._utcTabs.Controls.Add(this.ultraTabPageControl1);
            this._utcTabs.Controls.Add(this.ultraTabPageControl2);
            this._utcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._utcTabs.Location = new System.Drawing.Point(3, 16);
            this._utcTabs.Name = "_utcTabs";
            this._utcTabs.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this._utcTabs.Size = new System.Drawing.Size(424, 311);
            this._utcTabs.TabIndex = 6;
            ultraTab1.Key = "Simple";
            ultraTab1.TabPage = this.ultraTabPageControl1;
            ultraTab1.Text = "Simple";
            ultraTab2.Key = "Advanced";
            ultraTab2.TabPage = this.ultraTabPageControl2;
            ultraTab2.Text = "Advanced";
            this._utcTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            this._utcTabs.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(422, 288);
            // 
            // _utt
            // 
            this._utt.AutoPopDelay = 4000;
            this._utt.ContainingControl = this;
            // 
            // FrmConnectionDetails
            // 
            this.AcceptButton = this._ubOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._ubCancel;
            this.ClientSize = new System.Drawing.Size(436, 518);
            this.Controls.Add(this.FrmConnectionEdit_Fill_Panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmConnectionDetails";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection Details";
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl2.ResumeLayout(false);
            this.FrmConnectionEdit_Fill_Panel.ClientArea.ResumeLayout(false);
            this.FrmConnectionEdit_Fill_Panel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._ugbDatabaseType)).EndInit();
            this._ugbDatabaseType.ResumeLayout(false);
            this._ugbDatabaseType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._uceAutoCommit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._uneMaxiumumResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._uteConnectionName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._uceDatabaseType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).EndInit();
            this._utcTabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraButton _ubCancel;
        private Infragistics.Win.Misc.UltraButton _ubOk;
        private Infragistics.Win.Misc.UltraButton _ubTestConnection;
        private Infragistics.Win.Misc.UltraPanel FrmConnectionEdit_Fill_Panel;
        private Infragistics.Win.Misc.UltraGroupBox _ugbDatabaseType;
        private Infragistics.Win.UltraWinEditors.UltraComboEditor _uceDatabaseType;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.PropertyGrid _pgConnectionAdvanced;
        private Infragistics.Win.UltraWinEditors.UltraTextEditor _uteConnectionName;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraLabel ultraLabel4;
        private Infragistics.Win.UltraWinEditors.UltraCheckEditor _uceAutoCommit;
        private Infragistics.Win.UltraWinEditors.UltraNumericEditor _uneMaxiumumResults;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.UltraWinToolTip.UltraToolTipManager _utt;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl _utcTabs;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private System.Windows.Forms.PropertyGrid _pgConnectionSimple;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
    }
}