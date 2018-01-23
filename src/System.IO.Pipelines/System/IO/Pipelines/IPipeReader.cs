// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Sequences;
using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a pipeline from which data can be read.
    /// </summary>
    public abstract class IPipeReader
    {
        /// <summary>
        /// Attempt to synchronously read data the <see cref="IPipeReader"/>.
        /// </summary>
        /// <param name="result">The <see cref="ReadResult"/></param>
        /// <returns>True if data was available, or if the call was cancelled or the writer completed with an error.</returns>
        /// <remarks>If the pipe returns false, there's no need to call Advance.</remarks>
        public abstract bool TryRead(out ReadResult result);

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="IPipeReader"/>.
        /// </summary>
        /// <returns>A <see cref="ReadableBufferAwaitable"/> representing the asynchronous read operation.</returns>
        public abstract ValueAwaiter<ReadResult> ReadAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Moves forward the pipeline's read cursor to after the consumed data.
        /// </summary>
        /// <param name="consumed">Marks the extent of the data that has been succesfully proceesed.</param>
        /// <remarks>
        /// The memory for the consumed data will be released and no longer available.
        /// The examined data communicates to the pipeline when it should signal more data is available.
        /// </remarks>
        public abstract void Advance(Position consumed);

        /// <summary>
        /// Moves forward the pipeline's read cursor to after the consumed data.
        /// </summary>
        /// <param name="consumed">Marks the extent of the data that has been succesfully proceesed.</param>
        /// <param name="examined">Marks the extent of the data that has been read and examined.</param>
        /// <remarks>
        /// The memory for the consumed data will be released and no longer available.
        /// The examined data communicates to the pipeline when it should signal more data is available.
        /// </remarks>
        public abstract void Advance(Position consumed, Position examined);

        /// <summary>
        /// Cancel to currently pending or next call to <see cref="ReadAsync"/> if none is pending, without completing the <see cref="IPipeReader"/>.
        /// </summary>
        public abstract void CancelPendingRead();

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        public abstract void Complete(Exception exception = null);

        /// <summary>
        /// Registers callback that gets executed when writer side of pipe completes.
        /// </summary>
        public abstract void OnWriterCompleted(Action<Exception, object> callback, object state);
    }
}
