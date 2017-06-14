// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Runtime.CompilerServices;

namespace System.Text
{
    public static partial class Encoders
    {
        public static class Utf32
        {
            #region Tranforms

            public static readonly ITransformation FromUtf8 = new Utf8ToUtf32Transform();
            public static readonly ITransformation FromUtf16 = new Utf16ToUtf32Transform();

            private sealed class Utf8ToUtf32Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf8(source, destination, out bytesConsumed, out bytesWritten);
            }

            private sealed class Utf16ToUtf32Transform : ITransformation
            {
                public TransformationStatus Transform(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
                    => ConvertFromUtf16(source, destination, out bytesConsumed, out bytesWritten);
            }

            #endregion Transforms

            #region Utf-8 to Utf-32 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf8(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                bytesNeeded = 0;

                int index = 0;
                int length = source.Length;
                ref byte src = ref source.DangerousGetPinnableReference();

                while (index < length)
                {
                    int count = EncodingHelper.GetUtf8DecodedBytes(Unsafe.Add(ref src, index));
                    if (count == 0)
                        goto InvalidData;
                    if (length - index >= count)
                        goto NeedMoreData;

                    bytesNeeded += count;
                }

                return index < length ? TransformationStatus.DestinationTooSmall : TransformationStatus.Done;

            InvalidData:
                return TransformationStatus.InvalidData;

            NeedMoreData:
                return TransformationStatus.NeedMoreSourceData;
            }

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
            /// <returns>A <see cref="TransformationStatus"/> value representing the state of the conversion.</returns>
            public static TransformationStatus ConvertFromUtf8(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                bytesConsumed = 0;
                bytesWritten = 0;

                int srcLength = source.Length;
                int dstLength = destination.Length;
                ref byte src = ref source.DangerousGetPinnableReference();
                ref byte dst = ref destination.DangerousGetPinnableReference();

                while (bytesConsumed < srcLength && bytesWritten < dstLength)
                {
                    uint codePoint = Unsafe.Add(ref src, bytesConsumed);

                    int byteCount = EncodingHelper.GetUtf8DecodedBytes((byte)codePoint);
                    if (byteCount == 0)
                        goto InvalidData;
                    if (srcLength - bytesConsumed >= byteCount)
                        goto NeedMoreData;

                    if (byteCount > 1)
                        codePoint &= (byte)(0x7F >> byteCount);

                    for (var i = 1; i < byteCount; i++)
                    {
                        ref byte next = ref Unsafe.Add(ref src, bytesConsumed + i);
                        if ((next & EncodingHelper.b1100_0000U) != EncodingHelper.b1000_0000U)
                            goto InvalidData;

                        codePoint = (codePoint << 6) | (uint)(EncodingHelper.b0011_1111U & next);
                    }

                    Unsafe.As<byte, uint>(ref Unsafe.Add(ref dst, bytesWritten)) = codePoint;
                    bytesWritten += 4;
                    bytesConsumed += byteCount;
                }

                return bytesConsumed < srcLength ? TransformationStatus.DestinationTooSmall : TransformationStatus.Done;

            InvalidData:
                return TransformationStatus.InvalidData;

            NeedMoreData:
                return TransformationStatus.NeedMoreSourceData;
            }

            #endregion Utf-8 to Utf-32 conversion

            #region Utf-16 to Utf-32 conversion

            public static TransformationStatus ComputeEncodedBytesFromUtf16(ReadOnlySpan<byte> source, out int bytesNeeded)
            {
                bytesNeeded = 0;

                ref byte src = ref source.DangerousGetPinnableReference();
                int srcLength = source.Length;
                int srcIndex = 0;

                while (srcLength - srcIndex >= sizeof(char))
                {
                    uint codePoint = Unsafe.As<byte, char>(ref Unsafe.Add(ref src, srcIndex));
                    if (EncodingHelper.IsSurrogate(codePoint))
                    {
                        if (!EncodingHelper.IsHighSurrogate(codePoint))
                            return TransformationStatus.InvalidData;

                        if (srcLength - srcIndex < sizeof(char) * 2)
                            return TransformationStatus.NeedMoreSourceData;

                        uint lowSurrogate = Unsafe.As<byte, char>(ref Unsafe.Add(ref src, srcIndex + 2));
                        if (!EncodingHelper.IsLowSurrogate(lowSurrogate))
                            return TransformationStatus.InvalidData;

                        srcIndex += 2;
                    }

                    srcIndex += 2;
                    bytesNeeded += 4;
                }

                return srcIndex < srcLength ? TransformationStatus.NeedMoreSourceData : TransformationStatus.Done;
            }

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
            /// <returns>A <see cref="TransformationStatus"/> value representing the state of the conversion.</returns>
            public static TransformationStatus ConvertFromUtf16(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
            {
                bytesConsumed = 0;
                bytesWritten = 0;

                ref byte src = ref source.DangerousGetPinnableReference();
                int srcLength = source.Length;

                ref byte dst = ref destination.DangerousGetPinnableReference();
                int dstLength = destination.Length;

                while (srcLength - bytesConsumed >= sizeof(char))
                {
                    if (dstLength - bytesWritten < sizeof(uint))
                        return TransformationStatus.DestinationTooSmall;

                    uint codePoint = Unsafe.As<byte, char>(ref Unsafe.Add(ref src, bytesConsumed));
                    if (EncodingHelper.IsSurrogate(codePoint))
                    {
                        if (!EncodingHelper.IsHighSurrogate(codePoint))
                            return TransformationStatus.InvalidData;

                        if (srcLength - bytesConsumed < sizeof(char) * 2)
                            return TransformationStatus.NeedMoreSourceData;

                        uint lowSurrogate = Unsafe.As<byte, char>(ref Unsafe.Add(ref src, bytesConsumed + 2));
                        if (!EncodingHelper.IsLowSurrogate(lowSurrogate))
                            return TransformationStatus.InvalidData;

                        codePoint -= EncodingHelper.HighSurrogateStart;
                        lowSurrogate -= EncodingHelper.LowSurrogateStart;
                        codePoint = ((codePoint << 10) | lowSurrogate) + 0x010000u;
                        bytesConsumed += 2;
                    }

                    Unsafe.As<byte, uint>(ref Unsafe.Add(ref dst, bytesWritten)) = codePoint;
                    bytesConsumed += 2;
                    bytesWritten += 4;
                }

                return bytesConsumed < srcLength ? TransformationStatus.NeedMoreSourceData : TransformationStatus.Done;
            }

            #endregion Utf-16 to Utf-32 conversion
        }
    }
}
