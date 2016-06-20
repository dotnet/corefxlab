// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Utf8;
using System.Text;

namespace System.Text.Formatting
{
    // The data for culture-aware formatting is stored in this assembly resources
    // The data were produced from Unicode Locale Data Repository: http://cldr.unicode.org
    // using a custom tool that I will try to clean, debug, and commit to this repo.
    // For now, the data might contain errors.
    public class FormattingDataProvider
    {
        public static FormattingData CreateFormattingData(string localeId)
        {
            var resourceName = "System.Text.Formatting.locales.bin";
            var resourceStream = typeof(FormattingDataProvider).GetTypeInfo().Assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new Exception("resource missing");
            }

            using (resourceStream)
            {
                return CreateFormattingData(localeId, resourceStream);
            }
        }

        private static FormattingData CreateFormattingData(string localeId, Stream resourceStream)
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
            int idByteCount;
            if (!localeId.TryFormat(new Span<byte>(idBytes), default(Format.Parsed), FormattingData.InvariantUtf8, out idByteCount))
            {
                throw new Exception("bad locale id");
            }
            var id = new Utf8String(idBytes.Slice(0, idByteCount));

            int recordStart = -1;
            for (int record = 0; record < numberOfIDs; record++)
            {
                var indexId = index.Slice(record * recordSize, idByteCount);
                if (id.Equals(new Utf8String(indexId))) // found record
                {
                    var indexData = index.Slice(record * recordSize + maxIdLength);
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

            return new FormattingData(utf16digitsAndSymbols, FormattingData.Encoding.Utf16);
        }

        static ushort ReadUInt16At(byte[] data, int ushortIndex)
        {
            ushortIndex *= 2;
            ushort value = (ushort)(data[ushortIndex + 1] * 256 + data[ushortIndex + 0]);
            return value;

        }
    }
}