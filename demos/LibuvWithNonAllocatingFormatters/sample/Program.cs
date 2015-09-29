using System;
using System.Net.Libuv;
using System.Runtime.InteropServices;
using System.Text.Formatting;
using System.Threading;

static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("service started");
        int numberOfLoops = 1;
        //int numberOfLoops = Environment.ProcessorCount - 1; // this does not really work, i.e. there is a threading issue with the listener implementation.
        UVLoop[] loops = CreateLoops(numberOfLoops);

        var listener = new TcpListener("127.0.0.1", 8080, loops[0], loops);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            connection.ReadCompleted += (ByteSpan request) =>
            {
                var loop = connection.Loop as ServiceLoop;
                var formatter = loop.Formatter;
                formatter.Clear();
                formatter.Append("HTTP/1.1 200 OK");
                formatter.Append("\r\n\r\n");
                formatter.Append("Hello World!");
                WriteResponse(connection, formatter.Buffer.Slice(0, formatter.CommitedByteCount));
            };

            connection.ReadStart();
        };

        listener.Listen();
        loops.Run();
    }

    static void Run(this UVLoop[] loops)
    {
        for (int index = 1; index < loops.Length; index++)
        {
            ThreadPool.QueueUserWorkItem((context) =>
            {
                try
                {
                    ((UVLoop)context).Run();
                }
                catch (Exception e)
                {
                    Environment.FailFast(e.ToString());
                }
            }, loops[index]);
        }
        loops[0].Run();
    }

    class ServiceLoop : UVLoop
    {
        public readonly BufferFormatter Formatter = new BufferFormatter(512, FormattingData.InvariantUtf8);
    }

    private static UVLoop[] CreateLoops(int numberOfLoops)
    {
        UVLoop[] loops = new UVLoop[numberOfLoops];
        for (int i = 0; i < loops.Length; i++)
        {
            var loop = new ServiceLoop();
            var idle = new Idle(loop);
            idle.Start();
            loops[i] = loop;
        }
        return loops;
    }

    // this should be removed once formatters work with ByteSpan
    private static void WriteResponse(Tcp connection, Span<byte> response)
    {
        GCHandle gcHandle;
        var byteSpan = response.Pin(out gcHandle);
        connection.TryWrite(byteSpan);
        connection.Dispose();
        gcHandle.Free(); // TODO: formatter should format to ByteSpan, to avoid pinning
    }
}
