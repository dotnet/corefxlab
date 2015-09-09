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
    static bool receiveRequests = true;
    
    static void Main() {
        Console.WriteLine(".NET Core Micro-Server");
        var address = new IPAddress(new byte[] {127, 0, 0, 1});
        var listener = new TcpListener(address, 8080);
        listener.Start();
        while (receiveRequests)
        {
            TcpClient client = listener.AcceptTcpClient();
            Task.Run(() => { ProcessRequest(client); });
        }
        listener.Stop();
    }

    static Utf8String plaintextUri = new Utf8String("/plaintext");

    static void ProcessRequest(TcpClient socket) {
        NetworkStream stream = socket.GetStream();
        byte[] buffer = new byte[1024]; // TODO: this should be borrowed from a pool
        while(true){
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            if(bytesRead == 0)
            {
                break;
            }

            Console.WriteLine("Read {0} bytes, Payload: {1}", bytesRead, Encoding.ASCII.GetString(buffer, 0, bytesRead));
            unsafe
            {
                fixed (byte* pBuffer = buffer)
                {
                    var bufferSpan = new ByteSpan(pBuffer, bytesRead);
                    HttpRequestLine request;
                    if (!HttpRequestParser.TryParseRequestLine(bufferSpan, out request))
                    {
                        Console.WriteLine("request could not be parsed");
                    }

                    if (request.RequestUri.Equals(plaintextUri))
                    {
                        ProcessPlainTextRequest(socket);
                    }
                }
            }

            if (bytesRead < buffer.Length)
            {
                break;
            }
        }
        socket.Dispose();
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
