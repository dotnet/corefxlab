// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;

namespace System.Buffers
{
    public abstract class ReferenceCountedBuffer<T> : OwnedBuffer<T>
    {
        int _referenceCount;

        public override void AddReference()
        {
            Interlocked.Increment(ref _referenceCount);
        }

        public override void Release()
        {
            if (Interlocked.Decrement(ref _referenceCount) == 0) {
                OnZeroReferences();
            }
        }

        public override bool HasOutstandingReferences => _referenceCount > 0;
    }
}