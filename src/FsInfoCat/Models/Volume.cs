using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class Volume
    {
        public const string PATTERN_PATH_OR_URL = @"(?i)^([a-z]:[\\/]$|file:///[a-z]:/$|([a-z]:|[\\/]{2}([a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3}))([\\/][^\\/:""<>|*?\x00-\x19]+)+[\\/]?$|file://(/[a-z]:|[a-z]+(-[a-z\d]+)*(\.[a-z]+(-[a-z\d]+)*)*|(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)(\.(([01]\d?|[3-9])\d?|2(5[0-5]?|[0-4]?\d?)?)){3})?([\\/][^\\/:""<>|*?\x00-\x19]+)+/?$)";
        private string _displayName = "";
        private string _rootPathName = "";
        private string _fileSystemName = "";
        private string _volumeName = "";
        private string _notes = "";

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid VolumeID { get; set; }

        [Display(Name = "Host ID")]
        public Guid? HostID { get; set; }

        public HostDevice Host { get; set; }

        [MaxLength(256)]
        [Display(Name = "Display Name")]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(1024)]
        [RegularExpression(PATTERN_PATH_OR_URL)]
        [Display(Name = "Root Path Name")]
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName
        {
            get { return _rootPathName; }
            set { _rootPathName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(128)]
        [Display(Name = "Filesystem Name")]
        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string FileSystemName
        {
            get { return _fileSystemName; }
            set { _fileSystemName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(128)]
        [Display(Name = "Volume Name")]
        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName
        {
            get { return _volumeName; }
            set { _volumeName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Serial Number")]
        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        public uint SerialNumber { get; set; }

        [Required()]
        [Display(Name = "Max Name Length")]
        /// <summary>
        /// Gets the maximum length for file/directory names.
        /// </summary>
        public uint MaxNameLength { get; set; }

        [Required()]
        [Display(Name = "Flags")]
        [EnumDataType(typeof(FileSystemFeature))]
        /// <summary>
        /// Gets a value that indicates the volume capabilities and attributes.
        /// </summary>
        public FileSystemFeature Flags { get; set; }

        [Display(Name = "Is Inactive")]
        public bool IsInactive { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public Volume() { }

        public Volume(string displayName, Guid createdBy)
        {
            VolumeID = Guid.NewGuid();
            DisplayName = displayName;
            CreatedOn = ModifiedOn = DateTime.Now;
            CreatedBy = ModifiedBy = createdBy;
        }

    }
}
