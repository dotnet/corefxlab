// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public interface IReadOnlyMemorySequence<T> : ISequence<ReadOnlyMemory<T>>
    {
        int CopyTo(Span<T> buffer);
        ReadOnlyMemory<T> First { get; }
    }

    public interface IMemorySequence<T> : IReadOnlyMemorySequence<T>, ISequence<Memory<T>>
    {
        new Memory<T> Memory { get; }
        IMemorySequence<T> Rest { get; }
    }

    public interface IReadOnlyMemoryList<T> : IReadOnlyMemorySequence<T>
    {
        IReadOnlyMemoryList<T> Rest { get; }

        long Index { get; }
    }
}
