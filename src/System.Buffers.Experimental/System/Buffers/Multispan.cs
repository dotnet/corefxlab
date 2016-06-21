// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    /// <summary>
    /// A collection of slices backed by pooled arrays.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Slices should not be stored on the heap, and soon we will make it impossible to do.
    /// Yet if asynchronous operations must store their context on the heap. 
    /// This type creates and illusion of a collection of slices that can be stored on the heap.
    /// Note that this type is a mutable value type. You need to be careful when using it. 
    /// If you pass one of these as a method argument, you should probably pass it by ref.
    /// Also, be extra careful when disposing this type. If you dispose the original instance and its copy, 
    /// the pool used by this type will be corrupted.
    /// </remarks>
    public struct Multispan<T>
    {
        ArraySegment<T> _head;
        ArraySegment<T>[] _tail;
        int _count; // number of arrays (including the head and tail)

        /// <summary>
        /// Number of segments.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
        }

        public int CopyTo(T[] array)
        {
            Array.Copy(_head.Array, _head.Offset, array, 0, _head.Count);
            int index = _head.Count;
            for (int i = 0; i < _count - 1; i++)
            {
                var section = _tail[i];
                Array.Copy(section.Array, section.Offset, array, index, section.Count);
                index += section.Count;
            }
            return index;
        }

        public Span<T> Last
        {
            get { return this[Count - 1]; }
        }
        public Span<T> this[int index]
        {
            get
            {
                return AsSlice(GetAt(index));
            }
        }

        /// <summary>
        /// Removes items from the instance.
        /// </summary>
        /// <param name="itemCount">Number of items to remove</param>
        /// <returns></returns>
        /// <remarks>DO NOT dispose the original and then slice. Either the original or the sliced instance can be disposed, but not both.</remarks>
        public Multispan<T> Slice(int itemCount)
        {
            var result = new Multispan<T>();
            var first = GetAt(0);

            // if the only thing needing slicing is the head
            if (first.Count > itemCount)
            {
                result._head = _head.Slice(itemCount);
                EnsureTailCapacity(ref result, _count - 1);
                result._count = _count;
                Array.Copy(_tail, result._tail, _count - 1);
                return result;
            }

            // head will be removed; this computes how many tail segments need to be removed
            // and how many items from the first segment that is not removed
            var itemsLeftToRemove = itemCount - first.Count;
            int tailSegmentsToRemove = 1; // one is moved to the head
            for (int tailIndex = 0; tailIndex < _count - 1; tailIndex++)
            {
                if (itemsLeftToRemove == 0) { break; }
                var segment = _tail[tailIndex];
                if (segment.Count >= itemsLeftToRemove)
                {
                    break;
                }
                else {
                    tailSegmentsToRemove++;
                    itemsLeftToRemove -= segment.Count;
                }
            }

            result._head = _tail[tailSegmentsToRemove - 1].Slice(itemsLeftToRemove);
            result._count = _count - tailSegmentsToRemove;
            if (result._count == 1) return result; // we don't need tail; this multispan has just head
            EnsureTailCapacity(ref result, result._count - 1);
            Array.Copy(_tail, tailSegmentsToRemove, result._tail, 0, result._count - 1);
            return result;
        }

        public struct Enumerator
        {
            private Multispan<T> _buffer;
            int _index;

            internal Enumerator(Multispan<T> buffer)
            {
                _buffer = buffer;
                _index = -1;
            }

            public bool MoveNext()
            {
                _index++;
                if (_index >= _buffer._count) return false;
                return true;
            }

            public Span<T> Current
            {
                get
                {
                    return _buffer[_index];
                }
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Computes total number of items in all segments.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This method has O(Count) complexity.</remarks>
        public int TotalItemCount()
        {
            int length = _head.Count;
            for (int i = 0; i < _count - 1; i++)
            {
                length += _tail[i].Count;
            }
            return length;
        }

        /// <summary>
        /// Returns all allocated segments to the pool.
        /// </summary>
        public void Dispose()
        {
            if (_head.Array != null)
            {
                ArrayPool<T>.Shared.Return(_head.Array);
                _head = new ArraySegment<T>();
            }

            if (_tail != null)
            {
                for (int i = 0; i < _count - 1; i++)
                {
                    var segment = _tail[i];
                    ArrayPool<T>.Shared.Return(segment.Array);
                    _tail[i] = new ArraySegment<T>();
                }
                ArrayPool<ArraySegment<T>>.Shared.Return(_tail);
            }
            _tail = null;
            _count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfItems">Minimum number of items that can be stored in the new segment</param>
        /// <returns>Index of the newely allocated segment</returns>
        public int AppendNewSegment(int numberOfItems)
        {
            var array = ArrayPool<T>.Shared.Rent(numberOfItems);
            var buffer = new ArraySegment<T>(array, 0, array.Length);

            if (_count == 0)
            {
                _head = buffer;
                _count = 1;
            }
            else {
                if (_tail == null)
                {
                    _tail = ArrayPool<ArraySegment<T>>.Shared.Rent(4);
                }
                else if (_count > _tail.Length)
                {
                    EnsureTailCapacity(ref this, _count);
                }
                _tail[_count - 1] = buffer;
                _count++;
            }
            return _count - 1;
        }

        private static void EnsureTailCapacity(ref Multispan<T> ms, int count)
        {
            int desired = (ms._tail == null) ? 4 : ms._tail.Length * 2;
            while (desired < count)
            {
                desired = desired * 2;
            }
            var newSegments = ArrayPool<ArraySegment<T>>.Shared.Rent(desired);
            if (ms._tail != null)
            {
                ms._tail.CopyTo(newSegments, 0);
                ArrayPool<ArraySegment<T>>.Shared.Return(ms._tail);
            }
            ms._tail = newSegments;
        }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="factor">factor 2 doubles the size. Factor 0.5 halves the size.</param>
        /// <remarks>If the segment is enlarged, the data will be lost. The caller is responsible for copying the data befor eresizing.</remarks>
        public void ResizeSegment(int index, float factor)
        {
            var segment = GetAt(index);
            var newItemCount = (int)(segment.Array.Length * factor);
            ResizeSegment(index, newItemCount);
        }

        /// <summary>
        /// If the segment is enlarged, the data will be lost
        /// </summary>
        /// <param name="index"></param>
        /// <param name="newItemCount"></param>
        /// <remarks>If the segment is enlarged, the data will be lost. The caller is responsible for copying the data before resizing.</remarks>
        public void ResizeSegment(int index, int newItemCount)
        {
            var segment = GetAt(index);
            if (segment.Array.Length < newItemCount)
            {
                ArrayPool<T>.Shared.Return(segment.Array);
                var newArray = ArrayPool<T>.Shared.Rent(newItemCount);
                var newSegment = new ArraySegment<T>(newArray);
                if (index == 0) _head = newSegment;
                else _tail[index - 1] = newSegment;
            }
            if (segment.Array.Length >= newItemCount)
            {
                if (index == 0) { _head = new ArraySegment<T>(_head.Array, _head.Offset, newItemCount); }
                else {
                    int tailIndex = index - 1;
                    _tail[tailIndex] = new ArraySegment<T>(_tail[tailIndex].Array, _tail[tailIndex].Offset, newItemCount);
                }
            }
        }

        private ArraySegment<T> GetAt(int index)
        {
            if (index == 0) return _head;
            return _tail[index - 1];
        }

        private static Span<T> AsSlice(ArraySegment<T> segment)
        {
            return new Span<T>(segment.Array, segment.Offset, segment.Count);
        }
    }

    static class Extensions
    {
        public static ArraySegment<T> Slice<T>(this ArraySegment<T> source, int count)
        {
            var result = new ArraySegment<T>(source.Array, source.Offset + count, source.Count - count);
            return result;
        }
    }
}




