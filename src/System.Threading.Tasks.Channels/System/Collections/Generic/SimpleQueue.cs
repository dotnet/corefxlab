// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace System.Collections.Generic
{
    /// <summary>Simple FIFO queue without some of the overheads of <see cref="Queue{T}"/> </summary>
    /// <typeparam name="T">Type of the data stored in the queue.</typeparam>
    [DebuggerDisplay("Count = {_size}")]
    internal sealed class SimpleQueue<T>
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
