// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Utf8;
using System.Runtime.InteropServices;

namespace System.Text.Formatting
{
    // The data for culture-aware formatting is stored in this assembly resources
    // The data were produced from Unicode Locale Data Repository: http://cldr.unicode.org
    // using a custom tool that I will try to clean, debug, and commit to this repo.
    // For now, the data might contain errors.
    public class EncodingProvider
    {
        public static SymbolTable CreateEncoding(string localeId)
        {
            var resourceName = "System.Text.Formatting.Globalization.locales.bin";
            var resourceStream = typeof(EncodingProvider).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new Exception("resource missing");
            }

            using (resourceStream)
            {
                return CreateSymbolTable(localeId, resourceStream);
            }
        }

        private static SymbolTable CreateSymbolTable(string localeId, Stream resourceStream)
        {
            const int maxIdLength = 15;
            const int recordSize = 20;

            var b1 = resourceStream.ReadByte();
            var b2 = resourceStream.ReadByte();
            var numberOfIDs = b1 * 256 + b2;

            var indexSize = numberOfIDs * 20;
            var index = new byte[indexSize];
            resourceStream.Read(index, 0, indexSize);

            byte[] idBytes = new byte[maxIdLength];
            var status = Encodings.Utf16.ToUtf8(MemoryMarshal.AsBytes(localeId.AsSpan()), idBytes, out int consumed, out int idByteCount);
            if (status != System.Buffers.OperationStatus.Done)
                throw new Exception("bad locale id");

            var id = new Utf8Span(idBytes.AsSpan(0, idByteCount));

            int recordStart = -1;
            for (int record = 0; record < numberOfIDs; record++)
            {
                var indexId = index.AsSpan(record * recordSize, idByteCount);
                if (id.Equals(new Utf8Span(indexId))) // found record
                {
                    var indexData = index.AsSpan(record * recordSize + maxIdLength);
                    recordStart = 0;
                    recordStart += indexData[3] * 256 * 256 * 256;
                    recordStart += indexData[2] * 256 * 256;
                    recordStart += indexData[1] * 256;
                    recordStart += indexData[0];
                    break;
                }
            }

            if (recordStart == -1)
            {
                throw new Exception("local not found");
            }

            resourceStream.Position = recordStart;

            const int bufferSize = 512;
            var data = new byte[bufferSize];
            var bytesRead = resourceStream.Read(data, 0, bufferSize);
            // TODO: maybe we should store length in the index

            var numberOfStrings = ReadUInt16At(data, 0);
            Debug.Assert(numberOfStrings == 17);

            var utf16digitsAndSymbols = new byte[numberOfStrings][];

            for (int stringIndex = 0; stringIndex < numberOfStrings; stringIndex++)
            {
                var stringStart = ReadUInt16At(data, stringIndex * 2 + 1);
                var stringLength = ReadUInt16At(data, stringIndex * 2 + 2);
                utf16digitsAndSymbols[stringIndex] = new byte[stringLength];
                Array.Copy(data, stringStart, utf16digitsAndSymbols[stringIndex], 0, stringLength);
            }

            return new CultureUtf16SymbolTable(utf16digitsAndSymbols);
        }

        static ushort ReadUInt16At(byte[] data, int ushortIndex)
        {
            ushortIndex *= 2;
            ushort value = (ushort)(data[ushortIndex + 1] * 256 + data[ushortIndex + 0]);
            return value;
        }

        class CultureUtf16SymbolTable : SymbolTable
        {
            public CultureUtf16SymbolTable(byte[][] symbols) : base(symbols) {}

            public override bool TryEncode(byte utf8, Span<byte> destination, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryEncode(utf8, destination, out bytesWritten);

            public override bool TryEncode(ReadOnlySpan<byte> utf8, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryEncode(utf8, destination, out bytesConsumed, out bytesWritten);

            public override bool TryParse(ReadOnlySpan<byte> source, out byte utf8, out int bytesConsumed)
                => SymbolTable.InvariantUtf16.TryParse(source, out utf8, out bytesConsumed);

            public override bool TryParse(ReadOnlySpan<byte> source, Span<byte> utf8, out int bytesConsumed, out int bytesWritten)
                => SymbolTable.InvariantUtf16.TryParse(source, utf8, out bytesConsumed, out bytesWritten);
        }
    }
}
