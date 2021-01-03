using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FsInfoCat.Models.Volumes;
#if CORE
using Microsoft.EntityFrameworkCore;
#endif

namespace FsInfoCat.Models.DB
{
#if CORE
    [DisplayColumn("VolumeID", "DisplayName", false)]
#endif
    public class Volume : IVolume
    {
        #region Properties

        public string Name
        {
            get
            {
                string n = _displayName;
                if (string.IsNullOrWhiteSpace(n))
                {
                    n = _volumeName;
                    if (string.IsNullOrWhiteSpace(n))
                        n = _rootPathName;
                }
                return n;
            }
        }

#if CORE
        [Required()]
        [Key()]
        [Display(Name = "ID")]
#endif
        public Guid VolumeID { get; set; }

        public Guid? HostDeviceID { get; set; }

        public HostDevice Host { get; set; }

        public string HostName
        {
            get
            {
                HostDevice host = Host;
                return (null == host) ? "" : host.Name;
            }
        }

        #region DisplayName

        public const int Max_Length_DisplayName = 128;
        public const string DisplayName_DisplayName = "Display name";
        public const string PropertyName_DisplayName = "DisplayName";
        public const string Error_Message_DisplayName = "Display name too long.";
        private string _displayName = "";

#if CORE
        [MaxLength(Max_Length_DisplayName, ErrorMessage = Error_Message_DisplayName)]
        [Display(Name = DisplayName_DisplayName)]
#endif
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }
#if CORE

        public static async Task<List<Volume>> GetByHost(Guid HostDeviceID, DbSet<Volume> dbSet)
        {
            IQueryable<Volume> volumes = from d in dbSet select d;
            return await volumes.Where(v => v.HostDeviceID.HasValue && v.HostDeviceID.Value.Equals(HostDeviceID)).AsNoTracking().ToListAsync();
        }
#endif

        #endregion

        #region RootPathName

        public const string DisplayName_RootPathName = "Root Path Name";
        public const string PropertyName_RootPathName = "RootPathName";
        public const int Max_Length_RootPathName = 1024;
        public const string Error_Message_RootPathName_Empty = "Root path name cannot be empty.";
        public const string Error_Message_RootPathName_Length = "Root path name too long.";
        public const string Error_Message_RootPathName_Invalid = "Invalid path or url.";
        private string _rootPathName = "";

#if CORE
        [Required(ErrorMessage = Error_Message_RootPathName_Empty)]
        [MaxLength(Max_Length_RootPathName, ErrorMessage = Error_Message_RootPathName_Length)]
        [RegularExpression(ModelHelper.PATTERN_PATH_OR_URL, ErrorMessage = Error_Message_RootPathName_Invalid)]
        [Display(Name = DisplayName_RootPathName, Description = "Enter a file URI or a windows file path.")]
#endif
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

#if CORE
        [Required(ErrorMessage = Error_Message_FileSystemName_Empty)]
        [MaxLength(Max_Length_FileSystemName, ErrorMessage = Error_Message_FileSystemName_Length)]
        [Display(Name = DisplayName_FileSystemName)]
#endif
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

#if CORE
        [Required(ErrorMessage = Error_Message_VolumeName_Empty)]
        [MaxLength(Max_Length_Volume, ErrorMessage = Error_Message_VolumeName_Length)]
        [Display(Name = DisplayName_VolumeName)]
#endif
        public string VolumeName
        {
            get { return _volumeName; }
            set { _volumeName = (null == value) ? "" : value; }
        }

        #endregion

#if CORE
        [Required()]
        [Display(Name = "Serial Number")]
#endif
        public uint SerialNumber { get; set; }

#if CORE
        [Required()]
        [Display(Name = "Max Name Length")]
#endif
        public uint MaxNameLength { get; set; }

#if CORE
        [Required()]
        [Display(Name = "Flags")]
        [EnumDataType(typeof(FileSystemFeature))]
#endif
        public FileSystemFeature Flags { get; set; }

#if CORE
        [Display(Name = "Is Inactive")]
#endif
        public bool IsInactive { get; set; }

        #region Notes

        private string _notes = "";

#if CORE
        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
#endif
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        #endregion

        #region Audit

#if CORE
        [Editable(false)]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
#endif
        public DateTime CreatedOn { get; set; }

#if CORE
        [Editable(false)]
        [Display(Name = "Created By")]
#endif
        public Guid CreatedBy { get; set; }

        public Account Creator { get; set; }

        public string CreatorName
        {
            get
            {
                Account account = Creator;
                return (null == account) ? "" : account.Name;
            }
        }

#if CORE
        [Editable(false)]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
#endif
        public DateTime ModifiedOn { get; set; }

#if CORE
        [Editable(false)]
        [Display(Name = "Modified By")]
#endif
        public Guid ModifiedBy { get; set; }

        public Account Modifier { get; set; }

        public string ModifierName
        {
            get
            {
                Account account = Modifier;
                return (null == account) ? "" : account.Name;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public Volume()
        {
            CreatedOn = ModifiedOn = DateTime.UtcNow;
        }

        public Volume(string displayName, HostDevice host, Guid createdBy) : this()
        {
            if (null == createdBy)
                throw new ArgumentNullException("createdBy");
            VolumeID = Guid.NewGuid();
            DisplayName = displayName;
            if (null == host)
                HostDeviceID = null;
            else
                HostDeviceID = host.HostDeviceID;
            CreatedBy = ModifiedBy = createdBy;
        }

        public Volume(string displayName, HostDevice host, Account createdBy) : this()
        {
            if (null == createdBy)
                throw new ArgumentNullException("createdBy");
            VolumeID = Guid.NewGuid();
            DisplayName = displayName;
            if (null == host)
                HostDeviceID = null;
            else
                HostDeviceID = host.HostDeviceID;
            Creator = Modifier = createdBy;
            CreatedBy = ModifiedBy = createdBy.AccountID;
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
            CreatedOn = ModelHelper.CoerceAsLocalTime(CreatedOn);
            ModifiedOn = ModelHelper.CoerceAsLocalTime(ModifiedOn);
            if (null != Creator)
                CreatedBy = Creator.AccountID;
            if (null != Modifier)
                ModifiedBy = Modifier.AccountID;
            if (null != Host)
                HostDeviceID = Host.HostDeviceID;
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
