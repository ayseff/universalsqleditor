using System;
using System.ComponentModel;

namespace SqlEditor.SqlHistory
{
    public class ExecutedSqlStatement : INotifyPropertyChanged
    {
        private string _sqlStatement;
        [DisplayName("SQL")]
        public string SqlStatement
        {
            get
            {
                return _sqlStatement;
            }
            set
            {
                if (_sqlStatement != value)
                {
                    _sqlStatement = value;
                    OnPropertyChanged("SqlStatement");
                }
            }
        }

        private DateTime _runDateTime;
        [DisplayName("Date")]
        public DateTime RunDateTime
        {
            get
            {
                return _runDateTime;
            }
            set
            {
                if (!_runDateTime.Equals(value))
                {
                    _runDateTime = value;
                    OnPropertyChanged("RunDateTime");
                }
            }
        }

        private string _connectionName;
        [DisplayName("Connection")]
        public string ConnectionName
        {
            get
            {
                return _connectionName;
            }
            set
            {
                if (_connectionName != value)
                {
                    _connectionName = value;
                    OnPropertyChanged("ConnectionName");
                }
            }
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
