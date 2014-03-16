using System;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class DatabaseInstanceTreeNode : TreeNodeBase
    {
        public DatabaseInstance DatabaseInstance { get; protected set; }

        public DatabaseInstanceTreeNode(DatabaseInstance databaseInstance, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (databaseInstance == null) throw new ArgumentNullException("databaseInstance");
            DatabaseInstance = databaseInstance;
            Text = databaseInstance.DisplayName;
            Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images["database_yellow.png"];
        }
    }
}