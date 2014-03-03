using System.Collections.Generic;
using Utilities.Interfaces;

namespace Utilities.IO.Sync
{
    public interface ISyncResolver : IStoppable, IActionReporter, IProgressReporter
    {
    //    ISyncOptions SyncOptions { get; set; }
        IList<SyncOperation> ResolveSync(IDirectoryInfo source, IList<IDirectoryInfo> sourceDirectories,
                                         IList<IFileInfo> sourceFiles, IDirectoryInfo target,
                                         IList<IDirectoryInfo> targetDirectories, IList<IFileInfo> targetFiles,
                                         IEnumerable<string> filters = null);
    }
}