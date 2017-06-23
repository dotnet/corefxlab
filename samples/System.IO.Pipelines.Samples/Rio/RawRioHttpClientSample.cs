// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class RawRioHttpClientSample : RawHttpClientSampleBase
    {
        private PipeFactory factory;

        public RawRioHttpClientSample()
        {
            factory = new PipeFactory();
        }

        protected override Task<IPipeConnection> GetConnection()
        {
            //var client = new RioTcpClientPool();
            //return client.Connect(new IPEndPoint(IPAddress.Loopback, 5000));
            throw new NotImplementedException();
        }

        protected override PipeFactory GetPipeFactory()
        {
            throw new NotImplementedException();
        }
    }
}
