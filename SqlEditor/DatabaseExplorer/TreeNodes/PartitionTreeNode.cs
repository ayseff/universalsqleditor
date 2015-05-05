using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class PartitionTreeNode : TreeNodeBase
    {
        public Partition Partition { get; set; }

        public PartitionTreeNode(Partition partition, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (partition == null) throw new ArgumentNullException("partition");

            Partition = partition;
            Nodes.Clear();
            Text = partition.DisplayName;
            //LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["data_table.png"]);
            this.Override.NodeAppearance.Image = DatabaseExplorerImageList.Instance.ImageList.Images["data_table.png"];
            IsLoaded = true;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}