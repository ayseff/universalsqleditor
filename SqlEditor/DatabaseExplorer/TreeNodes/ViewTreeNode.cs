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

            //_log.DebugFormat("Loading columns for view {0} ...", View.FullyQualifiedName);
            //View.Clear();
            //IList<Column> columns;
            //using (var connection = DatabaseConnection.CreateNewConnection())
            //{
            //    connection.OpenIfRequired();
            //    var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
            //    columns = infoProvider.GetViewColumns(connection, View.Parent.Name, View.Name);
            //}
            //_log.DebugFormat("Loaded {0} column(s).", columns.Count);


            //var columnNodes =
            //    columns.Select(x => new ColumnTreeNode(x, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
            //return columnNodes;
        }
    }
}