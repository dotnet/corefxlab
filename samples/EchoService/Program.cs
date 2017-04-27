// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net.Libuv;

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
                connection.ReadCompleted += (data) =>
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
