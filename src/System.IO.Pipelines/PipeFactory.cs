// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Factory used to creaet instances of various pipelines.
    /// </summary>
    public class PipeFactory : IDisposable
    {
        private readonly IBufferPool _pool;

        public PipeFactory() : this(new MemoryPool())
        {
        }

        public PipeFactory(IBufferPool pool)
        {
            _pool = pool;
        }

        public IPipe Create()
        {
            return new Pipe(_pool);
        }

        public IPipe Create(PipeOptions options)
        {
            return new Pipe(_pool, options);
        }

        public IPipeReader CreateReader(Stream stream)
        {
            if (!stream.CanRead)
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            var pipe = new Pipe(_pool);
            var ignore = stream.CopyToEndAsync(pipe);
            return pipe;
        }

        public IPipeConnection CreateConnection(NetworkStream stream)
        {
            return new StreamPipeConnection(this, stream);
        }

        public IPipeWriter CreateWriter(Stream stream)
        {
            if (!stream.CanWrite)
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            var pipe = new Pipe(_pool);
            var ignore = pipe.CopyToEndAsync(stream);

            return pipe;
        }

        public void Dispose() => _pool.Dispose();
    }
}
