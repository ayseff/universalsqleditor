using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infragistics.Win.UltraWinTree;

namespace SqlEditor.DatabaseExplorer
{
    public class DbExplorerTreeNodeBase : UltraTreeNode
    {
        public bool IsLoaded { get; protected set; }


    }
}
