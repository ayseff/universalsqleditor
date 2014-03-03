using System.Diagnostics;

namespace Utilities.IO.Copying
{
    internal class FileProgressInfo
    {
        public FileProgressInfo(IFileInfo file)
        {
            File = file;
            Stopwatch = new Stopwatch();
        }

        public IFileInfo File { get; set; }
        public long BytesTransferred { get; set; }
        public Stopwatch Stopwatch { get; protected set; }
    }
}