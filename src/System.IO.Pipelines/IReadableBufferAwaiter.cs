// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.IO.Pipelines
{
    public interface IReadableBufferAwaiter
    {
        bool IsCompleted { get; }

        ReadResult GetResult();

        void OnCompleted(Action continuation);
    }
}
