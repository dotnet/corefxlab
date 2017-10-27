// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Libuv;
using System.Text.Utf8;

class QotdService
{
    static Utf8String quote = (Utf8String)"Insanity: doing the same thing over and over again and expecting different results. - Albert Einstein";

    static void Main()
    {
        var loop = new UVLoop();

        var listener = new TcpListener("0.0.0.0", 17, loop);

        listener.ConnectionAccepted += (Tcp connection) =>
        {
            connection.ReadCompleted += (data) =>
            {
                connection.TryWrite(quote.Bytes);
            };

            connection.ReadStart();
        };

        listener.Listen();
        loop.Run();
    }
}

