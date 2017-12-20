// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// The result of a <see cref="IPipeReader.ReadAsync"/> call.
    /// </summary>
    public struct ReadResult
    {
        internal ReadOnlyBuffer ResultBuffer;
        internal ResultFlags ResultFlags;

        public ReadResult(ReadOnlyBuffer buffer, bool isCancelled, bool isCompleted)
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
        public ReadOnlyBuffer Buffer => ResultBuffer;

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