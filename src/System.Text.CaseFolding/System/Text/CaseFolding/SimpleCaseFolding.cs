// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Text.CaseFolding
{
    /// <summary>
    /// </summary>
    public static partial class SimpleCaseFolding

    {
        private static ref ushort s_MapLevel1 => ref MapLevel1[0];
        private static ref char s_refMapData => ref MapData[0];
        private static ref ushort s_refMapSurrogateLevel1 => ref MapSurrogateLevel1[0];
        private static ref (char highSurrogate, char lowSurrogate) s_refMapSurrogateData => ref MapSurrogateData[0];

        /// <summary>
        /// </summary>
        public static char SimpleCaseFold(char c)
        {
            if (c <= 0x5ff)
            {
                return MapBelow5FF[c];
            }

            // var v = L1[c >> 8];
            // var ch = L3[v + (c & 0xFF)];
            // Still slow due to border checks.
            var v = Unsafe.Add(ref s_MapLevel1, c >> 8);
            var ch = Unsafe.Add(ref s_refMapData, v + (c & 0xFF));

            return ch == 0 ? c : ch;
        }

        // Mapping for chars > 0x5ff slowly due to 2-level mapping.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SimpleCaseFoldCompareAbove05ff(char c1, char c2, ref ushort refMapLevel1, ref char refMapData)
        {
            var v1 =  Unsafe.Add(ref refMapLevel1, c1 >> 8);
            var ch1 = Unsafe.Add(ref refMapData, v1 + (c1 & 0xFF));
            if (ch1 == 0)
            {
                ch1 = c1;
            }

            var v2 =  Unsafe.Add(ref refMapLevel1, c2 >> 8);
            var ch2 = Unsafe.Add(ref refMapData, v2 + (c2 & 0xFF));
            if (ch2 == 0)
            {
                ch2 = c2;
            }

            return ch1 - ch2;
        }

        // Mapping for surrogate chars slowly due to 2-level mapping.
        // For comparison we can ignore UNICODE_PLANE01_START
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int SimpleCaseFoldCompareSurrogates(int c1, int c2, ref ushort refMapLevel1, ref (char highSurrogate, char lowSurrogate) refMapData)
        {
            if (c1 <= 0xFFFF)
            {
                var v1 = Unsafe.Add(ref refMapLevel1, c1 >> 8);
                var ch1 = Unsafe.Add(ref refMapData, v1 + (c1 & 0xFF));
                if (ch1 != (0, 0))
                {
                    c1 = ((ch1.highSurrogate - HIGH_SURROGATE_START) * 0x400) + (ch1.lowSurrogate - LOW_SURROGATE_START);
                }
            }

            if (c2 <= 0xFFFF)
            {
                var v1 = Unsafe.Add(ref refMapLevel1, c2 >> 8);
                var ch2 = Unsafe.Add(ref refMapData, v1 + (c2 & 0xFF));
                if (ch2 != (0, 0))
                {
                    c2 = ((ch2.highSurrogate - HIGH_SURROGATE_START) * 0x400) + (ch2.lowSurrogate - LOW_SURROGATE_START);
                }
            }

            return c1 - c2;
        }

        /// <summary>
        /// Compare strings using Unicode Simple Case Folding.
        /// </summary>
        public static int CompareUsingSimpleCaseFolding(this string strA, string strB)
        {
            if (object.ReferenceEquals(strA, strB))
            {
                return 0;
            }

            if (strA == null)
            {
                return -1;
            }

            if (strB == null)
            {
                return 1;
            }

            var spanA = strA.AsSpan();
            ref char refA = ref MemoryMarshal.GetReference(spanA);
            var lengthA = spanA.Length;
            var spanB = strB.AsSpan();
            ref char refB = ref MemoryMarshal.GetReference(spanB);
            var lengthB = spanB.Length;

            return CompareUsingSimpleCaseFolding(ref refA, lengthA, ref refB, lengthB);
        }

        /// <summary>
        /// Compare spans using Unicode Simple Case Folding.
        /// </summary>
        public static int CompareUsingSimpleCaseFolding(this ReadOnlySpan<char> spanA, ReadOnlySpan<char> spanB)
        {
            ref char refA = ref MemoryMarshal.GetReference(spanA);
            ref char refB = ref MemoryMarshal.GetReference(spanB);

            return CompareUsingSimpleCaseFolding(ref refA, spanA.Length, ref refB, spanB.Length);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CompareUsingSimpleCaseFolding(ref char refA, int lengthA, ref char refB, int lengthB)
        {
            var result = lengthA - lengthB;
            var length = Math.Min(lengthA, lengthB);

            // var l0AsSpan = MapBelow5FF.AsSpan();
            // ref char refMapBelow5FF = ref MemoryMarshal.GetReference(l0AsSpan);
            ref char refMapBelow5FF = ref MapBelow5FF[0];

            // For char below 0x5ff use fastest 1-level mapping.
            while (length != 0 && refA <= MaxChar && refB <= MaxChar)
            {
                var compare1 = Unsafe.Add(ref refMapBelow5FF, refA) - Unsafe.Add(ref refMapBelow5FF, refB);
                if (compare1 == 0)
                {
                    length--;
                    refA = ref Unsafe.Add(ref refA, 1);
                    refB = ref Unsafe.Add(ref refB, 1);
                }
                else
                {
                    return compare1;
                }
            }

            if (length == 0)
            {
                return result;
            }

            ref ushort refMapLevel1 = ref s_MapLevel1;
            ref char refMapData = ref s_refMapData;

            // We catch a char above 0x5ff.
            // Process it with more slow two-level mapping.
            while (length != 0 && !IsSurrogate(refA) && !IsSurrogate(refB))
            {
                var compare2 = SimpleCaseFoldCompareAbove05ff(refA, refB, ref refMapLevel1, ref refMapData);

                if (compare2 == 0)
                {
                    length--;
                    refA = ref Unsafe.Add(ref refA, 1);
                    refB = ref Unsafe.Add(ref refB, 1);
                }
                else
                {
                    return compare2;
                }
            }

            if (length == 0)
            {
                return result;
            }
/*
            return -1;
*/
            return CompareUsingSimpleCaseFolding(
                        ref refA,
                        ref refB,
                        result,
                        length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CompareUsingSimpleCaseFolding(
            ref char refA,
            ref char refB,
            int result,
            int length)
        {
            ref char refMapBelow5FF = ref MapBelow5FF[0];

            ref ushort refMapLevel1 = ref s_MapLevel1;
            ref char refMapData = ref s_refMapData;

            ref ushort refMapSurrogateLevel1 = ref s_refMapSurrogateLevel1;
            ref (char highSurrogate, char lowSurrogate) refMapSurrogateData = ref s_refMapSurrogateData;
            // ref int refMapSurrogateData = ref Unsafe.As<(char, char), int>(ref s_refMapSurrogateData);

            while (length != 0)
            {
                // We catch a high or low surrogate.
                // Process it and fallback to fastest options.
                var c1 = refA;
                var isHighSurrogateA = IsHighSurrogate(c1);
                var c2 = refB;
                var isHighSurrogateB = IsHighSurrogate(c2);

                if (isHighSurrogateA && isHighSurrogateB)
                {
                    // Both char is high surrogates.
                    // Get low surrogates.
                    length--;
                    if (length == 0)
                    {
                        // No low surrogate - throw?
                        throw new ArgumentNullException("Low surrogate is expected.");
                    }

                    refA = ref Unsafe.Add(ref refA, 1);
                    var c1Low = refA;
                    refB = ref Unsafe.Add(ref refB, 1);
                    var c2Low = refB;

                    if (!IsLowSurrogate(c1Low) || !IsLowSurrogate(c2Low))
                    {
                        // No low surrogate - throw?
                        throw new ArgumentNullException("Low surrogate is expected.");
                    }

                    // The index is Utf32 minus 0x10000 (UNICODE_PLANE01_START)
                    var index1 = ((c1 - HIGH_SURROGATE_START) * 0x400) + (c1Low - LOW_SURROGATE_START);
                    var index2 = ((c2 - HIGH_SURROGATE_START) * 0x400) + (c2Low - LOW_SURROGATE_START);

                    var compare4 = SimpleCaseFoldCompareSurrogates(index1, index2, ref refMapSurrogateLevel1, ref refMapSurrogateData);

                    if (compare4 != 0)
                    {
                        return compare4;
                    }

                    // Move to next char.
                    length--;
                    refA = ref Unsafe.Add(ref refA, 1);
                    refB = ref Unsafe.Add(ref refB, 1);
                }
                else
                {
                    if (isHighSurrogateA ^ isHighSurrogateB)
                    {
                        // Only one char is a surrogate.
                        return isHighSurrogateA ? 1 : -1;
                    }
                    else
                    {
                        // We expect a high surrogate but get a low surrogate - throw?
                        throw new ArgumentNullException("High surrogate is expected.");
                    }
                }

                // Both char is not surrogates. 'length--' was already done.
                while (length != 0 && refA <= MaxChar && refB <= MaxChar)
                {
                    var compare1 = Unsafe.Add(ref refMapBelow5FF, refA) - Unsafe.Add(ref refMapBelow5FF, refB);
                    if (compare1 == 0)
                    {
                        length--;
                        refA = ref Unsafe.Add(ref refA, 1);
                        refB = ref Unsafe.Add(ref refB, 1);
                    }
                    else
                    {
                        return compare1;
                    }
                }

                if (length == 0)
                {
                    return result;
                }

                while (length != 0 && !IsSurrogate(refA) && !IsSurrogate(refB))
                {
                    var compare2 = SimpleCaseFoldCompareAbove05ff(refA, refB, ref refMapLevel1, ref refMapData);

                    if (compare2 == 0)
                    {
                        length--;
                        refA = ref Unsafe.Add(ref refA, 1);
                        refB = ref Unsafe.Add(ref refB, 1);
                    }
                    else
                    {
                        return compare2;
                    }
                }

                if (length == 0)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Simple case folding of the string.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <returns>
        /// Returns new allocated folded string.
        /// </returns>
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string SimpleCaseFold(this string source)
        {
            if (source == null || source.Length == 0)
            {
                return source;
            }

            return string.Create(
                source.Length,
                source,
                (chars, sourceString) =>
                {
                    SimpleCaseFold(sourceString, chars);
                });
        }

        /// <summary>
        /// Simple case folding of the Span&lt;char&gt; on place.
        /// </summary>
        /// <param name="source">Source and target span.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SimpleCaseFold(this Span<char> source)
        {
            SimpleCaseFold(source, source);
        }

        /// <summary>
        /// Simple case folding of the ReadOnlySpan&lt;char&gt;.
        /// </summary>
        /// <param name="source">Source span.</param>
        /// <returns>
        /// Returns new allocated folded span.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<char> SimpleCaseFold(this ReadOnlySpan<char> source)
        {
            Span<char> destination = new char[source.Length];

            SimpleCaseFold(source, destination);

            return destination;
        }

        /// <summary>
        /// Simple case folding of the ReadOnlySpan&lt;char&gt;.
        /// </summary>
        /// <param name="source">Source span.</param>
        /// <param name="destination">Destination span.</param>
        public static void SimpleCaseFold(ReadOnlySpan<char> source, Span<char> destination)
        {
            if (source.Length > destination.Length)
            {
                throw new ArgumentNullException(nameof(destination)); // throw?
            }

            // Diagnostics.Assert(destination.Length >= source.Length, "Destination span length must be equal or greater then source span length.");
            ref char dst = ref MemoryMarshal.GetReference(destination);
            ref char src = ref MemoryMarshal.GetReference(source);

            var length = source.Length;

            // var l0AsSpan = MapBelow5FF.AsSpan();
            // ref char refMapBelow5FF = ref MemoryMarshal.GetReference(l0AsSpan);
            ref char refMapBelow5FF = ref MapBelow5FF[0];

            // For char below 0x5ff use fastest 1-level mapping.
            while (length != 0 && src <= MaxChar)
            {
                dst = Unsafe.Add(ref refMapBelow5FF, src);
                src = ref Unsafe.Add(ref src, 1);
                dst = ref Unsafe.Add(ref dst, 1);
                length--;
            }

            if (length == 0)
            {
                return;
            }

            ref ushort refMapLevel1 = ref s_MapLevel1;
            ref char refMapData = ref s_refMapData;

            // We catch a char above 0x5ff.
            // Process it with more slow two-level mapping.
            while (length != 0 && !IsSurrogate(src))
            {
                var v1 =  Unsafe.Add(ref refMapLevel1, src >> 8);
                var ch1 = Unsafe.Add(ref refMapData, v1 + (src & 0xFF));
                if (ch1 == 0)
                {
                    ch1 = src;
                }

                dst = ch1;
                src = ref Unsafe.Add(ref src, 1);
                dst = ref Unsafe.Add(ref dst, 1);
                length--;
            }

            if (length == 0)
            {
                return;
            }

            ref ushort refMapSurrogateLevel1 = ref s_refMapSurrogateLevel1;
            ref var refMapSurrogateData = ref s_refMapSurrogateData;

            while (length != 0)
            {
                // We catch a high or low surrogate.
                // Process it and fallback to fastest options.
                var c1 = src;
                var isHighSurrogateA = IsHighSurrogate(c1);

                if (isHighSurrogateA)
                {
                    // Get low surrogates.
                    length--;
                    if (length == 0)
                    {
                        // No low surrogate - throw?
                        throw new ArgumentNullException("Low surrogate is expected.");
                    }

                    src = ref Unsafe.Add(ref src, 1);
                    var c1Low = src;

                    if (!IsLowSurrogate(c1Low))
                    {
                        // No low surrogate - throw?
                        throw new ArgumentNullException("Low surrogate is expected.");
                    }

                    // The index is Utf32 minus 0x10000 (UNICODE_PLANE01_START)
                    var index1 = ((c1 - HIGH_SURROGATE_START) * 0x400) + (c1Low - LOW_SURROGATE_START);

                    if (index1 <= 0xFFFF)
                    {
                        var v1 = Unsafe.Add(ref refMapSurrogateLevel1, index1 >> 8);
                        var ch1 = Unsafe.Add(ref refMapSurrogateData, v1 + (index1 & 0xFF));
                        if (ch1 != (0, 0))
                        {
                            dst = ch1.highSurrogate;
                            dst = ref Unsafe.Add(ref dst, 1);
                            dst = ch1.lowSurrogate;
                        }
                        else
                        {
                            dst = c1;
                            dst = ref Unsafe.Add(ref dst, 1);
                            dst = c1Low;
                        }
                    }

                    // Move to next char.
                    length--;
                    src = ref Unsafe.Add(ref src, 1);
                    dst = ref Unsafe.Add(ref dst, 1);
                }
                else
                {
                    // We expect a high surrogate but get a low surrogate - throw?
                    throw new ArgumentNullException("High surrogate is expected.");
                }

                // For char below 0x5ff use fastest 1-level mapping.
                while (length != 0 && src <= MaxChar)
                {
                    dst = Unsafe.Add(ref refMapBelow5FF, src);
                    src = ref Unsafe.Add(ref src, 1);
                    dst = ref Unsafe.Add(ref dst, 1);
                    length--;
                }

                if (length == 0)
                {
                    return;
                }

                // We catch a char above 0x5ff.
                // Process it with more slow two-level mapping.
                while (length != 0 && !IsSurrogate(src))
                {
                    var v1 =  Unsafe.Add(ref refMapLevel1, src >> 8);
                    var ch1 = Unsafe.Add(ref refMapData, v1 + (src & 0xFF));
                    if (ch1 == 0)
                    {
                        ch1 = src;
                    }

                    dst = ch1;
                    src = ref Unsafe.Add(ref src, 1);
                    dst = ref Unsafe.Add(ref dst, 1);
                    length--;
                }

                if (length == 0)
                {
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsAscii(char c)
        {
            return c < 0x80;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsHighSurrogate(char c)
        {
            return (uint)(c - HIGH_SURROGATE_START) <= (uint)(HIGH_SURROGATE_END - HIGH_SURROGATE_START);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsLowSurrogate(char c)
        {
            return (uint)(c - LOW_SURROGATE_START) <= (uint)(LOW_SURROGATE_END - LOW_SURROGATE_START);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsSurrogate(char c)
        {
            return (uint)(c - HIGH_SURROGATE_START) <= (uint)(LOW_SURROGATE_END - HIGH_SURROGATE_START);
        }

        /// <summary>
        /// Search the char position in the string with simple case folding.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="ch">Char to search.</param>
        /// <returns>
        /// Returns an index the char in the string or -1 if not found.
        /// </returns>
        public static int IndexOfFolded(this string source, char ch)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return IndexOfFolded(source.AsSpan(), ch);
        }

        /// <summary>
        /// Search the char position in the ReadOnlySpan&lt;char&gt; with simple case folding.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="ch">Char to search.</param>
        /// <returns>
        /// Returns an index the char in the ReadOnlySpan&lt;char&gt; or -1 if not found.
        /// </returns>
        public static int IndexOfFolded(this ReadOnlySpan<char> source, char ch)
        {
            var foldedChar = SimpleCaseFold(ch);

            for (int i = 0; i < source.Length; i++)
            {
                if (SimpleCaseFold(source[i]) == foldedChar)
                {
                    return i;
                }
            }

            return -1;
        }

        internal const char MaxChar = (char)0x5ff;
        internal const char HIGH_SURROGATE_START = '\ud800';
        internal const char HIGH_SURROGATE_END = '\udbff';
        internal const char LOW_SURROGATE_START = '\udc00';
        internal const char LOW_SURROGATE_END = '\udfff';
        internal const int HIGH_SURROGATE_RANGE = 0x3FF;
    }
}
