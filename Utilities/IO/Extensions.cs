using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using log4net;

namespace Utilities.IO
{
    public enum FileSystemType
    {
        None = 0,
        File = 1,
        Directory = 2
    }

    public static class Extensions
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the relative path of the file or directory in reference to base path.
        /// </summary>
        /// <param name="fileOrDirectory">File or directory.</param>
        /// <param name="basePath">Base path.</param>
        /// <exception cref="ArgumentNullException">When file or directory is null.</exception>
        /// <returns>Relative path of the file or directory in reference to base path.</returns>
        public static string GetRelativePath(this IFileSystemInfo fileOrDirectory, string basePath)
        {
            if (fileOrDirectory == null)
            {
                throw new ArgumentNullException("fileOrDirectory", "File is null.");
            }
            else if (basePath == null)
            {
                throw new ArgumentNullException("basePath", "Base path is null.");
            }
            return fileOrDirectory.FullName.GetRelativePath(basePath);
        }

        /// <summary>
        /// Gets the relative path of the file or directory in reference to base path.
        /// </summary>
        /// <param name="fileOrDirectory">File or directory.</param>
        /// <param name="basePath">Base path.</param>
        /// <exception cref="ArgumentNullException">When file is null.</exception>
        /// <returns>Relative path of the file or directory in reference to base path.</returns>
        public static string GetRelativePath(this FileSystemInfo fileOrDirectory, string basePath)
        {
            if (fileOrDirectory == null)
            {
                throw new ArgumentNullException("fileOrDirectory", "File is null.");
            }
            else if (basePath == null)
            {
                throw new ArgumentNullException("basePath", "Base path is null.");
            }
            return fileOrDirectory.FullName.GetRelativePath(basePath);
        }

