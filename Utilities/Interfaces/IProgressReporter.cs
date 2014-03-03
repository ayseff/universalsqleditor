using Utilities.EventArgs;

namespace Utilities.Interfaces
{
    public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);

    public interface IProgressReporter
    {
        event ProgressChangedEventHandler ProgressChanged;
    }
}