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

        public static bool Compress(ReadOnlySpan<byte> input, Span<byte> output, out nuint consumed, out nuint writen)
        {
            return Compress(input, output, out consumed, out writen, defQuality, defLgWin);
        }

        public static bool Compress(ReadOnlySpan<byte> input, Span<byte> output, out nuint consumed, out nuint writen, int quality, int lgwin)
        {
            unsafe
            {
                consumed = (nuint)input.Length;
                writen = (nuint)output.Length;
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &input.DangerousGetPinnableReference())
                fixed (byte* outBytes = &output.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    return BrotliNative.BrotliEncoderCompress(quality, lgwin, BrotliNative.BrotliEncoderMode.Generic, consumed, bufIn, ref writen, bufOut);
                }
            }
        }

        public static BrotliNative.BrotliDecoderResult Decompress(ReadOnlySpan<byte> input, Span<byte> output, out nuint consumed, out nuint writen)
        {
            unsafe
            {
                consumed = (nuint)input.Length;
                writen = (nuint)output.Length;
                IntPtr bufIn, bufOut;
                fixed (byte* inBytes = &input.DangerousGetPinnableReference())
                fixed (byte* outBytes = &output.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                    bufOut = new IntPtr(outBytes);
                    return BrotliNative.BrotliDecoderDecompress(ref consumed, bufIn,ref writen, bufOut);
                }
            }
        }
    }
}
