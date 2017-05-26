// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Runtime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xunit;

namespace System.Buffers.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void AutoDispose()
        {
            OwnedBuffer<byte> owned = new AutoPooledBuffer(1000);
            owned.Retain();
            var memory = owned.Buffer;
            Assert.Equal(false, owned.IsDisposed);
            var reservation = memory.Retain();
            Assert.Equal(false, owned.IsDisposed);
            owned.Release();
            Assert.Equal(false, owned.IsDisposed);
            reservation.Dispose();
            Assert.Equal(true, owned.IsDisposed);
        }

        [Fact]
        public void MemoryHandleFreeUninitialized()
        {
            var h = default(BufferHandle);
            h.Dispose();
        }

        [Fact]
        public void OnNoReferencesTest()
        {
            var owned = new CustomBuffer();
            var memory = owned.Buffer;
            Assert.Equal(0, owned.OnNoRefencesCalledCount);
            Assert.False(owned.IsRetained);
            using (memory.Retain())
            {
                Assert.Equal(0, owned.OnNoRefencesCalledCount);
                Assert.True(owned.IsRetained);
            }
            Assert.Equal(1, owned.OnNoRefencesCalledCount);
            Assert.False(owned.IsRetained);
        }

        [Fact(Skip = "This needs to be fixed and re-enabled or removed.")]
        public void RacyAccess()
        {
            for (int k = 0; k < 1000; k++)
            {
                var owners = new OwnedArray<byte>[128];
                var memories = new Buffer<byte>[owners.Length];
                var reserves = new BufferHandle[owners.Length];
                var disposeSuccesses = new bool[owners.Length];
                var reserveSuccesses = new bool[owners.Length];

                for (int i = 0; i < owners.Length; i++)
                {
                    var array = new byte[1024];
                    owners[i] = array;
                    memories[i] = owners[i].Buffer;
                }

                var dispose_task = Task.Run(() => {
                    for (int i = 0; i < owners.Length; i++)
                    {
                        try
                        {
                            owners[i].Dispose();
                            disposeSuccesses[i] = true;
                        }
                        catch (InvalidOperationException)
                        {
                            disposeSuccesses[i] = false;
                        }
                    }
                });

                var reserve_task = Task.Run(() => {
                    for (int i = owners.Length - 1; i >= 0; i--)
                    {
                        try
                        {
                            reserves[i] = memories[i].Retain();
                            reserveSuccesses[i] = true;
                        }
                        catch (ObjectDisposedException)
                        {
                            reserveSuccesses[i] = false;
                        }
                    }
                });

                Task.WaitAll(reserve_task, dispose_task);

                for (int i = 0; i < owners.Length; i++)
                {
                    Assert.False(disposeSuccesses[i] && reserveSuccesses[i]);
                }
            }
        }
    }

    class CustomBuffer : OwnedArray<byte>
    {
        int _noReferencesCalledCount;

        public CustomBuffer() : base(new byte[255]) { }

        public int OnNoRefencesCalledCount => _noReferencesCalledCount;

        protected override void OnNoReferences()
        {
            _noReferencesCalledCount++;
        }
    }

    class AutoDisposeBuffer<T> : ReferenceCountedBuffer<T>
    {
        public AutoDisposeBuffer(T[] array)
        {
            _array = array;
        }

        public override int Length => _array.Length;

        public override Span<T> AsSpan(int index, int length)
        {
            if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(CustomBuffer));
            return new Span<T>(_array, index, length);
        }

        protected override void Dispose(bool disposing)
        {
            _array = null;
            base.Dispose(disposing);
        }

        protected override void OnNoReferences()
        {
            Dispose();
        }

        protected override bool TryGetArray(out ArraySegment<T> buffer)
        {
            if (IsDisposed) BufferPrimitivesThrowHelper.ThrowObjectDisposedException(nameof(AutoDisposeBuffer<T>));
            buffer = new ArraySegment<T>(_array);
            return true;
        }

        public override BufferHandle Pin(int index = 0)
        {
            unsafe
            {
                Retain();
                var handle = GCHandle.Alloc(_array, GCHandleType.Pinned);
                var pointer = Add((void*)handle.AddrOfPinnedObject(), index);
                return new BufferHandle(this, pointer, handle);
            }
        }

        protected T[] _array;
    }

    class AutoPooledBuffer : AutoDisposeBuffer<byte>
    {
        public AutoPooledBuffer(int length) : base(ArrayPool<byte>.Shared.Rent(length)) {
        }

        protected override void Dispose(bool disposing)
        {
            var array = _array;
            if (array != null) {
                ArrayPool<byte>.Shared.Return(array);
            }
            _array = null;
            base.Dispose(disposing);
        }
    }

}
