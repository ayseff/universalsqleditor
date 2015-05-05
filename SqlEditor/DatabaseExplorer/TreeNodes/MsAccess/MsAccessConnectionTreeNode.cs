using System;
using System.Collections.Generic;
using System.Linq;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes.MsAccess
{
    public sealed class MsAccessConnectionTreeNode : ConnectionTreeNode
    {
        public MsAccessConnectionTreeNode(DatabaseConnection connection)
            : base(connection)
        { }

        protected override IList<TreeNodeBase> GetNodes()
        {
            try
            {
                var actionName = string.Format("Attempting to connect to {0} ...", DatabaseConnection.Name);
                _log.Debug(actionName);
                using (new WaitActionStatus(actionName))
                {
                    _log.DebugFormat("Creating connection for {0} ...", DatabaseConnection.Name);
                    using (var connection = DatabaseConnection.CreateNewConnection())
                    {
                        connection.OpenIfRequired();
                        DatabaseConnection.Connect();
                        _log.Debug("Connection is successful.");

                        var nodes = new List<TreeNodeBase>();
                        var dummySchema = new Schema();
                        var tablesNode = new TablesTreeNode(dummySchema, DatabaseConnection, DatabaseInstance);
                        nodes.Add(tablesNode);
                        var viewsNode = new ViewsTreeNode(dummySchema, DatabaseConnection, DatabaseInstance);
                        nodes.Add(viewsNode);
                        var indexesNode = new IndexesTreeNode(dummySchema, DatabaseConnection, DatabaseInstance);
                        nodes.Add(indexesNode);
                        return nodes;
                    }
                }
            }
            catch (Exception ex)
            {
                DatabaseConnection.Disconnect();
                _log.ErrorFormat("Error opening connection and loading data.");
                _log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}