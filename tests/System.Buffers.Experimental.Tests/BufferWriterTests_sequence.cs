// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Binary.Base64Experimental;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace System.Buffers.Tests
{
    public partial class GenericBufferWriterTests
    {
        TransformationFormat s_base64 = new TransformationFormat(Base64Experimental.BytesToUtf8Encoder);

        [Fact]
        public void Bytes()
        {
            IBufferWriter<byte> bufferWriter = new BufferWriter();
            var writer = Text.BufferWriter.Create(bufferWriter);
            writer.WriteBytes(Encoding.UTF8.GetBytes("Hello"));
            writer.WriteBytes(Encoding.UTF8.GetBytes(" World!"));
            writer.Flush();
            Assert.Equal("Hello World!", bufferWriter.ToString());
        }

        [Fact]
        public void Writable()
        {
            IBufferWriter<byte> bufferWriter = new BufferWriter();
            var writer = Text.BufferWriter.Create(bufferWriter);

            var ulonger = new UInt128();
            ulonger.Lower = ulong.MaxValue;
            ulonger.Upper = 1;

            writer.WriteBytes(ulonger, s_base64);
            writer.WriteBytes(ulonger, 't');
            writer.Write(123);
            writer.Write("This is just a longish string");
            writer.Flush();
            Assert.Equal("//////////8BAAAAAAAAAA==hello123This is just a longish string", bufferWriter.ToString());
        }
    }

    class BufferWriter : IBufferWriter<byte>
    {
        byte[] _current = new byte[0];
        List<byte[]> _commited = new List<byte[]>();

        public void Advance(int bytes)
        {
            if (bytes == 0) return;
            _commited.Add(_current.AsSpan().Slice(0, bytes).ToArray());
            _current = new byte[0];
        }

        public Memory<byte> GetMemory(int minimumLength = 0)
        {
            if (minimumLength == 0) minimumLength = _current.Length + 1;
            if (minimumLength < _current.Length) throw new InvalidOperationException();
            var newBuffer = new byte[minimumLength];
            _current.CopyTo(newBuffer.AsSpan());
            _current = newBuffer;
            return _current;
        }

        public Span<byte> GetSpan(int minimumLength) => GetMemory(minimumLength).Span;

        public int MaxBufferSize { get; } = Int32.MaxValue;

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var buffer in _commited)
            {
                builder.Append(Encoding.UTF8.GetString(buffer));
            }
            return builder.ToString();
        }
    }
}
