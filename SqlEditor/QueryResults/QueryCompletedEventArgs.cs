using System;

namespace SqlEditor.QueryResults
{
    public class QueryCompletedEventArgs 
    {
        public string Sql { get; set; }
        public TimeSpan ElapsedTime { get; set; }

        public QueryCompletedEventArgs()
        {
        }

        public QueryCompletedEventArgs(string sql, TimeSpan elapsedTime)
        {
            Sql = sql;
            ElapsedTime = elapsedTime;
        }
    }
}