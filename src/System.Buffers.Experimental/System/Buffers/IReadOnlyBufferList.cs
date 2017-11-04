// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Sequences;

namespace System.Buffers
{
    public interface IReadOnlyBufferList<T> : ISequence<ReadOnlyMemory<T>>
    {
        int CopyTo(Span<T> buffer);
        ReadOnlyMemory<T> First { get; }

        IReadOnlyBufferList<T> Rest { get; }

        long Index { get; }
    }
}
