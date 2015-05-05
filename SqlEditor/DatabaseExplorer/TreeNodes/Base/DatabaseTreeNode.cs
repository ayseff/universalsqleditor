using System;
using JetBrains.Annotations;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Base
{
    public abstract class DatabaseTreeNode : TreeNodeBase
    {
        public string DatabaseName { get; set; }

        protected DatabaseTreeNode(DatabaseConnection connection, [NotNull] string databaseName)
            : base(connection.Name, connection)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (databaseName == null) throw new ArgumentNullException("databaseName");

            var connectionImage = DatabaseServerFactory.Instance.GetDatabaseInstanceImage(connection.DatabaseServer);
            this.Override.NodeAppearance.Image = connectionImage;
            DatabaseName = databaseName;
            this.Text = databaseName;
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                DatabaseName = value;
            }
        }
    }
}
