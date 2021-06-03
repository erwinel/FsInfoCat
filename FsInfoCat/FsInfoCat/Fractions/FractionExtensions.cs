using System;
using System.Text.RegularExpressions;

namespace FsInfoCat.Fractions
{
    public static class FractionExtensions
    {
        #region Generic Methods

        private static T GetGCD<T>(IValueHelper<T> valueHelper, T d1, params T[] denominators)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (denominators == null || denominators.Length == 0)
                return d1;
            T gcd = valueHelper.Abs(d1);
            foreach (T d in denominators)
            {
                T b;
                if (d.CompareTo(gcd) > 0)
                {
                    b = gcd;
                    gcd = valueHelper.Abs(d);
                }
                else
                    b = valueHelper.Abs(d);
                while (b.CompareTo(valueHelper.Zero) > 0)
                {
                    T rem = valueHelper.Modulus(gcd, b);
                    gcd = b;
                    b = rem;
                }
            }

            return gcd;
        }

        private static T GetLCM<T>(IValueHelper<T> valueHelper, T d1, T d2, out T secondMultiplier)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            T zero = default(T);
            if (d1.CompareTo(zero) < 0)
                return GetLCM<T>(valueHelper, valueHelper.Abs(d1), d2, out secondMultiplier);

            if (d2.CompareTo(zero) < 0)
                return GetLCM<T>(valueHelper, d1, valueHelper.Abs(d2), out secondMultiplier);

            if (d1.Equals(d2))
            {
                secondMultiplier = valueHelper.PositiveOne;
                return secondMultiplier;
            }

            if (d1.CompareTo(d2) < 0)
            {
                secondMultiplier = GetLCM<T>(valueHelper, d2, d1, out d1);
                return d1;
            }

            secondMultiplier = d1;

            while (!valueHelper.Modulus(secondMultiplier, d2).Equals(zero))
                secondMultiplier = valueHelper.Add(secondMultiplier, d1);

