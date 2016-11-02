using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
    public abstract class OwnedMemory<T> : IDisposable, IMemory<T>
    {
        static long _nextId = InitializedId + 1;
        const long InitializedId = long.MinValue;
        int _referenceCount;

        public int Length { get; private set; }
        protected long Id { get; private set; }
        protected T[] Array { get; private set; }
        protected IntPtr Pointer { get; private set; }
        protected int Offset { get; private set; }
        public int ReferenceCount { get { return _referenceCount; } }

        private OwnedMemory() { }

        protected OwnedMemory(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            Id = InitializedId;
            Initialize(array, arrayOffset, length, pointer);
        }

        public Memory<T> Memory => new Memory<T>(this, Id);
        public Span<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
                if (Array != null)
                    return Array.Slice(Offset, Length);
                else unsafe {
                    return new Span<T>(Pointer.ToPointer(), Length);
                }
            }
        }

        public static implicit operator OwnedMemory<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

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
            Contract.Requires(array != null || pointer != null);
            Contract.Requires(array == null || arrayOffset + length <= array.Length);
            if (!IsDisposed) {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }

            Id = Interlocked.Increment(ref _nextId);
            Array = array;
            Offset = arrayOffset;
            Length = length;
            Pointer = pointer;
            _referenceCount = 0;
        }

        public void Dispose()
        {
            if (ReferenceCount != 0) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
            Id = InitializedId;
            Array = null;
            Pointer = IntPtr.Zero;
            Length = 0;
            Offset = 0;
        }

        protected virtual void Dispose(bool disposing)
        { }

        public bool IsDisposed => Id == InitializedId;

        public void AddReference()
        {
            OnReferenceCountChanged(Interlocked.Increment(ref _referenceCount));
        }

        public void Release()
        {
            OnReferenceCountChanged(Interlocked.Decrement(ref _referenceCount));
        }

        protected virtual void OnReferenceCountChanged(int newReferenceCount)
        { }

        DisposableReservation IReadOnlyMemory<T>.Reserve(ref ReadOnlyMemory<T> memory)
        {
            return ReserveCore(ref memory);
        }

        protected virtual DisposableReservation ReserveCore(ref ReadOnlyMemory<T> memory)
        {
            return new DisposableReservation(this, Id);
        }
        #endregion

        #region Used by Memory<T>
        void IReferenceCounted.AddReference(long id)
        {
            VerifyId(id);
            AddReference();
        }

        void IReferenceCounted.Release(long id)
        {
            VerifyId(id);
            Release();
        }

        internal unsafe bool TryGetPointerInternal(long id, out void* pointer)
        {
            VerifyId(id);
            return TryGetPointerCore(out pointer);
        }

        unsafe bool IReadOnlyMemory<T>.TryGetPointer(long id, out void* pointer)
        {
            return TryGetPointerInternal(id, out pointer);
        }

        internal bool TryGetArrayInternal(long id, out ArraySegment<T> buffer)
        {
            VerifyId(id);
            return TryGetArrayCore(out buffer);
        }

        bool IMemory<T>.TryGetArray(long id, out ArraySegment<T> buffer)
        {
            return TryGetArrayInternal(id, out buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<T> GetSpanInternal(long id)
        {
            VerifyId(id);
            return Span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Span<T> IMemory<T>.GetSpan(long id)
        {
            return GetSpanInternal(id);
        }
        ReadOnlySpan<T> IReadOnlyMemory<T>.GetSpan(long id)
        {
            return GetSpanInternal(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void VerifyId(long id) {
            if (Id != id) ThrowIdHelper();
        }

        void ThrowIdHelper() {
            throw new ObjectDisposedException(nameof(Memory<T>));
        }
        #endregion
    }

    public interface IReferenceCounted
    {
        void AddReference(long id);
        void Release(long id);
    }
    public interface IReadOnlyMemory<T> : IReferenceCounted
    {
        ReadOnlySpan<T> GetSpan(long id);

        unsafe bool TryGetPointer(long id, out void* pointer);

        DisposableReservation Reserve(ref ReadOnlyMemory<T> memory);
    }
    public interface IMemory<T> : IReadOnlyMemory<T>
    {
        new Span<T> GetSpan(long id);

        bool TryGetArray(long id, out ArraySegment<T> buffer);
    }

    public struct DisposableReservation : IDisposable
    {
        IReferenceCounted _owner;
        long _id;

        public DisposableReservation(IReferenceCounted owner, long id)
        {
            _id = id;
            _owner = owner;
            _owner.AddReference(_id);
        }

        public void Dispose()
        {
            _owner.Release(_id);
            _owner = null;
        }
    }
}