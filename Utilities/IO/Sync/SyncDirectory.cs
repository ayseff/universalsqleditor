using System.Collections.Generic;
using Utilities.IO.FileSystem;

namespace Utilities.IO.Sync
{
    public class SyncDirectory
    {
        public SyncDirectory(IDirectoryInfo directory, IList<IDirectoryInfo> subdirectories, IList<IFileInfo> files)
        {
            BaseDirectory = directory;
            Directories = subdirectories;
            Files = files;
        }

        public IDirectoryInfo BaseDirectory { get; set; }
        public IList<IDirectoryInfo> Directories { get; set; }
        public IList<IFileInfo> Files { get; set; }
    }
}