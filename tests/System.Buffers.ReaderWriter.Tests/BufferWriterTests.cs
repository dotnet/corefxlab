// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Binary.Base64Experimental;
using System.Buffers.Text;
using System.Buffers.Writer;
using System.Text;
using Xunit;

using static System.Runtime.InteropServices.MemoryMarshal;

namespace System.Buffers.Tests
{
    public partial class BufferWriterTests
    {
        private TransformationFormat s_base64 = new TransformationFormat(Base64Experimental.BytesToUtf8Encoder);

        [Fact]
        public void Basics()
        {
            Span<byte> buffer = stackalloc byte[256];
            var writer = BufferWriter.Create(buffer);

            writer.WrittenCount = 0;
            writer.Write("AaBc", new TransformationFormat(Encodings.Ascii.ToLowercase, Encodings.Ascii.ToUppercase));
            Assert.Equal("AABC", Encodings.Utf8.ToString(writer.Written));

            writer.WrittenCount = 0;
            writer.Write("AaBc", new TransformationFormat(Encodings.Ascii.ToLowercase));
            Assert.Equal("aabc", Encodings.Utf8.ToString(writer.Written));

            writer.WrittenCount = 0;
            writer.Write("AaBc", new TransformationFormat(Encodings.Ascii.ToUppercase));
            Assert.Equal("AABC", Encodings.Utf8.ToString(writer.Written));

            writer.WrittenCount = 0;
            writer.Write("AaBc", new TransformationFormat(
                Encodings.Ascii.ToUppercase,
                Base64Experimental.Utf8ToBytesDecoder,
                Base64Experimental.BytesToUtf8Encoder)
            );
            Assert.Equal("AABC", Encodings.Utf8.ToString(writer.Written));
        }

        [Fact]
        public void Writable()
        {
            Span<byte> buffer = stackalloc byte[256];
            var writer = BufferWriter.Create(buffer);

            var ulonger = new UInt128();
            ulonger.Lower = ulong.MaxValue;
            ulonger.Upper = 1;

            writer.WriteBytes(ulonger, s_base64);
            var result = Encodings.Utf8.ToString(writer.Written);
            Assert.Equal("//////////8BAAAAAAAAAA==", result);

            var ulongerSpan = new Span<UInt128>(new UInt128[1]);
            Assert.Equal(OperationStatus.Done, Base64.DecodeFromUtf8(writer.Written, AsBytes(ulongerSpan), out int consumed, out int written));
            Assert.Equal(ulongerSpan[0].Lower, ulonger.Lower);
            Assert.Equal(ulongerSpan[0].Upper, ulonger.Upper);
        }

        [Fact]
        public void WriteDateTime()
        {
            var now = DateTime.UtcNow;
            Span<byte> buffer = stackalloc byte[256];
            var writer = BufferWriter.Create(buffer);
            writer.WriteLine(now, 'R');
            var result = Encodings.Utf8.ToString(writer.Written);
            Assert.Equal(string.Format("{0:R}\n", now), result);
        }
    }

    public struct UInt128 : IWritable
    {
        public ulong Lower;
        public ulong Upper;
        private const int size = sizeof(ulong) * 2;

        public bool TryWrite(Span<byte> buffer, out int written, StandardFormat format = default)
        {
            if (format == default)
            {
                if (buffer.Length < size)
                {
                    written = 0;
                    return false;
                }

                if (BitConverter.IsLittleEndian)
                {
                    Write(buffer, ref Lower);
                    Write(buffer.Slice(sizeof(ulong)), ref Upper);
                }
                else
                {
                    Write(buffer, ref Upper);
                    Write(buffer.Slice(sizeof(ulong)), ref Lower);
                }
                written = size;
                return true;
            }
            if (format.Symbol == 't')
            {
                var utf8 = Encoding.UTF8.GetBytes("hello").AsSpan();
                if (utf8.TryCopyTo(buffer))
                {
                    written = utf8.Length;
                    return true;
                }
                written = 0;
                return false;
            }

            throw new Exception("invalid format");
        }
    }
}
