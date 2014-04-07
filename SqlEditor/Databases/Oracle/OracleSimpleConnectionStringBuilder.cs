using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Oracle.ManagedDataAccess.Client;

namespace SqlEditor.Databases.Oracle
{
    public class OracleSimpleConnectionStringBuilder
    {
        private readonly OracleConnectionStringBuilder _connectionStringBuilder;

        [DisplayName("Data Source")]
        [Description("Oracle data source. This may be Oracle SID (if you have specified your TNS_ADMIN directory) such as HR or a full descriptor such as (DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=hr-server)(PORT=1521))(CONNECT_DATA=(SID=HR)))")]
        public string Server 
        {
            get { return _connectionStringBuilder.DataSource; }
            set { _connectionStringBuilder.DataSource = value; }
        }

        [Description("User ID used to connect to the database")]
        [DisplayName("User ID")]
        public string UserID
        {
            get { return _connectionStringBuilder.UserID; }
            set { _connectionStringBuilder.UserID = value; }
        }

        [Description("Password used to connect to the database"), PasswordPropertyText(true)]
        public string Password
        {
            get { return _connectionStringBuilder.Password; }
            set { _connectionStringBuilder.Password = value; }
        }

        public OracleSimpleConnectionStringBuilder([NotNull] OracleConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
