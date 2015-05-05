using System;
using System.ComponentModel;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class ConnectionTreeNode : TreeNodeBase
    {
        public override bool OpensWorksheet
        {
            get { return true; }
        }

        protected ConnectionTreeNode(DatabaseConnection connection)
            : base(connection.Name, connection, null)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            
            var connectionImage = DatabaseServerFactory.Instance.GetConnectionOpenImage(connection.DatabaseServer);
            if (!connection.IsConnected)
            {
                connectionImage = DatabaseServerFactory.Instance.GetConnectionClosedImage(connection.DatabaseServer);
            }
            this.Override.NodeAppearance.Image = connectionImage;
            connection.PropertyChanged += ConnectionPropertyChanged;
        }

        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                DatabaseConnection.Name = value;
            }
        }

        private void ConnectionPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Text = DatabaseConnection.Name;
            if (e.PropertyName == "Name")
            {
                Text = DatabaseConnection.Name;
            }
            else if (e.PropertyName == "IsConnected")
            {
                UpdateConnectedStatus();
            }
            else if (e.PropertyName == "Unknown")
            {
                Text = DatabaseConnection.Name;
                UpdateConnectedStatus();
            }
        }

        private void UpdateConnectedStatus()
        {
            var imageName =
                DatabaseServerFactory.Instance.GetConnectionOpenImage(DatabaseConnection.DatabaseServer);
            if (!DatabaseConnection.IsConnected)
            {
                imageName =
                    DatabaseServerFactory.Instance.GetConnectionClosedImage(DatabaseConnection.DatabaseServer);
            }
            this.Override.NodeAppearance.Image = imageName;
        }

        //protected override IList<TreeNodeBase> GetNodes()
        //{
        //    try
        //    {
        //        var actionName = string.Format("Attempting to connect to {0} ...", DatabaseConnection.Name);
        //        _log.Debug(actionName);
        //        using (new WaitActionStatus(actionName))
        //        {
        //            _log.DebugFormat("Creating connection for {0} ...", DatabaseConnection.Name);
        //            using (var connection = DatabaseConnection.CreateNewConnection())
        //            {
        //                connection.OpenIfRequired();
        //                DatabaseConnection.Connect();
        //                _log.Debug("Connection is successful.");
        //                _log.Debug("Loading schemas ...");
        //                var infoProvider = DatabaseConnection.DatabaseServer.GetInfoProvider();
        //                _schemas.Clear();
        //                _schemas.AddRange(infoProvider.GetSchemas(connection));
        //                _log.DebugFormat("Loaded {0} schema(s).", _schemas.Count);

        //                _log.Debug("Creating nodes ...");
        //                var schemNodes =
        //                    _schemas.Select(schema => new SchemaTreeNode(schema, DatabaseConnection)).Cast<TreeNodeBase>().ToList();
        //                _log.Debug("Loading nodes finished.");
        //                return schemNodes;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.DatabaseConnection.Disconnect();
        //        _log.ErrorFormat("Error opening connection and loading data.");
        //        _log.Error(ex.Message, ex);
        //        throw;
        //    }
        //}

        public void Disconnect()
        {
            DatabaseConnection.Disconnect();
            Reset();
        }
    }
}