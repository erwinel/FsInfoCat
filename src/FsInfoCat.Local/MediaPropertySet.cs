using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class MediaPropertySet : LocalDbEntity, ILocalMediaPropertySet
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<string> _contentDistributor;
        private readonly IPropertyChangeTracker<string> _creatorApplication;
        private readonly IPropertyChangeTracker<string> _creatorApplicationVersion;
        private readonly IPropertyChangeTracker<string> _dateReleased;
        private readonly IPropertyChangeTracker<ulong?> _duration;
        private readonly IPropertyChangeTracker<string> _dvdID;
        private readonly IPropertyChangeTracker<uint?> _frameCount;
        private readonly IPropertyChangeTracker<MultiStringValue> _producer;
        private readonly IPropertyChangeTracker<string> _protectionType;
        private readonly IPropertyChangeTracker<string> _providerRating;
        private readonly IPropertyChangeTracker<string> _providerStyle;
        private readonly IPropertyChangeTracker<string> _publisher;
        private readonly IPropertyChangeTracker<string> _subtitle;
        private readonly IPropertyChangeTracker<MultiStringValue> _writer;
        private readonly IPropertyChangeTracker<uint?> _year;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public string ContentDistributor { get => _contentDistributor.GetValue(); set => _contentDistributor.SetValue(value); }
        public string CreatorApplication { get => _creatorApplication.GetValue(); set => _creatorApplication.SetValue(value); }
        public string CreatorApplicationVersion { get => _creatorApplicationVersion.GetValue(); set => _creatorApplicationVersion.SetValue(value); }
        public string DateReleased { get => _dateReleased.GetValue(); set => _dateReleased.SetValue(value); }
        public ulong? Duration { get => _duration.GetValue(); set => _duration.SetValue(value); }
        public string DVDID { get => _dvdID.GetValue(); set => _dvdID.SetValue(value); }
        public uint? FrameCount { get => _frameCount.GetValue(); set => _frameCount.SetValue(value); }
        public MultiStringValue Producer { get => _producer.GetValue(); set => _producer.SetValue(value); }
        public string ProtectionType { get => _protectionType.GetValue(); set => _protectionType.SetValue(value); }
        public string ProviderRating { get => _providerRating.GetValue(); set => _providerRating.SetValue(value); }
        public string ProviderStyle { get => _providerStyle.GetValue(); set => _providerStyle.SetValue(value); }
        public string Publisher { get => _publisher.GetValue(); set => _publisher.SetValue(value); }
        public string Subtitle { get => _subtitle.GetValue(); set => _subtitle.SetValue(value); }
        public MultiStringValue Writer { get => _writer.GetValue(); set => _writer.SetValue(value); }
        public uint? Year { get => _year.GetValue(); set => _year.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public MediaPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _contentDistributor = AddChangeTracker(nameof(ContentDistributor), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _creatorApplication = AddChangeTracker(nameof(CreatorApplication), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _creatorApplicationVersion = AddChangeTracker(nameof(CreatorApplicationVersion), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _dateReleased = AddChangeTracker(nameof(DateReleased), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _duration = AddChangeTracker<ulong?>(nameof(Duration), null);
            _dvdID = AddChangeTracker(nameof(DVDID), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _frameCount = AddChangeTracker<uint?>(nameof(FrameCount), null);
            _producer = AddChangeTracker<MultiStringValue>(nameof(Producer), null);
            _protectionType = AddChangeTracker(nameof(ProtectionType), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _providerRating = AddChangeTracker(nameof(ProviderRating), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _providerStyle = AddChangeTracker(nameof(ProviderStyle), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _publisher = AddChangeTracker(nameof(Publisher), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _subtitle = AddChangeTracker(nameof(Subtitle), null, FilePropertiesComparer.NormalizedStringValueCoersion);
            _writer = AddChangeTracker<MultiStringValue>(nameof(Writer), null);
            _year = AddChangeTracker<uint?>(nameof(Year), null);
        }

        internal static void BuildEntity(EntityTypeBuilder<MediaPropertySet> obj)
        {
            obj.Property(nameof(Producer)).HasConversion(MultiStringValue.Converter);
            obj.Property(nameof(Writer)).HasConversion(MultiStringValue.Converter);
        }

        internal static async Task RefreshAsync(EntityEntry<DbFile> entry, IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            MediaPropertySet oldMediaPropertySet = entry.Entity.MediaPropertySetId.HasValue ? await entry.GetRelatedReferenceAsync(f => f.MediaProperties, cancellationToken) : null;
            IMediaProperties currentMediaProperties = await fileDetailProvider.GetMediaPropertiesAsync(cancellationToken);
            // TODO: Implement RefreshAsync(EntityEntry<DbFile>, IFileDetailProvider, CancellationToken)
            throw new NotImplementedException();
        }
    }
}
