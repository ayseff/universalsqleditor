using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using SqlEditor.DatabaseExplorer;
using Utilities.Text;
using log4net;

namespace SqlEditor.Databases.Oracle
{
    public class OracleDatabaseServer : DatabaseServer
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*\$\#]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);

        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9\$\#]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);

        protected override string UserIdToken { get { return "USER ID"; } }
        protected override string PasswordToken { get { return "PASSWORD"; } }

        private static readonly string[] _numericDataTypes = { "NUMBER", "BINARY_FLOAT", "BINARY_DOUBLE" };
        private static readonly string[] _dateTimeDataTypes = { "DATE", "TIME", "TIMESTAMP", "DATETIME" };


        public override string[] NumericDataTypes
        {
            get { return _numericDataTypes; }
        }

        public override string[] DateTimeDataTypes
        {
            get { return _dateTimeDataTypes; }
        }

        public override string Name
        {
            get { return "Oracle"; }
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

        public override DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null)
        {
            var oracleConnectionStringBuilder = new OracleConnectionStringBuilder();
            if (!connectionString.IsNullEmptyOrWhitespace())
            {
                oracleConnectionStringBuilder.ConnectionString = connectionString;
            }
            oracleConnectionStringBuilder.BrowsableConnectionString = false;
            return oracleConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new OracleSimpleConnectionStringBuilder((OracleConnectionStringBuilder) connectionStringBuilder);
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return new OracleInfoProvider();
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new OracleDdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new OracleExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            var oracleConnection = new OracleConnection(connectionString);
            oracleConnection.OpenIfRequired();
            return oracleConnection;
        }
    }
}