// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace System.Buffers.Cryptography
{
    public struct Sha256 : IWritable
    {
        IncrementalHash _hash;
        public static Sha256 Create(byte[] seed)
        {
            var sha = new Sha256();
            sha._hash = IncrementalHash.CreateHMAC(HashAlgorithmName.SHA256, seed);
            return sha;
        }

        public int OutputSize => 256 / 8;
        public unsafe void Append(ReadOnlySpan<byte> input)
        {
            _hash.AppendData(input.ToArray());
        }

        public bool TryWrite(Span<byte> buffer, out int written, StandardFormat format = default)
        {
            if (!format.IsDefault) throw new ArgumentOutOfRangeException(nameof(format));
            if (buffer.Length < OutputSize) { written = 0; return false; }
            
            var hash = _hash.GetHashAndReset();
            Debug.Assert(hash.Length == OutputSize);
            hash.AsSpan().CopyTo(buffer);

            written = OutputSize;
            return true;
        }

        public void Dispose()
        {
            _hash.Dispose();
            _hash = null;
        }
    }
}
