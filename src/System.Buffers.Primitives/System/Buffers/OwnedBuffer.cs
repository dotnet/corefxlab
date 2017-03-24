// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
    public abstract class OwnedBuffer<T> : IDisposable, IKnown
    {
        static long _nextId = InitializedId + 1;
        const int InitializedId = int.MinValue;
        const int FreedId = int.MinValue + 1;

        T[] _array;
        int _arrayIndex;
        int _length;

        IntPtr _pointer;

        int _referenceCount;
        int _id;

        public int Length => _length;

        internal int Id => _id;
        protected T[] Array => _array;
        protected IntPtr Pointer => _pointer;
        protected int Offset => _arrayIndex;

        public bool HasOutstandingReferences { 
            get { 
                return _referenceCount != 0 
                        || (ReferenceCountingSettings.OwnedMemory == ReferenceCountingMethod.ReferenceCounter
                            && ReferenceCounter.HasReference(this)); 
            } 
        }

        private OwnedBuffer() { }

        protected OwnedBuffer(T[] array) : this(array, 0, array.Length) { }

        protected OwnedBuffer(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            _id = InitializedId;
            Initialize(array, arrayOffset, length, pointer);
        }

        public Buffer<T> Buffer => new Buffer<T>(this, 0, Length);
        public ReadOnlyBuffer<T> ReadOnlyBuffer => new ReadOnlyBuffer<T>(this, 0, Length);

        public Span<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                var array = Array;
                if (array != null)
                    return new Span<T>(array, _arrayIndex, _length);
                else unsafe {
                    return new Span<T>(Pointer.ToPointer(), Length);
                }
            }
        }

        public static implicit operator OwnedBuffer<T>(T[] array) => new Internal.OwnedArray<T>(array);

        protected bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            if (Array == null) {
                buffer = default(ArraySegment<T>);
                return false;
            }

            buffer = new ArraySegment<T>(Array, Offset, Length);
            return true;
        }

        protected unsafe bool TryGetPointerCore(out void* pointer)
        {
            if (Pointer == IntPtr.Zero) {
                pointer = null;
                return false;
            }

            pointer = Pointer.ToPointer();
            return true;
        }

        #region Lifetime Management
        protected void Initialize(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            Contract.Requires(array != null || pointer != IntPtr.Zero);
            Contract.Requires(array == null || arrayOffset + length <= array.Length);
            if (!IsDisposed && Id!=InitializedId) {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }

            _id = (int)Interlocked.Increment(ref _nextId);
            _array = array;
            _arrayIndex = arrayOffset;
            _length = length;
            _pointer = pointer;
            _referenceCount = 0;
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _id,  FreedId);
            if (HasOutstandingReferences) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        { 
            _id = FreedId;
            _array = null;
            _pointer = IntPtr.Zero;
            _length = 0;
            _arrayIndex = 0;
        }

        public bool IsDisposed => Id == FreedId;

        public void AddReference()
        {
            Interlocked.Increment(ref _referenceCount);
        }

        public void Release()
        {
            if(Interlocked.Decrement(ref _referenceCount) == 0)
                OnZeroReferences();
        }

        protected virtual void OnZeroReferences()
        { }
        #endregion

        #region Used by Memory<T>
        void IKnown.AddReference()
        {
            AddReference();
        }

        void IKnown.Release()
        {
            Release();
        }

        internal unsafe bool TryGetPointerInternal(out void* pointer)
        {
            return TryGetPointerCore(out pointer);
        }

        internal bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            return TryGetArrayCore(out buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<T> GetSpanInternal(int index, int length)
        {
            if (IsDisposed) ThrowIdHelper();

            var array = Array;
            if (array != null) 
            {
                return new Span<T>(array, Offset + index, length);
            }
            else
                unsafe {
                    if ((uint)index > (uint)Length || (uint)length > (uint)(Length - index))
                        ThrowArgHelper();
                    IntPtr newPtr = Add(Pointer, index);
                    return new Span<T>(newPtr.ToPointer(), length);
                }
        }

        // TODO: this is taken from SpanHelpers. If we move this type to System.Memory, we should remove this helper
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static IntPtr Add(IntPtr start, int index)
        {
            Debug.Assert(start.ToInt64() >= 0);
            Debug.Assert(index >= 0);

            unsafe
            {
                if (sizeof(IntPtr) == sizeof(int)) {
                    // 32-bit path.
                    uint byteLength = (uint)index * (uint)Unsafe.SizeOf<T>();
                    return (IntPtr)(((byte*)start) + byteLength);
                }
                else {
                    // 64-bit path.
                    ulong byteLength = (ulong)index * (ulong)Unsafe.SizeOf<T>();
                    return (IntPtr)(((byte*)start) + byteLength);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void VerifyId(long id) {
            if (Id != id) ThrowIdHelper();
        }

        void ThrowIdHelper() {
            throw new ObjectDisposedException(nameof(Buffer<T>));
        }
        void ThrowArgHelper()
        {
            throw new ArgumentOutOfRangeException();
        }
        #endregion

        public static OwnedBuffer<T> Create(ArraySegment<T> segment)
        {
            return new Internal.OwnedArray<T>(segment);
        }
    }

    interface IKnown
    {
        void AddReference();
        void Release();
    }

    public struct DisposableReservation<T> : IDisposable
    {
        OwnedBuffer<T> _owner;

        internal DisposableReservation(OwnedBuffer<T> owner)
        {
            _owner = owner;
            switch(ReferenceCountingSettings.OwnedMemory) {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).AddReference();
                    break;
                case ReferenceCountingMethod.ReferenceCounter:
                    ReferenceCounter.AddReference(_owner);
                    break;
                case ReferenceCountingMethod.None:
                    break;
            }
        }

        public Span<T> Span => _owner.Span;

        public void Dispose()
        {
            switch (ReferenceCountingSettings.OwnedMemory) {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).Release();
                    break;
                case ReferenceCountingMethod.ReferenceCounter:
                    ReferenceCounter.Release(_owner);
                    break;
                case ReferenceCountingMethod.None:
                    break;
            }
            _owner = null;
        }
    }
}

namespace System.Runtime
{
    public enum ReferenceCountingMethod {
        Interlocked,
        ReferenceCounter,
        None
    };

    public class ReferenceCountingSettings {
        public static ReferenceCountingMethod OwnedMemory = ReferenceCountingMethod.Interlocked;
    }
}