// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Text;
using System.Text.Formatting;
using System.IO.Pipelines.Networking.Sockets;

namespace System.IO.Pipelines.Samples
{
    public class SocketsHttpServerSample
    {
        public static void Run()
        {
            var listener = new SocketListener();
            listener.OnConnection(async connection =>
            {
                var httpParser = new HttpRequestParser();

                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    var input = result.Buffer;
                    var consumed = input.Start;
                    var examined = input.Start;

                    try
                    {
                        if (input.IsEmpty && result.IsCompleted)
                        {
                            // No more data
                            break;
                        }

                        // Parse the input http request
                        var parseResult = httpParser.ParseRequest(input, out consumed, out examined);

                        switch (parseResult)
                        {
                            case HttpRequestParser.ParseResult.Incomplete:
                                if (result.IsCompleted)
                                {
                                    // Didn't get the whole request and the connection ended
                                    throw new EndOfStreamException();
                                }
                                // Need more data
                                continue;
                            case HttpRequestParser.ParseResult.Complete:
                                break;
                            case HttpRequestParser.ParseResult.BadRequest:
                                throw new Exception();
                            default:
                                break;
                        }

                        // Writing directly to pooled buffers
                        var output = connection.Output.Alloc();
                        var formatter = new OutputFormatter<WritableBuffer>(output, TextEncoder.Utf8);
                        formatter.Append("HTTP/1.1 200 OK");
                        formatter.Append("\r\nContent-Length: 13");
                        formatter.Append("\r\nContent-Type: text/plain");
                        formatter.Append("\r\n\r\n");
                        formatter.Append("Hello, World!");
                        await output.FlushAsync();

                        httpParser.Reset();
                    }
                    finally
                    {
                        // Consume the input
                        connection.Input.Advance(consumed, examined);
                    }
                }
            });

            var ip = IPAddress.Any;
            int port = 5000;
            listener.Start(new IPEndPoint(ip, port));
            
            Console.WriteLine($"Listening on {ip} on port {port}");
            var wh = new ManualResetEventSlim();
            Console.CancelKeyPress += (sender, e) =>
            {
                wh.Set();
            };

            wh.Wait();

            listener.Dispose();
        }
    }
}
