using System.Threading;

namespace System.Buffers
{
    public abstract class OwnedMemory<T> : IDisposable, IKnown
    {
        long _id;
        int _references;

        protected T[] _array;
        protected IntPtr _pointer;
        protected int _length;
        protected int _offset;

        static long _nextId = InitializedId + 1;
        const long InitializedId = long.MinValue;
        
        private OwnedMemory() { }

        protected OwnedMemory(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            Contract.Requires(array != null || pointer != null);
            Contract.Requires(array == null || arrayOffset + length <= array.Length);

            _array = array;
            _offset = arrayOffset;
            _length = length;
            _pointer = pointer;
            _id = InitializedId;
            Initialize();
        }

        public Memory<T> Memory => new Memory<T>(this, _id);
        public Span<T> Span
        {
            get {
                if (_array != null)
                    return _array.Slice(_offset, _length);
                else unsafe
                    {
                        return new Span<T>(_pointer.ToPointer(), _length);
                    }
            }
        }

        public int Length => _length;
        public long Id => _id;

        public static implicit operator OwnedMemory<T>(T[] array)
        {
            return new OwnedArray<T>(array);
        }

        #region Lifetime Management
        public void Dispose()
        {
            if (_references != 0) throw new InvalidOperationException("outstanding references detected.");
            _id = InitializedId;
            DisposeCore();
        }

        public bool IsDisposed => _id == InitializedId;

        public int ReferenceCount => _references;

        public void AddReference(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            OnReferenceCountChanged(Interlocked.Increment(ref _references));
        }

        public void ReleaseReference(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            OnReferenceCountChanged(Interlocked.Decrement(ref _references));
        }

        protected virtual void OnReferenceCountChanged(int newReferenceCount)
        { }

        protected internal virtual DisposableReservation Reserve(ref ReadOnlyMemory<T> memory)
        {
            return new DisposableReservation(this, Id);
        }

        public virtual void Initialize()
        {
            if(!IsDisposed) {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }
            _id = Interlocked.Increment(ref _nextId);
            _references = 0;
        }

        // abstract members
        protected virtual void DisposeCore()
        {
            _array = null;
            _pointer = IntPtr.Zero;
            _length = 0;
            _offset = 0;
        }
        #endregion

        protected bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            if(_array == null) {
                buffer = default(ArraySegment<T>);
                return false;
            }

            buffer = new ArraySegment<T>(_array, _offset, _length);
            return true;
        }

        protected unsafe bool TryGetPointerCore(out void* pointer)
        {
            if(_pointer == IntPtr.Zero) {
                pointer = null;
                return false;
            }

            pointer = _pointer.ToPointer();
            return true;
        }

        // used by Memory<T>
        internal unsafe bool TryGetPointer(long id, out void* pointer)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            return TryGetPointerCore(out pointer);
        }

        internal bool TryGetArray(long id, out ArraySegment<T> buffer)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            return TryGetArrayCore(out buffer);
        }

        internal Span<T> GetSpan(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            return Span;
        }
    }

    /// <summary>
    /// Known only to some by its secret name
    /// </summary>
    internal interface IKnown
    {
        void AddReference(long secretId);
        void ReleaseReference(long secretId);
    }

    public struct DisposableReservation : IDisposable
    {
        IKnown _owner;
        long _id;

        internal DisposableReservation(IKnown owner, long id)
        {
            _id = id;
            _owner = owner;
            _owner.AddReference(_id);
        }

        public void Dispose()
        {
            _owner.ReleaseReference(_id);
            _owner = null;
        }
    }
}