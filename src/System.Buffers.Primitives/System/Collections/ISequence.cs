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
        /// <param name="sequenceIndex"></param>
        /// <param name="advance"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        bool TryGet(ref SequenceIndex sequenceIndex, out T item, bool advance = true);

        SequenceIndex GetPosition(SequenceIndex origin, long offset);

        SequenceIndex Start { get; }
    }
}
