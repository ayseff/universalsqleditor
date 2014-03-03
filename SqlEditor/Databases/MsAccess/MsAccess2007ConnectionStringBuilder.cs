namespace SqlEditor.Database.MsAccess
{
    public class MsAccess2007ConnectionStringBuilder : MsAccess2003ConnectionStringBuilder
    {
        private const string MICROSOFT_ACE_OLEDB = "Microsoft.ACE.OLEDB.12.0";

        public MsAccess2007ConnectionStringBuilder(string connectionString)
            : base(connectionString)
        {
            _provider = MICROSOFT_ACE_OLEDB;
            Provider = _provider;
        }

        public MsAccess2007ConnectionStringBuilder()
        {
            _provider = MICROSOFT_ACE_OLEDB;
            Provider = _provider;
        }
    }
}