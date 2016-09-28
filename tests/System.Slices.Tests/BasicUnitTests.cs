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
            var array = empty.CreateArray();
            Assert.Equal(0, array.Length);
        }

        [Fact]
        public void ByteReadOnlySpanEmptyCreateArrayTest()
        {
            var empty = ReadOnlySpan<byte>.Empty;
            var array = empty.CreateArray();
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
            Assert.Throws<ArgumentException>(() => { Span<byte> span = new Span<byte>(null, 0, 0); });
        }

        [Fact]
        public void ByteReadOnlySpanCtorWithRangeThrowsArgumentExceptionOnNull()
        {
            Assert.Throws<ArgumentException>(() => { ReadOnlySpan<byte> span = new ReadOnlySpan<byte>(null, 0, 0); });
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
            destination.Set(source);
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
            destination.Set(source);
            for (int i = 0; i < source.Length; i++)
            {
                Assert.Equal(source[i], destination[i]);
            }
        }

        [Fact]
        public void GetArrayOrPointer()
        {
            var original = new int[] { 1, 2, 3 };
            ArraySegment<int> array;
            Memory<int> slice;

            slice = new Memory<int>(original, 1, 2);
            Assert.True(slice.TryGetArray(out array));
            Assert.Equal(2, array.Array[array.Offset + 0]);
            Assert.Equal(3, array.Array[array.Offset + 1]);

            slice = new Memory<int>(original, 0, 3);
            Assert.True(slice.TryGetArray(out array));
            Assert.Equal(1, array.Array[array.Offset + 0]);
            Assert.Equal(2, array.Array[array.Offset + 1]);
            Assert.Equal(3, array.Array[array.Offset + 2]);

            slice = new Memory<int>(original, 0, 0);
            Assert.True(slice.TryGetArray(out array));
            Assert.Equal(0, array.Offset);
            Assert.Equal(original, array.Array);
            Assert.Equal(0, array.Count);

            unsafe
            {
                fixed (int* pBytes = original)
                {
                    slice = new Memory<int>(pBytes, 1);
                    Assert.False(slice.TryGetArray(out array));
                    void* p;
                    Assert.True(slice.TryGetPointer(out p));
                    Assert.True(null != p);
                    Assert.Equal(null, array.Array);
                    Assert.Equal(0, array.Offset);
                    Assert.Equal(0, array.Count);
                }
            }

            unsafe
            {
                fixed (int* pBytes = original)
                {
                    var roSlice = new ReadOnlySpan<int>(pBytes, 1);
                    Assert.False(slice.TryGetArray(out array));
                    void* p;
                    Assert.True(slice.TryGetPointer(out p));
                    Assert.True(null != p);
                    Assert.Equal(null, array.Array);
                    Assert.Equal(0, array.Offset);
                    Assert.Equal(0, array.Count);
                }
            }
        }

        [Fact]
        public void TryGetPointerReturnsFalseIfNotPinned()
        {
            var data = new byte[10];
            var memory = new Memory<byte>(data, 0, data.Length);
            unsafe
            {
                void* pointer;
                Assert.False(memory.TryGetPointer(out pointer));
            }
        }

        [Fact]
        public void TryGetPointerReturnsTrueIfPointerUsed()
        {
            unsafe
            {
                IntPtr raw = Marshal.AllocHGlobal(10);
                var memory = new Memory<byte>((void*)raw, 10);
                void* pointer;
                Assert.True(memory.TryGetPointer(out pointer));
                Assert.True(raw.ToPointer() == pointer);
                Marshal.FreeHGlobal(raw);
            }
        }

        [Fact]
        public void TryGetPointerReturnsTrueIfArrayAndPrePinned()
        {
            unsafe
            {
                var data = new byte[10];
                fixed (byte* ptr = data)
                {
                    var memory = new Memory<byte>(data, 0, data.Length, ptr);
                    void* pointer;
                    Assert.True(memory.TryGetPointer(out pointer));
                    Assert.True(ptr == pointer);
                }
            }
        }

        [Fact]
        public void MemoryCtorThrowsIfPtrDoesNotMatchArrayArgs()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                unsafe
                {
                    var data = new byte[10];

                    fixed (byte* ptr = data)
                    {
                        var memory = new Memory<byte>(data, 0, data.Length, ptr + 1);
                    }
                }
            });
        }

        [Fact]
        public void MemoryCtorSubArrayWithPinnedPtr()
        {
            unsafe
            {
                var data = new byte[10];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (byte)i;
                }

                fixed (byte* ptr = data)
                {
                    var memory = new Memory<byte>(data, 5, 5, ptr + 5);
                    Assert.Equal(5, memory.Length);
                    var span = memory.Span;
                    for (int i = 0; i < 5; i++)
                    {
                        Assert.Equal(span[i], i + 5);
                    }
                }
            }
        }

        [Fact]
        public void SliceArrayBackedMemory()
        {
            var data = new byte[10];

            for (int i = 0; i < 10; i++)
            {
                data[i] = (byte)i;
            }

            var memory = new Memory<byte>(data, 0, data.Length);
            var slice = memory.Slice(0, 5);
            var span = slice.Span;
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(i, span[i]);
            }

            var subSlice = slice.Slice(2, 1);
            Assert.Equal(2, subSlice.Span[0]);
        }

        [Fact]
        public void SlicePointerBackedMemory()
        {
            unsafe
            {
                var data = new byte[10];
                for (int i = 0; i < 10; i++)
                {
                    data[i] = (byte)i;
                }

                fixed (byte* ptr = data)
                {
                    var memory = new Memory<byte>(ptr, data.Length);
                    var slice = memory.Slice(0, 5);
                    var span = slice.Span;
                    for (int i = 0; i < 5; i++)
                    {
                        Assert.Equal(i, span[i]);
                    }

                    var subSlice = slice.Slice(2, 1);
                    Assert.Equal(2, subSlice.Span[0]);
                }
            }
        }

        [Fact]
        public void CovariantSlicesNotSupported1()
        {
            object[] array = new string[10];
            Assert.Throws<ArgumentException>(() => { var slice = new Span<object>(array); });
        }

        [Fact]
        public void CovariantSlicesNotSupported2()
        {
            object[] array = new string[10];
            Assert.Throws<ArgumentException>(() => { var slice = array.Slice(0); });
        }

        [Fact]
        public void CovariantSlicesNotSupported3()
        {
            object[] array = new string[10];
            Assert.Throws<ArgumentException>(() => { var slice = new Span<object>(array, 0, 10); });
        }
    }
}