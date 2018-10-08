// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public static partial class ReaderExtensions
    {
        // .NET parsers will typically read as many leading zeroes as are given. For a discontiguous input,
        // (i.e. ReadOnlySequence) this behavior can be extremely expensive and/or lead to overflow and
        // out-of-memory situations. We'll cap the maximum number of bytes to consume.
        private const int MaxParseBytes = 128;

        private const int MaxBoolLength = 5;       // False
        private const int MaxGuidLength = 38;      // {9f21bcb9-f5c2-4b54-9b1c-9e0869bf9c16}
        private const int MaxDateTimeLength = 34;  // 9999-12-31T15:59:59.9999999-08:00

        private delegate bool ParseDelegate<T>(ReadOnlySpan<byte> source, out T value, out int consumed, char standardFormat);
        private static ParseDelegate<bool> s_boolParse;
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
        private static ParseDelegate<Guid> s_guidParse;
        private static ParseDelegate<TimeSpan> s_timeSpanParse;
        private static ParseDelegate<DateTime> s_dateTimeParse;
        private static ParseDelegate<DateTimeOffset> s_dateTimeOffsetParse;

        // Note: We mark the getters as NoInlining as we want to make sure the delegate creation is lazy.

        private static ParseDelegate<bool> BoolParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_boolParse == null)
                    s_boolParse = Utf8Parser.TryParse;
                return s_boolParse;
            }
        }

        private static ParseDelegate<Guid> GuidParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_guidParse == null)
                    s_guidParse = Utf8Parser.TryParse;
                return s_guidParse;
            }
        }

        private static ParseDelegate<TimeSpan> TimeSpanParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_timeSpanParse == null)
                    s_timeSpanParse = Utf8Parser.TryParse;
                return s_timeSpanParse;
            }
        }

        private static ParseDelegate<DateTime> DateTimeParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_dateTimeParse == null)
                    s_dateTimeParse = Utf8Parser.TryParse;
                return s_dateTimeParse;
            }
        }

        private static ParseDelegate<DateTimeOffset> DateTimeOffsetParser
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get
            {
                if (s_dateTimeOffsetParse == null)
                    s_dateTimeOffsetParse = Utf8Parser.TryParse;
                return s_dateTimeOffsetParse;
            }
        }

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

        private static ParseDelegate<sbyte> SByteParser
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

        private static ParseDelegate<ushort> UShortParser
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

        private static ParseDelegate<uint> UIntParser
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

        private static ParseDelegate<ulong> ULongParser
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
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out bool, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out bool value, out int bytesConsumed, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            // For other types (int, etc) we won't know if we've consumed all of the type
            // ("235612" can be split over segments, for example). For bool, Utf8Parser
            // doesn't care what follows "True" or "False" and neither should we.
            if (Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat))
            {
                reader.Advance(bytesConsumed);
                return true;
            }
            else if (unread.Length >= MaxBoolLength)
            {
                // There was already enough available bytes for all valid bool possibilities
                // in the current span, no need to try again.
                return false;
            }

            return TryParseFixed(ref reader, out value, out bytesConsumed, standardFormat, MaxBoolLength, BoolParser);
        }

        /// <summary>
        /// Parse the specified buffer size.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private unsafe static bool TryParseFixed<T>(
            ref BufferReader<byte> reader,
            out T value,
            out int bytesConsumed,
            char standardFormat,
            int bufferSize,
            ParseDelegate<T> parseDelegate) where T : unmanaged
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            byte* buffer = stackalloc byte[bufferSize];
            Span<byte> tempSpan = new Span<byte>(buffer, bufferSize);
            if (parseDelegate(reader.PeekSlow(tempSpan), out value, out bytesConsumed, standardFormat))
            {
                reader.Advance(bytesConsumed);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try parsing until we've grabbed all available valid bytes or hit <see cref="MaxParseBytes"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe bool TryParseGreedy<T>(
            ref BufferReader<byte> reader,
            out T value,
            out int bytesConsumed,
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

                if (!parseDelegate(peekSpan, out value, out bytesConsumed, standardFormat)
                    || bytesConsumed == MaxParseBytes)
                {
                    // Output value overflow or we consumed more bytes than we're willing to
                    bytesConsumed = 0;
                    return false;
                }

                if (bytesConsumed < peekSpan.Length || peekSpan.Length < bufferSize)
                {
                    // The parser found a value it wouldn't consume or we hit the end of the reader
                    // (peekSpan was smaller than requested) so we have all valid data.
                    reader.Advance(bytesConsumed);
                    return true;
                }

                if (bufferSize == MaxParseBytes)
                {
                    bytesConsumed = 0;
                    return false;
                }

                bufferSize = Math.Min(MaxParseBytes, bufferSize * 2);
            }
        }

        /// <summary>
        /// Try to parse a <see cref="byte"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="byte"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out byte, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out byte value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                bytesConsumed = 0;
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 4 bytes ("255").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 4, ByteParser);
        }

        /// <summary>
        /// Try to parse a <see cref="sbyte"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="sbyte"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out sbyte, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out sbyte value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 5 bytes ("-128").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 5, SByteParser);
        }

        /// <summary>
        /// Try to parse a <see cref="short"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="short"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out short, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out short value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 8 bytes ("-32,768").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 8, ShortParser);
        }

        /// <summary>
        /// Try to parse a <see cref="ushort"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="ushort"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out ushort, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out ushort value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                bytesConsumed = 0;
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 7 bytes ("65,535").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 7, UShortParser);
        }

        /// <summary>
        /// Try to parse an <see cref="int"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="int"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out int, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out int value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits
            // The typical max size for an int will be less than 15 bytes ("-2,147,483,648").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 15, IntParser);
        }

        /// <summary>
        /// Try to parse a <see cref="uint"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="uint"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out int, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out uint value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                bytesConsumed = 0;
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits
            // The typical max size for an int will be less than 14 bytes ("4,294,967,295").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 14, UIntParser);
        }

        /// <summary>
        /// Try to parse an <see cref="long"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="long"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out long, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out long value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a long will be less than 27 bytes ("-9,223,372,036,854,775,808").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 27, LongParser);
        }

        /// <summary>
        /// Try to parse a <see cref="ulong"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="ulong"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out ulong, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out ulong value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                bytesConsumed = 0;
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a long will be less than 27 bytes ("18,446,744,073,709,551,615").
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, 27, ULongParser);
        }

        /// <summary>
        /// Try to parse an <see cref="float"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="float"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out float, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out float value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            // We have to have MaxParseBytes available when failing to parse float (or
            // be at the end of the reader) to ensure we've not failed only because we
            // don't have enough of the data available (i.e. the value crosses spans).
            //
            // We could be partially into "-Infinity" and have to hit the slow path. Same
            // goes for "-3.402823E+38" - if our current buffer ends on "E" or "+" we can't
            // know that we've truly failed.

            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // We have to pass MaxParseBytes for float as we have no idea where segments end
            // and the exponent separators would cause a failed parse ('E' and '+') if they
            // fail on it.
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, MaxParseBytes, FloatParser);
        }

        /// <summary>
        /// Try to parse an <see cref="double"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="double"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out double, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out double value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            // We have to have MaxParseBytes available when failing to parse double (or
            // be at the end of the reader) to ensure we've not failed only because we
            // don't have enough of the data available (i.e. the value crosses spans).
            //
            // We could be partially into "-Infinity" and have to hit the slow path. Same
            // goes for "-3.402823E+38" - if our current buffer ends on "E" or "+" we can't
            // know that we've truly failed.

            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // We have to pass MaxParseBytes for float as we have no idea where segments end
            // and the exponent separators would cause a failed parse ('E' and '+') if they
            // fail on it.
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, MaxParseBytes, DoubleParser);
        }

        /// <summary>
        /// Try to parse an <see cref="decimal"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="decimal"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out decimal, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out decimal value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            // We have to have MaxParseBytes available when failing to parse decimal (or
            // be at the end of the reader) to ensure we've not failed only because we
            // don't have enough of the data available (i.e. the value crosses spans).
            //
            // We could be partially into "-Infinity" and have to hit the slow path. Same
            // goes for "-3.402823E+38" - if our current buffer ends on "E" or "+" we can't
            // know that we've truly failed.

            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // We need to pass MaxParseBytes to ensure we fail correctly (see above).
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, MaxParseBytes, DecimalParser);
        }

        /// <summary>
        /// Try to parse a <see cref="Guid"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="Guid"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out Guid, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out Guid value, out int bytesConsumed, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat))
            {
                reader.Advance(bytesConsumed);
                return true;
            }
            else if (unread.Length >= MaxGuidLength)
            {
                // There was already enough available bytes for all valid Guid possibilities
                // in the current span, no need to try again.
                return false;
            }

            return TryParseFixed(ref reader, out value, out bytesConsumed, standardFormat, MaxGuidLength, GuidParser);
        }

        /// <summary>
        /// Try to parse an <see cref="TimeSpan"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="TimeSpan"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out TimeSpan, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out TimeSpan value,
            out int bytesConsumed,
            char standardFormat = '\0')
        {
            // We have to have MaxParseBytes available when failing to parse TimeSpans (or
            // be at the end of the reader) to ensure we've not failed only because we
            // don't have enough of the data available (i.e. the value crosses spans).
            //
            // TimeSpan allows unlimited leading zeroes in all but the last segment.
            // TimeSpan's max value is canonically 10675199.02:48:05.4775807 and
            // 000010675199.002:0048:00005.4775807 is equivalent, for example.

            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    bytesConsumed = 0;
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // We need to pass MaxParseBytes to ensure we fail correctly (see above).
            return TryParseGreedy(ref reader, out value, out bytesConsumed, standardFormat, MaxParseBytes, TimeSpanParser);
        }

        /// <summary>
        /// Try to parse a <see cref="DateTime"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="DateTime"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out DateTime, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out DateTime value, out int bytesConsumed, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat))
            {
                reader.Advance(bytesConsumed);
                return true;
            }
            else if (unread.Length >= MaxDateTimeLength)
            {
                // There was already enough available bytes for all valid DateTime possibilities
                // in the current span, no need to try again.
                return false;
            }

            return TryParseFixed(ref reader, out value, out bytesConsumed, standardFormat, MaxDateTimeLength, DateTimeParser);
        }

        /// <summary>
        /// Try to parse a <see cref="DateTimeOffset"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="DateTimeOffset"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <param name="bytesConsumed">The number of bytes advanced by the reader.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out DateTimeOffset, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out DateTimeOffset value, out int bytesConsumed, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (Utf8Parser.TryParse(unread, out value, out bytesConsumed, standardFormat))
            {
                reader.Advance(bytesConsumed);
                return true;
            }
            else if (unread.Length >= MaxDateTimeLength)
            {
                // There was already enough available bytes for all valid DateTimeOffset possibilities
                // in the current span, no need to try again.
                return false;
            }

            return TryParseFixed(ref reader, out value, out bytesConsumed, standardFormat, MaxDateTimeLength, DateTimeOffsetParser);
        }
    }
}
