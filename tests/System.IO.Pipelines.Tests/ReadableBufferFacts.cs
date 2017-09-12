// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO.Pipelines.Testing;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class ReadableBufferFacts
    {
        [Fact]
        public void EmptyIsCorrect()
        {
            var buffer = BufferUtilities.CreateBuffer(0, 0);
            Assert.Equal(0, buffer.Length);
            Assert.True(buffer.IsEmpty);
        }

        [Theory]
        [MemberData(nameof(OutOfRangeSliceCases))]
        public void ReadableBufferDoesNotAllowSlicingOutOfRange(Action<ReadableBuffer> fail)
        {
            foreach (var p in Size100ReadableBuffers)
            {
                var buffer = (ReadableBuffer) p[0];
                var ex = Assert.Throws<InvalidOperationException>(() => fail(buffer));
            }
        }

        [Theory]
        [MemberData(nameof(Size100ReadableBuffers))]
        public void ReadableBufferMove_MovesReadCursor(ReadableBuffer buffer)
        {
            var cursor = buffer.Move(buffer.Start, 65);
            Assert.Equal(buffer.Slice(65).Start, cursor);
        }

        [Theory]
        [MemberData(nameof(Size100ReadableBuffers))]
        public void ReadableBufferMove_ChecksBounds(ReadableBuffer buffer)
        {
            Assert.Throws<InvalidOperationException>(() => buffer.Move(buffer.Start, 101));
        }

        [Fact]
        public void ReadableBufferMove_DoesNotAlowNegative()
        {
            var data = new byte[20];
            var buffer = ReadableBuffer.Create(data);
            Assert.Throws<ArgumentOutOfRangeException>(() => buffer.Move(buffer.Start, -1));
        }

        [Fact]
        public void ReadCursorSeekChecksEndIfNotTrustingEnd()
        {
            var buffer = BufferUtilities.CreateBuffer(1, 1, 1);
            var buffer2 = BufferUtilities.CreateBuffer(1, 1, 1);
            Assert.Throws<InvalidOperationException>(() => buffer.Start.Seek(2, buffer2.End, true));
        }

        [Fact]
        public void ReadCursorSeekDoesNotCheckEndIfTrustingEnd()
        {
            var buffer = BufferUtilities.CreateBuffer(1, 1, 1);
            var buffer2 = BufferUtilities.CreateBuffer(1, 1, 1);
            buffer.Start.Seek(2, buffer2.End, false);
        }

        public static TheoryData<ReadableBuffer> Size100ReadableBuffers => new TheoryData<ReadableBuffer>
        {
            BufferUtilities.CreateBuffer(100),
            BufferUtilities.CreateBuffer(50, 50),
            BufferUtilities.CreateBuffer(33, 33, 34)
        };

        public static TheoryData<Action<ReadableBuffer>> OutOfRangeSliceCases => new TheoryData<Action<ReadableBuffer>>
        {
            b => b.Slice(101),
            b => b.Slice(0, 101),
            b => b.Slice(b.Start, 101),
            b => b.Slice(0, 70).Slice(b.End, b.End),
            b => b.Slice(0, 70).Slice(b.Start, b.End),
            b => b.Slice(0, 70).Slice(0, b.End),
            b => b.Slice(70, b.Start)
        };
    }
}
