using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using log4net;

namespace SqlEditor.Databases.MySql
{
    public class MySqlDatabaseServer : DatabaseServer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string[] _numericDataTypes = new[] { "BIGINT", "MEDIUMINT", "INT", "INTEGER", "SMALLINT", "TINYINT", "DEC", "DECIMAL", "FLOAT", "DOUBLE", "BIT" };
        private static readonly string[] _dateTimeDataTypes = new[] { "DATE", "TIME", "TIMESTAMP", "DATETIME" };

        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*\$\#]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);

        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9\$\#]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);


        protected override string UserIdToken { get { return "Uid"; } }
        protected override string PasswordToken { get { return "Pwd"; } }

        public override string[] NumericDataTypes
        {
            get { return _numericDataTypes; }
        }

        public override string[] DateTimeDataTypes
        {
            get { return _dateTimeDataTypes; }
        }

        public override Regex ValidIdentifierRegex
        {
            get { return _validIdentifier; }
        }

        public override Regex ValidFullyQualifiedIdentifierRegex
        {
            get { return _validFullyQualifiedIdentifier; }
        }

        public override string Name
        {
            get { return "MySQL"; }
            protected set { base.Name = value; }
        }

        public override DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null)
        {
            var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder(connectionString);
            mySqlConnectionStringBuilder.BrowsableConnectionString = false;
            if (connectionString == null || connectionString.IndexOf("Pooling", StringComparison.InvariantCultureIgnoreCase) < 0)
            {
                mySqlConnectionStringBuilder.Pooling = false;
            }
            return mySqlConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new MySqlSimpleConnectionStringBuilder((MySqlConnectionStringBuilder) connectionStringBuilder);
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return new MySqlInfoProvider();
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new MySqlDdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new MySqlExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public override void ValidateConnectionString(DbConnectionStringBuilder connectionBuilder)
        {
            var connectionStringBuilder = connectionBuilder as MySqlConnectionStringBuilder;
            if (connectionStringBuilder == null)
                throw new Exception("ConnectionState string builder is not of type  MySqlConnectionStringBuilder");
            if (string.IsNullOrEmpty(connectionStringBuilder.Database))
                throw new Exception("Database is required for this connection type");
        }
    }
}