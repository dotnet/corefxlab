// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.JsonLab
{
    internal class MemoryFormatter : IBufferWriter<byte>
    {
        private Memory<byte> _memory;

        public MemoryFormatter(Memory<byte> memory)
        {
            _memory = memory;
        }

        // Slice already does the necessary argument validation ( < 0 or > _memory.Length).
        public void Advance(int count)
            => _memory = _memory.Slice(count);

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            if (sizeHint == 0)
                return _memory;

            // Do we need to slice the _memory at all? Maybe just validate sizeHint is in range and return the entire memory?
            return _memory.Slice(0, sizeHint);
        }

        // Slice already does the necessary argument validation ( < 0 or > _memory.Length).
        public Span<byte> GetSpan(int sizeHint = 0)
        {
            Span<byte> span = _memory.Span;

            if (sizeHint == 0)
                return span;

            return span.Slice(0, sizeHint);
        }
    }
}
