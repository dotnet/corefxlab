// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Sockets;

namespace System.IO.Pipelines.Samples
{
    public class RawSocketHttpServerSample : RawHttpServerSampleBase
    {
        public SocketListener listener { get; private set; }

        protected override Task Start(IPEndPoint ipEndpoint)
        {
            listener = new SocketListener();
            listener.OnConnection(async connection => { await ProcessConnection(connection); });

            var ip = IPAddress.Any;
            int port = 5000;
            listener.Start(new IPEndPoint(ip, port));
            return Task.CompletedTask;
        }

        protected override Task Stop()
        {
            listener.Dispose();
            return Task.CompletedTask;
        }
    }
}
