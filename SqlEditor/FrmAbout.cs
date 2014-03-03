using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Utilities.Forms.Dialogs;

namespace SqlEditor
{
    public partial class FrmAbout : Form
    {
        private string AssemblyCopyright
        {
            get
            {
                var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                return attributes.Length == 0 ? "" : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public FrmAbout()
        {
            InitializeComponent();

            _lblCopyright.Text = AssemblyCopyright;
            _lblProduct.Text = Application.ProductName + " " + Application.ProductVersion;
        }

        private void LnkChangeHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var location = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, "Change Log.txt");
            if (!File.Exists(location))
            {
                Dialog.ShowErrorDialog(Application.ProductName, "Change history log not found.", string.Empty);
            }
            else
            {
                try
                {
                    Process.Start(location);
                }
                catch (Exception ex)
                {
                    Dialog.ShowErrorDialog(Application.ProductName, "Error opening change history log.", ex.Message);
                }
            }
        }
    }
}
