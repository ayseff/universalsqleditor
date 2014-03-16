using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class FunctionReturnValuesTreeNode : ColumnsTreeNode
    {
        public FunctionReturnValuesTreeNode(Function function, DatabaseConnection databaseConnection)
            : base(function, databaseConnection, "Return")
        {
        }

        protected override void Clear()
        {
        }

        protected override void AddColumns(IList<Column> columns)
        {
        }

        protected override IList<Column> GetColumns(DbInfoProvider infoProvider, IDbConnection connection)
        {
            return infoProvider.GetFunctionReturnValue(connection, (Function)DatabaseObject).Cast<Column>().ToList();
        }
    }
}