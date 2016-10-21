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

    public abstract class OwnedMemory<T> : IDisposable, IKnown
    {
        const long InitializedId = long.MinValue;
        static long _nextId = InitializedId + 1;

        long _id;
        int _references;

        protected T[] _array;
        protected IntPtr _pointer;
        protected int _length;
        protected int _offset;

        protected OwnedMemory(T[] array, int arrayOffset, IntPtr pointer, int length)
        {
            Contract.Requires(array != null || pointer != IntPtr.Zero);
            _length = length;
            _array = array;
            _pointer = pointer;
            _offset = arrayOffset;
            _id = InitializedId;
            Initialize();
        }

        protected OwnedMemory(T[] array) : this(array, 0, IntPtr.Zero, array.Length)
        {}
     
        public Memory<T> Memory => new Memory<T>(this, _id);
        public Span<T> Span {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get {
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
            if (_references != 0) throw new InvalidOperationException("outstanding references detected.");
            _id = InitializedId;
            DisposeCore();
        }

        public bool IsDisposed => _id == InitializedId;

        public int ReferenceCount => _references;

        public void AddReference(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            Interlocked.Increment(ref _references);
            OnReferenceCountChanged();
        }
        public void ReleaseReference(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            Interlocked.Decrement(ref _references);
            OnReferenceCountChanged();
        }

        protected virtual void OnReferenceCountChanged()
        { }

        public virtual void Initialize()
        {
            if(!IsDisposed) {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }
            _id = Interlocked.Increment(ref _nextId);
            _references = 0;
        }

        public int Length => _length;

        // abstract members
        protected abstract void DisposeCore();

        protected virtual bool TryGetArrayCore(out ArraySegment<T> buffer)
        {
            if(_array == null) {
                buffer = default(ArraySegment<T>);
                return false;
            }
            buffer = new ArraySegment<T>(_array, _offset, _length);
            return true;
        }

        protected virtual unsafe bool TryGetPointerCore(out void* pointer)
        {
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
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
            return TryGetPointerCore(out pointer);
        }

        internal bool TryGetArray(long id, out ArraySegment<T> buffer)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory<T>));
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

        private static void ThrowObjectDisposed()
        {
            throw new ObjectDisposedException(nameof(Memory<T>));
        }
    }
}