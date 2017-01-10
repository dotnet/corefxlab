// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Sequences;

public interface IReadOnlyMemoryList<T> : ISequence<ReadOnlyMemory<T>>
{
    int CopyTo(Span<T> buffer);
    ReadOnlyMemory<T> First { get; }

    IReadOnlyMemoryList<T> Rest { get; }
}