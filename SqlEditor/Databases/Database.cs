using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;
using SqlEditor.Database;

namespace SqlEditor.Databases
{
    [Serializable]
    public abstract class Database : INotifyPropertyChanged
    {
        #region Fields

        private string _connectionString;
        private string _folder;
        private bool _isConnected;
        private string _name;
        private bool _supportsMultipleRecordSets = true;
        private bool _supportsMultipleTransactions = true;

        #endregion

        #region ProtectedMembers

        public abstract DbInfoProvider GetInfoProvider();

        #endregion

        #region Properties

        public virtual string Name
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

        public virtual bool SupportsMultipleRecordSets
        {
            get { return _supportsMultipleRecordSets; }
            set
            {
                if (_supportsMultipleRecordSets != value)
                {
                    _supportsMultipleRecordSets = value;
                    OnPropertyChanged("SupportsMultipleRecordSets");
                }
            }
        }

        public virtual bool SupportsMultipleTransactions
        {
            get { return _supportsMultipleTransactions; }
            set
            {
                if (_supportsMultipleTransactions != value)
                {
                    _supportsMultipleTransactions = value;
                    OnPropertyChanged("SupportsMultipleTransactions");
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

        public abstract Regex ValidIdentifierRegex { get; }

        public abstract Regex ValidFullyQualifiedIdentifierRegex { get; }

        public abstract Schema Schema { get; }

        public abstract DatabaseServerType ServerType { get; }

        public abstract string GetLoginUserId();

        #endregion

        #region Methods

        public abstract DbConnectionStringBuilder GetConnectionStringBuilder(string connectionString = null);

        public abstract IDbConnection CreateNewConnection();

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}