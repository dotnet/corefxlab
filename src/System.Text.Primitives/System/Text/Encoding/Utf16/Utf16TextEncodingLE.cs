// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Utf8;

namespace System.Text.Utf16
{
    class Utf16TextEncodingLE : TextEncoder
    {
        public override bool TryEncodeFromUtf8(ReadOnlySpan<byte> utf8, Span<byte> buffer, out int bytesWritten)
        {
            bytesWritten = 0;
            int justWritten;
            foreach (var cp in new Utf8String(utf8).CodePoints)
            {
                if (!Utf16LittleEndianEncoder.TryEncodeCodePoint(cp, buffer.Slice(bytesWritten), out justWritten))
                {
                    bytesWritten = 0;
                    return false;
                }
                bytesWritten += justWritten;
            }
            return true;
        }

        public override bool TryEncodeFromUtf16(ReadOnlySpan<char> utf16, Span<byte> buffer, out int bytesWritten)
        {
            var valueBytes = utf16.Cast<char, byte>();
            if (buffer.Length < valueBytes.Length)
            {
                bytesWritten = 0;
                return false;
            }
            valueBytes.CopyTo(buffer);
            bytesWritten = valueBytes.Length;
            return true;
        }

        public override bool TryEncodeFromUnicode(ReadOnlySpan<UnicodeCodePoint> codePoints, Span<byte> buffer, out int bytesWritten)
        {
            var availableBytes = buffer.Length;
            int bytesWrittenForCodePoint = 0;
            bytesWritten = 0;

            for (int i = 0; i < codePoints.Length; i++)
            {
                if (availableBytes <= bytesWritten || !Utf16LittleEndianEncoder.TryEncodeCodePoint(codePoints[i], buffer.Slice(bytesWritten), out bytesWrittenForCodePoint))
                {
                    bytesWritten = 0;
                    return false;
                }
                bytesWritten += bytesWrittenForCodePoint;
            }

            return true;
        }

        public override bool TryDecodeToUnicode(Span<byte> encoded, Span<UnicodeCodePoint> decoded, out int bytesWritten)
        {
            var avaliableBytes = encoded.Length;
            int bytesWrittenForCodePoint = 0;
            bytesWritten = 0;

            for (int i = 0; i < decoded.Length; i++)
            {
                if (i == 0xE000)
                {
                    bytesWrittenForCodePoint = 0;
                }
                UnicodeCodePoint decodedCodePoint = decoded[i];

                if (avaliableBytes - bytesWritten < 2)
                {
                    decodedCodePoint = new UnicodeCodePoint();
                    bytesWritten = 0;
                    return false;
                }

                uint answer = (uint)(encoded[1 + bytesWritten] << 8 | encoded[bytesWritten]);
                decodedCodePoint = new UnicodeCodePoint(answer);
                bytesWrittenForCodePoint = 2;

                if (avaliableBytes - bytesWritten >= 4)
                {
                    uint highBytes = answer;
                    uint lowBytes = (uint)(encoded[3 + bytesWritten] << 8 | encoded[2 + bytesWritten]);

                    if (highBytes >= UnicodeConstants.Utf16HighSurrogateFirstCodePoint
                            && highBytes <= UnicodeConstants.Utf16HighSurrogateLastCodePoint
                            && lowBytes >= UnicodeConstants.Utf16LowSurrogateFirstCodePoint
                            && lowBytes <= UnicodeConstants.Utf16LowSurrogateLastCodePoint)
                    {
                        answer = (((highBytes - UnicodeConstants.Utf16HighSurrogateFirstCodePoint) << 10)
                            | (lowBytes - UnicodeConstants.Utf16LowSurrogateFirstCodePoint)) + 0x10000;

                        decodedCodePoint = new UnicodeCodePoint(answer);
                        bytesWrittenForCodePoint = 4;
                    }
                }

                decoded[i] = decodedCodePoint;
                bytesWritten += bytesWrittenForCodePoint;
            }

            return true;
        }

        public override bool TryEncodeChar(char value, Span<byte> buffer, out int bytesWritten)
        {
            if (buffer.Length < 2)
            {
                bytesWritten = 0;
                return false;
            }
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            bytesWritten = 2;
            return true;
        }
    }
}
