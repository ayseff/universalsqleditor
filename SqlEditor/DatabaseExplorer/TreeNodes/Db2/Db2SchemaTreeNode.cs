using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Db2
{
    public class Db2SchemaTreeNode : SchemaTreeNode
    {
        public Db2SchemaTreeNode(Schema schema, DatabaseConnection databaseConnection)
            : base(schema, databaseConnection)
        { }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tablesNode = new TablesTreeNode(Schema, DatabaseConnection);
            nodes.Add(tablesNode);
            var viewsNode = new ViewsTreeNode(Schema, DatabaseConnection);
            nodes.Add(viewsNode);
            var indexesNode = new IndexesTreeNode(Schema, DatabaseConnection);
            nodes.Add(indexesNode);
            var constraintsNode = new ConstraintsTreeNode(Schema, DatabaseConnection);
            nodes.Add(constraintsNode);
            var storedProcedures = new StoredProceduresTreeNode(Schema, DatabaseConnection);
            nodes.Add(storedProcedures);
            var functions = new FunctionsTreeNode(Schema, DatabaseConnection);
            nodes.Add(functions);
            var sequencesNode = new SequencesTreeNode(Schema, DatabaseConnection);
            nodes.Add(sequencesNode);
            var synonymsNode = new SynonymsTreeNode(Schema, DatabaseConnection);
            nodes.Add(synonymsNode);
            var publicSynonymsNode = new PublicSynonymsTreeNode(Schema, DatabaseConnection);
            nodes.Add(publicSynonymsNode);
            var triggerssNode = new TriggersTreeNode(Schema, DatabaseConnection);
            nodes.Add(triggerssNode);
            return nodes;
        }
    }
}