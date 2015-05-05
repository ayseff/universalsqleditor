using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class FolderContainerTreeNode : TreeNodeBase
    {
        protected FolderContainerTreeNode(DatabaseConnection databaseConnection, DatabaseInstance databaseInstance, string displayText = "", string imageName = "folder-horizontal.png", string expandedImageName = "folder-horizontal-open.png")
            : base(displayText, databaseConnection, databaseInstance)
        {
            //LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images[imageName]);
            this.Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images[imageName];
            if (expandedImageName != null)
            {
                this.Override.ExpandedNodeAppearance.Image =
                    DatabaseExplorerImageList.Instance.ImageList.Images[expandedImageName];
            }
        }

        protected FolderContainerTreeNode(string displayText, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : this(databaseConnection, databaseInstance)
        {
            Text = displayText;
        }
    }
}