using System;
using System.Net.Libuv;
using System.Runtime.InteropServices;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading.Tasks;

static class Program
{
    static string s_ipAddress;
    static int s_port;

    public static void Main(string[] args)
    {
        if(args.Length < 1 || args[0].Substring(0, 4) != "/ip:")
        {
            Usage();
            return;
        }

        var options = args[0].Substring(4).Split(':');
        if(options.Length!=2)
        {
            Usage();
            return;
        }
        s_ipAddress = options[0];
        s_port = Int32.Parse(options[1]);

        Console.WriteLine("browse to http://{0}:{1}", s_ipAddress, s_port);

        bool log = false;
        if (args.Length > 0 && args[0] == "/log")
        {
            log = true;
        }

        var numberOfLoops = Environment.ProcessorCount;
        var loops = new Task[numberOfLoops];
        for (int i = 0; i < numberOfLoops; i++)
        {
            var loop = Task.Run(() => RunLoop(log));
            loops[i] = loop;
        }

        Task.WaitAll(loops);
    }

    private static void Usage()
    {
        Console.WriteLine("Usage: {0} /ip:<ip_address>:<port>", Environment.GetCommandLineArgs()[0]);
    }

    static void RunLoop(bool log)
    {
        var loop = new UVLoop();

        var listener = new TcpListener(s_ipAddress, s_port, loop);
        var formatter = new BufferFormatter(512, FormattingData.InvariantUtf8);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            if (log)
            {
                Console.WriteLine("connection accepted");
            }

            connection.ReadCompleted += (ByteSpan data) =>
            {
                if (log)
                {
                    unsafe
                    {
                        var requestString = new Utf8String(data.UnsafeBuffer, data.Length);
                        Console.WriteLine("*REQUEST:\n {0}", requestString.ToString());
                    }
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
}
