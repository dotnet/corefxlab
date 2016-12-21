﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        /// The underlying <see cref="Pipe"/> the <see cref="PipelineReader"/> communicates over.
        /// </summary>
        protected readonly Pipe _pipe;

        /// <summary>
        /// Creates a base <see cref="PipelineReader"/>.
        /// </summary>
        /// <param name="pool">The <see cref="IBufferPool"/> that buffers will be allocated from.</param>
        protected PipelineReader(IBufferPool pool)
        {
            _pipe = new Pipe(pool);
        }

        /// <summary>
        /// Creates a base <see cref="PipelineReader"/>.
        /// </summary>
        /// <param name="pipe">The <see cref="Pipe"/> the <see cref="PipelineReader"/> communicates over.</param>
        protected PipelineReader(Pipe pipe)
        {
            _pipe = pipe;
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
        public void Advance(ReadCursor consumed, ReadCursor examined) => _pipe.AdvanceReader(consumed, examined);

        /// <summary>
        /// Cancel to currently pending call to <see cref="ReadAsync"/>
        /// </summary>
        public void CancelPendingRead() => _pipe.CancelPendingRead();

        /// <summary>
        /// Signal to the producer that the consumer is done reading.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        public void Complete(Exception exception = null) => _pipe.CompleteReader(exception);

        /// <summary>
        /// Asynchronously reads a sequence of bytes from the current <see cref="PipelineReader"/>.
        /// </summary>
        /// <returns>A <see cref="ReadableBufferAwaitable"/> representing the asynchronous read operation.</returns>
        public ReadableBufferAwaitable ReadAsync() => _pipe.ReadAsync();
    }
}
