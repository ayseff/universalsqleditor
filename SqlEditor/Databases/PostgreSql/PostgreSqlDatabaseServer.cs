using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Npgsql;

namespace SqlEditor.Databases.PostgreSql
{
    public class PostgreSqlDatabaseServer : DatabaseServer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private static readonly string[] _numericDataTypes = { "BIGINT", "INTEGER", "SMALLINT", "DECIMAL", "NUMERIC", "REAL", "DOUBLE", "SERIAL", "BIGSERIAL" };
        private static readonly string[] _dateTimeDataTypes = { "DATE", "TIME", "TIMESTAMP", "INTERVAL" };

        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\$]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);

        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9\$]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);


        protected override string UserIdToken { get { return "User Id"; } }
        protected override string PasswordToken { get { return "Password"; } }

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
            get { return "PostgreSQL"; }
            protected set { base.Name = value; }
        }

        public override DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null)
        {
            var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            npgsqlConnectionStringBuilder.BrowsableConnectionString = false;
            return npgsqlConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new PostgreSqlSimpleConnectionStringBuilder((NpgsqlConnectionStringBuilder) connectionStringBuilder);
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return new PostgreSqlInfoProvider();
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new PostgreSqlDdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new PostgreSqlExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        public override void ValidateConnectionString(DbConnectionStringBuilder connectionBuilder)
        {
            var connectionStringBuilder = connectionBuilder as NpgsqlConnectionStringBuilder;
            if (connectionStringBuilder == null)
                throw new Exception("ConnectionState string builder is not of type  NpgsqlConnectionStringBuilder");
            if (string.IsNullOrEmpty(connectionStringBuilder.Database))
                throw new Exception("Database is required for this connection type");
        }
    }
}