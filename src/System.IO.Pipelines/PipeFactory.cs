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
            ExecuteCopyToAsync(pipe, stream);
            return pipe;
        }

        private async void ExecuteCopyToAsync(Pipe pipe, Stream stream)
        {
            await pipe.ReadingStarted;

            await stream.CopyToAsync(pipe);
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

            pipe.CopyToAsync(stream).ContinueWith((task, state) =>
            {
                var innerPipe = (Pipe)state;
                if (task.IsFaulted)
                {
                    innerPipe.Reader.Complete(task.Exception.InnerException);
                }
                else
                {
                    innerPipe.Reader.Complete();
                }
            },
            pipe, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return pipe;
        }

        public IPipeWriter CreateWriter(IPipeWriter writer, Func<IPipeReader, IPipeWriter, Task> consume)
        {
            var pipe = new Pipe(_pool);

            consume(pipe, writer).ContinueWith(t =>
            {
            });

            return pipe;
        }

        public IPipeReader CreateReader(IPipeReader reader, Func<IPipeReader, IPipeWriter, Task> produce)
        {
            var pipe = new Pipe(_pool);
            Execute(reader, pipe, produce);
            return pipe;
        }

        private async void Execute(IPipeReader reader, Pipe pipe, Func<IPipeReader, IPipeWriter, Task> produce)
        {
            await pipe.ReadingStarted;

            await produce(reader, pipe);
        }

        public void Dispose() => _pool.Dispose();
    }
}
