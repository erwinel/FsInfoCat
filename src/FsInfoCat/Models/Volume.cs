using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Models
{
    [DisplayColumn("VolumeID", "DisplayName", false)]
    public class Volume : IModficationAuditable
    {
        #region Properties

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid VolumeID { get; set; }

        public Guid? HostID { get; set; }

        #region DisplayName

        public const int Max_Length_DisplayName = 256;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        private string _displayName = "";

        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        #endregion

        #region RootPathName

        public const string DisplayName_RootPathName = "Root Path Name";
        public const string PropertyName_RootPathName = "RootPathName";
        public const int Max_Length_RootPathName = 1024;
        public const string Error_Message_RootPathName_Empty = "Root path name cannot be empty.";
        public const string Error_Message_RootPathName_Length = "Root path name too long.";
        public const string Error_Message_RootPathName_Invalid = "Invalid path or url.";
        private string _rootPathName = "";

        [Required(ErrorMessage = Error_Message_RootPathName_Empty)]
        [MaxLength(Max_Length_RootPathName, ErrorMessage = Error_Message_RootPathName_Length)]
        [RegularExpression(ModelHelper.PATTERN_PATH_OR_URL, ErrorMessage = Error_Message_RootPathName_Invalid)]
        [Display(Name = DisplayName_RootPathName, Description = "Enter a file URI or a windows file path.")]
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName
        {
            get { return _rootPathName; }
            set { _rootPathName = (null == value) ? "" : value; }
        }

        #endregion

        #region FileSystemName

        public const string DisplayName_FileSystemName = "Name of filesystem type";
        public const string PropertyName_FileSystemName = "FileSystemName";
        public const int Max_Length_FileSystemName = 128;
        public const string Error_Message_FileSystemName_Empty = "Name of filesystem cannot be empty.";
        public const string Error_Message_FileSystemName_Length = "Name of filesystem type too long.";
        private string _fileSystemName = "";

        [Required(ErrorMessage = Error_Message_FileSystemName_Empty)]
        [MaxLength(Max_Length_FileSystemName, ErrorMessage = Error_Message_FileSystemName_Length)]
        [Display(Name = DisplayName_FileSystemName)]
        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string FileSystemName
        {
            get { return _fileSystemName; }
            set { _fileSystemName = (null == value) ? "" : value; }
        }

        #endregion

        #region VolumeName

        public const string DisplayName_VolumeName = "Volume name";
        public const string PropertyName_VolumeName = "VolumeName";
        public const int Max_Length_Volume = 128;
        public const string Error_Message_VolumeName_Empty = "Volume name cannot be empty.";
        public const string Error_Message_VolumeName_Length = "Volume name length too long.";
        private string _volumeName = "";

        [Required(ErrorMessage = Error_Message_VolumeName_Empty)]
        [MaxLength(Max_Length_Volume, ErrorMessage = Error_Message_VolumeName_Length)]
        [Display(Name = DisplayName_VolumeName)]
        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName
        {
            get { return _volumeName; }
            set { _volumeName = (null == value) ? "" : value; }
        }

        #endregion

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

        #region Notes

        private string _notes = "";

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        #endregion

        #region Audit

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedBy { get; set; }

        #endregion

        #endregion

        #region Constructors

        public Volume()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        public Volume(string displayName, Guid? hostID, Guid createdBy) : this()
        {
            VolumeID = Guid.NewGuid();
            DisplayName = displayName;
            HostID = hostID;
            CreatedBy = ModifiedBy = createdBy;
        }

        #endregion

        public void Normalize()
        {
            if (VolumeID.Equals(Guid.Empty))
                VolumeID = Guid.NewGuid();
            _rootPathName = _rootPathName.Trim();
            _fileSystemName = _fileSystemName.Trim();
            _rootPathName = _rootPathName.Trim();
            _volumeName = _volumeName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _volumeName;
            _notes = _notes.Trim();
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, PropertyName_DisplayName);
            Validate(result, PropertyName_FileSystemName);
            Validate(result, PropertyName_RootPathName);
            Validate(result, PropertyName_VolumeName);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_DisplayName:
                case DisplayName_DisplayName:
                    if (_displayName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_DisplayName, new string[] { PropertyName_DisplayName }));
                    break;
                case PropertyName_FileSystemName:
                case DisplayName_FileSystemName:
                    if (_fileSystemName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_FileSystemName_Empty, new string[] { PropertyName_FileSystemName }));
                    else if (_fileSystemName.Length > Max_Length_FileSystemName)
                        result.Add(new ValidationResult(Error_Message_FileSystemName_Length, new string[] { PropertyName_FileSystemName }));
                    break;
                case PropertyName_RootPathName:
                case DisplayName_RootPathName:
                    if (_rootPathName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_RootPathName_Empty, new string[] { PropertyName_RootPathName }));
                    else if (_rootPathName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_RootPathName_Length, new string[] { PropertyName_RootPathName }));
                    else if (!ModelHelper.PathOrUrlRegex.IsMatch(_rootPathName))
                        result.Add(new ValidationResult(Error_Message_RootPathName_Invalid, new string[] { PropertyName_RootPathName }));
                    break;
                case PropertyName_VolumeName:
                case DisplayName_VolumeName:
                    if (_volumeName.Length == 0)
                        result.Add(new ValidationResult(Error_Message_VolumeName_Empty, new string[] { PropertyName_VolumeName }));
                    else if (_volumeName.Length > Max_Length_DisplayName)
                        result.Add(new ValidationResult(Error_Message_VolumeName_Length, new string[] { PropertyName_VolumeName }));
                    break;
            }
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            if (string.IsNullOrEmpty(validationContext.DisplayName))
                OnValidateAll(result);
            else
                Validate(result, validationContext.DisplayName);
            return result;
        }
    }
}
