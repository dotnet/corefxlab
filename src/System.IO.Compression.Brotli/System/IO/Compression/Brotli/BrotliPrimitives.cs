using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.IO.Compression
{
    struct BrotliPrimitives
    {
        int quality;
        int lgwin;
        IntPtr state;

        bool Compress(Span<byte> input, Span<byte> output, out int consumed, out int writen)
        {
            unsafe
            {
                consumed = input.Length;
                writen = output.Length;
                IntPtr bufIn, bufOut;
                fixed (byte *inBytes = &input.DangerousGetPinnableReference())
                {
                    bufIn = new IntPtr(inBytes);
                }
                fixed (byte *outBytes = &output.DangerousGetPinnableReference())
                {
                    bufOut = new IntPtr(outBytes);
                }
                IntPtr in_size = (IntPtr)consumed;
                IntPtr out_size = (IntPtr)writen;
                if (!BrotliNative.BrotliEncoderCompress(11, 24, BrotliNative.BrotliEncoderMode.Generic, in_size, bufIn, out_size, bufOut))
                {
                    
                };
                writen = (int)out_size;
                consumed = (int)in_size;
            }

            consumed =writen = 0;
            return true;
        }
    }
}
