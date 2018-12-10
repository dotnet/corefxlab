// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab
{
    internal ref struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        internal T _output;
        private int _buffered;

        public Span<byte> Buffer { get; private set; }

        public long BytesWritten
        {
            get
            {
                Debug.Assert(BytesCommitted <= long.MaxValue - _buffered);
                return BytesCommitted + _buffered;
            }
        }

        public long BytesCommitted { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BufferWriter(T output)
        {
            _output = output;
            _buffered = 0;
            BytesCommitted = 0;
            Buffer = output.GetSpan();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Flush()
        {
            BytesCommitted += _buffered;
            _output.Advance(_buffered);
            _buffered = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            Debug.Assert(count >= 0 && _buffered <= int.MaxValue - count);

            _buffered += count;
            Buffer = Buffer.Slice(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ensure(int count)
        {
            Debug.Assert(count >= 0);

            if (Buffer.Length < count)
                EnsureMore(count);

            Debug.Assert(Buffer.Length >= count);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void EnsureMore(int count)
        {
            Flush();
            Buffer = _output.GetSpan(count);
        }
    }
}
