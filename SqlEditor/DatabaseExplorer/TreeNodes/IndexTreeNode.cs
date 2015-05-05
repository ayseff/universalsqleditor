using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class IndexTreeNode : TreeNodeBase
    {
        public Index IndexObject { get; set; }

        public IndexTreeNode(Index index, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (index == null) throw new ArgumentNullException("index");

            IndexObject = index;
            Text = IndexObject.DisplayName;
            this.Override.NodeAppearance.Image = index.IsUnique
                ? DatabaseExplorerImageList.Instance.ImageList.Images["node-tree-red.png"]
                : DatabaseExplorerImageList.Instance.ImageList.Images["node-tree.png"];
        }

        

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var columnsTreeNode = new IndexColumnsTreeNode(IndexObject, DatabaseConnection, DatabaseInstance);
            nodes.Add(columnsTreeNode);
            var includedColumnsTreeNode = new IndexIncludedColumnsTreeNode(IndexObject, DatabaseConnection, DatabaseInstance);
            nodes.Add(includedColumnsTreeNode);

            return nodes;
        }
    }
}