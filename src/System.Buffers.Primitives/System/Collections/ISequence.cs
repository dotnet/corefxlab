// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Collections.Sequences
{
    // new interface
    public interface ISequence<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="advance"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool TryGet(ref SequencePosition position, out T item, bool advance = true);

        SequencePosition GetPosition(SequencePosition origin, long offset);

        SequencePosition Start { get; }
    }
}
