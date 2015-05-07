using System;
using System.Collections.Generic;
using SqlEditor.DatabaseExplorer.TreeNodes.Base;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public sealed class LoginsTreeNode : FolderContainerTreeNode
    {
        public LoginsTreeNode(DatabaseConnection databaseConnection, DatabaseInstance databaseInstance)
            : base(databaseConnection, databaseInstance, "Logins", "users-blue.png", "users-blue.png")
        {
        }

        protected override IList<TreeNodeBase> GetNodes()
        {
            _log.Debug("Loading users ...");
            try
            {
                var actionName = string.Format("Getting logins for {0} ...", DatabaseConnection.Name);
                _log.Debug(actionName);
                using (new WaitActionStatus(actionName))
                {
                    _log.DebugFormat("Creating connection for {0} ...", DatabaseConnection.Name);
                    using (var connection = DatabaseConnection.CreateNewConnection())
                    {
                        connection.OpenIfRequired();
                        var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
                        var logins = infoProvider.GetLogins(connection, DatabaseInstance == null ? null : DatabaseInstance.Name);
                        return TreeNodeFactory.GetLoginNodes(logins, DatabaseConnection, DatabaseInstance);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error opening connection and loading logins.");
                _log.Error(ex.Message);
                throw new Exception("Error opening connection and loading logins.", ex);
            }
        }
    }
}