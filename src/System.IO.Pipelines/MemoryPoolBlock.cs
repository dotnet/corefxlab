// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.Text;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Block tracking object used by the byte buffer memory pool. A slab is a large allocation which is divided into smaller blocks. The
    /// individual blocks are then treated as independent array segments.
    /// </summary>
    public class MemoryPoolBlock : OwnedMemory<byte>
    {
        private readonly int _offset;
        private readonly int _length;

        /// <summary>
        /// This object cannot be instantiated outside of the static Create method
        /// </summary>
        protected unsafe MemoryPoolBlock(MemoryPool pool, MemoryPoolSlab slab, int offset, int length) : base(slab.Array, offset, length, slab.NativePointer + offset)
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

#if DEBUG
        public bool IsLeased { get; set; }
        public string Leaser { get; set; }
#endif

        ~MemoryPoolBlock()
        {
#if DEBUG
            Debug.Assert(Slab == null || !Slab.IsActive, $"{Environment.NewLine}{Environment.NewLine}*** Block being garbage collected instead of returned to pool: {Leaser} ***{Environment.NewLine}");
#endif
            if (Slab != null && Slab.IsActive)
            {
                // Need to make a new object because this one is being finalized
                Pool.Return(new MemoryPoolBlock(Pool, Slab, _offset, _length));
            }
        }

        internal void Initialize()
        {
            if (IsDisposed)
            {
                Initialize(Slab.Array, _offset, _length, Slab.NativePointer + _offset);
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
#if DEBUG
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
            var data = Memory.Span;

            for (int i = 0; i < data.Length; i++)
            {
                builder.Append((char)data[i]);
            }
            return builder.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            Pool.Return(this);

            base.Dispose(disposing);
        }
    }
}