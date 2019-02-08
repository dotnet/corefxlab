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

            Pipe pipe = new Pipe();
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
            Pipe pipe = new Pipe();

            pipe.Writer.GetMemory(1);
            await pipe.Writer.WriteAsync(Encoding.UTF8.GetBytes(@"1"));
            pipe.Writer.Complete();

            int i = await pipe.Reader.FromJsonAsync<int>();
            Assert.Equal(1, i);
        }
    }
}
