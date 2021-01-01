using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FsInfoCat
{
    [StructLayout(LayoutKind.Explicit)]
    public struct PwHash : IEquatable<PwHash>, IEquatable<string>
    {
        private const int FIELD_OFFSET_1 = 8;
        private const int FIELD_OFFSET_2 = 16;
        private const int FIELD_OFFSET_3 = 24;
        private const int FIELD_OFFSET_4 = 32;
        private const int FIELD_OFFSET_5 = 40;
        private const int FIELD_OFFSET_6 = 48;
        private const int FIELD_OFFSET_7 = 56;
        public const int HASH_BYTES_LENGTH = 64;
        public const int SALT_BYTES_LENGTH = 8;
        public const int TOTAL_LENGTH = HASH_BYTES_LENGTH + SALT_BYTES_LENGTH;

        [FieldOffset(0)]
        private readonly ulong _hashBits0;

        [FieldOffset(FIELD_OFFSET_1)]
        private readonly ulong _hashBits1;

        [FieldOffset(FIELD_OFFSET_2)]
        private readonly ulong _hashBits2;

        [FieldOffset(FIELD_OFFSET_3)]
        private readonly ulong _hashBits3;

        [FieldOffset(FIELD_OFFSET_4)]
        private readonly ulong _hashBits4;

        [FieldOffset(FIELD_OFFSET_5)]
        private readonly ulong _hashBits5;

        [FieldOffset(FIELD_OFFSET_6)]
        private readonly ulong _hashBits6;

        [FieldOffset(FIELD_OFFSET_7)]
        private readonly ulong _hashBits7;

        [FieldOffset(HASH_BYTES_LENGTH)]
        private readonly ulong _saltBits;

        private PwHash(byte[] sha512Hash, ulong salt)
        {
            _hashBits0 = BitConverter.ToUInt64(sha512Hash, 0);
            _hashBits1 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_1);
            _hashBits2 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_2);
            _hashBits3 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_3);
            _hashBits4 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_4);
            _hashBits5 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_5);
            _hashBits6 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_6);
            _hashBits7 = BitConverter.ToUInt64(sha512Hash, FIELD_OFFSET_7);
            _saltBits = salt;
        }

        private static byte[] ComputeHash(string rawPw, byte[] salt)
        {
            using (SHA512 sha = SHA512.Create())
            {
                sha.ComputeHash(Encoding.ASCII.GetBytes(rawPw).Concat(salt).ToArray());
                return sha.Hash;
            }
        }

        public static PwHash? Import(string base64EncodedHash)
        {
            if (string.IsNullOrWhiteSpace(base64EncodedHash))
                return null;
            byte[] data;
            try { data = Convert.FromBase64String(base64EncodedHash); }
            catch (Exception exc) { throw new ArgumentException("Invalid base-64 string", "base64EncodedHash", exc); }
            if (data.Length != TOTAL_LENGTH)
                throw new ArgumentException("Invalid data length", "base64EncodedHash");
            return new PwHash(data, BitConverter.ToUInt64(data, HASH_BYTES_LENGTH));
        }

        public static PwHash? Create(string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return null;
            byte[] salt = new byte[SALT_BYTES_LENGTH];
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                cryptoServiceProvider.GetBytes(salt);
            return new PwHash(ComputeHash(rawPw, salt), BitConverter.ToUInt64(salt, 0));
        }

        public static PwHash? Create(string rawPw, ulong salt)
        {
            if (string.IsNullOrEmpty(rawPw))
                return null;
            return new PwHash(ComputeHash(rawPw, BitConverter.GetBytes(salt)), salt);
        }

        public static bool Test(PwHash? hash, string rawPw)
        {
            return (hash.HasValue) ? hash.Value.Test(rawPw) : string.IsNullOrEmpty(rawPw);
        }

        public bool Test(string rawPw)
        {
            if (string.IsNullOrEmpty(rawPw))
                return false;
            byte[] hash = ComputeHash(rawPw, BitConverter.GetBytes(_saltBits));
            return BitConverter.ToUInt64(hash, 0) == _hashBits0 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_1) == _hashBits1 && BitConverter.ToUInt64(hash, FIELD_OFFSET_2) == _hashBits2 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_3) == _hashBits3 && BitConverter.ToUInt64(hash, FIELD_OFFSET_4) == _hashBits4 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_5) == _hashBits5 && BitConverter.ToUInt64(hash, FIELD_OFFSET_6) == _hashBits6 &&
                BitConverter.ToUInt64(hash, FIELD_OFFSET_7) == _hashBits7;
        }

        public override string ToString()
        {
            return Convert.ToBase64String(BitConverter.GetBytes(_hashBits0).Concat(BitConverter.GetBytes(_hashBits1)).Concat(BitConverter.GetBytes(_hashBits2))
                .Concat(BitConverter.GetBytes(_hashBits3)).Concat(BitConverter.GetBytes(_hashBits4)).Concat(BitConverter.GetBytes(_hashBits5))
                .Concat(BitConverter.GetBytes(_hashBits6)).Concat(BitConverter.GetBytes(_hashBits7)).Concat(BitConverter.GetBytes(_saltBits))
                .ToArray());
        }

        public bool Equals(PwHash other)
        {
            return _hashBits0 == other._hashBits0 && _hashBits1 == other._hashBits1 && _hashBits2 == other._hashBits2 &&
                _hashBits3 == other._hashBits3 && _hashBits4 == other._hashBits4 && _hashBits5 == other._hashBits5 &&
                _hashBits6 == other._hashBits6 && _hashBits7 == other._hashBits7 && _saltBits == other._saltBits;
        }

        public bool Equals([AllowNull] string other)
        {
            if (string.IsNullOrWhiteSpace(other))
                return false;
            byte[] data;
            try { data = Convert.FromBase64String(other); }
            catch { return false; }
            return data.Length == TOTAL_LENGTH && BitConverter.ToUInt64(data, 0) == _hashBits0 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_1) == _hashBits1 && BitConverter.ToUInt64(data, FIELD_OFFSET_2) == _hashBits2 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_3) == _hashBits3 && BitConverter.ToUInt64(data, FIELD_OFFSET_4) == _hashBits4 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_5) == _hashBits5 && BitConverter.ToUInt64(data, FIELD_OFFSET_6) == _hashBits6 &&
                BitConverter.ToUInt64(data, FIELD_OFFSET_7) == _hashBits7 && BitConverter.ToUInt64(data, HASH_BYTES_LENGTH) == _saltBits;
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            if (obj is PwHash)
                return Equals((PwHash)obj);
            return obj is string && Equals((string)obj);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new HashCode();
            hashCode.Add(_hashBits0);
            hashCode.Add(_hashBits1);
            hashCode.Add(_hashBits2);
            hashCode.Add(_hashBits3);
            hashCode.Add(_hashBits4);
            hashCode.Add(_hashBits5);
            hashCode.Add(_hashBits6);
            hashCode.Add(_hashBits7);
            hashCode.Add(_saltBits);
            return hashCode.ToHashCode();
        }
    }
}
