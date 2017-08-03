// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeResetTests : IDisposable
    {
        private PipeFactory _pipeFactory;

        private Pipe _pipe;

        public PipeResetTests()
        {
            _pipeFactory = new PipeFactory();
            _pipe = (Pipe) _pipeFactory.Create();
        }

        public void Dispose()
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipeFactory?.Dispose();
        }

        [Fact]
        public async Task ReadsAndWritesAfterReset()
        {
            var source = new byte[] { 1, 2, 3 };

            await _pipe.Writer.WriteAsync(source);
            var result = await _pipe.Reader.ReadAsync();

            Assert.Equal(source, result.Buffer.ToArray());
            _pipe.Reader.Advance(result.Buffer.End);

            _pipe.Reader.Complete();
            _pipe.Writer.Complete();

            _pipe.Reset();


            await _pipe.Writer.WriteAsync(source);
            result = await _pipe.Reader.ReadAsync();

            Assert.Equal(source, result.Buffer.ToArray());
            _pipe.Reader.Advance(result.Buffer.End);
        }

        [Fact]
        public async Task LengthIsReseted()
        {
            var source = new byte[] { 1, 2, 3 };

            await _pipe.Writer.WriteAsync(source);

            _pipe.Reader.Complete();
            _pipe.Writer.Complete();

            _pipe.Reset();

            Assert.Equal(0, _pipe.Length);
        }

        [Fact]
        public void ResetThrowsIfReaderNotCompleted()
        {
            _pipe.Writer.Complete();
            Assert.Throws<InvalidOperationException>(() => _pipe.Reset());
        }

        [Fact]
        public void ResetThrowsIfWriterNotCompleted()
        {
            _pipe.Reader.Complete();
            Assert.Throws<InvalidOperationException>(() => _pipe.Reset());
        }

    }
}
