using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class TranslatedAbsoluteUrl : DerivedRelativeUrl<AbsoluteMatchedUrl>, IEquatable<TranslatedAbsoluteUrl>
    {
        [XmlAttribute]
        public bool IsWellFormed { get; set; }

        public bool Equals([AllowNull] TranslatedAbsoluteUrl other) => !(other is null) && (ReferenceEquals(this, other) || (IsWellFormed == other.IsWellFormed && BaseEquals(other)));

        public override bool Equals(object obj) => Equals(obj as TranslatedAbsoluteUrl);

        public override int GetHashCode() => HashCode.Combine(Path, Query, Fragment, LocalPath, Value, IsWellFormed);

        public override string GetXPath()
        {
            AbsoluteMatchedUrl owner = Owner;
            return (owner is null) ? nameof(AbsoluteMatchedUrl.Translated) : $"{Owner.GetXPath()}/{nameof(AbsoluteMatchedUrl.Translated)}";
        }
    }
}