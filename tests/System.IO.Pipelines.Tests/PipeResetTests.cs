using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeResetTests
    {
        private PipeFactory _pipeFactory;

        private IPipe _pipe;

        public PipeResetTests()
        {
            _pipeFactory = new PipeFactory();
            _pipe = _pipeFactory.Create(new PipeOptions
            {
                MaximumSizeLow = 32,
                MaximumSizeHigh = 64
            });
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
