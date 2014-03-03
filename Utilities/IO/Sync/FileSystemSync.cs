using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Utilities.EventArgs;
using Utilities.IO.FileSystem;
using Utilities.Interfaces;
using Utilities.Text;
using log4net;
using ErrorEventArgs = Utilities.EventArgs.ErrorEventArgs;

namespace Utilities.IO.Sync
{
    public class FileSystemSync : IProgressReporter, IStoppable, IActionReporter, IErrorReporter
    {
        #region Fields
        protected static ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IFileSystemBrowser _browser;
        private readonly ISyncResolver _syncResolver;
        private readonly ISyncOperationRunner _operationRunner;

        protected int PreviouslyReportedProgress = -1;
        protected int ProgressPercentage;

        protected int BrowserAllocationPercentage = 5;
        protected int SyncResolverAllocationPercentage = 5;
        protected int RunOperationsAllocationPercentage = 85;

        public event ProgressChangedEventHandler ProgressChanged;
        public event ActionChangedEventHandler ActionChanged;
        public event ErrorOccuredEventHandler ErrorOccured;
        #endregion

        #region Properties
        public bool StopPending { get; protected set; }
        public long BytesToCopy { get { return _operationRunner.BytesToCopy; } }
        public long BytesCopied { get { return _operationRunner.BytesCopied; } }
        public List<string> Filters { get; protected set; }
        #endregion

        /// <summary>
        /// Stops the current sync operation.
        /// </summary>
        public void Stop()
        {
            StopPending = true;
            _browser.Stop();
            _syncResolver.Stop();
            _operationRunner.Stop();
        }

        public FileSystemSync(IFileSystemBrowser browser, ISyncResolver syncResolver, ISyncOperationRunner operationRunner)
        {
            if (browser == null)
            {
                throw new ArgumentNullException("browser", "Browser is null.");
            }
            else if (syncResolver == null)
            {
                throw new ArgumentNullException("syncResolver", "SyncResolver is null.");
            }
            else if (operationRunner == null)
            {
                throw new ArgumentNullException("operationRunner", "OperationRunner is null.");
            }
            Filters = new List<string>();

            _browser = browser;
            _browser.ActionChanged += OnActionChanged;
            _browser.ErrorOccured += OnErrorOccured;
            _browser.ProgressChanged += BrowserProgressChanged;

            _syncResolver = syncResolver;
            _syncResolver.ActionChanged += OnActionChanged;
            _syncResolver.ProgressChanged += SyncResolverProgressChanged;

            _operationRunner = operationRunner;
            _operationRunner.ActionChanged += OnActionChanged;
            _operationRunner.ProgressChanged += OpeationRunnerProgressChanged;
            _operationRunner.ErrorOccured += OnErrorOccured;
            
            StopPending = false;
        }

        /// <summary>
        /// Performs sync of source directory to target.
        /// </summary>
        /// <param name="sourcePath">Source directory path.</param>
        /// <param name="targetPath">Target directory path.</param>
        public void Sync(string sourcePath, string targetPath)
        {
            if (sourcePath.IsNullEmptyOrWhitespace())
            {
                throw new ArgumentNullException("sourcePath", "Source path is null or empty.");
            }
            else if (targetPath.IsNullEmptyOrWhitespace())
            {
                throw new ArgumentNullException("targetPath", "Target path is null or empty.");
            }
            Sync(new DirectoryInfoWrapper(sourcePath), new DirectoryInfoWrapper(targetPath));
        }

