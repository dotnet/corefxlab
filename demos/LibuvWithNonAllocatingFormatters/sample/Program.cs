using System;
using System.Buffers;
using System.Net.Libuv;
using System.Text;
using System.Text.Formatting;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("browse to http://localhost:8080");

        bool log = false;
        if(args.Length > 0 && args[0]=="/log")
        {
            log = true;
        }

        var loop = UVLoop.Default;
        var listener = new TcpListener("127.0.0.1", 8080, loop);

        listener.ConnectionAccepted += (Tcp connection) => {
            if (log) {
                Console.WriteLine("connection accepted");
            }

            connection.ReadCompleted += (Span<byte> data) =>
            {
                if (log){
                    var request = Encoding.ASCII.GetString(data.CreateArray());
                    Console.WriteLine("*REQUEST:\n {0}", request);
                }

                var formatter = new BufferFormatter(512, FormattingData.InvariantUtf8);

                formatter.Append("HTTP/1.1 200 OK\r\n");
                formatter.Append("\r\n\r\n");
                formatter.Append("Hello World!");
                if (log)
                {
                    formatter.Format(" @ {0:O}", DateTime.UtcNow);
                }

                var response = formatter.Buffer.Slice(0, formatter.CommitedByteCount);
                connection.TryWrite(response);
                connection.Dispose();

                var buffer = formatter.Buffer;
                BufferPool.Shared.ReturnBuffer(ref buffer);
            };

            connection.ReadStart();
        };

        listener.Listen();
        loop.Run();
    }
}
