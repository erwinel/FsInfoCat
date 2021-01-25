using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsRoot : IFsDirectory, IEquatable<FsRoot>, IEqualityComparer<IFsChildNode>
    {
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName { get; set; }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string FileSystemName { get; set; }

        public DriveType DriveType { get; private set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName { get; set; }

        private static IFsDirectory ImportDirectory(Collection<FsRoot> fsRoots, string path, Dictionary<string, DriveInfo> drives, out string realPath)
        {
            string key = path;
            FsRoot root;
            if (drives.ContainsKey(key) || null != (key = drives.Keys.FirstOrDefault(d => StringComparer.InvariantCultureIgnoreCase.Equals(d, path))))
            {
                if (null == (root = fsRoots.FirstOrDefault(r => StringComparer.InvariantCulture.Equals(key))))
                {
                    root = new FsRoot(drives[key]);
                    fsRoots.Add(root);
                }
                realPath = key;
                return root;
            }
            string name = Path.GetFileName(path);
            string directoryName = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directoryName))
                root = null;
            else
            {
                IFsDirectory parent = ImportDirectory(fsRoots, directoryName, drives, out realPath);
                if (null != parent)
                {
                    string[] names = Directory.GetDirectories(directoryName).Select(p => Path.GetFileName(p)).ToArray();
                    if (names.Any(n => StringComparer.InvariantCulture.Equals(n, name)) || null != (name = names.FirstOrDefault(n => StringComparer.InvariantCultureIgnoreCase.Equals(n, name))))
                    {
                        FsDirectory result = parent.ChildNodes.OfType<FsDirectory>().FirstOrDefault(d => StringComparer.InvariantCulture.Equals(d.Name, name));
                        if (null == result)
                        {
                            result = new FsDirectory() { Name = name };
                            parent.ChildNodes.Add(result);
                        }
                        realPath = Path.Combine(realPath, name);
                        return result;
                    }
                }
            }
            realPath = null;
            return null;
        }

        public static string NormalizePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            path = Path.GetFullPath(path);
            while (string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                string d = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(d))
                    break;
                path = d;
            }
            return path;
        }

        public static IFsDirectory ImportDirectory(Collection<FsRoot> fsRoots, string path, out string realPath)
        {
            if ((path = NormalizePath(path)).Length > 0 && Directory.Exists(path))
                return ImportDirectory(fsRoots, path, DriveInfo.GetDrives().ToDictionary(k => k.RootDirectory.FullName, v => v), out realPath);
            realPath = null;
            return null;
        }

        private Collection<IFsChildNode> _childNodes = null;
        public Collection<IFsChildNode> ChildNodes
        {
            get
            {
                Collection<IFsChildNode> childNodes = _childNodes;
                if (null == childNodes)
                    _childNodes = childNodes = new Collection<IFsChildNode>();
                return childNodes;
            }
            set { _childNodes = value; }
        }

        public FsRoot(DriveInfo driveInfo)
        {
            if (null == driveInfo)
                throw new ArgumentNullException(nameof(driveInfo));
            FileSystemName = driveInfo.DriveFormat;
            DriveType = driveInfo.DriveType;
            VolumeName = driveInfo.VolumeLabel;
        }

        public FsRoot() { }

        /// <summary>
        /// Looks for the first nested partial crawl.
        /// </summary>
        /// <param name="message">The <seealso cref="PartialCrawlWarning" /> representing the partial crawl.</param>
        /// <param name="segments">The <seealso cref="IFsDirectory" /> representing the path segments, relative to the root path, for the parent directory of the partial crawl.
        /// The last segment will contain the items that were already crawled.</param>
        /// <returns>True if a <seealso cref="PartialCrawlWarning" /> was found; otherwise, false.</returns>
        public bool TryFindPartialCrawl(out PartialCrawlWarning message, out IEnumerable<IFsDirectory> segments)
        {
            message = Messages.OfType<PartialCrawlWarning>().Where(m => m.NotCrawled.Any(s => !string.IsNullOrWhiteSpace(s))).FirstOrDefault();
            if (null != message)
            {
                segments = new IFsDirectory[0];
                return true;
            }
            foreach (IFsDirectory root in ChildNodes.OfType<IFsDirectory>())
            {
                if (root.TryFindPartialCrawl(out message, out segments))
                {
                    segments = (new IFsDirectory[] { root }).Concat(segments);
                    return true;
                }
            }
            segments = new IFsDirectory[0];
            return false;
        }

        private Collection<ICrawlMessage> _messages = null;
        public Collection<ICrawlMessage> Messages
        {
            get
            {
                Collection<ICrawlMessage> messages = _messages;
                if (null == messages)
                    _messages = messages = new Collection<ICrawlMessage>();
                return messages;
            }
            set { _messages = value; }
        }

        public bool Equals(FsRoot other)
        {
            return null != other && (ReferenceEquals(this, other) || (DriveType == other.DriveType && String.Equals(FileSystemName, other.FileSystemName, StringComparison.InvariantCultureIgnoreCase) &&
                String.Equals(VolumeName, other.VolumeName, StringComparison.InvariantCultureIgnoreCase) && RootPathName.Equals(other.RootPathName)));
        }

        public override bool Equals(object obj)
        {
            return null != obj && obj is FsRoot && Equals((FsRoot)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int GetHashCode(IFsChildNode obj)
        {
            string n;
            return StringComparer.InvariantCultureIgnoreCase.GetHashCode((null == obj || null == (n = obj.Name)) ? "" : n);
        }

        public bool Equals(IFsChildNode x, IFsChildNode y)
        {
            if (null == x)
                return null == y;
            if (null == y)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            string a = x.Name;
            string b = y.Name;
            if (null == a)
                return null == b;
            return null != b && StringComparer.InvariantCultureIgnoreCase.Equals(a, b);
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(RootPathName))
                return (null == VolumeName) ? "" : " " + VolumeName.Trim();
            if (String.IsNullOrWhiteSpace(VolumeName))
                return RootPathName.Trim();
            return RootPathName.Trim() + Path.PathSeparator + " " + VolumeName.Trim();
        }
    }
}
