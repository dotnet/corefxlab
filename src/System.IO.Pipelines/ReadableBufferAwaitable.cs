// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    /// <summary>
    /// An awaitable object that represents an asynchronous read operation
    /// </summary>
    public struct ReadableBufferAwaitable : ICriticalNotifyCompletion
    {
        private readonly IReadableBufferAwaiter _awaiter;

        public ReadableBufferAwaitable(IReadableBufferAwaiter awaiter)
        {
            _awaiter = awaiter;
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public ReadResult GetResult() => _awaiter.GetResult();

        public ReadableBufferAwaitable GetAwaiter() => this;

        public void UnsafeOnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);

        public void OnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);
    }
}
