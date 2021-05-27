using System.Collections.Generic;

namespace FsInfoCat
{
    public class TrimmedNonNullStringCoersion : NonNullStringCoersion
    {
        public static new readonly TrimmedNonNullStringCoersion Default = new TrimmedNonNullStringCoersion(null);

        public TrimmedNonNullStringCoersion(IEqualityComparer<string> comparer) : base(comparer) { }

        public override string Cast(object obj) => base.Cast(obj).Trim();

        public override string Coerce(object obj) => base.Coerce(obj).Trim();

        public override bool TryCast(object obj, out string result)
        {
            if (base.TryCast(obj, out result))
            {
                result = result.Trim();
                return true;
            }
            return false;
        }

        public override bool TryCoerce(object obj, out string result)
        {
            if (base.TryCast(obj, out result))
            {
                result = result.Trim();
                return true;
            }
            return false;
        }
    }
}
