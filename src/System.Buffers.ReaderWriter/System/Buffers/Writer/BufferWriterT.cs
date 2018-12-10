// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter<T> where T : IBufferWriter<byte>
    {
        private T _output;
        private Span<byte> _span;
        private int _buffered;

        private static readonly byte[] s_newLine = Encoding.UTF8.GetBytes(Environment.NewLine);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BufferWriter(T output)
        {
            _buffered = 0;
            _output = output;
            _span = output.GetSpan();
        }

        private static ReadOnlySpan<byte> NewLine => s_newLine;

        public Span<byte> Buffer => _span;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Flush()
        {
            int buffered = _buffered;
            if (buffered > 0)
            {
                _buffered = 0;
                _output.Advance(buffered);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Advance(int count)
        {
            _buffered += count;
            _span = _span.Slice(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Ensure(int count = 1)
        {
            if (_span.Length < count)
            {
                EnsureMore(count);
            }
            Debug.Assert(_span.Length >= count);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void EnsureMore(int count = 0)
        {
            Flush();
            _span = _output.GetSpan(count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Enlarge()
        {
            EnsureMore(_span.Length + 1);
        }
    }
}
