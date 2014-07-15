namespace SqlEditor
{
    partial class FrmSplash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSplash));
            this._lblCopyright = new System.Windows.Forms.Label();
            this._uaiLoadingActivity = new Infragistics.Win.UltraActivityIndicator.UltraActivityIndicator();
            this._lblVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _lblCopyright
            // 
            this._lblCopyright.AutoSize = true;
            this._lblCopyright.BackColor = System.Drawing.Color.Transparent;
            this._lblCopyright.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblCopyright.ForeColor = System.Drawing.Color.White;
            this._lblCopyright.Location = new System.Drawing.Point(12, 271);
            this._lblCopyright.Name = "_lblCopyright";
            this._lblCopyright.Size = new System.Drawing.Size(209, 16);
            this._lblCopyright.TabIndex = 0;
            this._lblCopyright.Text = "Copyright © 2013 Mensur Software";
            // 
            // _uaiLoadingActivity
            // 
            this._uaiLoadingActivity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._uaiLoadingActivity.AnimationEnabled = true;
            this._uaiLoadingActivity.AnimationSpeed = 40;
            this._uaiLoadingActivity.CausesValidation = true;
            this._uaiLoadingActivity.Location = new System.Drawing.Point(150, 217);
            this._uaiLoadingActivity.Name = "_uaiLoadingActivity";
            this._uaiLoadingActivity.Size = new System.Drawing.Size(300, 20);
            this._uaiLoadingActivity.TabIndex = 1;
            this._uaiLoadingActivity.TabStop = true;
            this._uaiLoadingActivity.ViewStyle = Infragistics.Win.UltraActivityIndicator.ActivityIndicatorViewStyle.Aero;
            // 
            // _lblVersion
            // 
            this._lblVersion.BackColor = System.Drawing.Color.Transparent;
            this._lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblVersion.ForeColor = System.Drawing.Color.White;
            this._lblVersion.Location = new System.Drawing.Point(394, 271);
            this._lblVersion.Name = "_lblVersion";
            this._lblVersion.Size = new System.Drawing.Size(194, 16);
            this._lblVersion.TabIndex = 2;
            this._lblVersion.Text = "Version ";
            this._lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // FrmSplash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(600, 306);
            this.ControlBox = false;
            this.Controls.Add(this._lblVersion);
            this.Controls.Add(this._uaiLoadingActivity);
            this.Controls.Add(this._lblCopyright);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(600, 306);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 306);
            this.Name = "FrmSplash";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSplash";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lblCopyright;
        private Infragistics.Win.UltraActivityIndicator.UltraActivityIndicator _uaiLoadingActivity;
        private System.Windows.Forms.Label _lblVersion;
    }
}