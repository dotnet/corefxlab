// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace System.Buffers.Cryptography
{
    public struct Sha256 : IWritable
    {
        IntPtr _algorithm;
        IntPtr _hash;

        public static Sha256 Create(byte[] seed)
        {
            var sha = new Sha256();
            var result = BCryptOpenAlgorithmProvider(out sha._algorithm, "SHA256", null, BCryptFlags.BCRYPT_ALG_HANDLE_HMAC);
            if (result != 0) throw new Exception();
            unsafe
            {
                fixed (byte* pSeed = seed)
                {
                    result = BCryptCreateHash(sha._algorithm, out sha._hash, IntPtr.Zero, 0, pSeed, seed.Length, BCryptFlags.BCRYPT_HASH_REUSABLE_FLAG);
                }
            }
            if (result != 0) throw new Exception();
            return sha;
        }

        public int OutputSize => 256 / 8; // Sha256 length in bytes
        public unsafe void Append(ReadOnlySpan<byte> input)
        {
            fixed (byte* pInput = &MemoryMarshal.GetReference(input))
            {
                var result = BCryptHashData(_hash, pInput, input.Length, 0);
                if (result != 0) throw new Exception();
            }
        }

        public bool TryWrite(Span<byte> buffer, out int written, StandardFormat format = default)
        {
            if (!format.IsDefault) throw new ArgumentOutOfRangeException(nameof(format));
            if (buffer.Length < OutputSize) { written = 0; return false; }

            unsafe
            {
                fixed (byte* pOutput = &MemoryMarshal.GetReference(buffer))
                {
                    var result = BCryptFinishHash(_hash, pOutput, OutputSize, 0);
                    if (result != 0) throw new Exception();
                }
            }
            written = OutputSize;
            return true;
        }

        public void Dispose()
        {
            var result = BCryptDestroyHash(_hash);
            if (result != 0) throw new Exception();
            result = BCryptCloseAlgorithmProvider(_algorithm, 0);
            if (result != 0) throw new Exception();
        }

        [DllImport("bcrypt.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        extern static int BCryptOpenAlgorithmProvider(out IntPtr handle, string algId, string implementation, BCryptFlags flags);

        [DllImport("bcrypt.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        extern static int BCryptCloseAlgorithmProvider(IntPtr handle, uint flags);

        [DllImport("bcrypt.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        extern static unsafe int BCryptCreateHash(IntPtr hAlgorithm, out IntPtr phHash, IntPtr pbHashObject, int cbHashObject, byte* pbSecret, int cbSecret, BCryptFlags dwFlags);

        [DllImport("bcrypt.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        extern static int BCryptDestroyHash(IntPtr hHash);

        [DllImport("bcrypt.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        extern static unsafe int BCryptHashData(IntPtr hHash, byte* pbInput, int cbInput, int dwFlags);

        [DllImport("bcrypt.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Unicode)]
        extern static unsafe int BCryptFinishHash(IntPtr hHash, byte* pbOutput, int cbOutput, int dwFlags);

        [Flags]
        enum BCryptFlags : int
        {
            None = 0x00000000,
            BCRYPT_ALG_HANDLE_HMAC = 0x00000008,
            BCRYPT_HASH_REUSABLE_FLAG = 0x00000020,
        }
    }
}
