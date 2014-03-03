using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Utilities.Interfaces;
using log4net;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Allows copying of file system items with events.
    /// </summary>
    [Serializable]
    public class FileSystemCopier : IFileSystemCopier
    {
        #region Fields
        private static ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private TransferRateInfo _averageTransferRate;
        private DirectoryProgressInfo _currentDirectory;
        private FileProgressInfo _currentFile;
        private Stopwatch _fileChunkStopwatch;
        private QueueInfo _queueInfo;
        #endregion
        

        #region Properties
        /// <summary>
        /// Gets or sets whether the <see cref="FileSystemCopier"/> will stop on error and throw an exception
        /// or fire an <see cref="ErrorOccured"/> event and keep going.
        /// </summary>
        public bool TrowOnError { get; set; }
        #endregion
        

        #region Constants
        //constants that specify how the file is to be copied
        private const uint CopyFileAllowDecryptedDestination = 0x00000008;

        private const uint CopyFileFailIfExists = 0x00000001;

        private const uint CopyFileOpenSourceForWrite = 0x00000004;

        private const uint CopyFileRestartable = 0x00000002;

        /// <summary>
        ///   Callback reason passed as dwCallbackReason in CopyProgressRoutine. Indicates another part of the data file was copied.
        /// </summary>
        private const uint CallbackChunkFinished = 0;

        /// <summary>
        ///   Callback reason passed as dwCallbackReason in CopyProgressRoutine. Indicates another stream was created and is about to be copied. This is the callback reason given when the callback routine is first invoked.
        /// </summary>
        private const uint CallbackStreamSwitch = 1;

        /// <summary>
        ///   Return value of the CopyProgressRoutine. Indicates continue the copy operation.
        /// </summary>
        private const uint ProgressContinue = 0;

        /// <summary>
        ///   Return value of the CopyProgressRoutine. Indicates cancel the copy operation and delete the destination file.
        /// </summary>
        private const uint ProgressCancel = 1;

        /// <summary>
        ///   Return value of the CopyProgressRoutine. Indicates stop the copy operation. It can be restarted at a later time.
        /// </summary>
        private const uint ProgressStop = 2;

        /// <summary>
        ///   Return value of the CopyProgressRoutine. Indicates continue the copy operation, but stop invoking CopyProgressRoutine to report progress.
        /// </summary>
        private const uint ProgressQuiet = 3;
        #endregion


        #region Methods

        #region Protected
        /// <summary>
        ///   The CopyProgressRoutine delegate is an application-defined callback function used with the CopyFileEx and MoveFileWithProgress functions. It is called when a portion of a copy or move operation is completed.
        /// </summary>
        /// <param name="totalFileSize">Total size of the file, in bytes.</param>
        /// <param name="totalBytesTransferred">Total number of bytes transferred from the source file to the destination file since the copy operation began.</param>
        /// <param name="streamSize">Total size of the current file stream, in bytes.</param>
        /// <param name="streamBytesTransferred">Total number of bytes in the current stream that have been transferred from the source file to the destination file since the copy operation began.</param>
        /// <param name="dwStreamNumber">Handle to the current stream. The first time CopyProgressRoutine is called, the stream number is 1.</param>
        /// <param name="dwCallbackReason">Reason that CopyProgressRoutine was called.</param>
        /// <param name="hSourceFile">Handle to the source file.</param>
        /// <param name="hDestinationFile">Handle to the destination file.</param>
        /// <param name="lpData">Argument passed to CopyProgressRoutine by the CopyFileEx or MoveFileWithProgress function.</param>
        /// <returns> A value indicating how to proceed with the copy operation. </returns>
        protected uint CopyProgressCallback(
            long totalFileSize,
            long totalBytesTransferred,
            long streamSize,
            long streamBytesTransferred,
            uint dwStreamNumber,
            uint dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData)
        {
            if (_log.IsDebugEnabled)
            {
                _log.DebugFormat("Finished chunk for file ...");
            }
            switch (dwCallbackReason)
            {
                case CallbackChunkFinished:
                    // Another part of the file was copied.
                    // Capture the time                    
                    TimeSpan elapsedTime = _fileChunkStopwatch.Elapsed;

                    // Reset the stopwtach and keep counting
                    _fileChunkStopwatch.Reset();
                    _fileChunkStopwatch.Start();

                    // Calculcate what was copied since last time
                    long bytesCopiedSinceLastReport = totalBytesTransferred - _currentFile.BytesTransferred;
                    _currentFile.BytesTransferred = totalBytesTransferred;
                    OnFileProgressChanged(new FileProgressChangedEventArgs(_currentFile.File.FullName, totalFileSize,
                                                                           totalBytesTransferred, elapsedTime));

                    // Report a change in transfer rate
                    OnCurrentTransferRateChanged(
                        new TransferRateChangedEventArgs(new TransferRateInfo(bytesCopiedSinceLastReport, elapsedTime)));

                    // Update average transfer rate
                    _averageTransferRate.TransferTime = _averageTransferRate.TransferTime.Add(elapsedTime);
                    _averageTransferRate.BytesTransferred += bytesCopiedSinceLastReport;
                    OnAverageTransferRateChanged(
                        new TransferRateChangedEventArgs(new TransferRateInfo(_averageTransferRate.BytesTransferred,
                                                                              _averageTransferRate.TransferTime)));

                    // Report change in directory progress if we're copying a directory
                    if (_currentDirectory != null)
                    {
                        _currentDirectory.BytesTransferred += bytesCopiedSinceLastReport;
                        OnDirectoryProgressChanged(
                            new DirectoryProgressChangedEventArgs(_currentDirectory.Directory.FullName,
                                                                  _currentDirectory.Size,
                                                                  _currentDirectory.BytesTransferred));
                    }

                    if (_queueInfo != null)
                    {
                        _queueInfo.QueueBytesTransferred += bytesCopiedSinceLastReport;
                        OnQueueProgressChanged(_queueInfo);
                    }

                    //Console.WriteLine("Chunk done ...");
                    return StopPending ? ProgressCancel : ProgressContinue;

                case CallbackStreamSwitch:
                    //Console.WriteLine("Chunk done ...");
                    // A new stream was created. We don't care about this one - just continue the move operation.
                    return StopPending ? ProgressCancel : ProgressContinue;

                default:
                    //Console.WriteLine("Chunk done ...");
                    return StopPending ? ProgressCancel : ProgressContinue;
            }
        }

        #endregion


        /// <summary>
        /// Constructor.
        /// </summary>
        public FileSystemCopier()
        {
            TrowOnError = true;
        }

        /// <summary>
        /// Copies file or directory overwriting if necessary.
        /// </summary>
        /// <param name="source">Source file path or directory to copy.</param>
        /// <param name="target">Target file path or directory.</param>
        public void Copy(string source, string target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source">Source file or directory to copy.</param>
        /// <param name="target">Target file path or directory.</param>
        public void Copy(FileInfo source, string target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source">Source file to copy.</param>
        /// <param name="target">Target file path.</param>
        public void Copy(IFileInfo source, string target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source">Source file.</param>
        /// <param name="target">Target file.</param>
        public void Copy(IFileInfo source, IFileInfo target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source">Source directory to copy.</param>
        /// <param name="target">Target directory.</param>
        public void Copy(IDirectoryInfo source, string target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source">Source directory to copy.</param>
        /// <param name="target">Target directory.</param>
        public void Copy(DirectoryInfo source, string target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="source">Source directory to copy.</param>
        /// <param name="target">Target directory.</param>
        public void Copy(IDirectoryInfo source, IDirectoryInfo target)
        {
            var pair = new CopyPair(source, target);
            Copy(pair);
        }

        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="copyPair"><code>CopyPair</code> to copy.</param>
        public void Copy(CopyPair copyPair)
        {
            if (copyPair == null)
            {
                throw new ArgumentNullException("copyPair", "Copy pair is null.");
            }

            try
            {
                Initialize();

                CopyHelper(copyPair);

                CheckStopped();
            }
            catch (StoppedException)
            {
                _log.Debug("FileSystemCopier execution stopped.");
                StopInternal();
            }
            catch (Exception ex)
            {
                var source = "NULL";
                if (copyPair.SourceInfo != null)
                {
                    source = copyPair.SourceInfo.FullName;
                }
                var target = "NULL";
                if (copyPair.TargetInfo != null)
                {
                    target = copyPair.TargetInfo.FullName;
                }
                _log.ErrorFormat("Error occurred while copying {0} to {1}.", source, target);
                ReportError(copyPair, ex);
                throw;
            }
        }
        
        /// <summary>
        /// Copies file system items overwriting if necessary.
        /// </summary>
        /// <param name="items">List of <code>CopyPair</code> to copy.</param>
        public void Copy(IList<CopyPair> items)
        {
            try
            {
                if (items == null)
                {
                    throw new ArgumentNullException("items", "List of items to copy is null.");
                }
                else if (items.Count == 0)
                {
                    return;
                }

                InitializeQueue(items);

                if (QueueStarted != null)
                {
                    var args = new QueueStartedEventArgs(_queueInfo);
                    OnQueueStarted(args);
                    if (args.Cancel)
                    {
                        return;
                    }
                }

                foreach (var copyPair in items)
                {
                    CheckStopped();
                    try
                    {
                        CopyHelper(copyPair);
                        _queueInfo.ItemsCompleteCount += 1;
                        OnQueueProgressChanged(_queueInfo);
                    }
                    catch (Exception ex)
                    {
                        ReportError(copyPair, ex);
                        _log.ErrorFormat("Error copying {0} to {1}.", copyPair.SourceInfo.FullName, copyPair.TargetInfo.FullName);
                        _log.ErrorFormat(ex.Message, ex);
                    }
                    CheckStopped();
                }
                OnQueueComplete(_queueInfo);
            }
            catch (StoppedException)
            {
                _log.Debug("FileSystemCopier execution stopped.");
                StopInternal();
            }
        }

        private void Initialize()
        {
            _currentFile = null;
            _currentDirectory = null;
            _averageTransferRate = new TransferRateInfo();
            _fileChunkStopwatch = new Stopwatch();
            _stopPending = false;
        }

        private void StopInternal()
        {
            _log.Info("Operation stopped.");
            _stopPending = false;
        }

        private void CopyHelper(CopyPair copyPair)
        {
            if (copyPair.IsFile && !copyPair.SourceInfo.Exists)
            {
                throw new FileNotFoundException(
                    string.Format("Source file {0} does not exist.", copyPair.SourceInfo.FullName),
                    copyPair.SourceInfo.FullName);
            }
            else if (!copyPair.IsFile && !copyPair.SourceInfo.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Source directory {0} does not exist.",
                                                                   copyPair.SourceInfo.FullName));
            }

            // Create parent directory if it does not exist
            var targetDirectory = Path.GetDirectoryName(copyPair.TargetInfo.FullName);
            if (targetDirectory != null && !Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            if (copyPair.IsFile)
            {
                if (_log.IsDebugEnabled)
                {
                    _log.DebugFormat("Copying file {0} ...", copyPair.SourceInfo.FullName);
                }
                CopyFile((IFileInfo) copyPair.SourceInfo, (IFileInfo) copyPair.TargetInfo);
            }
            else
            {
                if (_log.IsDebugEnabled)
                {
                    _log.DebugFormat("Copying directory {0} ...", copyPair.SourceInfo.FullName);
                }
                CopyDirectory((IDirectoryInfo) copyPair.SourceInfo, (IDirectoryInfo) copyPair.TargetInfo);
            }
        }

        private void InitializeQueue(ICollection<CopyPair> items)
        {
            Initialize();
            long queueByteSize = 0;
            foreach (var copyPair in items)
            {
                try
                {
                    queueByteSize += copyPair.SourceInfo.Size;
                }
                catch (Exception ex)
                {
                    _log.ErrorFormat("Error getting size from {0}.", copyPair.SourceInfo.FullName);
                    _log.Error(ex.Message);
                    ReportError(copyPair, ex);
                }
            }
            _queueInfo = new QueueInfo
                             {
                                 ItemCount = items.Count,
                                 ItemsCompleteCount = 0,
                                 QueueByteSize = queueByteSize,
                                 QueueBytesTransferred = 0
                             };
        }

        private void ReportError(CopyPair copyPair, Exception exception)
        {
            if (TrowOnError)
            {
                throw new Exception(
                    string.Format("Error copying {0} to {1}. Error: {2}", copyPair.SourceInfo.FullName, copyPair.TargetInfo.FullName, exception.Message), exception);
            }
            var errorMessage = string.Format("Error copying {0} to {1}.", copyPair == null ? "null" : copyPair.SourceInfo.FullName, copyPair == null ? "null" : copyPair.TargetInfo.FullName);
            OnCopyErrorOccured(errorMessage, copyPair, exception);
        }

        private void CopyFile(IFileInfo source, IFileInfo target)
        {
            _currentFile = new FileProgressInfo(source);
            if (FileStarted != null)
            {
                var fileSystemItemStartedEventArgs = new FileSystemItemStartedEventArgs(source.FullName, target.FullName,
                                                                                        source.Length);
                OnFileStarted(fileSystemItemStartedEventArgs);
                if (fileSystemItemStartedEventArgs.Cancel)
                {
                    return;
                }
            }
            
            CopyFileHelper(source, target);

            if (FileComplete != null)
            {
                OnFileComplete(new FileSystemItemCompleteEventArgs(source.FullName, target.FullName, source.Length,
                                                                   _currentFile.Stopwatch.Elapsed));
            }
        }

        private void CopyFileHelper(IFileInfo sourceFile, IFileInfo destinationFile)
        {
            _currentFile = new FileProgressInfo(sourceFile);
            _currentFile.Stopwatch.Start();
            var success =
                CopyFileEx(sourceFile.FullName, destinationFile.FullName, CopyProgressCallback, IntPtr.Zero, false,
                           CopyFileAllowDecryptedDestination);
            if (success)
            {
                destinationFile.Attributes = sourceFile.Attributes;
            }
            _currentFile.Stopwatch.Stop();

            //Throw an exception if the copy failed
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                if (error != 1235)
                {
                    throw new Win32Exception(error);
                }
            }
        }

        private void CopyDirectory(IDirectoryInfo source, IDirectoryInfo target)
        {
            _currentDirectory = new DirectoryProgressInfo(source, source.Size);
            if (DirectoryStarted != null)
            {
                var fileSystemItemStartedEventArgs = new FileSystemItemStartedEventArgs(_currentDirectory.Directory.FullName, target.FullName, _currentDirectory.Size);
                OnDirectoryStarted(fileSystemItemStartedEventArgs);
                if (fileSystemItemStartedEventArgs.Cancel)
                {
                    return;
                }
            }
            
            _currentDirectory.Stopwatch.Start();
            CopyDirectoryHelper(source, target);
            _currentDirectory.Stopwatch.Stop();

            if (DirectoryComplete != null)
            {
                OnDirectoryComplete(new FileSystemItemCompleteEventArgs(_currentDirectory.Directory.FullName,
                                                                        target.FullName,
                                                                        _currentDirectory.Size,
                                                                        _currentDirectory.Stopwatch.Elapsed));
            }
        }

        private void CopyDirectoryHelper(IDirectoryInfo source, IDirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (!target.Exists)
            {
                try
                {
                    Directory.CreateDirectory(target.FullName);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to create directory: {0}.", target.FullName), ex);
                }
            }

            // Copy each file into its new directory.
            IFileInfo[] files = source.GetFiles();
            foreach (IFileInfo file in files)
            {
                // Check if operation was cancelled
                CheckStopped();
                CopyFile(file, new FileInfoWrapper(Path.Combine(target.FullName, file.Name)));
            }

            // Copy each subdirectory using recursion.
            IDirectoryInfo[] subdirectories = source.GetDirectories();
            foreach (IDirectoryInfo sourceSubdirectory in subdirectories)
            {
                // Check if operation was cancelled
                CheckStopped();

                IDirectoryInfo targetSubdirectory =
                    new DirectoryInfoWrapper(Path.Combine(target.FullName, sourceSubdirectory.Name));
                if (!targetSubdirectory.Exists)
                {
                    try
                    {
                        targetSubdirectory.Create();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            string.Format("Failed to create directory: {0}.", targetSubdirectory.FullName), ex);
                    }
                }
                CopyDirectoryHelper(sourceSubdirectory, targetSubdirectory);
            }
            target.Attributes = source.Attributes;
            target.LastWriteTime = source.LastWriteTime;
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
        /// <summary>
        /// 
        /// </summary>
        public event FileProgressChangedEventHandler FileProgressChanged;
        protected void OnFileProgressChanged(FileProgressChangedEventArgs e)
        {
            if (FileProgressChanged != null)
            {
                FileProgressChanged(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event FileStartedEventHandler FileStarted;
        protected void OnFileStarted(FileSystemItemStartedEventArgs e)
        {
            if (FileStarted != null)
            {
                FileStarted(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event FileCompleteEventHandler FileComplete;
        protected void OnFileComplete(FileSystemItemCompleteEventArgs e)
        {
            if (FileComplete != null)
            {
                FileComplete(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event DirectoryProgressChangedEventHandler DirectoryProgressChanged;
        protected void OnDirectoryProgressChanged(DirectoryProgressChangedEventArgs e)
        {
            if (DirectoryProgressChanged != null)
            {
                DirectoryProgressChanged(this, e);
                if (e.Cancel)
                {
                    throw new StoppedException();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event DirectoryStartedEventHandler DirectoryStarted;
        protected void OnDirectoryStarted(FileSystemItemStartedEventArgs e)
        {
            if (DirectoryStarted != null)
            {
                DirectoryStarted(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event DirectoryCompleteEventHandler DirectoryComplete;
        protected void OnDirectoryComplete(FileSystemItemCompleteEventArgs e)
        {
            if (DirectoryComplete != null)
            {
                DirectoryComplete(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event CurrentTransferRateChangedEventHandler CurrentTransferRateChanged;
        protected void OnCurrentTransferRateChanged(TransferRateChangedEventArgs e)
        {
            if (CurrentTransferRateChanged != null)
            {
                CurrentTransferRateChanged(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event AverageTransferRateChangedEventHandler AverageTransferRateChanged;
        protected void OnAverageTransferRateChanged(TransferRateChangedEventArgs e)
        {
            if (AverageTransferRateChanged != null)
            {
                AverageTransferRateChanged(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event QueueStartedEventHandler QueueStarted;
        protected void OnQueueStarted(QueueStartedEventArgs e)
        {
            if (QueueStarted != null)
            {
                QueueStarted(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event QueueProgressChangedEventHandler QueueProgressChanged;
        protected void OnQueueProgressChanged(QueueInfo queueInfo)
        {
            if (QueueProgressChanged != null)
            {
                var e = new QueueProgressChangedEventArgs(queueInfo);
                QueueProgressChanged(this, e);
            }

            if (ProgressChanged != null)
            {
                var e = new EventArgs.ProgressChangedEventArgs((int)(queueInfo.QueueBytesTransferred * 100 / Math.Max(1, queueInfo.QueueByteSize)));
                OnProgressChanged(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event QueueCompleteEventHandler QueueComplete;
        protected void OnQueueComplete(QueueInfo queueInfo)
        {
            if (QueueComplete != null)
            {
                var e = new QueueCompleteEventArgs(queueInfo);
                QueueComplete(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public event CopyErrorOccuredEventHandler ErrorOccured;
        protected void OnCopyErrorOccured(string errorMessage, CopyPair copyPair, Exception ex)
        {
            if (ErrorOccured != null)
            {
                var e = new CopyErrorEventArgs(errorMessage, copyPair, ex);
                ErrorOccured(this, e);
            }
        }

        #region IProgressReporter
        public event Interfaces.ProgressChangedEventHandler ProgressChanged;
        protected void OnProgressChanged(EventArgs.ProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }
        #endregion

        #endregion


        #region P/Invoke

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CopyFileEx(
            string lpExistingFileName,
            string lpNewFileName,
            CopyProgressRoutine lpProgressRoutine,
            IntPtr lpData,
            bool pbCancel,
            uint dwCopyFlags
            );

        #endregion
        

        #region IStoppable Members
        private bool _stopPending;

        /// <summary>
        /// Gets whether a stop is pending.
        /// </summary>
        public bool StopPending { get { return _stopPending; } }

        /// <summary>
        /// Stops the operation.
        /// </summary>
        public void Stop()
        {
            _stopPending = true;
        }
        #endregion


        #region Nested type: CopyProgressRoutine
        /// <summary>
        ///   The CopyProgressRoutine delegate is an application-defined callback function used with the CopyFileEx and MoveFileWithProgress functions. It is called when a portion of a copy or move operation is completed.
        /// </summary>
        private delegate uint CopyProgressRoutine(
            long TotalFileSize,
            long TotalBytesTransferred,
            long StreamSize,
            long StreamBytesTransferred,
            uint dwStreamNumber,
            uint dwCallbackReason,
            IntPtr hSourceFile,
            IntPtr hDestinationFile,
            IntPtr lpData);

        #endregion
    }
    
    public delegate void CopyErrorOccuredEventHandler(object sender, CopyErrorEventArgs e);

    public delegate void FileProgressChangedEventHandler(object sender, FileProgressChangedEventArgs e);

    public delegate void FileStartedEventHandler(object sender, FileSystemItemStartedEventArgs e);

    public delegate void FileCompleteEventHandler(object sender, FileSystemItemCompleteEventArgs e);

    public delegate void DirectoryProgressChangedEventHandler(object sender, DirectoryProgressChangedEventArgs e);

    public delegate void DirectoryStartedEventHandler(object sender, FileSystemItemStartedEventArgs e);

    public delegate void DirectoryCompleteEventHandler(object sender, FileSystemItemCompleteEventArgs e);

    public delegate void QueueStartedEventHandler(object sender, QueueStartedEventArgs e);

    public delegate void QueueProgressChangedEventHandler(object sender, QueueProgressChangedEventArgs e);

    public delegate void QueueCompleteEventHandler(object sender, QueueCompleteEventArgs e);

    public delegate void CurrentTransferRateChangedEventHandler(object sender, TransferRateChangedEventArgs e);

    public delegate void AverageTransferRateChangedEventHandler(object sender, TransferRateChangedEventArgs e);
}