            return GetSimplifiedRational<T>(valueHelper, valueHelper.Divide(secondMultiplier, d1), secondMultiplier, out secondMultiplier);
        }

        private static T GetSimplifiedRational<T>(IValueHelper<T> valueHelper, T n, T d, out T denominator)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (d.Equals(valueHelper.Zero))
                throw new DivideByZeroException();

            if (n.Equals(valueHelper.Zero))
            {
                denominator = valueHelper.PositiveOne;
                return valueHelper.Zero;
            }

            if (n.Equals(d))
            {
                denominator = valueHelper.PositiveOne;
                return valueHelper.PositiveOne;
            }

            if (d.CompareTo(valueHelper.Zero) < 0)
            {
                d = valueHelper.Multiply(d, valueHelper.NegativeOne);
                n = valueHelper.Multiply(n, valueHelper.NegativeOne);
            }
            T gcd = GetGCD<T>(valueHelper, n, d);
            denominator = valueHelper.Divide(d, gcd);
            return valueHelper.Divide(n, gcd);
        }

        private static T GetNormalizedRational<T>(IValueHelper<T> valueHelper, T w, T n, T d, out T numerator, out T denominator)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            n = GetSimplifiedRational<T>(valueHelper, n, d, out denominator);

            if (n.Equals(valueHelper.Zero))
            {
                numerator = n;
                return w;
            }

            if (denominator.Equals(valueHelper.PositiveOne))
            {
                numerator = valueHelper.Zero;
                return valueHelper.Add(w, n);
            }

            if (n.CompareTo(denominator) > 0)
            {
                numerator = valueHelper.Modulus(n, denominator);
                if (w.CompareTo(valueHelper.Zero) < 0)
                    w = valueHelper.Subtract(w, valueHelper.Divide(valueHelper.Subtract(n, numerator), denominator));
                else
                    w = valueHelper.Add(w, valueHelper.Divide(valueHelper.Subtract(n, numerator), denominator));
                numerator = GetSimplifiedRational<T>(valueHelper, numerator, denominator, out denominator);
            }
            else
                numerator = n;

            if (w.Equals(valueHelper.Zero))
                return w;

            if (numerator.CompareTo(valueHelper.Zero) < 0)
            {
                if (w.CompareTo(valueHelper.Zero) < 0)
                    w = valueHelper.Add(w, valueHelper.PositiveOne);
                else
                    w = valueHelper.Subtract(w, valueHelper.PositiveOne);
                numerator = valueHelper.Add(numerator, denominator);
            }

            return w;
        }

        private static T GetInvertedRational<T>(IValueHelper<T> valueHelper, T w, T n, T d, out T numerator, out T denominator)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            w = GetNormalizedRational<T>(valueHelper, w, n, d, out numerator, out denominator);

            if (n.Equals(valueHelper.Zero))
            {
                if (w.Equals(valueHelper.Zero))
                {
                    numerator = n;
                    return w;
                }

                return GetNormalizedRational<T>(valueHelper, valueHelper.Zero, valueHelper.PositiveOne, w, out numerator, out denominator);
            }

            if (w.Equals(valueHelper.Zero))
                return GetNormalizedRational<T>(valueHelper, valueHelper.Zero, d, n, out numerator, out denominator);

            return GetNormalizedRational<T>(valueHelper, valueHelper.Zero, d, valueHelper.Add(n, valueHelper.Multiply(d, w)), out numerator, out denominator);
        }

        private static void ToCommonDenominator<T>(IValueHelper<T> valueHelper, ref T n1, ref T d1, ref T n2, ref T d2)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (d1.Equals(valueHelper.Zero) || d2.Equals(valueHelper.Zero))
                throw new DivideByZeroException();

            if (n1.Equals(valueHelper.Zero))
                d1 = d2;
            else if (n2.Equals(valueHelper.Zero))
                d2 = d1;
            else if (!d1.Equals(d2))
            {
                n1 = GetSimplifiedRational<T>(valueHelper, n1, d1, out d1);
                n2 = GetSimplifiedRational<T>(valueHelper, n2, d2, out d2);

                if (d1.Equals(valueHelper.PositiveOne))
                    n1 = valueHelper.Multiply(n1, d2);
                else if (d2.Equals(valueHelper.PositiveOne))
                    n2 = valueHelper.Multiply(n2, d1);
                else if (!d1.Equals(d2))
                {
                    T m2;
                    T m1 = GetLCM<T>(valueHelper, d1, d2, out m2);
                    n1 = valueHelper.Multiply(n1, m1);
                    d1 = valueHelper.Multiply(d1, m1);
                    n2 = valueHelper.Multiply(n2, m1);
                    d2 = valueHelper.Multiply(d2, m1);
                }
            }
        }

        private static bool TryConvertValue<T>(object obj, out T value)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            throw new NotImplementedException();
        }

        private static T ConvertValue<T>(object obj)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            throw new NotImplementedException();
        }

        private static string ToString<T>(IValueHelper<T> valueHelper, T w, T n, T d)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (n.Equals(valueHelper.Zero))
                return w.ToString();

            if (w.Equals(valueHelper.Zero))
                return n.ToString() + "/" + d.ToString();

            return w.ToString() + " " + n.ToString() + "/" + d.ToString();
        }

        private static readonly Regex FractionParseRegex = new Regex(@"^(?(?=-?\d+(\s|$))(?<w>-?\d+)(\s+(?<n>-?\d+)/(?<d>-?\d+))?|(?<n>-?\d+)/(?<d>-?\d+))$", RegexOptions.Compiled);

        private static T Parse<T>(IValueHelper<T> valueHelper, string s, out T n, out T d)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            if (s == null)
                throw new ArgumentNullException("s");

            if (s.Length == 0)
                throw new FormatException("Input string was empty.");

            Match m = FractionParseRegex.Match(s);
            if (!m.Success)
                throw new FormatException("Input string was not in a correct format.");

            T w;
            if (m.Groups["w"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["w"].Value, out w))
                    throw new FormatException("Whole number in input string was not in a correct format.");
            }
            else
                w = valueHelper.Zero;
            if (m.Groups["n"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["n"].Value, out n))
                    throw new FormatException("Numerator in input string was not in a correct format.");
                if (!valueHelper.TryParse(m.Groups["d"].Value, out d))
                    throw new FormatException("Denominator in input string was not in a correct format.");
            }
            else
            {
                n = valueHelper.Zero;
                d = valueHelper.PositiveOne;
            }
            return w;
        }

        private static bool TryParse<T>(IValueHelper<T> valueHelper, string s, out T w, out T n, out T d)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            Match m;
            if (String.IsNullOrEmpty(s) || !(m = FractionParseRegex.Match(s)).Success)
            {
                w = valueHelper.Zero;
                n = valueHelper.Zero;
                d = valueHelper.PositiveOne;
                return false;
            }
            if (m.Groups["w"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["w"].Value, out w))
                {
                    n = valueHelper.Zero;
                    d = valueHelper.PositiveOne;
                    return false;
                }
            }
            else
                w = valueHelper.Zero;
            if (m.Groups["n"].Success)
            {
                if (!valueHelper.TryParse(m.Groups["n"].Value, out n))
                {
                    d = valueHelper.PositiveOne;
                    return false;
                }
                if (!valueHelper.TryParse(m.Groups["d"].Value, out d))
                    return false;
            }
            else
            {
                n = valueHelper.Zero;
                d = valueHelper.PositiveOne;
            }

            return true;
        }

        public static bool EqualTo<T>(IFraction<T> fraction, object obj)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            throw new NotImplementedException();
        }

        public static int Compare<T>(IFraction<T> fraction, object obj)
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 64-bit Methods

        public static long Parse64(string s, out long n, out long d) { return Parse<long>(ValueHelper64.Instance, s, out n, out d); }

        public static bool TryParse64(string s, out long w, out long n, out long d) { return TryParse<long>(ValueHelper64.Instance, s, out w, out n, out d); }

        public static bool TryConvertToInt64(object obj, out long value) { return TryConvertValue<long>(obj, out value); }

        public static long ToInt64(object obj, long defaultValue)
        {
            long value;
            if (TryConvertToInt64(obj, out value))
                return value;
            return defaultValue;
        }

        public static long ToInt64(object obj) { return ConvertValue<long>(obj); }

        public static long GetGCD64(long d1, params long[] denominators) { return GetGCD<long>(ValueHelper64.Instance, d1, denominators); }

        public static long GetLCM64(long d1, long d2, out long secondMultiplier) { return GetLCM<long>(ValueHelper64.Instance, d1, d2, out secondMultiplier); }

        public static long GetSimplifiedRational64(long n, long d, out long denominator) { return GetSimplifiedRational<long>(ValueHelper64.Instance, n, d, out denominator); }

        public static long GetInvertedRational64(long w, long n, long d, out long numerator, out long denominator) { return GetInvertedRational<long>(ValueHelper64.Instance, w, n, d, out numerator, out denominator); }

        public static void ToCommonDenominator64(ref long n1, ref long d1, ref long n2, ref long d2) { ToCommonDenominator<long>(ValueHelper64.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static long GetNormalizedRational64(long w, long n, long d, out long numerator, out long denominator) { return GetNormalizedRational<long>(ValueHelper64.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region 32-bit Methods

        public static int Parse32(string s, out int n, out int d) { return Parse<int>(ValueHelper32.Instance, s, out n, out d); }

        public static uint ParseU32(string s, out uint n, out uint d) { return Parse<uint>(ValueHelperU32.Instance, s, out n, out d); }

        public static bool TryParse32(string s, out int w, out int n, out int d) { return TryParse<int>(ValueHelper32.Instance, s, out w, out n, out d); }

        public static bool TryParseU32(string s, out uint w, out uint n, out uint d) { return TryParse<uint>(ValueHelperU32.Instance, s, out w, out n, out d); }

        public static bool TryConvertToInt32(object obj, out int value) { return TryConvertValue<int>(obj, out value); }

        public static bool TryConvertToUInt32(object obj, out uint value) { return TryConvertValue<uint>(obj, out value); }

        public static int ToInt32(object obj, int defaultValue)
        {
            int value;
            if (TryConvertToInt32(obj, out value))
                return value;
            return defaultValue;
        }

        public static uint ToUInt32(object obj, uint defaultValue)
        {
            uint value;
            if (TryConvertToUInt32(obj, out value))
                return value;
            return defaultValue;
        }

        public static int ToInt32(object obj) { return ConvertValue<int>(obj); }

        public static uint ToUInt32(object obj) { return ConvertValue<uint>(obj); }

        public static int GetGCD(int d1, params int[] denominators) { return GetGCD<int>(ValueHelper32.Instance, d1, denominators); }

        public static uint GetGCD(uint d1, params uint[] denominators) { return GetGCD<uint>(ValueHelperU32.Instance, d1, denominators); }

        public static int GetLCM(int d1, int d2, out int secondMultiplier) { return GetLCM<int>(ValueHelper32.Instance, d1, d2, out secondMultiplier); }

        public static uint GetLCM(uint d1, uint d2, out uint secondMultiplier) { return GetLCM<uint>(ValueHelperU32.Instance, d1, d2, out secondMultiplier); }

        public static int GetSimplifiedRational(int n, int d, out int denominator) { return GetSimplifiedRational<int>(ValueHelper32.Instance, n, d, out denominator); }

        public static uint GetSimplifiedRational(uint n, uint d, out uint denominator) { return GetSimplifiedRational<uint>(ValueHelperU32.Instance, n, d, out denominator); }

        public static int GetInvertedRational(int w, int n, int d, out int numerator, out int denominator) { return GetInvertedRational<int>(ValueHelper32.Instance, w, n, d, out numerator, out denominator); }

        public static uint GetInvertedRational(uint w, uint n, uint d, out uint numerator, out uint denominator) { return GetInvertedRational<uint>(ValueHelperU32.Instance, w, n, d, out numerator, out denominator); }

        public static void ToCommonDenominator(ref uint n1, ref uint d1, ref uint n2, ref uint d2) { ToCommonDenominator<uint>(ValueHelperU32.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static void ToCommonDenominator(ref int n1, ref int d1, ref int n2, ref int d2) { ToCommonDenominator<int>(ValueHelper32.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static uint GetNormalizedRational(uint w, uint n, uint d, out uint numerator, out uint denominator) { return GetNormalizedRational<uint>(ValueHelperU32.Instance, w, n, d, out numerator, out denominator); }

        public static int GetNormalizedRational(int w, int n, int d, out int numerator, out int denominator) { return GetNormalizedRational<int>(ValueHelper32.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region 16-bit Methods

        public static short Parse16(string s, out short n, out short d) { return Parse<short>(ValueHelper16.Instance, s, out n, out d); }

        public static bool TryParse16(string s, out short w, out short n, out short d) { return TryParse<short>(ValueHelper16.Instance, s, out w, out n, out d); }

        public static bool TryConvertToInt16(object obj, out short value) { return TryConvertValue<short>(obj, out value); }

        public static short ToInt16(object obj, short defaultValue)
        {
            short value;
            if (TryConvertToInt16(obj, out value))
                return value;
            return defaultValue;
        }

        public static short ToInt16(object obj) { return ConvertValue<short>(obj); }

        public static short GetGCD16(short d1, params short[] denominators) { return GetGCD<short>(ValueHelper16.Instance, d1, denominators); }

        public static short GetLCM16(short d1, short d2, out short secondMultiplier) { return GetLCM<short>(ValueHelper16.Instance, d1, d2, out secondMultiplier); }

        public static short GetSimplifiedRational16(short n, short d, out short denominator) { return GetSimplifiedRational<short>(ValueHelper16.Instance, n, d, out denominator); }

        public static void ToCommonDenominator16(ref short n1, ref short d1, ref short n2, ref short d2) { ToCommonDenominator<short>(ValueHelper16.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static short GetInvertedRational16(short w, short n, short d, out short numerator, out short denominator) { return GetInvertedRational<short>(ValueHelper16.Instance, w, n, d, out numerator, out denominator); }

        public static short GetNormalizedRational16(short w, short n, short d, out short numerator, out short denominator) { return GetNormalizedRational<short>(ValueHelper16.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region 8-bit Methods

        public static short Parse8(string s, out sbyte n, out sbyte d) { return Parse<sbyte>(ValueHelper8.Instance, s, out n, out d); }

        public static bool TryParse8(string s, out sbyte w, out sbyte n, out sbyte d) { return TryParse<sbyte>(ValueHelper8.Instance, s, out w, out n, out d); }

        public static bool TryConvertToSByte(object obj, out sbyte value) { return TryConvertValue<sbyte>(obj, out value); }

        public static sbyte ToSByte(object obj, sbyte defaultValue)
        {
            sbyte value;
            if (TryConvertToSByte(obj, out value))
                return value;
            return defaultValue;
        }

        public static sbyte ToSByte(object obj) { return ConvertValue<sbyte>(obj); }

        public static sbyte GetGCD8(sbyte d1, params sbyte[] denominators) { return GetGCD<sbyte>(ValueHelper8.Instance, d1, denominators); }

        public static sbyte GetLCM8(sbyte d1, sbyte d2, out sbyte secondMultiplier) { return GetLCM<sbyte>(ValueHelper8.Instance, d1, d2, out secondMultiplier); }

        public static sbyte GetSimplifiedRational8(sbyte n, sbyte d, out sbyte denominator) { return GetSimplifiedRational<sbyte>(ValueHelper8.Instance, n, d, out denominator); }

        public static sbyte GetInvertedRational8(sbyte w, sbyte n, sbyte d, out sbyte numerator, out sbyte denominator) { return GetInvertedRational<sbyte>(ValueHelper8.Instance, w, n, d, out numerator, out denominator); }

        public static void ToCommonDenominator8(ref sbyte n1, ref sbyte d1, ref sbyte n2, ref sbyte d2) { ToCommonDenominator<sbyte>(ValueHelper8.Instance, ref n1, ref d1, ref n2, ref d2); }

        public static sbyte GetNormalizedRational8(sbyte w, sbyte n, sbyte d, out sbyte numerator, out sbyte denominator) { return GetNormalizedRational<sbyte>(ValueHelper8.Instance, w, n, d, out numerator, out denominator); }

        #endregion

        #region IValueHelper<T> Implementations

        interface IValueHelper<T>
            where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        {
            T Zero { get; }

            T PositiveOne { get; }

            T NegativeOne { get; }

            T Parse(string s);

            bool TryParse(string s, out T value);
            T Abs(T value);

            T Add(T x, T y);

            T Subtract(T x, T y);

            T Divide(T x, T y);

            T Modulus(T x, T y);

            T Multiply(T x, T y);
        }

        class ValueHelper8 : IValueHelper<sbyte>
        {
            public static readonly ValueHelper8 Instance = new ValueHelper8();

            private ValueHelper8() { }

            public sbyte Zero { get { return 0; } }

            public sbyte PositiveOne { get { return 1; } }

            public sbyte NegativeOne { get { return -1; } }

            public sbyte Parse(string s) { return sbyte.Parse(s); }

            public bool TryParse(string s, out sbyte value) { return sbyte.TryParse(s, out value); }

            public sbyte Abs(sbyte value) { return (sbyte)(Math.Abs(value)); }

            public sbyte Add(sbyte x, sbyte y) { return (sbyte)(x + y); }

            public sbyte Subtract(sbyte x, sbyte y) { return (sbyte)(x - y); }

            public sbyte Divide(sbyte x, sbyte y) { return (sbyte)(x / y); }

            public sbyte Modulus(sbyte x, sbyte y) { return (sbyte)(x % y); }

            public sbyte Multiply(sbyte x, sbyte y) { return (sbyte)(x * y); }
        }

        class ValueHelper16 : IValueHelper<short>
        {
            public static readonly ValueHelper16 Instance = new ValueHelper16();

            private ValueHelper16() { }

            public short Zero { get { return 0; } }

            public short PositiveOne { get { return 1; } }

            public short NegativeOne { get { return -1; } }

            public short Parse(string s) { return short.Parse(s); }

            public bool TryParse(string s, out short value) { return short.TryParse(s, out value); }

            public short Abs(short value) { return Math.Abs(value); }

            public short Add(short x, short y) { return (short)(x + y); }

            public short Subtract(short x, short y) { return (short)(x - y); }

            public short Divide(short x, short y) { return (short)(x / y); }

            public short Modulus(short x, short y) { return (short)(x % y); }

            public short Multiply(short x, short y) { return (short)(x * y); }
        }

        class ValueHelper32 : IValueHelper<int>
        {
            public static readonly ValueHelper32 Instance = new ValueHelper32();

            private ValueHelper32() { }

            public int Zero { get { return 0; } }

            public int PositiveOne { get { return 1; } }

            public int NegativeOne { get { return -1; } }

            public int Parse(string s) { return int.Parse(s); }

            public bool TryParse(string s, out int value) { return int.TryParse(s, out value); }

            public int Abs(int value) { return Math.Abs(value); }

            public int Add(int x, int y) { return x + y; }

            public int Subtract(int x, int y) { return x - y; }

            public int Divide(int x, int y) { return x / y; }

            public int Modulus(int x, int y) { return x % y; }

            public int Multiply(int x, int y) { return x * y; }
        }

        class ValueHelperU32 : IValueHelper<uint>
        {
            public static readonly ValueHelperU32 Instance = new ValueHelperU32();

            private ValueHelperU32() { }

            public uint Zero { get { return 0; } }

            public uint PositiveOne { get { return 1; } }

            public uint NegativeOne => throw new NotSupportedException();

            public uint Parse(string s) { return uint.Parse(s); }

            public bool TryParse(string s, out uint value) { return uint.TryParse(s, out value); }

            public uint Abs(uint value) => value;

            public uint Add(uint x, uint y) { return x + y; }

            public uint Subtract(uint x, uint y) { return x - y; }

            public uint Divide(uint x, uint y) { return x / y; }

            public uint Modulus(uint x, uint y) { return x % y; }

            public uint Multiply(uint x, uint y) { return x * y; }
        }

        class ValueHelper64 : IValueHelper<long>
        {
            public static readonly ValueHelper64 Instance = new ValueHelper64();

            private ValueHelper64() { }

            public long Zero { get { return 0L; } }

            public long PositiveOne { get { return 1L; } }

            public long NegativeOne { get { return -1L; } }

            public long Parse(string s) { return long.Parse(s); }

            public bool TryParse(string s, out long value) { return long.TryParse(s, out value); }

            public long Abs(long value) { return Math.Abs(value); }

            public long Add(long x, long y) { return x + y; }

            public long Subtract(long x, long y) { return x - y; }

            public long Divide(long x, long y) { return x / y; }

            public long Modulus(long x, long y) { return x % y; }

            public long Multiply(long x, long y) { return x * y; }
        }

        #endregion
    }
}
