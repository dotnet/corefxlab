using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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

        public PipelineReaderWriter Create() => new PipelineReaderWriter(_pool);

        public IPipelineReader CreateReader(Stream stream)
        {
            if (!stream.CanRead)
            {
                ThrowHelper.ThrowNotSupportedException();
            }

            var output = new PipelineReaderWriter(_pool);
            ExecuteCopyToAsync(output, stream);
            return output;
        }

        private async void ExecuteCopyToAsync(PipelineReaderWriter output, Stream stream)
        {
            await output.ReadingStarted;

            await stream.CopyToAsync(output);
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

            var input = new PipelineReaderWriter(_pool);

            input.CopyToAsync(stream).ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    input.CompleteReader(task.Exception);
                }
                else
                {
                    input.CompleteReader();
                }
            });

            return input;
        }

        public IPipelineWriter CreateWriter(IPipelineWriter writer, Func<IPipelineReader, IPipelineWriter, Task> consume)
        {
            var newWriter = new PipelineReaderWriter(_pool);

            consume(newWriter, writer).ContinueWith(t =>
            {
            });

            return newWriter;
        }

        public IPipelineReader CreateReader(IPipelineReader reader, Func<IPipelineReader, IPipelineWriter, Task> produce)
        {
            var newReader = new PipelineReaderWriter(_pool);
            Execute(reader, newReader, produce);
            return newReader;
        }

        private async void Execute(IPipelineReader reader, PipelineReaderWriter writer, Func<IPipelineReader, IPipelineWriter, Task> produce)
        {
            await writer.ReadingStarted;

            await produce(reader, writer);
        }

        public void Dispose() => _pool.Dispose();
    }
}
