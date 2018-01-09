// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Buffers
{
    /// <summary>
    ///  Multi-segment buffer
    /// </summary>
    public readonly struct ReadOnlyBytes : ISequence<ReadOnlyMemory<byte>>
    {
        readonly object _start;
        readonly int _startIndex;
        readonly object _end;
        readonly int _endIndex;

        public ReadOnlyBytes(byte[] bytes)
        {
            _start = bytes;
            _startIndex = 0;
            _end = bytes;
            _endIndex = bytes.Length;

            Validate();
        }

        public ReadOnlyBytes(ReadOnlyMemory<byte> bytes)
        {
            Memory<byte> memory = MemoryMarshal.AsMemory(bytes);
            if (!memory.TryGetArray(out var segment))
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

        // TODO: Hide this ctor and force users to pass Positions. This will let us hide Position.Object 
        public ReadOnlyBytes(IBufferList first, IBufferList last)
        {
            _start = first;
            _startIndex = 0;
            _end = last;
            _endIndex = last.Memory.Length;

            Validate();
        }

        public ReadOnlyBytes(Position first, Position last)
        {
            (_start, _startIndex) = first.Get<object>();
            (_end, _endIndex) = last.Get<object>();

            Validate();
        }

        private ReadOnlyBytes(byte[] bytes, int index, int length)
        {
            _start = bytes;
            _startIndex = index;
            _end = bytes;
            _endIndex = length + index;
        }

        private ReadOnlyBytes(object start, int startIndex, object end, int endIndex)
        {
            _start = start;
            _startIndex = startIndex;
            _end = end;
            _endIndex = endIndex;
        }

        public ReadOnlyBytes Slice(long index, long length)
        {
            if (length == 0) return Empty;
            if (index == 0 && length == Length) return this;

            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    return new ReadOnlyBytes((byte[])_start, (int)(_startIndex + index), (int)length);
                case Type.MemoryList:
                    var sl = (IBufferList)_start;
                    index += _startIndex;
                    while(true)
                    {
                        var m = sl.Memory;
                        if(m.Length > index)
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
                        if(m.Length > length)
                        {
                            return new ReadOnlyBytes(sl, (int)index, el, (int)length);
                        }

                        length -= m.Length;

                        if(length == 0)
                        {
                            return new ReadOnlyBytes(sl, (int)index, el, m.Length);
                        }
                        el = el.Next;
                    }
                default:
                    throw new NotImplementedException();
            }
        }

        public ReadOnlyBytes Slice(int index, int length)
            => Slice((long)index, (long)length);

        public ReadOnlyBytes Slice(long index)
            => Slice(index, Length - index);

        public ReadOnlyBytes Slice(int index)
            => Slice((long)index);

        public ReadOnlyBytes Slice(Position position)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var (array, index) = position.Get<byte[]>();
                    return new ReadOnlyBytes(array, index, array.Length - index);
                case Type.MemoryList:
                    return Slice(position, new Position((IBufferList)_end, _endIndex));
                default: throw new NotImplementedException();
            }
        }

        public ReadOnlyBytes Slice(Position start, Position end)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var (array, index) = start.Get<byte[]>();
                    return new ReadOnlyBytes(array, index, (int)end - index);
                case Type.MemoryList:
                    var (startList, startIndex) = start.Get<IBufferList>();
                    var (endList, endIndex) = end.Get<IBufferList>();
                    return new ReadOnlyBytes(startList, startIndex, endList, endIndex);
                default:
                    throw new NotImplementedException();
            }

        }

        public static readonly ReadOnlyBytes Empty = new ReadOnlyBytes(new byte[0]);

        public ReadOnlyMemory<byte> Memory {
            get {
                var kind = Kind;
                switch (kind)
                {
                    case Type.Array:
                        return new ReadOnlyMemory<byte>((byte[])_start, _startIndex, _endIndex - _startIndex);
                    case Type.MemoryList:
                        var list = (IBufferList)_start;
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
            get {
                var kind = Kind;
                switch (kind)
                {
                    case Type.Array:
                        return _endIndex - _startIndex;
                    case Type.MemoryList:
                        var sl = (IBufferList)_start;
                        var el = (IBufferList)_end;
                        return (el.VirtualIndex + _endIndex) - (sl.VirtualIndex + _startIndex);
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
                case Type.OwnedMemory:
                    if(!ReferenceEquals(_start, _end) || _startIndex > _endIndex) { throw new NotSupportedException(); }              
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
            get {
                if (_start is byte[]) return Type.Array;
                if (_start is OwnedMemory<byte>) return Type.OwnedMemory;
                if (_start is IBufferList) return Type.MemoryList;
                throw new NotSupportedException();
            }
        }

        public Position Start => new Position(_start, _startIndex);

        public int CopyTo(Span<byte> buffer)
        {
            var array = _start as byte[];
            if (array != null)
            {
                int length = _endIndex - _startIndex;
                if (buffer.Length < length) length = buffer.Length;
                array.AsSpan().Slice(_startIndex, length).CopyTo(buffer);
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

        public bool TryGet(ref Position position, out ReadOnlyMemory<byte> item, bool advance = true)
        {
            if(position == default)
            {
                item = default;
                return false;
            }

            var array = _start as byte[];
            if (array != null)
            {
                var start = (int)position;
                var length = _endIndex - (int)position;
                if (length == 0)
                {
                    item = ReadOnlyMemory<byte>.Empty;
                }
                else
                {
                    item = new ReadOnlyMemory<byte>(array, start, length);
                }
                if (advance) position = default;
                return true;
            }

            if (Kind == Type.MemoryList)
            {
                var (node, index) = position.Get<IBufferList>();
                item = node.Memory.Slice(index);
                if (ReferenceEquals(node, _end))
                {
                    item = item.Slice(0, _endIndex - index);
                    if (advance) position = default;
                }
                else
                {
                    if (advance) position = new Position(node.Next, 0);
                }
                return true;
            }

            var om = _start as OwnedMemory<byte>;
            if (om != null)
            {
                var start = (int)position;
                var length = _endIndex - (int)position;
                item = om.Memory.Slice(start, length);
                if (advance) position = default;
                return true;
            }

            throw new NotImplementedException();
        }

        public Position Seek(Position origin, long offset)
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
                    var (segment, index) = origin.Get<IBufferList>();
                    return new Position(segment, (int)(index + offset));
                }
            }
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        enum Type : byte
        {
            Array,
            OwnedMemory,
            MemoryList,
        }

        public override string ToString()
        {
            var buffer = ToSpan().ToArray();
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);

        }
    }
}


