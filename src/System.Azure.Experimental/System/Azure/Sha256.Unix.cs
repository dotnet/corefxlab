// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Security.Cryptography;

namespace System.Buffers.Cryptography
{
    public struct Sha256_Unix
    {
        IncrementalHash _hash; 
        public static Sha256_Unix Create(byte[] seed)
        {
            var sha = new Sha256_Unix();
            sha._hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            return sha;
        }

        public int OutputSize => 256 / 8;
        public unsafe void Append(ReadOnlySpan<byte> input)
        {
            _hash.AppendData(input.ToArray());
        }

        public unsafe void GetHash(Span<byte> output)
        {
            var hash = _hash.GetHashAndReset();
            if (hash.Length > output.Length) throw new ArgumentException();
            hash.AsSpan().CopyTo(output);
        }

        public void Dispose()
        {
            _hash.Dispose();
            _hash = null;
        }
    }
}
