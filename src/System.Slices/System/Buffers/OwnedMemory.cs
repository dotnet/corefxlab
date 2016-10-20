using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

    public interface IMemoryOwner
    {
    }

    public interface IMemoryDisposer<T> : IMemoryOwner
    {
        void Return(OwnedMemory<T> buffer);
    }

    public enum MemoryType
    {
        Array,
        Native,
        Pinned
    }

    public class OwnedMemory<T> : IDisposable, IKnown
    {
        public readonly static OwnedMemory<T> Empty = OwnerEmptyMemory.Shared;

        const long InitializedId = long.MinValue;
        static long _nextId = InitializedId + 1;

        private readonly IMemoryDisposer<T> _owner;
        private long _id;
        private int _references;

        private T[] _array;
        private IntPtr _memory;
        private int _length;
        private GCHandle _handle;

        [Flags]
        private enum SetData
        {
            None = 0,
            Array = 1,
            GcHandle = 2,
            HGlobal = 4
        }

        private SetData _setData;

        private OwnedMemory()
        {
            _setData = SetData.None;
            _id = Interlocked.Increment(ref _nextId);
            _references = 1;
        }

        public OwnedMemory(int length)
            : this()
        {
            AllocateArray(length);
        }

        public OwnedMemory(T[] array)
            : this()
        {
            Contract.Requires(array != null);

            _array = array;
        }

        public OwnedMemory(T[] array, IMemoryDisposer<T> owner)
            : this(array)
        {
            _owner = owner;
        }

        public unsafe OwnedMemory(T[] array, void* pointer, GCHandle handle = default(GCHandle))
            : this()
        {
            Contract.Requires(array != null);
            Contract.Requires(pointer != null);
            Contract.RequiresSameReference(Unsafe.AsPointer(ref array[0]), pointer);

            _array = array;
            _memory = new IntPtr(pointer);
            _handle = handle;

            _setData |= SetData.GcHandle;
        }


        public unsafe OwnedMemory(T[] array, void* pointer, IMemoryDisposer<T> owner, GCHandle handle = default(GCHandle))
            : this(array, pointer, handle)
        {
            _owner = owner;
        }

        public OwnedMemory(IntPtr address, int length)
            : this()
        {
            _memory = address;
            _length = length;
        }

        public OwnedMemory(IntPtr address, int length, IMemoryDisposer<T> owner)
            : this(address, length)
        {
            _owner = owner;
        }

        public OwnedMemory(int length, MemoryType type)
            : this()
        {
            switch (type)
            {
                case MemoryType.Array:
                    AllocateArray(length);
                    break;
                case MemoryType.Native:
                    AllocateNative(length);
                    break;
                case MemoryType.Pinned:
                    AllocatePinned(length);
                    break;
                default:
                    ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.type);
                    break;
            }
        }

        private void AllocateArray(int length)
        {
            _setData |= SetData.Array;

            _array = new T[length];
            _length = length;
        }

        private void AllocatePinned(int length)
        {
            AllocateArray(length);
            _handle = GCHandle.Alloc(_array, GCHandleType.Pinned);

            _setData |= SetData.GcHandle;

            _memory = _handle.AddrOfPinnedObject();
        }

        private void AllocateNative(int length)
        {
            _setData |= SetData.HGlobal;

            _memory = Marshal.AllocHGlobal(length * Unsafe.SizeOf<T>());
            _length = length;
        }

        public static explicit operator OwnedMemory<T>(T[] array)
        {
            // Allocates so make it an explicit conversion
            return new OwnedMemory<T>(array);
        }

        public Memory<T> Memory
        {
            get
            {
                if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);

                return new Memory<T>(this, _id);
            }
        }

        public unsafe Span<T> Span
        {
            get
            {
                if (IsDisposed) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);

                if (_memory != IntPtr.Zero)
                {
                    return new Span<T>(_memory.ToPointer(), _length);
                }
                else
                {
                    return new Span<T>(_array);
                }
            }
        }

        public IMemoryOwner Owner => _owner;

        public bool IsDisposed => _id == InitializedId;

        public int ReferenceCount => _references;

        public int Length => _length;

        public void Dispose()
        {
            Dispose(true);
        }

        internal virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var references = Interlocked.Decrement(ref _references);
                OnReferenceCountChanged();
                if (references == 0)
                {
                    _owner?.Return(this);

                    Clear();

                    _id = InitializedId;

                    GC.SuppressFinalize(this);

                    DisposeCore();
                }
                else if (references < 0)
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

        protected virtual void OnReferenceCountChanged() { }

        protected virtual void DisposeCore() { }

        private void Clear()
        {
            if ((_setData & SetData.Array) == SetData.Array)
            {
                _array = null;
            }

            if ((_setData & SetData.HGlobal) == SetData.HGlobal)
            {
                Marshal.FreeHGlobal(_memory);
            }

            if ((_setData & SetData.GcHandle) == SetData.GcHandle && _handle.IsAllocated)
            {
                _handle.Free();
            }

            _length = 0;
            _memory = IntPtr.Zero;
            _setData = SetData.None;
        }

        ~OwnedMemory()
        {
            Dispose(false);
        }

        // used by Memory<T>
        internal unsafe bool TryGetPointer(long id, out void* pointer)
        {
            if (_id != id) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);

            pointer = _memory.ToPointer();

            return pointer != null;
        }

        internal bool TryGetArray(long id, out ArraySegment<T> buffer)
        {
            if (_id != id) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);

            if (_array != null)
            {
                buffer = new ArraySegment<T>(_array);
                return true;
            }

            buffer = default(ArraySegment<T>);
            return false;
        }

        internal Span<T> GetSpan(long id)
        {
            if (_id != id) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);

            return Span;
        }

        void IKnown.AddReference(long secretId)
        {
            if (_id != secretId) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);
            Interlocked.Increment(ref _references);
            OnReferenceCountChanged();
        }

        void IKnown.ReleaseReference(long secretId)
        {
            if (_id != secretId) ThrowHelper.ThrowObjectDisposedException(ExceptionArgument.Memory);

            Dispose();
        }

        internal class OwnerEmptyMemory : OwnedMemory<T>
        {
            public readonly static OwnedMemory<T> Shared = new OwnerEmptyMemory();

            public OwnerEmptyMemory()
                : base(new T[0])
            { }

            internal override void Dispose(bool disposing)
            { }
        }
    }
}