// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.IO.Pipelines.Networking.Libuv;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class RawLibuvHttpServerSample : RawHttpServerSampleBase
    {
        private UvThread thread;
        private UvTcpListener listener;
        
        protected override async Task Start(IPEndPoint ipEndpoint)
        {
            thread = new UvThread();
            listener = new UvTcpListener(thread, ipEndpoint);
            listener.OnConnection(async connection => { await ProcessConnection(connection); });
            await listener.StartAsync();
        }

        protected override Task Stop()
        {
            listener.Dispose();
            thread.Dispose();
            return Task.CompletedTask;
        }
    }
}
