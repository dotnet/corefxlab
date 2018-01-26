// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class RioHttpClientHandler : PipelineHttpClientHandler
    {
        //private readonly RioTcpClientPool _pool = new RioTcpClientPool();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
          //  _pool.Stop();
        }

        protected override Task<IDuplexPipe> ConnectAsync(IPEndPoint ipEndpoint)
        {
            //return Task.FromResult<IPipeConnection>(_pool.Connect(ipEndpoint));
            throw new NotImplementedException();
        }
    }
}
