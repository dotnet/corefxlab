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
            ReadOnlySpan<byte> unread = UnreadSpan;

            // For other types (int, etc) we won't know if we've consumed all of the type
            // ("235612" can be split over segments, for example). For bool, Utf8Parser
            // doesn't care what follows "True" or "False" and neither should we.
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                Advance(consumed);
                return true;
            }

            return TryParseSlow(out value);
        }

        private unsafe bool TryParseSlow(out bool value)
        {
            const int MaxLength = 5;
            ReadOnlySpan<byte> unread = UnreadSpan;

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
            ReadOnlySpan<byte> unread = UnreadSpan;
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

            ReadOnlySpan<byte> unread = UnreadSpan;

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
            ReadOnlySpan<byte> unread = UnreadSpan;
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
            ReadOnlySpan<byte> unread = UnreadSpan;

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