        /// <summary>
        /// Gets the relative path of supplied full path in reference to base path.
        /// </summary>
        /// <param name="path">File or directory path.</param>
        /// <param name="basePath">Base path.</param>
        /// <exception cref="ArgumentNullException">When path or base path are null.</exception>
        /// <exception cref="ArgumentException">When base or supplied full path are invalid or full path is not a child path of base path.</exception>
        /// <returns>Relative path of the file or directory in reference to base path.</returns>
        public static string GetRelativePath(this string path, string basePath)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path is null.");
            }
            else if (basePath == null)
            {
                throw new ArgumentNullException("basePath", "Base path is null.");
            }
            else if (path == basePath)
            {
                return string.Empty;
            }

            // Validate base path
            basePath = basePath.CleanPath();
            try
            {
                Path.GetFullPath(basePath);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Base path {0} is invalid.", path), ex);
            }

            // Validate path
            path = path.CleanPath();
            try
            {
                Path.GetFullPath(path);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Path {0} is invalid.", path), ex);
            }

            // Ensure paths are relative
            if (!path.StartsWith(basePath))
            {
                throw new ArgumentException(string.Format("Path {0} does not start with base path {1}.", path, basePath));
            }

            // Get relative path
            string relativePath = path.Substring(basePath.Length);
            if (!relativePath.StartsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
            {
                throw new ArgumentException(string.Format("Path {0} does not start with base path {1}.", path, basePath));
            }
            relativePath = relativePath.Remove(0, 1);

            return relativePath;
        }

        /// <summary>
        /// Cleans up supplied path and returns the cleaned path.
        /// </summary>
        /// <param name="path">Path to clean.</param>
        /// <exception cref="ArgumentNullException">When path is null.</exception>
        /// <returns>Cleaned path.</returns>
        /// <remarks>Path is cleaned by converting all '/' path separators into '\' and removing any path
        /// separators at the end of the path.</remarks>
        public static string CleanPath(this string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path is null.");
            }

            path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Trim();
            while (path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))
            {
                path = path.Remove(path.Length - 1, 1);
            }
            return path;
        }

        /// <summary>
        /// Replaces all invalid characters from path with a replacement character.
        /// </summary>
        /// <param name="path">Path to clean.</param>
        /// <param name="replacementCharacter">Replacement character to use.</param>
        /// <returns>Cleaned path.</returns>
        public static string ReplaceInvalidCharactersFromPath(this string path, char replacementCharacter)
        {
            if (path == null)
            {
                return null;
            }
            
            var invalidCharacters = Path.GetInvalidPathChars().Union(Path.GetInvalidFileNameChars());
            foreach (var invalidCharacter in invalidCharacters)
            {
                path = path.Replace(invalidCharacter, replacementCharacter);
            }
            return path;
        }

        /// <summary>
        /// Computes and returns SHA1 hash of the file.
        /// </summary>
        /// <param name="file">File on which to compute the hash.</param>
        /// <exception cref="ArgumentNullException">When file is null.</exception>
        /// <exception cref="FileNotFoundException">Whne file does not exist.</exception>
        /// <returns>SHA1 hash of the file.</returns>
        public static string ComputeSHA1(this IFileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file", "File is null");
            }
            else if (!file.Exists)
            {
                throw new FileNotFoundException(string.Format("File {0} not found.", file.FullName), file.FullName);
            }

            using (var cryptoProvider = new SHA1CryptoServiceProvider())
            {
                using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return BitConverter.ToString(cryptoProvider.ComputeHash(fs));
                }
            }
        }

        /// <summary>
        /// Computes and returns SHA1 hash of the file.
        /// </summary>
        /// <param name="file">File on which to compute the hash.</param>
        /// <exception cref="ArgumentNullException">When file is null.</exception>
        /// <exception cref="FileNotFoundException">Whne file does not exist.</exception>
        /// <returns>SHA1 hash of the file.</returns>
        public static string ComputeSHA1(this FileInfo file)
        {
            var fileInfo = new FileInfoWrapper(file);
            return fileInfo.ComputeSHA1();
        }

        /// <summary>
        /// Returns the sum of sizes of all files in the directory (including subdirectories).
        /// </summary>
        /// <param name="directory"><code>DirectoryInfo</code> object.</param>
        /// <returns>The sum of sizes of all files in the directory.</returns>
        /// <exception cref="ArgumentNullException">When directory is null.</exception>
        /// <exception cref="DirectoryNotFoundException">When directory does not exist.</exception>
        public static long GetDirectorySize(this DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory", "Directory cannot be null.");
            }
            else if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Directory {0} does not exist.", directory.FullName));
            }

            var wrapper = new DirectoryInfoWrapper(directory);
            return wrapper.Size;

            //return directory.GetFiles().Sum(fi => fi.Length)
            //    + directory.GetDirectories().Sum(diSourceSubDir => GetDirectorySize(diSourceSubDir));
        }

        /// <summary>
        /// Deletes a file or directory including read only directories or files.
        /// </summary>
        /// <param name="fileOrDirectory">File or directory to delete.</param>
        /// <exception cref="ArgumentNullException">When file or direcory is null.</exception>
        public static void DeleteFileSystemInfo(this IFileSystemInfo fileOrDirectory)
        {
            if (fileOrDirectory == null)
            {
                throw new ArgumentNullException("fileOrDirectory");
            }

            fileOrDirectory.Attributes = FileAttributes.Normal;
            var di = fileOrDirectory as IDirectoryInfo;
            if (di != null)
            {
                foreach (var dirInfo in di.GetFileSystemInfos())
                {
                    DeleteFileSystemInfo(dirInfo);
                }
            }
            fileOrDirectory.Delete();
        }

        /// <summary>
        /// Deletes a file or directory including read only directories or files.
        /// </summary>
        /// <param name="fileOrDirectory">File or directory to delete.</param>
        /// <exception cref="ArgumentNullException">When file or direcory is null.</exception>
        public static void DeleteFileSystemInfo(this FileSystemInfo fileOrDirectory)
        {
            if (fileOrDirectory == null)
            {
                throw new ArgumentNullException("fileOrDirectory");
            }
            FileInfoWrapper fiw = new FileInfoWrapper(fileOrDirectory);
            fiw.DeleteFileSystemInfo();
        }

        /// <summary>
        ///   This method extends the LINQ methods to flatten a collection of 
        ///   items that have a property of children of the same type.
        /// </summary>
        /// <typeparam name = "T">Item type.</typeparam>
        /// <param name = "source">Source collection.</param>
        /// <param name = "childPropertySelector">
        ///   Child property selector delegate of each item. 
        ///   IEnumerable'T' childPropertySelector(T itemBeingFlattened)
        /// </param>
        /// <returns>Returns a one level list of elements of type T.</returns>
        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>> childPropertySelector)
        {
            return source
                .Flatten((itemBeingFlattened, objectsBeingFlattened) =>
                         childPropertySelector(itemBeingFlattened));
        }

        /// <summary>
        ///   This method extends the LINQ methods to flatten a collection of 
        ///   items that have a property of children of the same type.
        /// </summary>
        /// <typeparam name = "T">Item type.</typeparam>
        /// <param name = "source">Source collection.</param>
        /// <param name = "childPropertySelector">
        ///   Child property selector delegate of each item. 
        ///   IEnumerable'T' childPropertySelector(T itemBeingFlattened, IEnumerable'T' objectsBeingFlattened)
        /// </param>
        /// <returns>Returns a one level list of elements of type T.</returns>
        public static IEnumerable<T> Flatten<T>(
            this IEnumerable<T> source,
            Func<T, IEnumerable<T>, IEnumerable<T>> childPropertySelector)
        {
            var enumerable = source as IList<T> ?? source.ToList();
            return enumerable
                .Concat(enumerable
                            .Where(item => childPropertySelector(item, enumerable) != null)
                            .SelectMany(itemBeingFlattened =>
                                        childPropertySelector(itemBeingFlattened, enumerable)
                                            .Flatten(childPropertySelector)));
        }

        /// <summary>
        /// Retreives all files in supplied directories.
        /// </summary>
        /// <param name="source">List of directories whose files to get.</param>
        /// <returns>An array of files in all supplied directories></returns>
        public static IFileInfo[] GetAllFiles(this IEnumerable<IDirectoryInfo> source)
        {
            var files = new List<IFileInfo>();
            foreach (var directoryInfo in source)
            {
                files.AddRange(directoryInfo.GetFiles());
                files.AddRange(GetAllFiles(directoryInfo.GetDirectories()));
            }
            return files.ToArray();
        }

        /// <summary>
        /// Determines whether a any parent of a file has specified attribute.
        /// </summary>
        /// <param name="file">File whose parent to check.</param>
        /// <param name="attributes">Attributes the parent must have.</param>
        /// <returns><code>true</code> if any of the parents have an attribute, <code>false</code> otherwise.</returns>
        public static bool ParentHasAttribute(this IFileInfo file, params FileAttributes[] attributes)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (attributes == null) throw new ArgumentNullException("attributes");
            var parent = file.Directory;
            return ParentHasAttributeHelper(attributes, parent);
        }

        /// <summary>
        /// Retreives all files in supplied directories.
        /// </summary>
        /// <param name="directory">Directory whose parent to check.</param>
        /// <param name="attributes">Attributes the parent must have.</param>
        /// <returns><code>true</code> if any of the parents have an attribute, <code>false</code> otherwise.</returns>
        public static bool ParentHasAttribute(this IDirectoryInfo directory, params FileAttributes[] attributes)
        {
            if (directory == null) throw new ArgumentNullException("directory");
            if (attributes == null) throw new ArgumentNullException("attributes");
            var parent = directory.Parent;
            return ParentHasAttributeHelper(attributes, parent);
        }

        private static bool ParentHasAttributeHelper(FileAttributes[] attributes, IDirectoryInfo parent)
        {
            while (parent != null)
            {
                bool result = true;
                foreach (var attribute in attributes)
                {
                    result = result && (parent.Attributes & attribute) == attribute;
                }
                if (result)
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }

        /// <summary>
        /// Gets file system type (Directory, File or None) for a given path.
        /// </summary>
        /// <param name="path">Path for which to get the file sytsem type.</param>
        /// <returns><see cref="FileSystemType.Directory"/> if a path is a valid directory, <see cref="FileSystemType.File"/> if a path is a valid file 
        /// or <see cref="FileSystemType.None"/> if a path is not valid or FileSystem object does not exist.</returns>
        public static FileSystemType GetFileSystemType(this string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            try
            {
                var attr = File.GetAttributes(path);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    return FileSystemType.Directory;
                }
                else
                {
                    return FileSystemType.File;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error determining file system type.");
                _log.Error(ex.Message, ex);
            }
            return FileSystemType.None;
        }
    }
}