// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Runtime.InteropServices;

#if BIT64
    using nuint = System.UInt64;
#else
    using nuint = System.UInt32;
#endif 

namespace System.IO.Compression
{
    ///  /// <summary>
    /// Process - Process input. Encoder may postpone producing output, until it has processed enough input.
    /// Flush - Produce output for all processed input.  Actual flush is performed when input stream is depleted and there is enough space in output stream.
    /// Finish - Finalize the stream. Adding more input data to finalized stream is impossible.
    /// EmitMetadata - Emit metadata block to stream. Stream is soft-flushed before metadata block is emitted. Metadata bloc MUST be no longer than 16MiB.
    /// </summary>
    internal enum BrotliEncoderOperation
    {
        Process,
        Flush,
        Finish,
        EmitMetadata
    };

    /// <summary>
    /// Mode - BrotliEncoderMode enumerates all available values.
    /// Quality - The main compression speed-density lever. The higher the quality, the slower the compression.Range is from ::BROTLI_MIN_QUALITY to::BROTLI_MAX_QUALITY.
    /// LGWin - Recommended sliding LZ77 window size  2^value -16 .  Encoder may reduce this value, e.g. if input is much smaller than window size.
    /// LGBlock - Recommended input block size. 
    /// LCModeling-  Flag that affects usage of "literal context modeling" format feature. This flag is a "decoding-speed vs compression ratio" trade-off.
    /// SizeHint - Estimated total input size for all ::BrotliEncoderCompressStream calls. The default value is 0, which means that the total input size is unknown.
    /// </summary>
    internal enum BrotliEncoderParameter
    {
        Mode,
        Quality,
        LGWin,
        LGBlock,
        LCModeling,
        SizeHint
    };

    /// <summary>
    /// Error - Decoding error, e.g. corrupted input or memory allocation problem.
    /// Success - Decoding successfully completed
    /// NeedMoreInput - Partially done; should be called again with more input
    /// NeedMOreOutput - Partially done; should be called again with more output
    /// </summary>
    internal enum BrotliDecoderResult
    {
        Error,
        Success,
        NeedsMoreInput,
        NeedsMoreOutput
    };

    /// <summary>
    /// This class provides declaration for constants and PInvokes as well as some basic tools for exposing the
    /// native Brotli library to managed code.
    /// </summary>
    internal class BrotliNative
    {
       
        #region Encoder

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

