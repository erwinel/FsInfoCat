using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    [Table(TABLE_NAME)]
    public class DbFile : LocalDbEntity, ILocalFile
    {
        #region Fields

        public const string TABLE_NAME = "Files";

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _name;
        private readonly IPropertyChangeTracker<FileCrawlOptions> _options;
        private readonly IPropertyChangeTracker<DateTime> _lastAccessed;
        private readonly IPropertyChangeTracker<DateTime?> _lastHashCalculation;
        private readonly IPropertyChangeTracker<string> _notes;
        private readonly IPropertyChangeTracker<bool> _deleted;
        private readonly IPropertyChangeTracker<DateTime> _creationTime;
        private readonly IPropertyChangeTracker<DateTime> _lastWriteTime;
        private readonly IPropertyChangeTracker<Guid> _parentId;
        private readonly IPropertyChangeTracker<Guid> _contentId;
        private readonly IPropertyChangeTracker<Guid?> _extendedPropertiesId;
        private readonly IPropertyChangeTracker<Subdirectory> _parent;
        private readonly IPropertyChangeTracker<ContentInfo> _content;
        private readonly IPropertyChangeTracker<Redundancy> _redundancy;
        private readonly IPropertyChangeTracker<ExtendedProperties> _extendedProperties;
        private HashSet<FileAccessError> _accessErrors = new();
        private HashSet<FileComparison> _comparisonSources = new();
        private HashSet<FileComparison> _comparisonTargets = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [StringLength(DbConstants.DbColMaxLen_FileName, ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_NameLength),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual string Name { get => _name.GetValue(); set => _name.SetValue(value); }

        [Required]
        public virtual FileCrawlOptions Options { get => _options.GetValue(); set => _options.SetValue(value); }

        [Required]
        public virtual DateTime LastAccessed { get => _lastAccessed.GetValue(); set => _lastAccessed.SetValue(value); }

        public virtual DateTime? LastHashCalculation { get => _lastHashCalculation.GetValue(); set => _lastHashCalculation.SetValue(value); }

        [Required(AllowEmptyStrings = true)]
        public virtual string Notes { get => _notes.GetValue(); set => _notes.SetValue(value); }

        [Required]
        public virtual bool Deleted { get => _deleted.GetValue(); set => _deleted.SetValue(value); }

        public DateTime CreationTime { get => _creationTime.GetValue(); set => _creationTime.SetValue(value); }

        public DateTime LastWriteTime { get => _lastWriteTime.GetValue(); set => _lastWriteTime.SetValue(value); }

        public virtual Guid ParentId
        {
            get => _parentId.GetValue();
            set
            {
                if (_parentId.SetValue(value))
                {
                    Subdirectory nav = _parent.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _parent.SetValue(null);
                }
            }
        }

        public virtual Guid ContentId
        {
            get => _contentId.GetValue();
            set
            {
                if (_contentId.SetValue(value))
                {
                    ContentInfo nav = _content.GetValue();
                    if (!(nav is null || nav.Id.Equals(value)))
                        _content.SetValue(null);
                }
            }
        }

        public virtual Guid? ExtendedPropertiesId
        {
            get => _extendedPropertiesId.GetValue();
            set
            {
                if (_extendedPropertiesId.SetValue(value))
                {
                    ExtendedProperties nav = _extendedProperties.GetValue();
                    if (value.HasValue)
                    {
                        if (!(nav is null || nav.Id.Equals(value.Value)))
                            _content.SetValue(null);
                    }
                    else if (!(nav is null))
                        _content.SetValue(null);
                }
            }
        }

        public virtual ContentInfo Content
        {
            get => _content.GetValue();
            set
            {
                if (_content.SetValue(value))
                {
                    if (value is null)
                        _contentId.SetValue(Guid.Empty);
                    else
                        _contentId.SetValue(value.Id);
                }
            }
        }

        public virtual Subdirectory Parent
        {
            get => _parent.GetValue();
            set
            {
                if (_parent.SetValue(value))
                {
                    if (value is null)
                        _parentId.SetValue(Guid.Empty);
                    else
                        _parentId.SetValue(value.Id);
                }
            }
        }

        public virtual Redundancy Redundancy { get => _redundancy.GetValue(); set => _redundancy.SetValue(value); }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ExtendedProperties), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual ExtendedProperties ExtendedProperties
        {
            get => _extendedProperties.GetValue();
            set
            {
                if (_extendedProperties.SetValue(value))
                {
                    if (value is null)
                        _extendedPropertiesId.SetValue(null);
                    else
                        _extendedPropertiesId.SetValue(value.Id);
                }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileAccessError> AccessErrors
        {
            get => _accessErrors;
            set => CheckHashSetChanged(_accessErrors, value, h => _accessErrors = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ComparisonSources), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> ComparisonSources
        {
            get => _comparisonSources;
            set => CheckHashSetChanged(_comparisonSources, value, h => _comparisonSources = h);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_ComparisonTargets), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual HashSet<FileComparison> ComparisonTargets
        {
            get => _comparisonTargets;
            set => CheckHashSetChanged(_comparisonTargets, value, h => _comparisonTargets = h);
        }

        #endregion

        #region Explicit Members

        ILocalContentInfo ILocalFile.Content { get => Content; set => Content = (ContentInfo)value; }

        IContentInfo IFile.Content { get => Content; set => Content = (ContentInfo)value; }

        ILocalSubdirectory ILocalFile.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ISubdirectory IFile.Parent { get => Parent; set => Parent = (Subdirectory)value; }

        ILocalRedundancy ILocalFile.Redundancy => Redundancy;

        IRedundancy IFile.Redundancy => Redundancy;

        IEnumerable<ILocalComparison> ILocalFile.ComparisonSources => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonSources => ComparisonSources.Cast<IComparison>();

        IEnumerable<ILocalComparison> ILocalFile.ComparisonTargets => ComparisonSources.Cast<ILocalComparison>();

        IEnumerable<IComparison> IFile.ComparisonTargets => ComparisonSources.Cast<IComparison>();

        ILocalExtendedProperties ILocalFile.ExtendedProperties { get => ExtendedProperties; set => ExtendedProperties = (ExtendedProperties)value; }

        IExtendedProperties IFile.ExtendedProperties { get => ExtendedProperties; set => ExtendedProperties = (ExtendedProperties)value; }

        IEnumerable<IAccessError<ILocalFile>> ILocalFile.AccessErrors => AccessErrors.Cast<IAccessError<ILocalFile>>();

        IEnumerable<IAccessError<IFile>> IFile.AccessErrors => AccessErrors.Cast<IAccessError<IFile>>();

        #endregion

        public DbFile()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _name = AddChangeTracker(nameof(Name), "", NonNullStringCoersion.Default);
            _options = AddChangeTracker(nameof(FileCrawlOptions), FileCrawlOptions.None);
            _lastAccessed = AddChangeTracker(nameof(LastAccessed), CreatedOn);
            _lastHashCalculation = AddChangeTracker<DateTime?>(nameof(LastHashCalculation), null);
            _notes = AddChangeTracker(nameof(Notes), "", NonNullStringCoersion.Default);
            _deleted = AddChangeTracker(nameof(Deleted), false);
            _creationTime = AddChangeTracker(nameof(CreationTime), CreatedOn);
            _lastWriteTime = AddChangeTracker(nameof(LastWriteTime), CreatedOn);
            _parentId = AddChangeTracker(nameof(ParentId), Guid.Empty);
            _contentId = AddChangeTracker(nameof(ContentId), Guid.Empty);
            _extendedPropertiesId = AddChangeTracker<Guid?>(nameof(ExtendedPropertiesId), null);
            _parent = AddChangeTracker<Subdirectory>(nameof(Parent), null);
            _content = AddChangeTracker<ContentInfo>(nameof(Content), null);
            _redundancy = AddChangeTracker<Redundancy>(nameof(Redundancy), null);
            _extendedProperties = AddChangeTracker<ExtendedProperties>(nameof(ExtendedProperties), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<DbFile> builder)
        {
            builder.HasOne(sn => sn.Parent).WithMany(d => d.Files).HasForeignKey(nameof(ParentId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.Content).WithMany(d => d.Files).HasForeignKey(nameof(ContentId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(sn => sn.ExtendedProperties).WithMany(d => d.Files).HasForeignKey(nameof(ExtendedPropertiesId)).OnDelete(DeleteBehavior.Restrict);
        }

        private void ValidateLastHashCalculation(List<ValidationResult> results)
        {
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                return;
            DateTime? dateTime = LastHashCalculation;
            if (dateTime.HasValue)
            {
                if (dateTime.Value.CompareTo(CreatedOn) < 0)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastHashCalculationBeforeCreatedOn, new string[] { nameof(LastHashCalculation) }));
                else if (dateTime.Value.CompareTo(ModifiedOn) > 0)
                    results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastHashCalculationAfterModifiedOn, new string[] { nameof(LastHashCalculation) }));
            }
            else if (!(Content is null || Content.Hash is null))
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_LastHashCalculationRequired, new string[] { nameof(LastHashCalculation) }));
        }

        private void ValidateName(ValidationContext validationContext, List<ValidationResult> results)
        {
            string name = Name;
            EntityEntry entry;
            LocalDbContext dbContext;
            if (string.IsNullOrEmpty(name) || (entry = validationContext.GetService<EntityEntry>()) is null ||
                (dbContext = validationContext.GetService<LocalDbContext>()) is null)
                return;
            Guid parentId = ParentId;
            if (entry.State == EntityState.Added)
            {
                if (!dbContext.Files.Any(sn => sn.ParentId == parentId && sn.Name == name))
                    return;
            }
            else
            {
                Guid id = Id;
                // TODO: Need to test whether this fails to skip an item where the id matches
                if (!dbContext.Files.Any(sn => sn.ParentId == parentId && sn.Name == name && id != sn.Id))
                    return;
            }
            results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_DuplicateName, new string[] { nameof(Name) }));
        }

        // TODO: Change to async with LocalDbContext
        internal XElement Export(bool includeParentId = false)
        {
            XElement result = new(nameof(TABLE_NAME),
                new XAttribute(nameof(Id), XmlConvert.ToString(Id)),
                new XAttribute(nameof(Name), Name),
                new XAttribute(nameof(ContentId), XmlConvert.ToString(ContentId))
            );
            if (includeParentId)
            {
                Guid parentId = ParentId;
                if (!parentId.Equals(Guid.Empty))
                    result.SetAttributeValue(nameof(ParentId), XmlConvert.ToString(parentId));
            }
            FileCrawlOptions options = Options;
            if (options != FileCrawlOptions.None)
                result.SetAttributeValue(nameof(Options), Enum.GetName(typeof(FileCrawlOptions), Options));
            if (Deleted)
                result.SetAttributeValue(nameof(Deleted), Deleted);
            DateTime? lastHashCalculation = LastHashCalculation;
            if (lastHashCalculation.HasValue)
                result.SetAttributeValue(nameof(LastHashCalculation), XmlConvert.ToString(lastHashCalculation.Value, XmlDateTimeSerializationMode.RoundtripKind));
            result.SetAttributeValue(nameof(LastAccessed), XmlConvert.ToString(LastAccessed, XmlDateTimeSerializationMode.RoundtripKind));
            AddExportAttributes(result);
            if (Notes.Length > 0)
                result.Add(new XElement(nameof(Notes), new XCData(Notes)));
            foreach (FileAccessError accessError in AccessErrors)
                result.Add(accessError.Export());
            return result;
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                ValidateLastHashCalculation(results);
                ValidateName(validationContext, results);
            }
            else
                switch (validationContext.MemberName)
                {
                    case nameof(CreatedOn):
                    case nameof(ModifiedOn):
                    case nameof(LastHashCalculation):
                        ValidateLastHashCalculation(results);
                        break;
                    case nameof(Parent):
                    case nameof(Name):
                        ValidateName(validationContext, results);
                        break;
                }

        }

        internal static async Task ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid parentId, XElement fileElement)
        {
            string n = nameof(Id);
            Guid fileId = fileElement.GetAttributeGuid(n).Value;
            await new InsertQueryBuilder(nameof(LocalDbContext.Files), fileElement, n).AppendGuid(nameof(ParentId), parentId).AppendString(nameof(Name))
                .AppendGuid(nameof(ContentId)).AppendGuid(nameof(ExtendedPropertiesId)).AppendEnum<FileCrawlOptions>(nameof(Options))
                .AppendDateTime(nameof(LastHashCalculation)).AppendDateTime(nameof(LastHashCalculation)).AppendElementString(nameof(Notes))
                .AppendBoolean(nameof(Deleted)).AppendDateTime(nameof(CreationTime)).AppendDateTime(nameof(LastWriteTime))
                .AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            foreach (XElement accessErrorElement in fileElement.Elements(ElementName_AccessError))
                await FileAccessError.ImportAsync(dbContext, logger, fileId, accessErrorElement);
            foreach (XElement comparisonElement in fileElement.Elements(FileComparison.ELEMENT_NAME))
                await FileComparison.ImportAsync(dbContext, logger, fileId, comparisonElement);
        }
    }
}
