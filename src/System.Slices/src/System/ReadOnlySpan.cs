// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// ReadOnlySpan is a read-only view over Span<typeparam name="T"></typeparam>
    /// </summary>
    public partial struct ReadOnlySpan<T> : IEnumerable<T>, IEquatable<ReadOnlySpan<T>>
    {
        /// <summary>A managed array/string; or null for native ptrs.</summary>
        internal readonly object Object;
        /// <summary>An byte-offset into the array/string; or a native ptr.</summary>
        internal readonly UIntPtr Offset;
        /// <summary>Fetches the number of elements this Span contains.</summary>
        public readonly int Length;

        /// <summary>
        /// Creates a new span over the entirety of the target array.
        /// </summary>
        /// <param name="array">The target array.</param>
        /// <exception cref="System.ArgumentException">
        /// Thrown if the 'array' parameter is null.
        /// </exception>
        public ReadOnlySpan(T[] array)
        {
            Contract.Requires(array != null);
            Object = array;
            Offset = new UIntPtr((uint)SpanHelpers<T>.OffsetToArrayData);
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
        internal ReadOnlySpan(T[] array, int start)
        {
            Contract.Requires(array != null);
            Contract.RequiresInInclusiveRange(start, (uint)array.Length);
            if (start < array.Length)
            {
                Object = array;
                Offset = new UIntPtr(
                    (uint)(SpanHelpers<T>.OffsetToArrayData + (start * PtrUtils.SizeOf<T>())));
                Length = array.Length - start;
            }
            else
            {
                Object = null;
                Offset = UIntPtr.Zero;
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
        public ReadOnlySpan(T[] array, int start, int length)
        {
            Contract.Requires(array != null);
            Contract.RequiresInInclusiveRange(start, (uint)array.Length);
            Contract.RequiresNonNegative(length);
            Contract.RequiresInInclusiveRange(start + length, (uint)array.Length);
            if (start < array.Length)
            {
                Object = array;
                Offset = new UIntPtr(
                    (uint)(SpanHelpers<T>.OffsetToArrayData + (start * PtrUtils.SizeOf<T>())));
                Length = length;
            }
            else
            {
                Object = null;
                Offset = UIntPtr.Zero;
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
        public unsafe ReadOnlySpan(void* ptr, int length)
        {
            Contract.Requires(length >= 0);
            Contract.Requires(length == 0 || ptr != null);
            Object = null;
            Offset = new UIntPtr(ptr);
            Length = length;
        }

        /// <summary>
        /// An internal helper for creating spans. Not for public use.
        /// </summary>
        internal ReadOnlySpan(object obj, UIntPtr offset, int length)
        {
            Object = obj;
            Offset = offset;
            Length = length;
        }

        public static implicit operator ReadOnlySpan<T>(Span<T> slice)
        {
            return new ReadOnlySpan<T>(slice.Object, slice.Offset, slice.Length);
        }

        public static ReadOnlySpan<T> Empty { get { return default(ReadOnlySpan<T>); } }

        public bool IsEmpty
        {
            get { return Length == 0; }
        }

        public unsafe void* UnsafeReadOnlyPointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Offset.ToPointer(); }
        }

        /// <summary>
        /// Fetches the element at the specified index.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                Contract.RequiresInRange(index, (uint)Length);
                return PtrUtils.Get<T>(
                    Object, Offset + (index * PtrUtils.SizeOf<T>()));
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private set
            {
                Contract.RequiresInRange(index, (uint)Length);
                PtrUtils.Set<T>(
                    Object, Offset + (index * PtrUtils.SizeOf<T>()), value);
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

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start'.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        public ReadOnlySpan<T> Slice(int start)
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
        public ReadOnlySpan<T> Slice(int start, int length)
        {
            Contract.Requires(start + length <= Length);
            return new ReadOnlySpan<T>(
                Object, Offset + (start * PtrUtils.SizeOf<T>()), length);
        }

        /// <summary>
        /// Checks to see if two spans point at the same memory.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReferenceEquals(ReadOnlySpan<T> other)
        {
            return Object == other.Object &&
                Offset == other.Offset && Length == other.Length;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Offset.GetHashCode();
                hashCode = hashCode * 31 + Length;
                if (Object != null)
                {
                    hashCode = hashCode * 31 + Object.GetHashCode();
                }
                return hashCode;
            }
        }

        /// <summary>
        /// Checks to see if two spans point at the same memory.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        public bool Equals(ReadOnlySpan<T> other)
        {
            return ReferenceEquals(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is ReadOnlySpan<T>)
            {
                return Equals((ReadOnlySpan<T>)obj);
            }
            return false;
        }

        /// <summary>
        /// Returns item from given index without the boundaries check
        /// use only in places where moving outside the boundaries is impossible
        /// gain: performance: no boundaries check (single operation) 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T GetItemWithoutBoundariesCheck(int index)
        {
            return PtrUtils.Get<T>(
                    Object, Offset + (index * PtrUtils.SizeOf<T>()));
        }
    }
}


