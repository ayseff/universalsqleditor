using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlEditor.Databases;
using Utilities.Collections;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class StoredProcedureParametersTreeNode : ColumnsTreeNode
    {
        public StoredProcedureParametersTreeNode(StoredProcedure storedProcedure, DatabaseConnection databaseConnection)
            : base(storedProcedure, databaseConnection, "Parameters")
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
            return infoProvider.GetStoredProcedureParameters(connection, (StoredProcedure) DatabaseObject).Cast<Column>().ToList();
        }
    }
}