namespace SqlEditor
{
	partial class FrmFindReplaceSimple
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblReplaceWith = new System.Windows.Forms.Label();
            this._tbSearchTerm = new System.Windows.Forms.TextBox();
            this._tbReplacementText = new System.Windows.Forms.TextBox();
            this.chkMatchWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.btnFindNext = new Infragistics.Win.Misc.UltraButton();
            this.btnFindPrevious = new Infragistics.Win.Misc.UltraButton();
            this.btnHighlightAll = new Infragistics.Win.Misc.UltraButton();
            this.btnReplace = new Infragistics.Win.Misc.UltraButton();
            this.btnReplaceAll = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this._lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Fi&nd:";
            // 
            // lblReplaceWith
            // 
            this.lblReplaceWith.AutoSize = true;
            this.lblReplaceWith.Location = new System.Drawing.Point(12, 41);
            this.lblReplaceWith.Name = "lblReplaceWith";
            this.lblReplaceWith.Size = new System.Drawing.Size(72, 13);
            this.lblReplaceWith.TabIndex = 2;
            this.lblReplaceWith.Text = "Re&place with:";
            // 
            // _tbSearchTerm
            // 
            this._tbSearchTerm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbSearchTerm.Location = new System.Drawing.Point(90, 12);
            this._tbSearchTerm.Name = "_tbSearchTerm";
            this._tbSearchTerm.Size = new System.Drawing.Size(322, 20);
            this._tbSearchTerm.TabIndex = 1;
            // 
            // _tbReplacementText
            // 
            this._tbReplacementText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._tbReplacementText.Location = new System.Drawing.Point(90, 38);
            this._tbReplacementText.Name = "_tbReplacementText";
            this._tbReplacementText.Size = new System.Drawing.Size(322, 20);
            this._tbReplacementText.TabIndex = 2;
            this._tbReplacementText.Enter += new System.EventHandler(this.TxtReplaceWith_Enter);
            // 
            // chkMatchWholeWord
            // 
            this.chkMatchWholeWord.AutoSize = true;
            this.chkMatchWholeWord.Location = new System.Drawing.Point(178, 64);
            this.chkMatchWholeWord.Name = "chkMatchWholeWord";
            this.chkMatchWholeWord.Size = new System.Drawing.Size(113, 17);
            this.chkMatchWholeWord.TabIndex = 4;
            this.chkMatchWholeWord.Text = "Match &whole word";
            this.chkMatchWholeWord.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(90, 64);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 3;
            this.chkMatchCase.Text = "Match &case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // btnFindNext
            // 
            this.btnFindNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNext.Location = new System.Drawing.Point(337, 96);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(75, 23);
            this.btnFindNext.TabIndex = 14;
            this.btnFindNext.Text = "&Next";
            this.btnFindNext.Click += new System.EventHandler(this.BtnFindNext_Click);
            // 
            // btnFindPrevious
            // 
            this.btnFindPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindPrevious.Location = new System.Drawing.Point(256, 96);
            this.btnFindPrevious.Name = "btnFindPrevious";
            this.btnFindPrevious.Size = new System.Drawing.Size(75, 23);
            this.btnFindPrevious.TabIndex = 15;
            this.btnFindPrevious.Text = "Pre&vious";
            this.btnFindPrevious.Click += new System.EventHandler(this.BtnFindPrevious_Click);
            // 
            // btnHighlightAll
            // 
            this.btnHighlightAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHighlightAll.Location = new System.Drawing.Point(195, 127);
            this.btnHighlightAll.Name = "btnHighlightAll";
            this.btnHighlightAll.Size = new System.Drawing.Size(136, 23);
            this.btnHighlightAll.TabIndex = 20;
            this.btnHighlightAll.Text = "Find && Highlight &All";
            this.btnHighlightAll.Click += new System.EventHandler(this.BtnHighlightAll_Click);
            // 
            // btnReplace
            // 
            this.btnReplace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplace.Location = new System.Drawing.Point(174, 127);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new System.Drawing.Size(75, 23);
            this.btnReplace.TabIndex = 21;
            this.btnReplace.Text = "&Replace";
            this.btnReplace.Click += new System.EventHandler(this.BtnReplace_Click);
            // 
            // btnReplaceAll
            // 
            this.btnReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReplaceAll.Location = new System.Drawing.Point(256, 127);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new System.Drawing.Size(75, 23);
            this.btnReplaceAll.TabIndex = 22;
            this.btnReplaceAll.Text = "Replace &All";
            this.btnReplaceAll.Click += new System.EventHandler(this.BtnReplaceAll_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(337, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 27;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // _lblStatus
            // 
            this._lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._lblStatus.Location = new System.Drawing.Point(0, 166);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(424, 16);
            this._lblStatus.TabIndex = 28;
            // 
            // FrmFindReplace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(424, 182);
            this.Controls.Add(this._lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnReplaceAll);
            this.Controls.Add(this.btnReplace);
            this.Controls.Add(this.btnFindPrevious);
            this.Controls.Add(this.btnFindNext);
            this.Controls.Add(this.chkMatchCase);
            this.Controls.Add(this.chkMatchWholeWord);
            this.Controls.Add(this._tbReplacementText);
            this.Controls.Add(this._tbSearchTerm);
            this.Controls.Add(this.lblReplaceWith);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHighlightAll);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::SqlEditor.Properties.Settings.Default, "FrmFindReplace_Location", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Location = global::SqlEditor.Properties.Settings.Default.FrmFindReplace_Location;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 216);
            this.Name = "FrmFindReplace";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Find and Replace";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindAndReplaceForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblReplaceWith;
		private System.Windows.Forms.TextBox _tbSearchTerm;
        private System.Windows.Forms.TextBox _tbReplacementText;
		private System.Windows.Forms.CheckBox chkMatchWholeWord;
        private System.Windows.Forms.CheckBox chkMatchCase;
        private Infragistics.Win.Misc.UltraButton btnFindNext;
        private Infragistics.Win.Misc.UltraButton btnFindPrevious;
        private Infragistics.Win.Misc.UltraButton btnHighlightAll;
        private Infragistics.Win.Misc.UltraButton btnReplace;
        private Infragistics.Win.Misc.UltraButton btnReplaceAll;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private System.Windows.Forms.Label _lblStatus;
	}
}