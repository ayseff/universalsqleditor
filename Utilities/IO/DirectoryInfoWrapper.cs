using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;

namespace Utilities.IO
{
    public class DirectoryInfoWrapper : FileSystemInfoWrapper, IDirectoryInfo
    {
        private readonly DirectoryInfo _di;

        public DirectoryInfoWrapper(FileSystemInfo di)
            : this(new DirectoryInfo(di.FullName))
        {
        }

        public DirectoryInfoWrapper(DirectoryInfo di)
            : base(di)
        {
            _size = -1;
            _di = di;
        }

        public DirectoryInfoWrapper(string path)
            : this(new DirectoryInfo(path))
        {
        }

        #region IDirectoryInfo Members
        /// <summary>
        ///   Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see
        ///    cref="T:Utilities.IO.DirectoryInfoWrapper" /> class.
        /// </summary>
        /// <returns> The last directory specified in <paramref name="path" /> . </returns>
        /// <param name="path"> The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="path" />
        ///   does not specify a valid file path or contains invalid DirectoryInfoWrapper characters.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="path" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException">The subdirectory cannot be created.-or- A file or directory already has the name specified by
        ///   <paramref name="path" />
        ///   .</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. The specified path, file name, or both are too long.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have code access permission to create the directory.-or-The caller does not have code access permission to read the directory described by the returned
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object.  This can occur when the
        ///   <paramref name="path" />
        ///   parameter describes an existing directory.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   <paramref name="path" />
        ///   contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo CreateSubdirectory(string path)
        {
            return new DirectoryInfoWrapper(_di.CreateSubdirectory(path));
        }

        /// <summary>
        ///   Creates a subdirectory or subdirectories on the specified path with the specified security. The specified path can be relative to this instance of the <see
        ///    cref="T:Utilities.IO.DirectoryInfoWrapper" /> class.
        /// </summary>
        /// <returns> The last directory specified in <paramref name="path" /> . </returns>
        /// <param name="path"> The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name. </param>
        /// <param name="directorySecurity"> The security to apply. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="path" />
        ///   does not specify a valid file path or contains invalid DirectoryInfoWrapper characters.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="path" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException">The subdirectory cannot be created.-or- A file or directory already has the name specified by
        ///   <paramref name="path" />
        ///   .</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters. The specified path, file name, or both are too long.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have code access permission to create the directory.-or-The caller does not have code access permission to read the directory described by the returned
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object.  This can occur when the
        ///   <paramref name="path" />
        ///   parameter describes an existing directory.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   <paramref name="path" />
        ///   contains a colon character (:) that is not part of a drive label ("C:\").</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return new DirectoryInfoWrapper(_di.CreateSubdirectory(path, directorySecurity));
        }

        /// <summary>
        ///   Creates a directory using a <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object.
        /// </summary>
        /// <param name="directorySecurity"> The access control to apply to the directory. </param>
        /// <exception cref="T:System.IO.IOException">The directory specified by
        ///   <paramref name="path" />
        ///   is read-only or is not empty.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="path" />
        ///   is a zero-length string, contains only white space, or contains one or more invalid characters as defined by
        ///   <see cref="F:System.IO.Path.InvalidPathChars" />
        ///   .</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="path" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.NotSupportedException">Creating a directory with only the colon (:) character was attempted.</exception>
        /// <exception cref="T:System.IO.IOException">The directory specified by
        ///   <paramref name="path" />
        ///   is read-only or is not empty.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void Create(DirectorySecurity directorySecurity)
        {
            _di.Create(directorySecurity);
        }

        /// <summary>
        ///   Creates a directory.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">The directory cannot be created.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void Create()
        {
            _di.Create();
        }

        /// <summary>
        ///   Gets a <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object that encapsulates the access control list (ACL) entries for the directory described by the current <see
        ///    cref="T:Utilities.IO.DirectoryInfoWrapper" /> object.
        /// </summary>
        /// <returns> A <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object that encapsulates the access control rules for the directory. </returns>
        /// <exception cref="T:System.SystemException">The directory could not be found or modified.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The current process does not have access to open the directory.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the directory.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The directory is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DirectorySecurity GetAccessControl()
        {
            return _di.GetAccessControl();
        }

