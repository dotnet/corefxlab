using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
    /// <summary>
    /// Known only to some by its secret name
    /// </summary>
    internal interface IKnown
    {
        void AddReference(long secretId);
        void ReleaseReference(long secretId);
    }

    // Exposed for equality test on pool return; rather than as cast test
    // also enforces return is to correct pool
    public interface IMemoryOwner
    {
    }

    // Internal reference held in OwnedMemory<T>, passed out as IMemoryOwner
    public interface IMemoryDisposer<T> : IMemoryOwner
    {
        void Return(OwnedMemory<T> buffer);
    }

    // Implemented by pool, return implemented explicit as `IMemoryDisposer.Return`
    public interface IBufferPool<T> : IMemoryDisposer<T>, IDisposable
    {
        OwnedMemory<T> Rent(int minimumBufferSize);
    }

    public abstract class OwnedMemory<T> : IDisposable, IKnown
    {
        const long InitializedId = long.MinValue;
        static long _nextId = InitializedId + 1;

        private long _id;
        private int _references;

        private T[] _array;
        private IntPtr _pointer;
        private int _length;
        private int _offset;

        private readonly IMemoryDisposer<T> _owner;
        public IMemoryOwner Owner => _owner;

        protected OwnedMemory(IMemoryDisposer<T> owner = null)
        {
            _owner = owner;
            _id = InitializedId;
        }

        protected OwnedMemory(T[] array, IMemoryDisposer <T> owner = null)
            : this(array, 0, IntPtr.Zero, array.Length, owner)
        {}

        protected OwnedMemory(T[] array, int arrayOffset, IntPtr pointer, int length, IMemoryDisposer<T> owner = null)
            : this(owner)
        {
            ReInitialize(array, arrayOffset, pointer, length);
        }

        protected OwnedMemory(ArraySegment<T> segment, IMemoryDisposer<T> owner = null)
            : this(segment, IntPtr.Zero, owner)
        {}

        protected OwnedMemory(ArraySegment<T> segment, IntPtr pointer, IMemoryDisposer<T> owner = null)
            : this(owner)
        {
            ReInitialize(segment.Array, segment.Offset, pointer, segment.Count);
        }

        protected void ReInitialize(ArraySegment<T> segment, IntPtr pointer)
            => ReInitialize(segment.Array, segment.Offset, pointer, segment.Count);

        protected void ReInitialize(ArraySegment<T> segment)
            => ReInitialize(segment, IntPtr.Zero);

        protected void ReInitialize(T[] array, int arrayOffset, IntPtr pointer, int length)
        {
            if (!IsDisposed)
            {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }

            Contract.Requires(array != null || pointer != IntPtr.Zero);

            _id = Interlocked.Increment(ref _nextId);
            _references = 1;


            _length = length;
            _array = array;
            _pointer = pointer;
            _offset = arrayOffset;
        }

        public Memory<T> Memory
        {
            get
            {
                if (IsDisposed) ThrowObjectDisposed();

                return new Memory<T>(this, _id);
            }
        }

        public Span<T> Span {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (IsDisposed) ThrowObjectDisposed();

                if (_pointer != IntPtr.Zero) {
                    unsafe {
                        return new Span<T>(_pointer.ToPointer(), _length);
                    }
                }
                else
                {
                    return new Span<T>(_array, _offset, _length);
                }
            }
        }

        public static implicit operator OwnedMemory<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        ~OwnedMemory()
        {
            Dispose(false);
        }

        internal virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ThrowIfDisposed();

                var count = Interlocked.Decrement(ref _references);
                OnReferenceCountChanged(count);
                if (count == 0)
                {
                    _owner?.Return(this);

                    DisposeCore();

                    Clear();

                    _id = InitializedId;

                    GC.SuppressFinalize(this);
                }
                else if (count < 0)
                {
                    throw new InvalidOperationException("Owned Memory over disposed");
                }
            }
            else
            {
                Clear();

                throw new InvalidOperationException("Owned Memory not fully disposed");
            }
        }

        private void Clear()
        {
            _array = null;

            _length = 0;
            _offset = 0;

            _pointer = IntPtr.Zero;
        }

        public bool IsDisposed => _id == InitializedId;

        public int ReferenceCount => _references;

        public void AddReference(long id)
        {
            if (_id != id || IsDisposed) ThrowObjectDisposed();

            var count = Interlocked.Increment(ref _references);
            OnReferenceCountChanged(count);
        }
        public void ReleaseReference(long id)
        {
            if (_id != id || IsDisposed) ThrowObjectDisposed();

            Dispose();
        }

        protected virtual void OnReferenceCountChanged(int count)
        { }

        public int Length => _length;

        // abstract members
        protected abstract void DisposeCore();

        protected bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            ThrowIfDisposed();

            if (_array == null) {
                buffer = default(ArraySegment<T>);
                return false;
            }
            buffer = new ArraySegment<T>(_array, _offset, _length);
            return true;
        }

        protected unsafe bool TryGetPointerCore(out void* pointer)
        {
            ThrowIfDisposed();

            if (_pointer == IntPtr.Zero) {
                pointer = null;
                return false;
            }
            pointer = _pointer.ToPointer();
            return true;
        }

        // used by Memory<T>
        internal unsafe bool TryGetPointer(long id, out void* pointer)
        {
            if (_id != id) ThrowObjectDisposed();

            return TryGetPointerCore(out pointer);
        }

        internal bool TryGetArray(long id, out ArraySegment<T> buffer)
        {
            if (_id != id) ThrowObjectDisposed();

            return TryGetArrayCore(out buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<T> GetSpan(long id)
        {
            if (_id != id) {
                ThrowObjectDisposed();
            }
            return Span;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                ThrowObjectDisposed();
            }
        }

        private static void ThrowObjectDisposed()
        {
            throw new ObjectDisposedException(nameof(Memory<T>));
        }
    }
}