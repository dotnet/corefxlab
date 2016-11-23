﻿//------------------------------------------------------------------------------
// <auto-generated>look at the SpanExtensionsTemplate.tt</auto-generated>
//------------------------------------------------------------------------------
// Copyright (c) Microsoft. All rights reserved. 
// Licensed under the MIT license. See LICENSE file in the project root for full license information. 

using System.Runtime.CompilerServices;

namespace System
{
    public static partial class SpanExtensions
    {
        /// <summary>
        /// platform independent fast memory comparison
        /// for x64 it is as fast as memcmp of msvcrt.dll, for x86 it is up to two times faster!!
        /// </summary>
        internal static bool MemoryEqual<[Primitive]T>(Span<T> first, Span<T> second)
            where T : struct
        {
            int length = first.Length;
            if (length != second.Length)
            {
                return false;
            }

            // @todo: Needs pinning support to be added to Span to restore the original compare strategy.
            for (int i = 0; i < length; i++)
            {
                if (!(first[i].Equals(second[i])))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// platform independent fast memory comparison
        /// for x64 it is as fast as memcmp of msvcrt.dll, for x86 it is up to two times faster!!
        /// </summary>
        internal static bool MemoryEqual<[Primitive]T>(ReadOnlySpan<T> first, ReadOnlySpan<T> second)
            where T : struct
        {
            int length = first.Length;
            if (length != second.Length)
            {
                return false;
            }

            // @todo: Needs pinning support to be added to Span to restore the original compare strategy.
            for (int i = 0; i < length; i++)
            {
                if (!(first[i].Equals(second[i])))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether two spans are equal by comparing the elements by using generic Equals method
        /// </summary>
        /// <param name="first">A span of type T to compare to second.</param>
        /// <param name="second">A span of type T to compare to first.</param>
        public static bool SequenceEqual<T>(this Span<T> first, Span<T> second)
            where T : struct, IEquatable<T>
        {
            int length = first.Length;
            if (length != second.Length)
                return false;

            // @todo: Can capture and iterate ref with DangerousGetPinnedReference() like the IL used to, but need C#7 for that.
            for (int i = 0; i < length; i++)
            {
                if (!first[i].Equals(second[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether two spans are equal by comparing the elements by using generic Equals method
        /// </summary>
        /// <param name="first">A span of type T to compare to second.</param>
        /// <param name="second">A span of type T to compare to first.</param>
        public static bool SequenceEqual<T>(this ReadOnlySpan<T> first, ReadOnlySpan<T> second)
            where T : struct, IEquatable<T>
        {
            int length = first.Length;
            if (length != second.Length)
                return false;

            // @todo: Can capture and iterate ref with DangerousGetPinnedReference() like the IL used to, but need C#7 for that.
            for (int i = 0; i < length; i++)
            {
                if (!first[i].Equals(second[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines whether two spans are equal by comparing the elements by using generic Equals method
        /// </summary>
        /// <param name="first">A span of type T to compare to second.</param>
        /// <param name="second">A span of type T to compare to first.</param>
        public static bool SequenceEqual<T>(this Span<T> first, ReadOnlySpan<T> second)
            where T : struct, IEquatable<T>
        {
            return SequenceEqual((ReadOnlySpan<T>)first, second);
        }

        /// <summary>
        /// Determines whether two spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of bytes to compare to second.</param>
        /// <param name="second">A span of bytes T to compare to first.</param>
        public static bool SequenceEqual(this Span<byte> first, Span<byte> second)
        {
            return first.Length >= 320
                ? MemoryEqual(first, second)
                : SequenceEqual<byte>(first, second);
        }

        /// <summary>
        /// Determines whether two read-only spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of bytes to compare to second.</param>
        /// <param name="second">A span of bytes T to compare to first.</param>
        public static bool SequenceEqual(this ReadOnlySpan<byte> first, ReadOnlySpan<byte> second)
        {
            return first.Length >= 320
                ? MemoryEqual(first, second)
                : SequenceEqual<byte>(first, second);
        }

        /// <summary>
        /// Determines whether two spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of characters to compare to second.</param>
        /// <param name="second">A span of characters T to compare to first.</param>
        public static bool SequenceEqual(this Span<char> first, Span<char> second)
        {
            return first.Length >= 512
                ? MemoryEqual(first, second)
                : SequenceEqual<char>(first, second);
        }

        /// <summary>
        /// Determines whether two read-only spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of characters to compare to second.</param>
        /// <param name="second">A span of characters T to compare to first.</param>
        public static bool SequenceEqual(this ReadOnlySpan<char> first, ReadOnlySpan<char> second)
        {
            return first.Length >= 512
                ? MemoryEqual(first, second)
                : SequenceEqual<char>(first, second);
        }

        /// <summary>
        /// Determines whether two spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of shorts to compare to second.</param>
        /// <param name="second">A span of shorts T to compare to first.</param>
        public static bool SequenceEqual(this Span<short> first, Span<short> second)
        {
            return first.Length >= 512
                ? MemoryEqual(first, second)
                : SequenceEqual<short>(first, second);
        }

        /// <summary>
        /// Determines whether two read-only spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of shorts to compare to second.</param>
        /// <param name="second">A span of shorts T to compare to first.</param>
        public static bool SequenceEqual(this ReadOnlySpan<short> first, ReadOnlySpan<short> second)
        {
            return first.Length >= 512
                ? MemoryEqual(first, second)
                : SequenceEqual<short>(first, second);
        }

        /// <summary>
        /// Determines whether two spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of integers to compare to second.</param>
        /// <param name="second">A span of integers T to compare to first.</param>
        public static bool SequenceEqual(this Span<int> first, Span<int> second)
        {
            return first.Length >= 256
                ? MemoryEqual(first, second)
                : SequenceEqual<int>(first, second);
        }

        /// <summary>
        /// Determines whether two read-only spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of integers to compare to second.</param>
        /// <param name="second">A span of integers T to compare to first.</param>
        public static bool SequenceEqual(this ReadOnlySpan<int> first, ReadOnlySpan<int> second)
        {
            return first.Length >= 256
                ? MemoryEqual(first, second)
                : SequenceEqual<int>(first, second);
        }

        /// <summary>
        /// Determines whether two spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of long integers to compare to second.</param>
        /// <param name="second">A span of long integers T to compare to first.</param>
        public static bool SequenceEqual(this Span<long> first, Span<long> second)
        {
            return first.Length >= 256
                ? MemoryEqual(first, second)
                : SequenceEqual<long>(first, second);
        }

        /// <summary>
        /// Determines whether two read-only spans are equal (byte-wise) by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span of long integers to compare to second.</param>
        /// <param name="second">A span of long integers T to compare to first.</param>
        public static bool SequenceEqual(this ReadOnlySpan<long> first, ReadOnlySpan<long> second)
        {
            return first.Length >= 256
                ? MemoryEqual(first, second)
                : SequenceEqual<long>(first, second);
        }

        /// <summary>
        /// Determines whether two spans are structurally (byte-wise) equal by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span, of type T to compare to second.</param>
        /// <param name="second">A span, of type U to compare to first.</param>
        public static bool BlockEquals<[Primitive]T, [Primitive]U>(this Span<T> first, Span<U> second)
            where T : struct
            where U : struct
        {
            var bytesCount = (ulong)first.Length * (ulong)Unsafe.SizeOf<T>();
            if (bytesCount != (ulong)second.Length * (ulong)Unsafe.SizeOf<U>())
            {
                return false;
            }

            // perf: it is cheaper to compare 'n' long elements than 'n*8' bytes (in a loop)
            if ((bytesCount & 0x00000007) == 0) // fast % sizeof(long)
            {
                return SequenceEqual(Cast<T, long>(first), Cast<U, long>(second));
            }
            if ((bytesCount & 0x00000003) == 0) // fast % sizeof(int)
            {
                return SequenceEqual(Cast<T, int>(first), Cast<U, int>(second));
            }
            if ((bytesCount & 0x00000001) == 0) // fast % sizeof(short)
            {
                return SequenceEqual(Cast<T, short>(first), Cast<U, short>(second));
            }

            return SequenceEqual(Cast<T, byte>(first), Cast<U, byte>(second));
        }

        /// <summary>
        /// Determines whether two spans are structurally (byte-wise) equal by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span, of type T to compare to second.</param>
        /// <param name="second">A span, of type U to compare to first.</param>
        public static bool BlockEquals<[Primitive]T, [Primitive]U>(this ReadOnlySpan<T> first, ReadOnlySpan<U> second)
            where T : struct
            where U : struct
        {
            var bytesCount = (ulong)first.Length * (ulong)Unsafe.SizeOf<T>();
            if (bytesCount != (ulong)second.Length * (ulong)Unsafe.SizeOf<U>())
            {
                return false;
            }

            // perf: it is cheaper to compare 'n' long elements than 'n*8' bytes (in a loop)
            if ((bytesCount & 0x00000007) == 0) // fast % sizeof(long)
            {
                return SequenceEqual(Cast<T, long>(first), Cast<U, long>(second));
            }
            if ((bytesCount & 0x00000003) == 0) // fast % sizeof(int)
            {
                return SequenceEqual(Cast<T, int>(first), Cast<U, int>(second));
            }
            if ((bytesCount & 0x00000001) == 0) // fast % sizeof(short)
            {
                return SequenceEqual(Cast<T, short>(first), Cast<U, short>(second));
            }

            return SequenceEqual(Cast<T, byte>(first), Cast<U, byte>(second));
        }
    }
}
