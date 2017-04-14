// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal class OwnerEmptyMemory<T> : OwnedBuffer<T>
    {
        readonly static T[] s_empty = new T[0];
        public readonly static OwnedBuffer<T> Shared = new OwnerEmptyMemory<T>();

        public OwnerEmptyMemory() : base(s_empty, 0, 0) { }

        protected override void Dispose(bool disposing)
        {}
    }
}