using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeLengthTests : IDisposable
    {
        private PipelineFactory _pipelineFactory;

        private Pipe _pipe;

        public PipeLengthTests()
        {
            _pipelineFactory = new PipelineFactory();
            _pipe = _pipelineFactory.Create();
        }

        public void Dispose()
        {
            _pipe.CompleteWriter();
            _pipe.CompleteReader();
            _pipelineFactory?.Dispose();
        }

        [Fact]
        public void LengthCorrectAfterAllocAdvanceCommit()
        {
            var writableBuffer = _pipe.Alloc(100);
            writableBuffer.Advance(10);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthCorrectAfterAlloc0AdvanceCommit()
        {
            var writableBuffer = _pipe.Alloc();
            writableBuffer.Ensure(10);
            writableBuffer.Advance(10);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthIncreasesAfterAppend()
        {
            var writableBuffer = _pipe.Alloc();
            writableBuffer.Append(BufferUtilities.CreateBuffer(1, 2, 3));
            Assert.Equal(0, _pipe.Length);
            writableBuffer.Commit();

            Assert.Equal(6, _pipe.Length);
        }

        [Fact]
        public void LengthIncreasesAfterAdvanceAndAppend()
        {
            var writableBuffer = _pipe.Alloc(10);
            writableBuffer.Advance(4);
            writableBuffer.Append(BufferUtilities.CreateBuffer(1, 2, 3));
            Assert.Equal(0, _pipe.Length);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthDecreasedAfterReadAdvanceConsume()
        {
            var writableBuffer = _pipe.Alloc(100);
            writableBuffer.Advance(10);
            writableBuffer.Commit();
            writableBuffer.FlushAsync();

            var result = _pipe.ReadAsync().GetResult();
            var consumed = result.Buffer.Slice(5).Start;
            _pipe.AdvanceReader(consumed, consumed);

            Assert.Equal(5, _pipe.Length);
        }

        [Fact]
        public void LengthNotChangeAfterReadAdvanceExamine()
        {
            var writableBuffer = _pipe.Alloc(100);
            writableBuffer.Advance(10);
            writableBuffer.Commit();
            writableBuffer.FlushAsync();

            var result = _pipe.ReadAsync().GetResult();
            var consumed = result.Buffer.Slice(5).Start;
            _pipe.AdvanceReader(result.Buffer.Start, result.Buffer.End);

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void ByteByByteTest()
        {
            WritableBuffer writableBuffer;
            for (int i = 1; i <= 1024 * 1024; i++)
            {
                writableBuffer = _pipe.Alloc(100);
                writableBuffer.Advance(1);
                writableBuffer.Commit();

                Assert.Equal(i, _pipe.Length);
            }
            writableBuffer.FlushAsync();

            for (int i = 1024 * 1024 - 1; i >= 0; i--)
            {
                var result = _pipe.ReadAsync().GetResult();
                var consumed = result.Buffer.Slice(1).Start;
                _pipe.AdvanceReader(consumed, consumed);

                Assert.Equal(i, _pipe.Length);
            }
        }

    }
}
