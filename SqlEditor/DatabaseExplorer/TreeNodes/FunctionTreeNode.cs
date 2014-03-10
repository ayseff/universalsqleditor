﻿using System;
using System.Collections.Generic;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class FunctionTreeNode : TreeNodeBase
    {
        public Function Function { get; set; }

        public FunctionTreeNode(Function function, DatabaseConnection databaseConnection)
            : base(databaseConnection)
        {
            if (function == null) throw new ArgumentNullException("function");

            this.Function = function;
            Text = Function.DisplayName;
            LeftImages.Add(DatabaseExplorerImageList.Instance.ImageList.Images["table (2) gear_green.png"]);
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            var nodes = new List<TreeNodeBase>();
            var functionParametersTreeNode = new FunctionParametersTreeNode(Function, DatabaseConnection);
            nodes.Add(functionParametersTreeNode);
            var functionReturnTreeNode = new FunctionReturnValuesTreeNode(Function, DatabaseConnection);
            nodes.Add(functionReturnTreeNode);
            return nodes;
        }
    }
}