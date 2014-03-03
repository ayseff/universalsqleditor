namespace Utilities.IO.Sync
{
    //public class FreeSpaceManager
    //{
    //    //private readonly Dictionary<SyncDirectory, NameValuePair<string , long>> _targetDirectorySizes;
    //    //private readonly Dictionary<SyncDirectory, NameValuePair<string, long>> _sourceDirectorySizes;

    //    //public FreeSpaceManager(IEnumerable<SyncDirectory> sources, IEnumerable<SyncDirectory> targets)
    //    //{
    //    //    _sourceDirectorySizes = new Dictionary<SyncDirectory, NameValuePair<string, long>>();
    //    //    PopulateFreeSpace(_sourceDirectorySizes, sources);

    //    //    _targetDirectorySizes = new Dictionary<SyncDirectory, NameValuePair<string, long>>();
    //    //    PopulateFreeSpace(_targetDirectorySizes, targets);
    //    //}

    //    //public void RegisterOperation(SyncDirectory writeDirectory, long fileSize)
    //    //{
    //    //    NameValuePair<string, long> target;
    //    //    if (!_sourceDirectorySizes.TryGetValue(writeDirectory, out target) && !_targetDirectorySizes.TryGetValue(writeDirectory, out target))
    //    //    {
    //    //        throw new Exception("Could not find directory " + writeDirectory.BaseDirectory.FullName);
    //    //    }
    //    //    target.Value -= fileSize;
    //    //}

    //    ////public SyncDirectory GetWriteDirectory(long size, TargetType targetType, SyncOperationType operationType)
    //    ////{
    //    ////    if (operationType == SyncOperationType.DeleteSource
    //    ////        || operationType == SyncOperationType.CopyToSource
    //    ////        || operationType == SyncOperationType.CreateSource)
    //    ////    {
    //    ////        return GetSourcetWriteDirectory(size, targetType);
    //    ////    }
    //    ////    else if (operationType == SyncOperationType.DeleteTarget
    //    ////        || operationType == SyncOperationType.CopyToTarget
    //    ////        || operationType == SyncOperationType.CreateTarget)
    //    ////    {
    //    ////        return GetTargetWriteDirectory(size, targetType);
    //    ////    }
    //    ////    else
    //    ////    {
    //    ////        throw new Exception("Unknown SyncOperationType " + operationType.ToString());
    //    ////    }
    //    ////}


    //    //public SyncDirectory GetSourcetWriteDirectory(long size, TargetType targetType)
    //    //{
    //    //    return GetWriteDirectory(_sourceDirectorySizes, size, targetType);
    //    //}

    //    //public SyncDirectory GetTargetWriteDirectory(long size, TargetType targetType)
    //    //{
    //    //    return GetWriteDirectory(_targetDirectorySizes, size, targetType);
    //    //}

    //    //private SyncDirectory GetWriteDirectory(Dictionary<SyncDirectory, NameValuePair<string , long>> directorySizes, long size, TargetType targetType)
    //    //{
    //    //    List<SyncDirectory> smallestDirectoryList;
    //    //    var query = directorySizes.Where(s => s.Value.Value > size);
    //    //    if (targetType == TargetType.SmallestThatFits)
    //    //    {
    //    //        smallestDirectoryList =
    //    //            query.OrderBy(s => s.Value.Value).Select(s => s.Key).ToList();
    //    //    }
    //    //    else
    //    //    {
    //    //        smallestDirectoryList =
    //    //            query.OrderByDescending(s => s.Value.Value).Select(s => s.Key).ToList();
    //    //    }

    //    //    if (smallestDirectoryList.Count == 0)
    //    //    {
    //    //        return directorySizes.First().Key;
    //    //    }
    //    //    return smallestDirectoryList[0];
    //    //}

    //    //private void PopulateFreeSpace(Dictionary<SyncDirectory, NameValuePair<string, long>> directorySizes, IEnumerable<SyncDirectory> directories)
    //    //{
    //    //    foreach (var syncDirectory in directories)
    //    //    {
    //    //        if (!directorySizes.ContainsKey(syncDirectory))
    //    //        {
    //    //            var root =
    //    //                directorySizes.Values.FirstOrDefault(v => v.Name == syncDirectory.BaseDirectory.Root.FullName);
    //    //            if (root == null)
    //    //            {
    //    //                var driveInfo = new General.DriveInfo(syncDirectory.BaseDirectory.Root.FullName);
    //    //                root = new NameValuePair<string, long>(syncDirectory.BaseDirectory.Root.FullName,
    //    //                                                       driveInfo.AvailableFreeSpace);
    //    //            }
    //    //            directorySizes.Add(syncDirectory, root);
    //    //        }
    //    //    }
    //    //}
    //}
}