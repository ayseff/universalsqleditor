using System.Collections.Generic;
using System.IO;
using Utilities.Interfaces;

namespace Utilities.IO.Copying
{
    public interface IFileSystemCopier : IStoppable, IProgressReporter
    {
        /// <summary>
        ///   Copies file or directory overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source file path or directory to copy. </param>
        /// <param name="target"> Target file path or directory. </param>
        void Copy(string source, string target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source file or directory to copy. </param>
        /// <param name="target"> Target file path or directory. </param>
        void Copy(FileInfo source, string target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source file to copy. </param>
        /// <param name="target"> Target file path. </param>
        void Copy(IFileInfo source, string target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source file. </param>
        /// <param name="target"> Target filey. </param>
        void Copy(IFileInfo source, IFileInfo target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source directory to copy. </param>
        /// <param name="target"> Target directory. </param>
        void Copy(IDirectoryInfo source, string target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source directory to copy. </param>
        /// <param name="target"> Target directory. </param>
        void Copy(DirectoryInfo source, string target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source"> Source directory to copy. </param>
        /// <param name="target"> Target directory. </param>
        void Copy(IDirectoryInfo source, IDirectoryInfo target);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="copyPair"><code>CopyPair</code> to copy.</param>
        void Copy(CopyPair copyPair);

        /// <summary>
        ///   Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="items"> List of <code>CopyPair</code> to copy. </param>
        void Copy(IList<CopyPair> items);

        event FileProgressChangedEventHandler FileProgressChanged;
        event FileStartedEventHandler FileStarted;
        event FileCompleteEventHandler FileComplete;
        event DirectoryProgressChangedEventHandler DirectoryProgressChanged;
        event DirectoryStartedEventHandler DirectoryStarted;
        event DirectoryCompleteEventHandler DirectoryComplete;
        event CurrentTransferRateChangedEventHandler CurrentTransferRateChanged;
        event AverageTransferRateChangedEventHandler AverageTransferRateChanged;
        event QueueStartedEventHandler QueueStarted;
        event QueueProgressChangedEventHandler QueueProgressChanged;
        event QueueCompleteEventHandler QueueComplete;

        /// <summary>
        /// Gets or sets whether the <code>FileSystemCopier</code> will stop on error and throw an exception
        /// or keep going and fire an <code>ErrorOccured</code> event.
        /// </summary>
        bool TrowOnError { get; set; }
    }
}