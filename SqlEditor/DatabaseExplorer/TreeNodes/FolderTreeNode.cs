using System.Collections.Generic;
using Infragistics.Win.UltraWinTree;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class FolderTreeNode : FolderContainerTreeNode
    {
        public FolderTreeNode(string name)
            : base(name, null, null)
        {
            Nodes.Clear();
            IsLoaded = true;
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                UpdateChildren(this);
            }
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return null;
        }

        public override void ReloadAsync()
        {
            // Do nothing
        }

        private static void UpdateChildren(UltraTreeNode node)
        {
            foreach (var childNode in node.Nodes)
            {
                var connectionTreeNode = childNode as ConnectionTreeNode;
                if (connectionTreeNode != null)
                {
                    connectionTreeNode.DatabaseConnection.Folder = node.FullPath;
                }
                else if (childNode is FolderTreeNode)
                {
                    UpdateChildren(childNode as FolderTreeNode);
                }
            }
        }
    }
}