// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public struct FlushResult
    {
        public FlushResult(bool isCancelled, bool isCompleted)
        {
            IsCancelled = isCancelled;
            IsCompleted = isCompleted;
        }

        /// <summary>
        /// True if the currrent flush was cancelled
        /// </summary>
        public bool IsCancelled { get; }

        /// <summary>
        /// True if the <see cref="IPipeWriter"/> is complete
        /// </summary>
        public bool IsCompleted { get; }
    }
}
