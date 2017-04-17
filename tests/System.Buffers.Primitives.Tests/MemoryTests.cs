// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Collections.Generic;
using System.Runtime;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Xunit.Performance;

namespace System.Buffers.Tests
{
    public class MemoryTests
    {
        [Fact]
        public void SimpleTestS()
        {
            {
                var array = new byte[1024];
                OwnedBuffer<byte> owned = array;
                var span = owned.Span;
                span[10] = 10;
                Assert.Equal(10, array[10]);

                var memory = owned.Buffer;
                var toArrayResult = memory.ToArray();
                Assert.Equal(owned.Length, array.Length);
                Assert.Equal(10, toArrayResult[10]);

                Span<byte> copy = new byte[20];
                memory.Slice(10, 20).CopyTo(copy);
                Assert.Equal(10, copy[0]);
            }
        }

        [Fact]
        public void ArrayMemoryLifetime()
        {
            var array = new byte[1024];
            OwnedBuffer<byte> owned = array;
            TestLifetime(owned);
        }

        static void TestLifetime(OwnedBuffer<byte> owned)
        {
            Buffer<byte> copyStoredForLater;
            try
            {
                Buffer<byte> memory = owned.Buffer;
                Buffer<byte> memorySlice = memory.Slice(10);
                copyStoredForLater = memorySlice;
                var r = memorySlice.Reserve();
                try
                {
                    Assert.Throws<InvalidOperationException>(() => { // memory is reserved; premature dispose check fires
                        owned.Dispose();
                    });
                }
                finally
                {
                    r.Dispose(); // release reservation
                }
            }
            finally
            {
                owned.Dispose(); // can finish dispose with no exception
            }
            Assert.Throws<ObjectDisposedException>(() => {
                // memory is disposed; cannot use copy stored for later
                var span = copyStoredForLater.Span;
            });
        }


        [Fact(Skip = "This needs to be fixed and re-enabled or removed.")]
        public void RacyAccess()
        {
            for(int k = 0; k < 1000; k++) {
                var owners   = new OwnedBuffer<byte>[128];
                var memories = new Buffer<byte>[owners.Length];
                var reserves = new DisposableReservation<byte>[owners.Length];
                var disposeSuccesses = new bool[owners.Length];
                var reserveSuccesses = new bool[owners.Length];

                for (int i = 0; i < owners.Length; i++) {
                    var array = new byte[1024];
                    owners[i] = array;
                    memories[i] = owners[i].Buffer;
                }

                var dispose_task = Task.Run(() => {
                    for (int i = 0; i < owners.Length; i++) {
                        try {
                            owners[i].Dispose();
                            disposeSuccesses[i] = true;
                        } catch (InvalidOperationException) {
                            disposeSuccesses[i] = false;
                        }
                    }
                });

                var reserve_task = Task.Run(() => {
                    for (int i = owners.Length - 1; i >= 0; i--) {
                        try {
                            reserves[i] = memories[i].Reserve();
                            reserveSuccesses[i] = true;
                        } catch (ObjectDisposedException) {
                            reserveSuccesses[i] = false;
                        }
                    }
                });

                Task.WaitAll(reserve_task, dispose_task);

                for(int i = 0; i < owners.Length; i++) {
                    Assert.False(disposeSuccesses[i] && reserveSuccesses[i]);
                }
            }
        }


        #region helpers
        private static object[] MakeArray(params object[] args)
        {
            return args;
        }

        /// Generate the ith permutation of the argss array of arrays.
        private static object[] MakeIthPermutation(int i, object[][] argss) {
            var res = new object[argss.Length];
            for (int j = 0; j < argss.Length; j++) {
                var arg = argss[j];
                res[j] = arg[i % arg.Length];
                i /= arg.Length;
            }
            return res;
        }
        private static IEnumerable<object[]> MakePermutations(params object[][] argss)
        {
            if (argss.Length != 0) {
                var size = 1;
                // Calculate number of permutations.
                foreach (var args in argss) {
                    size *= args.Length;
                }
                object[][] ress = new object[size][];
                for (int i = 0; i < size; i++) {
                    ress[i] = MakeIthPermutation(i, argss);
                }
                return ress;
            }
            return null;
        }
        #endregion

        public static IEnumerable<object[]> ReservationPerformanceData =>
            MakePermutations(MakeArray(1,4,10),
                             MakeArray(100,1000),
                             MakeArray(1,2,4,8,12),
                             MakeArray(ReferenceCountingMethod.None,
                                       ReferenceCountingMethod.Interlocked,
                                       ReferenceCountingMethod.ReferenceCounter));


