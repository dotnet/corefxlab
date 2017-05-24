// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace System.IO.Pipelines.Samples.Http
{
    public abstract class HttpServerBase : IServer
    {
        public IFeatureCollection Features { get; } = new FeatureCollection();

        public HttpServerBase()
        {
            Features.Set<IServerAddressesFeature>(new ServerAddressesFeature());
        }

        public void Start<TContext>(IHttpApplication<TContext> application)
        {
            var feature = Features.Get<IServerAddressesFeature>();
            var address = feature.Addresses.FirstOrDefault();
            IPAddress ip;
            int port;
            GetIp(address, out ip, out port);
            Task.Run(() => StartAccepting(application, ip, port));
        }

        protected abstract void StartAccepting<TContext>(IHttpApplication<TContext> application, IPAddress ip, int port);

        private static void GetIp(string url, out IPAddress ip, out int port)
        {
            ip = null;

            var address = ServerAddress.FromUrl(url);
            switch (address.Host)
            {
                case "localhost":
                    ip = IPAddress.Loopback;
                    break;
                case "*":
                    ip = IPAddress.Any;
                    break;
                default:
                    break;
            }
            ip = ip ?? IPAddress.Parse(address.Host);
            port = address.Port;
        }
        
        private static async Task ProcessClient<TContext>(IHttpApplication<TContext> application, IPipeConnection pipeConnection)
        {
            var connection = new HttpConnection<TContext>(application, pipeConnection.Input, pipeConnection.Output);
            await connection.ProcessAllRequests();
        }

        public virtual void Dispose()
        {
        }
    }
}
