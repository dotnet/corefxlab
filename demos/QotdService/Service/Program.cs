using System;
using System.Buffers;
using System.Net.Libuv;
using System.Text;
using System.Text.Utf8;

class Program
{
    static void Main(string[] args)
    {
        byte[] buffer = new byte[1024];
        Utf8String quote = new Utf8String("Insanity: doing the same thing over and over again and expecting different results. - Albert Einstein");;

        var loop = new UVLoop();

        var listener = new TcpListener("0.0.0.0", 17, loop);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            connection.ReadCompleted += (Span<byte> data) =>
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

