// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO.Pipelines.Testing;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeLengthTests : IDisposable
    {
        private MemoryPool _pool;
        private ResetablePipe _pipe;

        public PipeLengthTests()
        {
            _pool = new MemoryPool();
            _pipe = new ResetablePipe(new PipeOptions(_pool));
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pool?.Dispose();
        }

        [Fact]
        public void LengthCorrectAfterAllocAdvanceCommit()
        {
            var writableBuffer = _pipe.Writer.WriteEmpty(10);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthCorrectAfterAlloc0AdvanceCommit()
        {
            _pipe.Writer.GetMemory(0);
            _pipe.Writer.WriteEmpty(10);
            _pipe.Writer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthDecreasedAfterReadAdvanceConsume()
        {
            _pipe.Writer.GetMemory(100);
            _pipe.Writer.Advance(10);
            _pipe.Writer.Commit();
            _pipe.Writer.FlushAsync();

            var result = _pipe.Reader.ReadAsync().GetResult();
            var consumed = result.Buffer.Slice(5).Start;
            _pipe.Reader.Advance(consumed, consumed);

            Assert.Equal(5, _pipe.Length);
        }

        [Fact]
        public void LengthNotChangeAfterReadAdvanceExamine()
        {
            var writableBuffer = _pipe.Writer.WriteEmpty(10);
            writableBuffer.Commit();
            writableBuffer.FlushAsync();

            var result = _pipe.Reader.ReadAsync().GetResult();
            _pipe.Reader.Advance(result.Buffer.Start, result.Buffer.End);

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void ByteByByteTest()
        {

            for (int i = 1; i <= 1024 * 1024; i++)
            {
                _pipe.Writer.GetMemory(100);
                _pipe.Writer.Advance(1);
                _pipe.Writer.Commit();

                Assert.Equal(i, _pipe.Length);
            }

            _pipe.Writer.FlushAsync();

            for (int i = 1024 * 1024 - 1; i >= 0; i--)
            {
                var result = _pipe.Reader.ReadAsync().GetResult();
                var consumed = result.Buffer.Slice(1).Start;

                Assert.Equal(i + 1, result.Buffer.Length);

                _pipe.Reader.Advance(consumed, consumed);

                Assert.Equal(i, _pipe.Length);
            }
        }

    }
}
