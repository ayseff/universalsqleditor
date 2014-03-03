using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Utilities.EventArgs;
using Utilities.IO.Copying;
using Utilities.Interfaces;
using log4net;
using ErrorEventArgs = Utilities.EventArgs.ErrorEventArgs;

namespace Utilities.IO.Sync
{
    public class SyncOperationRunner : ISyncOperationRunner
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFileSystemCopier _fileSystemCopier;
        public long BytesToCopy { get; private set; }
        public long BytesCopied { get; private set; }

        public SyncOperationRunner(IFileSystemCopier fileSystemCopier)
        {
            if (fileSystemCopier == null) throw new ArgumentNullException("fileSystemCopier");

            _fileSystemCopier = fileSystemCopier;
            _fileSystemCopier.FileStarted +=
                (sender, args) =>
                OnActionChanged(string.Format("Copying file {0}", args.SourceFullName));
            _fileSystemCopier.TrowOnError = true;
        }

        public void RunOperations(IList<SyncOperation> operations)
        {
            if (operations == null) throw new ArgumentNullException("operations");

            try
            {
                int percentageProgress = 0;
                _log.InfoFormat("Running {0} operations.", operations.Count.ToString("#,0"));
                OnActionChanged("Performing sync ...");

                // Delete file operations
                _log.Debug("Deleting files ...");
                int allocatedPercentage = 5;
                var deleteFiles =
                    operations.Where(o => o.OperationType == SyncOperationType.DeleteSource && o.IsFile).Select(o => o.Source.FullName).ToList();
                deleteFiles.AddRange(operations.Where(o => o.OperationType == SyncOperationType.DeleteTarget && o.IsFile).Select(o => o.Target.FullName).ToList());
                RunDeleteOperations(allocatedPercentage, percentageProgress, deleteFiles, true);
                percentageProgress += allocatedPercentage;

                // Delete directory operations
                _log.Debug("Deleting directories ...");
                allocatedPercentage = 4;
                var deleteDirectories =
                    operations.Where(o => o.OperationType == SyncOperationType.DeleteSource && !o.IsFile).Select(o => o.Source.FullName).ToList();
                deleteDirectories.AddRange(operations.Where(o => o.OperationType == SyncOperationType.DeleteTarget && !o.IsFile).Select(o => o.Target.FullName).ToList());
                RunDeleteOperations(allocatedPercentage, percentageProgress, deleteDirectories, false);
                percentageProgress += allocatedPercentage;

                // Create directories
                _log.Debug("Creating directories ...");
                allocatedPercentage = 1;
                var createDirectories =
                    operations.Where(o => (o.OperationType == SyncOperationType.CopyToSource || o.OperationType == SyncOperationType.CopyToTarget) && !o.IsFile).ToList();
                for (var i = 0; i < createDirectories.Count; i++)
                {
                    CheckStopped();
                    try
                    {
                        var sourceDirectory = (IDirectoryInfo)createDirectories[i].Source;
                        var targetDirectory = (IDirectoryInfo)createDirectories[i].Target;
                        if (createDirectories[i].OperationType == SyncOperationType.CopyToSource)
                        {
                            sourceDirectory = (IDirectoryInfo)createDirectories[i].Target;
                            targetDirectory = (IDirectoryInfo)createDirectories[i].Source;
                        }
                        var actionName = string.Format("Creating directory {0}", createDirectories[i]);
                        _log.Info(actionName);
                        OnActionChanged(actionName);
                        targetDirectory.Create();
                        targetDirectory.Attributes = sourceDirectory.Attributes;
                        targetDirectory.LastWriteTime = sourceDirectory.LastWriteTime;
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error creating directory file {0}", createDirectories[i]);
                        _log.Error(message);
                        _log.Error(ex.Message, ex);
                        OnErrorOccured(message, ex);
                    }
                    OnProgressChanged(percentageProgress + (i + 1) * allocatedPercentage / createDirectories.Count);
                }
                percentageProgress += allocatedPercentage;

                // Copy files
                _log.Debug("Copying files ...");
                allocatedPercentage = 90;
                var copyFiles =
                    operations.Where(o => (o.OperationType == SyncOperationType.CopyToTarget || o.OperationType == SyncOperationType.CopyToSource) && o.IsFile).ToList();
                var copyToTargetFiles = copyFiles.Where(o => o.OperationType == SyncOperationType.CopyToTarget).ToList();
                var copyToSourceFiles = copyFiles.Where(o => o.OperationType == SyncOperationType.CopyToSource).ToList();
                BytesToCopy = copyToTargetFiles.Sum(o => o.Source.Size) + copyToSourceFiles.Sum(o => o.Target.Size);
                BytesCopied = 0;
                foreach (var operation in copyFiles)
                {
                    CheckStopped();
                    string sourceFile, targetFile;
                    long operationSize;
                    if (operation.OperationType == SyncOperationType.CopyToTarget)
                    {
                        sourceFile = operation.Source.FullName;
                        targetFile = operation.Target.FullName;
                        operationSize = operation.Source.Size;
                    }
                    else
                    {
                        sourceFile = operation.Target.FullName;
                        targetFile = operation.Source.FullName;
                        operationSize = operation.Target.Size;
                    }
                    try
                    {
                        var actionName = string.Format("Copying {0} to {1}", sourceFile, targetFile);
                        _log.Info(actionName);
                        OnActionChanged(actionName);
                        _fileSystemCopier.Copy(sourceFile, targetFile);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("Error copying {0} to {1}", sourceFile, targetFile);
                        _log.Error(message);
                        _log.Error(ex.Message, ex);
                        OnErrorOccured(message, ex);
                    }
                    BytesCopied += operationSize;
                    if (BytesToCopy != 0)
                    {
                        OnProgressChanged(percentageProgress + (int)(BytesCopied * allocatedPercentage / BytesToCopy));
                    }
                }
                OnProgressChanged(100);
            }
            catch (StoppedException)
            {
                _log.Info("Operation stopped.");
                OnActionChanged("Stopped.");
                StopPending = false;
            }
        }

        

