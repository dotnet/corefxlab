// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Collections.Sequences;
using System.Runtime.CompilerServices;

namespace System.Buffers
{
    public ref partial struct BufferReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryParse(out bool value)
        {
            var unread = UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    _currentSegmentConsumedBytes += consumed;
                    return true;
                }
            }
            return TryParseStraddling(out value, out consumed);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryParse(out int value)
        {
            var unread = UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    _currentSegmentConsumedBytes += consumed;
                    return true;
                }
            }
            return TryParseStraddling(out value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryParse(out ulong value)
        {
            var unread = UnreadSegment;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    _currentSegmentConsumedBytes += consumed;
                    return true;
                }
            }
            return TryParseStraddling(out value, out consumed);
        }

        #region Straddling
        private bool TryParseStraddling(out bool value, out int consumed)
        {
            Span<byte> tempSpan = stackalloc byte[5];
            var copied = CopyUnread(this, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                Skip(consumed);
                return true;
            }

            return false;
        }

        private bool TryParseStraddling(out ulong value, out int consumed)
        {
            Span<byte> tempSpan = stackalloc byte[30];
            var copied = CopyUnread(this, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                Skip(consumed);
                return true;
            }
            return false;
        }

        private bool TryParseStraddling(out int value)
        {
            Span<byte> tempSpan = stackalloc byte[15];
            var copied = CopyUnread(this, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out int consumed))
            {
                Skip(consumed);
                return true;
            }
            return false;
        }
        #endregion
    }
}
