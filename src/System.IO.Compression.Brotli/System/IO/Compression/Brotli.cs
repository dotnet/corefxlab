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
            internal IntPtr BrotliNativeState { get; set; }
            internal int AvailableOut;
            internal byte[] NextOut;

            internal Stream BufferStream => _bufferStream;
            private MemoryStream _bufferStream;
           
            public void Dispose() {
                
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
            BrotliEncoderOperation op = is_finished ? BrotliEncoderOperation.Finish : BrotliEncoderOperation.Flush;
            bytesConsumed = source.Length;
            bytesWritten = 0;
            if (state.BrotliNativeState == IntPtr.Zero) return TransformationStatus.InvalidData;
            if (BrotliNative.BrotliEncoderIsFinished(state.BrotliNativeState)) return TransformationStatus.InvalidData;
            unsafe {
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &state.NextOut.AsSpan().DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint written = (nuint)destination.Length;
                    nuint consumed = (nuint)source.Length;
                    if (!BrotliNative.BrotliEncoderCompressStream(state.BrotliNativeState, op, ref consumed, ref bufIn, ref written, ref bufOut, out nuint totalOut));
                    bytesConsumed = (int)consumed;
                    state.AvailableOut = (int)written;
                    Console.WriteLine("Written: "+written.ToString());
                }
                var extraData = state.AvailableOut != destination.Length;
                if (extraData)
                {
                    var bytesWrote = (int)(destination.Length - state.AvailableOut);
                    bytesWritten = bytesWrote;
                    state.AvailableOut = destination.Length;
                    state.NextOut = destination.ToArray();
                    return TransformationStatus.DestinationTooSmall;
                }
            }
            return TransformationStatus.Done;
        }

        public static TransformationStatus Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, ref State state)
        {
            bytesConsumed = source.Length;
            bytesWritten = destination.Length;
            unsafe
            {
                IntPtr bufIn, bufOut;
                while (bytesConsumed > 0)
                {

                    fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                    fixed (byte* outBytes = &state.NextOut.AsSpan().DangerousGetPinnableReference())
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
                        Console.WriteLine("Consumed" + bytesConsumed);
                        Console.WriteLine("TotalOut" + totalOut);
                        state.AvailableOut = (int)written;
                        if (state.AvailableOut != destination.Length)
                        {
                            var bytesWrote = (int)(destination.Length - bytesWritten);
                            state.AvailableOut = destination.Length;
                            state.NextOut = destination.ToArray();
                            return TransformationStatus.DestinationTooSmall;
                        }
                    }
                }
                if (BrotliNative.BrotliEncoderIsFinished(state.BrotliNativeState))
                {
                    Console.WriteLine("Finished at Compress");
                    return TransformationStatus.Done;
                }
                return TransformationStatus.Done;
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
