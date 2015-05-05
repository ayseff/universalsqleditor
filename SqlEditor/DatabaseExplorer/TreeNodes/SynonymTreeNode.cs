using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SynonymTreeNode : TreeNodeBase
    {
        public Synonym Synonym { get; set; }

        public SynonymTreeNode(Synonym synonym, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (synonym == null) throw new ArgumentNullException("synonym");

            Synonym = synonym;
            Text = synonym.DisplayName;
            this.Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images["edit-replace.png"];
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var targetObjectNode = new GenericTreeNode(DatabaseConnection, DatabaseInstance, Synonym.TargetObjectName);
            return new List<TreeNodeBase> { targetObjectNode };
        }
    }
}