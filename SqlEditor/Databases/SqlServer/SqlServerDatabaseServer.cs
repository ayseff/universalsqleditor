using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace SqlEditor.Databases.SqlServer
{
    public class SqlServerDatabaseServer : DatabaseServer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SqlServerInfoProvider _sqlServerInfoProvider = new SqlServerInfoProvider();
        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*\$\#]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);

        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9\$\#]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);
        public override string[] SqlTerminators { get { return new[] { ";", "GO", "Go", "go", "gO" }; } }

        private static readonly string[] _numericDataTypes = new[] { "BIGINT", "INT", "SMALLINT", "TINYINT", "NUMERIC", "DECIMAL", "FLOAT", "REAL", "BIT", "MONEY", "SMALLMONEY" };
        private static readonly string[] _dateTimeDataTypes = new[] { "DATE", "TIME", "TIMESTAMP", "DATETIME", "DATETIME2", "SMALLDATETIME", "DATETIMEOFFSET" };

        public override string Name
        {
            get { return "SQL Server"; }
            protected set { base.Name = value; }
        }

        public override string HighlightingDefinitionsFile { get { return "SQLSERVERSQL"; } }

        public override Regex ValidIdentifierRegex
        {
            get { return _validIdentifier; }
        }

        public override Regex ValidFullyQualifiedIdentifierRegex
        {
            get { return _validFullyQualifiedIdentifier; }
        }

        public override string[] NumericDataTypes
        {
            get { return _numericDataTypes; }
        }

        public override string[] DateTimeDataTypes
        {
            get { return _dateTimeDataTypes; }
        }

        public override DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString)
                                                 {MultipleActiveResultSets = true};
            sqlConnectionStringBuilder.BrowsableConnectionString = false;
            return sqlConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new SqlServerSimpleConnectionStringBuilder((SqlConnectionStringBuilder)connectionStringBuilder);
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return _sqlServerInfoProvider;
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new SqlServerDdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new SqlServerExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            _log.Debug("Creating connection ...");
            var connection = new SqlConnection(connectionString);
            return connection;
        }

        public override void ValidateConnectionString(DbConnectionStringBuilder connectionBuilder)
        {
            var connectionStringBuilder = connectionBuilder as SqlConnectionStringBuilder;
            if (connectionStringBuilder == null)
                throw new Exception("ConnectionState string builder is not of type  SqlConnectionStringBuilder");
            if (string.IsNullOrEmpty(connectionStringBuilder.InitialCatalog))
                throw new Exception("InitialCatalog is required for this connection type");
        }
    }
}