// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public static partial class ReaderExtensions
    {
        // .NET parsers will typically read as many leading zeroes as are given. For a discontiguous input,
        // (i.e. ReadOnlySequence) this behavior can be extremely expensive and/or lead to overflow and
        // out-of-memory situations. We'll cap the maximum number of bytes to consume.
        private const int MaxParseBytes = 128;
        private const int MaxBoolLength = 5;

        private delegate bool ParseDelegate<T>(ReadOnlySpan<byte> source, out T value, out int consumed, char standardFormat);
        private static ParseDelegate<byte> s_byteParse;
        private static ParseDelegate<sbyte> s_sbyteParse;
        private static ParseDelegate<short> s_shortParse;
        private static ParseDelegate<ushort> s_ushortParse;
        private static ParseDelegate<int> s_intParse;
        private static ParseDelegate<uint> s_uintParse;
        private static ParseDelegate<long> s_longParse;
        private static ParseDelegate<ulong> s_ulongParse;
        private static ParseDelegate<float> s_floatParse;
        private static ParseDelegate<double> s_doubleParse;
        private static ParseDelegate<decimal> s_decimalParse;

        private static ParseDelegate<byte> ByteParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_byteParse == null)
                    s_byteParse = Utf8Parser.TryParse;
                return s_byteParse;
            }
        }

        private static ParseDelegate<sbyte> SbyteParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_sbyteParse == null)
                    s_sbyteParse = Utf8Parser.TryParse;
                return s_sbyteParse;
            }
        }

        private static ParseDelegate<short> ShortParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_shortParse == null)
                    s_shortParse = Utf8Parser.TryParse;
                return s_shortParse;
            }
        }

        private static ParseDelegate<ushort> UshortParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_ushortParse == null)
                    s_ushortParse = Utf8Parser.TryParse;
                return s_ushortParse;
            }
        }

        private static ParseDelegate<int> IntParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_intParse == null)
                    s_intParse = Utf8Parser.TryParse;
                return s_intParse;
            }
        }

        private static ParseDelegate<uint> UintParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_uintParse == null)
                    s_uintParse = Utf8Parser.TryParse;
                return s_uintParse;
            }
        }

        private static ParseDelegate<long> LongParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_longParse == null)
                    s_longParse = Utf8Parser.TryParse;
                return s_longParse;
            }
        }

        private static ParseDelegate<ulong> UlongParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_ulongParse == null)
                    s_ulongParse = Utf8Parser.TryParse;
                return s_ulongParse;
            }
        }

        private static ParseDelegate<float> FloatParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_floatParse == null)
                    s_floatParse = Utf8Parser.TryParse;
                return s_floatParse;
            }
        }

        private static ParseDelegate<double> DoubleParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_doubleParse == null)
                    s_doubleParse = Utf8Parser.TryParse;
                return s_doubleParse;
            }
        }

        private static ParseDelegate<decimal> DecimalParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_decimalParse == null)
                    s_decimalParse = Utf8Parser.TryParse;
                return s_decimalParse;
            }
        }

        /// <summary>
        /// Try to parse a bool out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed bool value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out bool, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(ref this BufferReader<byte> reader, out bool value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            // For other types (int, etc) we won't know if we've consumed all of the type
            // ("235612" can be split over segments, for example). For bool, Utf8Parser
            // doesn't care what follows "True" or "False" and neither should we.
            if (Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat))
            {
                reader.Advance(consumed);
                return consumed;
            }
            else if (unread.Length >= MaxBoolLength)
            {
                return 0;
            }

            return TryParseSlow(ref reader, out value, standardFormat);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe static int TryParseSlow(ref BufferReader<byte> reader, out bool value, char standardFormat)
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            byte* buffer = stackalloc byte[MaxBoolLength];
            Span<byte> tempSpan = new Span<byte>(buffer, MaxBoolLength);
            if (Utf8Parser.TryParse(reader.PeekSlow(tempSpan), out value, out int consumed, standardFormat))
            {
                reader.Advance(consumed);
            }

            return consumed;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe int TryParseSlow<T>(
            ref BufferReader<byte> reader,
            out T value,
            char standardFormat,
            int bufferSize,
            ParseDelegate<T> parseDelegate) where T: unmanaged
        {
            // Note that in calling this with the expected max size of a normal instance of the given
            // type may do more work than needed for the degenerate case of zero prefaced numbers at
            // the end of the span (e.g. 00000000000000000000000000123). Doing the extra check slows
            // down the perf of "normal" numbers so we just let the "bad" numbers eat up more cycles.

            // stackalloc is separated as we can't represent in C# that Peek will not capture the span.
            byte* stackBytes = stackalloc byte[MaxParseBytes];
            Span<byte> stackBuffer = new Span<byte>(stackBytes, MaxParseBytes);

            while (true)
            {
                ReadOnlySpan<byte> peekSpan = reader.Peek(stackBuffer.Slice(0, bufferSize));

                if (!parseDelegate(peekSpan, out value, out int consumed, standardFormat)
                    || consumed == MaxParseBytes)
                {
                    // Output value overflow or we consumed more bytes than we're willing to
                    return 0;
                }

                if (consumed < peekSpan.Length || peekSpan.Length < bufferSize)
                {
                    // The parser found a value it wouldn't consume or we hit the end of the reader
                    // (peekSpan was smaller than requested) so we have all valid data.
                    reader.Advance(consumed);
                    return consumed;
                }

                if (bufferSize == MaxParseBytes)
                {
                    return 0;
                }

                bufferSize = Math.Min(MaxParseBytes, bufferSize * 2);
            }
        }

        /// <summary>
        /// Try to parse a <see cref="byte"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="byte"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out byte, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out byte value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return 0;
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 4 bytes ("255").
            return TryParseSlow(ref reader, out value, standardFormat, 4, ByteParser);
        }

        /// <summary>
        /// Try to parse a <see cref="sbyte"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="sbyte"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out sbyte, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out sbyte value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 5 bytes ("-128").
            return TryParseSlow(ref reader, out value, standardFormat, 5, SbyteParser);
        }

        /// <summary>
        /// Try to parse a <see cref="short"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="short"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out short, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out short value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 8 bytes ("-32,768").
            return TryParseSlow(ref reader, out value, standardFormat, 8, ShortParser);
        }

        /// <summary>
        /// Try to parse a <see cref="ushort"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="ushort"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out ushort, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out ushort value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return 0;
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 7 bytes ("65,535").
            return TryParseSlow(ref reader, out value, standardFormat, 7, UshortParser);
        }

        /// <summary>
        /// Try to parse an <see cref="int"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="int"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out int, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out int value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits
            // The typical max size for an int will be less than 15 bytes ("-2,147,483,648").
            return TryParseSlow(ref reader, out value, standardFormat, 15, IntParser);
        }

        /// <summary>
        /// Try to parse a <see cref="uint"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="uint"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out int, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out uint value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return 0;
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits
            // The typical max size for an int will be less than 14 bytes ("4,294,967,295").
            return TryParseSlow(ref reader, out value, standardFormat, 14, UintParser);
        }

        /// <summary>
        /// Try to parse an <see cref="long"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="long"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out long, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out long value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a long will be less than 27 bytes ("-9,223,372,036,854,775,808").
            return TryParseSlow(ref reader, out value, standardFormat, 27, LongParser);
        }

        /// <summary>
        /// Try to parse a <see cref="ulong"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="ulong"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out ulong, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out ulong value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return 0;
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a long will be less than 27 bytes ("18,446,744,073,709,551,615").
            return TryParseSlow(ref reader, out value, standardFormat, 27, UlongParser);
        }

        /// <summary>
        /// Try to parse an <see cref="float"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="float"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out float, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out float value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // Unfortunately the Utf8Parser won't tell us where we've failed. We could
                // be partially into "-Infinity" and have to hit the slow path. Same goes
                // for "-3.402823E+38" - if our current buffer ends on "E" or "+" we can't
                // know that we've truly failed. Rather than add complicated logic here and
                // bloating the call sites we'll do one simple check for max space and fall
                // through to the slow path if we don't have enough space in our current
                // buffer to make a definitive failure statement.
                if (unread.Length > MaxParseBytes)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // We have to pass MaxParseBytes for float as we have no idea where segments end
            // and the exponent separators would cause a failed parse ('E' and '+') if they
            // fail on it.
            return TryParseSlow(ref reader, out value, standardFormat, MaxParseBytes, FloatParser);
        }

        /// <summary>
        /// Try to parse an <see cref="double"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="double"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out double, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out double value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // Unfortunately the Utf8Parser won't tell us where we've failed. We could
                // be partially into "-Infinity" and have to hit the slow path. Same goes
                // for "-3.402823E+38" - if our current buffer ends on "E" or "+" we can't
                // know that we've truly failed. Rather than add complicated logic here and
                // bloating the call sites we'll do one simple check for max space and fall
                // through to the slow path if we don't have enough space in our current
                // buffer to make a definitive failure statement.
                if (unread.Length > MaxParseBytes)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // We have to pass MaxParseBytes for float as we have no idea where segments end
            // and the exponent separators would cause a failed parse ('E' and '+') if they
            // fail on it.
            return TryParseSlow(ref reader, out value, standardFormat, MaxParseBytes, DoubleParser);
        }


        /// <summary>
        /// Try to parse an <see cref="decimal"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="decimal"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out decimal, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>Number of consumed bytes if successful, otherwise 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TryParse(
            ref this BufferReader<byte> reader,
            out decimal value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat)
                || consumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // Unfortunately the Utf8Parser won't tell us where we've failed. We could
                // be partially into "-Infinity" and have to hit the slow path. Same goes
                // for "-3.402823E+38" - if our current buffer ends on "E" or "+" we can't
                // know that we've truly failed. Rather than add complicated logic here and
                // bloating the call sites we'll do one simple check for max space and fall
                // through to the slow path if we don't have enough space in our current
                // buffer to make a definitive failure statement.
                if (unread.Length > MaxParseBytes)
                {
                    return 0;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(consumed);
                return consumed;
            }

            // We have to pass MaxParseBytes for float as we have no idea where segments end
            // and the exponent separators would cause a failed parse ('E' and '+') if they
            // fail on it.
            return TryParseSlow(ref reader, out value, standardFormat, MaxParseBytes, DecimalParser);
        }
    }
}
