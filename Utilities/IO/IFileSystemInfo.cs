using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace Utilities.IO
{
    public interface IFileSystemInfo
    {
        /// <summary>
        ///   Gets the full path of the directory or file.
        /// </summary>
        /// <returns> A string containing the full path. </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        string FullName { get; }

        /// <summary>
        ///   Gets the string representing the extension part of the file.
        /// </summary>
        /// <returns> A string containing the <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> extension. </returns>
        /// <filterpriority>1</filterpriority>
        string Extension { get; }

        /// <summary>
        ///   For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.
        /// </summary>
        /// <returns> A string that is the name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension. </returns>
        /// <filterpriority>1</filterpriority>
        string Name { get; }

        /// <summary>
        ///   Gets a value indicating whether the file or directory exists.
        /// </summary>
        /// <returns> true if the file or directory exists; otherwise, false. </returns>
        /// <filterpriority>1</filterpriority>
        bool Exists { get; }

        /// <summary>
        ///   Gets or sets the creation time of the current file or directory.
        /// </summary>
        /// <returns> The creation date and time of the current <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> object. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        DateTime CreationTime { get; set; }

        /// <summary>
        ///   Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.
        /// </summary>
        /// <returns> The creation date and time in UTC format of the current <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> object. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        DateTime CreationTimeUtc { get; set; }

        /// <summary>
        ///   Gets or sets the time the current file or directory was last accessed.
        /// </summary>
        /// <returns> The time that the current file or directory was last accessed. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        DateTime LastAccessTime { get; set; }

        /// <summary>
        ///   Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.
        /// </summary>
        /// <returns> The UTC time that the current file or directory was last accessed. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        DateTime LastAccessTimeUtc { get; set; }

        /// <summary>
        ///   Gets or sets the time when the current file or directory was last written to.
        /// </summary>
        /// <returns> The time the current file was last written. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        DateTime LastWriteTime { get; set; }

        /// <summary>
        ///   Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.
        /// </summary>
        /// <returns> The UTC time when the current file was last written to. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        DateTime LastWriteTimeUtc { get; set; }

        /// <summary>
        ///   Gets or sets the attributes for the current file or directory.
        /// </summary>
        /// <returns> <see cref="T:System.IO.FileAttributes" /> of the current <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> . </returns>
        /// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentException">The caller attempts to set an invalid file attribute.</exception>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        FileAttributes Attributes { get; set; }

        /// <summary>
        ///   Gets the size.
        /// </summary>
        long Size { get; }

        /// <summary>
        ///   Retrieves the current lifetime service object that controls the lifetime policy for this instance.
        /// </summary>
        /// <returns> An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        object GetLifetimeService();

        /// <summary>
        ///   Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns> An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see
        ///    cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property. </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        object InitializeLifetimeService();

        /// <summary>
        ///   Creates an object that contains all the relevant information required to generate a proxy used to communicate with a remote object.
        /// </summary>
        /// <returns> Information required to generate a proxy. </returns>
        /// <param name="requestedType"> The <see cref="T:System.Type" /> of the object that the new <see
        ///    cref="T:System.Runtime.Remoting.ObjRef" /> will reference. </param>
        /// <exception cref="T:System.Runtime.Remoting.RemotingException">This instance is not a valid remoting object.</exception>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="Infrastructure" />
        /// </PermissionSet>
        ObjRef CreateObjRef(Type requestedType);

        /// <summary>
        ///   Deletes a file or directory.
        /// </summary>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <filterpriority>2</filterpriority>
        void Delete();

        /// <summary>
        ///   Refreshes the state of the object.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">A device such as a disk drive is not ready.</exception>
        /// <filterpriority>1</filterpriority>
        void Refresh();

        /// <summary>
        ///   Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.
        /// </summary>
        /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        void GetObjectData(SerializationInfo info, StreamingContext context);

        /// <summary>
        ///  Compares <code>IFileSystemInfo</code> to another on name, last write time and attributes.
        /// </summary>
        /// <param name="fileSystemInfo">Comparison <code>IFileSystemInfo</code>.</param>
        /// <returns> <code>true</code> if <code>IFileSystemInfo</code> are identical, <code>false</code> otherwise. </returns>
        bool IsIdenticalTo(IFileSystemInfo fileSystemInfo);

        /// <summary>
        ///  Compares <code>IFileSystemInfo</code> to <code>FileSystemInfo</code> on name, last write time and attributes.
        /// </summary>
        /// <param name="fileSystemInfo">Comparison <code>FileSystemInfo</code>.</param>
        /// <returns> <code>true</code> if <code>IFileSystemInfo</code> and <code>FileSystemInfo</code> are identical, <code>false</code> otherwise. </returns>
        bool IsIdenticalTo(FileSystemInfo fileSystemInfo);
    }
}