using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class PackageProcedureTreeNode : TreeNodeBase
    {
        public PackageProcedure PackageProcedure { get; set; }

        public PackageProcedureTreeNode(PackageProcedure packageProcedure, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (packageProcedure == null) throw new ArgumentNullException("packageProcedure");

            PackageProcedure = packageProcedure;
            Nodes.Clear();
            Text = packageProcedure.DisplayName;
            this.Override.NodeAppearance.Image =
                DatabaseExplorerImageList.Instance.ImageList.Images["table (2) gear.png"];
            IsLoaded = true;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}