using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IHashCalculation : ITimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }
        long Length { get; }
        IReadOnlyCollection<byte> Data { get; }
        IReadOnlyCollection<IFile> Files { get; }
        bool TryGetMD5Checksum(out UInt128 result);
    }
}
