using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class DatabaseInstanceTreeNode : TreeNodeBase
    {
        public override bool OpensWorksheet
        {
            get { return true; }
        }

        public DatabaseInstanceTreeNode(DatabaseInstance databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseConnection, databaseInstance)
        {
            if (databaseInstance == null) throw new ArgumentNullException("databaseInstance");
            DatabaseInstance = databaseInstance;
            Text = databaseInstance.DisplayName;
            Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images["database_yellow.png"];
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var schemasTreeNode = new SchemasTreeNode(DatabaseConnection, DatabaseInstance);
            nodes.Add(schemasTreeNode);
            return nodes;
        }
    }
}