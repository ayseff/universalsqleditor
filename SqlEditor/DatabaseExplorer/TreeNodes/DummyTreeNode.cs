using Infragistics.Win.UltraWinTree;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class DummyTreeNode : UltraTreeNode
    {
        public DummyTreeNode()
        {
            Text = "Loading ...";
        }
    }
}