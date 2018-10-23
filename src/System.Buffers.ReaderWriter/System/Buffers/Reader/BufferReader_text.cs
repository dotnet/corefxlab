// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers
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
 
        /// <summary>
        /// Try to parse a bool out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed bool value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out bool, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out bool value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            // For other types (int, etc) we won't know if we've consumed all of the type
            // ("235612" can be split over segments, for example). For bool, Utf8Parser
            // doesn't care what follows "True" or "False" and neither should we.
            if (Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat))
            {
                reader.AdvanceCurrentSpan(bytesConsumed);
                return true;
            }

            return TryParseSlow(ref reader, out value, standardFormat);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static bool TryParseSlow(ref this BufferReader<byte> reader, out bool value, char standardFormat = '\0')
        {
            if (reader.CurrentSpan.Length - reader.CurrentSpanIndex >= MaxBoolLength)
            {
                // There was already enough available bytes for all valid bool possibilities
                // in the current span, no need to try again.
                value = default;
                return false;
            }

            bool TryParseBool(ref BufferReader<byte> r, out bool v, char f)
            {
                return TryParseFixed(ref r, out v, f, MaxBoolLength, Utf8Parser.TryParse);
            }

            return TryParseBool(ref reader, out value, standardFormat);
        }

        /// <summary>
        /// Parse the specified buffer size.
        /// </summary>
        private unsafe static bool TryParseFixed<T>(
            ref BufferReader<byte> reader,
            out T value,
            char standardFormat,
            int bufferSize,
            ParseDelegate<T> parseDelegate) where T : unmanaged
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            byte* buffer = stackalloc byte[bufferSize];
            Span<byte> tempSpan = new Span<byte>(buffer, bufferSize);
            if (parseDelegate(reader.PeekSlow(tempSpan), out value, out int bytesConsumed, standardFormat))
            {
                reader.Advance(bytesConsumed);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try parsing until we've grabbed all available valid bytes or hit <see cref="MaxParseBytes"/>.
        /// </summary>
        private static unsafe bool TryParseGreedy<T>(
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

                if (!parseDelegate(peekSpan, out value, out int bytesConsumed, standardFormat)
                    || bytesConsumed == MaxParseBytes)
                {
                    // Output value overflow or we consumed more bytes than we're willing to
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
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out byte, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out byte value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseByte(ref BufferReader<byte> r, out byte v, char f)
            {
                // The typical max size for a short will be less than 4 bytes ("255").
                return TryParseGreedy(ref r, out v, f, 4, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits.
            return TryParseByte(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out sbyte value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseSByte(ref BufferReader<byte> r, out sbyte v, char f)
            {
                // The typical max size for a short will be less than 5 bytes ("-128").
                return TryParseGreedy(ref r, out v, f, 5, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits.
            return TryParseSByte(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out short value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseShort(ref BufferReader<byte> r, out short v, char f)
            {
                // The typical max size for a short will be less than 8 bytes ("-32,768").
                return TryParseGreedy(ref r, out v, f, 8, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits.
            return TryParseShort(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out ushort value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseUShort(ref BufferReader<byte> r, out ushort v, char f)
            {
                // The typical max size for a short will be less than 7 bytes ("65,535").
                return TryParseGreedy(ref r, out v, f, 7, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits.
            return TryParseUShort(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out int value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseInt(ref BufferReader<byte> r, out int v, char f)
            {
                // The typical max size for an int will be less than 15 bytes ("-2,147,483,648").
                return TryParseGreedy(ref r, out v, f, 15, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits
            return TryParseInt(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out uint value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseUInt(ref BufferReader<byte> r, out uint v, char f)
            {
                // The typical max size for an int will be less than 14 bytes ("4,294,967,295").
                return TryParseGreedy(ref r, out v, f, 14, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits
            return TryParseUInt(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out long value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept

                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an integer, but not by themselves.
                if (unread.Length != 1)
                {
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseLong(ref BufferReader<byte> r, out long v, char f)
            {
                // The typical max size for a long will be less than 27 bytes ("-9,223,372,036,854,775,808").
                return TryParseGreedy(ref r, out v, f, 27, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits.
            return TryParseLong(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out ulong value,
            char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                // Failed to parse or we consumed more than we're willing to accept
                return false;
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseULong(ref BufferReader<byte> r, out ulong v, char f)
            {
                // The typical max size for a long will be less than 27 bytes ("18,446,744,073,709,551,615").
                return TryParseGreedy(ref r, out v, f, 27, Utf8Parser.TryParse);
            }

            // If we ate all of our unread there may be more valid digits.
            return TryParseULong(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out float value,
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

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseFloat(ref BufferReader<byte> r, out float v, char f)
            {
                // We have to pass MaxParseBytes for float as we have no idea where segments end
                // and the exponent separators would cause a failed parse ('E' and '+') if they
                // fail on it.
                return TryParseGreedy(ref r, out v, f, MaxParseBytes, Utf8Parser.TryParse);
            }

            return TryParseFloat(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out double value,
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

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseDouble(ref BufferReader<byte> r, out double v, char f)
            {
                // We have to pass MaxParseBytes for double as we have no idea where segments end
                // and the exponent separators would cause a failed parse ('E' and '+') if they
                // fail on it.
                return TryParseGreedy(ref r, out v, f, MaxParseBytes, Utf8Parser.TryParse);
            }

            return TryParseDouble(ref reader, out value, standardFormat);
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out decimal value,
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

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseDecimal(ref BufferReader<byte> r, out decimal v, char f)
            {
                // We need to pass MaxParseBytes to ensure we fail correctly (see above).
                return TryParseGreedy(ref r, out v, f, MaxParseBytes, Utf8Parser.TryParse);
            }

            return TryParseDecimal(ref reader, out value, standardFormat);
        }

        /// <summary>
        /// Try to parse a <see cref="Guid"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="Guid"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out Guid, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out Guid value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat))
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

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseGuid(ref BufferReader<byte> r, out Guid v, char f)
            {
                return TryParseFixed(ref r, out v, f, MaxGuidLength, Utf8Parser.TryParse);
            }

            return TryParseGuid(ref reader, out value, standardFormat);
        }

        /// <summary>
        /// Try to parse an <see cref="TimeSpan"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="TimeSpan"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out TimeSpan, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(
            ref this BufferReader<byte> reader,
            out TimeSpan value,
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

            if (!Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat)
                || bytesConsumed >= MaxParseBytes)
            {
                if (unread.Length >= MaxParseBytes)
                {
                    // We have enough space in the current span to definitively know that
                    // we've failed or we consumed more than we're willing to.
                    return false;
                }
            }
            else if (bytesConsumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all useful data.
                reader.Advance(bytesConsumed);
                return true;
            }

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseTimeSpan(ref BufferReader<byte> r, out TimeSpan v, char f)
            {
                // We need to pass MaxParseBytes to ensure we fail correctly (see above).
                return TryParseGreedy(ref r, out v, f, MaxParseBytes, Utf8Parser.TryParse);
            }

            return TryParseTimeSpan(ref reader, out value, standardFormat);
        }

        /// <summary>
        /// Try to parse a <see cref="DateTime"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="DateTime"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out DateTime, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out DateTime value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat))
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

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseDateTime(ref BufferReader<byte> r, out DateTime v, char f)
            {
                return TryParseFixed(ref r, out v, f, MaxGuidLength, Utf8Parser.TryParse);
            }

            return TryParseDateTime(ref reader, out value, standardFormat);
        }

        /// <summary>
        /// Try to parse a <see cref="DateTimeOffset"/> out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed <see cref="DateTimeOffset"/> value or default if failed.</param>
        /// <param name="standardFormat">Expected format.</param>
        /// <remarks>
        /// <see cref="Utf8Parser.TryParse(ReadOnlySpan{byte}, out DateTimeOffset, out int, char)"/> for supported formats.
        /// </remarks>
        /// <exception cref="FormatException">Thrown if the given format isn't supported.</exception>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryParse(ref this BufferReader<byte> reader, out DateTimeOffset value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (Utf8Parser.TryParse(unread, out value, out int bytesConsumed, standardFormat))
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

            // Avoid creating the delegate unless we really need it by putting in another method
            bool TryParseDateTimeOffset(ref BufferReader<byte> r, out DateTimeOffset v, char f)
            {
                return TryParseFixed(ref r, out v, f, MaxGuidLength, Utf8Parser.TryParse);
            }

            return TryParseDateTimeOffset(ref reader, out value, standardFormat);
        }
    }
}
