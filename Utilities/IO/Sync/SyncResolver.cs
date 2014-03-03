using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities.EventArgs;
using Utilities.IO.FileSystem;
using Utilities.Interfaces;
using log4net;

namespace Utilities.IO.Sync
{
    public class SyncResolver : ISyncResolver
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof (SyncResolver));

        public SyncResolver()
        {
        }

        public SyncResolver(ISyncOptions syncOptions)
        {
            SyncOptions = syncOptions;
            StopPending = false;
        }

        public ISyncOptions SyncOptions { get; set; }

        
        #region IActionReporter Members
        public event ActionChangedEventHandler ActionChanged;

        #endregion

        #region IProgressReporter Members

        public event ProgressChangedEventHandler ProgressChanged;

        #endregion

        public IList<SyncOperation> ResolveSync(IDirectoryInfo source, IList<IDirectoryInfo> sourceDirectories, IList<IFileInfo> sourceFiles, IDirectoryInfo target, IList<IDirectoryInfo> targetDirectories, IList<IFileInfo> targetFiles, IEnumerable<string> filters = null)
        {
            StopPending = false;
            if (SyncOptions == null)
            {
                throw new InvalidOperationException("Sync options are not specified.");
            }
            else if (source == null)
            {
                throw new ArgumentNullException("source", "Source directory is null.");
            }
            else if (sourceDirectories == null)
            {
                throw new ArgumentNullException("sourceDirectories", "Source directories is null.");
            }
            else if (sourceFiles == null)
            {
                throw new ArgumentNullException("sourceFiles", "Source files is null.");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target", "Target directory is null.");
            }
            else if (targetDirectories == null)
            {
                throw new ArgumentNullException("targetDirectories", "Target directories is null.");
            }
            else if (targetFiles == null)
            {
                throw new ArgumentNullException("targetFiles", "Target files is null.");
            }

            _log.Info("Resolving sync operations ...");
            _log.Debug("Preparing files ...");
            OnActionChanged("Preparing files ...");
            OnProgressChanged(0);
            var operations = new List<SyncOperation>();
            var sourceDirectoryMap = sourceDirectories.ToDictionary(d => d.FullName.GetRelativePath(source.FullName),
                                                                    d => d);
            var sourceFileMap = sourceFiles.ToDictionary(f => f.FullName.GetRelativePath(source.FullName), f => f);
            var targetDirectoryMap = targetDirectories.ToDictionary(d => d.FullName.GetRelativePath(target.FullName),
                                                                    d => d);
            var targetFileMap = targetFiles.ToDictionary(f => f.FullName.GetRelativePath(target.FullName), f => f);

            // SyncOptions.SourceExistsNoTarget
            if (SyncOptions.SourceExistsNoTarget != SyncOperationType.DoNothing)
            {
                const string message = "Looking for files that exist in source but not in target ...";
                _log.Debug(message);
                OnActionChanged(message);
                var filesExistingInSourceNotInTarget =
                    sourceFileMap.AsParallel().Where(sd => !targetFileMap.ContainsKey(sd.Key)).Select(
                        sd =>
                        new SyncOperation(sd.Value, new FileInfoWrapper(Path.Combine(target.FullName, sd.Key)),
                                          SyncOptions.SourceExistsNoTarget)).
                        ToList();
                _log.DebugFormat("Found {0} file(s).", filesExistingInSourceNotInTarget.Count);
                operations.AddRange(filesExistingInSourceNotInTarget);
            }
            OnProgressChanged(15);
            StopIfRequested();

            // SyncOptions.TargetExistsNoSource
            if (SyncOptions.TargetExistsNoSource != SyncOperationType.DoNothing)
            {
                const string message = "Looking for files that exist in target but not in source ...";
                _log.Debug(message);
                OnActionChanged(message);
                var filesExistingInTargetNotInSource =
                    targetFileMap.AsParallel().Where(td => !sourceFileMap.ContainsKey(td.Key)).Select(
                        td =>
                        new SyncOperation(new FileInfoWrapper(Path.Combine(source.FullName, td.Key)), td.Value,
                                          SyncOptions.TargetExistsNoSource)).
                        ToList();
                _log.DebugFormat("Found {0} file(s).", filesExistingInTargetNotInSource.Count);
                operations.AddRange(filesExistingInTargetNotInSource);
            }
            OnProgressChanged(30);
            StopIfRequested();

            // Common files
            var commonFiles = new List<SyncPair>();
            if (SyncOptions.SourceNewer != SyncOperationType.DoNothing
                || SyncOptions.TargetNewer != SyncOperationType.DoNothing
                || SyncOptions.SourceLarger != SyncOperationType.DoNothing
                || SyncOptions.TargetLarger != SyncOperationType.DoNothing)
            {
                const string message = "Looking for common files ...";
                _log.Debug(message);
                OnActionChanged(message);
                IFileInfo targetFile = null;
                commonFiles = (from sourceFileKeyValuPair
                                   in sourceFileMap
                               where
                                   targetFileMap.TryGetValue(sourceFileKeyValuPair.Key,
                                                             out targetFile)
                               select
                                   new SyncPair(sourceFileKeyValuPair.Value,
                                                          targetFile)).ToList();
                _log.DebugFormat("Found {0} file(s).", commonFiles.Count);
            }
            OnProgressChanged(40);
            StopIfRequested();

            // SyncOptions.SourceNewer
            var sourceNewerFiles = new List<SyncPair>();
            if (SyncOptions.SourceNewer != SyncOperationType.DoNothing || SyncOptions.TargetNewer != SyncOperationType.DoNothing)
            {
                const string message = "Looking for files that are newer in source than in target ...";
                _log.Debug(message);
                OnActionChanged(message);
                sourceNewerFiles =
                    commonFiles.AsParallel().Where(cf => cf.Source.LastWriteTime.CompareTo(cf.Target.LastWriteTime) > 0).ToList();
                _log.DebugFormat("Found {0} file(s).", sourceNewerFiles.Count);
                operations.AddRange(
                    sourceNewerFiles.Select(cf => new SyncOperation(cf.Source, cf.Target, SyncOptions.SourceNewer)));
            }
            OnProgressChanged(55);
            StopIfRequested();

            // SyncOptions.TargetNewer
            var targetNewerFiles = new List<SyncPair>();
            if (SyncOptions.SourceNewer != SyncOperationType.DoNothing || SyncOptions.TargetNewer != SyncOperationType.DoNothing)
            {
                const string message = "Looking for files that are newer in target than in source ...";
                _log.Debug(message);
                OnActionChanged(message);
                targetNewerFiles =
                    commonFiles.AsParallel().Where(cf => cf.Target.LastWriteTime.CompareTo(cf.Source.LastWriteTime) > 0).ToList();
                _log.DebugFormat("Found {0} file(s).", targetNewerFiles.Count);
                if (SyncOptions.TargetNewer != SyncOperationType.DoNothing)
                {
                    operations.AddRange(
                        targetNewerFiles.Select(cf => new SyncOperation(cf.Source, cf.Target, SyncOptions.TargetNewer)));
                }
            }
            OnProgressChanged(70);
            StopIfRequested();

            // Timestamp equal on files
            var commonEqualFiles = new List<SyncPair>();
            if (SyncOptions.SourceLarger != SyncOperationType.DoNothing
                || SyncOptions.TargetLarger != SyncOperationType.DoNothing)
            {
                const string message = "Looking for common files that have equal write timestamps ...";
                _log.Debug(message);
                OnActionChanged(message);
                var commonNotEqualFileMap =
                    sourceNewerFiles.Union(targetNewerFiles).ToDictionary(cf => cf.Source.FullName);
                commonEqualFiles =
                    commonFiles.AsParallel().Where(cf => !commonNotEqualFileMap.ContainsKey(cf.Source.FullName)).ToList();
                _log.DebugFormat("Found {0} file(s).", commonEqualFiles.Count);
            }
            OnProgressChanged(75);
            StopIfRequested();

            // SyncOptions.SourceNewer
            if (SyncOptions.SourceLarger != SyncOperationType.DoNothing)
            {
                const string message = "Looking for files that are larger in source than in target ...";
                _log.Debug(message);
                OnActionChanged(message);
                var sourceLargerFiles =
                    commonEqualFiles.AsParallel().Where(cf => cf.Source.Size > cf.Target.Size).ToList();
                _log.DebugFormat("Found {0} file(s).", sourceLargerFiles.Count);
                operations.AddRange(
                    sourceLargerFiles.Select(cf => new SyncOperation(cf.Source, cf.Target, SyncOptions.SourceLarger)));
            }
            OnProgressChanged(80);
            StopIfRequested();

            // SyncOptions.TargetNewer
            if (SyncOptions.TargetLarger != SyncOperationType.DoNothing)
            {
                const string message = "Looking for files that are larger in target than in source ...";
                _log.Debug(message);
                OnActionChanged(message);
                var targetLargerFiles =
                    commonEqualFiles.AsParallel().Where(cf => cf.Source.Size < cf.Target.Size).ToList();
                _log.DebugFormat("Found {0} file(s).", targetLargerFiles.Count);
                operations.AddRange(
                    targetLargerFiles.Select(cf => new SyncOperation(cf.Source, cf.Target, SyncOptions.TargetLarger)));
            }
            OnProgressChanged(85);
            StopIfRequested();

            // SyncOptions.SourceExistsNoTarget;
            if (SyncOptions.SourceExistsNoTarget != SyncOperationType.DoNothing)
            {
                const string message = "Looking for directories that exist in source but not in target ...";
                _log.Debug(message);
                OnActionChanged(message);
                var directoriesExistingInSourceNotInTarget =
                    sourceDirectoryMap.AsParallel().Where(sd => !targetDirectoryMap.ContainsKey(sd.Key)).Select(
                        sd =>
                        new SyncOperation(sd.Value, new DirectoryInfoWrapper(Path.Combine(target.FullName, sd.Key)),
                                          SyncOptions.SourceExistsNoTarget)).
                        ToList();
                _log.DebugFormat("Found {0} directories.", directoriesExistingInSourceNotInTarget.Count);
                operations.AddRange(directoriesExistingInSourceNotInTarget);
            }
            OnProgressChanged(92);
            StopIfRequested();

            // SyncOptions.TargetExistsNoSource
            if (SyncOptions.TargetExistsNoSource != SyncOperationType.DoNothing)
            {
                const string message = "Looking for directories that exist in target but not in source ...";
                _log.Debug(message);
                OnActionChanged(message);
                var directoriesExistingInTargetNotInSource =
                    targetDirectoryMap.AsParallel().Where(td => !sourceDirectoryMap.ContainsKey(td.Key)).Select(
                        td =>
                        new SyncOperation(new DirectoryInfoWrapper(Path.Combine(source.FullName, td.Key)), td.Value,
                                          SyncOptions.TargetExistsNoSource)).
                        ToList();
                _log.DebugFormat("Found {0} directories.", directoriesExistingInTargetNotInSource.Count);
                operations.AddRange(directoriesExistingInTargetNotInSource);
            }

            // Remove operations due to filters
            string[] filtersArray;
            if (filters != null && (filtersArray = filters as string[] ?? filters.ToArray()).Length > 0)
            {
                const string message = "Removing filtered operations ...";
                _log.Debug(message);
                OnActionChanged(message);
                //foreach (var filter in filtersArray)
                //{
                    for (var i = operations.Count - 1; i >= 0; i--)
                    {
                        var operation = operations[i];
                        //if ((operation.Source != null && Regex.IsMatch(operation.Source.FullName, filter.EscapedFilter))
                        //    || (operation.Target != null && Regex.IsMatch(operation.Target.FullName, filter.EscapedFilter)))
                        if ((operation.Source != null && FindFilesRegEx.MatchesFilePattern(operation.Source.FullName, filtersArray.ToArray()))
                           || (operation.Target != null && FindFilesRegEx.MatchesFilePattern(operation.Target.FullName, filtersArray.ToArray())))
                        {
                            operations.RemoveAt(i);
                        }
                    }
                //}
            }
            OnProgressChanged(100);

            _log.InfoFormat("Discovered {0} operation(s).", operations.Count.ToString("#,0"));
            return operations;
        }

        private void StopIfRequested()
        {
            // Stop if requested
            if (StopPending)
            {
                StopPending = false;
                throw new StoppedException();
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
                ProgressChanged(this, new ProgressChangedEventArgs(Math.Min(100, progressPercentage)));
            }
        }

        public bool StopPending { get; set; }

        public void Stop()
        {
            StopPending = true;
        }
    }
}