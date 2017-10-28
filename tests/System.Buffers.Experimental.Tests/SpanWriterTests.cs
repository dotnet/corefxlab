// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Binary.Base64;
using System.Buffers;
using System.Buffers.Text;
using System.Text;
using System.Text.Utf8;
using Xunit;
using static System.Buffers.Binary.BinaryPrimitives;

namespace System.Buffers.Tests
{
    public partial class SpanWritersTests
    {
        static IBufferTransformation[] s_transformations = new IBufferTransformation[] {
            Encodings.Ascii.ToLowercase,
            Encodings.Ascii.ToUppercase,
            Base64.Utf8ToBytesDecoder,
            Base64.BytesToUtf8Encoder
        };

        [Fact]
        public void Basics()
        {
            Span<IBufferTransformation> transformations = s_transformations;
            Span<byte> buffer = stackalloc byte[256];
            var writer = new SpanWriter(buffer);

            writer.Index = 0;
            writer.Write("AaBc", transformations.Slice(0, 2));
            Assert.Equal("AABC", Encodings.Utf8.ToString(writer.Written));

            writer.Index = 0;
            writer.Write("AaBc", transformations.Slice(0, 1));
            Assert.Equal("aabc", Encodings.Utf8.ToString(writer.Written));

            writer.Index = 0;
            writer.Write("AaBc", transformations.Slice(1, 1));
            Assert.Equal("AABC", Encodings.Utf8.ToString(writer.Written));

            writer.Index = 0;
            writer.Write("AaBc", transformations);
            Assert.Equal("AABC", Encodings.Utf8.ToString(writer.Written));
        }

        [Fact]
        public void Writable()
        {
            Span<IBufferTransformation> transformations = s_transformations;
            Span<byte> buffer = stackalloc byte[256];
            var writer = new SpanWriter(buffer);

            var ulonger = new UInt128();
            ulonger.Lower = ulong.MaxValue;
            ulonger.Upper = 1;

            writer.WriteBytes(ulonger, default, transformations.Slice(3));
            var result = Encodings.Utf8.ToString(writer.Written);
            Assert.Equal("//////////8BAAAAAAAAAA==", result);

            var ulongerSpan = new Span<UInt128>(new UInt128[1]); 
            Assert.Equal(OperationStatus.Done, Base64.DecodeFromUtf8(writer.Written, ulongerSpan.AsBytes(), out int consumed, out int written));
            Assert.Equal(ulongerSpan[0].Lower, ulonger.Lower);
            Assert.Equal(ulongerSpan[0].Upper, ulonger.Upper);
        }
    }

    public struct UInt128 : IWritable
    {
        public ulong Lower;
        public ulong Upper;

        const int size = sizeof(ulong) * 2;

        public bool TryWrite(Span<byte> buffer, out int written, ParsedFormat format = default)
        {
            if (!format.IsDefault) throw new Exception("invalid format");

            if (buffer.Length < size)
            {
                written = 0;
                return false;
            }

            if (BitConverter.IsLittleEndian)
            {
                WriteMachineEndian(buffer, ref Lower);
                WriteMachineEndian(buffer.Slice(sizeof(ulong)), ref Upper);
            }
            else
            {
                WriteMachineEndian(buffer, ref Upper);
                WriteMachineEndian(buffer.Slice(sizeof(ulong)), ref Lower);
            }
            written = size;
            return true;
        }
    }
}
