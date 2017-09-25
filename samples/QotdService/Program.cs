// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Libuv;
using System.Text.Utf8;

namespace QotdService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var buffer = new byte[1024];
            var quoteBytes = new Utf8String("Insanity: doing the same thing over and over again and expecting different results. - Albert Einstein").CopyBytes();

            var loop = new UVLoop();

            var listener = new TcpListener("0.0.0.0", 17, loop);

            listener.ConnectionAccepted += (Tcp connection) =>
            {
                connection.ReadCompleted += (data) =>
                {
                    var quote = new Utf8String(quoteBytes);
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
