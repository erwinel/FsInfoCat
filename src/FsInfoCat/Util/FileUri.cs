using System;
using System.Collections.Generic;
using System.IO;

namespace FsInfoCat.Util
{
    /// <summary>
    /// Represents a file URI that can be sorted and compared.
    /// </summary>
    public class FileUri : IEquatable<FileUri>
    {
        private readonly int _nameIndex;

        ///// <summary>
        ///// Indicates whether the path represents a subdirectory (ends with <c>/</c>)
        ///// </summary>
        //public bool IsDirectory { get; }

        ///// <summary>
        ///// Indicates whether the path represents a subdirectory (<see cref="Segments"/>[0] is <c>/</c>)
        ///// </summary>
        //public bool IsAbsolute { get; }

        ///// <summary>
        ///// Indicates whether this is an empty file URI (lengths of <see cref="Segments"/> and <see cref="Host"/> are zero).
        ///// </summary>
        //public bool IsEmpty { get; }

        /// <summary>
        /// URI encode host name.
        /// </summary>
        public string Host { get; }

        public string AbsolutePath { get; }

        ///// <summary>
        ///// URI path segments.
        ///// </summary>
        //public ReadOnlyCollection<string> Segments { get; }

        ///// <summary>
        ///// Create new <c>FileUri</c> to represent a parent <c>FileUri</c>.
        ///// </summary>
        ///// <param name="host">The host name.</param>
        ///// <param name="segments">The parent path segments</param>
        //private FileUri(string host, IEnumerable<string> segments, bool isAbsolute)
        //{
        //    Host = host;
        //    string[] arr = segments.ToArray();
        //    _nameIndex = arr.Length - 1;
        //    if (_nameIndex < 0)
        //    {
        //        Segments = new ReadOnlyCollection<string>(new string[0]);
        //    }
        //    else
        //    {
        //        string name = arr[_nameIndex];
        //        int len = name.Length - 1;
        //        if (len > 0 && name[len] == UriHelper.URI_PATH_SEPARATOR_CHAR)
        //            arr[_nameIndex] = name.Substring(0, len);
        //    }
        //    Segments = new ReadOnlyCollection<string>(arr);
        //    IsDirectory = true;
        //    IsEmpty = _nameIndex < 0 && !isAbsolute;
        //    IsAbsolute = isAbsolute;
        //}

        /// <summary>
        /// Creates a new <c>FileUri</c> object.
        /// </summary>
        /// <param name="fileUriString">The well-formed relative or absolute file URI. A <see langword="null"/> or empty value results in an <see cref="IsEmpty">empty</see> file URI.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="fileUriString"/> is not a well-formed file URI string or it is not a well-formed relative URI string.
        /// <para>This exception will also be thrown if any path segments are empty (2 or more consecutive <c>/</c> characters).</para></exception>
        /// <remarks>The <seealso cref="Uri.IsWellFormedUriString(string, UriKind)"/> method is utilized to determine if <paramref name="fileUriString"/> is well-formed.
        /// <para><see cref="IsDirectory"/> will be set to <see langword="true"/> if <paramref name="fileUriString"/> ends with the <c>/</c> character, is a file URI with no path specified, or if <paramref name="fileUriString"/> is null or empty.</para>
        /// <para><seealso cref="UriHelper.AsNormalized(Uri)"/></para></remarks>
        public FileUri(string fileUriString = "")
        {
            if (string.IsNullOrEmpty(fileUriString))
            {
                Host = AbsolutePath = "";
                _nameIndex = 0;
            }
            else if (UriHelper.TryParseFileUriString(fileUriString, out string hostName, out string absolutePath, out int leafIndex))
            {
                Host = hostName.ToLower();
                _nameIndex = leafIndex;
                AbsolutePath = absolutePath;
            }
            else
                throw new ArgumentOutOfRangeException(nameof(fileUriString));
        }