        /// <summary>
        ///   Gets a <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object that encapsulates the specified type of access control list (ACL) entries for the directory described by the current <see
        ///    cref="T:Utilities.IO.DirectoryInfoWrapper" /> object.
        /// </summary>
        /// <returns> A <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object that encapsulates the access control rules for the file described by the <paramref
        ///    name="path" /> parameter.ExceptionsException typeCondition <see cref="T:System.SystemException" /> The directory could not be found or modified. <see
        ///    cref="T:System.UnauthorizedAccessException" /> The current process does not have access to open the directory. <see
        ///    cref="T:System.IO.IOException" /> An I/O error occurred while opening the directory. <see
        ///    cref="T:System.PlatformNotSupportedException" /> The current operating system is not Microsoft Windows 2000 or later. <see
        ///    cref="T:System.UnauthorizedAccessException" /> The directory is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission. </returns>
        /// <param name="includeSections"> One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> values that specifies the type of access control list (ACL) information to receive. </param>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return _di.GetAccessControl(includeSections);
        }

        public new void Refresh()
        {
            _size = -1;
            _di.Refresh();
        }

        /// <summary>
        ///   Applies access control list (ACL) entries described by a <see cref="T:System.Security.AccessControl.DirectorySecurity" /> object to the directory described by the current <see
        ///    cref="T:Utilities.IO.DirectoryInfoWrapper" /> object.
        /// </summary>
        /// <param name="directorySecurity"> An object that describes an ACL entry to apply to the directory described by the <paramref
        ///    name="path" /> parameter. </param>
        /// <exception cref="T:System.ArgumentNullException">The
        ///   <paramref name="directorySecurity" />
        ///   parameter is null.</exception>
        /// <exception cref="T:System.SystemException">The file could not be found or modified.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The current process does not have access to open the file.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void SetAccessControl(DirectorySecurity directorySecurity)
        {
            _di.SetAccessControl(directorySecurity);
        }

        /// <summary>
        ///   Returns a file list from the current directory matching the given search pattern.
        /// </summary>
        /// <returns> An array of type <see cref="T:Utilities.IO.FileInfoWrapper" /> . </returns>
        /// <param name="searchPattern"> The search string, such as "*.txt". </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="searchPattern " />
        ///   contains one or more invalid characters defined by the
        ///   <see cref="M:System.IO.Path.GetInvalidPathChars" />
        ///   method.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileInfo[] GetFiles(string searchPattern)
        {
            return _di.GetFiles(searchPattern).Select(f => new FileInfoWrapper(f)).Cast<IFileInfo>().ToArray();
        }

        /// <summary>
        ///   Returns a file list from the current directory matching the given search pattern and using a value to determine whether to search subdirectories.
        /// </summary>
        /// <returns> An array of type <see cref="T:Utilities.IO.FileInfoWrapper" /> . </returns>
        /// <param name="searchPattern"> The search string. For example, "System*" can be used to search for all directories that begin with the word "System". </param>
        /// <param name="searchOption"> One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="searchPattern " />
        ///   contains one or more invalid characters defined by the
        ///   <see cref="M:System.IO.Path.GetInvalidPathChars" />
        ///   method.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="searchOption" />
        ///   is not a valid
        ///   <see cref="T:System.IO.SearchOption" />
        ///   value.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return _di.GetFiles(searchPattern, searchOption).Select(f => new FileInfoWrapper(f)).Cast<IFileInfo>().ToArray();
        }

        /// <summary>
        ///   Returns a file list from the current directory.
        /// </summary>
        /// <returns> An array of type <see cref="T:Utilities.IO.FileInfoWrapper" /> . </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid, such as being on an unmapped drive.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileInfo[] GetFiles()
        {
            return _di.GetFiles().Select(f => new FileInfoWrapper(f)).Cast<IFileInfo>().ToArray();
        }

        /// <summary>
        ///   Returns the subdirectories of the current directory.
        /// </summary>
        /// <returns> An array of <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> objects. </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo[] GetDirectories()
        {
            return _di.GetDirectories().Select(d => new DirectoryInfoWrapper(d)).Cast<IDirectoryInfo>().ToArray();
        }

        /// <summary>
        ///   Retrieves an array of strongly typed <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> objects representing the files and subdirectories that match the specified search criteria.
        /// </summary>
        /// <returns> An array of strongly typed FileSystemInfoWrapper objects matching the search criteria. </returns>
        /// <param name="searchPattern"> The search string. For example, "System*" can be used to search for all directories that begin with the word "System". </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="searchPattern " />
        ///   contains one or more invalid characters defined by the
        ///   <see cref="M:System.IO.Path.GetInvalidPathChars" />
        ///   method.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
            //return _di.GetFileSystemInfos(searchPattern).Select(d => new FileSystemInfoWrapper(d)).Cast<IFileSystemInfo>().ToArray();
        }

        /// <summary>
        ///   Retrieves an array of <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> objects that represent the files and subdirectories matching the specified search criteria.
        /// </summary>
        /// <returns> An array of file system entries that match the search criteria. </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all files and directories. </param>
        /// <param name="searchOption"> One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see
        ///    cref="F:System.IO.SearchOption.TopDirectoryOnly" /> . </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="searchPattern " />
        ///   contains one or more invalid characters defined by the
        ///   <see cref="M:System.IO.Path.GetInvalidPathChars" />
        ///   method.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="searchOption" />
        ///   is not a valid
        ///   <see cref="T:System.IO.SearchOption" />
        ///   value.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            var infos = new List<IFileSystemInfo>();
            infos.AddRange(_di.GetDirectories(searchPattern, searchOption).Select(d => new DirectoryInfoWrapper(d)));
            infos.AddRange(_di.GetFiles(searchPattern, searchOption).Select(d => new FileInfoWrapper(d)));
            return infos.ToArray();
        }

        /// <summary>
        ///   Returns an array of strongly typed <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> entries representing all the files and subdirectories in a directory.
        /// </summary>
        /// <returns> An array of strongly typed <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> entries. </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path is invalid (for example, it is on an unmapped drive).</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileSystemInfo[] GetFileSystemInfos()
        {
            return GetFileSystemInfos("*", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        ///   Returns an array of directories in the current <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> matching the given search criteria.
        /// </summary>
        /// <returns> An array of type DirectoryInfoWrapper matching <paramref name="searchPattern" /> . </returns>
        /// <param name="searchPattern"> The search string. For example, "System*" can be used to search for all directories that begin with the word "System". </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="searchPattern " />
        ///   contains one or more invalid characters defined by the
        ///   <see cref="M:System.IO.Path.GetInvalidPathChars" />
        ///   method.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the DirectoryInfoWrapper object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo[] GetDirectories(string searchPattern)
        {
            return _di.GetDirectories(searchPattern).Select(d => new DirectoryInfoWrapper(d)).Cast<IDirectoryInfo>().ToArray();
        }

        /// <summary>
        ///   Returns an array of directories in the current <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> matching the given search criteria and using a value to determine whether to search subdirectories.
        /// </summary>
        /// <returns> An array of type DirectoryInfoWrapper matching <paramref name="searchPattern" /> . </returns>
        /// <param name="searchPattern"> The search string. For example, "System*" can be used to search for all directories that begin with the word "System". </param>
        /// <param name="searchOption"> One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="searchPattern " />
        ///   contains one or more invalid characters defined by the
        ///   <see cref="M:System.IO.Path.GetInvalidPathChars" />
        ///   method.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="searchOption" />
        ///   is not a valid
        ///   <see cref="T:System.IO.SearchOption" />
        ///   value.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the DirectoryInfoWrapper object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return _di.GetDirectories(searchPattern, searchOption).Select(d => new DirectoryInfoWrapper(d)).Cast<IDirectoryInfo>().ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of directory information in the current directory.
        /// </summary>
        /// <returns> An enumerable collection of directories in the current directory. </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IDirectoryInfo> EnumerateDirectories()
        {
            return _di.EnumerateDirectories().Select(d => new DirectoryInfoWrapper(d)).ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of directory information that matches a specified search pattern.
        /// </summary>
        /// <returns> An enumerable collection of directories that matches <paramref name="searchPattern" /> . </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all directories. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return _di.EnumerateDirectories(searchPattern).Select(d => new DirectoryInfoWrapper(d)).ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <returns> An enumerable collection of directories that matches <paramref name="searchPattern" /> and <paramref
        ///    name="searchOption" /> . </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all directories. </param>
        /// <param name="searchOption"> One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see
        ///    cref="F:System.IO.SearchOption.TopDirectoryOnly" /> . </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="searchOption" />
        ///   is not a valid
        ///   <see cref="T:System.IO.SearchOption" />
        ///   value.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return
                _di.EnumerateDirectories(searchPattern, searchOption).Select(d => new DirectoryInfoWrapper(d)).ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of file information in the current directory.
        /// </summary>
        /// <returns> An enumerable collection of the files in the current directory. </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileInfo> EnumerateFiles()
        {
            return _di.EnumerateFiles().Select(f => new FileInfoWrapper(f)).ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of file information that matches a search pattern.
        /// </summary>
        /// <returns> An enumerable collection of files that matches <paramref name="searchPattern" /> . </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all files. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object is invalid, (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
        {
            return _di.EnumerateFiles(searchPattern).Select(f => new FileInfoWrapper(f)).ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <returns> An enumerable collection of files that matches <paramref name="searchPattern" /> and <paramref
        ///    name="searchOption" /> . </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all files. </param>
        /// <param name="searchOption"> One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see
        ///    cref="F:System.IO.SearchOption.TopDirectoryOnly" /> . </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="searchOption" />
        ///   is not a valid
        ///   <see cref="T:System.IO.SearchOption" />
        ///   value.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return _di.EnumerateFiles(searchPattern, searchOption).Select(f => new FileInfoWrapper(f)).ToArray();
        }

        /// <summary>
        ///   Returns an enumerable collection of file system information in the current directory.
        /// </summary>
        /// <returns> An enumerable collection of file system information in the current directory. </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.FileSystemInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
        {
            return _di.EnumerateFileSystemInfos().Select(f => new FileSystemInfoWrapper(f)).ToList();
        }

        /// <summary>
        ///   Returns an enumerable collection of file system information that matches a specified search pattern.
        /// </summary>
        /// <returns> An enumerable collection of file system information objects that matches <paramref name="searchPattern" /> . </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all files and directories. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.FileSystemInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            return _di.EnumerateFileSystemInfos(searchPattern).Select(f => new FileInfoWrapper(f)).ToList();
        }

        /// <summary>
        ///   Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.
        /// </summary>
        /// <returns> An enumerable collection of file system information objects that matches <paramref name="searchPattern" /> and <paramref
        ///    name="searchOption" /> . </returns>
        /// <param name="searchPattern"> The search string. The default pattern is "*", which returns all files or directories. </param>
        /// <param name="searchOption"> One of the enumeration values that specifies whether the search operation should include only the current directory or all subdirectories. The default value is <see
        ///    cref="F:System.IO.SearchOption.TopDirectoryOnly" /> . </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="searchPattern" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="searchOption" />
        ///   is not a valid
        ///   <see cref="T:System.IO.SearchOption" />
        ///   value.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The path encapsulated in the
        ///   <see cref="T:Utilities.IO.FileSystemInfoWrapper" />
        ///   object is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return _di.EnumerateFileSystemInfos(searchPattern, searchOption).Select(f => new FileInfoWrapper(f)).ToList();
        }

        ///// <summary>
        /////   Deletes this <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> if it is empty.
        ///// </summary>
        ///// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
        ///// <exception cref="T:System.IO.DirectoryNotFoundException">The directory described by this
        /////   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        /////   object does not exist or could not be found.</exception>
        ///// <exception cref="T:System.IO.IOException">The directory is not empty. -or-The directory is the application's current working directory.</exception>
        ///// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        ///// <filterpriority>1</filterpriority>
        ///// <PermissionSet>
        /////   <IPermission
        /////     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        /////     version="1" Unrestricted="true" />
        ///// </PermissionSet>
        //public void Delete()
        //{
        //    _di.Delete();
        //}

        /// <summary>
        ///   Moves a <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> instance and its contents to a new path.
        /// </summary>
        /// <param name="destDirName"> The name and path to which to move this directory. The destination cannot be another disk volume or a directory with the identical name. It can be an existing directory to which you want to add this directory as a subdirectory. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="destDirName" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="destDirName" />
        ///   is an empty string (''").</exception>
        /// <exception cref="T:System.IO.IOException">An attempt was made to move a directory to a different volume. -or-
        ///   <paramref name="destDirName" />
        ///   already exists.-or-You are not authorized to access this path.-or- The directory being moved and the destination directory have the same name.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The destination directory cannot be found.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void MoveTo(string destDirName)
        {
            _di.MoveTo(destDirName);
        }

        /// <summary>
        ///   Deletes this instance of a <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> , specifying whether to delete subdirectories and files.
        /// </summary>
        /// <param name="recursive"> true to delete this directory, its subdirectories, and all files; otherwise, false. </param>
        /// <exception cref="T:System.UnauthorizedAccessException">The directory contains a read-only file.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory described by this
        ///   <see cref="T:Utilities.IO.DirectoryInfoWrapper" />
        ///   object does not exist or could not be found.</exception>
        /// <exception cref="T:System.IO.IOException">The directory is read-only.-or- The directory contains one or more files or subdirectories and
        ///   <paramref name="recursive" />
        ///   is false.-or-The directory is the application's current working directory.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void Delete(bool recursive)
        {
            _di.Delete(recursive);
        }

        /// <summary>
        ///   Gets the parent directory of a specified subdirectory.
        /// </summary>
        /// <returns> The parent directory, or null if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share"). </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo Parent
        {
            get
            {
                if (_di.Parent == null)
                {
                    return null;
                }
                return new DirectoryInfoWrapper(_di.Parent);
            }
        }

        ///// <summary>
        /////   Gets a value indicating whether the directory exists.
        ///// </summary>
        ///// <returns> true if the directory exists; otherwise, false. </returns>
        ///// <filterpriority>1</filterpriority>
        //public bool Exists
        //{
        //    get { return _di.Exists; }
        //}

        /// <summary>
        ///   Gets the root portion of a path.
        /// </summary>
        /// <returns> A <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> object representing the root of a path. </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo Root
        {
            get { return new DirectoryInfoWrapper(_di.Root); }
        }

        /// <summary>
        ///   Compares directory to another on name and attributes.
        /// </summary>
        /// <param name="directory"> Comparison directory. </param>
        /// <returns> <code>true</code> if directories are identical, <code>false</code> otherise. </returns>
        /// <remarks>
        ///   This method does not compare directory contents.
        /// </remarks>
        public bool IsIdenticalTo(IDirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory", "Directory is null.");
            }

            return base.IsIdenticalTo(directory);
            //if (Name != directory.Name
            //    || (Attributes & directory.Attributes) != Attributes
            //    || !LastWriteTime.Equals(directory.LastWriteTime))
            //{
            //    return false;
            //}
            //return true;
        }

        /// <summary>
        ///   Compares directory to another on name and attributes.
        /// </summary>
        /// <param name="directory"> Comparison directory. </param>
        /// <returns> <code>true</code> if directories are identical, <code>false</code> otherise. </returns>
        /// <remarks>
        ///   This method does not compare directory contents.
        /// </remarks>
        public bool IsIdenticalTo(DirectoryInfo directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException("directory", "Directory is null.");
            }

            return base.IsIdenticalTo(directory);
            //if (Name != directory.Name
            //    || (Attributes & directory.Attributes) != Attributes
            //    || !LastWriteTime.Equals(directory.LastWriteTime))
            //{
            //    return false;
            //}
            //return true;
        }

        /// <summary>
        ///   Gets size of the directory by summing up all file sizes contained within the directory and its subdirectories.
        /// </summary>
        public override long Size
        {
            get
            {
                if (_size != -1)
                {
                    return _size;
                }

                _size = _di.GetFiles("*", SearchOption.AllDirectories).Sum(f => f.Length);
                return _size;
            }
        }
        #endregion
    }
}