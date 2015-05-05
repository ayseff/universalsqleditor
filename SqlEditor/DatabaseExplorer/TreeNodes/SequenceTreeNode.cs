using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class SequenceTreeNode : TreeNodeBase
    {
        public Sequence Sequence { get; set; }

        public SequenceTreeNode(Sequence sequence, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (sequence == null) throw new ArgumentNullException("sequence");

            Sequence = sequence;
            Text = Sequence.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["counter (2).png"]);
            IsLoaded = true;
            Nodes.Clear();
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}