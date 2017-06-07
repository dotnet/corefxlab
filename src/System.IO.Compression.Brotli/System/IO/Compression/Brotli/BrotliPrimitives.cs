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

        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            return Compress(source, destination, out bytesConsumed, out bytesWritten, defQuality, defLgWin);
        }
        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, int quality, int lgwin)
        {
            return Compress(source, destination, out bytesConsumed, out bytesWritten, quality, lgwin, BrotliNative.BrotliEncoderMode.Generic);
        }

        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, BrotliNative.BrotliEncoderMode encMode)
        {
            return Compress(source, destination, out bytesConsumed, out bytesWritten, defQuality, defLgWin, encMode);
        }

        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten, int quality, int lgwin, BrotliNative.BrotliEncoderMode encMode)
        {
            if (quality > defQuality || quality <= 0) throw new System.ArgumentOutOfRangeException(BrotliEx.WrongQuality);
            if (lgwin > defLgWin || lgwin <= 0) throw new System.ArgumentOutOfRangeException(BrotliEx.WrongWindowSize);
            unsafe
            {
                bytesConsumed = (int)source.Length;
                bytesWritten = (int)0;
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint written = 0,consumed=(nuint)bytesConsumed;
                    if (!BrotliNative.BrotliEncoderCompress(quality, lgwin, encMode, consumed, bufIn, ref written, bufOut))
                    {
                        consumed = written = 0;
                        return false;
                    };
                    bytesWritten = (int)written;
                    return true;
                }
            }
        }

        public static BrotliNative.BrotliDecoderResult Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesConsumed, out int bytesWritten)
        {
            unsafe
            {
                bytesConsumed = source.Length;
                bytesWritten = 0;
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    nuint written = 0, consumed=(nuint)bytesConsumed;
                    BrotliNative.BrotliDecoderResult res=BrotliNative.BrotliDecoderDecompress(ref consumed, bufIn, ref written, bufOut);
                    if (res!=BrotliNative.BrotliDecoderResult.Success)
                    {
                        consumed = written = 0;   
                    }
                    bytesWritten = (int)written;
                    bytesWritten = (int)consumed;
                    return res;
                }
            }
        }
    }
}
