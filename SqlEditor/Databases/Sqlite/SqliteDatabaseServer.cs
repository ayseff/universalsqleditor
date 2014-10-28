using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using SqlEditor.DatabaseExplorer;

namespace SqlEditor.Databases.Sqlite
{
    public class SqliteDatabaseServer : DatabaseServer
    {
        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*]+\s*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);

        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly string[] _numericDataTypes = new[] { "INTEGER", "REAL" };
        private static readonly string[] _dateTimeDataTypes = new[] { "DATE", "TIME", "TIMESTAMP", "DATETIME" };

        private static readonly SqliteInfoProvider _sqliteInfoProvider = new SqliteInfoProvider();

        public override string Name
        {
            get { return "SQLite"; }
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

        public override bool SupportsMultipleTransactions
        {
            get { return false; }
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
            var sqLiteConnectionStringBuilder = new SQLiteConnectionStringBuilder(connectionString);
            sqLiteConnectionStringBuilder.BrowsableConnectionString = false;
            return sqLiteConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new SqliteSimpleConnectionStringBuilder((SQLiteConnectionStringBuilder) connectionStringBuilder);
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return _sqliteInfoProvider;
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new SqliteDdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new SqliteExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            var connection = new SQLiteConnection(connectionString);
            connection.OpenIfRequired();
            return connection;
        }

        public override string GetUserId(string connectionString)
        {
            return string.Empty;
        }
    }
}