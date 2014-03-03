using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SynonymTreeNode : TreeNodeBase
    {
        public Synonym Synonym { get; set; }

        public SynonymTreeNode(Synonym synonym, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (synonym == null) throw new ArgumentNullException("synonym");

            Synonym = synonym;
            Text = synonym.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["edit-replace.png"]);
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}