using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    
    static void ProcessRequest(TcpClient socket) {
        NetworkStream stream = socket.GetStream();
        byte[] buffer = new byte[1024];
        while(true){
            var read = stream.Read(buffer, 0, buffer.Length);
            Console.WriteLine("\nread {0} bytes:", read);
            
            if(read > 0) {
                var requestText = Encoding.ASCII.GetString(buffer, 0, read);
                Console.WriteLine(requestText);
                                
                if(requestText.Contains("GET /plaintext")){
                    ProcessPlainTextRequest(socket);
                }
            }
            if (read < buffer.Length)
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
