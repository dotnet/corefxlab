// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers.Internal
{
    internal class OwnerEmptyMemory<T> : OwnedBuffer<T>
    {
        readonly static T[] s_empty = new T[0];
        public readonly static OwnedBuffer<T> Shared = new OwnerEmptyMemory<T>();

        public override int Length => s_empty.Length;

        public override Span<T> Span => s_empty;

        protected override void Dispose(bool disposing)
        {}

        public override Span<T> GetSpan(int index, int length)
        {
            if (IsDisposed) ThrowObjectDisposed();
            if (index > 0 || length > 0) ThrowIndexOutOfRange();
            return Span;
        }

        public override BufferHandle Pin(int index = 0)
        {
            return BufferHandle.Create(this, index);
        }

        protected internal override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(s_empty);
            return true;
        }

        protected internal override unsafe bool TryGetPointerInternal(out void* pointer)
        {
            pointer = null;
            return false;
        }
    }
}