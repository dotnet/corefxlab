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
            IOutput output = new Output();
            var writer = BufferWriter.Create(output);
            writer.WriteBytes(Encoding.UTF8.GetBytes("Hello"));
            writer.WriteBytes(Encoding.UTF8.GetBytes(" World!"));
            writer.Flush();
            Assert.Equal("Hello World!", output.ToString());
        }

        [Fact]
        public void Writable()
        {
            IOutput output = new Output();
            var writer = BufferWriter.Create(output);

            var ulonger = new UInt128();
            ulonger.Lower = ulong.MaxValue;
            ulonger.Upper = 1;

            writer.WriteBytes(ulonger, s_base64);
            writer.WriteBytes(ulonger, 't');
            writer.Write(123);
            writer.Write("This is just a longish string");
            writer.Flush();
            Assert.Equal("//////////8BAAAAAAAAAA==hello123This is just a longish string", output.ToString());
        }
    }

    class Output : IOutput
    {
        byte[] _current = new byte[0];
        List<byte[]> _commited = new List<byte[]>();

        public Span<byte> GetSpan() => _current;

        public void Advance(int bytes)
        {
            if (bytes == 0) return;
            _commited.Add(_current.AsSpan().Slice(0, bytes).ToArray());
            _current = new byte[0];
        }

        public void Enlarge(int desiredBufferLength = 0)
        {
            if (desiredBufferLength == 0) desiredBufferLength = _current.Length + 1;
            if (desiredBufferLength < _current.Length) throw new InvalidOperationException();
            var newBuffer = new byte[desiredBufferLength];
            _current.CopyTo(newBuffer.AsSpan());
            _current = newBuffer;
        }

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