        /// <summary>
        /// Performs sync of source directory to target.
        /// </summary>
        /// <param name="source">Source directory.</param>
        /// <param name="target">Target directory.</param>
        public void Sync(DirectoryInfo source, DirectoryInfo target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Source is null.");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null.");
            }
            Sync(new DirectoryInfoWrapper(source), new DirectoryInfoWrapper(target));
        }

        /// <summary>
        /// Performs sync of source directory to target.
        /// </summary>
        /// <param name="source">Source directory.</param>
        /// <param name="target">Target directory.</param>
        public void Sync(IDirectoryInfo source, IDirectoryInfo target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Source is null.");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null.");
            }
            else if (!source.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Source directory {0} does not exist.", source.FullName));
            }
            else if (!target.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Target directory {0} does not exist.", target.FullName));
            }

            PreviouslyReportedProgress = -1;
            ProgressPercentage = 0;
            StopPending = false;

            try
            {
                List<IDirectoryInfo> sourceDirectories;
                List<IFileInfo> sourceFiles;
                ScanSource(source, out sourceDirectories, out sourceFiles);

                List<IDirectoryInfo> targetDirectories;
                List<IFileInfo> targetFiles;
                ScanTarget(target, out targetDirectories, out targetFiles);

                var operations = ResolveOperations(source, target, targetFiles, targetDirectories, sourceFiles, sourceDirectories);

                RunOperations(operations);
                
                ProgressPercentage = 100;
                OnProgressChanged(ProgressPercentage);
            }
            catch (StoppedException)
            {
                Log.Info("Operation stopped.");
                OnActionChanged("Stopped.");
                StopPending = false;
            }
            catch (Exception ex)
            {
                var message = string.Format("Error syncing directory {0} to {1}", source.FullName, target.FullName);
                Log.ErrorFormat(message);
                Log.Error(ex.Message, ex);
                OnErrorOccured(message, ex);
            }
        }

        protected virtual void RunOperations(IList<SyncOperation> operations)
        {
            Log.Debug("Performing sync ...");
            OnActionChanged("Performing sync ...");
            _operationRunner.RunOperations(operations);
        }

        protected virtual IList<SyncOperation> ResolveOperations(IDirectoryInfo source, IDirectoryInfo target, List<IFileInfo> targetFiles,
                                                  List<IDirectoryInfo> targetDirectories, List<IFileInfo> sourceFiles, List<IDirectoryInfo> sourceDirectories)
        {
            const string actionName = "Resolving sync operations ...";
            Log.Debug(actionName);
            OnActionChanged(actionName);
            var operations = _syncResolver.ResolveSync(source, sourceDirectories, sourceFiles, target, targetDirectories,
                                                       targetFiles, Filters);
            ProgressPercentage += SyncResolverAllocationPercentage;
            CheckStopped();
            return operations;
        }

        protected virtual void Scan(IDirectoryInfo target, out List<IDirectoryInfo> targetDirectories, out List<IFileInfo> targetFiles, string actionName)
        {
            Log.Debug(actionName);
            OnActionChanged(actionName);
            targetFiles = new List<IFileInfo>();
            targetDirectories = new List<IDirectoryInfo>();
            _browser.Scan(target.FullName, targetFiles, targetDirectories);
            ProgressPercentage += BrowserAllocationPercentage;
            CheckStopped();
        }

        protected virtual void ScanSource(IDirectoryInfo source, out List<IDirectoryInfo> sourceDirectories, out List<IFileInfo> sourceFiles)
        {
            Scan(source, out sourceDirectories, out sourceFiles, "Scanning source ...");
            //const string actionName = "Scanning source ...";
            //OnProgressChanged(ProgressPercentage);
            //Log.Debug(actionName);
            //OnActionChanged(actionName);
            //sourceFiles = new List<IFileInfo>();
            //sourceDirectories = new List<IDirectoryInfo>();
            //_browser.Scan(source.FullName, sourceFiles, sourceDirectories);
            //ProgressPercentage += BrowserAllocationPercentage;
            //CheckStopped();
        }

        protected virtual void ScanTarget(IDirectoryInfo target, out List<IDirectoryInfo> targetDirectories, out List<IFileInfo> targetFiles)
        {
            Scan(target, out targetDirectories, out targetFiles, "Scanning target ...");
            //const string actionName = "Scanning source ...";
            //OnProgressChanged(ProgressPercentage);
            //Log.Debug(actionName);
            //OnActionChanged(actionName);
            //sourceFiles = new List<IFileInfo>();
            //sourceDirectories = new List<IDirectoryInfo>();
            //_browser.Scan(source.FullName, sourceFiles, sourceDirectories);
            //ProgressPercentage += BrowserAllocationPercentage;
            //CheckStopped();
        }

        private void CheckStopped()
        {
            if (StopPending)
            {
                throw new StoppedException();
            }
        }

        private void BrowserProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            OnProgressChanged(ProgressPercentage + BrowserAllocationPercentage * args.ProgressPercentage / 100);
        }

        private void SyncResolverProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            OnProgressChanged(ProgressPercentage + SyncResolverAllocationPercentage * args.ProgressPercentage / 100);
        }

        private void OpeationRunnerProgressChanged(object sender, ProgressChangedEventArgs args)
        {
            OnProgressChanged(ProgressPercentage + RunOperationsAllocationPercentage * args.ProgressPercentage / 100);
        }

        private void OnProgressChanged(int progressPercentage)
        {
            if (ProgressChanged != null && progressPercentage != PreviouslyReportedProgress)
            {
                ProgressChanged(this, new ProgressChangedEventArgs(Math.Min(100, progressPercentage)));
                PreviouslyReportedProgress = progressPercentage;
            }
        }

        private void OnActionChanged(object sender, ActionChangedEventArgs args)
        {
            if (ActionChanged != null)
            {
                ActionChanged(sender, args);
            }
        }

        private void OnActionChanged(string actionName)
        {
            if (ActionChanged != null)
            {
                ActionChanged(this, new ActionChangedEventArgs(actionName));
            }
        }

        private void OnErrorOccured(object sender, ErrorEventArgs args)
        {
            if (ErrorOccured != null)
            {
                ErrorOccured(sender, args);
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
