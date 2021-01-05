using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsDirectory : IFsDirectory, IFsChildNode
    {
        public string Name { get; set; }

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

        private Collection<CrawlError> _errors = null;
        public Collection<CrawlError> Errors
        {
            get
            {
                Collection<CrawlError> errors = _errors;
                if (null == errors)
                    _errors = errors = new Collection<CrawlError>();
                return errors;
            }
            set { _errors = value; }
        }

        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public static FsRoot GetRoot(FsHost host, DirectoryInfo directory, out IFsDirectory branch)
        {
            FsRoot root;
            if (null == directory.Parent)
            {
                VolumeInformation volumeInformation = new VolumeInformation(directory);
                root = host.Roots.FirstOrDefault(h => h.SerialNumber == volumeInformation.SerialNumber && string.Equals(h.VolumeName, volumeInformation.VolumeName, StringComparison.InvariantCultureIgnoreCase));
                if (null == root)
                {
                    root = new FsRoot
                    {
                        FileSystemName = volumeInformation.FileSystemName,
                        MaxNameLength = volumeInformation.MaxNameLength,
                        Flags = volumeInformation.Flags,
                        RootPathName = volumeInformation.RootPathName,
                        SerialNumber = volumeInformation.SerialNumber,
                        VolumeName = volumeInformation.VolumeName,
                    };
                    host.Roots.Add(root);
                }
                branch = root;
            }
            else
            {
                root = GetRoot(host, directory.Parent, out IFsDirectory parent);
                branch = root.Find<FsDirectory>(parent.ChildNodes, directory.Name);
                if (null == branch)
                {
                    FsDirectory childDir = new FsDirectory
                    {
                        Name = directory.Name,
                        CreationTime = directory.CreationTimeUtc,
                        LastWriteTime = directory.LastAccessTimeUtc,
                        Attributes = (int)directory.Attributes
                    };
                    branch = childDir;
                    parent.ChildNodes.Add(childDir);
                }
            }
            return root;
        }

        public static FsDirectory Import(DirectoryInfo directory, int i, IFsDirectory container)
        {
            string name;
            try
            {
                name = directory.Name;
            }
            catch (Exception exc)
            {
                container.Errors.Add(new CrawlError(exc, "Enumerating file #" + (i + 1).ToString()));
                return null;
            }
            FsDirectory item = new FsDirectory { Name = name };
            try { item.CreationTime = directory.CreationTimeUtc; }
            catch (Exception exc) { item.Errors.Add(new CrawlError(exc, "Getting file creation time")); }
            try { item.LastWriteTime = directory.LastWriteTimeUtc; }
            catch (Exception exc) { item.Errors.Add(new CrawlError(exc, "Getting file last write time")); }
            try { item.Attributes = (int)directory.Attributes; }
            catch (Exception exc) { item.Errors.Add(new CrawlError(exc, "Getting file system attributes")); }
            container.ChildNodes.Add(item);
            return item;
        }
    }
}
