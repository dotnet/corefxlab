// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// A collection of convenient span helpers, exposed as extension methods.
    /// </summary>
    public static partial class SpanExtensions
    {
        // span creation helpers:

        /// <summary>
        /// Creates a new slice over the portion of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        public static Span<T> Slice<T>(this T[] array)
        {
            return new Span<T>(array);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target array beginning
        /// at 'start' index.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public static Span<T> Slice<T>(this T[] array, int start)
        {
            return new Span<T>(array, start);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target array beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <param name="length">The number of items in the new slice.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start or end index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public static Span<T> Slice<T>(this T[] array, int start, int length)
        {
            return new Span<T>(array, start, length);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target string.
        /// </summary>
        /// <param name="str">The target string.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'str' parameter is null.
        /// </exception>
        public static Span<char> Slice(this string str)
        {
            Contract.Requires(str != null);
            return new Span<char>(
                str,
                new UIntPtr((uint)SpanHelpers.OffsetToStringData),
                str.Length
            );
        }

        /// <summary>
        /// Creates a new slice over the portion of the target string beginning
        /// at 'start' index.
        /// </summary>
        /// <param name="str">The target string.</param>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'str' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public static Span<char> Slice(this string str, int start)
        {
            Contract.Requires(str != null);
            Contract.RequiresInInclusiveRange(start, str.Length);
            return new Span<char>(
                str,
                new UIntPtr((uint)(SpanHelpers.OffsetToStringData + (start * sizeof(char)))),
                str.Length - start
            );
        }

        /// <summary>
        /// Creates a new slice over the portion of the target string beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="str">The target string.</param>
        /// <param name="start">The index at which to begin the slice.</param>
        /// <param name="end">The index at which to end the slice (exclusive).</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'start' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start or end index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public static Span<char> Slice(this string str, int start, int length)
        {
            Contract.Requires(str != null);
            Contract.Requires(start + length <= str.Length);
            return new Span<char>(
                str,
                new UIntPtr((uint)(SpanHelpers.OffsetToStringData + (start * sizeof(char)))),
                length
            );
        }

        // Some handy byte manipulation helpers:

        /// <summary>
        /// Casts a Slice of one primitive type (T) to another primitive type (U).
        /// These types may not contain managed objects, in order to preserve type
        /// safety.  This is checked statically by a Roslyn analyzer.
        /// </summary>
        /// <param name="slice">The source slice, of type T.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<U> Cast<[Primitive]T, [Primitive]U>(this Span<T> slice)
            where T : struct
            where U : struct
        {
            int countOfU = slice.Length * PtrUtils.SizeOf<T>() / PtrUtils.SizeOf<U>();
            if (countOfU == 0)
            {
                return default(Span<U>);
            }
            return new Span<U>(slice.Object, slice.Offset, countOfU);
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        public static T Read<[Primitive]T>(this Span<byte> slice)
            where T : struct
        {
            Contract.Requires(slice.Length >= PtrUtils.SizeOf<T>());
            return slice.Cast<byte, T>()[0];
        }

        /// <summary>
        /// Writes a structure of type T into a slice of bytes.
        /// </summary>
        public static void Write<[Primitive]T>(this Span<byte> slice, T value)
            where T : struct
        {
            Contract.Requires(slice.Length >= PtrUtils.SizeOf<T>());
            var cast = slice.Cast<byte, T>();
            cast[0] = value;
        }

        /// <summary>
        /// Determines whether two spans are equal by comparing the elements by using generic Equals method
        /// </summary>
        /// <param name="first">A span of type T to compare to second.</param>
        /// <param name="second">A span of type T to compare to first.</param>
        public static bool SequenceEqual<T>(this Span<T> first, Span<T> second)
            where T : struct, IEquatable<T>
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            // we can not call memcmp here because structures might have nontrivial Equals implementation
            for (int i = 0; i < first.Length; i++)
            {
                if (!first.GetItemWithoutBoundariesCheck(i).Equals(second.GetItemWithoutBoundariesCheck(i)))
                {
                    return false;
                }
            }
            return true;
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
            var bytesCount = first.Length * PtrUtils.SizeOf<T>();
            if (bytesCount != second.Length * PtrUtils.SizeOf<U>())
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

        // Helper methods similar to System.ArrayExtension:

        // String helper methods, offering methods like String on Slice<char>:
        // TODO(joe): culture-sensitive comparisons.
        // TODO: should these move to satring related assembly

        public static bool Contains(this Span<char> str, Span<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }
            return str.IndexOf(value) >= 0;
        }

        public static bool EndsWith(this Span<char> str, Span<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }

            int j = str.Length - value.Length;
            foreach (var c in value)
            {
                if (str[j] != c)
                {
                    return false;
                }
                j++;
            }

            return true;
        }

        public static int IndexOf(this Span<char> str, char value)
        {
            throw new NotImplementedException();
        }

        public static int IndexOf(this Span<char> str, string value)
        {
            return IndexOf(str, value.Slice());
        }

        public static int IndexOf(this Span<char> str, Span<char> value)
        {
            throw new NotImplementedException();
        }

        public static int IndexOfAny(this Span<char> str, params char[] values)
        {
            throw new NotImplementedException();
        }

        public static int IndexOfAny(this Span<char> str, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static int IndexOfAny(this Span<char> str, params Span<char>[] values)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOf(this Span<char> str, char value)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOf(this Span<char> str, string value)
        {
            return LastIndexOf(str, value.Slice());
        }

        public static int LastIndexOf(this Span<char> str, Span<char> value)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this Span<char> str, params char[] values)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this Span<char> str, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this Span<char> str, params Span<char>[] values)
        {
            throw new NotImplementedException();
        }

        public static SplitEnumerator Split(this Span<char> str, params char[] separator)
        {
            throw new NotImplementedException();
        }

        public struct SplitEnumerator
        {
        }

        public static bool StartsWith(this Span<char> str, Span<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }

            for (int i = 0; i < value.Length; i++)
            {
                if (str[i] != value[i])
                {
                    return false;
                }
            }

            return true;
        }


        [CLSCompliant(false)]
        public unsafe static void Set(this Span<byte> bytes, byte* values, int length)
        {
            if (bytes.Length < length)
            {
                throw new ArgumentOutOfRangeException("values");
            }

            // TODO(joe): specialize to use a fast memcpy if T is pointerless.
            for (int i = 0; i < length; i++)
            {
                bytes[i] = values[i];
            }
        }
    }
}

