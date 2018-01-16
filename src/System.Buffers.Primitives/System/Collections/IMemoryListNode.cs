// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections.Sequences
{
    public interface IMemoryListNode<T>
    {
        Memory<T> Memory { get; }

        IMemoryListNode<T> Next { get; }

        long RunningLength { get; }
    }
}
