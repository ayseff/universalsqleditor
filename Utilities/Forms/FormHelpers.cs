using System.Windows.Forms;

namespace Utilities.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public static class FormHelpers
    {
        /// <summary>
        /// Invokes the specified action on a UI thread if necessary.
        /// </summary>
        /// <param name="control">Control on which the action is being performed.</param>
        /// <param name="action">Action to perform.</param>
        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
