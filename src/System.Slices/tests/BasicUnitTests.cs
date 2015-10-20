using System.Collections.Generic;
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
                Span<byte> b1 = new Span<byte>(buffer1pinned, bufferLength);
                Span<byte> b2 = new Span<byte>(buffer2pinned, bufferLength);
                
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
                Span<byte> b1 = new Span<byte>(buffer1pinned, bufferLength);
                Span<byte> b2 = new Span<byte>(buffer2pinned, bufferLength);
                
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
