// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using nuint = System.UInt64;

namespace System.Text.CaseFolding
{
    internal static partial class SCFMarvin
    {
        /// <summary>
        /// Compute a Marvin hash and collapse it into a 32-bit hash.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ComputeHash32OrdinalIgnoreCase(ReadOnlySpan<char> data, ulong seed) => ComputeHash32OrdinalIgnoreCase(ref MemoryMarshal.GetReference(data), data.Length, (uint)seed, (uint)(seed >> 32));

        /// <summary>
        /// Compute a Marvin OrdinalIgnoreCase hash and collapse it into a 32-bit hash.
        /// n.b. <paramref name="count"/> is specified as char count, not byte count.
        /// </summary>
        public static int ComputeHash32OrdinalIgnoreCase(ref char data, int count, uint p0, uint p1)
        {
            uint ucount = (uint)count; // in chars
            nuint byteOffset = 0; // in bytes
            uint tempValue;

            // We operate on 32-bit integers (two chars) at a time.

            while (ucount >= 2)
            {
                tempValue = Unsafe.ReadUnaligned<uint>(ref Unsafe.As<char, byte>(ref Unsafe.AddByteOffset(ref data, (IntPtr)byteOffset)));
                if (!AllCharsInUInt32AreAscii(tempValue))
                {
                    goto NotAscii;
                }
                p0 += ConvertAllAsciiCharsInUInt32ToUppercase(tempValue);
                Block(ref p0, ref p1);

                byteOffset += 4;
                ucount -= 2;
            }

            // We have either one char (16 bits) or zero chars left over.
            Debug.Assert(ucount < 2);

            if (ucount > 0)
            {
                tempValue = Unsafe.AddByteOffset(ref data, (IntPtr)byteOffset);
                if (tempValue > 0x7Fu)
                {
                    goto NotAscii;
                }

                // addition is written with -0x80u to allow fall-through to next statement rather than jmp past it
                p0 += ConvertAllAsciiCharsInUInt32ToUppercase(tempValue) + (0x800000u - 0x80u);
            }
            p0 += 0x80u;

            Block(ref p0, ref p1);
            Block(ref p0, ref p1);

            return (int)(p1 ^ p0);

        NotAscii:
            Debug.Assert(0 <= ucount && ucount <= Int32.MaxValue); // this should fit into a signed int
            return ComputeHash32OrdinalIgnoreCaseSlow(ref Unsafe.AddByteOffset(ref data, (IntPtr)byteOffset), (int)ucount, p0, p1);
        }

        private static unsafe int ComputeHash32OrdinalIgnoreCaseSlow(ref char data, int count, uint p0, uint p1)
        {
            Debug.Assert(count > 0);

            char[] borrowedArr = null;
            Span<char> scratch = (uint)count <= 64 ? stackalloc char[64] : (borrowedArr = ArrayPool<char>.Shared.Rent(count));

            SimpleCaseFolding.SimpleCaseFold(new ReadOnlySpan<char>(Unsafe.AsPointer(ref data), count), scratch);

            // Slice the array to the size returned by ToUpperInvariant.
            // Multiplication below may overflow, that's fine since it's going to an unsigned integer.
            int hash = ComputeHash32(ref Unsafe.As<char, byte>(ref MemoryMarshal.GetReference(scratch)), count * 2, p0, p1);

            // Return the borrowed array if necessary.
            if (borrowedArr != null)
            {
                ArrayPool<char>.Shared.Return(borrowedArr);
            }

            return hash;
        }

        /// <summary>
        /// Returns true iff the UInt32 represents two ASCII UTF-16 characters in machine endianness.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AllCharsInUInt32AreAscii(uint value)
        {
            return (value & ~0x007F_007Fu) == 0;
        }

        /// <summary>
        /// Given a UInt32 that represents two ASCII UTF-16 characters, returns the invariant
        /// uppercase representation of those characters. Requires the input value to contain
        /// two ASCII UTF-16 characters in machine endianness.
        /// </summary>
        /// <remarks>
        /// This is a branchless implementation.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static uint ConvertAllAsciiCharsInUInt32ToUppercase(uint value)
        {
            // ASSUMPTION: Caller has validated that input value is ASCII.
            Debug.Assert(AllCharsInUInt32AreAscii(value));

            // the 0x80 bit of each word of 'lowerIndicator' will be set iff the word has value >= 'a'
            uint lowerIndicator = value + 0x0080_0080u - 0x0061_0061u;

            // the 0x80 bit of each word of 'upperIndicator' will be set iff the word has value > 'z'
            uint upperIndicator = value + 0x0080_0080u - 0x007B_007Bu;

            // the 0x80 bit of each word of 'combinedIndicator' will be set iff the word has value >= 'a' and <= 'z'
            uint combinedIndicator = (lowerIndicator ^ upperIndicator);

            // the 0x20 bit of each word of 'mask' will be set iff the word has value >= 'a' and <= 'z'
            uint mask = (combinedIndicator & 0x0080_0080u) >> 2;

            return value ^ mask; // bit flip lowercase letters [a-z] => [A-Z]
        }

    }
 }
