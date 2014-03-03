using System.Data;

namespace SqlEditor.QueryResults
{
    public class QueryResult
    {
        public DataTable ResultTable { get; set; }
        //public IDataReader DataReader { get; set; }
        //public DataTable DataTable { get; set; }
        public string Sql { get; set; }
        public SqlType SqlType { get; set; }
        public string FirstKeyword { get; set; }
        public int RowsAffected { get; set; }
        public bool HasMoreRows { get; set; }

        public QueryResult()
        {
            HasMoreRows = true;
        }

        public QueryResult(DataTable resultTable, string sql, SqlType sqlType, string firstKeyword)
        {
            ResultTable = resultTable;
            Sql = sql;
            SqlType = sqlType;
            FirstKeyword = firstKeyword;
            HasMoreRows = true;
        }
    }
}
