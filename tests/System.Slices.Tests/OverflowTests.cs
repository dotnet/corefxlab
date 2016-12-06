using System.Runtime.InteropServices;
using Xunit;

namespace System.Slices.Tests
{
    public unsafe class OverflowTests
    {
        private static unsafe bool Is64Bit => sizeof(void*) == 8;

        const long
            ThreeGiB = 3L * 1024L * 1024L * 1024L,
            TwoGiB = 2L * 1024L * 1024L * 1024L,
            OneGiB = 1L * 1024L * 1024L * 1024L;

        static readonly int
            GuidThreeGiBLimit = (int)(ThreeGiB / sizeof(Guid)),
            GuidTwoGiBLimit = (int)(TwoGiB / sizeof(Guid)),
            GuidOneGiBLimit = (int)(OneGiB / sizeof(Guid)),

            ULongThreeGiBLimit = (int)(ThreeGiB / sizeof(ulong)),
            ULongTwoGiBLimit = (int)(TwoGiB / sizeof(ulong)),
            Int32ThreeGiBLimit = (int)(ThreeGiB / sizeof(int));

        [Fact]
        public void SliceStartInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var span = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice(GuidTwoGiBLimit + 1);
                slice[0] = guid;

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }
        [Fact]
        public void SliceStartUInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var span = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice((GuidTwoGiBLimit + 1));
                slice[0] = guid;

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }
        [Fact]
        public void SliceStartLengthInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var span = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice(GuidTwoGiBLimit + 1, 20);
                slice[0] = guid;

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }
        [Fact]
        public void SliceStartLengthUInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var span = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice(GuidTwoGiBLimit + 1, 20);
                slice[0] = guid;

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        // review: note - can't prove that the 3GiB problem is fixed since core-clr doesn't support
        // very large arrays currently; however, we can prove that it still works correctly for
        // small arrays
        [Fact]
        public void ArrayCtorStartOverflow()
        {
            var arr = new Guid[20];

            var slice = arr.Slice(2);
            Guid guid = Guid.NewGuid();
            slice[1] = guid;

            Assert.Equal(guid, arr[3]);
        }

        // review: note - can't prove that the 3GiB problem is fixed since core-clr doesn't support
        // very large arrays currently; however, we can prove that it still works correctly for
        // small arrays
        [Fact]
        public void ArrayCtorStartLengthOverflow()
        {
            var arr = new Guid[20];

            var slice = arr.Slice(2, 2);
            Guid guid = Guid.NewGuid();
            slice[1] = guid;

            Assert.Equal(guid, arr[3]);
        }

        [ConditionalFact(nameof(Is64Bit))]
        public void CastOverflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var span = new Span<ulong>((void*)huge, ULongThreeGiBLimit);

                var asInt = span.Cast<ulong, int>();
                Assert.Equal(Int32ThreeGiBLimit, asInt.Length);

                var asULong = asInt.Cast<int, ulong>();
                Assert.Equal(ULongThreeGiBLimit, asULong.Length);

                asULong[ULongTwoGiBLimit + 4] = 42;
                Assert.Equal((ulong)42, span[ULongTwoGiBLimit + 4]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        [ConditionalFact(nameof(Is64Bit))]
        public void ReadOnlySliceStartInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var mutable = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                var span = new ReadOnlySpan<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice(GuidTwoGiBLimit + 1);
                mutable[GuidTwoGiBLimit + 1] = guid;
                Assert.Equal(guid, slice[0]);

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        [ConditionalFact(nameof(Is64Bit))]
        public void ReadOnlySliceStartUInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var mutable = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                var span = new ReadOnlySpan<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice((GuidTwoGiBLimit + 1));
                mutable[GuidTwoGiBLimit + 1] = guid;
                Assert.Equal(guid, slice[0]);

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        [ConditionalFact(nameof(Is64Bit))]
        public void ReadOnlySliceStartLengthInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var mutable = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                var span = new ReadOnlySpan<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice(GuidTwoGiBLimit + 1, 20);
                mutable[GuidTwoGiBLimit + 1] = guid;
                Assert.Equal(guid, slice[0]);

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        [ConditionalFact(nameof(Is64Bit))]
        public void ReadOnlySliceStartLengthUInt32Overflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var mutable = new Span<Guid>((void*)huge, GuidThreeGiBLimit);
                var span = new ReadOnlySpan<Guid>((void*)huge, GuidThreeGiBLimit);
                Guid guid = Guid.NewGuid();
                var slice = span.Slice(GuidTwoGiBLimit + 1, 20);
                mutable[GuidTwoGiBLimit + 1] = guid;
                Assert.Equal(guid, slice[0]);

                slice = span.Slice(GuidOneGiBLimit).Slice(1).Slice(GuidOneGiBLimit);
                Assert.Equal(guid, slice[0]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        //// review: note - can't prove that the 3GiB problem is fixed since core-clr doesn't support
        //// very large arrays currently; however, we can prove that it still works correctly for
        //// small arrays
        //[Fact]
        //public void ReadOnlyArrayCtorStartOverflow()
        //{
        //    var arr = new Guid[20];

        //    var slice = new ReadOnlySpan<Guid>(arr, 2);
        //    Guid guid = Guid.NewGuid();
        //    arr[3] = guid;

        //    Assert.Equal(guid, slice[1]);
        //}
        // ^^^ this constructor is not used anywhere

        // review: note - can't prove that the 3GiB problem is fixed since core-clr doesn't support
        // very large arrays currently; however, we can prove that it still works correctly for
        // small arrays
        [Fact]
        public void ReadOnlyArrayCtorStartLengthOverflow()
        {
            var arr = new Guid[20];
            
            var slice = new ReadOnlySpan<Guid>(arr, 2, 2);
            Guid guid = Guid.NewGuid();
            arr[3] = guid;

            Assert.Equal(guid, slice[1]);
        }


        [ConditionalFact(nameof(Is64Bit))]
        public void ReadOnlyCastOverflow()
        {
            var huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            try
            {
                var span = new ReadOnlySpan<ulong>((void*)huge, ULongThreeGiBLimit);

                var asInt = span.Cast<ulong, int>();
                Assert.Equal(Int32ThreeGiBLimit, asInt.Length);

                var asULong = asInt.Cast<int, ulong>();
                Assert.Equal(ULongThreeGiBLimit, asULong.Length);

                var writable = new Span<ulong>((void*)huge, ULongThreeGiBLimit);
                writable[ULongTwoGiBLimit + 4] = 42;
                Assert.Equal((ulong)42, asULong[ULongTwoGiBLimit + 4]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }
    }
}
