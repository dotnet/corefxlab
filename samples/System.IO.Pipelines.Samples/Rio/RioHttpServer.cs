// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Windows.RIO;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;

namespace System.IO.Pipelines.Samples.Http
{
    public class RioHttpServer : IServer
    {
        public IFeatureCollection Features { get; } = new FeatureCollection();

        private RioTcpServer _rioTcpServer;

        public RioHttpServer()
        {
            Features.Set<IServerAddressesFeature>(new ServerAddressesFeature());
        }

        public void Start<TContext>(IHttpApplication<TContext> application)
        {
            var feature = Features.Get<IServerAddressesFeature>();
            var address = feature.Addresses.FirstOrDefault();
            GetIp(address, out IPAddress ip, out int port);
            Task.Factory.StartNew(() => StartAccepting(application, ip, port), TaskCreationOptions.LongRunning);
        }


        private void StartAccepting<TContext>(IHttpApplication<TContext> application, IPAddress ip, int port)
        {
            Thread.CurrentThread.Name = "RIO Accept Thread";
            var addressBytes = ip.GetAddressBytes();

            try
            {
                _rioTcpServer = new RioTcpServer((ushort)port, addressBytes[0], addressBytes[1], addressBytes[2], addressBytes[3]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            while (true)
            {
                try
                {
                    var connection = _rioTcpServer.Accept();
                    var task = ProcessRIOConnection(application, connection);
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }
            }
        }


        public void Dispose()
        {
            _rioTcpServer?.Stop();
            _rioTcpServer = null;
        }

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

        private static async Task ProcessRIOConnection<TContext>(IHttpApplication<TContext> application, RioTcpConnection connection)
        {
            using (connection)
            {
                await ProcessClient(application, connection);
            }
        }

        private static async Task ProcessConnection<TContext>(IHttpApplication<TContext> application, MemoryPool<byte> memoryPool, Socket socket)
        {
            using (var ns = new NetworkStream(socket))
            {
                using (var connection = new StreamPipeConnection(new PipeOptions(memoryPool), ns))
                {
                    await ProcessClient(application, connection);
                }
            }
        }

        private static async Task ProcessClient<TContext>(IHttpApplication<TContext> application, IPipeConnection pipeConnection)
        {
            var connection = new HttpConnection<TContext>(application, pipeConnection.Input, pipeConnection.Output);

            await connection.ProcessAllRequests();
        }
    }


}
