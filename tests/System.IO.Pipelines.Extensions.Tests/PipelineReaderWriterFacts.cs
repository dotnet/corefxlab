// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipelineReaderWriterFacts : IDisposable
    {
        private Pipe _pipe;
        private MemoryPool<byte> _pool;

        public PipelineReaderWriterFacts()
        {
            _pool = new MemoryPool();
            _pipe = new Pipe(new PipeOptions(_pool));
        }
        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pool?.Dispose();
        }

        [Fact]
        public async Task IndexOfNotFoundReturnsEnd()
        {
            var bytes = Encoding.ASCII.GetBytes("Hello World");

            await _pipe.Writer.WriteAsync(bytes);
            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;

            Assert.False(buffer.TrySliceTo(10, out ReadOnlyBuffer<byte> slice, out SequenceIndex cursor));

            _pipe.Reader.AdvanceTo(buffer.Start, buffer.Start);
        }

        [Fact]
        public async Task FastPathIndexOfAcrossBlocks()
        {
            const int blockSize = 4032;
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var writeBuffer = _pipe.Writer;
            writeBuffer.Write(paddingBytes);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            Assert.False(buffer.TrySliceTo((byte)'R', out ReadOnlyBuffer<byte> slice, out SequenceIndex cursor));

            _pipe.Reader.AdvanceTo(buffer.Start, buffer.Start);
        }

        [Fact]
        public async Task SlowPathIndexOfAcrossBlocks()
        {
            const int blockSize = 4032;
            //     block 1       ->    block2
            // [padding..hello]  ->  [  world   ]
            var paddingBytes = Enumerable.Repeat((byte)'a', blockSize - 5).ToArray();
            var bytes = Encoding.ASCII.GetBytes("Hello World");
            var writeBuffer = _pipe.Writer;
            writeBuffer.Write(paddingBytes);
            writeBuffer.Write(bytes);
            await writeBuffer.FlushAsync();

            var result = await _pipe.Reader.ReadAsync();
            var buffer = result.Buffer;
            Assert.False(buffer.IsSingleSegment);
            Assert.True(buffer.TrySliceTo((byte)' ', out ReadOnlyBuffer<byte> slice, out SequenceIndex cursor));

            slice = buffer.Slice(cursor).Slice(1);
            var array = slice.ToArray();

            Assert.Equal("World", Encoding.ASCII.GetString(array));

            _pipe.Reader.AdvanceTo(buffer.Start, buffer.Start);
        }
    }
}
