// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace System.IO.Pipelines.Samples.Http
{
    public class LibuvHttpServer : HttpServerBase
    {
        private UvTcpListener _uvTcpListener;
        private UvThread _uvThread;

        public LibuvHttpServer()
        {
            Features.Set<IServerAddressesFeature>(new ServerAddressesFeature());
        }

        protected override void StartAccepting<TContext>(IHttpApplication<TContext> application, IPAddress ip, int port)
        {
            _uvThread = new UvThread();
            _uvTcpListener = new UvTcpListener(_uvThread, new IPEndPoint(ip, port));
            _uvTcpListener.OnConnection(async connection =>
            {
                await ProcessClient(application, connection);
            });

            _uvTcpListener.StartAsync();
        }

        public override void Dispose()
        {
            _uvTcpListener?.Dispose();
            _uvThread?.Dispose();

            _uvTcpListener = null;
            _uvThread = null;
        }

        private static async Task ProcessClient<TContext>(IHttpApplication<TContext> application, IDuplexPipe duplexPipe)
        {
            var connection = new HttpConnection<TContext>(application, duplexPipe.Input, duplexPipe.Output);
            await connection.ProcessAllRequests();
        }
    }
}
