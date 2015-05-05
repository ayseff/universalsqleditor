using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Oracle
{
    public class OracleSchemaTreeNode : SchemaTreeNode
    {
        public OracleSchemaTreeNode(Schema schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(schema, databaseConnection, databaseInstance)
        { }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tablesNode = new TablesTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(tablesNode);
            var viewsNode = new ViewsTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(viewsNode);
            var indexesNode = new IndexesTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(indexesNode);
            var constraintsNode = new ConstraintsTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(constraintsNode);
            var storedProcedures = new StoredProceduresTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(storedProcedures);
            var functions = new FunctionsTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(functions);
            var packages = new OraclePackagesTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(packages);
            var sequencesNode = new SequencesTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(sequencesNode);
            var synonymsNode = new SynonymsTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(synonymsNode);
            var publicSynonymsNode = new PublicSynonymsTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(publicSynonymsNode);
            var triggerssNode = new TriggersTreeNode(Schema, DatabaseConnection, DatabaseInstance);
            nodes.Add(triggerssNode);
            return nodes;
        }
    }
}