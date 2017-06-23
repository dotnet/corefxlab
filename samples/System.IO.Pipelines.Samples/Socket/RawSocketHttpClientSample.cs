// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace System.IO.Pipelines.Samples
{
    public class RawSocketHttpClientSample : RawHttpClientSampleBase
    {
        private PipeFactory factory;

        public RawSocketHttpClientSample()
        {
            factory = new PipeFactory();
        }

        protected override Task<IPipeConnection> GetConnection()
        {
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Connect(new IPEndPoint(IPAddress.Loopback, 5000));
            return Task.FromResult(factory.CreateConnection(new NetworkStream(s)));
        }

        protected override PipeFactory GetPipeFactory()
        {
            return factory;
        }
    }
}
