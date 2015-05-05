using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class ColumnTreeNode : TreeNodeBase
    {
        public Column Column { get; set; }

        public ColumnTreeNode(Column column, DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance)
        {
            if (column == null) throw new ArgumentNullException("column");

            Column = column;
            Nodes.Clear();
            Text = column.DisplayName;
            //LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["column_single.png"]);
            this.Override.NodeAppearance.Image =
                DatabaseExplorerImageList.Instance.ImageList.Images["column_single.png"];
            IsLoaded = true;
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            return new List<TreeNodeBase>();
        }

        public override string GetClipboardText()
        {
            return Column.Name;
        }
    }
}