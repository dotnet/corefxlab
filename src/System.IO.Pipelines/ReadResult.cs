// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    /// <summary>
    /// The result of a <see cref="IPipeReader.ReadAsync"/> call.
    /// </summary>
    public struct ReadResult
    {

        /// <summary>
        /// The <see cref="ReadableBuffer"/> that was read
        /// </summary>
        public ReadableBuffer Buffer;

        internal ResultFlags _flags;
        /// <summary>
        /// True if the currrent read was cancelled
        /// </summary>
        public bool IsCancelled => (_flags & ResultFlags.Cancelled) > 0;

        /// <summary>
        /// True if the <see cref="IPipeReader"/> is complete
        /// </summary>
        public bool IsCompleted => (_flags & ResultFlags.Completed) > 0;
    }

    [Flags]
    internal enum ResultFlags: byte
    {
        None = 0,
        Cancelled = 1,
        Completed = 1 >> 2
    }
}