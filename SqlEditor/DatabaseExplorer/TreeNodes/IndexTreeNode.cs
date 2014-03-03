using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class IndexTreeNode : TreeNodeBase
    {
        public Index IndexObject { get; set; }

        public IndexTreeNode(Index index, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (index == null) throw new ArgumentNullException("index");

            IndexObject = index;
            Text = IndexObject.DisplayName;
            LeftImages.Add(index.IsUnique ? DatabaseExplorerImageList.Instance.ImageList.Images["node-tree-red.png"] : DatabaseExplorerImageList.Instance.ImageList.Images["node-tree.png"]);
        }

        

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tableColumnsNode = new IndexColumnsTreeNode(IndexObject, DatabaseConnection);
            nodes.Add(tableColumnsNode);

            return nodes;
        }
    }
}