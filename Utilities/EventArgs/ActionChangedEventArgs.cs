#region

using System;

#endregion

namespace Utilities.EventArgs
{
    public class ActionChangedEventArgs : System.EventArgs
    {
        private readonly string _actionName;

        public ActionChangedEventArgs(string actionName)
        {
            if (actionName == null)
            {
                throw new ArgumentNullException("actionName", "Action name cannot be null.");
            }
            _actionName = actionName;
        }

        public string ActionName
        {
            get { return _actionName; }
        }
    }
}