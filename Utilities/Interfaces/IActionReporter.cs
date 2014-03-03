using Utilities.EventArgs;

namespace Utilities.Interfaces
{
    public delegate void ActionChangedEventHandler(object sender, ActionChangedEventArgs e);

    public interface IActionReporter
    {
        event ActionChangedEventHandler ActionChanged;
    }
}