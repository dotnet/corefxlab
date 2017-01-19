using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

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

namespace System.Buffers
{
    public abstract class OwnedMemory<T> : IDisposable, IMemory<T>
    {
        static long _nextId = InitializedId + 1;
        const long InitializedId = long.MinValue;
        const long FreedId = long.MinValue + 1;
        int _referenceCount;

        private long _id;

        public int Length { get; private set; }
        protected long Id { get { return _id; } }
        protected T[] Array { get; private set; }
        protected IntPtr Pointer { get; private set; }
        protected int Offset { get; private set; }
        public bool HasOutstandingReferences { 
            get { 
                return _referenceCount != 0 
                        || (ReferenceCountingSettings.OwnedMemory == ReferenceCountingMethod.ReferenceCounter
                            && ReferenceCounter.HasReference(this)); 
            } 
        }

        private OwnedMemory() { }

        protected OwnedMemory(T[] array) : this(array, 0, array.Length) { }

        protected OwnedMemory(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            _id = InitializedId;
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

        public static implicit operator OwnedMemory<T>(T[] array) => new OwnedArray<T>(array);

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
            if (!IsDisposed && Id!=InitializedId) {
                throw new InvalidOperationException("this instance has to be disposed to initialize");
            }

            _id = Interlocked.Increment(ref _nextId);
            Array = array;
            Offset = arrayOffset;
            Length = length;
            Pointer = pointer;
            _referenceCount = 0;
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _id,  FreedId);
            if (HasOutstandingReferences) throw new InvalidOperationException("outstanding references detected.");
            Dispose(true);
            Array = null;
            Pointer = IntPtr.Zero;
            Length = 0;
            Offset = 0;
        }

        protected virtual void Dispose(bool disposing)
        { }

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
        void IKnown.AddReference(long id)
        {
            AddReference();
            try {
                VerifyId(id);
            } catch (ObjectDisposedException e) {
                Release();
                throw e;
            }
        }

        void IKnown.Release(long id)
        {
            VerifyId(id);
            Release();
        }

        internal unsafe bool TryGetPointerInternal(long id, out void* pointer)
        {
            VerifyId(id);
            return TryGetPointerCore(out pointer);
        }

        unsafe bool IMemory<T>.TryGetPointer(long id, out void* pointer)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void VerifyId(long id) {
            if (Id != id) ThrowIdHelper();
        }

        void ThrowIdHelper() {
            throw new ObjectDisposedException(nameof(Memory<T>));
        }
        #endregion
    }

    interface IKnown
    {
        void AddReference(long id);
        void Release(long id);
    }

    internal interface IMemory<T> : IKnown
    {
        Span<T> GetSpan(long id);

        bool TryGetArray(long id, out ArraySegment<T> buffer);
        unsafe bool TryGetPointer(long id, out void* pointer);
    }

    public struct DisposableReservation<T> : IDisposable
    {
        OwnedMemory<T> _owner;
        long _id;

        internal DisposableReservation(OwnedMemory<T> owner, long id)
        {
            _id = id;
            _owner = owner;
            switch(ReferenceCountingSettings.OwnedMemory) {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).AddReference(_id);
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
                    ((IKnown)_owner).Release(_id);
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