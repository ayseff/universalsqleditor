using System.Collections.Generic;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class GenericTreeNode : TreeNodeBase
    {
        public GenericTreeNode(DatabaseConnection databaseConnection, string displayText)
            : base(databaseConnection)
        {
            Text = displayText;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}