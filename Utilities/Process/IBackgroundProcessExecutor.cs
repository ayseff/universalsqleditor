using System.Diagnostics;

namespace Utilities.Process
{
    public interface IBackgroundProcessExecutor
    {
        BackgroundProcessOutput RunBackgroundProcess(string executable, string arguments, DataReceivedEventHandler pOnOutputDataReceived = null, DataReceivedEventHandler pOnErrorDataReceived = null);
        void VerifyExitCode(BackgroundProcessOutput backgroundProcessOutput, params int[] successExitCodes);
    }
}