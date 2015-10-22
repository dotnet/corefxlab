using System;
using System.Net.Libuv;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        byte[] quote = Encoding.ASCII.GetBytes("Insanity: doing the same thing over and over again and expecting different results. - Albert Einstein");
        
        var loop = new UVLoop();

        var listener = new TcpListener("0.0.0.0", 17, loop);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            connection.ReadCompleted += (ByteSpan data) =>
            {
                unsafe
                {
                    fixed (byte* pQuote = quote)
                    {
                        connection.TryWrite(new ByteSpan(pQuote, quote.Length));
                    }
                }
            };

            connection.ReadStart();
        };

        listener.Listen();
        loop.Run();
    }
}

