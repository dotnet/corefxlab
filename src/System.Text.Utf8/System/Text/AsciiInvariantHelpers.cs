// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text
{
    internal static class AsciiInvariantHelpers
    {
        /// <summary>
        /// Given 4 ASCII bytes packed into a DWORD, returns the equivalent packed
        /// byte representation where all uppercase ASCII bytes have been converted
        /// to lowercase. The input must be ASCII-only.
        /// </summary>
        public static uint ConvertPackedBytesToLowercase(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // See comment in PackedBytesContainsLowercaseAsciiChar for how this works.
            // n.b. 0x41 is 'A', 0x5A is 'Z'

            uint p = packedBytes + 0x80808080U - 0x41414141U;
            uint q = packedBytes + 0x80808080U - 0x5B5B5B5BU;
            uint mask = (p ^ q) & 0x80808080U;

            // Each high bit of mask represents a byte that needs to have its 0x20 bit flipped.
            // This will convert lowercase <-> uppercase.

            return packedBytes ^ (mask >> 2);
        }

        /// <summary>
        /// Given 4 ASCII bytes packed into a DWORD, returns the equivalent packed
        /// byte representation where all lowercase ASCII bytes have been converted
        /// to uppercase. The input must be ASCII-only.
        /// </summary>
        public static uint ConvertPackedBytesToUppercase(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // See comment in PackedBytesContainsLowercaseAsciiChar for how this works.
            // n.b. 0x61 is 'a', 0x7A is 'z'

            uint p = packedBytes + 0x80808080U - 0x61616161U;
            uint q = packedBytes + 0x80808080U - 0x7B7B7B7BU;
            uint mask = (p ^ q) & 0x80808080U;

            // Each high bit of mask represents a byte that needs to have its 0x20 bit flipped.
            // This will convert lowercase <-> uppercase.

            return packedBytes ^ (mask >> 2);
        }

        /// <summary>
        /// Returns <see langword="true"/> iff <paramref name="utf16"/> is a valid UTF-16 represention
        /// of the ASCII buffer <paramref name="ascii"/>. The <paramref name="ascii"/> parameter must
        /// contain only ASCII data.
        /// </summary>
        public static bool EqualsCaseSensitive(ReadOnlySpan<byte> ascii, ReadOnlySpan<char> utf16)
        {
            // Assuming the input is all-ASCII, the UTF-16 representation should have the same code unit count.

            if (ascii.Length != utf16.Length)
            {
                return false;
            }

            return EqualsCaseSensitiveCore(ref MemoryMarshal.GetReference(ascii), ref MemoryMarshal.GetReference(utf16), (nuint)ascii.Length);
        }

        private static bool EqualsCaseSensitiveCore(ref byte ascii, ref char utf16, nuint length)
        {
            // Use vectorization if available

            if (Vector.IsHardwareAccelerated)
            {
                nuint tooFarPosition = length & ~(Vector<byte>.Count - 1);
                for (nuint i = 0; i < tooFarPosition; i += Vector<byte>.Count)
                {
                    Vector.Widen(
                        Unsafe.ReadUnaligned<Vector<byte>>(ref Unsafe.Add(ref ascii, i)),
                        out Vector<ushort> widenedAscii1,
                        out Vector<ushort> widenedAscii2);

                    if ((widenedAscii1 != Unsafe.ReadUnaligned<Vector<ushort>>(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref utf16, i))))
                        || (widenedAscii1 != Unsafe.ReadUnaligned<Vector<ushort>>(ref Unsafe.Add(ref Unsafe.As<char, byte>(ref Unsafe.Add(ref utf16, i)), Vector<byte>.Count))))
                    {
                        return false; // found a block that didn't match
                    }
                }

                ascii = ref Unsafe.Add(ref ascii, tooFarPosition);
                utf16 = ref Unsafe.Add(ref utf16, tooFarPosition);
                length &= Vector<byte>.Count - 1;
            }

            // Now work with remaining data, non-vectorized

            for (nuint i = 0; i < length; i++)
            {
                if (Unsafe.Add(ref ascii, i) != Unsafe.Add(ref utf16, i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the index of the first lowercase ASCII character in the buffer,
        /// or -1 if the buffer does not contain any lowercase ASCII characters. The input
        /// must be ASCII-only. The return value may eagerly return an index *before* the
        /// real first lowercase character, but it will never return an index *after* the
        /// first such character. For example, if the first lowercase character is at
        /// index 7, this function may return 4, but it will not return 8.
        /// </summary>
        public static unsafe int GetIndexOfFirstLowercaseAsciiChar(ReadOnlySpan<byte> buffer)
        {
            // TODO: Consider whether real vectorization would make a difference here.
            // Are typical inputs to this function long enough to benefit from vectorization?

            // Try checking 32-bit blocks first.

            uint chunkCount = (uint)buffer.Length / 4; // round down
            IntPtr i = IntPtr.Zero;
            for (; (uint)(void*)i < chunkCount; i += 1)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref MemoryMarshal.GetReference(buffer));
                uint originalData = Unsafe.ReadUnaligned<uint>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                if (PackedBytesContainsLowercaseAsciiChar(originalData))
                {
                    return 4 * (int)(void*)i;
                }
            }

            // Now for the last three bytes.

            if ((buffer.Length & 2) != 0)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref MemoryMarshal.GetReference(buffer));
                uint originalData = Unsafe.ReadUnaligned<ushort>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                if (PackedBytesContainsLowercaseAsciiChar(originalData))
                {
                    return 4 * (int)(void*)i;
                }
            }

            if ((buffer.Length & 1) != 0)
            {
                if (UnicodeHelpers.IsInRangeInclusive(Unsafe.Add(ref Unsafe.Add(ref MemoryMarshal.GetReference(buffer), buffer.Length), -1), 'a', 'z'))
                {
                    return buffer.Length - 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the first uppercase ASCII character in the buffer,
        /// or -1 if the buffer does not contain any uppercase ASCII characters. The input
        /// must be ASCII-only. The return value may eagerly return an index *before* the
        /// real first uppercase character, but it will never return an index *after* the
        /// first such character. For example, if the first uppercase character is at
        /// index 7, this function may return 4, but it will not return 8.
        /// </summary>
        public static unsafe int GetIndexOfFirstUppercaseAsciiChar(ReadOnlySpan<byte> buffer)
        {
            // TODO: Consider whether real vectorization would make a difference here.
            // Are typical inputs to this function long enough to benefit from vectorization?

            // Try checking 32-bit blocks first.

            uint chunkCount = (uint)buffer.Length / 4; // round down
            IntPtr i = IntPtr.Zero;
            for (; (uint)(void*)i < chunkCount; i += 1)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref MemoryMarshal.GetReference(buffer));
                uint originalData = Unsafe.ReadUnaligned<uint>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                if (PackedBytesContainsUppercaseAsciiChar(originalData))
                {
                    return 4 * (int)(void*)i;
                }
            }

            // Now for the last three bytes.

            if ((buffer.Length & 2) != 0)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref MemoryMarshal.GetReference(buffer));
                uint originalData = Unsafe.ReadUnaligned<ushort>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                if (PackedBytesContainsUppercaseAsciiChar(originalData))
                {
                    return 4 * (int)(void*)i;
                }
            }

            if ((buffer.Length & 1) != 0)
            {
                if (UnicodeHelpers.IsInRangeInclusive(Unsafe.Add(ref Unsafe.Add(ref MemoryMarshal.GetReference(buffer), buffer.Length), -1), 'A', 'Z'))
                {
                    return buffer.Length - 1;
                }
            }

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Returns <see langword="true"/> iff any byte of the input value represents a
        /// lowercase ASCII character. The input must be ASCII-only.
        /// </summary>
        public static bool PackedBytesContainsLowercaseAsciiChar(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // The input value is of the following form:
            // 0wwwwwww 0xxxxxxx 0yyyyyyy 0zzzzzzz
            //
            // We can set the high bit of each byte of the input, which allows
            // us to treat the high bit as a carry bit to determine if each
            // individual byte was greater than or equal to the search value.

            // 0x61 is 'a', so the high bit of each byte of p will be set
            // iff the corresponding byte was >= 0x61.

            uint p = packedBytes + 0x80808080U - 0x61616161U;

            // 0x7A is 'z', so the high bit of each byte of q will be set
            // iff the corresponding byte was >= 0x7B.

            uint q = packedBytes + 0x80808080U - 0x7B7B7B7BU;

            // We now compare the high bit each byte of p against the high
            // bit of each byte of q. This has the following result matrix.
            //
            // p_high = 0, q_high = 0 ===> byte is < 0x61, not a lowercase ASCII char
            //          0           1 ===> (cannot happen)
            //          1           0 ===> 0x61h <= byte < 0x7B, lowercase ASCII char
            //          1           1 ===> byte is >= 0x7B, not a lowercase ASCII char

            return (((p ^ q) & 0x80808080U) != 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        /// <summary>
        /// Returns <see langword="true"/> iff any byte of the input value represents a
        /// uppercase ASCII character. The input must be ASCII-only.
        /// </summary>
        public static bool PackedBytesContainsUppercaseAsciiChar(uint packedBytes)
        {
            UnicodeDebug.AssertContainsOnlyAsciiBytes(packedBytes);

            // See comment in PackedBytesContainsLowercaseAsciiChar for how this works.
            // n.b. 0x41 is 'A', 0x5A is 'Z'

            uint p = packedBytes + 0x80808080U - 0x41414141U;
            uint q = packedBytes + 0x80808080U - 0x5B5B5B5BU;
            return (((p ^ q) & 0x80808080U) != 0);
        }

        /// <summary>
        /// Copies ASCII data from the source to the destination, converting uppercase characters
        /// to lowercase during the copy. The input buffers may not overlap, and the input data
        /// must be ASCII.
        /// </summary>
        public static unsafe void ToLowerInvariant(ref byte source, ref byte destination, uint length)
        {
            UnicodeDebug.AssertDoesNotOverlap(ref source, ref destination, length);

            // TODO: Consider whether real vectorization would make a difference here.
            // Are typical inputs to this function long enough to benefit from vectorization?

            // Try checking 32-bit blocks first.

            uint chunkCount = length / 4; // round down
            IntPtr i = IntPtr.Zero;
            for (; (uint)(void*)i < chunkCount; i += 1)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref source);
                uint originalData = Unsafe.ReadUnaligned<uint>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                uint modifiedData = ConvertPackedBytesToLowercase(originalData);
                ref uint destinationAsDwordPtr = ref Unsafe.As<byte, uint>(ref destination);
                Unsafe.WriteUnaligned<uint>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref destinationAsDwordPtr, i)), modifiedData);
            }

            // Now for the last three bytes.

            if ((length & 2) != 0)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref source);
                uint originalData = Unsafe.ReadUnaligned<ushort>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                uint modifiedData = ConvertPackedBytesToLowercase(originalData);
                ref uint destinationAsDwordPtr = ref Unsafe.As<byte, uint>(ref destination);
                Unsafe.WriteUnaligned<ushort>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref destinationAsDwordPtr, i)), (ushort)modifiedData);
            }

            if ((length & 1) != 0)
            {
                IntPtr naturalLength = (IntPtr)(void*)length;
                uint originalData = Unsafe.Add(ref Unsafe.Add(ref source, naturalLength), -1);
                uint modifiedData = ConvertPackedBytesToLowercase(originalData);
                Unsafe.Add(ref Unsafe.Add(ref destination, naturalLength), -1) = (byte)modifiedData;
            }
        }

        /// <summary>
        /// Copies ASCII data from the source to the destination, converting lowercase characters
        /// to uppercase during the copy. The input buffers may not overlap, and the input data
        /// must be ASCII.
        /// </summary>
        public static unsafe void ToUpperInvariant(ref byte source, ref byte destination, uint length)
        {
            UnicodeDebug.AssertDoesNotOverlap(ref source, ref destination, length);

            // TODO: Consider whether real vectorization would make a difference here.
            // Are typical inputs to this function long enough to benefit from vectorization?

            // Try checking 32-bit blocks first.

            uint chunkCount = length / 4; // round down
            IntPtr i = IntPtr.Zero;
            for (; (uint)(void*)i < chunkCount; i += 1)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref source);
                uint originalData = Unsafe.ReadUnaligned<uint>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                uint modifiedData = ConvertPackedBytesToUppercase(originalData);
                ref uint destinationAsDwordPtr = ref Unsafe.As<byte, uint>(ref destination);
                Unsafe.WriteUnaligned<uint>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref destinationAsDwordPtr, i)), modifiedData);
            }

            // Now for the last three bytes.

            if ((length & 2) != 0)
            {
                ref uint sourceAsDwordPtr = ref Unsafe.As<byte, uint>(ref source);
                uint originalData = Unsafe.ReadUnaligned<ushort>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref sourceAsDwordPtr, i)));
                uint modifiedData = ConvertPackedBytesToUppercase(originalData);
                ref uint destinationAsDwordPtr = ref Unsafe.As<byte, uint>(ref destination);
                Unsafe.WriteUnaligned<ushort>(ref Unsafe.As<uint, byte>(ref Unsafe.Add(ref destinationAsDwordPtr, i)), (ushort)modifiedData);
            }

            if ((length & 1) != 0)
            {
                IntPtr naturalLength = (IntPtr)(void*)length;
                uint originalData = Unsafe.Add(ref Unsafe.Add(ref source, naturalLength), -1);
                uint modifiedData = ConvertPackedBytesToUppercase(originalData);
                Unsafe.Add(ref Unsafe.Add(ref destination, naturalLength), -1) = (byte)modifiedData;
            }
        }
    }
}
