using System;
using System.ComponentModel;
using System.Data.SQLite;
using JetBrains.Annotations;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteSimpleConnectionStringBuilder
    {
        private readonly SQLiteConnectionStringBuilder _connectionStringBuilder;

        [DisplayName("Data Source")]
        [Description("SQLite database file to open")]
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

        public SqliteSimpleConnectionStringBuilder([NotNull] SQLiteConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
