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
                var channel = MakePipeline(connection);

                var decoder = new LineDecoder();
                var handler = new LineHandler();

                // Initialize the handler with the channel
                handler.Initialize(channel);

                try
                {
                    while (true)
                    {
                        // Wait for data
                        var result = await channel.Input.ReadAsync();
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
                            channel.Input.Advance(input.Start, input.End);
                        }
                    }
                }
                finally
                {
                    // Close the input channel, which will tell the producer to stop producing
                    channel.Input.Complete();

                    // Close the output channel, which will close the connection
                    channel.Output.Complete();
                }
            });

            listener.StartAsync().GetAwaiter().GetResult();

            Console.WriteLine($"Listening on {ip} on port {port}");
            Console.ReadKey();

            listener.Dispose();
            thread.Dispose();
        }

        public static IPipelineConnection MakePipeline(IPipelineConnection channel)
        {
            // Do something fancy here to wrap the channel, SSL etc
            return channel;
        }
    }

    public class Line
    {
        public string Data { get; set; }
    }

    public class LineHandler : IFrameHandler<Line>
    {
        private WritableChannelFormatter _formatter;

        public void Initialize(IPipelineConnection channel)
        {
            _formatter = new WritableChannelFormatter(channel.Output, EncodingData.InvariantUtf8);
        }

        public Task HandleAsync(Line message)
        {
            // Echo back to the caller
            _formatter.Append(message.Data);
            return _formatter.FlushAsync();
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
        void Initialize(IPipelineConnection channel);

        Task HandleAsync(TInput message);
    }
}
