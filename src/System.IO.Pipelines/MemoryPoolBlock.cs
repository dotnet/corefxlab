// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Text;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Block tracking object used by the byte buffer memory pool. A slab is a large allocation which is divided into smaller blocks. The
    /// individual blocks are then treated as independent array segments.
    /// </summary>
    public class MemoryPoolBlock : ReferenceCountedBuffer<byte>
    {
        private readonly int _offset;
        private readonly int _length;

        /// <summary>
        /// This object cannot be instantiated outside of the static Create method
        /// </summary>
        protected MemoryPoolBlock(MemoryPool pool, MemoryPoolSlab slab, int offset, int length)
        {
            _offset = offset;
            _length = length;

            Pool = pool;
            Slab = slab;
        }

        /// <summary>
        /// Back-reference to the memory pool which this block was allocated from. It may only be returned to this pool.
        /// </summary>
        public MemoryPool Pool { get; }

        /// <summary>
        /// Back-reference to the slab from which this block was taken, or null if it is one-time-use memory.
        /// </summary>
        public MemoryPoolSlab Slab { get; }

        public override int Length => _length;

        public override Span<byte> Span
        {
            get
            {
                if (IsDisposed) PipelinesThrowHelper.ThrowObjectDisposedException(nameof(MemoryPoolBlock));
                return new Span<byte>(Slab.Array, _offset, _length);
            }
        }

#if BLOCK_LEASE_TRACKING
        public bool IsLeased { get; set; }
        public string Leaser { get; set; }
#endif

        ~MemoryPoolBlock()
        {
#if BLOCK_LEASE_TRACKING
            Debug.Assert(Slab == null || !Slab.IsActive, $"{Environment.NewLine}{Environment.NewLine}*** Block being garbage collected instead of returned to pool: {Leaser} ***{Environment.NewLine}");
#endif
            if (Slab != null && Slab.IsActive)
            {
                // Need to make a new object because this one is being finalized
                Pool.Return(new MemoryPoolBlock(Pool, Slab, _offset, _length));
            }
        }

        internal static MemoryPoolBlock Create(
            int offset,
            int length,
            MemoryPool pool,
            MemoryPoolSlab slab)
        {
            return new MemoryPoolBlock(pool, slab, offset, length)
            {
#if BLOCK_LEASE_TRACKING
                Leaser = Environment.StackTrace,
#endif
            };
        }

        /// <summary>
        /// ToString overridden for debugger convenience. This displays the "active" byte information in this block as ASCII characters.
        /// ToString overridden for debugger convenience. This displays the byte information in this block as ASCII characters.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            SpanExtensions.AppendAsLiteral(Buffer.Span, builder);
            return builder.ToString();
        }

        internal void Initialize()
        {
            // This is VERY bad, but is required while there is no re-initialization support
            base.Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            // Dispose before returning to pool to prevent race between Lease and Dispose
            base.Dispose(disposing);

            Pool.Return(this);
        }

// In kestrel both MemoryPoolBlock and OwnedBuffer end up in the same assembly so
// this method access modifiers need to be `protected internal`
#if KESTREL_BY_SOURCE
        internal
#endif
        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            if (IsDisposed) PipelinesThrowHelper.ThrowObjectDisposedException(nameof(MemoryPoolBlock));
            buffer = new ArraySegment<byte>(Slab.Array, _offset, _length);
            return true;
        }

// In kestrel both MemoryPoolBlock and OwnedBuffer end up in the same assembly so
// this method access modifiers need to be `protected internal`
#if KESTREL_BY_SOURCE
        internal
#endif
        protected override unsafe bool TryGetPointerAt(int index, out void* pointer)
        {
            if (IsDisposed) PipelinesThrowHelper.ThrowObjectDisposedException(nameof(MemoryPoolBlock));
            pointer = (Slab.NativePointer + _offset + index).ToPointer();
            return true;
        }
    }
}