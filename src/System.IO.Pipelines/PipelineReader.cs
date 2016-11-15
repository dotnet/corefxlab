using System;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Represents a pipeline from which data can be read.
    /// </summary>
    public abstract class PipelineReader : IPipelineReader
    {
        /// <summary>
        /// The underlying <see cref="PipelineReaderWriter"/> the <see cref="PipelineReader"/> communicates over.
        /// </summary>
        protected readonly PipelineReaderWriter _input;

        /// <summary>
        /// Creates a base <see cref="PipelineReader"/>.
        /// </summary>
        /// <param name="pool">The <see cref="IBufferPool"/> that buffers will be allocated from.</param>
        protected PipelineReader(IBufferPool pool)
        {
            _input = new PipelineReaderWriter(pool);
        }

        /// <summary>
        /// Creates a base <see cref="PipelineReader"/>.
        /// </summary>
        /// <param name="input">The <see cref="PipelineReaderWriter"/> the <see cref="PipelineReader"/> communicates over.</param>
        protected PipelineReader(PipelineReaderWriter input)
        {
            _input = input;
        }

        /// <summary>
        /// Moves forward the pipelines read cursor to after the consumed data.
        /// </summary>
        /// <param name="consumed">Marks the extent of the data that has been succesfully proceesed.</param>
        /// <param name="examined">Marks the extent of the data that has been read and examined.</param>
        /// <remarks>
        /// The memory for the consumed data will be released and no longer available.
        /// The examined data communicates to the pipeline when it should signal more data is available.
        /// </remarks>
        public void Advance(ReadCursor consumed, ReadCursor examined) => _input.AdvanceReader(consumed, examined);

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        public void Complete(Exception exception = null) => _input.CompleteReader(exception);

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="PipelineReader"/>.
        /// </summary>
        /// <returns>A <see cref="ReadableBufferAwaitable"/> representing the asynchronous read operation.</returns>
        public ReadableBufferAwaitable ReadAsync() => _input.ReadAsync();
    }
}
