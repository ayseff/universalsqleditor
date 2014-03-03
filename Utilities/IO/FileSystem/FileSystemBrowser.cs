using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utilities.EventArgs;
using Utilities.Interfaces;
using log4net;
using ErrorEventArgs = Utilities.EventArgs.ErrorEventArgs;


namespace Utilities.IO.FileSystem
{
    public class FileSystemBrowser : IFileSystemBrowser
    {
        #region Fields
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string[] _defautExclussions = new[]
                                                                  {
                                                                      "$Recycle.Bin",
                                                                      "System Volume Information",
                                                                      "RECYCLER",
                                                                      "RECYCLED"
                                                                  };

        private int _currentDirectoryCount;
        private int _totalDirectoryCount;
        #endregion


        #region IFileSystemBrowser Members
        /// <summary>
        ///   Gets or sets whether hidden files and directories will be scanned.
        /// </summary>
        public bool ScanHidden { get; set; }

        /// <summary>
        ///   Gets or sets whether system files and directories will be scanned.
        /// </summary>
        public bool ScanSystem { get; set; }

        /// <summary>
        /// Gets or sets include filters.
        /// </summary>
        public List<string> IncludeFilters { get; set; }

        /// <summary>
        /// Gets or gets exclude filters.
        /// </summary>
        public List<string> ExcludeFilters { get; set; }
        #endregion


        #region IStoppable Members
        /// <summary>
        ///   Gets or sets whether the stop is pending.
        /// </summary>
        public bool StopPending { get; set; }
        #endregion


        #region Business Methods
        /// <summary>
        ///   Created a new instance of <code>FileSystemBrowser</code> .
        /// </summary>
        public FileSystemBrowser()
        {
            ScanHidden = true;
            ScanSystem = true;
            StopPending = false;
            IncludeFilters = new List<string>();
            ExcludeFilters = new List<string>();
        }

        #region IFileSystemBrowser Members
        /// <summary>
        ///   Scans a specified path and collects all files and directories that are under the path.
        /// </summary>
        /// <param name="path"> Path name to scan. </param>
        /// <param name="files"> Collection to which all files will be added. </param>
        /// <param name="directories"> Collection to which all directories will be added. </param>
        public void Scan(string path, ICollection<IFileInfo> files, ICollection<IDirectoryInfo> directories)
        {
            if (path == null)
            {
                _log.Error("Path is null.");
                throw new ArgumentNullException("path", "Path cannot be null.");
            }
            var directory = new DirectoryInfoWrapper(path);
            Scan(directory, files, directories);
        }

        #endregion

        #region IStoppable Members

        /// <summary>
        ///   Sends a stop signal to the <code>FileSystemBrowser</code> . As soon as the current operation is complete, the process will stop.
        /// </summary>
        public void Stop()
        {
            _log.Info("Stopping ...");
            StopPending = true;
            OnActionChanged("Stopping ...");
        }

        #endregion

        private void Init()
        {
            _log.Debug("Initializing file system browser ...");
            _totalDirectoryCount = 0;
            _currentDirectoryCount = 0;
            StopPending = false;

            OnActionChanged("Initializing file system browser ...");
            _log.Debug("Initialization complete.");
        }

        /// <summary>
        ///   Scans a specified path and collects all files and directories that are under the path.
        /// </summary>
        /// <param name="directory"> Directory to scan. </param>
        /// <param name="files"> Collection to which all files will be added. </param>
        /// <param name="directories"> Collection to which all directories will be added. </param>
        public void Scan(IDirectoryInfo directory, ICollection<IFileInfo> files, ICollection<IDirectoryInfo> directories)
        {
            if (directory == null)
            {
                _log.Error("Path is null.");
                throw new ArgumentNullException("directory", "Path cannot be null.");
            }
            else if (!directory.Exists)
            {
                _log.ErrorFormat("Path {0} does not exist.", directory.FullName);
                throw new DirectoryNotFoundException(string.Format("Path: {0} does not exist.", directory.FullName));
            }
            else if (files == null)
            {
                _log.Error("Files collection is null.");
                throw new ArgumentNullException("files", "Files cannot be null.");
            }
            else if (directories == null)
            {
                _log.Error("Directories collection is null.");
                throw new ArgumentNullException("directories", "Directories cannot be null.");
            }

            files.Clear();
            directories.Clear();
            Init();
            _log.InfoFormat("Scanning directory: {0} ...", directory);
            OnActionChanged("Calculating ...");
            try
            {
                _totalDirectoryCount = GetSubDirectoryCount(directory);
                ScanHelper(directory, files, directories, -1);
                _log.InfoFormat("Scanning of directory {0} complete.", directory.FullName);
            }
            catch (StoppedException)
            {
                _log.Info("File system browser stopped.");
                OnActionChanged("Stopped.");
                StopPending = false;
            }
            catch (Exception ex)
            {
                var message = string.Format("Error scanning directory {0}", directory.FullName);
                _log.ErrorFormat(message);
                _log.Error(ex.Message, ex);
                OnErrorOccured(string.Format("{0}. {1}.", message, ex.Message), ex);
            }
        }

