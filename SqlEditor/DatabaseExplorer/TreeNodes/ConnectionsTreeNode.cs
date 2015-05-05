using System.Collections.Generic;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class ConnectionsTreeNode : TreeNodeBase
    {
        private static ConnectionsTreeNode _instance;

        public ConnectionsTreeNode()
            : base("Connections", null, null)
        {
            //LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["database_server.png"]);
            this.Override.NodeAppearance.Image =
                DatabaseExplorerImageList.Instance.ImageList.Images["database_server.png"];
        }

        public static ConnectionsTreeNode Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConnectionsTreeNode();
                }
                return _instance;
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
    }
}