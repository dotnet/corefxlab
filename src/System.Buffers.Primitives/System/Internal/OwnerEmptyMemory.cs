// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime;

namespace System.Buffers.Internal
{
    internal class OwnedEmptyBuffer<T> : OwnedBuffer<T>
    {
        T[] s_empty = new T[0];
        public readonly static OwnedBuffer<T> Shared = new OwnedEmptyBuffer<T>();

        public override int Length => 0;

        public override Span<T> Span => Span<T>.Empty;

        protected override void Dispose(bool disposing)
        {}

        protected internal override bool TryGetArrayInternal(out ArraySegment<T> buffer)
        {
            buffer = new ArraySegment<T>(s_empty, 0, 0);
            return true;
        }

        protected internal override unsafe bool TryGetPointerAt(int index, out void* pointer)
        {
            pointer = null;
            return false;
        }

        public override void Retain() { }
        public override void Release() { }
        public override bool IsRetained => false;

        public override bool IsDisposed => false;
    }
}