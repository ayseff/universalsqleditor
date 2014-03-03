using System.Windows.Forms;

namespace Utilities.Forms
{
    public interface IStatusReceiver
    {
        string StatusLabel { get; set; }
        int ProgressPercentage { get; set; }
        Cursor Cursor { get; set; }
    }
}
