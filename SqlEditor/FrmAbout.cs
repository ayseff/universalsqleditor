using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Utilities.Forms.Dialogs;

namespace SqlEditor
{
    public partial class FrmAbout : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
            var tmpFile = Path.Combine(Path.GetTempPath(), "SQL Editor Change History.txt");
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SqlEditor.Change Log.txt"))
            {
                if (stream == null)
                {
                    Dialog.ShowErrorDialog(Application.ProductName, "Could not find change history file.",
                        "Please reinstall the application.", null);
                    return;
                }
                try
                {
                    using (var file = File.Create(tmpFile))
                    {
                        stream.CopyTo(file);
                    }
                    Process.Start(tmpFile);
                }
                catch (Exception ex)
                {
                    _log.Error("Error saving or opening the stream.");
                    _log.Error(ex.Message, ex);
                    Dialog.ShowErrorDialog(Application.ProductName, "Error opening change history log.", ex.Message, ex.StackTrace);
                }
            }
        }
    }
}
