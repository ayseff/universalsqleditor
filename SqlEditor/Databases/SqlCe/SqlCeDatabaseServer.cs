using System.Data;
using System.Data.Common;
using System.Data.SqlServerCe;
using System.Text.RegularExpressions;

namespace SqlEditor.Databases.SqlCe
{
    public class SqlCeDatabaseServer : DatabaseServer
    {
        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);
        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);
        private static readonly string[] _numericDataTypes = new[] { "BIGINT", "INT", "SMALLINT", "TINYINT", "NUMERIC", "DECIMAL", "FLOAT", "REAL", "BIT", "MONEY", "SMALLMONEY" };
        private static readonly string[] _dateTimeDataTypes = new[] { "DATE", "TIME", "TIMESTAMP", "DATETIME", "DATETIME2", "SMALLDATETIME", "DATETIMEOFFSET" };

        private static readonly SqlCeInfoProvider _sqlCeInfoProvider = new SqlCeInfoProvider();

        public override string[] SqlTerminators { get { return new[] { ";", "GO", "Go", "go", "gO" }; } }

        public override string HighlightingDefinitionsFile { get { return "SQLCESQL"; } }

        public override string Name
        {
            get { return "SQL Server Compact"; }
            protected set { base.Name = value; }
        }

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
            var sqlCeConnectionStringBuilder = new SqlCeConnectionStringBuilder(connectionString);
            sqlCeConnectionStringBuilder.BrowsableConnectionString = false;
            return sqlCeConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new SqlCeSimpleConnectionStringBuilder((SqlCeConnectionStringBuilder) connectionStringBuilder);
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return _sqlCeInfoProvider;
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new SqlCeDdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new SqlCeExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            var sqlCeConnection = new SqlCeConnection(connectionString);
            return sqlCeConnection;
        }

        public override string GetUserId(string connectionString)
        {
            return string.Empty;
        }
    }
}