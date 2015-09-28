using System;
using System.ComponentModel;
using JetBrains.Annotations;

namespace SqlEditor.Databases.PostgreSql
{
    public class PostgreSqlSimpleConnectionStringBuilder
    {
        private readonly Npgsql.NpgsqlConnectionStringBuilder _connectionStringBuilder;

        [DisplayName("Data Source")]
        [Description("PostgreSQL server host name or IP address")]
        public string Server 
        {
            get { return _connectionStringBuilder.Host; }
            set { _connectionStringBuilder.Host = value; }
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
            get { return _connectionStringBuilder.Username; }
            set { _connectionStringBuilder.Username = value; }
        }

        [Description("Password used to connect to the database"), PasswordPropertyText(true)]
        public string Password
        {
            get { return _connectionStringBuilder.Password; }
            set { _connectionStringBuilder.Password = value; }
        }

        public PostgreSqlSimpleConnectionStringBuilder([NotNull] Npgsql.NpgsqlConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
