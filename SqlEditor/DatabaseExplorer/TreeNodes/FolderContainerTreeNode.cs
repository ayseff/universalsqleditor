namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class FolderContainerTreeNode : TreeNodeBase
    {
        protected FolderContainerTreeNode(DatabaseConnection databaseConnection, string displayText = "", string imageName = "folder.png")
            : base(displayText, databaseConnection)
        {
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images[imageName]);
        }

        protected FolderContainerTreeNode(string displayText, DatabaseConnection databaseConnection)
            : this(databaseConnection)
        {
            Text = displayText;
        }
    }
}