// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Net;
using System.Threading.Tasks;
using System.IO.Pipelines.Networking.Libuv;

namespace System.IO.Pipelines.Samples
{
    public class LibuvHttpClientHandler : PipelineHttpClientHandler
    {
        private readonly UvThread _thread = new UvThread();

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _thread.Dispose();
        }

        protected override async Task<IDuplexPipe> ConnectAsync(IPEndPoint ipEndpoint)
        {
            return await new UvTcpClient(_thread, ipEndpoint).ConnectAsync();
        }
    }
}
