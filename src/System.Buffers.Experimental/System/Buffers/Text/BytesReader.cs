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
    public ref struct BytesReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        readonly TSequence _bytes;
        Position _currentSegmentPosition;
        Position _nextSegmentPosition;
        ReadOnlySpan<byte> _currentSpan;
        int _currentSpanIndex;

        // TODO: should there be a ctor that takes sequence + position? 
        // TODO: should there be a type that is sequence + position?
        public BytesReader(TSequence bytes)
        {
            _bytes = bytes;
            _currentSegmentPosition = bytes.First;
            _nextSegmentPosition = _currentSegmentPosition;
            _bytes.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory);
            _currentSpan = memory.Span;
            _currentSpanIndex = 0;
        }

        public BytesReader(ReadOnlyMemory<byte> bytes)
        {
            _bytes = default;
            _nextSegmentPosition = default;
            _currentSegmentPosition = Position.Create(0);
            _currentSpan = bytes.Span;
            _currentSpanIndex = 0;
        }

        public BytesReader(ReadOnlySpan<byte> bytes)
        {
            _bytes = default;
            _nextSegmentPosition = default;
            _currentSegmentPosition = Position.Create(0);
            _currentSpan = bytes;
            _currentSpanIndex = 0;
        }

        public bool IsEmpty {
            get {
                if (_currentSpan.Length - _currentSpanIndex > 0)
                {
                    return false;
                }

                Position position = _nextSegmentPosition;
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

        public PositionRange ReadRange(byte delimiter)
        {
            var range = new PositionRange();
            range.From = Position;
            range.To = AdvanceToDelimiter(delimiter);
            return range;
        }

        public PositionRange ReadRange(ReadOnlySpan<byte> delimiter)
        {
            var range = new PositionRange();
            range.From = Position;
            range.To = PositionOf(delimiter);
            if (!range.To.IsEnd)
            {
                Advance(range.To);
                Advance(delimiter.Length);
            }
            return range;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(long count)
        {
            var unreadLength = _currentSpan.Length - _currentSpanIndex;
            if (count <= unreadLength)
            {
                _currentSpanIndex += (int)count;
            }
            else
            {
                if (!_bytes.TryGet(ref _nextSegmentPosition, out ReadOnlyMemory<byte> memory))
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                _currentSpan = memory.Span;
                _currentSpanIndex = 0;
                Advance(count - unreadLength);
            }
        }

        public void Advance(Position position)
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

        Position Position
        {
            get {
                var result = _currentSegmentPosition;
                result.Index += _currentSpanIndex;
                return result;
            }
        }

        Position AdvanceToDelimiter(byte value)
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
            return Position.End;
        }

        Position PositionOf(ReadOnlySpan<byte> value)
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
                int copied = Sequence.Copy(_bytes, _currentSegmentPosition + _currentSpanIndex + index, temp);
                if (copied < value.Length) return Position.End;

                if (temp.SequenceEqual(value)) return _currentSegmentPosition + _currentSpanIndex + index;
                else throw new NotImplementedException(); // need to check farther in this span
            }

            if (unread.Length == 0) return Position.End;

            throw new NotImplementedException();
        }

        static int CopyTo(BytesReader<TSequence> bytes, Span<byte> buffer)
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
        public static BytesReader<T> Create<T>(T sequence) where T : ISequence<ReadOnlyMemory<byte>>
        {
            return new BytesReader<T>(sequence);
        }
    }
}
