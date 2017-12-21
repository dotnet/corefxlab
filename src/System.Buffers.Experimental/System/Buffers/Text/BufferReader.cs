// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Binary;
using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers.Text
{
    /// <summary>
    /// Used to read text from byte buffers
    /// </summary>
    public ref struct BufferReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        readonly TSequence _bytes;
        Collections.Sequences.Position _currentSegmentPosition;
        Collections.Sequences.Position _nextSegmentPosition;
        ReadOnlySpan<byte> _currentSpan;
        int _currentSpanIndex;

        // TODO: should there be a ctor that takes sequence + position? 
        // TODO: should there be a type that is sequence + position?
        public BufferReader(TSequence bytes)
        {
            _bytes = bytes;
            _nextSegmentPosition = bytes.First;
            _currentSegmentPosition = _nextSegmentPosition;
            if(_bytes.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
            {
                _currentSpan = memory.Span;
            }
            else
            {
                _currentSpan = ReadOnlySpan<byte>.Empty;
            }
            _currentSpanIndex = 0;
        }

        public BufferReader(ReadOnlyMemory<byte> bytes)
        {
            _bytes = default;
            _nextSegmentPosition = default;
            _currentSegmentPosition = 0;
            _currentSpan = bytes.Span;
            _currentSpanIndex = 0;
        }

        public BufferReader(ReadOnlySpan<byte> bytes)
        {
            _bytes = default;
            _nextSegmentPosition = default;
            _currentSegmentPosition = 0;
            _currentSpan = bytes;
            _currentSpanIndex = 0;
        }

        public bool IsEmpty {
            get {
                if (_currentSpan.Length - _currentSpanIndex > 0)
                {
                    return false;
                }

                Collections.Sequences.Position position = _nextSegmentPosition;
                while (_bytes.TryGet(ref position, out ReadOnlyMemory<byte> next))
                {
                    if (!next.IsEmpty) return false;
                }
                return true;
            }
        }

        public bool TryPeek(out byte value)
        {
            if (_currentSpan.Length > _currentSpanIndex)
            {
                value = _currentSpan[_currentSpanIndex];
                return true;
            }

            // TODO: this should try to advance
            value = default;
            return false;
        }

        public bool TryReadBytes(out ReadOnlyBytes bytes, byte delimiter)
        {
            var range = ReadRange(delimiter);
            if(range.Start == default || range.End == default)
            {
                bytes = default;
                return false;
            }

            var (startObj, startIndex) = range.Start.Get<object>();
            var (endObj, endIndex) = range.End.Get<object>();
            // TODO: this is a hack. Once we move this to System.Memory, we should remove
            if (startObj == null || endObj == null)
            {
                bytes = default;
                return false;
            }

            bytes = new ReadOnlyBytes(range.Start, range.End);
            return true;
        }
        public bool TryReadBytes(out ReadOnlyBytes bytes, ReadOnlySpan<byte> delimiter)
        {
            var range = ReadRange(delimiter);
            if (range.Start == default || range.End == default)
            {
                bytes = default;
                return false;
            }
            bytes = new ReadOnlyBytes(range.Start, range.End);
            return true;
        }

        PositionRange ReadRange(byte delimiter)
            => new PositionRange(Position, AdvanceToDelimiter(delimiter).GetValueOrDefault());

        PositionRange ReadRange(ReadOnlySpan<byte> delimiter)
        {
            var range = new PositionRange(Position, PositionOf(delimiter).GetValueOrDefault());
            if (range.End != default)
            {
                Advance(range.End);
                Advance(delimiter.Length);
            }
            return range;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(long count)
        {
            var unreadLength = _currentSpan.Length - _currentSpanIndex;
            if (count < unreadLength) {
                _currentSpanIndex += (int)count;
            }
            else {
                AdvanceNextSegment(count, unreadLength);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            var unreadLength = _currentSpan.Length - _currentSpanIndex;
            if (count < unreadLength)
            {
                _currentSpanIndex += count;
            }
            else
            {
                AdvanceNextSegment(count, unreadLength);
            }
        }

        private void AdvanceNextSegment(long count, int currentSegmentUnread)
        {
            if (!_bytes.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
            {
                if (count > currentSegmentUnread) throw new ArgumentOutOfRangeException(nameof(count));
                else
                {
                    Debug.Assert(count == currentSegmentUnread);
                    _currentSpan = Span<byte>.Empty;
                    _currentSpanIndex = 0;
                    return;
                }
            }
            _currentSpan = memory.Span;
            _currentSpanIndex = 0;
            Advance(count - currentSegmentUnread);
        }

        public void Advance(Collections.Sequences.Position position)
        {
            _currentSegmentPosition = position;
            _nextSegmentPosition = position;
            if (_bytes.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
            {
                _currentSpan = memory.Span;
            }
            else
            {
                _currentSpan = default;
            }
            _currentSpanIndex = 0;
        }

        #region Parsing Methods
        // TODO: how to we chose the lengths of the temp buffers?
        // TODO: these methods call the slow overloads of Parsers.Custom.TryParseXXX. Do we need fast path?
        // TODO: these methods hardcode the format. Do we need this to be something that can be specified?
        public bool TryParse(out bool value)
        {
            var unread = Unread;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    _currentSpanIndex += consumed;
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[5];
            var copied = CopyTo(this, tempSpan);

            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                Advance(consumed);
                return true;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryParse(out int value)
        {
            var unread = Unread;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed) {
                    _currentSpanIndex += consumed;
                    return true;
                }
            }
            return TryParseStraddling(out value);
        }

        private bool TryParseStraddling(out int value)
        {
            Span<byte> tempSpan = stackalloc byte[15];
            var copied = CopyTo(this, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out int consumed))
            {
                Advance(consumed);
                return true;
            }
            return false;
        }

        public bool TryParse(out ulong value)
        {
            var unread = Unread;
            if (Utf8Parser.TryParse(unread, out value, out int consumed))
            {
                if (unread.Length > consumed)
                {
                    _currentSpanIndex += consumed;
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[30];
            var copied = CopyTo(this, tempSpan);
            if (Utf8Parser.TryParse(tempSpan.Slice(0, copied), out value, out consumed))
            {
                Advance(consumed);
                return true;
            }
            return false;
        }
        #endregion

        #region Binary Read APIs
        public bool TryRead(out int value, bool littleEndian = false)
        {
            var unread = Unread;
            if (littleEndian) {
                if (BinaryPrimitives.TryReadInt32LittleEndian(unread, out value)) {
                    Advance(sizeof(int));
                    return true;
                }
            }
            else if (BinaryPrimitives.TryReadInt32BigEndian(unread, out value)) {
                Advance(sizeof(int));
                return true;
            }           

            Span<byte> span = stackalloc byte[4];
            var copied = CopyTo(this, span);
            if (copied < 4) {
                value = default;
                return false;
            }

            if (littleEndian) {
                value = BinaryPrimitives.ReadInt32LittleEndian(span);
            }
            else {
                value = BinaryPrimitives.ReadInt32BigEndian(span);
            }
            Advance(sizeof(int));
            return true;
        }
        #endregion

        ReadOnlySpan<byte> Unread => _currentSpan.Slice(_currentSpanIndex);

        Collections.Sequences.Position Position =>_currentSegmentPosition + _currentSpanIndex;      

        Collections.Sequences.Position? AdvanceToDelimiter(byte value)
        {
            var unread = Unread;
            var index = unread.IndexOf(value);
            if (index != -1)
            {
                _currentSpanIndex += index;
                var result = _currentSegmentPosition + _currentSpanIndex;
                _currentSpanIndex++; // skip delimiter
                return result;
            }

            var nextPosition = _nextSegmentPosition;
            var currentPosition = _currentSegmentPosition;
            var previousPosition = _nextSegmentPosition;
            while (_bytes.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
            {
                var span = memory.Span;
                index = span.IndexOf(value);
                if (index != -1)
                {
                    _currentSegmentPosition = previousPosition;
                    _currentSpan = span;
                    _currentSpanIndex = index + 1;
                    return _currentSegmentPosition + index;
                }
                previousPosition = _nextSegmentPosition;
            }

            _nextSegmentPosition = nextPosition;
            _currentSegmentPosition = currentPosition;
            return null;
        }

        Collections.Sequences.Position? PositionOf(ReadOnlySpan<byte> value)
        {
            var unread = Unread;
            var index = unread.IndexOf(value);
            if (index != -1)
            {
                return _currentSegmentPosition + (index + _currentSpanIndex);
            }

            index = unread.IndexOf(value[0]);
            if(index != -1 && unread.Length - index < value.Length)
            {
                Span<byte> temp = stackalloc byte[value.Length];
                int copied = Sequence.Copy(_bytes, _currentSegmentPosition + (_currentSpanIndex + index), temp);
                if (copied < value.Length) return null;

                if (temp.SequenceEqual(value)) return _currentSegmentPosition + (_currentSpanIndex + index);
                else throw new NotImplementedException(); // need to check farther in this span
            }

            if (unread.Length == 0) return null;

            throw new NotImplementedException();
        }

        static int CopyTo(BufferReader<TSequence> bytes, Span<byte> buffer)
        {
            var first = bytes.Unread;
            if (first.Length > buffer.Length)
            {
                first.Slice(0, buffer.Length).CopyTo(buffer);
                return buffer.Length;
            }
            else if (first.Length == buffer.Length)
            {
                first.CopyTo(buffer);
                return buffer.Length;
            }
            else
            {
                first.CopyTo(buffer);
                return first.Length + Sequence.Copy(bytes._bytes, bytes._nextSegmentPosition, buffer.Slice(first.Length));
            }
        }
    }

    public static class BytesReader
    {
        public static BufferReader<T> Create<T>(T sequence) where T : ISequence<ReadOnlyMemory<byte>>
        {
            return new BufferReader<T>(sequence);
        }
    }

    struct PositionRange
    {
        public readonly Collections.Sequences.Position Start;
        public readonly Collections.Sequences.Position End;

        public PositionRange(Collections.Sequences.Position start, Collections.Sequences.Position end)
        {
            Start = start;
            End = end;
        }
    }
}
