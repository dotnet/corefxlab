// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


namespace System.Buffers
{
    public struct DisposableReservation : IDisposable
    {
        IRetainable _owner;

        internal DisposableReservation(IRetainable owner)
        {
            _owner = owner;
            _owner.Retain();
        }

        public void Dispose()
        {
            _owner.Release();
            _owner = null;
        }
    }
}
