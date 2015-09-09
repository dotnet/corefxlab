using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.Http.Buffered;
using System.Text;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading.Tasks;

static class Program {
    
    static void Main() {
        Console.WriteLine(".NET Core Micro-Server");

        var address = new IPAddress(new byte[] {127, 0, 0, 1});

        SocketServer.Listen(address, 8080, (socket) => {
            ProcessRequest(socket);
        });
    }

    static void ProcessRequest(TcpClient socket) {
        HttpServer.OnRequest(socket, (request) => {
            if (request.RequestUri.Equals("/plaintext"))
            {
                ProcessPlainTextRequest(socket);
            }
        });
    }
        
    static void ProcessPlainTextRequest(TcpClient client){
        NetworkStream stream = client.GetStream();
        var writer = new StreamWriter(stream, Encoding.ASCII);
        writer.WriteLine("HTTP/1.1 200 OK");
        writer.WriteLine("Server: .NET Core");
        writer.WriteLine("Content-Type: text/html; charset=UTF-8");
        writer.WriteLine("Content-Length: 13");
        writer.WriteLine("Connection: close");
        writer.WriteLine();
        writer.WriteLine("Hello, World!");
        writer.Dispose();
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
            Task.Run(() => { handler(client); });
        }
        //listener.Stop();
    }
}

static class HttpServer
{
    public static void OnRequest(this TcpClient connection, Action<HttpRequestLine> request)
    {
        NetworkStream stream = connection.GetStream();
        byte[] buffer = new byte[1024]; // TODO: this should be borrowed from a pool
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
        connection.Dispose();
    }
}
