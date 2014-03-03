using System.Collections.Generic;
using SqlEditor.Database;
using SqlEditor.Databases;

namespace SqlEditor.Intellisense
{
    public class IntelisenseData
    {
        //private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //private readonly DatabaseConnection _databaseConnection;
        //private AbortableBackgroundWorker _worker;

        public List<Schema> AllSchemas { get; protected set; }
        public List<DatabaseObjectWithColumns> AllObjects { get; protected set; }
        public List<Column> AllColumns { get; protected set; }
        public Schema CurrentSchema { get; set; }
        public bool IsLoaded { get; protected set; }

        public IntelisenseData()
        {
            AllSchemas = new List<Schema>();
            AllObjects = new List<DatabaseObjectWithColumns>();
            AllColumns = new List<Column>();
            IsLoaded = false;
        }

        //public void LoadAsync(DatabaseConnection databaseConnection, bool force)
        //{
        //    if (databaseConnection == null || !databaseConnection.IsConnected)
        //    {
        //        _log.Warn("Intellisense database connection is NULL or not connected. Skipping getting data.");
        //        return;
        //    }
        //    else if (!force && IsLoaded)
        //    {
        //        _log.Debug("Intellisense already loaded.");
        //        return;
        //    }
        //    _log.Debug("Running gathering of intellisense data async ...");
        //    _worker = new AbortableBackgroundWorker();
        //    _worker.DoWork += WorkerDoWork;
        //    _worker.RunWorkerCompleted += WorkerRunWorkerCompleted;
        //    _worker.RunWorkerAsync(databaseConnection);
        //}

        //private void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        _log.Error("Error loading intellisense data.");
        //        _log.Error(e.Error.Message, e.Error);
        //    }
        //    else if (e.Cancelled)
        //    {
        //        _log.Info("Loading of intellisense data cancelled.");
        //    }
        //    else
        //    {
        //        _log.Debug("Loading of intellisense data completed.");
        //    }
        //}

        //private void WorkerDoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        Load((DatabaseConnection)e.Argument);
        //    }
        //    catch (ThreadAbortException)
        //    {
        //        _log.Info("Loading of intellisense data thread aborted.");
        //    }
        //}

        //public void Load(DatabaseConnection databaseConnection)
        //{
        //    using (new ActionStatus("Loading intellisense data ..."))
        //    {
        //        _log.Debug("Loading intellisense data ...");            
        //        AllSchemas.Clear();
        //        AllObjects.Clear();
        //        AllColumns.Clear();
        //        IsLoaded = false;

        //        try
        //        {
        //            using (var connection = databaseConnection.CreateNewConnection())
        //            {
        //                var infoProvider = databaseConnection.DatabaseServer.GetInfoProvider();
        //                infoProvider.GetIntelisenseData(connection);
        //                var command = connection.CreateCommand();
        //                command.CommandText = "select owner, object_name, column_name"
        //                                      + " from ("
        //                                      +
        //                                      " select owner, table_name as object_name, column_name from all_tab_columns where owner not like 'SYS%' and owner not like '%SYS'"
        //                                      + " union all"
        //                                      +
        //                                      " SELECT o.owner, o.view_name as object_name, column_name FROM cols c, all_views o WHERE c.TABLE_NAME = o.view_name and owner not like 'SYS%' and owner not like '%SYS'"
        //                                      + " union all"
        //                                      +
        //                                      " SELECT o.owner, o.mview_name as object_name, column_name FROM cols c, all_mviews o WHERE c.TABLE_NAME = o.mview_name and owner not like 'SYS%' and owner not like '%SYS')"
        //                                      + " order by owner, object_name, column_name";
        //                using (var dr = command.ExecuteReader())
        //                {
        //                    string previousOwner = null, previousObject = null;
        //                    OracleSchema schema = null;
        //                    DatabaseObject dbObject = null;
        //                    while (dr.Read())
        //                    {
        //                        string owner = dr.GetString(0).Trim().ToUpper();
        //                        if (owner != previousOwner)
        //                        {
        //                            previousOwner = owner;
        //                            schema = AllSchemas.Where(s => s.Name.Trim().ToUpper() == owner).FirstOrDefault();
        //                            if (schema == null)
        //                            {
        //                                schema = new OracleSchema(owner, _databaseConnection);
        //                                AllSchemas.Add(schema);
        //                            }
        //                        }

        //                        string objectName = dr.GetString(1).Trim().ToUpper();
        //                        if (objectName != previousObject)
        //                        {
        //                            previousObject = objectName;
        //                            dbObject = AllObjects.Where(o => o.Name.Trim().ToUpper() == objectName && o.ParentSchema != null && o.ParentSchema.Name == owner).FirstOrDefault();
        //                            if (dbObject == null)
        //                            {
        //                                dbObject = new DatabaseObject(schema, objectName);
        //                                schema.Tables.Add(dbObject);
        //                                AllObjects.Add(dbObject);
        //                            }
        //                        }

        //                        string columnName = dr.GetString(2).Trim().ToUpper();
        //                        var column = new Column(columnName, null) { Name = columnName };
        //                        dbObject.Columns.Add(column);
        //                        AllColumns.Add(column);
        //                    }
        //                }
        //            }
        //        }
        //        catch (System.Exception ex)
        //        {
        //            _log.Error("Error loading intellisense data.");
        //            _log.Error(ex.Message, ex);                
        //        }            
        //        IsLoaded = true;
        //        if (_log.IsDebugEnabled)
        //        {
        //            _log.Debug("Loading intellisense data completed sucessfully.");
        //            _log.DebugFormat("Loaded {0} schemas.", AllSchemas.Count);
        //            _log.DebugFormat("Loaded {0} tables.", AllObjects.Count);
        //            _log.DebugFormat("Loaded {0} columns.", AllColumns.Count);
        //        }
        //    }
        //}
    }
}
