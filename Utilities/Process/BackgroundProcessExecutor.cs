using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;

namespace Utilities.Process
{
    public class BackgroundProcessExecutor : IBackgroundProcessExecutor
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BackgroundProcessOutput RunBackgroundProcess(string executable, string arguments, DataReceivedEventHandler pOnOutputDataReceived = null, DataReceivedEventHandler pOnErrorDataReceived = null)
        {
            if (executable == null) throw new ArgumentNullException("executable");
            _log.DebugFormat("Starting process {0} with arguments {1}.", executable, arguments);
            var stdOutBuffer = new StringBuilder();
            var stdErrBuffer = new StringBuilder();
            var p = new System.Diagnostics.Process();
            var psi = new ProcessStartInfo(executable, arguments ?? string.Empty);
            p.StartInfo = psi;
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.CreateNoWindow = true;
            if (pOnOutputDataReceived != null)
            {
                p.OutputDataReceived += (sender, e) =>
                                        {
                                            if (e.Data == null)
                                            {
                                                return;
                                            }
                                            _log.Debug(e.Data);
                                            stdOutBuffer.AppendLine(e.Data);
                                        };
                p.OutputDataReceived += pOnOutputDataReceived;
            }
            if (pOnErrorDataReceived != null)
            {
                p.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        return;
                    }
                    _log.Debug(e.Data);
                    stdErrBuffer.AppendLine(e.Data);
                };
                p.ErrorDataReceived += pOnErrorDataReceived;
            }

            p.Start();
            if (pOnOutputDataReceived != null)
            {
                p.BeginOutputReadLine();
            }
            else
            {
                stdOutBuffer.Append(p.StandardOutput.ReadToEnd());
            }
            if (pOnErrorDataReceived != null)
            {
                p.BeginErrorReadLine();
            }
            else
            {
                stdErrBuffer.Append(p.StandardError.ReadToEnd());
            }

            p.WaitForExit();
            _log.Debug("Process finished.");
            var commandOutput = new BackgroundProcessOutput
                                {
                                    Executable = executable,
                                    Arguments = arguments,
                                    StandardOutput = stdOutBuffer.ToString(),
                                    StandardError = stdErrBuffer.ToString(),
                                    ExitCode = p.ExitCode
                                };
            _log.DebugFormat("Process finished with exit code {0}.", p.ExitCode);
            return commandOutput;
        }

        public void VerifyExitCode(BackgroundProcessOutput backgroundProcessOutput, params int[] successExitCodes)
        {
            if (successExitCodes.Contains(backgroundProcessOutput.ExitCode)) return;
            var message = string.Format("Process {0} returned error code {1}.", backgroundProcessOutput.Executable, backgroundProcessOutput.ExitCode);
            _log.ErrorFormat(message);
            _log.ErrorFormat("Standard Output: {0}.", backgroundProcessOutput.StandardOutput);
            _log.ErrorFormat("Standard Error: {0}.", backgroundProcessOutput.StandardError);
            throw new Exception(message);
        }
    }
}
