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
    public class PipelineFactory : IDisposable
    {
        private readonly IBufferPool _pool;

        public PipelineFactory() : this(new MemoryPool())
        {
        }

        public PipelineFactory(IBufferPool pool)
        {
            _pool = pool;
        }

        public Pipe Create() =>
            Create(0);

        public Pipe Create(long maximumSize) =>
            Create(maximumSize, maximumSize);

        public Pipe Create(long maximumSizeLow, long maximumSizeHigh) =>
            new Pipe(_pool, maximumSizeLow: maximumSizeLow, maximumSizeHigh: maximumSizeHigh);

        public IPipelineReader CreateReader(Stream stream)
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

        public IPipelineConnection CreateConnection(NetworkStream stream)
        {
            return new StreamPipelineConnection(this, stream);
        }

        public IPipelineWriter CreateWriter(Stream stream)
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
                    innerPipe.CompleteReader(task.Exception.InnerException);
                }
                else
                {
                    innerPipe.CompleteReader();
                }
            },
            pipe, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);

            return pipe;
        }

        public IPipelineWriter CreateWriter(IPipelineWriter writer, Func<IPipelineReader, IPipelineWriter, Task> consume)
        {
            var pipe = new Pipe(_pool);

            consume(pipe, writer).ContinueWith(t =>
            {
            });

            return pipe;
        }

        public IPipelineReader CreateReader(IPipelineReader reader, Func<IPipelineReader, IPipelineWriter, Task> produce)
        {
            var pipe = new Pipe(_pool);
            Execute(reader, pipe, produce);
            return pipe;
        }

        private async void Execute(IPipelineReader reader, Pipe pipe, Func<IPipelineReader, IPipelineWriter, Task> produce)
        {
            await pipe.ReadingStarted;

            await produce(reader, pipe);
        }

        public void Dispose() => _pool.Dispose();
    }
}
