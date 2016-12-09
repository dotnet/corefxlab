// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// The result of a <see cref="IPipelineReader.ReadAsync"/> call.
    /// </summary>
    public struct ReadResult
    {
        public ReadResult(ReadableBuffer buffer, bool isCancelled, bool isCompleted)
        {
            Buffer = buffer;
            IsCancelled = isCancelled;
            IsCompleted = isCompleted;
        }

        /// <summary>
        /// The <see cref="ReadableBuffer"/> that was read
        /// </summary>
        public ReadableBuffer Buffer { get; }

        /// <summary>
        /// True if the currrent read was cancelled
        /// </summary>
        public bool IsCancelled { get; }

        /// <summary>
        /// True if the <see cref="IPipelineReader"/> is complete
        /// </summary>
        public bool IsCompleted { get; }
    }
}