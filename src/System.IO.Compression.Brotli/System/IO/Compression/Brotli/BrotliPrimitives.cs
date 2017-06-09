using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Compression.Brotli.Resources;
using System.Runtime.CompilerServices;
using System.Text;

#if BIT64
    using nuint = System.UInt64;
#else
    using nuint = System.UInt32;
#endif 

namespace System.IO.Compression
{
    public struct BrotliPrimitives
    {
        const int defQuality = 11;
        const int defwindowSize = 24;

        public static int GetMaximumCompressedSize(int inputSize)
        {
            if (inputSize == 0) return 1;
            int numLargeBlocks = inputSize >> 24;
            int tail = inputSize & 0xFFFFFF;
            int tailOverhead = (tail > (1 << 20)) ? 4 : 3;
            int overhead = 2 + (4 * numLargeBlocks) + tailOverhead + 1;
            int result = inputSize + overhead;
            return (result < inputSize) ? inputSize : result;
        }

        private static TransformationStatus GetTransformationStatusFromBrotliDecoderResult(BrotliDecoderResult result)
        {
            if (result == BrotliDecoderResult.Success) return TransformationStatus.Done;
            if (result == BrotliDecoderResult.NeedsMoreOutput) return TransformationStatus.DestinationTooSmall;
            if (result == BrotliDecoderResult.NeedsMoreInput) return TransformationStatus.NeedMoreSourceData;
            return TransformationStatus.Error;
        }

        public static TransformationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, int quality = defQuality, int windowSize = defwindowSize, BrotliEncoderMode encMode = BrotliEncoderMode.Generic)
        {
            if (quality > defQuality || quality <= 0) throw new System.ArgumentOutOfRangeException(BrotliEx.WrongQuality);
            if (windowSize > defwindowSize || windowSize <= 0) throw new System.ArgumentOutOfRangeException(BrotliEx.WrongWindowSize);
            bytesConsumed = bytesWritten = 0;
            unsafe
            {
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint written = (nuint)destination.Length;
                    nuint consumed = (nuint)source.Length;
                    if (!BrotliNative.BrotliEncoderCompress(quality, windowSize, encMode, consumed, bufIn, ref written, bufOut))
                    {
                        return TransformationStatus.InvalidData;
                    };
                    bytesConsumed = (int)consumed;
                    bytesWritten = (int)written;
                    return TransformationStatus.Done;
                }
            }
        }

        public static TransformationStatus Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            bytesConsumed = bytesWritten = 0;
            unsafe
            {
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint written = (nuint)destination.Length;
                    nuint consumed = (nuint)source.Length;
                    BrotliDecoderResult res = BrotliNative.BrotliDecoderDecompress(ref consumed, bufIn, ref written, bufOut);
                    if (res == BrotliDecoderResult.Success)
                    {
                        bytesWritten = (int)written;
                        bytesConsumed = (int)consumed;
                    }
                    return GetTransformationStatusFromBrotliDecoderResult(res);
                }
            }
        }
    }
}
