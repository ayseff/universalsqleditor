using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Oracle
{
    public class OracleSchemaTreeNode : SchemaTreeNode
    {
        public OracleSchemaTreeNode(Schema databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseInstance, databaseConnection)
        { }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tablesNode = new TablesTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(tablesNode);
            var viewsNode = new ViewsTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(viewsNode);
            var indexesNode = new IndexesTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(indexesNode);
            var storedProcedures = new StoredProceduresTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(storedProcedures);
            var functions = new FunctionsTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(functions);
            var sequencesNode = new SequencesTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(sequencesNode);
            var synonymsNode = new SynonymsTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(synonymsNode);
            var publicSynonymsNode = new PublicSynonymsTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(publicSynonymsNode);
            var triggerssNode = new TriggersTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(triggerssNode);
            return nodes;
        }
    }
}