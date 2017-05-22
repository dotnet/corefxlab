// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Text;
using System.Text.Formatting;
using System.IO.Pipelines.Networking.Windows.RIO;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class RawRioHttpServerSample : RawHttpServerSampleBase
    {
        private RioTcpServer listener;
        private bool running = true;
        private Task runningtask;

        protected override Task Start(IPEndPoint ipEndpoint)
        {
            var bytes = ipEndpoint.Address.GetAddressBytes();
            listener = new RioTcpServer((ushort)ipEndpoint.Port, bytes[0], bytes[1], bytes[2], bytes[3]);
            runningtask = Task.Run(() =>
            {
                while (running)
                {
                    var socket = listener.Accept();
                    var task = ProcessConnection(socket);
                }
            });

            return Task.CompletedTask;
        }

        protected override async Task Stop()
        {
            running = false;
            await runningtask;
            listener.Stop();
        }
    }
}
