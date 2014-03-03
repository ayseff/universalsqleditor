using System;
using System.IO;

namespace Utilities.IO.Copying
{
    [Serializable]
    public class CopyPair
    {
        /// <summary>
        ///   Creates a new copy pair.
        /// </summary>
        /// <param name="source"> Source file or directory. </param>
        /// <param name="target"> Traget file or directory. </param>
        public CopyPair(IFileSystemInfo source, IFileSystemInfo target)
        {
            Initialize(source, target);
            SourceInfo = source;
            TargetInfo = target;
        }

        /// <summary>
        ///   Creates a new copy pair.
        /// </summary>
        /// <param name="source"> Source file or directory path. </param>
        /// <param name="target"> Traget file or directory path. </param>
        public CopyPair(string source, string target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Source is null!");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null!");
            }
            SourceInfo = CreateSourceFileSystemInfo(source);
            TargetInfo = CreateTargetFileSystemInfo(target);
        }

        /// <summary>
        ///   Creates a new copy pair.
        /// </summary>
        /// <param name="source"> Source file or directory. </param>
        /// <param name="target"> Traget file or directory path. </param>
        public CopyPair(IFileSystemInfo source, string target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null.");
            }
            SourceInfo = source;
            TargetInfo = CreateTargetFileSystemInfo(target);
            Initialize(source, TargetInfo);
        }

        /// <summary>
        ///   Creates a new copy pair.
        /// </summary>
        /// <param name="source"> Source file. </param>
        /// <param name="target"> Traget file path. </param>
        public CopyPair(FileInfo source, string target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null.");
            }
            SourceInfo = new FileInfoWrapper(source);
            TargetInfo = CreateTargetFileSystemInfo(target);
            Initialize(SourceInfo, TargetInfo);
        }

        /// <summary>
        ///   Creates a new copy pair.
        /// </summary>
        /// <param name="source"> Source directory. </param>
        /// <param name="target"> Traget directory path. </param>
        public CopyPair(DirectoryInfo source, string target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null.");
            }
            IsFile = false;
            SourceInfo = new DirectoryInfoWrapper(source);
            TargetInfo = CreateTargetFileSystemInfo(target);
            Initialize(SourceInfo, TargetInfo);
        }

        /// <summary>
        ///   Creates a new copy pair.
        /// </summary>
        /// <param name="source"> Source file or directory. </param>
        /// <param name="target"> Traget file or directory. </param>
        public CopyPair(FileSystemInfo source, FileSystemInfo target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Source is null!");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null!");
            }
            SourceInfo = CreateSourceFileSystemInfo(source.FullName);
            TargetInfo = CreateTargetFileSystemInfo(target.FullName);
            Initialize(SourceInfo, TargetInfo);
        }

        /// <summary>
        ///   Gets source file or directory.
        /// </summary>
        public IFileSystemInfo SourceInfo { get; protected set; }

        /// <summary>
        ///   Gets target file or directory.
        /// </summary>
        public IFileSystemInfo TargetInfo { get; protected set; }

        /// <summary>
        ///   Gets wheather the copy pair is a pair of files.
        /// </summary>
        public bool IsFile { get; protected set; }

        private void Initialize(IFileSystemInfo source, IFileSystemInfo target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source", "Source is null!");
            }
            else if (target == null)
            {
                throw new ArgumentNullException("target", "Target is null!");
            }

            IsFile = source is IFileInfo;
            var isDirectory = source is IDirectoryInfo;
            if (IsFile && !(target is IFileInfo))
            {
                throw new InvalidOperationException("Cannot copy file to directory.");
            }
            else if (isDirectory && !(target is IDirectoryInfo))
            {
                throw new InvalidOperationException("Cannot copy directory to file.");
            }
            else if (!IsFile && !isDirectory)
            {
                throw new InvalidOperationException("Only file and directory copying is allowed.");
            }
            else if (IsFile && !source.Exists)
            {
                throw new FileNotFoundException(string.Format("File {0} does not exist.", source.FullName),
                                                source.FullName);
            }
            else if (isDirectory && !source.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Directory {0} does not exist.", source.FullName));
            }
        }

        private IFileSystemInfo CreateSourceFileSystemInfo(string path)
        {
            IFileSystemInfo info;
            if (Directory.Exists(path))
            {
                info = new DirectoryInfoWrapper(path);
                IsFile = false;
            }
            else if (File.Exists(path))
            {
                info = new FileInfoWrapper(path);
                IsFile = true;
            }
            else
            {
                throw new ArgumentException(string.Format("Path {0} does not exist.", path), "path");
            }
            return info;
        }

        private IFileSystemInfo CreateTargetFileSystemInfo(string path)
        {
            if (!IsFile)
            {
                return new DirectoryInfoWrapper(path);
            }
            else
            {
                return new FileInfoWrapper(path);
            }
        }
    }
}