// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.Buffers.Internal
{
    internal sealed class OwnedEmptyBuffer<T> : OwnedBuffer<T>
    {
        public readonly static OwnedBuffer<T> Shared = new OwnedEmptyBuffer<T>();

        public override int Length => 0;

        public override Span<T> AsSpan(int index, int length) => Span<T>.Empty;

        protected override void Dispose(bool disposing)
        {}

        protected internal override bool TryGetArray(out ArraySegment<T> buffer)
        {
            buffer = default(ArraySegment<T>);
            return true;
        }

        public override void Retain() { }
        public override void Release() { }

        public override BufferHandle Pin(int index = 0)
        {
            return default(BufferHandle);
        }

        public override bool IsRetained => false;

        public override bool IsDisposed => false;
    }
}