        private void CheckStopped()
        {
            if (StopPending)
            {
                throw new StoppedException();
            }
        }

        private void RunDeleteOperations(int allocatedPercentage, int percentageProgress, IList<string> deletePaths, bool isFile)
        {
            for (var i = 0; i < deletePaths.Count; i++)
            {
                CheckStopped();
                var deletePath = deletePaths[i];
                OnActionChanged(string.Format("Deleting {0}", deletePath));
                if (isFile && File.Exists(deletePath))
                {
                    try
                    {
                        _log.InfoFormat("Deleting file {0}", deletePath);
                        File.Delete(deletePath);
                    }
                    catch (Exception ex1)
                    {
                        // Attempt to delete after stripping attibutes
                        _log.WarnFormat("Error deleting file {0}.", deletePath);
                        _log.Warn(ex1.Message, ex1);
                        _log.Warn("Attempting by stripping attributes ...");
                        try
                        {
                            File.SetAttributes(deletePath, FileAttributes.Normal);
                            File.Delete(deletePath);
                        }
                        catch (Exception ex)
                        {
                            var message = string.Format("Error deleting file {0}", deletePath);
                            _log.Error(message);
                            _log.Error(ex.Message, ex);
                            OnErrorOccured(message, ex);
                        }
                    }
                }
                else if (!isFile && Directory.Exists(deletePath))
                {
                    try
                    {
                        _log.InfoFormat("Deleting directory {0}", deletePath);
                        Directory.Delete(deletePath, true);
                    }
                    catch (Exception ex1)
                    {
                        // Attempt to delete one by one
                        _log.WarnFormat("Error deleting directory {0}.", deletePath);
                        _log.Warn(ex1.Message, ex1);
                        _log.Warn("Attempting to delete individually ...");
                        try
                        {
                            Recycler.Delete(new DirectoryInfo(deletePath));
                        }
                        catch (Exception ex)
                        {
                            var message = string.Format("Error deleting directory {0}", deletePath);
                            _log.Error(message);
                            _log.Error(ex.Message, ex);
                            OnErrorOccured(message, ex);
                        }
                    }
                }
                OnProgressChanged(percentageProgress + (i+1)*allocatedPercentage/deletePaths.Count);
            }
        }


        public event ProgressChangedEventHandler ProgressChanged;
        public event ActionChangedEventHandler ActionChanged;
        public bool StopPending { get; private set; }
        public void Stop()
        {
            StopPending = true;
        }

        public event ErrorOccuredEventHandler ErrorOccured;

        private void OnProgressChanged(int progressPercentage)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, new ProgressChangedEventArgs(Math.Min(100, progressPercentage)));
            }
        }

        private void OnActionChanged(string actionName)
        {
            if (ActionChanged != null)
            {
                ActionChanged(this, new ActionChangedEventArgs(actionName));
            }
        }

        private void OnErrorOccured(string errorMessage, Exception ex)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured(this, new ErrorEventArgs(errorMessage, ex));
            }
        }
    }
}
