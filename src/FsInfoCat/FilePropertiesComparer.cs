using FsInfoCat.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    /// <summary>
    /// Common comparer for comparing extended file properties
    /// </summary>
    /// <seealso cref="ISummaryProperties" />
    /// <seealso cref="IDocumentProperties" />
    /// <seealso cref="IAudioProperties" />
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="IGPSProperties" />
    /// <seealso cref="IImageProperties" />
    /// <seealso cref="IMediaProperties" />
    /// <seealso cref="IMusicProperties" />
    /// <seealso cref="IPhotoProperties" />
    /// <seealso cref="IRecordedTVProperties" />
    /// <seealso cref="IVideoProperties" />
    public class FilePropertiesComparer : IEqualityComparer<ISummaryProperties>, IEqualityComparer<IDocumentProperties>, IEqualityComparer<IAudioProperties>,
        IEqualityComparer<IDRMProperties>, IEqualityComparer<IGPSProperties>, IEqualityComparer<IImageProperties>, IEqualityComparer<IMediaProperties>,
        IEqualityComparer<IMusicProperties>, IEqualityComparer<IPhotoProperties>, IEqualityComparer<IRecordedTVProperties>, IEqualityComparer<IVideoProperties>
    {
        public static readonly FilePropertiesComparer Default = new();

        public static readonly NullIfWhiteSpaceOrTrimmedStringCoersion StringValueCoersion = new(StringComparer.InvariantCultureIgnoreCase);

        public static readonly NullIfWhiteSpaceOrNormalizedStringCoersion NormalizedStringValueCoersion = new(StringComparer.InvariantCultureIgnoreCase);

        public static bool Equals(ISummaryProperties x, ISummaryProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.Rating == y.Rating && x.Sensitivity == y.Sensitivity && x.SimpleRating == y.SimpleRating &&
                NormalizedStringValueCoersion.Equals(x.ApplicationName, y.ApplicationName) && StringValueCoersion.Equals(x.Comment, y.Comment) &&
                NormalizedStringValueCoersion.Equals(x.Title, y.Title) && NormalizedStringValueCoersion.Equals(x.Subject, y.Subject) &&
                NormalizedStringValueCoersion.Equals(x.Company, y.Company) && NormalizedStringValueCoersion.Equals(x.ContentType, y.ContentType) &&
                NormalizedStringValueCoersion.Equals(x.Copyright, y.Copyright) && NormalizedStringValueCoersion.Equals(x.ParentalRating, y.ParentalRating) &&
                NormalizedStringValueCoersion.Equals(x.ItemType, y.ItemType) && NormalizedStringValueCoersion.Equals(x.MIMEType, y.MIMEType) &&
                StringValueCoersion.Equals(x.ItemTypeText, y.ItemTypeText) &&
                NormalizedStringValueCoersion.Equals(x.ParentalRatingsOrganization, y.ParentalRatingsOrganization) &&
                StringValueCoersion.Equals(x.ParentalRatingReason, y.ParentalRatingReason) &&
                StringValueCoersion.Equals(x.SensitivityText, y.SensitivityText) && StringValueCoersion.Equals(x.Trademarks, y.Trademarks) &&
                NormalizedStringValueCoersion.Equals(x.ProductName, y.ProductName) && MultiStringValue.AreEqual(x.Author, y.Author) &&
                MultiStringValue.AreEqual(x.Keywords, y.Keywords) && MultiStringValue.AreEqual(x.ItemAuthors, y.ItemAuthors) &&
                MultiStringValue.AreEqual(x.Kind, y.Kind)));
        }

        public static bool Equals(IDocumentProperties x, IDocumentProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DateCreated == y.DateCreated && x.Security == y.Security &&
                NormalizedStringValueCoersion.Equals(x.ClientID, y.ClientID) && NormalizedStringValueCoersion.Equals(x.LastAuthor, y.LastAuthor) &&
                NormalizedStringValueCoersion.Equals(x.RevisionNumber, y.RevisionNumber) && StringValueCoersion.Equals(x.Division, y.Division) &&
                NormalizedStringValueCoersion.Equals(x.DocumentID, y.DocumentID) && NormalizedStringValueCoersion.Equals(x.Manager, y.Manager) &&
                NormalizedStringValueCoersion.Equals(x.PresentationFormat, y.PresentationFormat) && NormalizedStringValueCoersion.Equals(x.Version, y.Version) &&
                MultiStringValue.AreEqual(x.Contributor, y.Contributor)));
        }

        public static bool Equals(IAudioProperties x, IAudioProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.EncodingBitrate == y.EncodingBitrate && x.IsVariableBitrate == y.IsVariableBitrate &&
                x.SampleRate == y.SampleRate && x.SampleSize == y.SampleSize && x.StreamNumber == y.StreamNumber &&
                NormalizedStringValueCoersion.Equals(x.Compression, y.Compression) &&
                NormalizedStringValueCoersion.Equals(x.Format, y.Format) && NormalizedStringValueCoersion.Equals(x.StreamName, y.StreamName)));
        }

        public static bool Equals(IDRMProperties x, IDRMProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DatePlayExpires == y.DatePlayExpires && x.DatePlayStarts == y.DatePlayStarts &&
                x.IsProtected == y.IsProtected && x.PlayCount == y.PlayCount && StringValueCoersion.Equals(x.Description, y.Description)));
        }

        public static bool Equals(IGPSProperties x, IGPSProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.LatitudeDegrees == y.LatitudeDegrees && x.LatitudeMinutes == y.LatitudeMinutes &&
                x.LatitudeSeconds == y.LatitudeSeconds && x.LongitudeDegrees == y.LongitudeDegrees && x.LongitudeMinutes == y.LongitudeMinutes &&
                x.LongitudeSeconds == y.LongitudeSeconds && NormalizedStringValueCoersion.Equals(x.AreaInformation, y.AreaInformation) &&
                NormalizedStringValueCoersion.Equals(x.LatitudeRef, y.LatitudeRef) && NormalizedStringValueCoersion.Equals(x.LongitudeRef, y.LongitudeRef) &&
                NormalizedStringValueCoersion.Equals(x.MeasureMode, y.MeasureMode) && NormalizedStringValueCoersion.Equals(x.ProcessingMethod, y.ProcessingMethod) &&
                x.VersionID.EmptyIfNull().SequenceEqual(y.VersionID.EmptyIfNull())));
        }

        public static bool Equals(IImageProperties x, IImageProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.BitDepth == y.BitDepth && x.ColorSpace == y.ColorSpace &&
                x.CompressedBitsPerPixel == y.CompressedBitsPerPixel && x.Compression == y.Compression && x.HorizontalResolution == y.HorizontalResolution &&
                x.HorizontalSize == y.HorizontalSize && x.ResolutionUnit == y.ResolutionUnit && x.VerticalResolution == y.VerticalResolution &&
                x.VerticalSize == y.VerticalSize && NormalizedStringValueCoersion.Equals(x.CompressionText, y.CompressionText) &&
                NormalizedStringValueCoersion.Equals(x.ImageID, y.ImageID)));
        }

        public static bool Equals(IMediaProperties x, IMediaProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.Duration == y.Duration && x.FrameCount == y.FrameCount && x.Year == y.Year &&
                NormalizedStringValueCoersion.Equals(x.ContentDistributor, y.ContentDistributor) &&
                NormalizedStringValueCoersion.Equals(x.CreatorApplication, y.CreatorApplication) &&
                NormalizedStringValueCoersion.Equals(x.CreatorApplicationVersion, y.CreatorApplicationVersion) &&
                NormalizedStringValueCoersion.Equals(x.DateReleased, y.DateReleased) && NormalizedStringValueCoersion.Equals(x.DVDID, y.DVDID) &&
                NormalizedStringValueCoersion.Equals(x.ProtectionType, y.ProtectionType) &&
                NormalizedStringValueCoersion.Equals(x.ProviderRating, y.ProviderRating) && NormalizedStringValueCoersion.Equals(x.ProviderStyle, y.ProviderStyle) &&
                NormalizedStringValueCoersion.Equals(x.Publisher, y.Publisher) && NormalizedStringValueCoersion.Equals(x.Subtitle, y.Subtitle) &&
                MultiStringValue.AreEqual(x.Producer, y.Producer) && MultiStringValue.AreEqual(x.Writer, y.Writer)));
        }

        public static bool Equals(IMusicProperties x, IMusicProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.ChannelCount == y.ChannelCount && x.TrackNumber == y.TrackNumber &&
                NormalizedStringValueCoersion.Equals(x.AlbumArtist, y.AlbumArtist) && NormalizedStringValueCoersion.Equals(x.AlbumTitle, y.AlbumTitle) &&
                NormalizedStringValueCoersion.Equals(x.DisplayArtist, y.DisplayArtist) && NormalizedStringValueCoersion.Equals(x.PartOfSet, y.PartOfSet) &&
                NormalizedStringValueCoersion.Equals(x.Period, y.Period) && MultiStringValue.AreEqual(x.Artist, y.Artist) &&
                MultiStringValue.AreEqual(x.Composer, y.Composer) && MultiStringValue.AreEqual(x.Conductor, y.Conductor) &&
                MultiStringValue.AreEqual(x.Genre, y.Genre)));
        }

        public static bool Equals(IPhotoProperties x, IPhotoProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.DateTaken == y.DateTaken && x.Orientation == y.Orientation &&
                NormalizedStringValueCoersion.Equals(x.CameraManufacturer, y.CameraManufacturer) &&
                NormalizedStringValueCoersion.Equals(x.CameraModel, y.CameraModel) &&
                NormalizedStringValueCoersion.Equals(x.EXIFVersion, y.EXIFVersion) && StringValueCoersion.Equals(x.OrientationText, y.OrientationText) &&
                MultiStringValue.AreEqual(x.Event, y.Event) && MultiStringValue.AreEqual(x.PeopleNames, y.PeopleNames)));
        }

        public static bool Equals(IRecordedTVProperties x, IRecordedTVProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.ChannelNumber == y.ChannelNumber && x.IsDTVContent == y.IsDTVContent &&
                x.IsHDContent == y.IsHDContent && x.OriginalBroadcastDate == y.OriginalBroadcastDate &&
                NormalizedStringValueCoersion.Equals(x.EpisodeName, y.EpisodeName) &&
                NormalizedStringValueCoersion.Equals(x.NetworkAffiliation, y.NetworkAffiliation) &&
                StringValueCoersion.Equals(x.ProgramDescription, y.ProgramDescription) &&
                NormalizedStringValueCoersion.Equals(x.StationCallSign, y.StationCallSign) && NormalizedStringValueCoersion.Equals(x.StationName, y.StationName)));
        }

        public static bool Equals(IVideoProperties x, IVideoProperties y)
        {
            if (x.IsNullOrAllPropertiesEmpty())
                return y.IsNullOrAllPropertiesEmpty();
            return y is not null && (ReferenceEquals(x, y) || (x.EncodingBitrate == y.EncodingBitrate && x.FrameHeight == y.FrameHeight && x.FrameRate == y.FrameRate &&
                x.FrameWidth == y.FrameWidth && x.HorizontalAspectRatio == y.HorizontalAspectRatio && x.StreamNumber == y.StreamNumber &&
                x.VerticalAspectRatio == y.VerticalAspectRatio && NormalizedStringValueCoersion.Equals(x.Compression, y.Compression) &&
                NormalizedStringValueCoersion.Equals(x.StreamName, y.StreamName) && MultiStringValue.AreEqual(x.Director, y.Director)));
        }

        bool IEqualityComparer<ISummaryProperties>.Equals(ISummaryProperties x, ISummaryProperties y) => Equals(x, y);
        bool IEqualityComparer<IDocumentProperties>.Equals(IDocumentProperties x, IDocumentProperties y) => Equals(x, y);
        bool IEqualityComparer<IAudioProperties>.Equals(IAudioProperties x, IAudioProperties y) => Equals(x, y);
        bool IEqualityComparer<IDRMProperties>.Equals(IDRMProperties x, IDRMProperties y) => Equals(x, y);
        bool IEqualityComparer<IGPSProperties>.Equals(IGPSProperties x, IGPSProperties y) => Equals(x, y);
        bool IEqualityComparer<IImageProperties>.Equals(IImageProperties x, IImageProperties y) => Equals(x, y);
        bool IEqualityComparer<IMediaProperties>.Equals(IMediaProperties x, IMediaProperties y) => Equals(x, y);
        bool IEqualityComparer<IMusicProperties>.Equals(IMusicProperties x, IMusicProperties y) => Equals(x, y);
        bool IEqualityComparer<IPhotoProperties>.Equals(IPhotoProperties x, IPhotoProperties y) => Equals(x, y);
        bool IEqualityComparer<IRecordedTVProperties>.Equals(IRecordedTVProperties x, IRecordedTVProperties y) => Equals(x, y);
        bool IEqualityComparer<IVideoProperties>.Equals(IVideoProperties x, IVideoProperties y) => Equals(x, y);
        
        public int GetHashCode([DisallowNull] ISummaryProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            return (new int?[] { obj.Rating?.GetHashCode(), obj.Rating?.GetHashCode(), obj.Rating?.GetHashCode(), obj.Author?.GetHashCode(), obj.Keywords?.GetHashCode(), obj.ItemAuthors?.GetHashCode(), obj.Kind?.GetHashCode() })
                .Select(n => n ?? 0)
                .Concat((new string[] { obj.Title, obj.Subject, obj.Company, obj.ContentType, obj.Copyright, obj.ParentalRating, obj.ItemType, obj.MIMEType, obj.ParentalRatingsOrganization, obj.ProductName })
                    .Select(s => NormalizedStringValueCoersion.GetHashCode(s)))
                .Concat((new string[] { obj.Comment, obj.ItemTypeText, obj.ParentalRatingReason, obj.SensitivityText, obj.Copyright }).Select(s => NormalizedStringValueCoersion.GetHashCode(s))).ToAggregateHashCode();
        }

        public int GetHashCode([DisallowNull] IDocumentProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IDocumentProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IAudioProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IAudioProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IDRMProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IDRMProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IGPSProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IGPSProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IImageProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IImageProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IMediaProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IMediaProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IMusicProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IMusicProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IPhotoProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IPhotoProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IRecordedTVProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IRecordedTVProperties);
            throw new NotImplementedException();
        }

        public int GetHashCode([DisallowNull] IVideoProperties obj)
        {
            if (obj.IsNullOrAllPropertiesEmpty())
                return 0;
            // TODO: Implement GetHashCode(IVideoProperties);
            throw new NotImplementedException();
        }
    }
}
