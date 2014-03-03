using System.Data;

namespace SqlEditor
{
    public class SqlQueryResult
    {
        public DataTable Result { get; set; }
        public bool HasMoreRows { get; set; }
        public int RowsAffected { get; set; }
        public IDbTransaction Transaction { get; set; }

        public SqlQueryResult(DataTable result, bool hasMoreRows)
        {
            Result = result;
            HasMoreRows = hasMoreRows;
        }

        public SqlQueryResult(int rowsAffected)
        {
            RowsAffected = rowsAffected;
        }
    }
}