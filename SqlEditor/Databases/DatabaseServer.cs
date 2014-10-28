using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace SqlEditor.Databases
{
    public abstract class DatabaseServer
    {
        public virtual string Name { get; protected set; }
        public virtual bool SupportsMultipleTransactions { get { return true; }}
        public abstract Regex ValidIdentifierRegex { get; }
        public abstract Regex ValidFullyQualifiedIdentifierRegex { get; }
        public virtual Regex LineCommentRegex { get { return new Regex(@"^\s*--.*?[\r\n]+", RegexOptions.Multiline | RegexOptions.Compiled); }}
        public virtual Regex InlineCommentRegex { get { return new Regex(@"--.*?((?=[\r\n]+)|$)", RegexOptions.Multiline | RegexOptions.Compiled); } }
        public virtual Regex BlockCommentRegex { get { return new Regex(@"/\*(?>(?:(?>[^*]+)|\*(?!/))*)\*/", RegexOptions.Multiline | RegexOptions.Compiled); }}
        public virtual string[] SqlTerminators { get { return new[] {";"}; }}
        public virtual string HighlightingDefinitionsFile { get { return Name.ToUpper() + "SQL"; } }
        protected virtual string UserIdToken { get { return "User ID";  } }
        protected virtual string PasswordToken { get { return "Password"; } }
        public abstract string[] NumericDataTypes { get; }
        public virtual string[] DateTimeDataTypes { get { return new[] {"DATE", "TIME", "TIMESTAMP"}; } }
        

        public abstract DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null);

        public abstract object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder);
        public abstract DbInfoProvider GetInfoProvider();
        public abstract DdlGenerator GetDdlGenerator();

        public abstract ExplainPlanGenerator GetExplainPlanGenerator();
        public abstract IDbConnection CreateConnection(string connectionString);

        public virtual string GetUserId(string connectionString)
        {
            var value = GetConnectionStringValue(connectionString, UserIdToken);
            return value == null ? null : value.Trim().ToUpper();
        }
        
        public virtual string GetPassword(string connectionString)
        {
            return GetConnectionStringValue(connectionString, PasswordToken);
        }

        /// <summary>
        /// Sets password on the connection string.
        /// </summary>
        /// <param name="connectionString">Connection string on which to set the password.</param>
        /// <param name="password">Password to set.</param>
        /// <returns>New connection string with password set.</returns>
        public virtual string SetPassword(string connectionString, string password)
        {
            if (connectionString == null) throw new ArgumentNullException("connectionString");
            var sb = GetConnectionStringBuilder(connectionString);
            sb[PasswordToken] = password;
            return sb.ConnectionString;
        }

        public virtual void ValidateConnectionString(DbConnectionStringBuilder connectionBuilder)
        {
            
        }

        protected string GetConnectionStringValue(string connectionString, string token)
        {
            var dbConnectionStringBuilder = GetConnectionStringBuilder(connectionString);
            if (!dbConnectionStringBuilder.ContainsKey(token)) return null;
            var value = dbConnectionStringBuilder[token];
            return value == null ? null : ((string) value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}