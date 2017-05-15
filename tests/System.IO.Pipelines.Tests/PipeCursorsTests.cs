using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace System.IO.Pipelines.Tests
{
    public class PipeCursorsTests: PipeTest
    {
        public PipeCursorsTests() : base(0, 0)
        {
        }

        [Fact]
        public async Task EmpyBufferStartCrossingSegmentBoundaryIsTreatedLikeAndEnd()
        {
            // Append one full segment to a pipe
            var buffer = Pipe.Writer.Alloc(1);
            buffer.Advance(buffer.Buffer.Length);
            buffer.Commit();
            await buffer.FlushAsync();

            // Consume entire segment
            var result = await Pipe.Reader.ReadAsync();
            Pipe.Reader.Advance(result.Buffer.End);

            // Append empty segment
            buffer = Pipe.Writer.Alloc(1);
            buffer.Commit();
            await buffer.FlushAsync();

            result = await Pipe.Reader.ReadAsync();

            Assert.True(result.Buffer.IsEmpty);
            Assert.Equal(result.Buffer.Start, result.Buffer.End);

            Pipe.Reader.Advance(result.Buffer.Start);
            var awaitable = Pipe.Reader.ReadAsync();
            Assert.False(awaitable.IsCompleted);
        }
    }
}
