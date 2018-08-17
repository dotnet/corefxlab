// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public static partial class ReaderExtensions
    {
        /// <summary>
        /// Try to parse a bool out of the reader (as UTF-8).
        /// </summary>
        /// <param name="value">The parsed bool value or default if failed.</param>
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out bool value)
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;

            // For other types (int, etc) we won't know if we've consumed all of the type
            // ("235612" can be split over segments, for example). For bool, Utf8Parser
            // doesn't care what follows "True" or "False" and neither should we.
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                reader.Advance(consumed);
                return true;
            }

            return TryParseSlow(ref reader, out value);
        }

        private unsafe static bool TryParseSlow(ref BufferReader<byte> reader, out bool value)
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
            if (Utf8Parser.TryParse(reader.PeekSlow(tempSpan), out value, out int consumed))
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
        /// <returns>True if successfully parsed.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool TryParse(ref this BufferReader<byte> reader, out int value)
        {
            ReadOnlySpan<byte> unread = reader.UnreadSpan;
            if (Utf8Parser.TryParse(unread, out value, out int consumed) && consumed < unread.Length)
            {
                reader.Advance(consumed);
                return true;
            }

            return TryParseSlow(ref reader, out value);
        }

        private static unsafe bool TryParseSlow(ref BufferReader<byte> reader, out int value)
        {
            const int MaxLength = 15;

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
