// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Buffers;
using System.Runtime.InteropServices;
using Xunit;

namespace System.Slices.Tests
{
    public class SlicesTests
    {
        [Fact]
        public void ByteSpanEmptyCreateArrayTest()
        {
            var empty = Span<byte>.Empty;
            var array = empty.ToArray();
            Assert.Equal(0, array.Length);
        }

        [Fact]
        public void ByteReadOnlySpanEmptyCreateArrayTest()
        {
            var empty = ReadOnlySpan<byte>.Empty;
            var array = empty.ToArray();
            Assert.Equal(0, array.Length);
        }

        [Fact]
        public unsafe void ByteSpanEqualsTestsTwoDifferentInstancesOfBuffersWithOneValueDifferent()
        {
            const int bufferLength = 128;
            byte[] buffer1 = new byte[bufferLength];
            byte[] buffer2 = new byte[bufferLength];

            for (int i = 0; i < bufferLength; i++)
            {
                buffer1[i] = (byte)(bufferLength + 1 - i);
                buffer2[i] = (byte)(bufferLength + 1 - i);
            }

            fixed (byte* buffer1pinned = buffer1)
            fixed (byte* buffer2pinned = buffer2)
            {
                Span<byte> b1 = new Span<byte>(buffer1pinned, bufferLength);
                Span<byte> b2 = new Span<byte>(buffer2pinned, bufferLength);

                for (int i = 0; i < bufferLength; i++)
                {
                    for (int diffPosition = i; diffPosition < bufferLength; diffPosition++)
                    {
                        buffer1[diffPosition] = unchecked((byte)(buffer1[diffPosition] + 1));
                        Assert.False(b1.Slice(i).SequenceEqual(b2.Slice(i)));
                    }
                }
            }
        }

        [Fact]
        public unsafe void ByteReadOnlySpanEqualsTestsTwoDifferentInstancesOfBuffersWithOneValueDifferent()
        {
            const int bufferLength = 128;
            byte[] buffer1 = new byte[bufferLength];
            byte[] buffer2 = new byte[bufferLength];

            for (int i = 0; i < bufferLength; i++)
            {
                buffer1[i] = (byte)(bufferLength + 1 - i);
                buffer2[i] = (byte)(bufferLength + 1 - i);
            }

            fixed (byte* buffer1pinned = buffer1)
            fixed (byte* buffer2pinned = buffer2)
            {
                ReadOnlySpan<byte> b1 = new ReadOnlySpan<byte>(buffer1pinned, bufferLength);
                ReadOnlySpan<byte> b2 = new ReadOnlySpan<byte>(buffer2pinned, bufferLength);

                for (int i = 0; i < bufferLength; i++)
                {
                    for (int diffPosition = i; diffPosition < bufferLength; diffPosition++)
                    {
                        buffer1[diffPosition] = unchecked((byte)(buffer1[diffPosition] + 1));
                        Assert.False(b1.Slice(i).SequenceEqual(b2.Slice(i)));
                    }
                }
            }
        }

