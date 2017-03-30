// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// The result of a <see cref="IPipeReader.ReadAsync"/> call.
    /// </summary>
    public struct ReadResult
    {
        internal BufferSegment Segment;
        internal int Index;
        internal int Length;
        internal ResultFlags ResultFlags;

        public ReadResult(ReadableBuffer buffer, bool isCancelled, bool isCompleted)
        {
            Segment = buffer.BufferStart.Segment;
            Index = buffer.BufferStart.Index;
            Length = buffer.BufferLength;
            ResultFlags = ResultFlags.None;

            if (isCompleted)
            {
                ResultFlags |= ResultFlags.Completed;
            }
            if (isCancelled)
            {
                ResultFlags |= ResultFlags.Cancelled;
            }
        }

        /// <summary>
        /// The <see cref="ReadableBuffer"/> that was read
        /// </summary>
        public ReadableBuffer Buffer => new ReadableBuffer(Segment, Index, Length);

        /// <summary>
        /// True if the currrent read was cancelled
        /// </summary>
        public bool IsCancelled => (ResultFlags & ResultFlags.Cancelled) != 0;

        /// <summary>
        /// True if the <see cref="IPipeReader"/> is complete
        /// </summary>
        public bool IsCompleted => (ResultFlags & ResultFlags.Completed) != 0;
    }
}