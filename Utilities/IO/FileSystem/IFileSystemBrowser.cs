using System.Collections.Generic;
using Utilities.Interfaces;

namespace Utilities.IO.FileSystem
{
    public interface IFileSystemBrowser : IStoppable, IActionReporter, IProgressReporter, IErrorReporter
    {
        /// <summary>
        ///   Gets or sets whether hidden files and directories will be scanned.
        /// </summary>
        bool ScanHidden { get; set; }

        /// <summary>
        ///   Gets or sets whether system files and directories will be scanned.
        /// </summary>
        bool ScanSystem { get; set; }

        /// <summary>
        /// Gets or sets include filters.
        /// </summary>
        List<string> IncludeFilters { get; set; }

        /// <summary>
        /// Gets or gets exclude filters.
        /// </summary>
        List<string> ExcludeFilters { get; set; }

        /// <summary>
        ///   Scans a specified path and collects all files and directories that are under the path.
        /// </summary>
        /// <param name="path"> Path name to scan. </param>
        /// <param name="files"> Collection to which all files will be added. </param>
        /// <param name="directories"> Collection to which all directories will be added. </param>
        void Scan(string path, ICollection<IFileInfo> files, ICollection<IDirectoryInfo> directories);
    }
}