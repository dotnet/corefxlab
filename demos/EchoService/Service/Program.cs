using System;
using System.Net.Libuv;

class Program
{
    static void Main(string[] args)
    {
        var loop = new UVLoop();

        var listener = new TcpListener("0.0.0.0", 7, loop);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            connection.ReadCompleted += (ByteSpan data) =>
            {
                connection.TryWrite(data);
            };

            connection.ReadStart();
        };

        listener.Listen();
        loop.Run();
    }
}