        private void ScanHelper(IDirectoryInfo directory, ICollection<IFileInfo> files,
                                ICollection<IDirectoryInfo> directories, int level)
        {
            OnActionChanged(string.Format("Scanning {0} ...", directory.FullName));
            if (level <= 0)
            {
                if (level == 0)
                {
                    ++_currentDirectoryCount;
                }
                int progressPercentage = _currentDirectoryCount*100/
                                         (_totalDirectoryCount == 0
                                              ? 1
                                              : _totalDirectoryCount);
                OnProgressChanged(progressPercentage);
            }

            try
            {
                try
                {
                    IFileInfo[] fileInfos = directory.GetFiles();
                    foreach (var fileInfo in fileInfos)
                    {
                        CheckStopped();
                        if (ShouldAddFile(fileInfo))
                        {
                            files.Add(fileInfo);
                        }
                    }
                }
                catch (StoppedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    var message = string.Format("Error scanning for files in directory {0}", directory.FullName);
                    _log.ErrorFormat(message);
                    _log.Error(ex.Message, ex);
                    OnErrorOccured(string.Format("{0}. {1}.", message, ex.Message), ex);
                }

                try
                {
                    IDirectoryInfo[] directoryInfos = directory.GetDirectories();
                    foreach (var directoryInfo in directoryInfos)
                    {
                        CheckStopped();
                        if (ShouldBrowseDirectory(directoryInfo))
                        {
                            directories.Add(directoryInfo);
                            ScanHelper(directoryInfo, files, directories, level + 1);
                        }
                    }
                }
                catch (StoppedException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    string message = string.Format("Error scanning for subdirectories in directory {0}",
                                                   directory.FullName);
                    _log.ErrorFormat(message);
                    _log.Error(ex.Message, ex);
                    OnErrorOccured(string.Format("{0}. {1}.", message, ex.Message), ex);
                }
            }
            catch (StoppedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                var message = string.Format("Error while scanning directory {0}. {1}.", directory.FullName, ex.Message);
                _log.ErrorFormat(message);
                _log.Error(ex.Message, ex);
                OnErrorOccured(message, ex);
            }
        }

        private bool ShouldAddFile(IFileSystemInfo fileInfo)
        {
            var result = true;
            if (!ScanHidden && (fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                _log.DebugFormat("Skipping scanning file {0}.", fileInfo.FullName);
                result = false;
            }
            else if (!ScanSystem && (fileInfo.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                _log.DebugFormat("Skipping scanning file {0}.", fileInfo.FullName);
                result = false;
            }
            else if (IncludeFilters.Count > 0 && !FindFilesRegEx.MatchesFilePattern(fileInfo.FullName, IncludeFilters.ToArray()))
            {
                return false;
            }
            else if (ExcludeFilters.Count > 0 && FindFilesRegEx.MatchesFilePattern(fileInfo.FullName, ExcludeFilters.ToArray()))
            {
                return false;
            }
            return result;
        }

        private bool ShouldBrowseDirectory(IDirectoryInfo directory)
        {
            var result = true;
            if (!directory.Exists)
            {
                _log.DebugFormat("Skipping scanning directory {0}.", directory.FullName);
                result = false;
            }
            else if (directory.Parent == null)
            {
// ReSharper disable RedundantAssignment
                result = true;
// ReSharper restore RedundantAssignment
            }
            else if (_defautExclussions.Contains(directory.Name)
                     && (directory.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                _log.DebugFormat("Skipping scanning directory {0} because it is directory that is a part of default exclusion.", directory.FullName);
                result = false;
            }
            else if ((directory.Attributes & FileAttributes.Offline) == FileAttributes.Offline
                     || (directory.Attributes & FileAttributes.Device) == FileAttributes.Device)
            {
                _log.DebugFormat("Skipping scanning directory {0} because it is a Offline or a Device.", directory.FullName);
                result = false;
            }
            else if (!ScanSystem && (directory.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                _log.DebugFormat("Skipping scanning directory {0} because it is a System directory.", directory.FullName);
                result = false;
            }
            else if (!ScanHidden && (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                _log.DebugFormat("Skipping scanning directory {0} because it is a Hidden directory.", directory.FullName);
                result = false;
            }

            if (IncludeFilters.Count > 0 && !FindFilesRegEx.MatchesFilePattern(directory.FullName, IncludeFilters.ToArray()))
            {
                return false;
            }

            if (ExcludeFilters.Count > 0 && FindFilesRegEx.MatchesFilePattern(directory.FullName, ExcludeFilters.ToArray()))
            {
                return false;
            }
            return result;
        }


        private int GetSubDirectoryCount(IDirectoryInfo directory)
        {
            _log.DebugFormat("Scanning {0} for directory count ...", directory.FullName);
            int count = 0;
            IDirectoryInfo[] directories = directory.GetDirectories();
            foreach (var directoryInfo in directories)
            {
                CheckStopped();

                if (_log.IsDebugEnabled)
                {
                    _log.DebugFormat("Found directory {0}.", directoryInfo.FullName);
                }

                if (ShouldBrowseDirectory(directoryInfo))
                {
                    ++count;
                }
            }
            _log.DebugFormat("Found {0} directories.", count.ToString("#,0"));
            return count;
        }

        private void CheckStopped()
        {
            if (StopPending)
            {
                throw new StoppedException();
            }
        }

        #endregion


        #region Events

        #region IActionReporter Members

        public event ActionChangedEventHandler ActionChanged;

        #endregion

        #region IErrorReporter Members

        public event ErrorOccuredEventHandler ErrorOccured;

        #endregion

        #region IProgressReporter Members

        public event ProgressChangedEventHandler ProgressChanged;

        #endregion

        private void OnErrorOccured(string errorMessage, Exception e)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured(this, new ErrorEventArgs(errorMessage, e));
            }
        }

        private void OnActionChanged(string actionName)
        {
            if (ActionChanged != null)
            {
                ActionChanged(this, new ActionChangedEventArgs(actionName));
            }
        }

        private void OnProgressChanged(int progressPercentage)
        {
            if (ProgressChanged != null)
            {
                if (progressPercentage > 100)
                {
                    progressPercentage = 100;
                }
                ProgressChanged(this, new ProgressChangedEventArgs(progressPercentage));
            }
        }

        #endregion
    }
}