        [MemberData(nameof(ReservationPerformanceData))]
        [Benchmark]
        public void ReservationPerformance(int number, int size, int threads, ReferenceCountingMethod m)
        {
            var iterations = 1000000;

            var o = ReferenceCountingSettings.OwnedMemory;
            ReferenceCountingSettings.OwnedMemory = m;

            Benchmark.Iterate( () => {
                var owners   = new OwnedBuffer<byte>[number];
                var memories = new Buffer<byte>[owners.Length];

                for (int i = 0; i < owners.Length; i++) {
                    owners[i] = new AutoPooledMemory(size);
                    memories[i] = owners[i].Buffer;
                }

                var tasks = new List<Task>(threads);
                for (int t = 0; t < threads; t++) {
                    tasks.Add(Task.Run(() => {
                        for (int k = 0; k < iterations / owners.Length; k++) {
                            for (int i = 0; i < owners.Length; i++) {
                                using (var reserve = memories[i].Reserve()) {
                                    var s = reserve.Span;
                                    for (int j = 0; j < owners.Length; j++) {
                                        s[j] = (byte)1;
                                    }
                                }
                            }
                        }
                    }));
                }

                Task.WaitAll(tasks.ToArray());

                for (int i = 0; i < owners.Length; i++) {
                    owners[i].Release();
                }
            });

            ReferenceCountingSettings.OwnedMemory = o;
        }

        [Fact]
        public unsafe void ReferenceCounting()
        {
            var owned = new CustomMemory();
            var memory = owned.Buffer;
            Assert.Equal(0, owned.OnZeroRefencesCount);
            Assert.False(owned.HasOutstandingReferences);
            using (memory.Reserve()) {
                Assert.Equal(0, owned.OnZeroRefencesCount);
                Assert.True(owned.HasOutstandingReferences);
            }
            Assert.Equal(1, owned.OnZeroRefencesCount);
            Assert.False(owned.HasOutstandingReferences);
        }

        [Fact]
        public void AutoDispose()
        {
            OwnedBuffer<byte> owned = new AutoPooledMemory(1000);
            var memory = owned.Buffer;
            Assert.Equal(false, owned.IsDisposed);
            var reservation = memory.Reserve();
            Assert.Equal(false, owned.IsDisposed);
            owned.Release();
            Assert.Equal(false, owned.IsDisposed);
            reservation.Dispose();
            Assert.Equal(true, owned.IsDisposed);
        }

        [Fact]
        public void PinAddReferenceReleaseTest()
        {
            var array = new byte[1024];
            OwnedBuffer<byte> owned = array;
            var memory = owned.Buffer;
            Assert.False(owned.HasOutstandingReferences);
            var h = memory.Pin();
            Assert.True(owned.HasOutstandingReferences);
            h.Free();
            Assert.False(owned.HasOutstandingReferences);
        }

        [Fact]
        public void MemoryHandleFreeUninitialized()
        {
            var h = default(BufferHandle);
            h.Free();
        }

        [Fact]
        public void MemoryHandleDoubleFree() 
        {
            var array = new byte[1024];
            OwnedBuffer<byte> owned = array;
            var memory = owned.Buffer;
            var h = memory.Pin();
            Assert.True(owned.HasOutstandingReferences);
            owned.AddReference();
            Assert.True(owned.HasOutstandingReferences);
            h.Free();
            Assert.True(owned.HasOutstandingReferences);
            h.Free();
            Assert.True(owned.HasOutstandingReferences);
            owned.Release();
            Assert.False(owned.HasOutstandingReferences);
        }


        WeakReference LeakHandle()
        {
            // Creates an object that is both Pinned with a MemoryHandle,
            // and has a weak reference.
            var array = new byte[1024];
            OwnedBuffer<byte> owned = array;
            var memory = owned.Buffer;
            memory.Pin();
            return new WeakReference(array);
        }

        void DoGC()
        {
            GC.Collect(2);
            GC.WaitForPendingFinalizers();
            GC.Collect(2);
        }

        [Fact]
        void PinGCArrayTest()
        {
            var w = LeakHandle();
            // Weak reference should be kept alive.
            DoGC();
            Assert.True(w.IsAlive);
        }
    }

    class CustomMemory : OwnedBuffer<byte>
    {
        public CustomMemory()
        {
            _array = new byte[255];
        }

        public int OnZeroRefencesCount => _onZeroRefencesCount;

        public override int Length => _array.Length;

        public override Span<byte> Span => _array;

        public override Span<byte> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return Span.Slice(index, length);
        }

        protected override void OnZeroReferences()
        {
            _onZeroRefencesCount++;
        }

        protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
        {
            buffer = new ArraySegment<byte>(_array);
            return true;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }

        int _onZeroRefencesCount;
        byte[] _array;
    }

    class AutoDisposeMemory<T> : OwnedBuffer<T>
    {
        public AutoDisposeMemory(T[] array)
        {
            _array = array;
            AddReference();
        }

        public override int Length => _array.Length;

        public override Span<T> Span => _array;

        public override Span<T> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            return Span.Slice(index, length);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnZeroReferences()
        {
            Dispose();
        }

        protected override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(_array);
            return true;
        }

        protected override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }

        protected T[] _array;
    }

    class AutoPooledMemory : AutoDisposeMemory<byte>
    {
        public AutoPooledMemory(int length) : base(ArrayPool<byte>.Shared.Rent(length)) {
        }

        protected override void Dispose(bool disposing)
        {
            ArrayPool<byte>.Shared.Return(_array);
            _array = null;
            base.Dispose(disposing);
        }
    }

}
