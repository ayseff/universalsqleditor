using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Infragistics.Win.UltraWinTree;
using SqlEditor.Annotations;
using log4net;

namespace SqlEditor.DatabaseExplorer.TreeNodes
{
    public abstract class TreeNodeBase : UltraTreeNode, INotifyPropertyChanged
    {
        #region Fields
        protected static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private bool _isLoaded;
        private bool _isLoading;
        #endregion


        #region Properties
        public DatabaseConnection DatabaseConnection { get; set; }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            protected set
            {
                if (value.Equals(_isLoaded)) return;
                _isLoaded = value;
                OnPropertyChanged("IsLoaded");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value.Equals(_isLoading)) return;
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }
        #endregion


        #region Contructors
        protected TreeNodeBase(DatabaseConnection databaseConnection)
            : base(Guid.NewGuid().ToString())
        {
            Nodes.Add(new DummyTreeNode());
            DatabaseConnection = databaseConnection;
        }

        protected TreeNodeBase(string text, DatabaseConnection databaseConnection)
            : this(databaseConnection)
        {
            Text = text;
        }
        #endregion


        public async Task<IList<TreeNodeBase>> GetNodesAsync()
        {
            _log.DebugFormat("Getting tree nodes for {0} using background thread ...", Text);
            IsLoading = true;
            var nodes = await Task.Run(() => GetNodes());
            _log.DebugFormat("Getting tree nodes for {0} finished.", Text);
            return nodes;
        }

        public async void LoadAsync()
        {
            try
            {
                IsLoading = true;
                IsLoaded = false;
                var nodes = await GetNodesAsync();
                Nodes.Clear();
                foreach (var node in nodes)
                {
                    Nodes.Add(node);
                }
                IsLoaded = true;
            }
            catch (Exception)
            {
                _log.ErrorFormat("Error loading nodes for {0}.", Text);
                CleanupNodes();
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void Load(IEnumerable<TreeNodeBase> nodes)
        {
            try
            {
                IsLoading = true;
                IsLoaded = false;
                Nodes.Clear();
                foreach (var node in nodes)
                {
                    Nodes.Add(node);
                }
                IsLoaded = true;
            }
            catch (Exception)
            {
                _log.DebugFormat("Error loading nodes for {0}.", Text);
                CleanupNodes();
                throw;
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void CleanupNodes()
        {
            Nodes.Clear();
            Nodes.Add(new DummyTreeNode());
            Expanded = false;
            IsLoaded = false;
            IsLoading = false;
        }

        public virtual void ReloadAsync()
        {
            LoadAsync();
        }

        public void Reset()
        {
            IsLoading = false;
            IsLoaded = false;
            Nodes.Clear();
            Nodes.Add(new DummyTreeNode());
            CollapseAll();
        }

        public virtual string GetClipboardText()
        {
            return Text;
        }

        protected abstract IList<TreeNodeBase> GetNodes();
        
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}