// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Collections.Sequences
{
    // a List<T> like type designed to be embeded in other types
    public struct ResizableArray<T>
    {
        private static T[] s_empty = new T[0];

        private T[] _array;
        private int _count;

        public ResizableArray(int capacity)
        {
            _array = capacity == 0 ? s_empty : new T[capacity];
            _count = 0;
        }

        public ResizableArray(T[] array, int count = 0)
        {
            _array = array;
            _count = count;
        }

        public T[] Items
        {
            get { return _array; }
            set { _array = value; }
        }
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public int Capacity => _array.Length;

        public T this[int index]
        {
            get
            {
                if (index > _count - 1) throw new IndexOutOfRangeException();
                return _array[index];
            }
            set
            {
                if (index > _count - 1) throw new IndexOutOfRangeException();
                _array[index] = value;
            }
        }

        public void Add(T item)
        {
            if (_array.Length == _count)
            {
                Resize();
            }
            _array[_count++] = item;
        }

        public void AddAll(T[] items)
        {
            if (items.Length > _array.Length - _count)
            {
                Resize(items.Length + _count);
            }
            items.CopyTo(_array, _count);
            _count += items.Length;
        }

        public void AddAll(ReadOnlySpan<T> items)
        {
            if (items.Length > _array.Length - _count)
            {
                Resize(items.Length + _count);
            }
            items.CopyTo(new Span<T>(_array).Slice(_count));
            _count += items.Length;
        }

        public void Clear()
        {
            _count = 0;
        }

        public T[] Resize(int newSize = -1)
        {
            T[] oldArray = _array;
            if (newSize == -1)
            {
                if (_array == null || _array.Length == 0)
                {
                    newSize = 4;
                }
                else
                {
                    newSize = _array.Length << 1;
                }
            }

            T[] newArray = new T[newSize];
            _array.AsSpan(0, _count).CopyTo(newArray);    // CopyTo will throw if newArray.Length < _count
            _array = newArray;
            return oldArray;
        }

        public T[] Resize(T[] newArray)
        {
            T[] oldArray = _array;
            _array.AsSpan(0, _count).CopyTo(newArray);  // CopyTo will throw if newArray.Length < _count
            _array = newArray;
            return oldArray;
        }

        public bool TryGet(ref SequencePosition position, out T item, bool advance = true)
        {
            int index = position.GetInteger();
            if (index < _count)
            {
                item = _array[index];
                if (advance) { position = new SequencePosition(null, index + 1); }
                return true;
            }

            item = default;
            position = default;
            return false;
        }

        public Span<T> Span => new Span<T>(_array, 0, _count);

        public ArraySegment<T> Full => new ArraySegment<T>(_array, 0, _count);
        public ArraySegment<T> Free => new ArraySegment<T>(_array, _count, _array.Length - _count);

        public Span<T> FreeSpan => new Span<T>(_array, _count, _array.Length - _count);

        public Memory<T> FreeMemory => new Memory<T>(_array, _count, _array.Length - _count);

        public int FreeCount => _array.Length - _count;
    }
}
