using System;
using System.ComponentModel;
using System.Data.Common;
using Utilities.Text;

namespace SqlEditor.Database.MsAccess
{
    public class MsAccess2003ConnectionStringBuilder : DbConnectionStringBuilder
    {
        private const string MICROSOFT_JET_OLEDB = "Microsoft.Jet.OLEDB.4.0";
        private string _dataSource;
        private string _password;
        protected string _provider = MICROSOFT_JET_OLEDB;
        private string _userId;

        public MsAccess2003ConnectionStringBuilder()
        {
            Provider = _provider;
            UserId = "Admin";
        }

        public MsAccess2003ConnectionStringBuilder(string connectionString)
        {
            if (connectionString.IsNullEmptyOrWhitespace())
            {
                ConnectionString = connectionString;
            }
            Provider = _provider;
        }


        [DisplayName("Data Source")]
        public string DataSource
        {
            get { return _dataSource; }
            set
            {
                SetValue("Data Source", value);
                _dataSource = value;
            }
        }

        [DisplayName("User Id")]
        public string UserId
        {
            get { return _userId; }
            set
            {
                SetValue("User Id", value);
                _userId = value;
            }
        }

        [DisplayName("Password")]
        public string Password
        {
            get { return _password; }
            set
            {
                SetValue("Password", value);
                _password = value;
            }
        }

        [DisplayName("Provider")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public string Provider
        {
            get { return _provider; }
            set
            {
                SetValue("Provider", value);
                _provider = value;
            }
        }

        [DisplayName("Connection String")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public new string ConnectionString
        {
            get { return base.ConnectionString; }
            set { base.ConnectionString = value; }
        }

        private void SetValue(string keyword, string value)
        {
            if (value == null) throw new ArgumentNullException("keyword");
            base[keyword] = value;
        }
    }
}