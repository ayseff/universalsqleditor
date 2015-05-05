using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class FunctionParametersTreeNode : ColumnsTreeNode
    {
        public FunctionParametersTreeNode(Function function, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(function, databaseConnection, databaseInstance, "Parameters")
        {
        }

        protected override void Clear()
        {
            DatabaseObject.Columns.Clear();
        }

        protected override void AddColumns(IList<Column> columns)
        {
            DatabaseObject.Columns.AddRange(columns);
        }

        protected override IList<Column> GetColumns(DbInfoProvider infoProvider, IDbConnection connection)
        {
            return infoProvider.GetFunctionParameters(connection, (Function)DatabaseObject).Cast<Column>().ToList();
        }
    }
}