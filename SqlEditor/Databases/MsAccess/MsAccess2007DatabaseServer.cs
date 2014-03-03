using System.Data.Common;
using SqlEditor.Database.MsAccess;

namespace SqlEditor.Databases.MsAccess
{
    public class MsAccess2007DatabaseServer : MsAccess2003DatabaseServer
    {
        public override string Name
        {
            get { return "Microsoft Access 2007"; }
            protected set { base.Name = value; }
        }

        public override DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null)
        {
            var msAccess2007ConnectionStringBuilder = new MsAccess2007ConnectionStringBuilder(connectionString);
            msAccess2007ConnectionStringBuilder.BrowsableConnectionString = false;
            return msAccess2007ConnectionStringBuilder;
        }
    }
}