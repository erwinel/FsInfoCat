using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class ContentInfo : LocalDbEntity, ILocalContentInfo
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<long> _length;
        private readonly IPropertyChangeTracker<MD5Hash?> _hash;
        private HashSet<DbFile> _files = new();
        private HashSet<RedundantSet> _redundantSets = new();

        #endregion

        #region Properties

        [Key]
        public virtual Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        [Required]
        public virtual long Length { get => _length.GetValue(); set => _length.SetValue(value); }

        public virtual MD5Hash? Hash { get => _hash.GetValue(); set => _hash.SetValue(value); }

        public virtual HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        public virtual HashSet<RedundantSet> RedundantSets
        {
            get => _redundantSets;
            set => CheckHashSetChanged(_redundantSets, value, h => _redundantSets = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalContentInfo.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IContentInfo.Files => Files.Cast<IFile>();

        IEnumerable<ILocalRedundantSet> ILocalContentInfo.RedundantSets => RedundantSets.Cast<ILocalRedundantSet>();

        IEnumerable<IRedundantSet> IContentInfo.RedundantSets => RedundantSets.Cast<IRedundantSet>();

        #endregion

        public ContentInfo()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _length = AddChangeTracker(nameof(Length), 0L);
            _hash = AddChangeTracker<MD5Hash?>(nameof(Hash), null);
        }

        protected override void OnPropertyChanging(PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(Id) && _id.IsChanged)
                throw new InvalidOperationException();
            base.OnPropertyChanging(args);
        }

        protected override void OnValidate(ValidationContext validationContext, List<ValidationResult> results)
        {
            base.OnValidate(validationContext, results);
            if (Length < 0L)
                results.Add(new ValidationResult(FsInfoCat.Properties.Resources.ErrorMessage_InvalidFileLength, new string[] { nameof(Length) }));
        }

        internal static void BuildEntity(EntityTypeBuilder<ContentInfo> obj)
        {
            obj.Property(nameof(Hash)).HasConversion(MD5Hash.Converter);
        }

        internal static async Task<(Guid redundantSetId, XElement[] redundancies)[]> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, XElement contentInfoElement)
        {
            string n = nameof(Id);
            Guid contentInfoId = contentInfoElement.GetAttributeGuid(n).Value;
            logger.LogInformation($"Inserting {nameof(ContentInfo)} with Id {{Id}}", contentInfoId);
            await new InsertQueryBuilder(nameof(LocalDbContext.ContentInfos), contentInfoElement, n).AppendInt64(nameof(Length)).AppendMd5Hash(nameof(Hash))
                .AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
            return contentInfoElement.Elements(nameof(RedundantSet)).Select(e => RedundantSet.ImportAsync(dbContext, logger, contentInfoId, e).Result).ToArray();
        }
    }
}
