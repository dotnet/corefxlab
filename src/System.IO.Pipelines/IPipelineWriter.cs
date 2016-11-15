using System;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    /// <summary>
    /// Defines a class that provides a channel to which data can be written.
    /// </summary>
    public interface IPipelineWriter
    {
        /// <summary>
        /// Gets a task that completes when no more data will be read from the channel.
        /// </summary>
        /// <remarks>
        /// This task indicates the consumer has completed and will not read anymore data.
        /// When this task is triggered, the producer should stop producing data.
        /// </remarks>
        Task Writing { get; }

        /// <summary>
        /// Allocates memory from the channel to write into.
        /// </summary>
        /// <param name="minimumSize">The minimum size buffer to allocate</param>
        /// <returns>A <see cref="WritableBuffer"/> that can be written to.</returns>
        WritableBuffer Alloc(int minimumSize = 0);

        /// <summary>
        /// Marks the channel as being complete, meaning no more items will be written to it.
        /// </summary>
        /// <param name="exception">Optional Exception indicating a failure that's causing the channel to complete.</param>
        void Complete(Exception exception = null);
    }
}
