using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class StoredProcedureTreeNode : TreeNodeBase
    {
        public StoredProcedure StoredProcedure { get; set; }

        public StoredProcedureTreeNode(StoredProcedure storedProcedure, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (storedProcedure == null) throw new ArgumentNullException("storedProcedure");

            this.StoredProcedure = storedProcedure;
            Text = StoredProcedure.DisplayName;
            //LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["table (2) gear.png"]);
            this.Override.NodeAppearance.Image =
                DatabaseExplorerImageList.Instance.ImageList.Images["table (2) gear.png"];
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var storedProcedureParametersTreeNode = new StoredProcedureParametersTreeNode(StoredProcedure, DatabaseConnection, DatabaseInstance);
            nodes.Add(storedProcedureParametersTreeNode);
            return nodes;
        }
    }
}