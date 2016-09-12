using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Libuv;
using System.Threading.Tasks;

namespace EchoService
{
    public class Program
    {
        static void Main(string[] args)
        {
            var loop = new UVLoop();

            var listener = new TcpListener("0.0.0.0", 7, loop);

            listener.ConnectionAccepted += (Tcp connection) =>
            {
                connection.ReadCompleted += (Span<byte> data) =>
                {
                    connection.TryWrite(data);
                };

                connection.ReadStart();
            };

            listener.Listen();
            loop.Run();
        }
    }
}
