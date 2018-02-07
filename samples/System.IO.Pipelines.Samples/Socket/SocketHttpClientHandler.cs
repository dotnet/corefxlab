// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public class SocketHttpClientHandler : PipelineHttpClientHandler
    {
        MemoryPool<byte> bufferPool = MemoryPool<byte>.Shared;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            bufferPool.Dispose();
        }

        protected override Task<IDuplexPipe> ConnectAsync(IPEndPoint ipEndpoint)
        {
            Socket s = new Socket(SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ipEndpoint);
            return Task.FromResult((IDuplexPipe)new StreamDuplexPipe(new PipeOptions(bufferPool), new NetworkStream(s)));
        }
    }
}
