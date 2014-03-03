using System;
using System.Reflection;
using System.Windows.Forms;
using log4net;

namespace Utilities.Forms
{
    public class StatusBusy : IDisposable
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private int _oldProgress;
        private string _oldStatus;
        private Cursor _oldCursor;
        private readonly Control _control;
        private readonly IStatusReceiver _statusReceiver;

        /// <summary>
        /// Gets or sets the status label on the target control.
        /// </summary>
        public string Status
        {
            get
            {
                if (_statusReceiver == null)
                {
                    throw new Exception("Control passed in does not implement IStatusReceiver.");
                }
                string label = null;
                _control.InvokeIfRequired(() => label = _statusReceiver.StatusLabel);
                return label;
            }
            set
            {
                if (_statusReceiver == null)
                {
                    throw new Exception("Control passed in does not implement IStatusReceiver.");
                }
                _control.InvokeIfRequired(() => _statusReceiver.StatusLabel = value);
                _log.Info(value);
            }
        }

        /// <summary>
        /// Gets or sets progress on the target form.
        /// </summary>
        public int Progress
        {
            get
            {
                if (_statusReceiver == null)
                {
                    throw new Exception("Control passed in does not implement IStatusReceiver.");
                }
                var progress = 0;
                _control.InvokeIfRequired(() => progress = _statusReceiver.ProgressPercentage);
                return progress;
            }
            set
            {
                if (_statusReceiver == null)
                {
                    throw new Exception("Control passed in does not implement IStatusReceiver.");
                }
                _control.InvokeIfRequired(() => _statusReceiver.ProgressPercentage = value);
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="control">Control which tracks status. Control must implement <see cref="IStatusReceiver"/> in order to be able to change status and progress.</param>
        public StatusBusy(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            _control = control;
// ReSharper disable SuspiciousTypeConversion.Global
            _statusReceiver = control as IStatusReceiver;
// ReSharper restore SuspiciousTypeConversion.Global
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="receiver">Control which tracks status. Control must implement <see cref="IStatusReceiver"/> in order to be able to change status and progress.</param>
        /// <param name="statusText">Statsu text to set.</param>
        /// <param name="showWaitCursor">Wthether to show wait cursor on the target control.</param>
        /// <param name="progress">Progress to set.</param>
        public StatusBusy(Control receiver, string statusText, bool showWaitCursor = true, int? progress = null)
            : this(receiver)
        {
            if (_statusReceiver != null)
            {
                _control.InvokeIfRequired(() =>
                    {
                        _oldStatus = _statusReceiver.StatusLabel;
                        _statusReceiver.StatusLabel = statusText;
                        _oldProgress = _statusReceiver.ProgressPercentage;
                        if (progress != null && _oldProgress != progress.Value)
                        {
                            _statusReceiver.ProgressPercentage = progress.Value;
                        }
                    });
            }

            _control.InvokeIfRequired(() =>
            {
                _oldCursor = _control.Cursor;
                if (showWaitCursor)
                {
                    _control.Cursor = Cursors.WaitCursor;
                }
            });
            _log.Info(statusText);
        }

        // IDisposable
        private bool _disposedValue; // To detect redundant calls

        protected void Dispose(bool disposing)
        {
            if (!_disposedValue)
            if (disposing)
            {
                if (_statusReceiver != null)
                {
                    _control.InvokeIfRequired(() =>
                        {
                            _statusReceiver.StatusLabel = _oldStatus;
                            _statusReceiver.ProgressPercentage = _oldProgress;
                            _control.Cursor = _oldCursor;
                        });
                }
                _control.InvokeIfRequired(() =>
                {
                    _control.Cursor = _oldCursor;
                });
            }
            _disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
