// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;
using System.Runtime.InteropServices;

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
            var m = MemoryMarshal.AsMemory(bytes);
            if (!m.TryGetArray(out var segment))
            {
                throw new NotSupportedException();
            }
            _start = segment.Array;
            _startIndex = segment.Offset;
            _end = segment.Array;
            _endIndex = segment.Count + _startIndex;

            Validate();
        }

        public ReadOnlyBytes(IMemoryList<byte> first, IMemoryList<byte> last)
        {
            _start = first;
            _startIndex = 0;
            _end = last;
            _endIndex = last.Memory.Length;

            Validate();
        }

        private ReadOnlyBytes(byte[] bytes, int index, int length)
        {
            _start = bytes;
            _startIndex = index;
            _end = bytes;
            _endIndex = length + index;
        }

        internal ReadOnlyBytes(object start, int startIndex, object end, int endIndex)
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
                    var sl = (IMemoryList<byte>)_start;
                    index += _startIndex;
                    while(true)
                    {
                        var m = sl.Memory;
                        if(m.Length > index)
                        {
                            break;
                        }
                        index -= m.Length;
                        sl = sl.Rest;
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
                        el = el.Rest;
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
                    var array = position.GetItem<byte[]>();
                    return new ReadOnlyBytes(array, position.Index, array.Length - position.Index);
                case Type.MemoryList:
                    return Slice(position, Position.Create((IMemoryList<byte>)_end, _endIndex));
                default: throw new NotImplementedException();
            }
        }

        public ReadOnlyBytes Slice(Position start, Position end)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var startArray = start.GetItem<byte[]>();
                    return new ReadOnlyBytes(startArray, start.Index, end.Index - start.Index);
                case Type.MemoryList:
                    var startList = start.GetItem<IMemoryList<byte>>();
                    var endList = end.GetItem<IMemoryList<byte>>();
                    return new ReadOnlyBytes(startList, start.Index, endList, end.Index);
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
                        var list = (IMemoryList<byte>)_start;
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
                        var sl = (IMemoryList<byte>)_start;
                        var el = (IMemoryList<byte>)_end;
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
                case Type.String:
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
                if (_start is string) return Type.String;
                if (_start is OwnedMemory<byte>) return Type.OwnedMemory;
                if (_start is IMemoryList<byte>) return Type.MemoryList;
                throw new NotSupportedException();
            }
        }

        public Position First => Position.Create(_start, _startIndex);

        public int CopyTo(Span<byte> buffer)
        {
            var array = _start as byte[];
            int toCopy;
            if (array != null)
            {
                toCopy = Math.Min((int)Length, buffer.Length);
                array.AsSpan().Slice(_startIndex, toCopy).CopyTo(buffer);
                return toCopy;
            }

            var position = this.First;
            int copied = 0;
            while(this.TryGet(ref position, out var memory) && buffer.Length > 0){
                var segment = memory.Span;
                toCopy = Math.Min(segment.Length, buffer.Length);
                segment.Slice(0, toCopy).CopyTo(buffer);
                buffer = buffer.Slice(toCopy);
                copied += toCopy;
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
            if(position.IsEnd)
            {
                item = default;
                return false;
            }

            var array = _start as byte[];
            if (array != null)
            {
                var start = _startIndex + position.Index;
                var length = (int)Length - position.Index;
                item = new ReadOnlyMemory<byte>(array, start, length);
                if (advance) position = Position.End;
                return true;
            }

            if (Kind == Type.MemoryList)
            {
                if(position == default)
                {
                    var first = _start as IMemoryList<byte>;
                    item = first.Memory.Slice(_startIndex);
                    if (advance) position = Position.Create(first.Rest);
                    if (ReferenceEquals(_end, _start)){
                        item = item.Slice(0, (int)Length);
                        if (advance) position = Position.End;
                    }
                    return true;
                }

                var (node, index) = position.Get<IMemoryList<byte>>();
                item = node.Memory.Slice(index);
                if (ReferenceEquals(node, _end))
                {
                    item = item.Slice(0, _endIndex - index);
                    if (advance) position = Position.End;
                }
                else
                {
                    if (advance) position = Position.Create(node.Rest);
                }
                return true;
            }

            throw new NotImplementedException();
        }

        enum Type : byte
        {
            Array,
            String,
            OwnedMemory,
            MemoryList,
        }
    }
}


