// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public interface IReadOnlyBuffer<T> : ISequence<ReadOnlyMemory<T>>
    {
        int CopyTo(Span<T> buffer);
        ReadOnlyMemory<T> First { get; }
    }

    public interface IReadOnlyBufferList<T> : IReadOnlyBuffer<T>
    {
        IReadOnlyBufferList<T> Rest { get; }

        long Index { get; }
    }
}
