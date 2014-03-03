using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SchemaTreeNode : TreeNodeBase
    {
        public SchemaTreeNode(Schema schema, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            Schema = schema;
            Text = schema.DisplayName;
            LeftImages.Add(
                DatabaseExplorerImageList.Instance.ImageList.Images[
                    DatabaseServerFactory.Instance.GetSchemaImage(DatabaseConnection.DatabaseServer)]);
        }

        public Schema Schema { get; protected set; }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tablesNode = new TablesTreeNode(Schema, DatabaseConnection);
            nodes.Add(tablesNode);
            var viewsNode = new ViewsTreeNode(Schema, DatabaseConnection);
            nodes.Add(viewsNode);
            var indexesNode = new IndexesTreeNode(Schema, DatabaseConnection);
            nodes.Add(indexesNode);
            var storedProcedures = new StoredProceduresTreeNode(Schema, DatabaseConnection);
            nodes.Add(storedProcedures);
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