// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Text;
using System.Collections.Sequences;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Buffers.Text
{
    public static class BytesReader
    {
        public static BytesReader<T> Create<T>(T sequence) where T : ISequence<ReadOnlyMemory<byte>>
        {
            return new BytesReader<T>(sequence);
        }
    }

    /// <summary>
    /// Used to read text from byte buffers
    /// </summary>
    public ref struct BytesReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        readonly TSequence _bytes;
        Position _currentSegmentPosition;
        Position _nextSegmentPosition;
        ReadOnlyMemory<byte> _currentMemory; // TODO: I think this can be removed
        ReadOnlySpan<byte> _currentSpan;
        int _currentSpanIndex;

        readonly SymbolTable _symbolTable;

        // TODO: should there be a ctor that takes sequence + position? 
        // TODO: should there be a type that is sequence + position?
        public BytesReader(TSequence bytes)
        {
            _bytes = bytes;
            _currentSegmentPosition = bytes.First;
            _nextSegmentPosition = _currentSegmentPosition;
            _bytes.TryGet(ref _nextSegmentPosition, out _currentMemory);
            _currentSpan = _currentMemory.Span;
            _symbolTable = SymbolTable.InvariantUtf8;
            _currentSpanIndex = 0;
        }

        public BytesReader(ReadOnlyMemory<byte> bytes)
        {
            _bytes = default;
            _nextSegmentPosition = default;
            _currentSegmentPosition = Position.Create(0);
            _currentMemory = bytes;
            _currentSpan = bytes.Span;
            _symbolTable = SymbolTable.InvariantUtf8;
            _currentSpanIndex = 0;
        }

        public BytesReader(ReadOnlySpan<byte> bytes)
        {
            _bytes = default;
            _nextSegmentPosition = default;
            _currentSegmentPosition = Position.Create(0);
            _currentMemory = default;
            _currentSpan = bytes;
            _symbolTable = SymbolTable.InvariantUtf8;
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
                Seek(range.To);
                Seek(delimiter.Length);
            }
            return range;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Seek(long count)
        {
            var unreadLength = _currentSpan.Length - _currentSpanIndex;
            if (count <= unreadLength)
            {
                _currentSpanIndex += (int)count;
            }
            else
            {
                if (!_bytes.TryGet(ref _nextSegmentPosition, out _currentMemory))
                {
                    throw new ArgumentOutOfRangeException(nameof(count));
                }
                _currentSpan = _currentMemory.Span;
                _currentSpanIndex = 0;
                Seek(count - unreadLength);
            }
        }

        public void Seek(Position position)
        {
            _currentSegmentPosition = position;
            _nextSegmentPosition = position;
            if (_bytes.TryGet(ref _nextSegmentPosition, out _currentMemory))
            {
                _currentSpan = _currentMemory.Span;
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
        public bool TryParseBoolean(out bool value)
        {
            var unread = Unread;
            if (CustomParser.TryParseBoolean(unread, out value, out int consumed, _symbolTable))
            {
                Debug.Assert(consumed <= unread.Length);
                if (unread.Length > consumed)
                {
                    _currentSpanIndex += consumed;
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[15];
            CopyTo(this, tempSpan, out var copied);

            if (CustomParser.TryParseBoolean(tempSpan.Slice(0, copied), out value, out consumed, _symbolTable))
            {
                Seek(consumed);
                return true;
            }

            return false;
        }

        public bool TryParseUInt64(out ulong value)
        {
            var unread = Unread;
            if (CustomParser.TryParseUInt64(unread, out value, out int consumed, default, _symbolTable))
            {
                if (unread.Length > consumed)
                {
                    _currentSpanIndex += consumed;
                    return true;
                }
            }

            Span<byte> tempSpan = stackalloc byte[32];
            CopyTo(this, tempSpan, out int copied);

            if (CustomParser.TryParseUInt64(tempSpan.Slice(0, copied), out value, out consumed, 'G', _symbolTable))
            {
                Seek(consumed);
                return true;
            }

            return false;
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
            var memory = _currentMemory;
            while (_bytes.TryGet(ref _nextSegmentPosition, out _currentMemory))
            {
                index = _currentMemory.Span.IndexOf(value);
                if (index != -1)
                {
                    _currentSegmentPosition = previousPosition;
                    _currentSpan = _currentMemory.Span;
                    _currentSpanIndex = index + 1;
                    return _currentSegmentPosition + index;
                }
                previousPosition = _nextSegmentPosition;
            }

            _currentMemory = memory;
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

        static void CopyTo(BytesReader<TSequence> bytes, Span<byte> buffer, out int copied)
        {
            var first = bytes.Unread;
            if (first.Length > buffer.Length)
            {
                first.Slice(0, buffer.Length).CopyTo(buffer);
                copied = buffer.Length;
            }
            else if (first.Length == buffer.Length)
            {
                first.CopyTo(buffer);
                copied = buffer.Length;
            }
            else
            {
                first.CopyTo(buffer);
                copied = first.Length;
                copied += Sequence.Copy(bytes._bytes, bytes._nextSegmentPosition, buffer.Slice(copied));
            }
        }
    }
}
