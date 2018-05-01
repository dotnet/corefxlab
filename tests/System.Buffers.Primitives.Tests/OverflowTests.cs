// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;
using Xunit;

namespace System.Buffers.Tests
{
    public unsafe class OverflowTests
    {
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
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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

            var slice = arr.AsSpan(2);
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

            var slice = arr.AsSpan(2, 2);
            Guid guid = Guid.NewGuid();
            slice[1] = guid;

            Assert.Equal(guid, arr[3]);
        }


        [Fact]
        public void CastOverflow()
        {
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

            try
            {
                var span = new Span<ulong>((void*)huge, ULongThreeGiBLimit);

                var asInt = MemoryMarshal.Cast<ulong, int>(span);
                Assert.Equal(Int32ThreeGiBLimit, asInt.Length);

                var asULong = MemoryMarshal.Cast<int, ulong>(asInt);
                Assert.Equal(ULongThreeGiBLimit, asULong.Length);

                asULong[ULongTwoGiBLimit + 4] = 42;
                Assert.Equal((ulong)42, span[ULongTwoGiBLimit + 4]);
            }
            finally
            {
                Marshal.FreeHGlobal(huge);
            }
        }

        [Fact]
        public void ReadOnlySliceStartInt32Overflow()
        {
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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
        [Fact]
        public void ReadOnlySliceStartUInt32Overflow()
        {
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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
        [Fact]
        public void ReadOnlySliceStartLengthInt32Overflow()
        {
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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
        [Fact]
        public void ReadOnlySliceStartLengthUInt32Overflow()
        {
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

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


        [Fact]
        public void ReadOnlyCastOverflow()
        {
            IntPtr huge;
            try
            {
                huge = Marshal.AllocHGlobal(new IntPtr(ThreeGiB));
            }
            catch (Exception)
            {
                return;  // It's not implausible to believe that a 3gb allocation will fail - if so, skip this test to avoid unnecessary test flakiness.
            }

            try
            {
                var span = new ReadOnlySpan<ulong>((void*)huge, ULongThreeGiBLimit);

                var asInt = MemoryMarshal.Cast<ulong, int>(span);
                Assert.Equal(Int32ThreeGiBLimit, asInt.Length);

                var asULong = MemoryMarshal.Cast<int, ulong>(asInt);
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
