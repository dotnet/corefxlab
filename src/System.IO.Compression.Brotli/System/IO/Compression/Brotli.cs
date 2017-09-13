﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Buffers;
using System.IO.Compression.Resources;

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
        private const int MaxQuality = 11;
        private const int MaxInputSize = 2147483132; // 2^32 - 1 - 515 (max compressed extra bytes)

        public struct State : IDisposable
        {
            internal IntPtr BrotliNativeState { get; private set; }
            internal BrotliDecoderResult LastDecoderResult;
            public bool CompressMode { get; private set; }

            public void Dispose()
            {
                if (BrotliNativeState == IntPtr.Zero) return;
                if (CompressMode)
                {
                    BrotliNative.BrotliEncoderDestroyInstance(BrotliNativeState);
                }
                else
                {
                    BrotliNative.BrotliDecoderDestroyInstance(BrotliNativeState);
                }
                BrotliNativeState = IntPtr.Zero;
            }

            internal void InitializeDecoder()
            {
                BrotliNativeState = BrotliNative.BrotliDecoderCreateInstance();
                LastDecoderResult = BrotliDecoderResult.NeedsMoreInput;
                if (BrotliNativeState == IntPtr.Zero)
                {
                    throw new System.Exception(BrotliEx.DecoderInstanceCreate);
                }
                CompressMode = false;
            }

            internal void InitializeEncoder()
            {
                BrotliNativeState = BrotliNative.BrotliEncoderCreateInstance();
                if (BrotliNativeState == IntPtr.Zero)
                {
                    throw new System.Exception(BrotliEx.EncoderInstanceCreate);
                }
                CompressMode = true;
                SetQuality((uint)GetQualityFromCompressionLevel(CompressionLevel.Optimal));
                SetWindow(MaxWindowBits);
            }

            public void SetQuality(uint quality)
            {
                if (BrotliNativeState == IntPtr.Zero)
                {
                    InitializeEncoder();
                }
                if (quality > MaxQuality)
                {
                    throw new ArgumentOutOfRangeException(BrotliEx.WrongQuality);
                }
                BrotliNative.BrotliEncoderSetParameter(BrotliNativeState, BrotliEncoderParameter.Quality, quality);
            }

            public void SetWindow(uint window)
            {
                if (BrotliNativeState == IntPtr.Zero)
                {
                    InitializeEncoder();
                }
                if (window - MinWindowBits > MaxWindowBits - MinWindowBits)
                {
                    throw new ArgumentOutOfRangeException(BrotliEx.WrongWindowSize);
                }
                BrotliNative.BrotliEncoderSetParameter(BrotliNativeState, BrotliEncoderParameter.LGWin, window);
            }
        }

        internal static void EnsureInitialized(ref State state, bool compress)
        {
            if (state.BrotliNativeState != IntPtr.Zero)
            {
                if (state.CompressMode != compress)
                {
                    throw new System.Exception((BrotliEx.InvalidModeChange));
                }
                return;
            }
            if (compress)
            {
                state.InitializeEncoder();
            }
            else
            {
                state.InitializeDecoder();
                state.LastDecoderResult = BrotliDecoderResult.NeedsMoreInput;
            }
        }

        public static int GetMaximumCompressedSize(int inputSize)
        {
            if (inputSize < 0 || inputSize > MaxInputSize)
            {
                throw new System.ArgumentOutOfRangeException("inputSize");
            }
            if (inputSize == 0) return 1;
            int numLargeBlocks = inputSize >> 24;
            int tail = inputSize & 0xFFFFFF;
            int tailOverhead = (tail > (1 << 20)) ? 4 : 3;
            int overhead = 2 + (4 * numLargeBlocks) + tailOverhead + 1;
            int result = inputSize + overhead;
            return result;
        }

        internal static int GetQualityFromCompressionLevel(CompressionLevel level)
        {
            if (level == CompressionLevel.Optimal) return 11;
            if (level == CompressionLevel.NoCompression) return 0;
            if (level == CompressionLevel.Fastest) return 1;
            return (int)level;
        }

        private static OperationStatus GetTransformationStatusFromBrotliDecoderResult(BrotliDecoderResult result)
        {
            if (result == BrotliDecoderResult.Success) return OperationStatus.Done;
            if (result == BrotliDecoderResult.NeedsMoreOutput) return OperationStatus.DestinationTooSmall;
            if (result == BrotliDecoderResult.NeedsMoreInput) return OperationStatus.NeedMoreData;
            return OperationStatus.InvalidData;
        }

        public static OperationStatus FlushEncoder(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state, bool isFinished = true)
        {
            EnsureInitialized(ref state, true);
            BrotliEncoderOperation operation = isFinished ? BrotliEncoderOperation.Finish : BrotliEncoderOperation.Flush;
            bytesWritten = destination.Length;
            bytesConsumed = 0;
            if (state.BrotliNativeState == IntPtr.Zero) return OperationStatus.InvalidData;
            if (BrotliNative.BrotliEncoderIsFinished(state.BrotliNativeState)) return OperationStatus.Done;
            unsafe
            {
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint availableOutput = (nuint)destination.Length;
                    nuint consumed = (nuint)source.Length;
                    if (!BrotliNative.BrotliEncoderCompressStream(state.BrotliNativeState, operation, ref consumed, ref bufIn, ref availableOutput, ref bufOut, out nuint totalOut))
                    {
                        return OperationStatus.InvalidData;
                    }
                    bytesConsumed = (int)consumed;
                    bytesWritten = (int)availableOutput;
                }
                bytesWritten = destination.Length - bytesWritten;
                if (bytesWritten > 0)
                {
                    if (BrotliNative.BrotliEncoderIsFinished(state.BrotliNativeState)) return OperationStatus.Done;
                    else return OperationStatus.DestinationTooSmall;
                }
            }
            return OperationStatus.Done;
        }

        public static OperationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state)
        {
            EnsureInitialized(ref state, true);
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
                        nuint availableOutput = (nuint)bytesWritten;
                        nuint consumed = (nuint)bytesConsumed;
                        if (!BrotliNative.BrotliEncoderCompressStream(state.BrotliNativeState, BrotliEncoderOperation.Process, ref consumed, ref bufIn, ref availableOutput, ref bufOut, out nuint totalOut))
                        {
                            return OperationStatus.InvalidData;
                        };
                        bytesConsumed = (int)consumed;
                        bytesWritten = destination.Length - (int)availableOutput;
                        if (availableOutput != (nuint)destination.Length)
                        {
                            return OperationStatus.DestinationTooSmall;
                        }
                    }
                }
                return OperationStatus.Done;
            }
        }

        public static OperationStatus Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state)
        {
            EnsureInitialized(ref state, false);
            bytesConsumed = source.Length;
            bytesWritten = destination.Length;
            if (BrotliNative.BrotliDecoderIsFinished(state.BrotliNativeState)) return OperationStatus.Done;
            unsafe
            {
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint availableOutput = (nuint)bytesWritten;
                    nuint consumed = (nuint)bytesConsumed;
                    state.LastDecoderResult = BrotliNative.BrotliDecoderDecompressStream(state.BrotliNativeState, ref consumed, ref bufIn, ref availableOutput, ref bufOut, out nuint totalOut);
                    bytesWritten = destination.Length - (int)availableOutput;
                    bytesConsumed = (int)consumed;
                }
                if (state.LastDecoderResult == BrotliDecoderResult.NeedsMoreInput)
                {
                    return OperationStatus.NeedMoreData;
                }
                else if (state.LastDecoderResult == BrotliDecoderResult.NeedsMoreOutput)
                {
                    return OperationStatus.DestinationTooSmall;
                }

                if (state.LastDecoderResult == BrotliDecoderResult.Error || !BrotliNative.BrotliDecoderIsFinished(state.BrotliNativeState))
                {
                    var error = BrotliNative.BrotliDecoderGetErrorCode(state.BrotliNativeState);
                    var text = BrotliNative.BrotliDecoderErrorString(error);
                    throw new System.IO.IOException(text + BrotliEx.unableDecode);
                }
                return GetTransformationStatusFromBrotliDecoderResult(state.LastDecoderResult);
            }

        }
    }
}
