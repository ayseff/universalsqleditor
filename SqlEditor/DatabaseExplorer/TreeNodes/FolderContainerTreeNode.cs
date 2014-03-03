namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class FolderContainerTreeNode : TreeNodeBase
    {
        protected FolderContainerTreeNode(DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["folder.png"]);
        }

        protected FolderContainerTreeNode(string text, DatabaseConnection databaseConnection)
            : this(databaseConnection)
        {
            Text = text;
        }
    }
}