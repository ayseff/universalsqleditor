using System;
using System.Collections.Concurrent;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using SqlEditor.DatabaseExplorer;
using log4net;

namespace SqlEditor.Intellisense
{
    public class IntellisenseManager
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static IntellisenseManager _instance;
        private readonly ConcurrentDictionary<DatabaseConnection, IntelisenseData> _intellisenseMap = new ConcurrentDictionary<DatabaseConnection, IntelisenseData>();
        
        public static IntellisenseManager Instance
        {
            get { return _instance ?? (_instance = new IntellisenseManager()); }
        }

        private IntellisenseManager()
        {
        }

        //private static void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        _log.Error("Intellisense data load failed.");
        //        _log.Error(e.Error.Message, e.Error);
        //    }
        //}

        //private void WorkerDoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        var databaseConnection = (DatabaseConnection) e.Argument;
        //        _log.DebugFormat("Worker is loading Intellisense data loaded for {0}.", databaseConnection.Name);
        //        var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
        //        using (var connection = databaseConnection.CreateNewConnection())
        //        {
        //            if (connection.State != ConnectionState.Open)
        //            {
        //                connection.Open();
        //            }
        //            var intellisenseData = infoProvider.GetIntelisenseData(connection, databaseConnection.UserId);
        //            _intellisenseMap.AddOrUpdate(databaseConnection, intellisenseData, (key, existingVal) => existingVal);
        //        }
        //        _log.DebugFormat("Intellisense data loaded for {0}.", databaseConnection.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error("Error loading intellisense data.");
        //        _log.Error(ex.Message, ex);
        //    }
        //}

        public IntelisenseData GetIntellisenseData(DatabaseConnection databaseConnection)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            _log.DebugFormat("Getting intellisense data for {0} ...", databaseConnection.Name);
            IntelisenseData intelisenseData;
            if (_intellisenseMap.TryGetValue(databaseConnection, out intelisenseData))
            {
                _log.DebugFormat("Cashed intellisense data found.");
                return intelisenseData;
            }
            _log.DebugFormat("Cashed intellisense data NOT found. Starting load of intellisense data ...");
            LoadIntellisenseDataAsync(databaseConnection);
            return null;
        }

        public Task LoadIntellisenseDataAsync(DatabaseConnection databaseConnection, bool forceRefresh = false)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
            _log.DebugFormat("Loading intellisense data for {0} ...", databaseConnection.Name);
            if (forceRefresh)
            {
                _log.Debug("Force refresh requested ...");
                IntelisenseData intellisenseData;
                _intellisenseMap.TryRemove(databaseConnection, out intellisenseData);
            }
            else if (_intellisenseMap.ContainsKey(databaseConnection))
            {
                _log.DebugFormat("Intellisense data is already in cache for {0}.", databaseConnection.Name);
                return null;
            }

            _log.DebugFormat("Starting background thread to load intellisense data for {0} ...", databaseConnection.Name);
            var task = Task.Run(() =>
                                    {
                                        try
                                        {
                                            _log.DebugFormat("Worker is loading Intellisense data for {0}.", databaseConnection.Name);
                                            var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
                                            using (var connection = databaseConnection.CreateNewConnection())
                                            {
                                                connection.OpenIfRequired();
                                                var intellisenseData = infoProvider.GetIntelisenseData(connection, databaseConnection.UserId);
                                                _intellisenseMap.AddOrUpdate(databaseConnection, intellisenseData, (key, existingVal) => existingVal);
                                            }
                                            _log.DebugFormat("Intellisense data loaded for {0}.", databaseConnection.Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            _log.Error("Error loading intellisense data.");
                                            _log.Error(ex.Message, ex);
                                        }
                                    });
            return task;
        }

        //public void LoadIntellisenseData(DatabaseConnection databaseConnection, bool forceRefresh = false)
        //{
        //    if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");
        //    _log.DebugFormat("Loading intellisense data for {0} ...", databaseConnection.Name);
        //    if (forceRefresh)
        //    {
        //        _log.Debug("Force refresh requested ...");
        //        IntelisenseData intellisenseData;
        //        _intellisenseMap.TryRemove(databaseConnection, out intellisenseData);
        //    }

        //    _log.DebugFormat("Starting background thread to load intellisense data for {0} ...", databaseConnection.Name);
        //    var worker = GetNextWorker();
        //    worker.RunWorkerAsync(databaseConnection);
        //    _log.DebugFormat("Background thread to load intellisense data for {0} started.", databaseConnection.Name);
        //}

        //private BackgroundWorker GetNextWorker()
        //{
        //    lock (_workers)
        //    {
        //        var worker = _workers.FirstOrDefault(x => !x.IsBusy);
        //        if (worker == null)
        //        {
        //            worker = new BackgroundWorker();
        //            worker.DoWork += WorkerDoWork;
        //            worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        //            _workers.Add(worker);
        //        }
        //        return worker;
        //    }
        //}
    }
}
