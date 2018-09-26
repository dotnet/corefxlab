// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Sequences;
using System.Runtime.InteropServices;

namespace System.Buffers
{
    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public readonly struct ReadWriteBytes : ISequence<Memory<byte>>
    {
        readonly object _start;
        readonly int _startIndex;
        readonly object _end;
        readonly int _endIndex;

        public ReadWriteBytes(byte[] bytes)
        {
            _start = bytes;
            _startIndex = 0;
            _end = bytes;
            _endIndex = bytes.Length;

            Validate();
        }

        public ReadWriteBytes(Memory<byte> bytes)
        {
            if (!MemoryMarshal.TryGetArray<byte>(bytes, out var segment))
            {
                // TODO: once we are in System.Memory, this will get OwnedMemory out of the Memory
                throw new NotImplementedException();
            }
            _start = segment.Array;
            _startIndex = segment.Offset;
            _end = segment.Array;
            _endIndex = segment.Count + _startIndex;

            Validate();
        }

        public ReadWriteBytes(ReadOnlySequenceSegment<byte> first, ReadOnlySequenceSegment<byte> last)
        {
            _start = first;
            _startIndex = 0;
            _end = last;
            _endIndex = last.Memory.Length;

            Validate();
        }

        public ReadWriteBytes(SequencePosition first, SequencePosition last)
        {
            (_start, _startIndex) = first.Get<object>();
            (_end, _endIndex) = last.Get<object>();

            Validate();
        }

        private ReadWriteBytes(byte[] bytes, int index, int length)
        {
            _start = bytes;
            _startIndex = index;
            _end = bytes;
            _endIndex = length + index;
        }

        private ReadWriteBytes(object start, int startIndex, object end, int endIndex)
        {
            _start = start;
            _startIndex = startIndex;
            _end = end;
            _endIndex = endIndex;
        }

        public ReadWriteBytes Slice(long index, long length)
        {
            if (length == 0) return Empty;
            if (index == 0 && length == Length) return this;

            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    return new ReadWriteBytes((byte[])_start, (int)(_startIndex + index), (int)length);
                case Type.MemoryList:
                    var sl = (ReadOnlySequenceSegment<byte>)_start;
                    index += _startIndex;
                    while (true)
                    {
                        var m = sl.Memory;
                        if (m.Length > index)
                        {
                            break;
                        }
                        index -= m.Length;
                        sl = sl.Next;
                    }

                    var el = sl;
                    length += index;
                    while (true)
                    {
                        var m = el.Memory;
                        if (m.Length > length)
                        {
                            return new ReadWriteBytes(sl, (int)index, el, (int)length);
                        }

                        length -= m.Length;

                        if (length == 0)
                        {
                            return new ReadWriteBytes(sl, (int)index, el, m.Length);
                        }
                        el = el.Next;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public ReadWriteBytes Slice(int index, int length)
            => Slice((long)index, (long)length);

        public ReadWriteBytes Slice(long index)
            => Slice(index, Length - index);

        public ReadWriteBytes Slice(int index)
            => Slice((long)index);

        public ReadWriteBytes Slice(SequencePosition position)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var (array, index) = position.Get<byte[]>();
                    return new ReadWriteBytes(array, index, array.Length - index);
                case Type.MemoryList:
                    return Slice(position, new SequencePosition((ReadOnlySequenceSegment<byte>)_end, _endIndex));
                default: throw new NotImplementedException();
            }
        }

        public ReadWriteBytes Slice(SequencePosition start, SequencePosition end)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var (array, index) = start.Get<byte[]>();
                    return new ReadWriteBytes(array, index, end.GetInteger() - index);
                case Type.MemoryList:
                    var (startList, startIndex) = start.Get<ReadOnlySequenceSegment<byte>>();
                    var (endList, endIndex) = end.Get<ReadOnlySequenceSegment<byte>>();
                    return new ReadWriteBytes(startList, startIndex, endList, endIndex);
                default:
                    throw new NotImplementedException();
            }

        }

        public static readonly ReadWriteBytes Empty = new ReadWriteBytes(new byte[0]);

        public ReadOnlyMemory<byte> Memory
        {
            get
            {
                var kind = Kind;
                switch (kind)
                {
                    case Type.Array:
                        return new ReadOnlyMemory<byte>((byte[])_start, _startIndex, _endIndex - _startIndex);
                    case Type.MemoryList:
                        var list = (ReadOnlySequenceSegment<byte>)_start;
                        if (ReferenceEquals(list, _end))
                        {
                            return list.Memory.Slice(_startIndex, _endIndex - _startIndex);
                        }
                        else
                        {
                            return list.Memory.Slice(_startIndex);
                        }
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public long Length
        {
            get
            {
                var kind = Kind;
                switch (kind)
                {
                    case Type.Array:
                        return _endIndex - _startIndex;
                    case Type.MemoryList:
                        var sl = (ReadOnlySequenceSegment<byte>)_start;
                        var el = (ReadOnlySequenceSegment<byte>)_end;
                        return (el.RunningIndex + _endIndex) - (sl.RunningIndex + _startIndex);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void Validate()
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                case Type.MemoryOwner:
                    if (!ReferenceEquals(_start, _end) || _startIndex > _endIndex) { throw new NotSupportedException(); }
                    break;
                case Type.MemoryList:
                    // assume it's good?
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        Type Kind
        {
            get
            {
                if (_start is byte[]) return Type.Array;
                if (_start is IMemoryOwner<byte>) return Type.MemoryOwner;
                if (_start is ReadOnlySequenceSegment<byte>) return Type.MemoryList;
                throw new NotSupportedException();
            }
        }

        public SequencePosition Start => new SequencePosition(_start, _startIndex);

        public int CopyTo(Span<byte> buffer)
        {
            var array = _start as byte[];
            if (array != null)
            {
                int length = _endIndex - _startIndex;
                if (buffer.Length < length) length = buffer.Length;
                array.AsSpan(_startIndex, length).CopyTo(buffer);
                return length;
            }

            var position = Start;
            int copied = 0;
            while (TryGet(ref position, out var memory) && buffer.Length > 0)
            {
                var segment = memory.Span;
                var length = segment.Length;
                if (buffer.Length < length) length = buffer.Length;
                segment.Slice(0, length).CopyTo(buffer);
                buffer = buffer.Slice(length);
                copied += length;
            }
            return copied;
        }

        public Span<byte> ToSpan()
        {
            var array = new byte[Length];
            CopyTo(array);
            return array;
        }

        public bool TryGet(ref SequencePosition position, out Memory<byte> item, bool advance = true)
        {
            if (position.Equals(default))
            {
                item = default;
                return false;
            }

            var array = _start as byte[];
            if (array != null)
            {
                var start = _startIndex + position.GetInteger();
                var length = _endIndex - _startIndex - position.GetInteger();
                item = new Memory<byte>(array, start, length);
                if (advance) position = default;
                return true;
            }

            if (Kind == Type.MemoryList)
            {
                var (node, index) = position.Get<ReadOnlySequenceSegment<byte>>();
                item = MemoryMarshal.AsMemory(node.Memory.Slice(index));
                if (ReferenceEquals(node, _end))
                {
                    item = item.Slice(0, _endIndex - index);
                    if (advance) position = default;
                }
                else
                {
                    if (advance) position = new SequencePosition(node.Next, 0);
                }
                return true;
            }

            throw new NotImplementedException();
        }

        public SequencePosition GetPosition(SequencePosition origin, long offset)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

            var previous = origin;
            while (TryGet(ref origin, out var memory))
            {
                var length = memory.Length;
                if (length < offset)
                {
                    offset -= length;
                    previous = origin;
                }
                else
                {
                    var (segment, index) = origin.Get<ReadOnlySequenceSegment<byte>>();
                    return new SequencePosition(segment, (int)(index + offset));
                }
            }
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        enum Type : byte
        {
            Array,
            MemoryOwner,
            MemoryList,
        }
    }
}


