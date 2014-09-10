namespace Utilities.Process
{
    public class BackgroundProcessOutput
    {
        public string Executable { get; set; }
        public string Arguments { get; set; }
        public string StandardOutput { get; set; }
        public string StandardError { get; set; }
        public int ExitCode { get; set; }
    }
}
