// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Threading
{
    public interface IAwaiter<out T>
    {
        bool IsCompleted { get; }

        T GetResult();

        void OnCompleted(Action continuation);
    }
}
