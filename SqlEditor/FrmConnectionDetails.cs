using System;
using System.ComponentModel;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Infragistics.Win;
using Microsoft.WindowsAPICodePack.Dialogs;
using SqlEditor.DatabaseExplorer;
using SqlEditor.Databases;
using Utilities.Text;
using log4net;

namespace SqlEditor
{
    public partial class FrmConnectionDetails : Form
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DatabaseConnection _databaseConnection;
        private bool _connectionTested;
        private DbConnectionStringBuilder _connectionStringBuilder;
        private DatabaseServer _databaseServer;

        public FrmConnectionDetails(DatabaseConnection databaseConnection, bool readOnly = false)
        {
            if (databaseConnection == null) throw new ArgumentNullException("databaseConnection");

            InitializeComponent();

            _databaseConnection = databaseConnection;
            if (_databaseConnection.DatabaseServer == null)
            {
                _databaseConnection.DatabaseServer = DatabaseServerFactory.Instance.SupportedServers.First();
            }
            _uteConnectionName.Text = _databaseConnection.Name ?? string.Empty;
            _uceDatabaseType.DataSource = DatabaseServerFactory.Instance.SupportedServers;
            var selectedItem = _uceDatabaseType.Items.ValueList.ValueListItems.Cast<ValueListItem>().FirstOrDefault(
                        li => li.DisplayText == _databaseConnection.DatabaseServer.Name);
            if (selectedItem != null)
            {
                _uceDatabaseType.SelectedItem = selectedItem;   
            }
            else
            {
                _uceDatabaseType.SelectedIndex = 0;
            }
            _databaseServer = (DatabaseServer)_uceDatabaseType.SelectedItem.ListObject;
            _connectionStringBuilder = _databaseConnection.DatabaseServer.GetConnectionStringBuilder(_databaseConnection.ConnectionString);
            var simpleConnectionStringBuilder = _databaseConnection.DatabaseServer.GetSimpleConnectionStringBuilder(_connectionStringBuilder);
            _connectionStringBuilder.BrowsableConnectionString = false;
            _uneMaxiumumResults.Text = _databaseConnection.MaxResults.ToString(CultureInfo.InvariantCulture);
            _uceAutoCommit.CheckState = _databaseConnection.AutoCommit ? CheckState.Checked : CheckState.Unchecked;

            if (readOnly)
            {
                this.Text += " (Read Only)";
                TypeDescriptor.AddAttributes(_connectionStringBuilder, new Attribute[] { new ReadOnlyAttribute(true) });
                TypeDescriptor.AddAttributes(simpleConnectionStringBuilder, new Attribute[] { new ReadOnlyAttribute(true) });
                _uceAutoCommit.Enabled = false;
                _uteConnectionName.ReadOnly = true;
                _uceDatabaseType.ReadOnly = true;
                _uneMaxiumumResults.ReadOnly = true;
                _ubOk.Enabled = false;
                _ubTestConnection.Enabled = false;
            }
            _pgConnectionAdvanced.SelectedObject = _connectionStringBuilder;
            _pgConnectionSimple.SelectedObject = simpleConnectionStringBuilder;
        }

        private bool IsConnectionValid(out Exception e)
        {
            _log.Debug("Validating required elements ...");
            try
            {
                _databaseConnection.DatabaseServer.ValidateConnectionString(_connectionStringBuilder);
            }
            catch (Exception ex)
            {
                _log.Warn("Error validating required elements.");
                _log.Warn(ex.Message, ex);
                e = ex;
                _connectionTested = false;
                return false;
            }

            _log.DebugFormat("Testing connection against {0} database for connection string {1}.", _databaseConnection.DatabaseServer.Name, _databaseConnection.ConnectionString);
            e = null;
            _databaseConnection.ConnectionString = _connectionStringBuilder.ConnectionString;
            try
            {
                using (var connection = _databaseServer.CreateConnection(_connectionStringBuilder.ConnectionString))
                {
                    connection.OpenIfRequired();
                    _log.Debug("Connection opened successfully.");
                    connection.Close();
                    _connectionTested = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                e = ex;
                _log.Error("Failed to open connection");
                _log.Error(ex.Message, ex);
                _connectionTested = false;
            }
            return false;
        }

        private void UceDatabaseTypeSelectionChangeCommitted(object sender, EventArgs e)
        {
            _databaseServer = (DatabaseServer) _uceDatabaseType.SelectedItem.ListObject;
            _databaseConnection.DatabaseServer = _databaseServer;
            //_log.InfoFormat("Database server: {0}", _databaseServer.GetType());
            _connectionStringBuilder = _databaseServer.GetConnectionStringBuilder();
            //_log.InfoFormat("_connectionStringBuilder: {0}", _connectionStringBuilder.GetType());
            var simpleConnectionStringBuilder = _databaseServer.GetSimpleConnectionStringBuilder(_connectionStringBuilder);
            //_log.InfoFormat("simpleConnectionStringBuilder: {0}", simpleConnectionStringBuilder.GetType());
            _pgConnectionAdvanced.SelectedObject = _connectionStringBuilder;
            _pgConnectionSimple.SelectedObject = simpleConnectionStringBuilder;
            _connectionTested = false;
        }

        private void UbTestConnectionClick(object sender, EventArgs e)
        {
            Exception ex;
            if (IsConnectionValid(out ex))
            {
                Utilities.Forms
                    .Dialogs.Dialog.ShowDialog("Success", "Connection opened successfully.", string.Format("Connection to {0} was opened successfully.", _databaseServer.Name));
            }
            else
            {
                Utilities.Forms
                    .Dialogs.Dialog.ShowDialog("Error", "Error opening connection.", string.Format("Connection information for {0} is not valid. {1}.", _databaseServer.Name, ex.Message), TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Error);
            }
        }

        private void PgConnectionPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _connectionTested = false;
        }

        private void UbOkClick(object sender, EventArgs e)
        {
            Exception ex;
            if (!_connectionTested && !IsConnectionValid(out ex))
            {
                Utilities.Forms
                    .Dialogs.Dialog.ShowDialog("Error", "Error opening connection.", string.Format("Connection information for {0} is not valid. {1}.", _databaseServer.Name, ex.Message), TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Error);
            }

            else if (_uteConnectionName.Text.IsNullEmptyOrWhitespace())
            {
                Utilities.Forms
                    .Dialogs.Dialog.ShowDialog("Error", "Connection name missing.", "Please specify connection name.", TaskDialogStandardButtons.Ok, TaskDialogStandardIcon.Error);
            }
            else
            {
                _databaseConnection.DatabaseServer = _databaseServer;
                _databaseConnection.Name = _uteConnectionName.Text;
                _databaseConnection.ConnectionString = _connectionStringBuilder.ConnectionString;
                _databaseConnection.AutoCommit = _uceAutoCommit.CheckState == CheckState.Checked;
                _databaseConnection.MaxResults = (int) _uneMaxiumumResults.Value;
                DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void UbCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
