// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public interface IWritableBufferAwaiter
    {
        bool IsCompleted { get; }

        FlushResult GetResult();

        bool IsCompletedSuccessfully { get; }

        void OnCompleted(Action continuation);
    }
}