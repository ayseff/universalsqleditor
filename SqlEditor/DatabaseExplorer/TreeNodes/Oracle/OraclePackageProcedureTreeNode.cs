using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.Oracle
{
    public sealed class OraclePackageProcedureTreeNode : TreeNodeBase
    {
        public PackageProcedure PackageProcedure { get; set; }

        public OraclePackageProcedureTreeNode(PackageProcedure packageProcedure, DatabaseConnection databaseConnection)
            : base(databaseConnection)
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