        [Theory]
        [InlineData(new byte[0], 0, 0)]
        [InlineData(new byte[1] { 0 }, 0, 0)]
        [InlineData(new byte[1] { 0 }, 0, 1)]
        [InlineData(new byte[2] { 0, 0 }, 0, 2)]
        [InlineData(new byte[2] { 0, 0 }, 0, 1)]
        [InlineData(new byte[2] { 0, 0 }, 1, 1)]
        [InlineData(new byte[2] { 0, 0 }, 2, 0)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0, 3)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 1)]
        public void ByteSpanCtorWithRangeValidCases(byte[] bytes, int start, int length)
        {
            Span<byte> span = new Span<byte>(bytes, start, length);
        }

        [Theory]
        [InlineData(new byte[0], 0, 0)]
        [InlineData(new byte[1] { 0 }, 0, 0)]
        [InlineData(new byte[1] { 0 }, 0, 1)]
        [InlineData(new byte[2] { 0, 0 }, 0, 2)]
        [InlineData(new byte[2] { 0, 0 }, 0, 1)]
        [InlineData(new byte[2] { 0, 0 }, 1, 1)]
        [InlineData(new byte[2] { 0, 0 }, 2, 0)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0, 3)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 1)]
        public void ByteReadOnlySpanCtorWithRangeValidCases(byte[] bytes, int start, int length)
        {
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(bytes, start, length);
        }

        [Fact]
        public void ByteSpanCtorWithRangeThrowsArgumentExceptionOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => { Span<byte> span = new Span<byte>(null, 0, 0); });
        }

        [Fact]
        public void ByteReadOnlySpanCtorWithRangeThrowsArgumentExceptionOnNull()
        {
            Assert.Throws<ArgumentNullException>(() => { ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(null, 0, 0); });
        }

        [Theory]
        [InlineData(new byte[0], 1, 0)]
        [InlineData(new byte[0], 1, -1)]
        [InlineData(new byte[0], 0, 1)]
        [InlineData(new byte[0], -1, 0)]
        [InlineData(new byte[0], 5, 5)]
        [InlineData(new byte[1] { 0 }, 0, 2)]
        [InlineData(new byte[1] { 0 }, 1, 1)]
        [InlineData(new byte[1] { 0 }, -1, 2)]
        [InlineData(new byte[2] { 0, 0 }, 0, 3)]
        [InlineData(new byte[2] { 0, 0 }, 1, 2)]
        [InlineData(new byte[2] { 0, 0 }, 2, 1)]
        [InlineData(new byte[2] { 0, 0 }, 3, 0)]
        [InlineData(new byte[2] { 0, 0 }, 1, -1)]
        [InlineData(new byte[2] { 0, 0 }, 2, int.MaxValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MinValue, int.MinValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MaxValue, int.MaxValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MinValue, int.MaxValue)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 3)]
        [InlineData(new byte[3] { 0, 0, 0 }, 2, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 15, 0)]
        public void ByteSpanCtorWithRangeThrowsArgumentOutOfRangeException(byte[] bytes, int start, int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { Span<byte> span = new Span<byte>(bytes, start, length); });
        }

        [Theory]
        [InlineData(new byte[0], 0, 0)]
        [InlineData(new byte[1] { 0 }, 0, 0)]
        [InlineData(new byte[1] { 0 }, 0, 1)]
        [InlineData(new byte[2] { 0, 0 }, 0, 2)]
        [InlineData(new byte[2] { 0, 0 }, 0, 1)]
        [InlineData(new byte[2] { 0, 0 }, 1, 1)]
        [InlineData(new byte[2] { 0, 0 }, 2, 0)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0, 3)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 1)]
        public void ByteSpanSliceWithRangeValidCases(byte[] bytes, int start, int length)
        {
            Span<byte> span = new Span<byte>(bytes, start, length);
        }

        [Theory]
        [InlineData(new byte[0], 1, 0)]
        [InlineData(new byte[0], 1, -1)]
        [InlineData(new byte[0], 0, 1)]
        [InlineData(new byte[0], -1, 0)]
        [InlineData(new byte[0], 5, 5)]
        [InlineData(new byte[1] { 0 }, 0, 2)]
        [InlineData(new byte[1] { 0 }, 1, 1)]
        [InlineData(new byte[1] { 0 }, -1, 2)]
        [InlineData(new byte[2] { 0, 0 }, 0, 3)]
        [InlineData(new byte[2] { 0, 0 }, 1, 2)]
        [InlineData(new byte[2] { 0, 0 }, 2, 1)]
        [InlineData(new byte[2] { 0, 0 }, 3, 0)]
        [InlineData(new byte[2] { 0, 0 }, 1, -1)]
        [InlineData(new byte[2] { 0, 0 }, 2, int.MaxValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MinValue, int.MinValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MaxValue, int.MaxValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MinValue, int.MaxValue)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1, 3)]
        [InlineData(new byte[3] { 0, 0, 0 }, 2, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 15, 0)]
        public void ByteSpanSliceWithRangeThrowsArgumentOutOfRangeException1(byte[] bytes, int start, int length)
        {
            var span = new Span<byte>(bytes);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Span<byte> slice = span.Slice(start, length);
            });
        }

        [Theory]
        [InlineData(new byte[0], 0)]
        [InlineData(new byte[1] { 0 }, 0)]
        [InlineData(new byte[1] { 0 }, 1)]
        [InlineData(new byte[2] { 0, 0 }, 0)]
        [InlineData(new byte[2] { 0, 0 }, 1)]
        [InlineData(new byte[2] { 0, 0 }, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 0)]
        [InlineData(new byte[3] { 0, 0, 0 }, 1)]
        [InlineData(new byte[3] { 0, 0, 0 }, 2)]
        [InlineData(new byte[3] { 0, 0, 0 }, 3)]
        public void ByteSpanSliceWithStartRangeValidCases(byte[] bytes, int start)
        {
            Span<byte> span = new Span<byte>(bytes).Slice(start);
        }

        [Theory]
        [InlineData(new byte[0], int.MinValue)]
        [InlineData(new byte[0], -1)]
        [InlineData(new byte[0], 1)]
        [InlineData(new byte[0], int.MaxValue)]
        [InlineData(new byte[1] { 0 }, int.MinValue)]
        [InlineData(new byte[1] { 0 }, -1)]
        [InlineData(new byte[1] { 0 }, 2)]
        [InlineData(new byte[1] { 0 }, int.MaxValue)]
        [InlineData(new byte[2] { 0, 0 }, int.MinValue)]
        [InlineData(new byte[2] { 0, 0 }, -1)]
        [InlineData(new byte[2] { 0, 0 }, 3)]
        [InlineData(new byte[2] { 0, 0 }, int.MaxValue)]
        public void ByteSpanSliceWithStartRangeThrowsArgumentOutOfRangeException(byte[] bytes, int start)
        {
            var span = new Span<byte>(bytes);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                Span<byte> slice = span.Slice(start);
            });
        }

        public void ByteReadOnlySpanCtorWithRangeThrowsArgumentOutOfRangeException(byte[] bytes, int start, int length)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(bytes, start, length); });
        }

        [Fact]
        public void SetSpan()
        {
            var destination = new Span<byte>(new byte[100]);
            var source = new Span<byte>(new byte[] { 1, 2, 3 });
            source.CopyTo(destination);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.Equal(source[i], destination[i]);
            }
        }

        [Fact]
        public void SetArray()
        {
            var destination = new Span<byte>(new byte[100]);
            var source = new byte[] { 1, 2, 3 };
            source.CopyTo(destination);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.Equal(source[i], destination[i]);
            }
        }

        [Fact]
        public void CovariantSlicesNotSupported1()
        {
            object[] array = new string[10];
            Assert.Throws<ArrayTypeMismatchException>(() => { var slice = new Span<object>(array); });
        }

        [Fact]
        public void CovariantSlicesNotSupported2()
        {
            object[] array = new string[10];
            Assert.Throws<ArrayTypeMismatchException>(() => { var slice = array.Slice(0); });
        }

        [Fact]
        public void CovariantSlicesNotSupported3()
        {
            object[] array = new string[10];
            Assert.Throws<ArrayTypeMismatchException>(() => { var slice = new Span<object>(array, 0, 10); });
        }

        #region ReadOnlySpanStartsWithByte
        [Fact]
        public static void ZeroLengthStartsWith_Byte()
        {
            byte[] a = { 4, 5, 6 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(a, 2, 0);
            Assert.True(bytes.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithNoMatchEmptySpan_Byte()
        {
            byte[] a = { 4, 5, 6 };
            byte[] b = { 1, 2, 3 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a, 0, 0);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(b, 0, 1);
            Assert.False(bytes.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithMatchEmptySpans_Byte()
        {
            byte[] a = { 4, 5, 6 };
            byte[] b = { 1, 2, 3 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a, 0, 0);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(b, 0, 0);
            Assert.True(bytes.StartsWith(slice));
        }

        [Fact]
        public static void SameSpanStartsWith_Byte()
        {
            byte[] a = { 4, 5, 6 };
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);
            Assert.True(span.StartsWith(span));
        }

        [Fact]
        public static void SameSpanValuesStartsWith_Byte()
        {
            byte[] a = { 4, 5, 6 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(a);
            Assert.True(bytes.StartsWith(slice));
        }

        [Fact]
        public static void LengthMismatchStartsWith_Byte()
        {
            byte[] a = { 4, 5, 6 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a, 0, 2);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(a);
            Assert.False(bytes.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithMatch_Byte()
        {
            byte[] a = { 4, 5, 6 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(a, 0, 1);
            Assert.True(bytes.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithNoMatch_Byte()
        {
            byte[] a = { 4, 5, 6 };
            byte[] b = { 1, 2, 3 };
            ReadOnlySpan<byte> bytes = new ReadOnlySpan<byte>(a);
            ReadOnlySpan<byte> slice = new ReadOnlySpan<byte>(b, 0, 1);
            Assert.False(bytes.StartsWith(slice));
        }
        #endregion

        #region ReadOnlySpanStartsWithChar
        [Fact]
        public static void ZeroLengthStartsWith_Char()
        {
            char[] a = { '4', '5', '6' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(a, 2, 0);
            Assert.True(str.StartsWith(value));
        }

        [Fact]
        public static void StartsWithNoMatchEmptySpan_Char()
        {
            char[] a = { '4', '5', '6' };
            char[] b = { '1', '2', '3' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a, 0, 0);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(b, 0, 1);
            Assert.False(str.StartsWith(value));
        }

        [Fact]
        public static void StartsWithMatchEmptySpans_Char()
        {
            char[] a = { '4', '5', '6' };
            char[] b = { '1', '2', '3' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a, 0, 0);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(b, 0, 0);
            Assert.True(str.StartsWith(value));
        }

        [Fact]
        public static void SameSpanStartsWith_Char()
        {
            char[] a = { '4', '5', '6' };
            ReadOnlySpan<char> span = new ReadOnlySpan<char>(a);
            Assert.True(span.StartsWith(span));
        }

        [Fact]
        public static void SameSpanValuesStartsWith_Char()
        {
            char[] a = { '4', '5', '6' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(a);
            Assert.True(str.StartsWith(value));
        }

        [Fact]
        public static void LengthMismatchStartsWith_Char()
        {
            char[] a = { '4', '5', '6' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a, 0, 2);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(a);
            Assert.False(str.StartsWith(value));
        }

        [Fact]
        public static void StartsWithMatch_Char()
        {
            char[] a = { '4', '5', '6' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(a, 0, 1);
            Assert.True(str.StartsWith(value));
        }

        [Fact]
        public static void StartsWithNoMatch_Char()
        {
            char[] a = { '4', '5', '6' };
            char[] b = { '1', '2', '3' };
            ReadOnlySpan<char> str = new ReadOnlySpan<char>(a);
            ReadOnlySpan<char> value = new ReadOnlySpan<char>(b, 0, 1);
            Assert.False(str.StartsWith(value));
        }
        #endregion

        #region ReadOnlySpanStartsWith<T>
        [Fact]
        public static void ZeroLengthStartsWith()
        {
            int[] a = { 4, 5, 6 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(a, 2, 0);
            Assert.True(items.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithNoMatchEmptySpan()
        {
            int[] a = { 4, 5, 6 };
            int[] b = { 1, 2, 3 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a, 0, 0);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(b, 0, 1);
            Assert.False(items.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithMatchEmptySpans()
        {
            int[] a = { 4, 5, 6 };
            int[] b = { 1, 2, 3 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a, 0, 0);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(b, 0, 0);
            Assert.True(items.StartsWith(slice));
        }

        [Fact]
        public static void SameSpanStartsWith()
        {
            int[] a = { 4, 5, 6 };
            ReadOnlySpan<int> span = new ReadOnlySpan<int>(a);
            Assert.True(span.StartsWith(span));
        }

        [Fact]
        public static void SameSpanValuesStartsWith()
        {
            int[] a = { 4, 5, 6 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(a);
            Assert.True(items.StartsWith(slice));
        }

        [Fact]
        public static void LengthMismatchStartsWith()
        {
            int[] a = { 4, 5, 6 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a, 0, 2);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(a);
            Assert.False(items.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithMatch()
        {
            int[] a = { 4, 5, 6 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(a, 0, 1);
            Assert.True(items.StartsWith(slice));
        }

        [Fact]
        public static void StartsWithNoMatch()
        {
            int[] a = { 4, 5, 6 };
            int[] b = { 1, 2, 3 };
            ReadOnlySpan<int> items = new ReadOnlySpan<int>(a);
            ReadOnlySpan<int> slice = new ReadOnlySpan<int>(b, 0, 1);
            Assert.False(items.StartsWith(slice));
        }
        #endregion

        #region SpanIndexOfByte
        [Fact]
        public static void TestMatchWithIndexAndCount_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            Span<byte> span = new Span<byte>(a);

            int idx = span.IndexOf(45, 10, 99);
            Assert.Equal(50, idx);
        }

        [Fact]
        public static void TestMatchWithIndexAndCountGreaterThanLength_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            Span<byte> span = new Span<byte>(a);

            int idx = span.IndexOf(45, 75, 99);
            Assert.Equal(50, idx);
        }

        [Fact]
        public static void TestIndexOfComparison()
        {
            byte[] a = new byte[1000];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = (byte)(i + 1);
            }

            var bytes = new Span<byte>(a);
            Assert.Equal(bytes.IndexOf(250, 10, 255) - 250, bytes.Slice(250, 10).IndexOf(255));
        }

        [Fact]
        public static void TestMatchWithCountGreaterThanLength_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            Span<byte> span = new Span<byte>(a);

            int idx = span.IndexOf(0, 105, 99);
            Assert.Equal(50, idx);
        }

        [Fact]
        public static void TestNoMatchWithCountGreaterThanLength_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            Span<byte> span = new Span<byte>(a);

            int idx = span.IndexOf(0, 105, 5);
            Assert.Equal(-1, idx);
        }

        [Fact]
        public static void TestNoMatchWithIndexAndCount_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);

            int idx = span.IndexOf(45, 3, 99);
            Assert.Equal(-1, idx);
        }

        [Fact]
        public static void StartIndexTooLargeIndexOf_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            Span<byte> span = new Span<byte>(a);
            int idx = span.IndexOf(length + 1, 10, 99);
            Assert.Equal(-1, idx);
        }

        public static void ZeroCountIndexOf_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            Span<byte> span = new Span<byte>(a);
            int idx = span.IndexOf(0, 0, 99);
            Assert.Equal(-1, idx);
        }
        #endregion

        #region ReadOnlySpanIndexOfByte
        [Fact]
        public static void TestMatchWithIndexAndCountReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);

            int idx = span.IndexOf(45, 10, 99);
            Assert.Equal(50, idx);
        }

        [Fact]
        public static void TestMatchWithIndexAndCountGreaterThanLengthReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);

            int idx = span.IndexOf(45, 75, 99);
            Assert.Equal(50, idx);
        }

        [Fact]
        public static void TestMatchWithCountGreaterThanLengthReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);

            int idx = span.IndexOf(0, 105, 99);
            Assert.Equal(50, idx);
        }

        [Fact]
        public static void TestNoMatchWithCountGreaterThanLengthReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);

            int idx = span.IndexOf(0, 105, 5);
            Assert.Equal(-1, idx);
        }

        [Fact]
        public static void TestNoMatchWithIndexAndCountReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);

            int idx = span.IndexOf(45, 3, 99);
            Assert.Equal(-1, idx);
        }

        [Fact]
        public static void StartIndexTooLargeIndexOfReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);
            int idx = span.IndexOf(length + 1, 10, 99);
            Assert.Equal(-1, idx);
        }

        public static void ZeroCountIndexOfReadOnly_Byte()
        {
            int length = 100;
            byte[] a = new byte[length];
            a[50] = 99;
            ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(a);
            int idx = span.IndexOf(0, 0, 99);
            Assert.Equal(-1, idx);
        }
        #endregion
    }
}
