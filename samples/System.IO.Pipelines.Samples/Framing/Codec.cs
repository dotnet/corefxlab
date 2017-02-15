// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Formatting;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv;
using System.IO.Pipelines.Text.Primitives;

namespace System.IO.Pipelines.Samples.Framing
{
    public static class ProtocolHandling
    {
        public static void Run()
        {
            var ip = IPAddress.Any;
            int port = 5000;
            var thread = new UvThread();
            var listener = new UvTcpListener(thread, new IPEndPoint(ip, port));
            listener.OnConnection(async connection =>
            {
                var pipelineConnection = MakePipeline(connection);

                var decoder = new LineDecoder();
                var handler = new LineHandler();

                // Initialize the handler with the connection
                handler.Initialize(pipelineConnection);

                try
                {
                    while (true)
                    {
                        // Wait for data
                        var result = await pipelineConnection.Input.ReadAsync();
                        var input = result.Buffer;

                        try
                        {
                            if (input.IsEmpty && result.IsCompleted)
                            {
                                // No more data
                                break;
                            }

                            Line line;
                            while (decoder.TryDecode(ref input, out line))
                            {
                                await handler.HandleAsync(line);
                            }

                            if (!input.IsEmpty && result.IsCompleted)
                            {
                                // Didn't get the whole frame and the connection ended
                                throw new EndOfStreamException();
                            }
                        }
                        finally
                        {
                            // Consume the input
                            pipelineConnection.Input.Advance(input.Start, input.End);
                        }
                    }
                }
                finally
                {
                    // Close the input, which will tell the producer to stop producing
                    pipelineConnection.Input.Complete();

                    // Close the output, which will close the connection
                    pipelineConnection.Output.Complete();
                }
            });

            listener.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Listening on {ip} on port {port}");
            Console.ReadKey();

            listener.Dispose();
            thread.Dispose();
        }

        public static IPipeConnection MakePipeline(IPipeConnection connection)
        {
            // Do something fancy here to wrap the connection, SSL etc
            return connection;
        }
    }

    public class Line
    {
        public string Data { get; set; }
    }

    public class LineHandler : IFrameHandler<Line>
    {
        private PipelineTextOutput _textOutput;

        public void Initialize(IPipeConnection connection)
        {
            _textOutput = new PipelineTextOutput(connection.Output, TextEncoder.Utf8);
        }

        public Task HandleAsync(Line message)
        {
            // Echo back to the caller
            _textOutput.Append(message.Data);
            return _textOutput.FlushAsync();
        }
    }

    public class LineDecoder : IFrameDecoder<Line>
    {
        public bool TryDecode(ref ReadableBuffer input, out Line frame)
        {
            ReadableBuffer slice;
            ReadCursor cursor;
            if (input.TrySliceTo((byte)'\r', (byte)'\n', out slice, out cursor))
            {
                frame = new Line { Data = slice.GetUtf8String() };
                input = input.Slice(cursor).Slice(1);
                return true;
            }

            frame = null;
            return false;
        }
    }

    public interface IFrameDecoder<TInput>
    {
        bool TryDecode(ref ReadableBuffer input, out TInput frame);
    }

    public interface IFrameHandler<TInput>
    {
        void Initialize(IPipeConnection connection);

        Task HandleAsync(TInput message);
    }
}
