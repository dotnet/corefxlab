// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Used to allocate and distribute re-usable blocks of memory.
    /// </summary>
    public class MemoryPool : BufferPool
    {
        /// <summary>
        /// The gap between blocks' starting address. 4096 is chosen because most operating systems are 4k pages in size and alignment.
        /// </summary>
        private const int _blockStride = 4096;

        /// <summary>
        /// The last 64 bytes of a block are unused to prevent CPU from pre-fetching the next 64 byte into it's memory cache. 
        /// See https://github.com/aspnet/KestrelHttpServer/issues/117 and https://www.youtube.com/watch?v=L7zSU9HI-6I
        /// </summary>
        private const int _blockUnused = 64;

        /// <summary>
        /// Allocating 32 contiguous blocks per slab makes the slab size 128k. This is larger than the 85k size which will place the memory
        /// in the large object heap. This means the GC will not try to relocate this array, so the fact it remains pinned does not negatively
        /// affect memory management's compactification.
        /// </summary>
        private const int _blockCount = 32;

        /// <summary>
        /// 4096 - 64 gives you a blockLength of 4032 usable bytes per block.
        /// </summary>
        private const int _blockLength = _blockStride - _blockUnused;

        /// <summary>
        /// Max allocation block size for pooled blocks, 
        /// larger values can be leased but they will be disposed after use rather than returned to the pool.
        /// </summary>
        public const int MaxPooledBlockLength = _blockLength;

        /// <summary>
        /// 4096 * 32 gives you a slabLength of 128k contiguous bytes allocated per slab
        /// </summary>
        private const int _slabLength = _blockStride * _blockCount;

#if !BLOCK_LEASE_TRACKING
        [ThreadStatic]
        private static MemoryPoolBlock _threadCached;
#endif

        /// <summary>
        /// Thread-safe collection of blocks which are currently in the pool. A slab will pre-allocate all of the block tracking objects
        /// and add them to this collection. When memory is requested it is taken from here first, and when it is returned it is re-added.
        /// </summary>
        private readonly ConcurrentQueue<MemoryPoolBlock> _blocks = new ConcurrentQueue<MemoryPoolBlock>();

        /// <summary>
        /// Thread-safe collection of slabs which have been allocated by this pool. As long as a slab is in this collection and slab.IsActive, 
        /// the blocks will be added to _blocks when returned.
        /// </summary>
        private readonly ConcurrentStack<MemoryPoolSlab> _slabs = new ConcurrentStack<MemoryPoolSlab>();

        /// <summary>
        /// This is part of implementing the IDisposable pattern.
        /// </summary>
        private bool _disposedValue = false; // To detect redundant calls

        private Action<MemoryPoolSlab> _slabAllocationCallback;

        private Action<MemoryPoolSlab> _slabDeallocationCallback;

        public override OwnedBuffer<byte> Rent(int size)
        {
            if ((uint)size > (uint)_blockLength)
            {
                // Negative or too large
                ThrowHelper.ThrowArgumentOutOfRangeException_BufferRequest(_blockLength);
            }

#if !BLOCK_LEASE_TRACKING
            // Only use ThreadStatic if lease tracking is not enabled
            var block = _threadCached;
            if (block != null)
            {
                if (block.Slab.IsActive)
                {
                    _threadCached = null;
                    block.Initialize();
                    return block;
                }
                else
                {
                    TrashBlock(block);
                }
            }
#endif
            return Lease();
        }

        public void RegisterSlabAllocationCallback(Action<MemoryPoolSlab> callback)
        {
            _slabAllocationCallback = callback;
        }

        public void RegisterSlabDeallocationCallback(Action<MemoryPoolSlab> callback)
        {
            _slabDeallocationCallback = callback;
        }

        /// <summary>
        /// Called to take a block from the pool.
        /// </summary>
        /// <returns>The block that is reserved for the called. It must be passed to Return when it is no longer being used.</returns>

#if !BLOCK_LEASE_TRACKING
        private MemoryPoolBlock Lease()
        {
            if (_blocks.TryDequeue(out var block))
            {
                // block successfully taken from the stack - return it
                block.Initialize();
                return block;
            }
            // no blocks available - grow the pool
            return AllocateSlab();
        }
#else
        private MemoryPoolBlock Lease()
        {
            Debug.Assert(!_disposedValue, "Block being leased from disposed pool!");

            MemoryPoolBlock block;
            if (_blocks.TryDequeue(out block))
            {
                // block successfully taken from the stack - return it

                block.Leaser = Environment.StackTrace;
                block.IsLeased = true;
                block.Initialize();
                return block;
            }
            // no blocks available - grow the pool
            block = AllocateSlab();
            block.Leaser = Environment.StackTrace;
            block.IsLeased = true;
            block.Initialize();
            return block;
        }
#endif

        /// <summary>
        /// Internal method called when a block is requested and the pool is empty. It allocates one additional slab, creates all of the 
        /// block tracking objects, and adds them all to the pool.
        /// </summary>
        private MemoryPoolBlock AllocateSlab()
        {
            var slab = MemoryPoolSlab.Create(_slabLength);
            _slabs.Push(slab);

            _slabAllocationCallback?.Invoke(slab);
            slab._deallocationCallback = _slabDeallocationCallback;

            var basePtr = slab.NativePointer;
            var firstOffset = (int)((_blockStride - 1) - ((ulong)(basePtr + _blockStride - 1) % _blockStride));

            var poolAllocationLength = _slabLength - _blockStride;

            var offset = firstOffset;
            for (;
                offset + _blockLength < poolAllocationLength;
                offset += _blockStride)
            {
                var block = MemoryPoolBlock.Create(
                    offset,
                    _blockLength,
                    this,
                    slab);
#if BLOCK_LEASE_TRACKING
                block.IsLeased = true;
#endif
                Return(block);
            }

            // return last block rather than adding to pool
            var newBlock = MemoryPoolBlock.Create(
                    offset,
                    _blockLength,
                    this,
                    slab);

            return newBlock;
        }

        /// <summary>
        /// Called to return a block to the pool. Once Return has been called the memory no longer belongs to the caller, and
        /// Very Bad Things will happen if the memory is read of modified subsequently. If a caller fails to call Return and the
        /// block tracking object is garbage collected, the block tracking object's finalizer will automatically re-create and return
        /// a new tracking object into the pool. This will only happen if there is a bug in the server, however it is necessary to avoid
        /// leaving "dead zones" in the slab due to lost block tracking objects.
        /// </summary>
        /// <param name="block">The block to return. It must have been acquired by calling Lease on the same memory pool instance.</param>
#if !BLOCK_LEASE_TRACKING
        public void Return(MemoryPoolBlock block)
        {
            if (block.Slab.IsActive)
            {
                // Added returned segment to thread static keeps it hotter
                // and means it won't get reset and used by other threads if there is 
                // about to be a use after free by the current thread
                var currentBlock = _threadCached;
                _threadCached = block;

                if (currentBlock != null)
                {
                    // Was a current block in thread cache
                    // Add this less recently used block to queue
                    _blocks.Enqueue(block);
                }
            }
            else
            {
                TrashBlock(block);
            }
        }
#else
        public void Return(MemoryPoolBlock block)
        {
            Debug.Assert(block.Pool == this, "Returned block was not leased from this pool");
            Debug.Assert(block.IsLeased, $"Block being returned to pool twice: {block.Leaser}{Environment.NewLine}");
            block.IsLeased = false;

            if (block.Slab.IsActive)
            {
                _blocks.Enqueue(block);
            }
            else
            {
                TrashBlock(block);
            }
        }
#endif
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void TrashBlock(MemoryPoolBlock block)
        {
            GC.SuppressFinalize(block);
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
#if BLOCK_LEASE_TRACKING
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
#endif
                if (disposing)
                {
                    while (_slabs.TryPop(out var slab))
                    {
                        // dispose managed state (managed objects).
                        slab.Dispose();
                    }
                }

                // Discard blocks in pool
                while (_blocks.TryDequeue(out var block))
                {
                    GC.SuppressFinalize(block);
                }
            }
        }
    }
}