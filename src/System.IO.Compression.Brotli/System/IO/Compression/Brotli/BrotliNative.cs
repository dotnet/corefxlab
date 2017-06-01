// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

#if BIT64
    using nuint = System.UInt64;
#else // BIT64
using nuint = System.UInt32;
#endif // BIT64

namespace System.IO.Compression
{
    /// <summary>
    /// This class provides declaration for constants and PInvokes as well as some basic tools for exposing the
    /// native Brotli library to managed code.
    /// </summary>
    //internal static partial class BrotliNative
    public class BrotliNative
    {
        /// <summary>
        /// 0 - Process input. Encoder may postpone producing output, until it has processed enough input.
        /// 1 - Produce output for all processed input.  Actual flush is performed when input stream is depleted and there is enoughspace in output stream.
        /// 2 - Finalize the stream. Adding more input data to finalized stream is impossible.
        /// 3 - Emit metadata block to stream.Stream is soft-flushed before metadata block is emitted. Metadata bloc MUST be no longer than than 16MiB.
        /// </summary>
        public enum BrotliEncoderOperation
        {
            Process,
            Flush,
            Finish,
            EmitMetadata
        };

        /// <summary>
        /// 0 - BrotliEncoderMode enumerates all available values.
        /// 1 - The main compression speed-density lever. The higher the quality, the slower the compression.Range is from ::BROTLI_MIN_QUALITY to::BROTLI_MAX_QUALITY.
        /// 2 - Recommended sliding LZ77 window size  2^value -16 .  Encoder may reduce this value, e.g. if input is much smaller than window size.
        /// 3 - Recommended input block size. 
        /// 4-  Flag that affects usage of "literal context modeling" format feature. This flag is a "decoding-speed vs compression ratio" trade-off.
        /// 5 - Estimated total input size for all ::BrotliEncoderCompressStream calls. The default value is 0, which means that the total input size is unknown.
        /// </summary>
        public enum BrotliEncoderParameter
        {
            Mode,
            Quality,
            LGWin,
            LGBlock,
            LCModeling,
            SizeHint
        };

        /// <summary>
        /// 0 - Decoding error, e.g. corrupted input or memory allocation problem.
        /// 1 - Decoding successfully completed
        /// 2 - Partially done; should be called again with more input
        /// 3 - Partially done; should be called again with more output
        /// </summary>
        public enum BrotliDecoderResult
        {
            Error,
            Success,
            NeedsMoreInput,
            NeedsMoreOutput
        };

        /// <summary>
        /// 0 - Default (Compressor does not know anything in advance properties of the input)
        /// 1 - For UTF-8 formatted text input
        /// 2 - Mode used in WOFF 2.0
        /// </summary>
        public enum BrotliEncoderMode
        {
            Generic,
            Text,
            Font
        };
        #region Encoder
        public static bool BrotliEncoderCompress(int quality, int lgwin, BrotliNative.BrotliEncoderMode mode, nuint input_size,
                IntPtr input_buffer, ref nuint encoded_size, IntPtr encoded_buffer)
        {
            return Interop.Brotli.BrotliEncoderCompress(quality, lgwin, mode, input_size, input_buffer, ref encoded_size, encoded_buffer);
        }
        
        public static IntPtr BrotliEncoderCreateInstance()
        {
           return Interop.Brotli.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter param, UInt32 value)
        {
            return Interop.Brotli.BrotliEncoderSetParameter(state, param, value);
        }

        public static void BrotliEncoderSetCustomDictionary(IntPtr state, nuint size, IntPtr dict)
        {
            Interop.Brotli.BrotliEncoderSetCustomDictionary(state, size, dict);
        }

        public static bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref nuint availableIn,
            ref IntPtr nextIn, ref nuint availableOut, ref IntPtr nextOut, out nuint totalOut)
        {
            return Interop.Brotli.BrotliEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }

        public static bool BrotliEncoderIsFinished(IntPtr state)
        {
            return Interop.Brotli.BrotliEncoderIsFinished(state);
        }

        public static void BrotliEncoderDestroyInstance(IntPtr state)
        {
            Interop.Brotli.BrotliEncoderDestroyInstance(state);
        }

        public static UInt32 BrotliEncoderVersion()
        {
            return Interop.Brotli.BrotliEncoderVersion();
            
        }
        #endregion
        #region Decoder
        public static BrotliDecoderResult BrotliDecoderDecompress(ref nuint availableIn, IntPtr nextIn, ref nuint availableOut, IntPtr nextOut)
        {
            return Interop.Brotli.BrotliDecoderDecompress(ref availableIn,  nextIn, ref availableOut, nextOut);
        }

        public static IntPtr BrotliDecoderCreateInstance()
        {
            return Interop.Brotli.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }

        public static void BrotliDecoderSetCustomDictionary(IntPtr state, nuint size, IntPtr dict)
        {
            Interop.Brotli.BrotliDecoderSetCustomDictionary(state, size, dict);
        }

        public static BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref nuint availableIn,
            ref IntPtr nextIn, ref nuint availableOut, ref IntPtr nextOut, out nuint totalOut)
        {
            return Interop.Brotli.BrotliDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
        }

        public static void BrotliDecoderDestroyInstance(IntPtr state)
        {
           Interop.Brotli.BrotliDecoderDestroyInstance(state);
        }

        public static nuint BrotliDecoderVersion()
        {
            return Interop.Brotli.BrotliDecoderVersion();
        }

        public static bool BrotliDecoderIsUsed(IntPtr state)
        {
            return Interop.Brotli.BrotliDecoderIsUsed(state);
        }

        public static bool BrotliDecoderIsFinished(IntPtr state)
        {
            return Interop.Brotli.BrotliDecoderIsFinished(state);
        }

        public static Int32 BrotliDecoderGetErrorCode(IntPtr state)
        {
           return Interop.Brotli.BrotliDecoderGetErrorCode(state);
           
        }

        public static String BrotliDecoderErrorString(Int32 code)
        {
            IntPtr result = IntPtr.Zero;
            result = Interop.Brotli.BrotliDecoderErrorString(code);
            if (result != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(result);
            }
            return String.Empty;
        }
        #endregion
    }
}

