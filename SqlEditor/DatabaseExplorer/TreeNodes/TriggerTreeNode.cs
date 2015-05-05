using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TriggerTreeNode : TreeNodeBase
    {
        public Trigger Trigger { get; set; }

        public TriggerTreeNode(Trigger trigger, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (trigger == null) throw new ArgumentNullException("trigger");

            Trigger = trigger;
            Text = trigger.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["lightning.png"]);
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }
    }
}