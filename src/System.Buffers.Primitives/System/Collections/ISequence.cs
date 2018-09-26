// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
