﻿using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class TableTreeNode : TreeNodeBase
    {
        public Table Table { get; set; }

        public TableTreeNode(Table table, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (table == null) throw new ArgumentNullException("table");

            Table = table;
            Text = Table.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["table (2).png"]);
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var tableColumnsNode = new TableColumnsTreeNode(Table, DatabaseConnection);
            nodes.Add(tableColumnsNode);
            var tablePrimaryKeyColumnsNode = new TablePrimaryKeyColumnsTreeNode(Table, DatabaseConnection);
            nodes.Add(tablePrimaryKeyColumnsNode);
            var indexesNode = new TableIndexesTreeNode(Table, DatabaseConnection);
            nodes.Add(indexesNode);
            var constraintsNode = new TableConstraintsTreeNode(Table, DatabaseConnection);
            nodes.Add(constraintsNode);
            var partitionsNode = new TablePartitionsTreeNode(Table, DatabaseConnection);
            nodes.Add(partitionsNode);
            return nodes;
        }
    }
}