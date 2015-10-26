using System;
using System.Diagnostics;
using System.IO;
using System.IO.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Net.Http.Buffered;
using System.Text;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading.Tasks;

static class Program {
    
    static void Main() {
        Console.WriteLine(".NET Core Service");
        Console.WriteLine("Browse to http://localhost:8080/plaintext");

        var address = new IPAddress(new byte[] {0, 0, 0, 0});

        SocketServer.Listen(address, 8080, (socket) => {
            ProcessRequest(socket);
        });
    }

    static void ProcessRequest(TcpClient socket) {
        HttpServer.Listen(socket, (request) => {
            if (request.RequestUri.Equals(new Utf8String("/plaintext")))
            {
                var formatter = new BufferFormatter(1024, FormattingData.InvariantUtf8);
                HttpWriter.WriteCommonHeaders(formatter, "HTTP/1.1 200 OK");

                formatter.Append("Hello, World!");

                socket.Write(formatter);
                socket.Dispose();
            }
        });
    }
}

static class SocketServer
{
    //TODO: this should return cancellation source
    public static void Listen(IPAddress address, int port, Action<TcpClient> handler)
    {
        var listener = new TcpListener(address, port);
        listener.Start();
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Task.Run(() => {
                try {
                    handler(client);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
        //listener.Stop();
    }
}

static class HttpServer
{
    public static void Listen(this TcpClient connection, Action<HttpRequestLine> request)
    {
        NetworkStream stream = connection.GetStream();
        var buffer = BufferPool.Shared.RentBuffer(1024);
        while (true)
        {
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead == 0)
            {
                break;
            }

            Console.WriteLine("Read {0} bytes, Payload: {1}", bytesRead, Encoding.ASCII.GetString(buffer, 0, bytesRead));
            unsafe
            {
                fixed (byte* pBuffer = buffer)
                {
                    var bufferSpan = new ByteSpan(pBuffer, bytesRead);
                    HttpRequestLine requestLine;
                    if (!HttpRequestParser.TryParseRequestLine(bufferSpan, out requestLine))
                    {
                        Console.WriteLine("request could not be parsed");
                    }

                    request(requestLine);
                }
            }

            if (bytesRead < buffer.Length)
            {
                break;
            }
        }

        BufferPool.Shared.ReturnBuffer(ref buffer);
        connection.Dispose();
    }
}

static class TemporaryHelpers
{
    // this will be removed once we implement socket APIs that use pooled memory buffers
    public static void Write(this TcpClient socket, BufferFormatter formatter)
    {
        Console.WriteLine("writing");
        NetworkStream stream = socket.GetStream();

        var buffer = formatter.Buffer;
        stream.Write(buffer, 0, formatter.CommitedByteCount);
        stream.Flush();

        //var text = Encoding.UTF8.GetString(buffer, 0, formatter.CommitedByteCount);
        //Console.WriteLine("response {0} bytes", formatter.CommitedByteCount);
        //Console.WriteLine(text);
        
        BufferPool.Shared.ReturnBuffer(ref buffer);
    }
}
