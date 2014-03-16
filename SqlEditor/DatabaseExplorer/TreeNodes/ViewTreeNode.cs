#region

using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlEditor.Database;
using SqlEditor.Databases;

#endregion

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class ViewTreeNode : TreeNodeBase
    {
        public View View { get; set; }

        public ViewTreeNode(View table, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            View = table;
            Text = table.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["3d_glasses.png"]);
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tableColumnsNode = new ViewColumnsTreeNode(View, DatabaseConnection);
            nodes.Add(tableColumnsNode);

            return nodes;
        }
    }
}