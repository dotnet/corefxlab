// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;
using Xunit;
using Xunit.Abstractions;

namespace System.Slices.Tests
{
    public class UsageScenarioTests
    {
        private readonly ITestOutputHelper output;

        public UsageScenarioTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        private struct MyByte
        {
            public MyByte(byte value)
            {
                Value = value;
            }

            public byte Value { get; private set; }
        }

        [Theory]
        [InlineData(new byte[] { })]
        [InlineData(new byte[] { 0 })]
        [InlineData(new byte[] { 0, 1 })]
        [InlineData(new byte[] { 0, 1, 2 })]
        [InlineData(new byte[] { 0, 1, 2, 3 })]
        [InlineData(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        public void CtorSpanOverByteArrayValidCasesWithPropertiesAndBasicOperationsChecks(byte[] array)
        {
            Span<byte> span = new Span<byte>(array);
            Assert.Equal(array.Length, span.Length);

            Assert.NotSame(array, span.CreateArray());
            Assert.False(span.Equals(array));

            ReadOnlySpan<byte>.Enumerator it = span.GetEnumerator();
            for (int i = 0; i < span.Length; i++)
            {
                Assert.True(it.MoveNext());
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                array[i] = unchecked((byte)(array[i] + 1));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                span.Slice(i).Write<byte>(unchecked((byte)(array[i] + 1)));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                span.Slice(i).Write<MyByte>(unchecked(new MyByte((byte)(array[i] + 1))));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);
            }
            Assert.False(it.MoveNext());

            it.Reset();
            for (int i = 0; i < span.Length; i++)
            {
                Assert.True(it.MoveNext());
                Assert.Equal(array[i], it.Current);
            }
            Assert.False(it.MoveNext());
        }

        [Theory]
        [InlineData(new byte[] { })]
        [InlineData(new byte[] { 0 })]
        [InlineData(new byte[] { 0, 1 })]
        [InlineData(new byte[] { 0, 1, 2 })]
        [InlineData(new byte[] { 0, 1, 2, 3 })]
        [InlineData(new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        public void CtorReadOnlySpanOverByteArrayValidCasesWithPropertiesAndBasicOperationsChecks(byte[] array)
        {
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(array);
            Assert.Equal(array.Length, span.Length);

            Assert.NotSame(array, span.CreateArray());
            Assert.False(span.Equals(array));

            ReadOnlySpan<byte>.Enumerator it = span.GetEnumerator();
            for (int i = 0; i < span.Length; i++)
            {
                Assert.True(it.MoveNext());
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);

                array[i] = unchecked((byte)(array[i] + 1));
                Assert.Equal(array[i], it.Current);
                Assert.Equal(array[i], span.Slice(i).Read<byte>());
                Assert.Equal(array[i], span.Slice(i).Read<MyByte>().Value);
            }
            Assert.False(it.MoveNext());

            it.Reset();
            for (int i = 0; i < span.Length; i++)
            {
                Assert.True(it.MoveNext());
                Assert.Equal(array[i], it.Current);
            }
            Assert.False(it.MoveNext());
        }

        [Theory]
        // copy whole buffer
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 6)]
        // copy first half to first half (length match)
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 3)]
        // copy second half to second half (length match)
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 4, 5, 6 }, 3, 3,
            new byte[] { 1, 2, 3, 7, 7, 7 }, 3, 3)]
        // copy first half to first half
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting from index 0
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting from index 3
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 3, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting at the end
        [InlineData(
            new byte[] { 7, 7, 7, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 6, 0,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 6)]
        // copy first byte of 1 element array to last position
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 6 }, 0, 1,
            new byte[] { 1, 2, 3, 4, 5, 7 }, 5, 1)]
        // copy first two bytes of 2 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 5, 6 }, 0, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy first two bytes of 3 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 5, 6, 7 }, 0, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy last two bytes of 3 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 5, 6 }, 1, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy first two bytes of 2 element array to the middle of other array
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 3, 4 }, 0, 2,
            new byte[] { 1, 2, 7, 7, 5, 6 }, 2, 3)]
        // copy one byte from the beginning at the end of other array
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 1,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 6, 0)]
        // copy two bytes from the beginning at 5th element
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 2,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1)]
        // copy one byte from the beginning at the end of other array
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 6, 0)]
        // copy two bytes from the beginning at 5th element
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 4, 2,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1)]
        public void SpanOfByteCopyToAnotherSpanOfByteTwoDifferentBuffersValidCases(byte[] expected, byte[] a, int aidx, int acount, byte[] b, int bidx, int bcount)
        {
            if (expected != null)
            {
                Span<byte> spanA = new Span<byte>(a, aidx, acount);
                Span<byte> spanB = new Span<byte>(b, bidx, bcount);

                Assert.True(spanA.TryCopyTo(spanB));
                Assert.Equal(expected, b);

                Span<byte> spanExpected = new Span<byte>(expected);
                Span<byte> spanBAll = new Span<byte>(b);
                Assert.True(spanExpected.SequenceEqual(spanBAll));
            }
            else
            {
                Span<byte> spanA = new Span<byte>(a, aidx, acount);
                Span<byte> spanB = new Span<byte>(b, bidx, bcount);
                Assert.False(spanA.TryCopyTo(spanB));
            }
        }

        [Theory]
        // copy whole buffer
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 6)]
        // copy first half to first half (length match)
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 3)]
        // copy second half to second half (length match)
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 4, 5, 6 }, 3, 3,
            new byte[] { 1, 2, 3, 7, 7, 7 }, 3, 3)]
        // copy first half to first half
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting from index 0
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting from index 3
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 3, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6)]
        // copy no bytes starting at the end
        [InlineData(
            new byte[] { 7, 7, 7, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 6, 0,
            new byte[] { 7, 7, 7, 4, 5, 6 }, 0, 6)]
        // copy first byte of 1 element array to last position
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 6 }, 0, 1,
            new byte[] { 1, 2, 3, 4, 5, 7 }, 5, 1)]
        // copy first two bytes of 2 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 5, 6 }, 0, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy first two bytes of 3 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 5, 6, 7 }, 0, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy last two bytes of 3 element array to last two positions
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 5, 6 }, 1, 2,
            new byte[] { 1, 2, 3, 4, 7, 7 }, 4, 2)]
        // copy first two bytes of 2 element array to the middle of other array
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 3, 4 }, 0, 2,
            new byte[] { 1, 2, 7, 7, 5, 6 }, 2, 3)]
        // copy one byte from the beginning at the end of other array
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 1,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 6, 0)]
        // copy two bytes from the beginning at 5th element
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 2,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1)]
        // copy one byte from the beginning at the end of other array
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 6, 0)]
        // copy two bytes from the beginning at 5th element
        [InlineData(
            (byte[])null,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 4, 2,
            new byte[] { 7, 7, 7, 7, 7, 7 }, 5, 1)]
        public void ReadOnlySpanOfByteCopyToAnotherSpanOfByteTwoDifferentBuffersValidCases(byte[] expected, byte[] a, int aidx, int acount, byte[] b, int bidx, int bcount)
        {
            if (expected != null)
            {
                ReadOnlySpan<byte> spanA = new ReadOnlySpan<byte>(a, aidx, acount);
                Span<byte> spanB = new Span<byte>(b, bidx, bcount);

                Assert.True(spanA.TryCopyTo(spanB));
                Assert.Equal(expected, b);

                ReadOnlySpan<byte> spanExpected = new ReadOnlySpan<byte>(expected);
                ReadOnlySpan<byte> spanBAll = new ReadOnlySpan<byte>(b);
                Assert.True(spanExpected.SequenceEqual(spanBAll));
            }
            else
            {
                ReadOnlySpan<byte> spanA = new ReadOnlySpan<byte>(a, aidx, acount);
                Span<byte> spanB = new Span<byte>(b, bidx, bcount);
                Assert.False(spanA.TryCopyTo(spanB));
            }

            ReadOnlySpanOfByteCopyToAnotherSpanOfByteTwoDifferentBuffersValidCasesNative(expected, a, aidx, acount, b, bidx, bcount);
        }

        public unsafe void ReadOnlySpanOfByteCopyToAnotherSpanOfByteTwoDifferentBuffersValidCasesNative(byte[] expected, byte[] a, int aidx, int acount, byte[] b, int bidx, int bcount)
        {
            IntPtr pa = Marshal.AllocHGlobal(a.Length);
            Span<byte> na = new Span<byte>(pa.ToPointer(), a.Length);
            na.Set(a);

            IntPtr pb = Marshal.AllocHGlobal(b.Length);
            Span<byte> nb = new Span<byte>(pb.ToPointer(), b.Length);
            nb.Set(b);

            ReadOnlySpan<byte> spanA = na.Slice(aidx, acount);
            Span<byte> spanB = nb.Slice(bidx, bcount);

            if (expected != null) {               
                Assert.True(spanA.TryCopyTo(spanB));
                Assert.Equal(expected, b);

                ReadOnlySpan<byte> spanExpected = new ReadOnlySpan<byte>(expected);
                ReadOnlySpan<byte> spanBAll = new ReadOnlySpan<byte>(b);
                Assert.True(spanExpected.SequenceEqual(spanBAll));
            }
            else {
                Assert.False(spanA.TryCopyTo(spanB));
            }

            Marshal.FreeHGlobal(pa);
            Marshal.FreeHGlobal(pb);
        }

        [Theory]
        // copy whole buffer
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6,
            new byte[] { 7, 7, 7, 7, 7, 7 })]
        // copy first half
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 })]
        // copy second half
        [InlineData(
            new byte[] { 4, 5, 6, 7, 7, 7 },
            new byte[] { 7, 7, 7, 4, 5, 6 }, 3, 3,
            new byte[] { 7, 7, 7, 7, 7, 7 })]
        // copy no bytes starting from index 0
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 })]
        // copy no bytes starting from index 3
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 3, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 })]
        // copy no bytes starting at the end
        [InlineData(
            new byte[] { 7, 7, 7, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 6, 0,
            new byte[] { 7, 7, 7, 4, 5, 6 })]
        // copy first byte of 1 element array
        [InlineData(
            new byte[] { 6, 2, 3, 4, 5, 6 },
            new byte[] { 6 }, 0, 1,
            new byte[] { 1, 2, 3, 4, 5, 6 })]       
        public void SpanCopyToArrayTwoDifferentBuffersValidCases(byte[] expected, byte[] a, int aidx, int acount, byte[] b)
        {
            if (expected != null)
            {
                Span<byte> spanA = new Span<byte>(a, aidx, acount);

                Assert.True(spanA.TryCopyTo(b));
                Assert.Equal(expected, b);

                Span<byte> spanExpected = new Span<byte>(expected);
                Span<byte> spanBAll = new Span<byte>(b);
                Assert.True(spanExpected.SequenceEqual(spanBAll));
            }
            else
            {
                Span<byte> spanA = new Span<byte>(a, aidx, acount);
                Assert.False(spanA.TryCopyTo(b));
            }
        }

        [Theory]
        // copy whole buffer
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6,
            new byte[] { 7, 7, 7, 7, 7, 7 })]
        // copy first half
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 0, 3,
            new byte[] { 7, 7, 7, 4, 5, 6 })]
        // copy second half
        [InlineData(
            new byte[] { 4, 5, 6, 7, 7, 7 },
            new byte[] { 7, 7, 7, 4, 5, 6 }, 3, 3,
            new byte[] { 7, 7, 7, 7, 7, 7 })]
        // copy no bytes starting from index 0
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 0, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 })]
        // copy no bytes starting from index 3
        [InlineData(
            new byte[] { 1, 2, 3, 4, 5, 6 },
            new byte[] { 7, 7, 7, 7, 7, 7 }, 3, 0,
            new byte[] { 1, 2, 3, 4, 5, 6 })]
        // copy no bytes starting at the end
        [InlineData(
            new byte[] { 7, 7, 7, 4, 5, 6 },
            new byte[] { 1, 2, 3, 7, 7, 7 }, 6, 0,
            new byte[] { 7, 7, 7, 4, 5, 6 })]
        // copy first byte of 1 element array
        [InlineData(
            new byte[] { 6, 2, 3, 4, 5, 6 },
            new byte[] { 6 }, 0, 1,
            new byte[] { 1, 2, 3, 4, 5, 6 })]
        public void ROSpanCopyToArrayTwoDifferentBuffersValidCases(byte[] expected, byte[] a, int aidx, int acount, byte[] b)
        {
            if (expected != null)
            {
                ReadOnlySpan<byte> spanA = new ReadOnlySpan<byte>(a, aidx, acount);

                Assert.True(spanA.TryCopyTo(b));
                Assert.Equal(expected, b);

                ReadOnlySpan<byte> spanExpected = new ReadOnlySpan<byte>(expected);
                ReadOnlySpan<byte> spanBAll = new ReadOnlySpan<byte>(b);
                Assert.True(spanExpected.SequenceEqual(spanBAll));
            }
            else
            {
                ReadOnlySpan<byte> spanA = new ReadOnlySpan<byte>(a, aidx, acount);
                Assert.False(spanA.TryCopyTo(b));
            }
        }
    }
}