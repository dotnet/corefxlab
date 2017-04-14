// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime;

namespace System.Buffers
{
    public struct DisposableReservation<T> : IDisposable
    {
        OwnedBuffer<T> _owner;

        internal DisposableReservation(OwnedBuffer<T> owner)
        {
            _owner = owner;
            switch (ReferenceCountingSettings.OwnedMemory)
            {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).AddReference();
                    break;
                case ReferenceCountingMethod.ReferenceCounter:
                    ReferenceCounter.AddReference(_owner);
                    break;
                case ReferenceCountingMethod.None:
                    break;
            }
        }

        public Span<T> Span => _owner.Span;

        public void Dispose()
        {
            switch (ReferenceCountingSettings.OwnedMemory)
            {
                case ReferenceCountingMethod.Interlocked:
                    ((IKnown)_owner).Release();
                    break;
                case ReferenceCountingMethod.ReferenceCounter:
                    ReferenceCounter.Release(_owner);
                    break;
                case ReferenceCountingMethod.None:
                    break;
            }
            _owner = null;
        }
    }
}