        /// <summary>
        /// Creates a <see cref="FileUri"/> from a <see cref="FileSystemInfo"/> object representing a local file system path.
        /// </summary>
        /// <param name="fileSystemInfo">The<see cref="FileSystemInfo"/> object representing a local file system path.</param>
        public FileUri(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo is null)
            {
                Host = AbsolutePath = "";
                _nameIndex = 0;
            }
            else
            {
                Uri uri = fileSystemInfo.ToUri();
                Host = uri.Host.ToLower();
                string absolutePath = uri.AbsolutePath;
                int i = absolutePath.LastIndexOf(UriHelper.URI_PATH_SEPARATOR_CHAR);
                if (i > 0 && absolutePath[i] == UriHelper.URI_PATH_SEPARATOR_CHAR)
                {
                    AbsolutePath = absolutePath.Substring(0, i);
                    i = AbsolutePath.LastIndexOf(UriHelper.URI_PATH_SEPARATOR_CHAR);
                }
                else
                    AbsolutePath = absolutePath;
                _nameIndex = (i < 0) ? 0 : i + 1;
            }
        }

        //public static implicit operator FileUri(Uri uri)
        //{
        //    if (uri is null)
        //        return null;
        //    if (uri.IsAbsoluteUri)
        //    {
        //        if (uri.Scheme != Uri.UriSchemeFile || !(string.IsNullOrEmpty(uri.Query) && string.IsNullOrEmpty(uri.Fragment)))
        //            return null;
        //        return new FileUri(uri.Host, uri.AbsolutePath);
        //    }
        //    if (Uri.IsWellFormedUriString(uri.OriginalString, UriKind.Relative))
        //        return new FileUri("", uri.OriginalString);
        //    return null;
        //}

        //public static implicit operator Uri(FileUri uri) => (uri is null) ? null : ((uri.IsAbsolute) ?
        //    new Uri($"file://{uri.Host}{string.Join("", uri.Segments)}", UriKind.Absolute) : new Uri(string.Join("", uri.Segments), UriKind.Absolute));

        public static implicit operator FileUri(FileSystemInfo fsi) => (fsi is null) ? null : new FileUri(fsi);

        public bool IsEmpty() => AbsolutePath.Length == 0 && Host.Length == 0;

        public string GetName() => Uri.UnescapeDataString(AbsolutePath.Substring(_nameIndex));

        public bool Equals(FileUri other, IEqualityComparer<string> comparer) => !(other is null) && Host.Equals(other.Host) && comparer.Equals(AbsolutePath, other.AbsolutePath);

        /// <summary>
        /// Creates a local path string from file URI.
        /// </summary>
        /// <returns>A local path string from file URI.</returns>
        public string ToLocalPath()
        {
            if (IsEmpty())
                return "";
            return new Uri(ToString()).LocalPath;
        }

        /// <summary>
        /// Gets a <seealso cref="DirectoryInfo"/> or <seealso cref="FileInfo"/> representing the current <c>FileUri</c>.
        /// </summary>
        /// <returns>A <seealso cref="DirectoryInfo"/> or <seealso cref="FileInfo"/> representing the current <c>FileUri</c> or <see langword="null"/> if the current
        /// <c>FileUri</c> <see cref="IsEmpty"/> is <see langword="true"/>, <see cref="IsAbsolute"/> is <see langword="false"/>, or if a <seealso cref="FileSystemInfo"/>
        /// object could not be created.</returns>
        public FileSystemInfo ToFileSystemInfo()
        {
            if (IsEmpty())
                return null;
            try
            {
                return new FileInfo(new Uri(ToString()).LocalPath);
            }
            catch
            {
                return null;
            }
        }

        public bool Equals(FileUri other) => !(other is null) && (ReferenceEquals(this, other) || Host.Equals(other.Host) && AbsolutePath.Equals(other.AbsolutePath));

        public override bool Equals(object obj) => obj is FileUri fileUri && Equals(fileUri);

        public override int GetHashCode() => ToString().GetHashCode();

        public override string ToString() => (IsEmpty()) ? "" : $"file://{Host}/{AbsolutePath}/";
    }
}