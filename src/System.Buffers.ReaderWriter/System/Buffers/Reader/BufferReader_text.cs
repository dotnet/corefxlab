// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public static partial class ReaderExtensions
    {
        // .NET parsers will typically read as many leading zeroes as are given. In our case this can be prohibitively expensive.
        private const int MaxParseBuffer = 1024;

        private delegate bool ParseDelegate<T>(ReadOnlySpan<byte> source, out T value, out int consumed, char standardFormat);
        private static ParseDelegate<short> s_shortParse;
        private static ParseDelegate<int> s_intParse;
        private static ParseDelegate<long> s_longParse;

        private static ParseDelegate<short> ShortParser
        {
            get
            {
                if (s_shortParse == null)
                    s_shortParse = Utf8Parser.TryParse;
                return s_shortParse;
            }
        }

        private static ParseDelegate<int> IntParser
        {
            get
            {
                if (s_intParse == null)
                    s_intParse = Utf8Parser.TryParse;
                return s_intParse;
            }
        }

        private static ParseDelegate<long> LongParser
        {
            get
            {
                if (s_longParse == null)
                    s_longParse = Utf8Parser.TryParse;
                return s_longParse;
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out bool value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            // For other types (int, etc) we won't know if we've consumed all of the type
            // ("235612" can be split over segments, for example). For bool, Utf8Parser
            // doesn't care what follows "True" or "False" and neither should we.
            if (Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat))
            {
                reader.Advance(consumed);
                return true;
            }

            return TryParseSlow(ref reader, out value, standardFormat);
        }

        private unsafe static bool TryParseSlow(ref BufferReader<byte> reader, out bool value, char standardFormat)
        {
            const int MaxLength = 5;
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (unread.Length > MaxLength)
            {
                // Fast path had enough space but couldn't find valid data
                value = default;
                return false;
            }

            byte* buffer = stackalloc byte[MaxLength];
            Span<byte> tempSpan = new Span<byte>(buffer, MaxLength);
            if (Utf8Parser.TryParse(reader.PeekSlow(tempSpan), out value, out int consumed, standardFormat))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }

        private static unsafe bool TryParseSlow<T>(
            ref BufferReader<byte> reader,
            out T value,
            char standardFormat,
            int bufferSize,
            ParseDelegate<T> parseDelegate) where T: unmanaged
        {
            // Try using the stack for scratch space.
            do
            {
                // stackalloc is separated as we can't represent that PeekSlow will not capture the span.
                byte* stackBuffer = stackalloc byte[bufferSize];
                Span<byte> buffer = new Span<byte>(stackBuffer, bufferSize);
                ReadOnlySpan<byte> peekSpan = reader.PeekSlow(buffer);

                if (!parseDelegate(peekSpan, out value, out int consumed, standardFormat))
                {
                    // Overflow
                    return false;
                }

                if (consumed < peekSpan.Length || peekSpan.Length < buffer.Length)
                {
                    // The parser found a value it wouldn't consume or we hit the end of the reader
                    // (peekSpan was smaller than requested)so we have all valid data.
                    reader.Advance(consumed);
                    return true;
                }


                bufferSize *= 2;
            } while (bufferSize <= 60);

            // Start heap allocating to get enough space.
            do
            {
                byte[] buffer = new byte[bufferSize];
                ReadOnlySpan<byte> peekSpan = reader.PeekSlow(buffer);

                if (!parseDelegate(peekSpan, out value, out int consumed, standardFormat))
                {
                    // Overflow
                    return false;
                }

                if (consumed < peekSpan.Length || peekSpan.Length < buffer.Length)
                {
                    // The parser found a value it wouldn't consume or we hit the end of the reader
                    // (peekSpan was smaller than requested)so we have all valid data.
                    reader.Advance(consumed);
                    return true;
                }

                // Throw if doubling would overflow.
                bufferSize = checked(bufferSize * 2);
            } while (true);
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
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out short value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;
            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat))
            {
                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an int, but not by themselves.
                if (unread.Length != 1 || unread[0] > '.')
                {
                    return false;
                }
            }

            if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all valid data.
                reader.Advance(consumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a short will be less than 8 bytes ("-32,768").
            return TryParseSlow(ref reader, out value, standardFormat, 8, ShortParser);
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
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out int value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;
            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat))
            {
                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an int, but not by themselves.
                if (unread.Length != 1 || unread[0] > '.')
                {
                    return false;
                }
            }
            else if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all valid data.
                reader.Advance(consumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits
            // The typical max size for an int will be less than 15 bytes ("-2,147,483,648").
            return TryParseSlow(ref reader, out value, standardFormat, 15, IntParser);
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
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out long value, char standardFormat = '\0')
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;
            if (!Utf8Parser.TryParse(unread, out value, out int consumed, standardFormat))
            {
                // If there is only one byte in our current span, it might be '+', '-', or '.'.
                // These are all valid starts to an int, but not by themselves.
                if (unread.Length != 1 || unread[0] > '.')
                {
                    return false;
                }
            }

            if (consumed < unread.Length)
            {
                // The parser found a value it wouldn't consume so we have all valid data.
                reader.Advance(consumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits.
            // The typical max size for a long will be less than 27 bytes ("-9,223,372,036,854,775,808").
            return TryParseSlow(ref reader, out value, standardFormat, 27, LongParser);
        }
    }
}
