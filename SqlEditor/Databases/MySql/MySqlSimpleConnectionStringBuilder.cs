using System;
using System.ComponentModel;
using JetBrains.Annotations;
using MySql.Data.MySqlClient;

namespace SqlEditor.Databases.MySql
{
    public class MySqlSimpleConnectionStringBuilder
    {
        private readonly MySqlConnectionStringBuilder _connectionStringBuilder;

        [DisplayName("Data Source")]
        [Description("Database server host name or IP address")]
        public string Server 
        {
            get { return _connectionStringBuilder.Server; }
            set { _connectionStringBuilder.Server = value; }
        }

        [DisplayName("Database")]
        [Description("Database name you wish to switch to upon connecting")]
        public string Database
        {
            get { return _connectionStringBuilder.Database; }
            set { _connectionStringBuilder.Database = value; }
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

        public MySqlSimpleConnectionStringBuilder([NotNull] MySqlConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
