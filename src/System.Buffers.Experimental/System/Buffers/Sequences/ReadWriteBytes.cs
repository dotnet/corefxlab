// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

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
            if (!bytes.TryGetArray(out var segment))
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

        public ReadWriteBytes(IMemoryList<byte> first, IMemoryList<byte> last)
        {
            _start = first;
            _startIndex = 0;
            _end = last;
            _endIndex = last.Memory.Length;

            Validate();
        }

        public ReadWriteBytes(SequenceIndex first, SequenceIndex last)
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
                    var sl = (IMemoryList<byte>)_start;
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

        public ReadWriteBytes Slice(SequenceIndex sequenceIndex)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var (array, index) = sequenceIndex.Get<byte[]>();
                    return new ReadWriteBytes(array, index, array.Length - index);
                case Type.MemoryList:
                    return Slice(sequenceIndex, new SequenceIndex((IMemoryList<byte>)_end, _endIndex));
                default: throw new NotImplementedException();
            }
        }

        public ReadWriteBytes Slice(SequenceIndex start, SequenceIndex end)
        {
            var kind = Kind;
            switch (kind)
            {
                case Type.Array:
                    var (array, index) = start.Get<byte[]>();
                    return new ReadWriteBytes(array, index, (int)end - index);
                case Type.MemoryList:
                    var (startList, startIndex) = start.Get<IMemoryList<byte>>();
                    var (endList, endIndex) = end.Get<IMemoryList<byte>>();
                    return new ReadWriteBytes(startList, startIndex, endList, endIndex);
                default:
                    throw new NotImplementedException();
            }

        }

        public static readonly ReadWriteBytes Empty = new ReadWriteBytes(new byte[0]);

        public ReadOnlyMemory<byte> Memory
        {
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
                case Type.OwnedMemory:
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
            get {
                if (_start is byte[]) return Type.Array;
                if (_start is OwnedMemory<byte>) return Type.OwnedMemory;
                if (_start is IMemoryList<byte>) return Type.MemoryList;
                throw new NotSupportedException();
            }
        }

        public SequenceIndex Start => new SequenceIndex(_start, _startIndex);

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

        public bool TryGet(ref SequenceIndex sequenceIndex, out Memory<byte> item, bool advance = true)
        {
            if (sequenceIndex == default)
            {
                item = default;
                return false;
            }

            var array = _start as byte[];
            if (array != null)
            {
                var start = _startIndex + (int)sequenceIndex;
                var length = _endIndex - _startIndex - (int)sequenceIndex;
                item = new Memory<byte>(array, start, length);
                if (advance) sequenceIndex = default;
                return true;
            }

            if (Kind == Type.MemoryList)
            {
                var (node, index) = sequenceIndex.Get<IMemoryList<byte>>();
                item = node.Memory.Slice(index);
                if (ReferenceEquals(node, _end))
                {
                    item = item.Slice(0, _endIndex - index);
                    if (advance) sequenceIndex = default;
                }
                else
                {
                    if (advance) sequenceIndex = new SequenceIndex(node.Next, 0);
                }
                return true;
            }

            throw new NotImplementedException();
        }

        public SequenceIndex GetPosition(SequenceIndex origin, long offset)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

            var previous = origin;
            while(TryGet(ref origin, out var memory))
            {
                var length = memory.Length;
                if(length < offset)
                {
                    offset -= length;
                    previous = origin;
                }
                else
                {
                    var (segment, index) = origin.Get<IMemoryList<byte>>();
                    return new SequenceIndex(segment, (int)(index + offset));
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
    }
}


