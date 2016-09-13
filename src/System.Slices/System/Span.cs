// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System
{
    /// <summary>
    /// Span is a uniform API for dealing with arrays and subarrays, strings
    /// and substrings, and unmanaged memory buffers.  It adds minimal overhead
    /// to regular accesses and is a struct so that creation and subslicing do
    /// not require additional allocations.  It is type- and memory-safe.
    /// </summary>
    [DebuggerTypeProxy(typeof(SpanDebuggerView<>))]
    [DebuggerDisplay("Length = {Length}")]
    public partial struct Span<T> : IEnumerable<T>, IEquatable<Span<T>>, IEquatable<ReadOnlySpan<T>>, IEquatable<T[]>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array)
        {
            Contract.Requires(array != null);
            Contract.Requires(default(T) != null || array.GetType() == typeof(T[]));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span(T[] array, int start)
        {
            Contract.Requires(array != null);
            Contract.Requires(default(T) != null || array.GetType() == typeof(T[]));
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span(T[] array, int start, int length)
        {
            Contract.Requires(array != null);
            Contract.Requires(default(T) != null || array.GetType() == typeof(T[]));
            Contract.RequiresInInclusiveRange(start, length, (uint)array.Length);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe Span(void* ptr, int length)
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
        internal Span(object obj, UIntPtr offset, int length)
        {
            Object = obj;
            Offset = offset;
            Length = length;
        }

        public static implicit operator Span<T>(T[] array)
        {
            return new Span<T>(array);
        }

        public static Span<T> Empty { get { return default(Span<T>); } }

        public bool IsEmpty
        {
            get { return Length == 0; }
        }

        /// <summary>
        /// Gets array if the slice is over an array, otherwise gets a pointer to memory.
        /// </summary>
        /// <returns>true if it's a span over an array; otherwise false (if over a pointer)</returns>
        /// <remarks>This method can be used for interop, while we are waiting for proper pinning support</remarks>
        public unsafe bool TryGetArrayElseGetPointer(out ArraySegment<T> array, out void* pointer)
        {
            var a = Object as T[];
            if (a == null)
            {
                array = new ArraySegment<T>();
                pointer = PtrUtils.ComputeAddress(Object, Offset).ToPointer();
                return false;
            }

            var offsetToData = SpanHelpers<T>.OffsetToArrayData;
            var index = (int)((Offset.ToUInt32() - offsetToData) / PtrUtils.SizeOf<T>());
            array = new ArraySegment<T>(a, index, Length);
            pointer = null;
            return true;
        }

        public unsafe void* UnsafePointer
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return PtrUtils.ComputeAddress(Object, Offset).ToPointer(); }
        }

        [Obsolete("use TryGetArrayElseGetPointer instead")]
        public unsafe bool TryGetArray(void* dummy, out ArraySegment<T> array)
        {
            var a = Object as T[];
            if (a == null)
            {
                array = new ArraySegment<T>();
                return false;
            }

            var offsetToData = SpanHelpers<T>.OffsetToArrayData;

            var index = (int)((Offset.ToUInt32() - offsetToData) / PtrUtils.SizeOf<T>());
            array = new ArraySegment<T>(a, index, Length);
            return true;
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
                return PtrUtils.Get<T>(Object, Offset, (UIntPtr)index);
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                Contract.RequiresInRange(index, (uint)Length);
                PtrUtils.Set(Object, Offset, (UIntPtr)index, value);
            }
        }

        /// <summary>
        /// Copies the contents of this span into a new array.  This heap
        /// allocates, so should generally be avoided, however is sometimes
        /// necessary to bridge the gap with APIs written in terms of arrays.
        /// </summary>
        public T[] ToArray()
        {
            var dest = new T[Length];
            TryCopyTo(dest);
            return dest;
        }

        /// <summary>
        /// Copies the contents of this span into another.  The destination
        /// must be at least as big as the source, and may be bigger.
        /// </summary>
        /// <param name="destination">The span to copy items into.</param>
        public bool TryCopyTo(Span<T> destination)
        {
            // There are some benefits of making local copies. See https://github.com/dotnet/coreclr/issues/5556
            var dest = destination;
            var src = this;
            
            if (src.Length > dest.Length)
                return false;

            if (default(T) != null && MemoryUtils.IsPrimitiveValueType<T>())
            {
                PtrUtils.CopyBlock(src.Object, src.Offset, dest.Object, dest.Offset,
                                   src.Length * PtrUtils.SizeOf<T>());
            }
            else
            {
                for (int i = 0; i < src.Length; i++)
                {
                    // We don't check bounds here as we are surely within them
                    T value = PtrUtils.Get<T>(src.Object, src.Offset, (UIntPtr)i);
                    PtrUtils.Set(dest.Object, dest.Offset, (UIntPtr)i, value);
                }
            }

            return true;
        }

        /// <summary>
        /// Copies the contents of this span into an array.  The destination
        /// must be at least as big as the source, and may be bigger.
        /// </summary>
        /// <param name="dest">The span to copy items into.</param>
        public bool TryCopyTo(T[] destination)
        {
            return TryCopyTo(destination);
        }

        public void Set(ReadOnlySpan<T> values)
        {
            if (!values.TryCopyTo(this))
            {
                throw new ArgumentOutOfRangeException("values");
            }
        }

        public void Set(T[] values)
        {
            if (!values.Slice().TryCopyTo(this))
            {
                throw new ArgumentOutOfRangeException("values");
            }
        }

        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start'.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start)
        {
            Contract.RequiresInInclusiveRange(start, (uint)Length);
            return new Span<T>(Object, Offset + (start * PtrUtils.SizeOf<T>()), Length - start);
        }
        
        /// <summary>
        /// Forms a slice out of the given span, beginning at 'start'.
        /// </summary>
        /// <param name="start">The index at which to begin this slice.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Thrown when the specified start index is not in range (&lt;0 or &gt;&eq;length).
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(uint start)
        {
            Contract.RequiresInInclusiveRange(start, (uint)Length);
            return new Span<T>(Object, Offset + (((int)start) * PtrUtils.SizeOf<T>()), Length - (int)start);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(int start, int length)
        {
            Contract.RequiresInInclusiveRange(start, length, (uint)Length);
            return new Span<T>(
                Object, Offset + (start * PtrUtils.SizeOf<T>()), length);
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<T> Slice(uint start, uint length)
        {
            Contract.RequiresInInclusiveRange(start, length, (uint)Length);
            return new Span<T>(
                Object, Offset + (((int)start) * PtrUtils.SizeOf<T>()), (int)length);
        }

        /// <summary>
        /// Checks to see if two spans point at the same memory.  Note that
        /// this does *not* check to see if the *contents* are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ReferenceEquals(Span<T> other)
        {
            return Object == other.Object &&
                Offset == other.Offset && Length == other.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool StructuralEquals(object obj, UIntPtr offset, int length)
        {
            return Object == obj && Offset == offset && Length == length;
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
        public bool Equals(Span<T> other)
        {
            return ReferenceEquals(other);
        }


        public override bool Equals(object obj)
        {
            if (obj is Span<T>)
            {
                return Equals((Span<T>)obj);
            }
            return false;
        }

        public bool Equals(ReadOnlySpan<T> other) => StructuralEquals(other.Object, other.Offset, other.Length);
        public bool Equals(T[] other) => Equals(new Span<T>(other));
        public static bool operator ==(Span<T> span1, Span<T> span2) => span1.Equals(span2);
        public static bool operator !=(Span<T> span1, Span<T> span2) => !span1.Equals(span2);

        public ReadOnlySpan<T>.Enumerator GetEnumerator()
        {
            return new ReadOnlySpan<T>.Enumerator(Object, Offset, Length);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ReadOnlySpan<T>.EnumeratorObject(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new ReadOnlySpan<T>.EnumeratorObject(this);
        }
    }
}