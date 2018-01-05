// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public static partial class BufferReaderExtensions
    {
        //public Position? PositionOf(byte value)
        //{
        //    var unread = UnreadSegment;
        //    var index = unread.IndexOf(value);
        //    if (index != -1) return _currentPosition + (_index + index);

        //    var nextSegmentPosition = _nextPosition;
        //    var currentSegmentPosition = _nextPosition;
        //    while (_sequence.TryGet(ref nextSegmentPosition, out var memory, true))
        //    {
        //        var segment = memory.Span;
        //        index = segment.IndexOf(value);
        //        if (index != -1) return currentSegmentPosition + index;
        //        currentSegmentPosition = nextSegmentPosition;
        //    }
        //    return default;
        //}

        //public Position? PositionOf(ReadOnlySpan<byte> value)
        //{
        //    var unread = UnreadSegment;
        //    var index = unread.IndexOf(value);
        //    if (index != -1) return _currentPosition + (_index + index);
        //    if (value.Length == 0) return default;

        //    Span<byte> temp = stackalloc byte[(value.Length - 1) * 2];
        //    var currentSegmentPosition = _currentPosition;
        //    var nextSegmentPosition = _nextPosition;
        //    var currentSegmentConsumedBytes = _index;

        //    while (true)
        //    {
        //        // Try Stitched Match
        //        int tempStartIndex = unread.Length - Math.Min(unread.Length, value.Length - 1);
        //        var candidatePosition = currentSegmentPosition + (currentSegmentConsumedBytes + tempStartIndex);
        //        int copied = CopyFromPosition(_sequence, candidatePosition, temp);
        //        if (copied < value.Length) return null;

        //        index = temp.Slice(0, copied).IndexOf(value);
        //        if (index < value.Length)
        //        {
        //            if (index != -1) return candidatePosition + index;
        //        }

        //        currentSegmentPosition = nextSegmentPosition;
        //        // Try Next Segment
        //        if (!_sequence.TryGet(ref nextSegmentPosition, out var memory, true))
        //        {
        //            return default;
        //        }
        //        currentSegmentConsumedBytes = 0;
        //        unread = memory.Span;

        //        index = unread.IndexOf(value);
        //        if (index != -1) return currentSegmentPosition + index;
        //    }
        //}
    }
}
