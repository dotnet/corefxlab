using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Compression.Resources;
using System.Runtime.CompilerServices;
using System.Text;

#if BIT64
    using nuint = System.UInt64;
#else
    using nuint = System.UInt32;
#endif 

namespace System.IO.Compression
{
    public static class Brotli
    {
        private const int MinWindowBits = 10;
        private const int MaxWindowBits = 24;
        private const int MinQuality = 0;
        private const int MaxQuality = 11;

        public struct State : IDisposable
        {
            internal IntPtr BrotliNativeState { get; private set; }

            public void Dispose()
            {
                //    BrotliNative.BrotliDecoderDestroyInstance(BrotliNativeState);
                //   BrotliNative.BrotliEncoderDestroyInstance(BrotliNativeState);
            }

            public void InitializeDecoder()
            {
                BrotliNativeState = BrotliNative.BrotliDecoderCreateInstance();
                if (BrotliNativeState == IntPtr.Zero)
                {
                    throw new System.IO.IOException(BrotliEx.DecoderInstanceCreate);
                }
            }

            public void InitializeEncoder()
            {
                BrotliNativeState = BrotliNative.BrotliEncoderCreateInstance();
                if (BrotliNativeState == IntPtr.Zero)
                {
                    throw new System.IO.IOException(BrotliEx.EncoderInstanceCreate);
                }
            }

            public void SetQuality(uint quality)
            {
                if (quality < MinQuality || quality > MaxQuality)
                {
                    throw new ArgumentException(BrotliEx.WrongQuality);
                }
                BrotliNative.BrotliEncoderSetParameter(BrotliNativeState, BrotliEncoderParameter.Quality, quality);
            }

            public void SetQuality()
            {
                SetQuality(MaxQuality);
            }

            public void SetWindow(uint window)
            {
                if (window < MinWindowBits || window > MaxWindowBits)
                {
                    throw new ArgumentException(BrotliEx.WrongWindowSize);
                }
                BrotliNative.BrotliEncoderSetParameter(BrotliNativeState, BrotliEncoderParameter.LGWin, window);
            }

            public void SetWindow()
            {
                SetWindow(MaxWindowBits);
            }
        }

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

        internal static int GetQualityFromCompressionLevel(CompressionLevel level)
        {
            if (level == CompressionLevel.Fastest) return 1;
            if (level == CompressionLevel.Optimal) return 10;
            return (int)level;
        }

        private static TransformationStatus GetTransformationStatusFromBrotliDecoderResult(BrotliDecoderResult result)
        {
            if (result == BrotliDecoderResult.Success) return TransformationStatus.Done;
            if (result == BrotliDecoderResult.NeedsMoreOutput) return TransformationStatus.DestinationTooSmall;
            if (result == BrotliDecoderResult.NeedsMoreInput) return TransformationStatus.NeedMoreSourceData;
            return TransformationStatus.InvalidData;
        }

        public static TransformationStatus FlushEncoder(Span<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state, bool is_finished = true)
        {
            BrotliEncoderOperation operation = is_finished ? BrotliEncoderOperation.Finish : BrotliEncoderOperation.Flush;
            bytesConsumed = source.Length;
            bytesWritten = destination.Length;
            if (state.BrotliNativeState == IntPtr.Zero) return TransformationStatus.InvalidData;
            if (BrotliNative.BrotliEncoderIsFinished(state.BrotliNativeState)) return TransformationStatus.InvalidData;
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
                    if (!BrotliNative.BrotliEncoderCompressStream(state.BrotliNativeState, operation, ref consumed, ref bufIn, ref written, ref bufOut, out nuint totalOut))
                    {
                        return TransformationStatus.InvalidData;
                    }
                    bytesConsumed = (int)consumed;
                    bytesWritten = (int)written;
                }
                bytesWritten = destination.Length - bytesWritten;
                if (bytesWritten > 0)
                {
                    return TransformationStatus.DestinationTooSmall;
                }
            }
            return TransformationStatus.Done;
        }

        public static TransformationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state)
        {
            bytesWritten = destination.Length;
            bytesConsumed = source.Length;
            unsafe
            {
                IntPtr bufIn, bufOut;
                while (bytesConsumed > 0)
                {
                    fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                    fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                    {
                        bufIn = new IntPtr(inBytes);
                        bufOut = new IntPtr(outBytes);
                        nuint written = (nuint)bytesWritten;
                        nuint consumed = (nuint)bytesConsumed;
                        if (!BrotliNative.BrotliEncoderCompressStream(state.BrotliNativeState, BrotliEncoderOperation.Process, ref consumed, ref bufIn, ref written, ref bufOut, out nuint totalOut))
                        {
                            return TransformationStatus.InvalidData;
                        };
                        bytesConsumed = (int)consumed;
                        bytesWritten = (int)((nuint)destination.Length - written);
                        if (written != (nuint)destination.Length)
                        {
                            return TransformationStatus.DestinationTooSmall;
                        }
                    }
                }
                return TransformationStatus.Done;
            }
        }

        public static TransformationStatus Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state)
        {
            bool endOfStream = false;
            bool errorDetected = false;
            bytesConsumed = source.Length;
            bytesWritten = destination.Length;
            BrotliDecoderResult LastDecoderResult = BrotliDecoderResult.NeedsMoreInput;
            unsafe
            {
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint written = (nuint)bytesWritten;
                    nuint consumed = (nuint)bytesConsumed;
                    LastDecoderResult = BrotliNative.BrotliDecoderDecompressStream(state.BrotliNativeState, ref consumed, ref bufIn, ref written, ref bufOut, out nuint totalOut);
                    bytesWritten = (int)((nuint)destination.Length - written);
                    bytesConsumed = (int)consumed;
                }
                if (LastDecoderResult == BrotliDecoderResult.NeedsMoreInput)
                {
                    return TransformationStatus.NeedMoreSourceData;
                }
                else if (LastDecoderResult == BrotliDecoderResult.NeedsMoreOutput)
                {
                    return TransformationStatus.DestinationTooSmall;
                }
                else
                {
                    endOfStream = true;
                }

                if (endOfStream && !BrotliNative.BrotliDecoderIsFinished(state.BrotliNativeState))
                {
                    errorDetected = true;
                }
                if (LastDecoderResult == BrotliDecoderResult.Error || errorDetected)
                {
                    var error = BrotliNative.BrotliDecoderGetErrorCode(state.BrotliNativeState);
                    var text = BrotliNative.BrotliDecoderErrorString(error);
                    throw new System.IO.IOException(text + BrotliEx.unableDecode);
                }
                if (endOfStream && !BrotliNative.BrotliDecoderIsFinished(state.BrotliNativeState) && LastDecoderResult == BrotliDecoderResult.NeedsMoreInput)
                {
                    throw new System.IO.IOException(BrotliEx.FinishDecompress);
                }
                return GetTransformationStatusFromBrotliDecoderResult(LastDecoderResult);
            }

        }
    }
}
