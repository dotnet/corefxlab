﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Buffers.Text
{
    public static partial class TextEncodings
    {
        public static class Utf32
        {
            #region UTF-8 Conversions

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-32 bytes from the specified UTF-8 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-8 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus FromUtf8Length(ReadOnlySpan<byte> source, out int bytesNeeded)
                => Utf8.ToUtf32Length(source, out bytesNeeded);

            /// <summary>
            /// Converts a span containing a sequence of UTF-8 bytes into UTF-32 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-8 bytes.</param>
            /// <param name="destination">A span to write the UTF-32 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus FromUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Utf8.ToUtf32(source, destination, out bytesConsumed, out bytesWritten);

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-8 bytes from the specified UTF-32 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-32 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus ToUtf8Length(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                bytesNeeded = 0;

                ref uint utf32 = ref Unsafe.As<byte, uint>(ref MemoryMarshal.GetReference(source));
                int utf32Length = source.Length >> 2; // byte => uint count

                for (int i = 0; i < utf32Length; i++)
                {
                    uint codePoint = Unsafe.Add(ref utf32, i);
                    if (!EncodingHelper.IsValidScalarValue(codePoint))
                        return OperationStatus.InvalidData;

                    bytesNeeded += EncodingHelper.GetUtf8EncodedBytes(codePoint);
                }

                if (utf32Length << 2 != source.Length)
                    return OperationStatus.NeedMoreData;

                return OperationStatus.Done;
            }

            /// <summary>
            /// Converts a span containing a sequence of UTF-32 bytes into UTF-8 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-32 bytes.</param>
            /// <param name="destination">A span to write the UTF-8 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus ToUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                bytesConsumed = 0;
                bytesWritten = 0;

                ref byte src = ref MemoryMarshal.GetReference(source);
                int srcLength = source.Length;

                ref byte dst = ref MemoryMarshal.GetReference(destination);
                int dstLength = destination.Length;

                while (srcLength - bytesConsumed >= sizeof(uint))
                {
                    uint codePoint = Unsafe.As<byte, uint>(ref Unsafe.Add(ref src, bytesConsumed));
                    if (!EncodingHelper.IsValidScalarValue(codePoint))
                        return OperationStatus.InvalidData;

                    int bytesNeeded = EncodingHelper.GetUtf8EncodedBytes(codePoint);
                    if (dstLength - bytesWritten < bytesNeeded)
                        return OperationStatus.DestinationTooSmall;

                    switch (bytesNeeded)
                    {
                        case 1:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(EncodingHelper.b0111_1111U & codePoint);
                            break;

                        case 2:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(((codePoint >> 6) & EncodingHelper.b0001_1111U) | EncodingHelper.b1100_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 1) = (byte)((codePoint & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            break;

                        case 3:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(((codePoint >> 12) & EncodingHelper.b0000_1111U) | EncodingHelper.b1110_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 1) = (byte)(((codePoint >> 6) & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 2) = (byte)((codePoint & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            break;

                        case 4:
                            Unsafe.Add(ref dst, bytesWritten) = (byte)(((codePoint >> 18) & EncodingHelper.b0000_0111U) | EncodingHelper.b1111_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 1) = (byte)(((codePoint >> 12) & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 2) = (byte)(((codePoint >> 6) & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            Unsafe.Add(ref dst, bytesWritten + 3) = (byte)((codePoint & EncodingHelper.b0011_1111U) | EncodingHelper.b1000_0000U);
                            break;

                        default:
                            return OperationStatus.InvalidData;
                    }

                    bytesConsumed += 4;
                    bytesWritten += bytesNeeded;
                }

                return bytesConsumed < srcLength ? OperationStatus.NeedMoreData : OperationStatus.Done;
            }

            #endregion UTF-8 Conversions

            #region UTF-16 Conversions

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-32 bytes from the specified UTF-16 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-16 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus FromUtf16Length(ReadOnlySpan<byte> source, out int bytesNeeded)
                => Utf16.ToUtf32Length(source, out bytesNeeded);

            /// <summary>
            /// Converts a span containing a sequence of UTF-16 bytes into UTF-32 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-16 bytes.</param>
            /// <param name="destination">A span to write the UTF-32 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus FromUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                => Utf16.ToUtf32(source, destination, out bytesConsumed, out bytesWritten);

            /// <summary>
            /// Calculates the byte count needed to encode the UTF-16 bytes from the specified UTF-32 sequence.
            ///
            /// This method will consume as many of the input bytes as possible.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-32 bytes.</param>
            /// <param name="bytesNeeded">On exit, contains the number of bytes required for encoding from the <paramref name="source"/>.</param>
            /// <returns>A <see cref="OperationStatus"/> value representing the expected state of the conversion.</returns>
            public static OperationStatus ToUtf16Length(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                int index = 0;
                int length = source.Length;
                ref byte src = ref MemoryMarshal.GetReference(source);

                bytesNeeded = 0;

                while (length - index >= 4)
                {
                    ref uint codePoint = ref Unsafe.As<byte, uint>(ref Unsafe.Add(ref src, index));

                    if (!EncodingHelper.IsValidScalarValue(codePoint))
                        return OperationStatus.InvalidData;

                    bytesNeeded += EncodingHelper.IsBmp(codePoint) ? 2 : 4;
                    index += 4;
                }

                return index < length ? OperationStatus.NeedMoreData : OperationStatus.Done;
            }

            /// <summary>
            /// Converts a span containing a sequence of UTF-32 bytes into UTF-16 bytes.
            ///
            /// This method will consume as many of the input bytes as possible.
            ///
            /// On successful exit, the entire input was consumed and encoded successfully. In this case, <paramref name="bytesConsumed"/> will be
            /// equal to the length of the <paramref name="source"/> and <paramref name="bytesWritten"/> will equal the total number of bytes written to
            /// the <paramref name="destination"/>.
            /// </summary>
            /// <param name="source">A span containing a sequence of UTF-32 bytes.</param>
            /// <param name="destination">A span to write the UTF-16 bytes into.</param>
            /// <param name="bytesConsumed">On exit, contains the number of bytes that were consumed from the <paramref name="source"/>.</param>
            /// <param name="bytesWritten">On exit, contains the number of bytes written to <paramref name="destination"/></param>
            /// <returns>A <see cref="OperationStatus"/> value representing the state of the conversion.</returns>
            public static OperationStatus ToUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                ref byte src = ref MemoryMarshal.GetReference(source);
                ref byte dst = ref MemoryMarshal.GetReference(destination);
                int srcLength = source.Length;
                int dstLength = destination.Length;

                bytesConsumed = 0;
                bytesWritten = 0;

                while (srcLength - bytesConsumed >= sizeof(uint))
                {
                    ref uint codePoint = ref Unsafe.As<byte, uint>(ref Unsafe.Add(ref src, bytesConsumed));

                    if (!EncodingHelper.IsValidScalarValue(codePoint))
                        return OperationStatus.InvalidData;

                    int written = EncodingHelper.IsBmp(codePoint) ? 2 : 4;
                    if (dstLength - bytesWritten < written)
                        return OperationStatus.DestinationTooSmall;

                    unchecked
                    {
                        if (written == 2)
                            Unsafe.As<byte, char>(ref Unsafe.Add(ref dst, bytesWritten)) = (char)codePoint;
                        else
                        {
                            Unsafe.As<byte, char>(ref Unsafe.Add(ref dst, bytesWritten)) = (char)(((codePoint - 0x010000u) >> 10) + EncodingHelper.HighSurrogateStart);
                            Unsafe.As<byte, char>(ref Unsafe.Add(ref dst, bytesWritten + 2)) = (char)((codePoint & 0x3FF) + EncodingHelper.LowSurrogateStart);
                        }
                    }

                    bytesWritten += written;
                    bytesConsumed += 4;
                }

                return bytesConsumed < srcLength ? OperationStatus.NeedMoreData : OperationStatus.Done;
            }

            #endregion UTF-16 Conversions
        }
    }
}
