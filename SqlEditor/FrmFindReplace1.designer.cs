namespace SqlEditor
{
	partial class FrmFindReplace1
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab3 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.ultraTabPageControl1 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this._ubFindNext = new Infragistics.Win.Misc.UltraButton();
            this._ucFind = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraLabel1 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl2 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraGroupBox4 = new Infragistics.Win.Misc.UltraGroupBox();
            this._cbInSelection = new System.Windows.Forms.CheckBox();
            this._ubReplaceAll = new Infragistics.Win.Misc.UltraButton();
            this._ubReplace = new Infragistics.Win.Misc.UltraButton();
            this._ucReplaceWith = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraLabel3 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraButton1 = new Infragistics.Win.Misc.UltraButton();
            this._ucReplaceFind = new Infragistics.Win.UltraWinGrid.UltraCombo();
            this.ultraLabel2 = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl3 = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ultraGroupBox2 = new Infragistics.Win.Misc.UltraGroupBox();
            this._rbDirectionDown = new System.Windows.Forms.RadioButton();
            this._rbDirectionUp = new System.Windows.Forms.RadioButton();
            this._utcTabs = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.ultraGroupBox3 = new Infragistics.Win.Misc.UltraGroupBox();
            this._cbMatchCase = new System.Windows.Forms.CheckBox();
            this._cbMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.ultraGroupBox1 = new Infragistics.Win.Misc.UltraGroupBox();
            this._rbSearchModeRegularExpression = new System.Windows.Forms.RadioButton();
            this._rbSearchModeNormal = new System.Windows.Forms.RadioButton();
            this._ulStatus = new Infragistics.Win.Misc.UltraLabel();
            this.ultraTabPageControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ucFind)).BeginInit();
            this.ultraTabPageControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).BeginInit();
            this.ultraGroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ucReplaceWith)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._ucReplaceFind)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).BeginInit();
            this.ultraGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).BeginInit();
            this._utcTabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).BeginInit();
            this.ultraGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).BeginInit();
            this.ultraGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageControl1
            // 
            this.ultraTabPageControl1.Controls.Add(this._ubFindNext);
            this.ultraTabPageControl1.Controls.Add(this._ucFind);
            this.ultraTabPageControl1.Controls.Add(this.ultraLabel1);
            this.ultraTabPageControl1.Location = new System.Drawing.Point(1, 22);
            this.ultraTabPageControl1.Name = "ultraTabPageControl1";
            this.ultraTabPageControl1.Size = new System.Drawing.Size(531, 132);
            // 
            // _ubFindNext
            // 
            this._ubFindNext.Location = new System.Drawing.Point(415, 13);
            this._ubFindNext.Name = "_ubFindNext";
            this._ubFindNext.Size = new System.Drawing.Size(102, 23);
            this._ubFindNext.TabIndex = 2;
            this._ubFindNext.Text = "Find Next";
            this._ubFindNext.Click += new System.EventHandler(this.BtnFindNext_Click);
            // 
            // _ucFind
            // 
            this._ucFind.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this._ucFind.Location = new System.Drawing.Point(103, 14);
            this._ucFind.Name = "_ucFind";
            this._ucFind.Size = new System.Drawing.Size(306, 22);
            this._ucFind.TabIndex = 1;
            // 
            // ultraLabel1
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.TextVAlignAsString = "Middle";
            this.ultraLabel1.Appearance = appearance1;
            this.ultraLabel1.Location = new System.Drawing.Point(8, 14);
            this.ultraLabel1.Name = "ultraLabel1";
            this.ultraLabel1.Size = new System.Drawing.Size(89, 23);
            this.ultraLabel1.TabIndex = 0;
            this.ultraLabel1.Text = "Find what:";
            // 
            // ultraTabPageControl2
            // 
            this.ultraTabPageControl2.Controls.Add(this.ultraGroupBox4);
            this.ultraTabPageControl2.Controls.Add(this._ubReplace);
            this.ultraTabPageControl2.Controls.Add(this._ucReplaceWith);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel3);
            this.ultraTabPageControl2.Controls.Add(this.ultraButton1);
            this.ultraTabPageControl2.Controls.Add(this._ucReplaceFind);
            this.ultraTabPageControl2.Controls.Add(this.ultraLabel2);
            this.ultraTabPageControl2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl2.Name = "ultraTabPageControl2";
            this.ultraTabPageControl2.Size = new System.Drawing.Size(531, 132);
            // 
            // ultraGroupBox4
            // 
            this.ultraGroupBox4.Controls.Add(this._cbInSelection);
            this.ultraGroupBox4.Controls.Add(this._ubReplaceAll);
            this.ultraGroupBox4.Location = new System.Drawing.Point(275, 70);
            this.ultraGroupBox4.Name = "ultraGroupBox4";
            this.ultraGroupBox4.Size = new System.Drawing.Size(248, 35);
            this.ultraGroupBox4.TabIndex = 9;
            this.ultraGroupBox4.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // _cbInSelection
            // 
            this._cbInSelection.AutoSize = true;
            this._cbInSelection.BackColor = System.Drawing.Color.Transparent;
            this._cbInSelection.Location = new System.Drawing.Point(6, 10);
            this._cbInSelection.Name = "_cbInSelection";
            this._cbInSelection.Size = new System.Drawing.Size(80, 17);
            this._cbInSelection.TabIndex = 2;
            this._cbInSelection.Text = "In selection";
            this._cbInSelection.UseVisualStyleBackColor = false;
            // 
            // _ubReplaceAll
            // 
            this._ubReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._ubReplaceAll.Location = new System.Drawing.Point(140, 6);
            this._ubReplaceAll.Name = "_ubReplaceAll";
            this._ubReplaceAll.Size = new System.Drawing.Size(102, 23);
            this._ubReplaceAll.TabIndex = 10;
            this._ubReplaceAll.Text = "Replace All";
            // 
            // _ubReplace
            // 
            this._ubReplace.Location = new System.Drawing.Point(415, 41);
            this._ubReplace.Name = "_ubReplace";
            this._ubReplace.Size = new System.Drawing.Size(102, 23);
            this._ubReplace.TabIndex = 8;
            this._ubReplace.Text = "Replace";
            // 
            // _ucReplaceWith
            // 
            this._ucReplaceWith.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this._ucReplaceWith.Location = new System.Drawing.Point(103, 42);
            this._ucReplaceWith.Name = "_ucReplaceWith";
            this._ucReplaceWith.Size = new System.Drawing.Size(306, 22);
            this._ucReplaceWith.TabIndex = 7;
            // 
            // ultraLabel3
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.TextVAlignAsString = "Middle";
            this.ultraLabel3.Appearance = appearance2;
            this.ultraLabel3.Location = new System.Drawing.Point(8, 42);
            this.ultraLabel3.Name = "ultraLabel3";
            this.ultraLabel3.Size = new System.Drawing.Size(89, 23);
            this.ultraLabel3.TabIndex = 6;
            this.ultraLabel3.Text = "Replace with:";
            // 
            // ultraButton1
            // 
            this.ultraButton1.Location = new System.Drawing.Point(415, 13);
            this.ultraButton1.Name = "ultraButton1";
            this.ultraButton1.Size = new System.Drawing.Size(102, 23);
            this.ultraButton1.TabIndex = 5;
            this.ultraButton1.Text = "Find Next";
            // 
            // _ucReplaceFind
            // 
            this._ucReplaceFind.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Office2010;
            this._ucReplaceFind.Location = new System.Drawing.Point(103, 14);
            this._ucReplaceFind.Name = "_ucReplaceFind";
            this._ucReplaceFind.Size = new System.Drawing.Size(306, 22);
            this._ucReplaceFind.TabIndex = 4;
            // 
            // ultraLabel2
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.TextVAlignAsString = "Middle";
            this.ultraLabel2.Appearance = appearance3;
            this.ultraLabel2.Location = new System.Drawing.Point(8, 14);
            this.ultraLabel2.Name = "ultraLabel2";
            this.ultraLabel2.Size = new System.Drawing.Size(89, 23);
            this.ultraLabel2.TabIndex = 3;
            this.ultraLabel2.Text = "Find what:";
            // 
            // ultraTabPageControl3
            // 
            this.ultraTabPageControl3.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPageControl3.Name = "ultraTabPageControl3";
            this.ultraTabPageControl3.Size = new System.Drawing.Size(531, 132);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.4F));
            this.tableLayoutPanel1.Controls.Add(this.ultraGroupBox2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this._utcTabs, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ultraGroupBox3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ultraGroupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this._ulStatus, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(539, 265);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ultraGroupBox2
            // 
            this.ultraGroupBox2.Controls.Add(this._rbDirectionDown);
            this.ultraGroupBox2.Controls.Add(this._rbDirectionUp);
            this.ultraGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox2.Location = new System.Drawing.Point(359, 162);
            this.ultraGroupBox2.Margin = new System.Windows.Forms.Padding(1, 1, 2, 1);
            this.ultraGroupBox2.Name = "ultraGroupBox2";
            this.ultraGroupBox2.Size = new System.Drawing.Size(178, 72);
            this.ultraGroupBox2.TabIndex = 3;
            this.ultraGroupBox2.Text = "Direction";
            this.ultraGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // _rbDirectionDown
            // 
            this._rbDirectionDown.AutoSize = true;
            this._rbDirectionDown.BackColor = System.Drawing.Color.Transparent;
            this._rbDirectionDown.Checked = true;
            this._rbDirectionDown.Location = new System.Drawing.Point(6, 42);
            this._rbDirectionDown.Name = "_rbDirectionDown";
            this._rbDirectionDown.Size = new System.Drawing.Size(53, 17);
            this._rbDirectionDown.TabIndex = 1;
            this._rbDirectionDown.TabStop = true;
            this._rbDirectionDown.Text = "Down";
            this._rbDirectionDown.UseVisualStyleBackColor = false;
            // 
            // _rbDirectionUp
            // 
            this._rbDirectionUp.AutoSize = true;
            this._rbDirectionUp.BackColor = System.Drawing.Color.Transparent;
            this._rbDirectionUp.Location = new System.Drawing.Point(6, 19);
            this._rbDirectionUp.Name = "_rbDirectionUp";
            this._rbDirectionUp.Size = new System.Drawing.Size(39, 17);
            this._rbDirectionUp.TabIndex = 0;
            this._rbDirectionUp.Text = "Up";
            this._rbDirectionUp.UseVisualStyleBackColor = false;
            // 
            // _utcTabs
            // 
            this._utcTabs.CloseButtonLocation = Infragistics.Win.UltraWinTabs.TabCloseButtonLocation.None;
            this.tableLayoutPanel1.SetColumnSpan(this._utcTabs, 3);
            this._utcTabs.Controls.Add(this.ultraTabSharedControlsPage1);
            this._utcTabs.Controls.Add(this.ultraTabPageControl1);
            this._utcTabs.Controls.Add(this.ultraTabPageControl2);
            this._utcTabs.Controls.Add(this.ultraTabPageControl3);
            this._utcTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this._utcTabs.Location = new System.Drawing.Point(3, 3);
            this._utcTabs.Name = "_utcTabs";
            this._utcTabs.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this._utcTabs.Size = new System.Drawing.Size(533, 155);
            this._utcTabs.TabIndex = 0;
            ultraTab2.AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            ultraTab2.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab2.CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            ultraTab2.FixedWidth = 75;
            ultraTab2.Key = "Find";
            ultraTab2.TabPage = this.ultraTabPageControl1;
            ultraTab2.Text = "Find";
            ultraTab3.AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            ultraTab3.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab3.CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            ultraTab3.FixedWidth = 75;
            ultraTab3.Key = "Replace";
            ultraTab3.TabPage = this.ultraTabPageControl2;
            ultraTab3.Text = "Replace";
            ultraTab4.AllowClosing = Infragistics.Win.DefaultableBoolean.False;
            ultraTab4.AllowMoving = Infragistics.Win.DefaultableBoolean.False;
            ultraTab4.CloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.Never;
            ultraTab4.FixedWidth = 75;
            ultraTab4.Key = "Mark";
            ultraTab4.TabPage = this.ultraTabPageControl3;
            ultraTab4.Text = "Mark";
            this._utcTabs.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab2,
            ultraTab3,
            ultraTab4});
            this._utcTabs.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(531, 132);
            // 
            // ultraGroupBox3
            // 
            this.ultraGroupBox3.Controls.Add(this._cbMatchCase);
            this.ultraGroupBox3.Controls.Add(this._cbMatchWholeWord);
            this.ultraGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox3.Location = new System.Drawing.Point(180, 162);
            this.ultraGroupBox3.Margin = new System.Windows.Forms.Padding(1, 1, 2, 1);
            this.ultraGroupBox3.Name = "ultraGroupBox3";
            this.ultraGroupBox3.Size = new System.Drawing.Size(176, 72);
            this.ultraGroupBox3.TabIndex = 4;
            this.ultraGroupBox3.Text = "Options";
            this.ultraGroupBox3.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // _cbMatchCase
            // 
            this._cbMatchCase.AutoSize = true;
            this._cbMatchCase.BackColor = System.Drawing.Color.Transparent;
            this._cbMatchCase.Location = new System.Drawing.Point(6, 42);
            this._cbMatchCase.Name = "_cbMatchCase";
            this._cbMatchCase.Size = new System.Drawing.Size(82, 17);
            this._cbMatchCase.TabIndex = 1;
            this._cbMatchCase.Text = "Match case";
            this._cbMatchCase.UseVisualStyleBackColor = false;
            // 
            // _cbMatchWholeWord
            // 
            this._cbMatchWholeWord.AutoSize = true;
            this._cbMatchWholeWord.BackColor = System.Drawing.Color.Transparent;
            this._cbMatchWholeWord.Location = new System.Drawing.Point(6, 20);
            this._cbMatchWholeWord.Name = "_cbMatchWholeWord";
            this._cbMatchWholeWord.Size = new System.Drawing.Size(135, 17);
            this._cbMatchWholeWord.TabIndex = 0;
            this._cbMatchWholeWord.Text = "Match whole word only";
            this._cbMatchWholeWord.UseVisualStyleBackColor = false;
            // 
            // ultraGroupBox1
            // 
            this.ultraGroupBox1.Controls.Add(this._rbSearchModeRegularExpression);
            this.ultraGroupBox1.Controls.Add(this._rbSearchModeNormal);
            this.ultraGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraGroupBox1.Location = new System.Drawing.Point(2, 162);
            this.ultraGroupBox1.Margin = new System.Windows.Forms.Padding(2, 1, 1, 1);
            this.ultraGroupBox1.Name = "ultraGroupBox1";
            this.ultraGroupBox1.Size = new System.Drawing.Size(176, 72);
            this.ultraGroupBox1.TabIndex = 2;
            this.ultraGroupBox1.Text = "Search Mode";
            this.ultraGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.Office2007;
            // 
            // _rbSearchModeRegularExpression
            // 
            this._rbSearchModeRegularExpression.AutoSize = true;
            this._rbSearchModeRegularExpression.BackColor = System.Drawing.Color.Transparent;
            this._rbSearchModeRegularExpression.Location = new System.Drawing.Point(6, 42);
            this._rbSearchModeRegularExpression.Name = "_rbSearchModeRegularExpression";
            this._rbSearchModeRegularExpression.Size = new System.Drawing.Size(116, 17);
            this._rbSearchModeRegularExpression.TabIndex = 1;
            this._rbSearchModeRegularExpression.Text = "Regular Expression";
            this._rbSearchModeRegularExpression.UseVisualStyleBackColor = false;
            // 
            // _rbSearchModeNormal
            // 
            this._rbSearchModeNormal.AutoSize = true;
            this._rbSearchModeNormal.BackColor = System.Drawing.Color.Transparent;
            this._rbSearchModeNormal.Checked = true;
            this._rbSearchModeNormal.Location = new System.Drawing.Point(6, 19);
            this._rbSearchModeNormal.Name = "_rbSearchModeNormal";
            this._rbSearchModeNormal.Size = new System.Drawing.Size(58, 17);
            this._rbSearchModeNormal.TabIndex = 0;
            this._rbSearchModeNormal.TabStop = true;
            this._rbSearchModeNormal.Text = "Normal";
            this._rbSearchModeNormal.UseVisualStyleBackColor = false;
            // 
            // _ulStatus
            // 
            this._ulStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._ulStatus.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this._ulStatus, 3);
            this._ulStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this._ulStatus.Location = new System.Drawing.Point(3, 250);
            this._ulStatus.Name = "_ulStatus";
            this._ulStatus.Size = new System.Drawing.Size(0, 0);
            this._ulStatus.TabIndex = 1;
            // 
            // FrmFindReplace1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(539, 265);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::SqlEditor.Properties.Settings.Default, "FrmFindReplace_Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = global::SqlEditor.Properties.Settings.Default.FrmFindReplace_Location;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 216);
            this.Name = "FrmFindReplace1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Find and Replace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAndReplaceForm_FormClosing);
            this.ultraTabPageControl1.ResumeLayout(false);
            this.ultraTabPageControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ucFind)).EndInit();
            this.ultraTabPageControl2.ResumeLayout(false);
            this.ultraTabPageControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox4)).EndInit();
            this.ultraGroupBox4.ResumeLayout(false);
            this.ultraGroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._ucReplaceWith)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._ucReplaceFind)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox2)).EndInit();
            this.ultraGroupBox2.ResumeLayout(false);
            this.ultraGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._utcTabs)).EndInit();
            this._utcTabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox3)).EndInit();
            this.ultraGroupBox3.ResumeLayout(false);
            this.ultraGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraGroupBox1)).EndInit();
            this.ultraGroupBox1.ResumeLayout(false);
            this.ultraGroupBox1.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox2;
        private System.Windows.Forms.RadioButton _rbDirectionDown;
        private System.Windows.Forms.RadioButton _rbDirectionUp;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl _utcTabs;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl1;
        private Infragistics.Win.UltraWinGrid.UltraCombo _ucFind;
        private Infragistics.Win.Misc.UltraLabel ultraLabel1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl2;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageControl3;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox1;
        private System.Windows.Forms.RadioButton _rbSearchModeRegularExpression;
        private System.Windows.Forms.RadioButton _rbSearchModeNormal;
        private Infragistics.Win.Misc.UltraLabel _ulStatus;
        private Infragistics.Win.Misc.UltraButton _ubFindNext;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox4;
        private System.Windows.Forms.CheckBox _cbInSelection;
        private Infragistics.Win.Misc.UltraButton _ubReplaceAll;
        private Infragistics.Win.Misc.UltraButton _ubReplace;
        private Infragistics.Win.UltraWinGrid.UltraCombo _ucReplaceWith;
        private Infragistics.Win.Misc.UltraLabel ultraLabel3;
        private Infragistics.Win.Misc.UltraButton ultraButton1;
        private Infragistics.Win.UltraWinGrid.UltraCombo _ucReplaceFind;
        private Infragistics.Win.Misc.UltraLabel ultraLabel2;
        private Infragistics.Win.Misc.UltraGroupBox ultraGroupBox3;
        private System.Windows.Forms.CheckBox _cbMatchCase;
        private System.Windows.Forms.CheckBox _cbMatchWholeWord;

    }
}