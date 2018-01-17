// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections.Sequences
{
    public interface IBufferList<T>
    {
        Memory<T> Memory { get; }

        IBufferList<T> Next { get; }

        long RunningIndex { get; }
    }
}
