// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Formatting;

namespace System.IO.Pipelines.Samples
{
    public abstract class RawHttpClientSampleBase : ISample
    {
        public async Task Run()
        {
            var consoleOutput = GetPipeFactory().CreateWriter(Console.OpenStandardOutput());
            var connection = await GetConnection();

            while (true)
            {
                var buffer = connection.Output.Alloc();
                buffer.Append("GET / HTTP/1.1", TextEncoder.Utf8);
                buffer.Append("\r\n\r\n", TextEncoder.Utf8);
                await buffer.FlushAsync();

                // Write the client output to the console
                await CopyCompletedAsync(connection.Input, consoleOutput);

                await Task.Delay(1000);
            }
        }

        protected abstract PipeFactory GetPipeFactory();

        protected abstract Task<IPipeConnection> GetConnection();

        private async Task CopyCompletedAsync(IPipeReader input, IPipeWriter output)
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

                    var buffer = output.Alloc();

                    buffer.Append(inputBuffer);

                    await buffer.FlushAsync();
                }
                finally
                {
                    input.Advance(inputBuffer.End);
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
