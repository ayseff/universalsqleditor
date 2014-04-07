using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using JetBrains.Annotations;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeSimpleConnectionStringBuilder
    {
        private readonly SqlCeConnectionStringBuilder _connectionStringBuilder;

        [DisplayName("Data Source")]
        [Description("SQL Server Compact database file name to open")]
        public string DataSource 
        {
            get { return _connectionStringBuilder.DataSource; }
            set { _connectionStringBuilder.DataSource = value; }
        }

        [Description("Password used to connect to the database"), PasswordPropertyText(true)]
        public string Password
        {
            get { return _connectionStringBuilder.Password; }
            set { _connectionStringBuilder.Password = value; }
        }

        public SqlCeSimpleConnectionStringBuilder([NotNull] SqlCeConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
