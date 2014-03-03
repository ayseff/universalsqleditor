using System.Diagnostics;

namespace Utilities.IO.Copying
{
    public class DirectoryProgressInfo
    {
        public DirectoryProgressInfo(IDirectoryInfo directory, long size)
        {
            Directory = directory;
            Size = size;
            Stopwatch = new Stopwatch();
        }

        public IDirectoryInfo Directory { get; set; }
        public long Size { get; set; }
        public long BytesTransferred { get; set; }
        public Stopwatch Stopwatch { get; protected set; }
    }
}