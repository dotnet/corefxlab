using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Buffers
{
    public abstract class OwnedMemory<T> : IDisposable, IKnown
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

        protected int Id => _id;
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

        private OwnedMemory() { }

        protected OwnedMemory(T[] array) : this(array, 0, array.Length) { }

        protected OwnedMemory(T[] array, int arrayOffset, int length, IntPtr pointer = default(IntPtr))
        {
            _id = InitializedId;
            Initialize(array, arrayOffset, length, pointer);
        }

        public Memory<T> Memory => new Memory<T>(this, Id, 0, Length);
        public ReadOnlyMemory<T> ReadOnlyMemory => new ReadOnlyMemory<T>(this, Id, 0, Length);

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

        internal bool TryGetArrayInternal(long id, out ArraySegment<T> buffer)
        {
            VerifyId(id);
            return TryGetArrayCore(out buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Span<T> GetSpanInternal(long id, int index, int length)
        {
            VerifyId(id);
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
            throw new ObjectDisposedException(nameof(Memory<T>));
        }
        void ThrowArgHelper()
        {
            throw new ArgumentOutOfRangeException();
        }
        #endregion
    }

    interface IKnown
    {
        void AddReference(long id);
        void Release(long id);
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