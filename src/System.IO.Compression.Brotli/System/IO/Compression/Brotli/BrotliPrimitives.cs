using System;
using System.Collections.Generic;
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

        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out nuint bytesConsumed, out nuint bytesWritten)
        {
            return Compress(source, destination, out bytesConsumed, out bytesWritten, defQuality, defLgWin);
        }

        public static bool Compress(ReadOnlySpan<byte> source, Span<byte> destination, out nuint bytesConsumed, out nuint bytesWritten, int quality, int lgwin)
        {
            unsafe
            {
                bytesConsumed = (nuint)source.Length;
                bytesWritten = (nuint)destination.Length;
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    return BrotliNative.BrotliEncoderCompress(quality, lgwin, BrotliNative.BrotliEncoderMode.Generic, bytesConsumed, bufIn, ref bytesWritten, bufOut);
                }
            }
        }

        public static BrotliNative.BrotliDecoderResult Decompress(ReadOnlySpan<byte> source, Span<byte> destination, out nuint bytesConsumed, out nuint bytesWritten)
        {
            unsafe
            {
                bytesConsumed = (nuint)source.Length;
                bytesWritten = (nuint)destination.Length;
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &source.DangerousGetPinnableReference())
                fixed (byte* outBytes = &destination.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    return BrotliNative.BrotliDecoderDecompress(ref bytesConsumed, bufIn,ref bytesWritten, bufOut);
                }
            }
        }
    }
}
