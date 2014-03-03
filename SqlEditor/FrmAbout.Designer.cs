namespace SqlEditor
{
    partial class FrmAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAbout));
            this._lblCopyright = new System.Windows.Forms.Label();
            this._lblProduct = new System.Windows.Forms.Label();
            this._lnkChangeHistory = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // _lblCopyright
            // 
            this._lblCopyright.AutoSize = true;
            this._lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this._lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblCopyright.ForeColor = System.Drawing.Color.White;
            this._lblCopyright.Location = new System.Drawing.Point(12, 262);
            this._lblCopyright.Name = "_lblCopyright";
            this._lblCopyright.Size = new System.Drawing.Size(45, 16);
            this._lblCopyright.TabIndex = 1;
            this._lblCopyright.Text = "label1";
            // 
            // _lblProduct
            // 
            this._lblProduct.AutoSize = true;
            this._lblProduct.BackColor = System.Drawing.Color.Transparent;
            this._lblProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblProduct.ForeColor = System.Drawing.Color.Black;
            this._lblProduct.Location = new System.Drawing.Point(451, 151);
            this._lblProduct.Name = "_lblProduct";
            this._lblProduct.Size = new System.Drawing.Size(45, 16);
            this._lblProduct.TabIndex = 2;
            this._lblProduct.Text = "label1";
            // 
            // _lnkChangeHistory
            // 
            this._lnkChangeHistory.AutoSize = true;
            this._lnkChangeHistory.Location = new System.Drawing.Point(451, 184);
            this._lnkChangeHistory.Name = "_lnkChangeHistory";
            this._lnkChangeHistory.Size = new System.Drawing.Size(109, 13);
            this._lnkChangeHistory.TabIndex = 3;
            this._lnkChangeHistory.TabStop = true;
            this._lnkChangeHistory.Text = "Show Change History";
            this._lnkChangeHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkChangeHistory_LinkClicked);
            // 
            // FrmAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(599, 308);
            this.Controls.Add(this._lnkChangeHistory);
            this.Controls.Add(this._lblProduct);
            this.Controls.Add(this._lblCopyright);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblCopyright;
        private System.Windows.Forms.Label _lblProduct;
        private System.Windows.Forms.LinkLabel _lnkChangeHistory;
    }
}