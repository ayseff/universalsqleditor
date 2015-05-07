using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.PostgreSql
{
    public class PostgreSqlDatabaseInstanceTreeNode : DatabaseInstanceTreeNode
    {
        public override bool OpensWorksheet
        {
            get { return true; }
        }

        public PostgreSqlDatabaseInstanceTreeNode(DatabaseInstance databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseInstance, databaseConnection)
        {
        }


        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var schemasTreeNode = new SchemasTreeNode(DatabaseConnection, DatabaseInstance);
            nodes.Add(schemasTreeNode);
            var loginsTreeNode = new LoginsTreeNode(DatabaseConnection, DatabaseInstance);
            nodes.Add(loginsTreeNode);
            return nodes;
        }
    }
}