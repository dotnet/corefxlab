// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace System.IO.Pipelines
{
    public struct WritableBufferAwaitable : ICriticalNotifyCompletion
    {
        private readonly IWritableBufferAwaiter _awaiter;

        public WritableBufferAwaitable(IWritableBufferAwaiter awaiter)
        {
            _awaiter = awaiter;
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public void GetResult() => _awaiter.GetResult();

        public WritableBufferAwaitable GetAwaiter() => this;

        public void UnsafeOnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);

        public void OnCompleted(Action continuation) => _awaiter.OnCompleted(continuation);
    }
}
