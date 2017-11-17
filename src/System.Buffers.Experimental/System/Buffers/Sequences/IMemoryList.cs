// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public interface IMemoryList<T> : ISequence<Memory<T>>, ISequence<ReadOnlyMemory<T>>
    {
        Memory<T> Memory { get; }

        IMemoryList<T> Rest { get; }

        long VirtualIndex { get; }

        int CopyTo(Span<T> buffer);
    }
}
