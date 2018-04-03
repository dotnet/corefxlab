// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO.Pipelines;

namespace System.Buffers
{
    /// <summary>
    /// Block tracking object used by the byte buffer memory pool. A slab is a large allocation which is divided into smaller blocks. The
    /// individual blocks are then treated as independent array segments.
    /// </summary>
    internal class RioMemoryPoolBlock : MemoryManager<byte>
    {
        private readonly int _offset;
        private readonly int _length;
        private int _referenceCount;
        private bool _disposed;

        /// <summary>
        /// This object cannot be instantiated outside of the static Create method
        /// </summary>
        protected RioMemoryPoolBlock(RioMemoryPool pool, RioMemoryPoolSlab slab, int offset, int length)
        {
            _offset = offset;
            _length = length;

            Pool = pool;
            Slab = slab;
        }

        /// <summary>
        /// Back-reference to the memory pool which this block was allocated from. It may only be returned to this pool.
        /// </summary>
        public RioMemoryPool Pool { get; }

        /// <summary>
        /// Back-reference to the slab from which this block was taken, or null if it is one-time-use memory.
        /// </summary>
        public RioMemoryPoolSlab Slab { get; }

        public override int Length => _length;

        public override Span<byte> GetSpan()
        {
            if (IsDisposed) RioPipelinesThrowHelper.ThrowObjectDisposedException(nameof(RioMemoryPoolBlock));
            return new Span<byte>(Slab.Array, _offset, _length);
        }

#if BLOCK_LEASE_TRACKING
        public bool IsLeased { get; set; }
        public string Leaser { get; set; }
#endif

        ~RioMemoryPoolBlock()
        {
            if (Slab != null && Slab.IsActive)
            {
#if DEBUG
                Debug.Assert(false, $"{Environment.NewLine}{Environment.NewLine}*** Block being garbage collected instead of returned to pool" +
#if BLOCK_LEASE_TRACKING
                    $": {Leaser}" +
#endif
                    $" ***{ Environment.NewLine}");
#endif

                // Need to make a new object because this one is being finalized
                Pool.Return(new RioMemoryPoolBlock(Pool, Slab, _offset, _length));
            }
        }

        internal static RioMemoryPoolBlock Create(
            int offset,
            int length,
            RioMemoryPool pool,
            RioMemoryPoolSlab slab)
        {
            return new RioMemoryPoolBlock(pool, slab, offset, length);
        }

        /// <summary>
        /// ToString overridden for debugger convenience. This displays the "active" byte information in this block as ASCII characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            RioSpanLiteralExtensions.AppendAsLiteral(Memory.Span, builder);
            return builder.ToString();
        }

        protected void OnZeroReferences()
        {
            Pool.Return(this);
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
        }

        public void Retain()
        {
            if (IsDisposed) RioPipelinesThrowHelper.ThrowObjectDisposedException(nameof(RioMemoryPoolBlock));
            Interlocked.Increment(ref _referenceCount);
        }

        public bool Release()
        {
            int newRefCount = Interlocked.Decrement(ref _referenceCount);
            if (newRefCount < 0) RioPipelinesThrowHelper.ThrowInvalidOperationException(ExceptionResource.ReferenceCountZero);
            if (newRefCount == 0)
            {
                OnZeroReferences();
                return false;
            }
            return true;
        }

        protected bool IsRetained => _referenceCount > 0;
        public bool IsDisposed => _disposed;

        // In kestrel both MemoryPoolBlock and OwnedMemory end up in the same assembly so
        // this method access modifiers need to be `protected internal`
        protected override bool TryGetArray(out ArraySegment<byte> arraySegment)
        {
            if (IsDisposed) RioPipelinesThrowHelper.ThrowObjectDisposedException(nameof(RioMemoryPoolBlock));
            arraySegment = new ArraySegment<byte>(Slab.Array, _offset, _length);
            return true;
        }

        public override MemoryHandle Pin(int elementIndex = 0)
        {
            Retain();   // checks IsDisposed
            if (elementIndex < 0 || elementIndex > _length) RioPipelinesThrowHelper.ThrowArgumentOutOfRangeException(_length, elementIndex);
            unsafe
            {
                return new MemoryHandle((Slab.NativePointer + _offset + elementIndex).ToPointer(), default, this);
            }
        }

        public override void Unpin()
        {
            Release();
        }
    }
}
