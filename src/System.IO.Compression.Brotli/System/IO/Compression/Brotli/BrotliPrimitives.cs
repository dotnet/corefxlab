using System;
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
        const int defLgWin = 24;

        public static int GetMaximumCompressedSize(int input_size)
        {
            if (input_size == 0) return 1;
            int num_large_blocks = input_size >> 24;
            int tail = input_size & 0xFFFFFF;
            int tail_overhead = (tail > (1 << 20)) ? 4 : 3;
            int overhead = 2 + (4 * num_large_blocks) + tail_overhead + 1;
            int result = input_size + overhead;
            return (result < input_size) ? input_size : result;
        }

        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, int quality=defQuality, int lgwin=defLgWin, BrotliEncoderMode encMode=BrotliEncoderMode.Generic)
        {
            if (quality > defQuality || quality <= 0) throw new System.ArgumentOutOfRangeException(BrotliEx.WrongQuality);
            if (lgwin > defLgWin || lgwin <= 0) throw new System.ArgumentOutOfRangeException(BrotliEx.WrongWindowSize);
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
                    if (!BrotliNative.BrotliEncoderCompress(quality, lgwin, encMode, consumed, bufIn, ref written, bufOut))
                    {
                        return false;
                    };
                    bytesConsumed = (int)consumed;
                    bytesWritten = (int)written;
                    return true;
                }
            }
        }

        public static BrotliDecoderResult Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
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
                    if (res != BrotliDecoderResult.Success)
                    {
                        return res;
                    }
                    bytesWritten = (int)written;
                    bytesConsumed = (int)consumed;
                    return res;
                }
            }
        }
    }
}
