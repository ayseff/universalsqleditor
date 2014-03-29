using System;
using System.ComponentModel;
using IBM.Data.DB2;
using JetBrains.Annotations;

namespace SqlEditor.Databases.Db2
{
    public class Db2SimpleConnectionStringBuilder
    {
        private readonly DB2ConnectionStringBuilder _connectionStringBuilder;

        [Description("Server:Port of the DB2 database server (i.e. MyServer:5800)")]
        public string Server 
        {
            get { return _connectionStringBuilder.Server; }
            set { _connectionStringBuilder.Server = value; }
        }

        [Description("Database name you wish to connect to")]
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

        [Description("Password used to connect to the database")]
        public string Password
        {
            get { return _connectionStringBuilder.Password; }
            set { _connectionStringBuilder.Password = value; }
        }

        public Db2SimpleConnectionStringBuilder([NotNull] DB2ConnectionStringBuilder connectionStringBuilder)
        {
            if (connectionStringBuilder == null) throw new ArgumentNullException("connectionStringBuilder");
            _connectionStringBuilder = connectionStringBuilder;
        }
    }
}
