// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.Collections.Sequences
{
    // a List<T> like type designed to be embeded in other types
    public struct ResizableArray<T>
    {
        public T[] _array;
        public int _count;

        public ResizableArray(int capacity)
        {
            _array = new T[capacity];
            _count = 0;
        }

        public ResizableArray(T[] array, int count = 0)
        {
            _array = array;
            _count = count;
        }

        public void Add(T item)
        {
            if (_array.Length == _count) {
                Resize();
            }
            _array[_count++] = item;
        }

        public T[] Resize(int newSize = -1)
        {
            var oldArray = _array;
            if (newSize == -1) {
                if(_array == null || _array.Length == 0) {
                    newSize = 4;
                }
                else {
                    newSize = _array.Length << 1;
                }
            }           

            var newArray = new T[newSize];
            _array.CopyTo(newArray, 0);
            _array = newArray;
            return oldArray;
        }

        public T[] Resize(T[] newArray)
        {
            if (newArray.Length < _count) throw new Exception(String.Format("{0} {1}", newArray.Length, _count));
            var oldArray = _array;
            Array.Copy(_array, 0, newArray, 0, _count);
            _array = newArray;
            return oldArray;
        }

        public int Count => _count;

        public int Capacity => _array.Length;

        public T this[int index] {
            get {
                if (index > _count - 1) throw new IndexOutOfRangeException();
                return _array[index];
            }
            set {
                if (index > _count - 1) throw new IndexOutOfRangeException();
                _array[index] = value;
            }
        }

        public T GetAt(ref Position position, bool advance = false)
        {
            if( _count == 0) {
                position = Position.Invalid;
                return default(T);
            }

            if (position.Equals(Position.BeforeFirst)) {
                position = Position.Invalid;
                if (advance) {
                    position = Position.First;
                }
                return default(T);
            }

            if (position.IntegerPosition < _count) {
                var item = _array[position.IntegerPosition];
                if (advance) {
                    position.IntegerPosition++;
                    if (position.IntegerPosition == _count) position = Position.AfterLast;
                }
                return item;
            }

            position = Position.Invalid;
            return default(T);
        }

        public ArraySegment<T> Full => new ArraySegment<T>(_array, 0, _count);
        public ArraySegment<T> Free => new ArraySegment<T>(_array, _count, _array.Length - _count);
    }
}
