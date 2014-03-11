using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class SchemaTreeNode : TreeNodeBase
    {
        public SchemaTreeNode(Schema databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (databaseInstance == null) throw new ArgumentNullException("databaseInstance");
            DatabaseInstance = databaseInstance;
            Text = databaseInstance.DisplayName;
            //LeftImages.Add(
            //    DatabaseExplorerImageList.Instance.ImageList.Images[
            //        DatabaseServerFactory.Instance.GetSchemaImage(DatabaseConnection.DatabaseServer)]);
            this.Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images[
                DatabaseServerFactory.Instance.GetSchemaImage(DatabaseConnection.DatabaseServer)];
        }

        public Schema DatabaseInstance { get; protected set; }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tablesNode = new TablesTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(tablesNode);
            var viewsNode = new ViewsTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(viewsNode);
            var indexesNode = new IndexesTreeNode(DatabaseInstance, DatabaseConnection);
            nodes.Add(indexesNode);
            var storedProcedures = new StoredProceduresTreeNode(DatabaseInstance, DatabaseConnection, "Stored Procedures");
            nodes.Add(storedProcedures);
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

    public abstract class DatabaseInstanceTreeNode : TreeNodeBase
    {
        public DatabaseInstanceTreeNode(DatabaseInstance databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (databaseInstance == null) throw new ArgumentNullException("databaseInstance");
            DatabaseInstance = databaseInstance;
            Text = databaseInstance.DisplayName;
            LeftImages.Add(
                DatabaseExplorerImageList.Instance.ImageList.Images[
                    DatabaseServerFactory.Instance.GetDatabaseInstanceImage(DatabaseConnection.DatabaseServer)]);
        }

        public DatabaseInstance DatabaseInstance { get; protected set; }
    }
}