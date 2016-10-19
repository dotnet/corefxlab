using System.Threading;

namespace System.Buffers
{
    public abstract class MemoryManager<T> : IDisposable
    {
        long _id;
        int _references;

        static long _nextId = InitializedId + 1;

        const long InitializedId = long.MinValue;
        
        public Memory2<T> Memory => new Memory2<T>(this, _id);

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
            if (_id != id) throw new ObjectDisposedException(nameof(Memory2<T>));
            Interlocked.Increment(ref _references);
        }
        public void ReleaseReference(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory2<T>));
            Interlocked.Decrement(ref _references);
        }

        public virtual void Initialize()
        {
            if(!IsDisposed) {
                throw new InvalidOperationException("manager has to be disposed to initialize");
            }
            _id = Interlocked.Increment(ref _nextId);
            _references = 0;
        }

        // abstract members
        protected abstract Span<T> GetSpanCore();
        protected abstract void DisposeCore();

        protected abstract bool TryGetArrayCore(out ArraySegment<T> buffer);

        protected abstract unsafe bool TryGetPointerCore(out void* pointer);

        // used by Memory<T>
        internal unsafe bool TryGetPointer(long id, out void* pointer)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory2<T>));
            return TryGetPointerCore(out pointer);
        }

        internal bool TryGetArray(long id, out ArraySegment<T> buffer)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory2<T>));
            return TryGetArrayCore(out buffer);
        }

        internal Span<T> GetSpan(long id)
        {
            if (_id != id) throw new ObjectDisposedException(nameof(Memory2<T>));
            return GetSpanCore();
        }

        // protected
        protected MemoryManager()
        {
            _id = InitializedId;
            Initialize();
        }
    }
}