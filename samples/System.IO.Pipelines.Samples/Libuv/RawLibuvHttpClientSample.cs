// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Net;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class RawLibuvHttpClientSample : RawHttpClientSampleBase
    {
        private UvThread thread;
        private UvTcpClient client;

        public RawLibuvHttpClientSample()
        {
            thread = new UvThread();
            client = new UvTcpClient(thread, new IPEndPoint(IPAddress.Loopback, 5000));
        }

        protected override async Task<IDuplexPipe> GetConnection()
        {
            return await client.ConnectAsync();
        }

        protected override MemoryPool<byte> GetBufferPool()
        {
            return thread.Pool;
        }
    }
}
