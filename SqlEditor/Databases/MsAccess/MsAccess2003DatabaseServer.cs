using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using SqlEditor.Database.MsAccess;

namespace SqlEditor.Databases.MsAccess
{
    public class MsAccess2003DatabaseServer : DatabaseServer
    {
        private const string DEFAULT_SCHEMA = "MAIN";
        public static readonly Schema DefaultSchema = new Schema(string.Empty, DEFAULT_SCHEMA);
        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);
        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly string[] _numericDataTypes = new[] { "BIGINT", "INT", "SMALLINT", "TINYINT", "NUMERIC", "DECIMAL", "FLOAT", "REAL", "BIT", "MONEY", "SMALLMONEY" };
        private static readonly string[] _dateTimeDataTypes = new[] { "DATE", "TIME", "TIMESTAMP", "DATETIME", "DATETIME2", "SMALLDATETIME", "DATETIMEOFFSET" };

        public override string HighlightingDefinitionsFile { get { return "MSACCESSSQL"; } }


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
            get { return "Microsoft Access 2003"; }
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
            var msAccess2003ConnectionStringBuilder = new MsAccess2003ConnectionStringBuilder(connectionString);
            msAccess2003ConnectionStringBuilder.BrowsableConnectionString = false;
            return msAccess2003ConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return connectionStringBuilder;
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return new MsAccessInfoProvider();
        }

        public override DdlGenerator GetDdlGenerator()
        {
            throw new System.NotSupportedException("MS Access database does not support DDL generation");
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            throw new System.NotSupportedException("MS Access database does not support execution plans");
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            var connection = new OleDbConnection(connectionString);
            connection.Open();
            return connection;
        }

        public override string GetUserId(string connectionString)
        {
            return DEFAULT_SCHEMA;
        }
    }
}