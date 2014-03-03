namespace Utilities.IO.Sync
{
    public class SyncOperation
    {
        public SyncOperation(IFileSystemInfo source, IFileSystemInfo target, SyncOperationType operationType)
        {
            Source = source;
            Target = target;
            OperationType = operationType;
        }

        public IFileSystemInfo Source { get; set; }
        public IFileSystemInfo Target { get; set; }
        public SyncOperationType OperationType { get; set; }

        public bool IsFile
        {
            get { return Source is IFileInfo; }
        }

        public override string ToString()
        {
            return string.Format("{0}: {1} - {2}", OperationType.ToString(), Source.FullName, Target.FullName);
        }
    }
}