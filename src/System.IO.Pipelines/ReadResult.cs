// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public struct ReadResult
    {
        public ReadResult(ReadableBuffer buffer, bool isCompleted)
        {
            Buffer = buffer;
            IsCompleted = isCompleted;
        }

        public ReadableBuffer Buffer { get; }

        public bool IsCompleted { get; }
    }
}