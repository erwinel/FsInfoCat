using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local
{
    public class SymbolicName : NotifyPropertyChanged, ILocalSymbolicName
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<Guid> _fileSystemId;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _isInactive;
        private readonly IPropertyChangeTracker<Guid?> _upstreamId;
        private readonly IPropertyChangeTracker<DateTime?> _lastSynchronizedOn;
        private readonly IPropertyChangeTracker<DateTime> _createdOn;
        private readonly IPropertyChangeTracker<DateTime> _modifiedOn;
        private readonly IPropertyChangeTracker<FileSystem> _fileSystem;

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        [MaxLength(DbConstants.DbColMaxLen_SimpleName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool IsInactive { get => _isInactive.GetValue(); set => _isInactive.SetValue(value); }

        public virtual int Priority { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(Properties.Resources))]
        public virtual Guid? UpstreamId { get => _upstreamId.GetValue(); set => _upstreamId.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime? LastSynchronizedOn { get => _lastSynchronizedOn.GetValue(); set => _lastSynchronizedOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime CreatedOn { get => _createdOn.GetValue(); set => _createdOn.SetValue(value); }

        [Required]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(Properties.Resources))]
        public virtual DateTime ModifiedOn { get => _modifiedOn.GetValue(); set => _modifiedOn.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(Properties.Resources))]
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

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        IFileSystem ISymbolicName.FileSystem { get => FileSystem; set => FileSystem = (FileSystem)value; }

        #endregion

        public SymbolicName()
        {
            _id = CreateChangeTracker(nameof(Id), Guid.Empty);
            _name = CreateChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _notes = CreateChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _isInactive = CreateChangeTracker(nameof(IsInactive), false);
            _upstreamId = CreateChangeTracker<Guid?>(nameof(UpstreamId), null);
            _lastSynchronizedOn = CreateChangeTracker<DateTime?>(nameof(LastSynchronizedOn), null);
            _modifiedOn = CreateChangeTracker(nameof(ModifiedOn), (_createdOn = CreateChangeTracker(nameof(CreatedOn), DateTime.Now)).GetValue());
            _fileSystemId = CreateChangeTracker(nameof(FileSystemId), Guid.Empty);
            _fileSystem = CreateChangeTracker<FileSystem>(nameof(FileSystem), null);
        }

        public bool IsNew() => !_id.IsSet;

        public bool IsSameDbRow(IDbEntity other) => IsNew() ? ReferenceEquals(this, other) : (other is ILocalSymbolicName entity && Id.Equals(entity.Id));

        internal static void BuildEntity(EntityTypeBuilder<SymbolicName> builder)
        {
            builder.HasOne(sn => sn.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(nameof(FileSystemId)).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            if (_createdOn.GetValue().CompareTo(_modifiedOn.GetValue()) > 0)
                result.Add(new ValidationResult($"{nameof(CreatedOn)} cannot be later than {nameof(ModifiedOn)}.", new string[] { nameof(CreatedOn) }));
            return result;
        }
    }
}
