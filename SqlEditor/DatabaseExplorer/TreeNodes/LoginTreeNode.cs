using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public class LoginTreeNode : TreeNodeBase
    {
        public Login Login { get; protected set; }

        public LoginTreeNode(Login login, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (login == null) throw new ArgumentNullException("login");
            this.Login = login;
            Text = login.DisplayName;
            this.Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images[
                DatabaseServerFactory.Instance.GetSchemaImage(DatabaseConnection.DatabaseServer)];
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}