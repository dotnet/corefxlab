// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO.Pipelines;
using System.Threading.Tasks;
using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class PipeTests
    {
        [Fact]
        public static async Task PipeRoundTripAsync()
        {
            LargeDataTestClass objOriginal = new LargeDataTestClass();
            objOriginal.Initialize();
            objOriginal.Verify();

            Pipe pipe = new Pipe(new PipeOptions(readerScheduler: PipeScheduler.Inline, writerScheduler: PipeScheduler.Inline));
            ValueTask toTask = JsonConverter.ToJsonAsync(pipe.Writer, objOriginal);
            Task<LargeDataTestClass> fromTask = JsonConverter.FromJsonAsync<LargeDataTestClass>(pipe.Reader);

            await toTask;
            pipe.Writer.Complete();

            LargeDataTestClass objCopy = await fromTask;
            pipe.Reader.Complete();

            objCopy.Verify();
        }

        [Fact]
        public static async Task PipePrimitivesAsync()
        {
            Pipe pipe = new Pipe(new PipeOptions(readerScheduler: PipeScheduler.Inline, writerScheduler: PipeScheduler.Inline));

            var t1 = pipe.Reader.FromJsonAsync<bool>();

            await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(@"t"));
            await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(@"r"));
            await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(@"u"));
            await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(@"e"));
            pipe.Writer.Complete();

            bool b = await t1;

            Assert.Equal(true, b);
        }
    }
}
