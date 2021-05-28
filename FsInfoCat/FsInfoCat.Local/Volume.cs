using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace FsInfoCat.Local
{
    public class Volume : NotifyPropertyChanged, ILocalVolume
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _displayName;
        private readonly IPropertyChangeTracker<string> _volumeName;
        private readonly IPropertyChangeTracker<VolumeIdentifier> _identifier;
        private readonly IPropertyChangeTracker<DriveType> _type;
        private readonly IPropertyChangeTracker<bool?> _caseSensitiveSearch;
        private readonly IPropertyChangeTracker<bool?> _readOnly;
        private readonly IPropertyChangeTracker<int?> _maxNameLength;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<VolumeStatus> _status;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;
        private readonly IPropertyChangeTracker<Subdirectory> _rootDirectory;

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_DisplayName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_LongName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string DisplayName { get => _displayName.GetValue(); set => _displayName.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_VolumeName), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_ShortName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_VolumeNameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string VolumeName { get => _volumeName.GetValue(); set => _volumeName.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_Identifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        //[StringLength(DbConstants.DbColMaxLen_Identifier, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength),
        //    ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        //[Column(TypeName = "nvarchar(1024)")]
        public virtual VolumeIdentifier Identifier { get => _identifier.GetValue(); set => _identifier.SetValue(value); }

        [Required]
        public virtual VolumeStatus Status { get => _status.GetValue(); set => _status.SetValue(value); }

        [Required]
        public virtual DriveType Type { get => _type.GetValue(); set => _type.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CaseSensitiveSearch), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool? CaseSensitiveSearch { get => _caseSensitiveSearch.GetValue(); set => _caseSensitiveSearch.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ReadOnly), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual bool? ReadOnly { get => _readOnly.GetValue(); set => _readOnly.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_MaxNameLength), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_MaxNameLengthInvalid),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual int? MaxNameLength { get => _maxNameLength.GetValue(); set => _maxNameLength.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual Guid FileSystemId
        {
            get => _fileSystemId.GetValue();
            set
            {
                if (_fileSystemId.SetValue(value))
                {
                    FileSystem nav = _fileSystem.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _fileSystem.SetValue(null);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem.GetValue();
            set
            {
                if (_fileSystem.SetValue(value))
                {
                    if (value is null)
                        _fileSystemId.SetValue(Guid.Empty);
                    else
                        _fileSystemId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_RootDirectory), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Subdirectory RootDirectory { get => _rootDirectory.GetValue(); set => _rootDirectory.SetValue(value); }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalVolume.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        IFileSystem IVolume.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        ILocalSubdirectory ILocalVolume.RootDirectory => RootDirectory;

        ISubdirectory IVolume.RootDirectory => RootDirectory;

        #endregion

        public Volume()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _displayName = CreateChangeTracker(nameof(DisplayName), "", TrimmedNonNullStringCoersion.Default);
            _volumeName = CreateChangeTracker(nameof(VolumeName), "", TrimmedNonNullStringCoersion.Default);
            _identifier = CreateChangeTracker<VolumeIdentifier>(nameof(Identifier), default);
            _caseSensitiveSearch = CreateChangeTracker<bool?>(nameof(CaseSensitiveSearch), null);
            _readOnly = CreateChangeTracker<bool?>(nameof(ReadOnly), null);
            _maxNameLength = CreateChangeTracker<int?>(nameof(MaxNameLength), null);
            _type = CreateChangeTracker(nameof(Type), DriveType.Unknown);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _status = CreateChangeTracker(nameof(Status), VolumeStatus.Unknown);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _fileSystemId = CreateChangeTracker(nameof(FileSystemId), Guid.Empty);
            _fileSystem = CreateChangeTracker<FileSystem>(nameof(FileSystem), null);
            _rootDirectory = CreateChangeTracker<Subdirectory>(nameof(RootDirectory), null);
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalVolume entity && Id.Equals(entity.Id));

        internal static void BuildEntity(EntityTypeBuilder<Volume> builder)
        {
            builder.HasOne(sn => sn.FileSystem).WithMany(d => d.Volumes).HasForeignKey(nameof(FileSystemId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.Property(nameof(Identifier)).HasConversion(VolumeIdentifier.Converter);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            LocalDbContext.GetBasicLocalDbEntityValidationResult(this, validationContext, OnValidate);

        private void OnValidate(EntityEntry<Volume> entityEntry, LocalDbContext dbContext, List<ValidationResult> validationResults)
        {
            if (!Enum.IsDefined(Type))
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DriveTypeInvalid, new string[] { nameof(Type) }));
            if (!Enum.IsDefined(Status))
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidVolumeStatus, new string[] { nameof(Status) }));
            VolumeIdentifier identifier = Identifier;
            if (identifier.IsEmpty())
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierRequired, new string[] { nameof(Identifier) }));
            else if (identifier.ToString().Length > DbConstants.DbColMaxLen_Identifier)
                validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_IdentifierLength, new string[] { nameof(Identifier) }));
            else
            {
                Guid id = Id;
                var entities = from v in dbContext.Volumes where id != v.Id && v.Identifier == identifier select v;
                if (entities.Any())
                    validationResults.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateVolumeIdentifier, new string[] { nameof(Identifier) }));
            }
        }
    }
}
