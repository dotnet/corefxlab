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
        /// Creates a new slice over the portion of the target array segment.
        /// </summary>
        /// <param name="arraySegment">The target array segment.</param>
        /// </exception>
        public static Span<T> Slice<T>(this ArraySegment<T> arraySegment)
        {
            return new Span<T>(arraySegment.Array, arraySegment.Offset, arraySegment.Count);
        }

        /// <summary>
        /// Creates a new slice over the portion of the target string.
        /// </summary>
        /// <param name="str">The target string.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'str' parameter is null.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> Slice(this string str)
        {
            Contract.Requires(str != null);
            return new ReadOnlySpan<char>(
                str,
                UIntPtr.Zero,
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> Slice(this string str, int start)
        {
            Contract.Requires(str != null);
            Contract.RequiresInInclusiveRange(start, (uint)str.Length);

            return new ReadOnlySpan<char>(
                str,
                new UIntPtr((uint)start),
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<char> Slice(this string str, int start, int length)
        {
            Contract.Requires(str != null);
            Contract.RequiresInInclusiveRange(start, length, (uint)str.Length);
            return new ReadOnlySpan<char>(
                str,
                new UIntPtr((uint)start),
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
            int countOfU;

            /// This comparison is a jittime constant
            if (Unsafe.SizeOf<T>() > Unsafe.SizeOf<U>())
            {
                long count;
                checked
                {
                    count = ((uint) slice.Length*Unsafe.SizeOf<T>())/Unsafe.SizeOf<U>();
                }
                bool fits = count <= int.MaxValue;
                Contract.Requires(fits);
                countOfU = (int)count;
            }
            else
            {
                countOfU = slice.Length * Unsafe.SizeOf<T>() / Unsafe.SizeOf<U>();
            }
            
            object obj = slice.Object;
            UIntPtr offset = slice.Offset; 

            if (countOfU == 0)
            {
                obj = null;
                offset = (UIntPtr)0;
            }

            return new Span<U>(obj, offset, countOfU);
        }

        /// <summary>
        /// Casts a Slice of one primitive type (T) to another primitive type (U).
        /// These types may not contain managed objects, in order to preserve type
        /// safety.  This is checked statically by a Roslyn analyzer.
        /// </summary>
        /// <param name="slice">The source slice, of type T.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<U> Cast<[Primitive]T, [Primitive]U>(this ReadOnlySpan<T> slice)
            where T : struct
            where U : struct
        {
            int countOfU;

            /// This comparison is a jittime constant
            if (Unsafe.SizeOf<T>() > Unsafe.SizeOf<U>())
            {
                long count;
                checked
                {
                    count = ((uint)slice.Length * Unsafe.SizeOf<T>()) / Unsafe.SizeOf<U>();
                }
                bool fits = count <= int.MaxValue;
                Contract.Requires(fits);
                countOfU = (int)count;
            }
            else
            {
                countOfU = slice.Length * Unsafe.SizeOf<T>() / Unsafe.SizeOf<U>();
            }

            object obj = slice.Object;
            UIntPtr offset = slice.Offset;

            if (countOfU == 0)
            {
                obj = null;
                offset = (UIntPtr)0;
            }

            return new ReadOnlySpan<U>(obj, offset, countOfU);
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this Span<byte> slice)
            where T : struct
        {
            Contract.RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)slice.Length);
            return PtrUtils.Get<T>(slice.Object, slice.Offset, (UIntPtr)0);
        }

        /// <summary>
        /// Reads a structure of type T out of a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<[Primitive]T>(this ReadOnlySpan<byte> slice)
            where T : struct
        {
            Contract.RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)slice.Length);
            return PtrUtils.Get<T>(slice.Object, slice.Offset, (UIntPtr)0);
        }

        /// <summary>
        /// Writes a structure of type T into a slice of bytes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<[Primitive]T>(this Span<byte> slice, T value)
            where T : struct
        {
            Contract.RequiresInInclusiveRange(Unsafe.SizeOf<T>(), (uint)slice.Length);
            PtrUtils.Set(slice.Object, slice.Offset, (UIntPtr)0, value);
        }

        /// <summary>
        /// Determines whether two spans are equal by comparing the elements by using generic Equals method
        /// </summary>
        /// <param name="first">A span of type T to compare to second.</param>
        /// <param name="second">A span of type T to compare to first.</param>
        public static bool SequenceEqual<T>(this ReadOnlySpan<T> first, ReadOnlySpan<T> second)
            where T : struct, IEquatable<T>
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            var e1 = first.GetEnumerator();
            var e2 = second.GetEnumerator();

            var more = e1.MoveNext();
            if (e2.MoveNext() != more)
            {
                return false;
            }

            while (more)
            {
                if (!e1.Current.Equals(e2.Current))
                {
                    return false;
                }

                more = e1.MoveNext();
                if (e2.MoveNext() != more)
                {
                    return false;
                }
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
        /// Determines whether two spans are structurally (byte-wise) equal by comparing the elements by using memcmp
        /// </summary>
        /// <param name="first">A span, of type T to compare to second.</param>
        /// <param name="second">A span, of type U to compare to first.</param>
        public static bool BlockEquals<[Primitive]T, [Primitive]U>(this Span<T> first, Span<U> second)
            where T : struct
            where U : struct
        {
            var bytesCount = first.Length * Unsafe.SizeOf<T>();
            if (bytesCount != second.Length * Unsafe.SizeOf<U>())
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
            var bytesCount = first.Length * Unsafe.SizeOf<T>();
            if (bytesCount != second.Length * Unsafe.SizeOf<U>())
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

        /// <summary>Searches for the specified value and returns the index of the first occurrence within the entire <see cref="T:System.Span" />.</summary>
        /// <returns>The zero-based index of the first occurrence of <paramref name="value" /> within the entire <paramref name="slice" />, if found; otherwise, �1.</returns>
        /// <param name="slice">The <see cref="T:System.Span" /> to search.</param>
        /// <param name="value">The value to locate in <paramref name="slice" />.</param>
        /// <typeparam name="T">The type of the elements of the slice.</typeparam>
        public static int IndexOf<T>(this ReadOnlySpan<T> slice, T value)
           where T : struct, IEquatable<T>
        {
            var index = 0;
            foreach (var item in slice)
            {
                if (value.Equals(item))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }

        // Helper methods similar to System.ArrayExtension:

        // String helper methods, offering methods like String on Slice<char>:
        // TODO(joe): culture-sensitive comparisons.
        // TODO: should these move to satring related assembly

        public static bool Contains(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
        {
            if (value.Length > str.Length)
            {
                return false;
            }
            return str.IndexOf(value) >= 0;
        }

        public static bool EndsWith(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
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

        public static int IndexOf(this ReadOnlySpan<char> str, string value)
        {
            return IndexOf(str, value.Slice());
        }

        public static int IndexOf(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
        {
            throw new NotImplementedException();
        }

        public static int IndexOfAny(this ReadOnlySpan<char> str, params char[] values)
        {
            throw new NotImplementedException();
        }

        public static int IndexOfAny(this ReadOnlySpan<char> str, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static int IndexOfAny(this ReadOnlySpan<char> str, params ReadOnlySpan<char>[] values)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOf(this ReadOnlySpan<char> str, char value)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOf(this ReadOnlySpan<char> str, string value)
        {
            return LastIndexOf(str, value.Slice());
        }

        public static int LastIndexOf(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this ReadOnlySpan<char> str, params char[] values)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this ReadOnlySpan<char> str, params string[] values)
        {
            throw new NotImplementedException();
        }

        public static int LastIndexOfAny(this ReadOnlySpan<char> str, params ReadOnlySpan<char>[] values)
        {
            throw new NotImplementedException();
        }

        public static SplitEnumerator Split(this ReadOnlySpan<char> str, params char[] separator)
        {
            throw new NotImplementedException();
        }

        public struct SplitEnumerator
        {
        }

        public static bool StartsWith(this ReadOnlySpan<char> str, ReadOnlySpan<char> value)
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

