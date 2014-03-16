using System.Drawing;
using System.Windows.Forms;

namespace SqlEditor
{
    public partial class FrmSplash : Form
    {
        private static FrmSplash _instance;
        public static FrmSplash Instance
        {
            get { return _instance ?? (_instance = new FrmSplash()); }
        }

        public FrmSplash()
        {
            InitializeComponent();
            _lblVersion.Text = "Version " + Application.ProductVersion;
            _uaiLoadingActivity.Location = new Point(150, _uaiLoadingActivity.Location.Y);
        }

        public void CloseForm()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() => CloseForm()));
                return;
            }
            Close();
        }
    }
}
