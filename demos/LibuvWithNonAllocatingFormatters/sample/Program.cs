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
        var listenerLoop = new UVLoop();
        UVLoop[] connectionLoops = CreateConnectionLoops(Environment.ProcessorCount - 1);

        var listener = new TcpListener("127.0.0.1", 8080, listenerLoop, connectionLoops);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            connection.ReadCompleted += (ByteSpan request) =>
            {
                var loop = connection.Loop as WorkerLoop;
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
        listenerLoop.Run();
    }

    class WorkerLoop : UVLoop
    {
        public readonly BufferFormatter Formatter = new BufferFormatter(512, FormattingData.InvariantUtf8);
    }

    private static UVLoop[] CreateConnectionLoops(int numberOfLoops)
    {
        UVLoop[] loops = new UVLoop[numberOfLoops];
        for (int i = 0; i < loops.Length; i++)
        {
            loops[i] = new WorkerLoop();
            ThreadPool.QueueUserWorkItem((context) =>
            {
                try
                {
                    var l = (UVLoop)context;
                    var idle = new Idle(l);
                    idle.Start();
                    l.Run();
                }
                catch (Exception e)
                {
                    Environment.FailFast(e.ToString());
                }
            }, loops[i]);
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
