// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Reader
{
    public ref partial struct BufferReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool TryParse(out bool value)
        {
            var unread = UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed) && consumed < unread.Length)
            {
                Advance(consumed);
                return true;
            }

            return TryParseSlow(out value);
        }

        private unsafe bool TryParseSlow(out bool value)
        {
            const int MaxLength = 5;
            var unread = UnreadSegment;

            if (unread.Length > MaxLength)
            {
                // Fast path had enough space but couldn't find valid data
                value = default;
                return false;
            }

            byte* buffer = stackalloc byte[MaxLength];
            Span<byte> tempSpan = new Span<byte>(buffer, MaxLength);
            if (Utf8Parser.TryParse(PeekSlow(tempSpan), out value, out int consumed))
            {
                Advance(consumed);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool TryParse(out int value)
        {
            var unread = UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed) && consumed < unread.Length)
            {
                Advance(consumed);
                return true;
            }

            return TryParseSlow(out value);
        }

        private unsafe bool TryParseSlow(out int value)
        {
            const int MaxLength = 15;
            var unread = UnreadSegment;

            if (unread.Length > MaxLength)
            {
                // Fast path had enough space but couldn't find valid data
                value = default;
                return false;
            }

            byte* buffer = stackalloc byte[MaxLength];
            Span<byte> tempSpan = new Span<byte>(buffer, MaxLength);
            if (Utf8Parser.TryParse(PeekSlow(tempSpan), out value, out int consumed))
            {
                Advance(consumed);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe bool TryParse(out ulong value)
        {
            var unread = UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed) && consumed < unread.Length)
            {
                Advance(consumed);
                return true;
            }

            return TryParseSlow(out value);
        }

        private unsafe bool TryParseSlow(out ulong value)
        {
            const int MaxLength = 30;
            var unread = UnreadSegment;

            if (unread.Length > MaxLength)
            {
                // Fast path had enough space but couldn't find valid data
                value = default;
                return false;
            }

            byte* buffer = stackalloc byte[MaxLength];
            Span<byte> tempSpan = new Span<byte>(buffer, MaxLength);
            if (Utf8Parser.TryParse(PeekSlow(tempSpan), out value, out int consumed))
            {
                Advance(consumed);
                return true;
            }

            return false;
        }
    }
}
