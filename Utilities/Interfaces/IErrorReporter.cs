using Utilities.EventArgs;

namespace Utilities.Interfaces
{
    public delegate void ErrorOccuredEventHandler(object sender, ErrorEventArgs e);

    public interface IErrorReporter
    {
        event ErrorOccuredEventHandler ErrorOccured;
    }
}