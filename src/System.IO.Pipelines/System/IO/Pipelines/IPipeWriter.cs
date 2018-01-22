// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Threading;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a pipeline to which data can be written.
    /// </summary>
    public interface IPipeWriter: IOutput
    {
        /// <summary>
        /// Marks the pipeline as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the pipeline to complete.</param>
        void Complete(Exception exception = null);

        /// <summary>
        /// Cancel to currently pending or next call to <see cref="ReadAsync"/> if none is pending, without completing the <see cref="IPipeReader"/>.
        /// </summary>
        void CancelPendingFlush();

        /// <summary>
        /// Registers callback that gets executed when reader side of pipe completes.
        /// </summary>
        void OnReaderCompleted(Action<Exception, object> callback, object state);

        ValueAwaiter<FlushResult> FlushAsync(CancellationToken cancellationToken = default);

        void Commit();
    }
}
