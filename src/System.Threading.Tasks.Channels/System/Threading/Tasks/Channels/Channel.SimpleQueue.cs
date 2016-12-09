// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Threading.Tasks.Channels
{
    public static partial class Channel
    {
        [DebuggerDisplay("Count = {_size}")]
        private sealed class SimpleQueue<T>
        {
            private T[] _array = Array.Empty<T>();
            private int _head; // First valid element in the queue
            private int _tail; // Last valid element in the queue
            private int _size; // Number of elements.

            public int Count => _size;

            public void Enqueue(T item)
            {
                if (_size == _array.Length)
                {
                    Grow();
                }

                _array[_tail] = item;
                if (++_tail == _array.Length)
                {
                    _tail = 0;
                }
                _size++;
            }

            public T Dequeue()
            {
                T item = _array[_head];
                _array[_head] = default(T);

                if (++_head == _array.Length)
                {
                    _head = 0;
                }
                _size--;

                return item;
            }

            public IEnumerator<T> GetEnumerator() // meant for debug purposes only
            {
                int pos = _head;
                int count = _size;
                while (count-- > 0)
                {
                    yield return _array[pos];
                    pos = (pos + 1) % _size;
                }
            }

            private void Grow()
            {
                const int MinimumGrow = 4;

                int capacity = (int)(_array.Length * 2L);
                if (capacity < _array.Length + MinimumGrow)
                {
                    capacity = _array.Length + MinimumGrow;
                }

                T[] newArray = new T[capacity];

                if (_head < _tail)
                {
                    Array.Copy(_array, _head, newArray, 0, _size);
                }
                else
                {
                    Array.Copy(_array, _head, newArray, 0, _array.Length - _head);
                    Array.Copy(_array, 0, newArray, _array.Length - _head, _tail);
                }

                _array = newArray;
                _head = 0;
                _tail = (_size == capacity) ? 0 : _size;
            }

        }
    }
}
