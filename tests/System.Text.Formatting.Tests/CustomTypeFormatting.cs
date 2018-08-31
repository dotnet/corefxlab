// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.IO;
using Xunit;

namespace System.Text.Formatting.Tests
{
    struct Age : IBufferFormattable
    {
        int _age;
        bool _inMonths;

        public Age(int age, bool inMonths = false)
        {
            _age = age;
            _inMonths = inMonths;
        }

        public bool TryFormat(Span<byte> buffer, out int bytesWritten, StandardFormat format, SymbolTable symbolTable)
        {
            if (!CustomFormatter.TryFormat(_age, buffer, out bytesWritten, format, symbolTable))
                return false;

            char symbol = _inMonths ? 'm' : 'y';
            if (!symbolTable.TryEncode((byte)symbol, buffer.Slice(bytesWritten), out int written))
                return false;

            bytesWritten += written;
            return true;
        }

        public override string ToString()
        {
            return _age.ToString() + (_inMonths ? "m" : "y");
        }
    }

    public class CustomTypeFormatting
    {
        static ArrayPool<byte> pool = ArrayPool<byte>.Shared;

        [Fact]
        public void CustomTypeToStreamUtf16()
        {
            byte[] buffer = new byte[1024];
            MemoryStream stream = new MemoryStream(buffer);
            using (var writer = new StreamFormatter(stream, pool))
            {
                writer.Append(new Age(56));
                writer.Append(new Age(14, inMonths: true));

                var writtenText = Encoding.Unicode.GetString(buffer, 0, (int)stream.Position);
                Assert.Equal(writtenText, "56y14m");
            }
        }

        [Fact]
        public void CustomTypeToStreamUtf8()
        {
            byte[] buffer = new byte[1024];
            MemoryStream stream = new MemoryStream(buffer);
            using (var writer = new StreamFormatter(stream, SymbolTable.InvariantUtf8, pool))
            {
                writer.Append(new Age(56));
                writer.Append(new Age(14, inMonths: true));
                var writtenText = Encoding.UTF8.GetString(buffer, 0, (int)stream.Position);
                Assert.Equal(writtenText, "56y14m");
            }
        }

        [Fact]
        public void CustomTypeToString()
        {
            var sb = new StringFormatter();
            sb.Append(new Age(56));
            sb.Append(new Age(14, inMonths: true));
            Assert.Equal(sb.ToString(), "56y14m");
        }
    }
}
