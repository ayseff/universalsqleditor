using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class SchemaTreeNode : TreeNodeBase
    {
        public Schema Schema { get; protected set; }

        public SchemaTreeNode(Schema schema, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
            Text = schema.DisplayName;
            this.Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images[
                DatabaseServerFactory.Instance.GetSchemaImage(DatabaseConnection.DatabaseServer)];
        }

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