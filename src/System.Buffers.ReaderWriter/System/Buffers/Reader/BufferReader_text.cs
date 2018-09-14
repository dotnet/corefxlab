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

        /// <summary>
        /// Try to parse an int out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed int value or default if failed.</param>
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
                return false;
            }

            if (consumed < unread.Length)
            {
                // The parser found a digit it wouldn't consume so we have all valid data.
                reader.Advance(consumed);
                return true;
            }

            // If we ate all of our unread there may be more valid digits
            return TryParseSlow(ref reader, out value, standardFormat);
        }

        private static unsafe bool TryParseSlow(ref BufferReader<byte> reader, out int value, char standardFormat)
        {
            // Note that ints can have leading zeros ("-00000000000000127") which makes predicting the max
            // length difficult. Typically the int will be less than 15 characters ("-2,147,483,648") so we'll
            // work in those increments.
            int bufferSize = 15;

            // Try using the stack for scratch space.
            do
            {
                // stackalloc is separated as we can't represent that PeekSlow will not capture the span.
                byte* stackBuffer = stackalloc byte[bufferSize];
                Span<byte> buffer = new Span<byte>(stackBuffer, bufferSize);
                ReadOnlySpan<byte> peekSpan = reader.PeekSlow(buffer);

                if (!Utf8Parser.TryParse(peekSpan, out value, out int consumed, standardFormat))
                {
                    // Overflow
                    return false;
                }

                if (consumed < peekSpan.Length || peekSpan.Length < buffer.Length)
                {
                    // The parser found a digit it wouldn't consume or we hit the end of the reader
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

                if (!Utf8Parser.TryParse(peekSpan, out value, out int consumed, standardFormat))
                {
                    // Overflow
                    return false;
                }

                if (consumed < peekSpan.Length || peekSpan.Length < buffer.Length)
                {
                    // The parser found a digit it wouldn't consume or we hit the end of the reader
                    // (peekSpan was smaller than requested)so we have all valid data.
                    reader.Advance(consumed);
                    return true;
                }

                // Throw if doubling would overflow.
                bufferSize = checked(bufferSize * 2);
            } while (true);
        }

        /// <summary>
        /// Try to parse a long out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed long value or default if failed.</param>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out long value)
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;
            if (Utf8Parser.TryParse(unread, out value, out int consumed) && consumed < unread.Length)
            {
                reader.Advance(consumed);
                return true;
            }

            return TryParseSlow(ref reader, out value);
        }

        private static unsafe bool TryParseSlow(ref BufferReader<byte> reader, out long value)
        {
            const int MaxLength = 30;
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            if (unread.Length > MaxLength)
            {
                // Fast path had enough space but couldn't find valid data
                value = default;
                return false;
            }

            byte* buffer = stackalloc byte[MaxLength];
            Span<byte> tempSpan = new Span<byte>(buffer, MaxLength);
            if (Utf8Parser.TryParse(reader.PeekSlow(tempSpan), out value, out int consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return false;
        }
    }
}
