// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Operations;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Binary.Base64Experimental
{
    public static partial class Base64Experimental
    {
        public static readonly Utf8Encoder BytesToUtf8Encoder = new Utf8Encoder();

        private const int MaxLineLength = 76;

        private static StandardFormat ValidateFormat(StandardFormat format)
        {
            char symbol = format.Symbol;

            if (symbol == 'M')  // M (for MIME) == N76
            {
                symbol = 'N';
                return new StandardFormat(symbol, MaxLineLength);
            }

            if (symbol == 'N')
            {
                if (format.Precision > MaxLineLength || format.Precision % 4 != 0 || format.Precision == 0)
                {
                    throw new FormatException($"Format {format.Symbol}:{format.Precision} not supported for Base64 Encoding.");
                }
                return format;
            }

            throw new FormatException($"Format {format.Symbol}:{format.Precision} not supported for Base64 Encoding.");
        }

        public static int GetMaxEncodedToUtf8Length(int length, StandardFormat format)
        {
            Debug.Assert(length >= 0);

            int defaultLength = Base64.GetMaxEncodedToUtf8Length(length);
            if (format.IsDefault) return defaultLength;

            format = ValidateFormat(format);

            int bytesInOneLine = (format.Precision >> 2) * 3;
            int extra = ((length - 1) / bytesInOneLine) * 2;
            checked
            {
                return defaultLength + extra;
            }
        }

        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int consumed, out int written, StandardFormat format, bool isFinalBlock = true)
        {
            if (format.IsDefault) return Base64.EncodeToUtf8(bytes, utf8, out consumed, out written, isFinalBlock);

            format = ValidateFormat(format);

            int inputLength = bytes.Length;
            int lineLength = format.Precision;
            int bytesInOneLine = (format.Precision >> 2) * 3;
            consumed = 0;
            written = 0;
            OperationStatus status = OperationStatus.Done;

            int numLineBreaks = utf8.Length / (lineLength + 2);
            if ((utf8.Length % (lineLength + 2)) == (lineLength + 1))
            {
                numLineBreaks++;
            }

            for (int i = 0; i < numLineBreaks; i++)
            {
                status = Base64.EncodeToUtf8(bytes.Slice(0, bytesInOneLine), utf8.Slice(0, lineLength), out int bytesConsumedLoop, out int bytesWrittenLoop);
                utf8[lineLength] = (byte)'\r';
                utf8[lineLength + 1] = (byte)'\n';
                bytesWrittenLoop += 2;
                bytes = bytes.Slice(bytesConsumedLoop);
                utf8 = utf8.Slice(bytesWrittenLoop);
                consumed += bytesConsumedLoop;
                written += bytesWrittenLoop;
            }

            if (isFinalBlock)
            {
                status = Base64.EncodeToUtf8(bytes, utf8, out int leftOverbytesConsumed, out int leftOverbytesWritten);
                consumed += leftOverbytesConsumed;
                written += leftOverbytesWritten;
                return status;
            }

            return (bytes.Length > bytesInOneLine) ? OperationStatus.DestinationTooSmall : OperationStatus.NeedMoreData;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ShiftByN(Span<byte> utf8, int num)
        {
            utf8.Slice(0, utf8.Length - num).CopyTo(utf8.Slice(num));
        }

        public sealed class Utf8Encoder : IBufferOperation, IBufferTransformation
        {
            public OperationStatus Encode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Base64.EncodeToUtf8(source, destination, out bytesConsumed, out bytesWritten);

            public OperationStatus EncodeInPlace(Span<byte> buffer, int dataLength, out int written)
                => Base64.EncodeToUtf8InPlace(buffer, dataLength, out written);

            OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written)
                => Encode(input, output, out consumed, out written);

            OperationStatus IBufferTransformation.Transform(Span<byte> buffer, int dataLength, out int written)
                => EncodeInPlace(buffer, dataLength, out written);
        }
    }
}
