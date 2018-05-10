// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Runtime.CompilerServices;

namespace System.Buffers.Writer
{
    public ref partial struct BufferWriter
    {
        private Span<byte> _buffer;
        private int _written;

        public ReadOnlySpan<byte> NewLine { get; set; }
        public Func<int, Memory<byte>> Enlarge { get; set; }

        private static byte[] s_defaultNewline = new byte[] { (byte)'\n' };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BufferWriter Create(Span<byte> buffer) => new BufferWriter(buffer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BufferWriter<TOutput> Create<TOutput>(TOutput output)
            where TOutput : IBufferWriter<byte>
            => new BufferWriter<TOutput>(output);

        private BufferWriter(Span<byte> buffer)
        {
            _buffer = buffer;
            _written = 0;
            NewLine = s_defaultNewline;
            Enlarge = null;
        }

        public Span<byte> Free => _buffer.Slice(_written);

        public Span<byte> Written => _buffer.Slice(0, _written);

        public int WrittenCount
        {
            get { return _written; }
            set
            {
                while (value > _buffer.Length) Resize(value);
                _written = value;
            }
        }

        public void Clear()
            => _written = 0;

        private void Resize(int desiredBufferSize = 0)
        {
            if (Enlarge == null) throw new BufferTooSmallException();
            // double the size; add one to ensure that an empty buffer still resizes
            if (desiredBufferSize <= 0) desiredBufferSize = _buffer.Length * 2 + 1;
            else if (desiredBufferSize < _buffer.Length + 1) throw new ArgumentOutOfRangeException(nameof(desiredBufferSize));
            var newBuffer = Enlarge(desiredBufferSize).Span;
            if (newBuffer.Length <= _buffer.Length) throw new Exception("Enlarge delegate created too small buffer");
            Written.CopyTo(newBuffer);
            _buffer = newBuffer;
        }

        public override string ToString()
            => Encodings.Utf8.ToString(Written);

        public class BufferTooSmallException : Exception
        {
            public BufferTooSmallException() : base("Buffer is too small") { }
        }
    }
}
