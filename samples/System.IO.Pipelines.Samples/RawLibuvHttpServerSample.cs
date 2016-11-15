using System;
using System.IO;
using System.Net;
using System.Threading;
using System.IO.Pipelines.Networking.Libuv;
using System.IO.Pipelines.Text.Primitives;

namespace System.IO.Pipelines.Samples
{
    public class RawLibuvHttpServerSample
    {
        public static void Run()
        {
            var ip = IPAddress.Any;
            int port = 5000;
            var thread = new UvThread();
            var listener = new UvTcpListener(thread, new IPEndPoint(ip, port));
            listener.OnConnection(async connection =>
            {
                var httpParser = new HttpRequestParser();

                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    var input = result.Buffer;

                    try
                    {
                        if (input.IsEmpty && result.IsCompleted)
                        {
                            // No more data
                            break;
                        }

                        // Parse the input http request
                        var parseResult = httpParser.ParseRequest(ref input);

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
                        output.WriteUtf8String("HTTP/1.1 200 OK");
                        output.WriteUtf8String("\r\nContent-Length: 13");
                        output.WriteUtf8String("\r\nContent-Type: text/plain");
                        output.WriteUtf8String("\r\n\r\n");
                        output.WriteUtf8String("Hello, World!");
                        await output.FlushAsync();

                        httpParser.Reset();
                    }
                    finally
                    {
                        // Consume the input
                        connection.Input.Advance(input.Start, input.End);
                    }
                }
            });

            listener.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Listening on {ip} on port {port}");
            var wh = new ManualResetEventSlim();
            Console.CancelKeyPress += (sender, e) =>
            {
                wh.Set();
            };

            wh.Wait();

            listener.Dispose();
            thread.Dispose();
        }
    }
}
