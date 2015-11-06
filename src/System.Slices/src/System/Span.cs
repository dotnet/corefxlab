// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace System
{
    /// <summary>
    /// Span is a uniform API for dealing with arrays and subarrays, strings
    /// and substrings, and unmanaged memory buffers.  It adds minimal overhead
    /// to regular accesses and is a struct so that creation and subslicing do
    /// not require additional allocations.  It is type- and memory-safe.
    /// </summary>
    public struct Span<T> : IEnumerable<T>, IEquatable<Span<T>>
    {
        /// <summary>A managed array/string; or null for native ptrs.</summary>
        readonly object _object;
        /// <summary>An byte-offset into the array/string; or a native ptr.</summary>
        readonly UIntPtr _offset;
        /// <summary>Fetches the number of elements this Span contains.</summary>
        public int Length { get; private set; }

        /// <summary>
        /// Creates a new span over the entirety of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        public Span(T[] array)
        {
            Contract.Requires(array != null);
            _object = array;
            _offset = new UIntPtr((uint)SpanHelpers<T>.OffsetToArrayData);
            Length = array.Length;
        }

        /// <summary>
        /// Creates a new span over the portion of the target array beginning
        /// at 'start' index.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the span.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        // TODO: Should we have this overload? It is really confusing when you also have Span(T* array, int length)
        //       While with Slice it makes sense it might not in here.
        internal Span(T[] array, int start)
        {
            Contract.Requires(array != null);
            Contract.RequiresInInclusiveRange(start, array.Length);
            if (start < array.Length)
            {
                _object = array;
                _offset = new UIntPtr(
                    (uint)(SpanHelpers<T>.OffsetToArrayData + (start * PtrUtils.SizeOf<T>())));
                Length = array.Length - start;
            }
            else
            {
                _object = null;
                _offset = UIntPtr.Zero;
                Length = 0;
            }
        }

        /// <summary>
        /// Creates a new span over the portion of the target array beginning
        /// at 'start' index and ending at 'end' index (exclusive).
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <param name="start">The index at which to begin the span.</param>
        /// <param name="length">The number of items in the span.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start or end index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public Span(T[] array, int start, int length)
        {
            Contract.Requires(array != null);
            Contract.RequiresInInclusiveRange(start, array.Length);
            Contract.RequiresNonNegative(length);
            Contract.RequiresInInclusiveRange(start + length, array.Length);
            if (start < array.Length)
            {
                _object = array;
                _offset = new UIntPtr(
                    (uint)(SpanHelpers<T>.OffsetToArrayData + (start * PtrUtils.SizeOf<T>())));
                Length = length;
            }
            else
            {
                _object = null;
                _offset = UIntPtr.Zero;
                Length = 0;
            }
        }

        /// <summary>
        /// Creates a new span over the target unmanaged buffer.  Clearly this
        /// is quite dangerous, because we are creating arbitrarily typed T's
        /// out of a void*-typed block of memory.  And the length is not checked.
        /// But if this creation is correct, then all subsequent uses are correct.
        /// </summary>
        /// <param name="ptr">An unmanaged pointer to memory.</param>
        /// <param name="length">The number of T elements the memory contains.</param>
        [CLSCompliant(false)]
        public unsafe Span(void* ptr, int length)
        {
            Contract.Requires(length >= 0);
            Contract.Requires(length == 0 || ptr != null);
            _object = null;
            _offset = new UIntPtr(ptr);
            Length = length;
        }

        /// <summary>
        /// An internal helper for creating spans. Not for public use.
        /// </summary>
        internal Span(object obj, UIntPtr offset, int length)
        {
            _object = obj;
            _offset = offset;
            Length = length;
        }

        public static Span<T> Empty { get { return default(Span<T>); } }

        /// <summary>
        /// Fetches the managed object (if any) that this span points at.
        /// </summary>
        internal object Object
        {
            get { return _object; }
        }

        /// <summary>
        /// Fetches the offset -- or sometimes, raw pointer -- for this span.
        /// </summary>
        internal UIntPtr Offset
        {
            get { return _offset; }
        }

        [CLSCompliant(false)]
        public unsafe void* UnsafePointer
        {
            get { return _offset.ToPointer(); }
        }

        /// <summary>
        /// Fetches the element at the specified index.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public T this[int index]
        {
            get
            {
                Contract.RequiresInRange(index, Length);
                return PtrUtils.Get<T>(
                    _object, _offset + (index * PtrUtils.SizeOf<T>()));
            }
            set
            {
                Contract.RequiresInRange(index, Length);
                PtrUtils.Set<T>(
                    _object, _offset + (index * PtrUtils.SizeOf<T>()), value);
            }
        }

        /// <summary>
        /// Copies the contents of this span into a new array.  This heap
        /// allocates, so should generally be avoided, however is sometimes
        /// necessary to bridge the gap with APIs written in terms of arrays.
        /// </summary>
        public T[] CreateArray()
        {
            var dest = new T[Length];
            TryCopyTo(dest.Slice());
            return dest;
        }

        /// <summary>
        /// Copies the contents of this span into another.  The destination
        /// must be at least as big as the source, and may be bigger.
        /// </summary>
        /// <param name="dest">The span to copy items into.</param>
        public bool TryCopyTo(Span<T> dest)
        {
            if (Length > dest.Length)
            {
                return false;
            }

            // TODO(joe): specialize to use a fast memcpy if T is pointerless.
            for (int i = 0; i < Length; i++)
            {
                dest[i] = this[i];
            }
            return true;
        }

        public void Set(Span<T> values)
        {
            if (Length < values.Length)
            {
                throw new ArgumentOutOfRangeException("values");
            }

            // TODO(joe): specialize to use a fast memcpy if T is pointerless.
            for (int i = 0; i < Length; i++)
            {
                this[i] = values[i];
            }
        }

        public void Set(T[] values)
        {
            if (Length < values.Length)
            {
                throw new ArgumentOutOfRangeException("values");
            }

            // TODO(joe): specialize to use a fast memcpy if T is pointerless.
            for (int i = 0; i < values.Length; i++)
            {
                this[i] = values[i];
            }
        }

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start'.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public Span<T> Slice(int start)
        {
            return Slice(start, Length - start);
        }

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start', and
        /// ending at 'end' (exclusive).
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <param name="end">The index at which to end this slice (exclusive).</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start or end index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public Span<T> Slice(int start, int length)
        {
            Contract.Requires(start + length <= Length);
            return new Span<T>(
                _object, _offset + (start * PtrUtils.SizeOf<T>()), length);
        }

        /// <summary>
        /// Checks to see if two spans point at the same memory.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        public bool ReferenceEquals(Span<T> other)
        {
            return Object == other.Object &&
                Offset == other.Offset && Length == other.Length;
        }

        public override int GetHashCode()
        {
            // TODO: Write something better
            uint hash = unchecked((uint)Length);
            foreach (T el in this)
            {
                hash = (hash >> 7) | (hash << 25);
                hash ^= unchecked((uint)el.GetHashCode());
            }

            return unchecked((int)(hash));
        }

        public bool Equals(Span<T> other)
        {
            if (ReferenceEquals(other))
            {
                return true;
            }

            if (this.Length != other.Length)
            {
                return false;
            }

            Enumerator thisIt = this.GetEnumerator();
            Enumerator otherIt = other.GetEnumerator();
            while (true)
            {
                bool hasNext = thisIt.MoveNext();
                if (hasNext != otherIt.MoveNext())
                {
                    return false;
                }

                if (!hasNext)
                {
                    return true;
                }

                // TODO: Fix it so it doesn't box (memcmp)
                if (!thisIt.Current.Equals(otherIt.Current))
                {
                    return false;
                }
            }
        }

        public override bool Equals(object obj)
        {
            // TODO: Should this work with other spans?
            if (obj is Span<T>)
            {
                return Equals((Span<T>)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns an enumerator over the span's entire contents.
        /// </summary>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// A struct-based enumerator, to make fast enumerations possible.
        /// This isn't designed for direct use, instead see GetEnumerator.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            Span<T> _slice;    // The slice being enumerated.
            int _position; // The current position.

            public Enumerator(Span<T> slice)
            {
                _slice = slice;
                _position = -1;
            }

            public T Current
            {
                get { return _slice[_position]; }
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
                _slice = default(Span<T>);
                _position = -1;
            }

            public bool MoveNext()
            {
                return ++_position < _slice.Length;
            }

            public void Reset()
            {
                _position = -1;
            }
        }
    }
}


