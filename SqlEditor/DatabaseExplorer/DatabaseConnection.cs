using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using SqlEditor.Databases;

namespace SqlEditor.DatabaseExplorer
{
    [Serializable]
    public class DatabaseConnection : INotifyPropertyChanged, ICloneable
    {
        #region Fields

        private string _connectionString = string.Empty;
        private DatabaseServer _databaseServer;
        private string _folder = string.Empty;
        private bool _isConnected;
        private string _name = string.Empty;
        private int _maxResults = 100;
        private bool _autoCommit;

        #endregion


        #region Properties
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public DatabaseServer DatabaseServer
        {
            get { return _databaseServer; }
            set
            {
                if ((_databaseServer == null && value != null) || (_databaseServer != null && value == null) ||
                    (_databaseServer != null && value != null && _databaseServer.Name != value.Name))
                {
                    _databaseServer = value;
                    OnPropertyChanged("DatabaseServer");
                }
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (_connectionString != value)
                {
                    _connectionString = value;
                    OnPropertyChanged("ConnectionString");
                }
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }

        public string Folder
        {
            get { return _folder; }
            set
            {
                if (_folder != value)
                {
                    _folder = value;
                    OnPropertyChanged("Folder");
                }
            }
        }

        public int MaxResults
        {
            get { return _maxResults; }
            set
            {
                if (_maxResults != value)
                {
                    _maxResults = value;
                    OnPropertyChanged("MaxResults");
                }
            }
        }

        public bool AutoCommit
        {
            get { return _autoCommit; }
            set
            {
                if (value.Equals(_autoCommit)) return;
                _autoCommit = value;
                OnPropertyChanged("AutoCommit");
            }
        }

        public string UserId
        {
            get { return _databaseServer.GetUserId(ConnectionString); }
        }

        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        public DatabaseConnection()
        {
            IsConnected = false;
        }

        public void Connect()
        {
            using (var connection = CreateNewConnection())
            {
                connection.Open();
                connection.Close();
            }
            IsConnected = true;
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }
            IsConnected = false;
        }

        [Annotations.NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public IDbConnection CreateNewConnection()
        {
            return _databaseServer.CreateConnection(ConnectionString);
        }

        public Task<IDbConnection> CreateNewConnectionAsync()
        {
            var task = Task.Run(() => CreateNewConnection());
            return task;
        }

        public IDbConnection TestConnection()
        {
            return _databaseServer.CreateConnection(ConnectionString);
        }

        public override string ToString()
        {
            return Name + (DatabaseServer != null ? (": " + DatabaseServer.Name) : string.Empty);
        }

        public object Clone()
        {
            var connection = new DatabaseConnection();
            connection.AutoCommit = this.AutoCommit;
            connection.ConnectionString = ConnectionString;
            connection.DatabaseServer = this.DatabaseServer;
            connection.Folder = this.Folder;
            connection.IsConnected = this.IsConnected;
            connection.MaxResults = this.MaxResults;
            connection.Name = this.Name;
            return connection;
        }
    }
}