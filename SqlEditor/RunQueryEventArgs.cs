using System;

namespace SqlEditor
{
    public class RunQueryEventArgs : EventArgs
    {
        public string Sql { get; set; }
        public string ConnectionName { get; set; }
        public int MaxResults { get; set; }

        public RunQueryEventArgs(string sql, string connectionName, int maxResults)
        {
            if (sql == null) throw new ArgumentNullException("sql");
            if (connectionName == null) throw new ArgumentNullException("connectionName");
            if (maxResults < 0) throw new ArgumentException("Max results cannot be less than zero.", "maxResults");

            Sql = sql;
            ConnectionName = connectionName;
            MaxResults = maxResults;
        }
    }
}