using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net.Libuv;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace QotdService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var buffer = new byte[1024];
            var quote = new Utf8String("Insanity: doing the same thing over and over again and expecting different results. - Albert Einstein"); ;

            var loop = new UVLoop();

            var listener = new TcpListener("0.0.0.0", 17, loop);

            listener.ConnectionAccepted += (Tcp connection) =>
            {
                connection.ReadCompleted += (data) =>
                {
                    quote.CopyTo(buffer);
                    connection.TryWrite(buffer, quote.Length);
                };

                connection.ReadStart();
            };

            listener.Listen();
            loop.Run();
        }
    }
}
