using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class ConstraintTreeNode : TreeNodeBase
    {
        public Constraint Constraint { get; set; }

        public ConstraintTreeNode(Constraint constraint, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (constraint == null) throw new ArgumentNullException("constraint");

            Constraint = constraint;
            Text = Constraint.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["tables-relation.png"]);
            IsLoaded = true;
            Nodes.Clear();
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}