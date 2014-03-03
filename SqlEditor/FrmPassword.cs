using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor
{
    public partial class FrmPassword : Form
    {

        public string Password
        {
            get { return _utePassword.Text; }
        }

        public FrmPassword(string userId, string dataSource)
        {
            InitializeComponent();
            this.Text = "Password for " + dataSource;
            _uteUserId.Text = userId;
            _utePassword.Text = string.Empty;
        }
    }
}
