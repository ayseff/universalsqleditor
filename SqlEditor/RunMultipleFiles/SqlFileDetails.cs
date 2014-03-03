using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SqlEditor.Annotations;

namespace SqlEditor.RunMultipleFiles
{
    public class SqlFileDetails : INotifyPropertyChanged
    {
        #region Fields
        private string _fileName;
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private string _status = "Pending";
        #endregion


        #region Properties
        [DisplayName("SQL File")]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (value == _fileName) return;
                _fileName = value;
                OnPropertyChanged();
            }
        }

        [DisplayName("Elapsed Time")]
        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
            set
            {
                if (value.Equals(_elapsedTime)) return;
                _elapsedTime = value;
                OnPropertyChanged();
            }
        }
        
        public string Status
        {
            get { return _status; }
            set
            {
                if (value == _status) return;
                _status = value;
                OnPropertyChanged();
            }
        }
        #endregion


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
