// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv;
using System.IO.Pipelines.Text.Primitives;
using System.Text;
using System.Text.Formatting;

namespace System.IO.Pipelines.Samples
{
    public class RawLibuvHttpClientSample
    {
        public static async Task Run()
        {
            var thread = new UvThread();
            var client = new UvTcpClient(thread, new IPEndPoint(IPAddress.Loopback, 5000));

            var consoleOutput = thread.PipeFactory.CreateWriter(Console.OpenStandardOutput());

            var connection = await client.ConnectAsync();

            while (true)
            {
                var buffer = connection.Output.Alloc();

                buffer.Append("GET / HTTP/1.1", EncodingData.InvariantUtf8);
                buffer.Append("\r\n\r\n", EncodingData.InvariantUtf8);

                await buffer.FlushAsync();

                // Write the client output to the console
                await CopyCompletedAsync(connection.Input, consoleOutput);

                await Task.Delay(1000);
            }
        }
        private static async Task CopyCompletedAsync(IPipeReader input, IPipeWriter output)
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
