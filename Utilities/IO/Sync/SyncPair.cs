namespace Utilities.IO.Sync
{
    internal class SyncPair
    {
        public SyncPair()
        {
        }

        public SyncPair(IFileSystemInfo source, IFileSystemInfo target)
        {
            Target = target;
            Source = source;
        }

        public IFileSystemInfo Source { get; set; }
        public IFileSystemInfo Target { get; set; }

        public override string ToString()
        {
            var src = Source == null ? "NULL" : Source.FullName;
            var trg = Target == null ? "NULL" : Target.FullName;
            return string.Format("{0} -- {1}", src, trg);
        }
    }
}