using System;
using System.Net.Libuv;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Formatting;

static class Program
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
        var formatter = new BufferFormatter(512, FormattingData.InvariantUtf8);

        listener.ConnectionAccepted += (Tcp connection) => {
            if (log) {
                Console.WriteLine("connection accepted");
            }

            connection.ReadCompleted += (ByteSpan data) =>
            {
                if (log){

                    Console.WriteLine("*REQUEST:\n {0}", data.Utf8BytesToString());
                }

                formatter.Clear();
                formatter.Append("HTTP/1.1 200 OK");
                formatter.Append("\r\n\r\n");
                formatter.Append("Hello World!");
                if (log)
                {
                    formatter.Format(" @ {0:O}", DateTime.UtcNow);
                }

                var response = formatter.Buffer.Slice(0, formatter.CommitedByteCount); // formatter should have a property for written bytes
                GCHandle gcHandle;
                var byteSpan = response.Pin(out gcHandle);
                connection.TryWrite(byteSpan);
                connection.Dispose();
                gcHandle.Free(); // TODO: formatter should format to ByteSpan, to avoid pinning
            };

            connection.ReadStart();
        };

        listener.Listen();
        loop.Run();
    }

    static string Utf8BytesToString(this ByteSpan utf8)
    {
        unsafe
        {
            return Encoding.UTF8.GetString(utf8.UnsafeBuffer, utf8.Length);
        }
    } 
}
