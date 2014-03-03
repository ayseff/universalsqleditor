namespace SqlEditor.QueryResults
{
    public class QueryStartedEventArgs
    {
        public string Sql { get; set; }

        public QueryStartedEventArgs()
        {
        }

        public QueryStartedEventArgs(string sql)
        {
            Sql = sql;
        }
    }
}