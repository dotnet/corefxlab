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
        long _id;
        int _references;

        static long _nextId = InitializedId + 1;

        const long InitializedId = long.MinValue;
        
        public Memory<T> Memory => new Memory<T>(this, _id);
        public Span<T> Span => GetSpanCore();

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

        public virtual int Length => Memory.Length;

        // abstract members
        protected abstract Span<T> GetSpanCore();
        protected abstract void DisposeCore();

        protected abstract bool TryGetArrayCore(out ArraySegment<T> buffer);

        protected abstract unsafe bool TryGetPointerCore(out void* pointer);

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
            return GetSpanCore();
        }

        // protected
        protected OwnedMemory()
        {
            _id = InitializedId;
            Initialize();
        }
    }
}