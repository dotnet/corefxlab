// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public interface IMemorySegment<T> : ISequence<Memory<T>>, ISequence<ReadOnlyMemory<T>>
    {
        Memory<T> First { get; }

        IMemorySegment<T> Rest { get; }

        long Index { get; }

        int CopyTo(Span<T> buffer);
    }
}
