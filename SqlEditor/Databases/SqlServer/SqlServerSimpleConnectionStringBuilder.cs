using System;
using System.ComponentModel;
using System.Data.SqlClient;
using JetBrains.Annotations;

namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerSimpleConnectionStringBuilder
    {
        private readonly SqlConnectionStringBuilder _connectionStringBuilder;

        [DisplayName("Data Source")]
        [Description("SQL Server host name or IP address")]
        public string DataSource 
        {
            get { return _connectionStringBuilder.DataSource; }
            set { _connectionStringBuilder.DataSource = value; }
        }

        [DisplayName("Initial Catalog")]
        [Description("Database name you wish to switch to upon connecting")]
        public string InitialCatalog
        {
            get { return _connectionStringBuilder.InitialCatalog; }
            set { _connectionStringBuilder.InitialCatalog = value; }
        }

        [DisplayName("User ID")]
        [Description("User ID used to connect to the database")]
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

        public SqlServerSimpleConnectionStringBuilder([NotNull] SqlConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
