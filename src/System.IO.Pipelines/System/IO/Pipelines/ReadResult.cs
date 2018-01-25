// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;

namespace System.IO.Pipelines
{
    /// <summary>
    /// The result of a <see cref="PipeReader.ReadAsync"/> call.
    /// </summary>
    public struct ReadResult
    {
        internal ReadOnlyBuffer<byte> ResultBuffer;
        internal ResultFlags ResultFlags;

        public ReadResult(ReadOnlyBuffer<byte> buffer, bool isCancelled, bool isCompleted)
        {
            ResultBuffer = buffer;
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
        /// The <see cref="ReadOnlyBuffer"/> that was read
        /// </summary>
        public ReadOnlyBuffer<byte> Buffer => ResultBuffer;

        /// <summary>
        /// True if the currrent read was cancelled
        /// </summary>
        public bool IsCancelled => (ResultFlags & ResultFlags.Cancelled) != 0;

        /// <summary>
        /// True if the <see cref="PipeReader"/> is complete
        /// </summary>
        public bool IsCompleted => (ResultFlags & ResultFlags.Completed) != 0;
    }
}
