using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using SqlEditor.DatabaseExplorer;
using SqlEditor.Databases;
using SqlEditor.SqlHelpers;
using Utilities.Collections;
using Utilities.Forms.Dialogs;
using Utilities.InfragisticsUtilities.UltraGridUtilities;
using log4net;

namespace SqlEditor
{
    public sealed partial class FrmTableDetails : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(FrmTableDetails));
        private static readonly UltraGridNullValueDataFilter _nullColumnDataFilter = new UltraGridNullValueDataFilter();
        private readonly DatabaseConnection _databaseConnection;
        private readonly Table _table;
        private DataTable _data;
        private IList<Index> _indexes;
        private IList<Partition> _partitions;  

        public FrmTableDetails(DatabaseConnection connection, Table table)
        {
            InitializeComponent();
            _databaseConnection = connection;
            _table = table;
            Text = table.FullyQualifiedName;
        }

        private void FrmTableDetailsLoad(object sender, EventArgs e)
        {            
            _utcTabs.Tabs["Columns"].Selected = true;
            foreach (var grid in new[] { _ugColumns, _ugData, _ugIndexes, _ugPartitions })
            {
                foreach (var band in grid.DisplayLayout.Bands)
                {
                    foreach (var column in band.Columns)
                    {
                        column.CellActivation = Activation.ActivateOnly;
                    }
                }   
            }
        }

        private void UtcTabsSelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            if (e.Tab == _utcTabs.Tabs["Data"])
            {
                LoadData();
            }
            else if (e.Tab == _utcTabs.Tabs["Indexes"])
            {
                LoadIndexes();
            }
            else if (e.Tab == _utcTabs.Tabs["Columns"])
            {
                LoadColumns();
            }
            else if (e.Tab == _utcTabs.Tabs["Partitions"])
            {
                LoadPartitions();
            }
        }    
    
        private void LoadPartitions()
        {
            try
            {
                if (_partitions == null)
                {
                    using (new WaitActionStatus("Loading partitions ..."))
                    {
                        var dbInfoProvider = _databaseConnection.DatabaseServer.GetInfoProvider();
                        using (var connection = _databaseConnection.CreateNewConnection())
                        {
                            connection.OpenIfRequired();
                            _partitions = dbInfoProvider.GetTablePartitions(connection, _table.Parent.Name, _table.Name);
                            _ugPartitions.DataSource = _partitions;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error getting partitions for table {0}.", _table.FullyQualifiedName);
                _log.Error(ex.Message);
                Dialog.ShowErrorDialog(Application.ProductName, "Error retrieving partitions. ", ex.Message,
                                       ex.StackTrace);
            }               
        }

        private void LoadColumns()
        {
            try
            {
                if (_table.Columns.Count == 0)
                {
                    using (new WaitActionStatus("Loading table columns ..."))
                    {
                        var dbInfoProvider = _databaseConnection.DatabaseServer.GetInfoProvider();
                        using (var connection = _databaseConnection.CreateNewConnection())
                        {
                            connection.OpenIfRequired();
                            _table.Columns.AddRange(dbInfoProvider.GetTableColumns(connection, _table.Parent.Name,
                                                                                   _table.Name));
                        }
                    }
                }
                _ugColumns.DataSource = _table.Columns;
                _ugColumns.DisplayLayout.Bands[0].Columns["Name"].Header.VisiblePosition = 0;
                _ugColumns.DisplayLayout.Bands[0].Columns["DataType"].Header.VisiblePosition = 1;
                _ugColumns.DisplayLayout.Bands[0].Columns["CharacterLength"].Header.VisiblePosition = 2;
                _ugColumns.DisplayLayout.Bands[0].Columns["DataScale"].Header.VisiblePosition = 3;
                _ugColumns.DisplayLayout.Bands[0].Columns["DataPrecision"].Header.VisiblePosition = 4;
                _ugColumns.DisplayLayout.Bands[0].Columns["Nullable"].Header.VisiblePosition = 5;
                _ugColumns.DisplayLayout.Bands[0].Columns["OrdinalPosition"].Header.VisiblePosition = 6;
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error getting columns for table {0}.", _table.FullyQualifiedName);
                _log.Error(ex.Message);
                Dialog.ShowErrorDialog(Application.ProductName, "Error retrieving columns. ", ex.Message,
                                       ex.StackTrace);
            } 
        }

        private void LoadIndexes()
        {
            try
            {
                if (_indexes == null)
                {
                    using (new WaitActionStatus("Loading indexes ..."))
                    {
                        var provider = _databaseConnection.DatabaseServer.GetInfoProvider();
                        using (var connection = _databaseConnection.CreateNewConnection())
                        {
                            connection.OpenIfRequired();
                            _indexes = provider.GetIndexesForTable(connection,
                                                                   _table.Parent.Name, _table.Name);
                        } 
                        _ugIndexes.DataSource = _indexes;
                        _ugIndexes.ResizeColumnsToFit(PerformAutoSizeType.VisibleRows);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error getting indexes for table {0}.", _table.FullyQualifiedName);
                _log.Error(ex.Message);
                Dialog.ShowErrorDialog(Application.ProductName, "Error retrieving indexes. ", ex.Message,
                                       ex.StackTrace);
            } 
            
        }

        //private void UpdateIndexedColumns(DataTable _indexes, string tableName, string tableOwner, OracleConnection conn)
        //{            
        //    var command = conn.CreateCommand();
        //    command.BindByName = true;
        //    command.CommandText = "select INDEX_OWNER, COLUMN_NAME from ALL_IND_COLUMNS where INDEX_NAME = :1 and TABLE_OWNER = :2 and TABLE_NAME = :3 order by COLUMN_POSITION";
        //    foreach (DataRow row in _indexes.Rows)
        //    {
        //        string indexName = row.Field<string>("INDEX_NAME").Trim().ToUpper();                
        //        command.Parameters.Clear();
        //        command.Parameters.Add(new OracleParameter(":1", indexName));
        //        command.Parameters.Add(new OracleParameter(":2", tableOwner));
        //        command.Parameters.Add(new OracleParameter(":3", tableName));
        //        StringBuilder sb = new StringBuilder();
        //        string indexOwner = null;
        //        using (var dr = command.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                if (string.IsNullOrEmpty(indexOwner))
        //                {
        //                    indexOwner = dr.GetString(0);
        //                }
        //                if (sb.Length > 0)
        //                {
        //                    sb.Append(", ");
        //                }
        //                sb.Append(dr.GetString(1));
        //            }
        //        }
        //        var indexOwnerColumn = _indexes.Columns["INDEX_OWNER"];
        //        indexOwnerColumn.ReadOnly = false;
        //        indexOwnerColumn.MaxLength = 30;
        //        var indexedColumnsColumn = _indexes.Columns["COLUMNS"];
        //        indexedColumnsColumn.ReadOnly = false;
        //        indexedColumnsColumn.MaxLength = int.MaxValue;
        //        row[indexOwnerColumn] = indexOwner;
        //        row[indexedColumnsColumn] = sb.ToString();                
        //    }            
        //}

        private void LoadData()
        {
            try
            {
                if (_data == null)
                {
                    _data = new DataTable();
                    using (new WaitActionStatus("Loading data ..."))
                    {
                        using (var conn = _databaseConnection.CreateNewConnection())
                        {
                            conn.OpenIfRequired();
                            using (var sqlCommand = conn.CreateCommand())
                            {
                                sqlCommand.CommandText = "SELECT * FROM " + _table.FullyQualifiedName;
                                using (var dataReader = sqlCommand.ExecuteReader())
                                {
                                    _ugData.DataSource = dataReader.FetchDataTable(_databaseConnection.MaxResults);
                                }
                            }
                        }
                        _ugData.ResizeColumnsToFit(PerformAutoSizeType.VisibleRows);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ErrorFormat("Error getting data for table {0}.", _table.FullyQualifiedName);
                _log.Error(ex.Message);
                Dialog.ShowErrorDialog(Application.ProductName, "Error retrieving data. ", ex.Message,
                                       ex.StackTrace);
            }           
        }

        private UltraGrid GetActiveGrid()
        {
            if (_utcTabs.Tabs["Columns"].Selected)
            {
                return _ugColumns;
            }
            else if (_utcTabs.Tabs["Data"].Selected)
            {
                return _ugData;
            }
            else if (_utcTabs.Tabs["Indexes"].Selected)
            {
                return _ugIndexes;
            }
            else if (_utcTabs.Tabs["Partitions"].Selected)
            {
                return _ugPartitions;
            }
            return null;
        }

        private void Utm_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            var grid = GetActiveGrid();
            if (grid == null) return;

            try
            {
                switch (e.Tool.Key)
                {
                    case "Export to Excel":
                        grid.ExportToExcel();
                        break;

                    case "CopyText":
                        grid.PerformAction(UltraGridAction.Copy);
                        break;

                }
            }
            catch (Exception ex)
            {
                _log.Error("Error performing action.");
                _log.Error(ex.Message, ex);
                Dialog.ShowErrorDialog(Application.ProductName, "Error occurred.", ex.Message, ex.StackTrace);
            }
        }

        private void Grid_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            foreach (var band in e.Layout.Bands)
            {
                foreach (var column in band.Columns)
                {
                    column.CellActivation = Activation.ActivateOnly;
                }
            }

            foreach (var column in e.Layout.Bands[0].Columns)
            {
                column.Editor.DataFilter = _nullColumnDataFilter;
            }
        }
    }
}
