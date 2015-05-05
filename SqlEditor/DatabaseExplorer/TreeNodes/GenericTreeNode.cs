using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class GenericTreeNode : TreeNodeBase
    {
        public GenericTreeNode(DatabaseConnection databaseConnection, DatabaseInstance databaseInstance, string displayText)
            : base(databaseConnection, databaseInstance)
        {
            Text = displayText;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}