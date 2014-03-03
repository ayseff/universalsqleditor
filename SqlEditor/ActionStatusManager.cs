using System.Collections.Generic;
using System.Windows.Forms;

namespace SqlEditor
{
    public class ActionStatusManager
    {
        private readonly List<string> _statusList = new List<string>();
        private const string DEFAULT_STATUS = "Ready.";
        private static ActionStatusManager _instance;
        public static ActionStatusManager Instance
        {
            get { return _instance ?? (_instance = new ActionStatusManager()); }
        }

        public void StartAction(string action)
        {
            lock (_statusList)
            {
                _statusList.Add(action);
            }
            FrmMdiParent.Instance.StatusBarText = action;
            //Application.DoEvents();
        }

        public void EndAction(string action)
        {
            string currentStatus;
            lock (_statusList)
            {
                var success = _statusList.Remove(action);
                currentStatus = success && _statusList.Count > 0 ? _statusList[0] : DEFAULT_STATUS;
            }
            FrmMdiParent.Instance.StatusBarText = currentStatus;
            //Application.DoEvents();
        }
    }
}
