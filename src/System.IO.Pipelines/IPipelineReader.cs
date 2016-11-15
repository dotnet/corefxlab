using System;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a channel from which data can be read.
    /// </summary>
    public interface IPipelineReader
    {
        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="IPipelineReader"/>.
        /// </summary>
        /// <returns>A <see cref="ReadableBufferAwaitable"/> representing the asynchronous read operation.</returns>
        ReadableBufferAwaitable ReadAsync();

        /// <summary>
        /// Moves forward the channel's read cursor to after the consumed data.
        /// </summary>
        /// <param name="consumed">Marks the extent of the data that has been succesfully proceesed.</param>
        /// <param name="examined">Marks the extent of the data that has been read and examined.</param>
        /// <remarks>
        /// The memory for the consumed data will be released and no longer available.
        /// The examined data communicates to the channel when it should signal more data is available.
        /// </remarks>
        void Advance(ReadCursor consumed, ReadCursor examined);

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the channel to complete.</param>
        void Complete(Exception exception = null);
    }
}
