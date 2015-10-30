using System.Buffers;
using System.Collections.Generic;
using Xunit;

namespace System.Text.Utf8.Tests
{
    public class Utf8StringTests
    {
        [Fact]
        public void NativePoolBasicsLegacy()
        {
            using (var pool = new NativeBufferPool(256, 10))
            {
                var buffers = new List<ByteSpan>();
                for (byte i = 0; i < 10; i++)
                {
                    var buffer = pool.RentLegacy();
                    buffers.Add(buffer);
                    for (int bi = 0; bi < buffer.Length; bi++)
                    {
                        buffer[bi] = i;
                    }
                }
                for (byte i = 0; i < 10; i++)
                {
                    var buffer = buffers[i];
                    for (int bi = 0; bi < buffer.Length; bi++)
                    {
                        Assert.Equal(i, buffer[bi]);
                    }
                    pool.Return(buffer);
                }
            }
        }

        [Fact]
        public void NativePoolBasics()
        {
            using (var pool = new NativeBufferPool(256, 10))
            {
                var buffers = new List<Span<byte>>();
                for (byte i = 0; i < 10; i++)
                {
                    var buffer = pool.Rent();
                    buffers.Add(buffer);
                    for (int bi = 0; bi < buffer.Length; bi++)
                    {
                        buffer[bi] = i;
                    }
                }
                for (byte i = 0; i < 10; i++)
                {
                    var buffer = buffers[i];
                    for (int bi = 0; bi < buffer.Length; bi++)
                    {
                        Assert.Equal(i, buffer[bi]);
                    }
                    pool.Return(buffer);
                }
            }
        }

        [Fact]
        public void ByteSpanEmptyCreateArrayTest()
        {
            var empty = ByteSpan.Empty;
            var array = empty.CreateArray();
            Assert.Equal(0, array.Length);
        }
        
        [Fact]
        public unsafe void ByteSpanEqualsTestsTwoDifferentInstancesOfBuffersWithSameValues()
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
                ByteSpan b1 = new ByteSpan(buffer1pinned, bufferLength);
                ByteSpan b2 = new ByteSpan(buffer2pinned, bufferLength);
                
                for (int i = 0; i < bufferLength; i++)
                {
                    Assert.True(b1.Slice(i).Equals(b2.Slice(i)));
                }
            }
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
                ByteSpan b1 = new ByteSpan(buffer1pinned, bufferLength);
                ByteSpan b2 = new ByteSpan(buffer2pinned, bufferLength);
                
                for (int i = 0; i < bufferLength; i++)
                {
                    for (int diffPosition = i; diffPosition < bufferLength; diffPosition++)
                    {
                        buffer1[diffPosition] = unchecked((byte)(buffer1[diffPosition] + 1));
                        Assert.False(b1.Slice(i).Equals(b2.Slice(i)));
                    }
                }
            }
        }
    }
}
