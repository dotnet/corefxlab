using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
    public interface IReferenceCounted<T>
    {
        int AddReference(long versionId, int reservationId);
        void Release(long versionId, int reservationId);
    }

    public interface IAllowsDependencyMemory<T> : IReferenceCounted<T>
    {
        void ThrowIfDisposed(long versionId, int reservationId);
        bool IsDependancyDisposed(long versionId, int reservationId);
    }

    public interface IReadOnlyMemory<T> : IAllowsDependencyMemory<T>
    {
        ReservedReadOnlyMemory<T> Reserve(ref ReadOnlyMemory<T> memory, long versionId, int reservationId);
        ReadOnlySpan<T> GetSpan(long versionId, int reservationId);
    }

    public interface IMemory<T> : IReadOnlyMemory<T>
    {
        ReservedMemory<T> Reserve(ref Memory<T> memory, long versionId, int reservationId);
        new Span<T> GetSpan(long versionId, int reservationId);
        bool TryGetDependancyArray(long _versionId, int _reservationId, out ArraySegment<T> buffer);
        unsafe bool TryGetDependancyPointer(long versionId, int reservationId, out void* pointer);
    }

    public abstract class OwnedMemory<T> : IDisposable, IMemory<T>
    {
        const int BaseReservationId = 0;
        const long InitializedId = long.MinValue;
        static long _nextId = InitializedId + 1;
        static readonly uint[] EmptyReferences = new uint[0];

        long _versionId;
        int _lastReference;
        int _references;
        uint[] _outstandingReferences = EmptyReferences;

        public int Length { get; private set; }
        protected T[] Array { get; private set; }
        protected IntPtr Pointer { get; private set; }
        protected int Offset { get; private set; }
        public int ReferenceCount => _references;

        private OwnedMemory() { }

        protected OwnedMemory(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            _versionId = InitializedId;
            Initialize(array, arrayOffset, length, pointer);
        }

        public Memory<T> Memory
        {
            get
            {
                ThrowIfDisposed();
                return new Memory<T>(this, _versionId, BaseReservationId);
            }
        }

        public Span<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                ThrowIfDisposed();
                return GetSpanCore();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Span<T> GetSpanCore()
        {
            if (Array != null)
                return Array.Slice(Offset, Length);
            else unsafe
                {
                    return new Span<T>(Pointer.ToPointer(), Length);
                }
        }

        public static implicit operator OwnedMemory<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

        protected bool TryGetArray(out ArraySegment<T> buffer)
        {
            ThrowIfDisposed();
            return TryGetArrayCore(out buffer);
        }

        private bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            if (Array == null)
            {
                buffer = default(ArraySegment<T>);
                return false;
            }

            buffer = new ArraySegment<T>(Array, Offset, Length);
            return true;
        }

        protected unsafe bool TryGetPointer(out void* pointer)
        {
            ThrowIfDisposed();
            return TryGetPointerCore(out pointer);
        }

        private unsafe bool TryGetPointerCore(out void* pointer)
        {
            if (Pointer == IntPtr.Zero)
            {
                pointer = null;
                return false;
            }

            pointer = Pointer.ToPointer();
            return true;
        }

        protected void Initialize(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            Contract.Requires(array != null || pointer != null);
            Contract.Requires(array == null || arrayOffset + length <= array.Length);
            if (!IsDisposed) {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }

            _versionId = Interlocked.Increment(ref _nextId);
            Array = array;
            Offset = arrayOffset;
            Length = length;
            Pointer = pointer;
            _references = 1;
            _lastReference = 0;

            if (_outstandingReferences.Length == 0)
            {
                _outstandingReferences = new uint[1];
            }
            _outstandingReferences[0] = 1;
        }

        public void Dispose()
        {
            var refCount = DisposeReservation(BaseReservationId);
            if (refCount == 0)
            {
                DisposeCore(refCount);
            }
        }

        private void DisposeCore(int references)
        {
            Dispose(true);

            _versionId = InitializedId;
            Array = null;
            Pointer = IntPtr.Zero;
            Length = 0;
            Offset = 0;
            if (_outstandingReferences.Length > 1)
            {
                // Release memory for large reservations
                _outstandingReferences = EmptyReferences;
            }

            DisposeComplete();
        }

        /// <summary>
        /// Override this to dispose of native data or pool the referenced data.
        /// Use <seealso cref="DisposeComplete"/> to pool the object to avoid race conditions with its data being cleared.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        { }

        /// <summary>
        /// Override this if you need notification of when pooling the <seealso cref="OwnedMemory{T}"/> itself is safe. 
        /// Its references will be cleared by this point, so use <seealso cref="Dispose(bool)"/> if you want to pool its data.
        /// </summary>
        protected virtual void DisposeComplete()
        { }

        public bool IsDisposed => _versionId == InitializedId;

        bool IAllowsDependencyMemory<T>.IsDependancyDisposed(long versionId, int reservationId)
        {
            return (_versionId != versionId || IsReservationDisposed(reservationId));
        }

        private bool IsReservationDisposed(int reservationId)
        {
            var index = reservationId >> 5; // divide 32
            var offset = (uint)(1 << (reservationId & 31));

            return (_outstandingReferences[index] & offset) == 0;
        }

        private int DisposeReservation(int reservationId)
        {
            var index = reservationId >> 5; // divide 32
            var offset = (uint)(1 << (reservationId & 31));
            var current = _outstandingReferences[index];

            if ((current & offset) != 0) {
                _outstandingReferences[index] = current & ~offset;
                return Interlocked.Decrement(ref _references);
            }
            return -1;
        }

        ReservedMemory<T> IMemory<T>.Reserve(ref Memory<T> memory, long versionId, int reservationId)
            => Reserve(ref memory, versionId, reservationId);

        protected internal virtual ReservedMemory<T> Reserve(ref Memory<T> memory, long versionId, int reservationId)
        {
            return new ReservedMemory<T>(ref memory, this, versionId, reservationId);
        }

        ReservedReadOnlyMemory<T> IReadOnlyMemory<T>.Reserve(ref ReadOnlyMemory<T> memory, long versionId, int reservationId)
            => Reserve(ref memory, versionId, reservationId);

        protected internal virtual ReservedReadOnlyMemory<T> Reserve(ref ReadOnlyMemory<T> memory, long versionId, int reservationId)
        {
            return new ReservedReadOnlyMemory<T>(ref memory, this, versionId, reservationId);
        }

        int IReferenceCounted<T>.AddReference(long versionId, int reservationId)
        {
            ThrowIfDisposed(versionId, reservationId);
            Interlocked.Increment(ref _references);
            var newReservationId = Interlocked.Increment(ref _lastReference);
            var index = newReservationId >> 5; // divide 32
            var offset = (uint)(1 << (newReservationId & 31));

            if (index >= _outstandingReferences.Length)
            {
                var refs = new uint[index + 1];
                _outstandingReferences.CopyTo(refs, 0);
                _outstandingReferences = refs;
            }

            _outstandingReferences[index] |= offset;
            return newReservationId;
        }

        void IReferenceCounted<T>.Release(long versionId, int reservationId)
        {
            if (!IsActive(versionId, reservationId)) return;

            var refCount = DisposeReservation(reservationId);
            if (refCount == 0)
            {
                DisposeCore(refCount);
            }
        }

        unsafe bool IMemory<T>.TryGetDependancyPointer(long versionId, int reservationId, out void* pointer)
        {
            ThrowIfDisposed(versionId, reservationId);
            return TryGetPointerCore(out pointer);
        }

        bool IMemory<T>.TryGetDependancyArray(long versionId, int reservationId, out ArraySegment<T> buffer)
        {
            ThrowIfDisposed(versionId, reservationId);
            return TryGetArrayCore(out buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Span<T> IMemory<T>.GetSpan(long versionId, int reservationId)
        {
            ThrowIfDisposed(versionId, reservationId);
            return GetSpanCore();
        }

        ReadOnlySpan<T> IReadOnlyMemory<T>.GetSpan(long versionId, int reservationId)
        {
            ThrowIfDisposed(versionId, reservationId);
            return GetSpanCore();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsActive(long versionId, int reservationId)
        {
            return !(_versionId != versionId || IsReservationDisposed(reservationId));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsActive()
        {
            return !(IsDisposed || IsReservationDisposed(BaseReservationId));
        }

        void IAllowsDependencyMemory<T>.ThrowIfDisposed(long versionId, int reservationId)
            => ThrowIfDisposed(versionId, reservationId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ThrowIfDisposed(long versionId, int reservationId) {
            if (!IsActive(versionId, reservationId)) ThrowObjectDisposedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfDisposed()
        {
            if (!IsActive()) ThrowObjectDisposedException();
        }

        void ThrowObjectDisposedException() {
            throw new ObjectDisposedException(nameof(Memory<T>));
        }
    }
}