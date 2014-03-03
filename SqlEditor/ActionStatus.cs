using System;

namespace SqlEditor
{
    public class ActionStatus : IDisposable
    {
        private readonly string _actionName;
        public ActionStatus(string actionName)
        {
            _actionName = actionName;
            if (!string.IsNullOrEmpty(actionName))            
            {
                ActionStatusManager.Instance.StartAction(actionName);
            }
        }        

        public virtual void Dispose()
        {
            if (!string.IsNullOrEmpty(_actionName))
            {
                ActionStatusManager.Instance.EndAction(_actionName);
            }
        }
    }
}
