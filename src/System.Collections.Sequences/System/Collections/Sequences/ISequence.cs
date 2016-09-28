// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections.Sequences
{
    // new interface
    public interface ISequence<T>
    {
        T TryGetItem(ref Position position);

        SequenceEnumerator<T> GetEnumerator();
    }
}
