// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeLengthTests : IDisposable
    {
        private PipeFactory _pipeFactory;

        private Pipe _pipe;

        public PipeLengthTests()
        {
            _pipeFactory = new PipeFactory();
            _pipe = (Pipe)_pipeFactory.Create();
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipeFactory?.Dispose();
        }

        [Fact]
        public void LengthCorrectAfterAllocAdvanceCommit()
        {
            var writableBuffer = _pipe.Writer.Alloc(100);
            writableBuffer.Advance(10);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthCorrectAfterAlloc0AdvanceCommit()
        {
            var writableBuffer = _pipe.Writer.Alloc();
            writableBuffer.Ensure(10);
            writableBuffer.Advance(10);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthIncreasesAfterAppend()
        {
            var writableBuffer = _pipe.Writer.Alloc();
            writableBuffer.Append(BufferUtilities.CreateBuffer(1, 2, 3));
            Assert.Equal(0, _pipe.Length);
            writableBuffer.Commit();

            Assert.Equal(6, _pipe.Length);
        }

        [Fact]
        public void LengthIncreasesAfterAdvanceAndAppend()
        {
            var writableBuffer = _pipe.Writer.Alloc(10);
            writableBuffer.Advance(4);
            writableBuffer.Append(BufferUtilities.CreateBuffer(1, 2, 3));
            Assert.Equal(0, _pipe.Length);
            writableBuffer.Commit();

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void LengthDecreasedAfterReadAdvanceConsume()
        {
            var writableBuffer = _pipe.Writer.Alloc(100);
            writableBuffer.Advance(10);
            writableBuffer.Commit();
            writableBuffer.FlushAsync();

            var result = _pipe.Reader.ReadAsync().GetResult();
            var consumed = result.Buffer.Slice(5).Start;
            _pipe.Reader.Advance(consumed, consumed);

            Assert.Equal(5, _pipe.Length);
        }

        [Fact]
        public void LengthNotChangeAfterReadAdvanceExamine()
        {
            var writableBuffer = _pipe.Writer.Alloc(100);
            writableBuffer.Advance(10);
            writableBuffer.Commit();
            writableBuffer.FlushAsync();

            var result = _pipe.Reader.ReadAsync().GetResult();
            _pipe.Reader.Advance(result.Buffer.Start, result.Buffer.End);

            Assert.Equal(10, _pipe.Length);
        }

        [Fact]
        public void ByteByByteTest()
        {
            WritableBuffer writableBuffer;
            for (int i = 1; i <= 1024 * 1024; i++)
            {
                writableBuffer = _pipe.Writer.Alloc(100);
                writableBuffer.Advance(1);
                writableBuffer.Commit();

                Assert.Equal(i, _pipe.Length);
            }
            writableBuffer.FlushAsync();

            for (int i = 1024 * 1024 - 1; i >= 0; i--)
            {
                var result = _pipe.Reader.ReadAsync().GetResult();
                var consumed = result.Buffer.Slice(1).Start;
                _pipe.Reader.Advance(consumed, consumed);

                Assert.Equal(i, _pipe.Length);
            }
        }

    }
}
