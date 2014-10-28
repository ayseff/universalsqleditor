using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using IBM.Data.DB2;
using Utilities.Text;

namespace SqlEditor.Databases.Db2
{
    public class Db2DatabaseServer : DatabaseServer
    {
        private static readonly Regex _validFullyQualifiedIdentifier = new Regex(@"[a-zA-Z_0-9\.\*\$\#]+[\r\n\f]*$",
                                                                                 RegexOptions.Compiled |
                                                                                 RegexOptions.Multiline);

        private static readonly Regex _validIdentifier = new Regex(@"[a-zA-Z_0-9\$\#]+$",
                                                                   RegexOptions.Compiled | RegexOptions.Multiline);

        private static readonly string[] _numericDataTypes = new[] { "SMALLINT", "INT", "INTEGER", "BIGINT", "NUMERIC", "DECIMAL", "DOUBLE", "REAL", "DOUBLE" };
        private static readonly string[] _dateTimeDataTypes = new[] { "DATE", "TIME", "TIMESTAMP" };

        public override string Name
        {
            get { return "DB2"; }
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
            var db2ConnectionStringBuilder = new DB2ConnectionStringBuilder(connectionString);
            var isolationLevel = (string) db2ConnectionStringBuilder["IsolationLevel"];
            if (isolationLevel.IsNullEmptyOrWhitespace())
            {
                db2ConnectionStringBuilder.IsolationLevel = "ReadUnCommitted";
            }
            db2ConnectionStringBuilder.QueryTimeout = 0;
            db2ConnectionStringBuilder.BrowsableConnectionString = false;
            return db2ConnectionStringBuilder;
        }

        public override object GetSimpleConnectionStringBuilder(DbConnectionStringBuilder connectionStringBuilder)
        {
            return new Db2SimpleConnectionStringBuilder((DB2ConnectionStringBuilder)connectionStringBuilder);
            //var connectionStringBuilder = GetConnectionStringBuilder(connectionStringBuilder);
            //// prepare our property overriding type descriptor
            //var ctd = new PropertyOverridingTypeDescriptor(TypeDescriptor.GetProvider(connectionStringBuilder).GetTypeDescriptor(connectionStringBuilder));

            //// iterate through properties in the supplied object/type
            
            //foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(connectionStringBuilder))
            //{
            //    var isBrowsable = pd.Name == "Database"
            //                       || pd.Name == "Server"
            //                       || pd.Name == "UserID"
            //                       || pd.Name == "Password";
            //    var pd2 = TypeDescriptor.CreateProperty(connectionStringBuilder.GetType(), pd, new BrowsableAttribute(isBrowsable));
            //    ctd.OverrideProperty(pd2);
            //}

            //// then we add new descriptor provider that will return our descriptor instead of default
            //TypeDescriptor.AddProvider(new TypeDescriptorOverridingProvider(ctd), connectionStringBuilder);
            //return connectionStringBuilder;
            //return null;
        }

        public override DbInfoProvider GetInfoProvider()
        {
            return new Db2InfoProvider();
        }

        public override DdlGenerator GetDdlGenerator()
        {
            return new Db2DdlGenerator();
        }

        public override ExplainPlanGenerator GetExplainPlanGenerator()
        {
            return new Db2ExplainPlanGenerator();
        }

        public override IDbConnection CreateConnection(string connectionString)
        {
            var db2Connection = new DB2Connection(connectionString);
            return db2Connection;
        }
    }
}