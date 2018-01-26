// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Formatting;
using System.Buffers.Text;

namespace System.IO.Pipelines.Samples
{
    public abstract class RawHttpClientSampleBase : ISample
    {
        public async Task Run()
        {
            var pipeOptions = new PipeOptions(GetBufferPool());

            var consoleOutput = StreamDuplexPipe.CreateWriter(pipeOptions, Console.OpenStandardOutput());
            var connection = await GetConnection();

            while (true)
            {
                var buffer = connection.Output;
                var output = buffer;

                output.Append("GET / HTTP/1.1", SymbolTable.InvariantUtf8);
                output.Append("\r\n\r\n", SymbolTable.InvariantUtf8);
                await buffer.FlushAsync();

                // Write the client output to the console
                await CopyCompletedAsync(connection.Input, consoleOutput);

                await Task.Delay(1000);
            }
        }

        protected abstract MemoryPool<byte> GetBufferPool();

        protected abstract Task<IDuplexPipe> GetConnection();

        private async Task CopyCompletedAsync(PipeReader input, PipeWriter output)
        {
            var result = await input.ReadAsync();
            var inputBuffer = result.Buffer;

            while (true)
            {
                try
                {
                    if (inputBuffer.IsEmpty && result.IsCompleted)
                    {
                        return;
                    }

                    var buffer = output;
                    foreach (var memory in inputBuffer)
                    {
                        buffer.Write(memory.Span);
                    }
                    await buffer.FlushAsync();
                }
                finally
                {
                    input.AdvanceTo(inputBuffer.End);
                }

                var awaiter = input.ReadAsync();

                if (!awaiter.IsCompleted)
                {
                    // No more data
                    break;
                }

                result = await input.ReadAsync();
                inputBuffer = result.Buffer;
            }
        }
    }
}
