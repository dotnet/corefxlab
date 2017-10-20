// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Binary.Base64
{
    public static partial class Base64
    {
        public static readonly BufferEncoder BytesToUtf8Encoder = new ToBase64Utf8();

        private const int MaxLineLength = 76;

        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int consumed, out int written, bool isFinalBlock = true)
        {
            ref byte srcBytes = ref bytes.DangerousGetPinnableReference();
            ref byte destBytes = ref utf8.DangerousGetPinnableReference();

            int srcLength = bytes.Length;
            int destLength = utf8.Length;

            int sourceIndex = 0;
            int destIndex = 0;
            int result = 0;

            while (sourceIndex < srcLength - 2)
            {
                result = Encode(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 3;
            }

            // TODO: Implement logic for when isFinalBlock is false
            if (isFinalBlock != true) throw new NotImplementedException();

            if (sourceIndex == srcLength - 1)
            {
                result = EncodeAndPadTwo(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 1;
            }
            else if (sourceIndex == srcLength - 2)
            {
                result = EncodeAndPadOne(ref Unsafe.Add(ref srcBytes, sourceIndex));
                if (destIndex > destLength - 4) goto DestinationSmallExit;
                Unsafe.WriteUnaligned(ref Unsafe.Add(ref destBytes, destIndex), result);
                destIndex += 4;
                sourceIndex += 2;
            }

            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.Done;

            DestinationSmallExit:
            consumed = sourceIndex;
            written = destIndex;
            return OperationStatus.DestinationTooSmall;
        }

        private static ParsedFormat ValidateFormat(ParsedFormat format)
        {
            char symbol = format.Symbol;

            if (symbol == 'M')  // M (for MIME) == N76
            {
                symbol = 'N';
                return new ParsedFormat(symbol, MaxLineLength);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetMaxEncodedToUtf8Length(int length)
        {
            Debug.Assert(length >= 0);
            checked
            {
                return (((length + 2) / 3) * 4);
            }
        }

        public static int GetMaxEncodedToUtf8Length(int length, ParsedFormat format)
        {
            Debug.Assert(length >= 0);

            int defaultLength = GetMaxEncodedToUtf8Length(length);
            if (format.IsDefault) return defaultLength;

            format = ValidateFormat(format);

            int bytesInOneLine = (format.Precision >> 2) * 3;
            int extra = ((length - 1) / bytesInOneLine) * 2;
            checked
            {
                return defaultLength + extra;
            }
        }

        public static OperationStatus EncodeToUtf8(ReadOnlySpan<byte> bytes, Span<byte> utf8, out int consumed, out int written, ParsedFormat format, bool isFinalBlock = true)
        {
            if (format.IsDefault) return EncodeToUtf8(bytes, utf8, out consumed, out written, isFinalBlock);

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
                status = EncodeToUtf8(bytes.Slice(0, bytesInOneLine), utf8.Slice(0, lineLength), out int bytesConsumedLoop, out int bytesWrittenLoop);
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
                status = EncodeToUtf8(bytes, utf8, out int leftOverbytesConsumed, out int leftOverbytesWritten);
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

        public static OperationStatus EncodeToUtf8InPlace(Span<byte> buffer, int bytesLength, out int written)
        {
            var encodedLength = GetMaxEncodedToUtf8Length(bytesLength);
            if (buffer.Length < encodedLength) goto FalseExit;

            var leftover = bytesLength - bytesLength / 3 * 3; // how many bytes after packs of 3

            var destinationIndex = encodedLength - 4;
            var sourceIndex = bytesLength - leftover;

            // encode last pack to avoid conditional in the main loop
            if (leftover != 0)
            {
                var sourceSlice = buffer.Slice(sourceIndex, leftover);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                var result = EncodeToUtf8(sourceSlice, desitnationSlice, out _, out int tempWritten);
                Debug.Assert(result == OperationStatus.Done);
            }

            for (int index = sourceIndex - 3; index >= 0; index -= 3)
            {
                var sourceSlice = buffer.Slice(index, 3);
                var desitnationSlice = buffer.Slice(destinationIndex, 4);
                destinationIndex -= 4;
                var result = EncodeToUtf8(sourceSlice, desitnationSlice, out _, out int tempWritten);
                Debug.Assert(result == OperationStatus.Done);
            }

            written = encodedLength;
            return OperationStatus.Done;

            FalseExit:
            written = 0;
            return OperationStatus.DestinationTooSmall;
        }

        sealed class ToBase64Utf8 : BufferEncoder, IBufferTransformation
        {
            public override OperationStatus Encode(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => EncodeToUtf8(source, destination, out bytesConsumed, out bytesWritten);

            public override OperationStatus Transform(Span<byte> buffer, int dataLength, out int written)
                => EncodeToUtf8InPlace(buffer, dataLength, out written);

            public override bool IsEncodeInPlaceSupported => true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Encode(byte b0, byte b1, byte b2, out byte r0, out byte r1, out byte r2, out byte r3)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];

            int i2 = (b1 & 0xF) << 2 | (b2 >> 6);
            r2 = s_encodingMap[i2];

            int i3 = b2 & 0x3F;
            r3 = s_encodingMap[i3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Encode(byte b0, byte b1, byte b2, out byte r0, out byte r1, out byte r2)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];

            int i2 = (b1 & 0xF) << 2 | (b2 >> 6);
            r2 = s_encodingMap[i2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Encode(byte b0, byte b1, out byte r0, out byte r1)
        {
            int i0 = b0 >> 2;
            r0 = s_encodingMap[i0];

            int i1 = (b0 & 0x3) << 4 | (b1 >> 4);
            r1 = s_encodingMap[i1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Encode(ref byte threeBytes)
        {
            byte b0 = threeBytes;
            byte b1 = Unsafe.Add(ref threeBytes, 1);
            byte b2 = Unsafe.Add(ref threeBytes, 2);

            Encode(b0, b1, b2, out byte r0, out byte r1, out byte r2, out byte r3);

            int result = r3 << 24 | r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EncodeAndPadOne(ref byte twoBytes)
        {
            Encode(twoBytes, Unsafe.Add(ref twoBytes, 1), 0, out byte r0, out byte r1, out byte r2);
            int result = s_encodingPad << 24 | r2 << 16 | r1 << 8 | r0;
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int EncodeAndPadTwo(ref byte oneByte)
        {
            Encode(oneByte, 0, out byte r0, out byte r1);
            int result = s_encodingPad << 24 | s_encodingPad << 16 | r1 << 8 | r0;
            return result;
        }

        // Pre-computing this table using a custom string(s_characters) and GenerateEncodingMapAndVerify (found in tests)
        static readonly byte[] s_encodingMap = {
            65, 66, 67, 68, 69, 70, 71, 72,         //A..H
            73, 74, 75, 76, 77, 78, 79, 80,         //I..P
            81, 82, 83, 84, 85, 86, 87, 88,         //Q..X
            89, 90, 97, 98, 99, 100, 101, 102,      //Y..Z, a..f
            103, 104, 105, 106, 107, 108, 109, 110, //g..n
            111, 112, 113, 114, 115, 116, 117, 118, //o..v
            119, 120, 121, 122, 48, 49, 50, 51,     //w..z, 0..3
            52, 53, 54, 55, 56, 57, 43, 47          //4..9, +, /
        };

        const byte s_encodingPad = (byte)'='; // '=', for padding
    }
}
