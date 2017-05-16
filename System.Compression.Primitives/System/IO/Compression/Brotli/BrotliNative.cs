using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace System.IO.Compression
{
    /// <summary>
    /// This class provides declaration for constants and PInvokes as well as some basic tools for exposing the
    /// native Brotli library to managed code.
    /// </summary>
   internal static partial class BrotliNative
    {
        public enum BrotliDecoderResult : int
        {
            /// <summary>
            /// Decoding error, e.g. corrupt input or memory allocation problem
            /// </summary>
            Error = 0,
            /// <summary>
            /// Decoding successfully completed
            /// </summary>
            Success = 1,
            /// <summary>
            /// Partially done; should be called again with more input
            /// </summary>
            NeedsMoreInput = 2,
            /// <summary>
            /// Partially done; should be called again with more output
            /// </summary>
            NeedsMoreOutput = 3
        };
        public enum BrotliEncoderOperation : int
        {
            Process = 0,
            /// <summary>
            /// Request output stream to flush. Performed when input stream is depleted
            /// and there is enough space in output stream.
            /// </summary>
            Flush = 1,
            /// <summary>
            /// Request output stream to finish. Performed when input stream is depleted
            /// and there is enough space in output stream.
            /// </summary>
            Finish = 2,

            /// <summary>
            /// Emits metadata block to stream. Stream is soft-flushed before metadata
            /// block is emitted. CAUTION: when operation is started, length of the input
            /// buffer is interpreted as length of a metadata block; changing operation,
            /// expanding or truncating input before metadata block is completely emitted
            /// will cause an error; metadata block must not be greater than 16MiB.
            /// </summary>
            EmitMetadata = 3
        };

        public enum BrotliEncoderParameter : int
        {
            Mode = 0,
            /// <summary>
            ///  Controls the compression-speed vs compression-density tradeoffs. The higher
            ///  the quality, the slower the compression. Range is 0 to 11.
            /// </summary>
            Quality = 1,
            /// <summary>
            /// Base 2 logarithm of the sliding window size. Range is 10 to 24. 
            /// </summary>
            LGWin = 2,

            /// <summary>
            /// Base 2 logarithm of the maximum input block size. Range is 16 to 24.
            /// If set to 0, the value will be set based on the quality.
            /// </summary>
            LGBlock = 3
        };

        static bool UseX86 = IntPtr.Size == 4;
        #region Encoder
        public static IntPtr BrotliEncoderCreateInstance()
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return Interop.Brotli64.BrotliEncoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }




        public static bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliEncoderSetParameter(state, parameter, value);
            }
            else
            {
                return Interop.Brotli64.BrotliEncoderSetParameter(state, parameter, value);
            }
        }

        public static void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                Interop.Brotli86.BrotliEncoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                Interop.Brotli64.BrotliEncoderSetCustomDictionary(state, size, dict);
            }
        }

        public static bool BrotliEncoderCompressStream(
            IntPtr state, BrotliEncoderOperation op, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliEncoderCompressStream(state, op, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = Interop.Brotli64.BrotliEncoderCompressStream(state, op, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
                availableIn = (UInt32)availableInL;
                availableOut = (UInt32)availableOutL;
                totalOut = (UInt32)totalOutL;
                return r;
            }
        }

        public static bool BrotliEncoderIsFinished(IntPtr state)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliEncoderIsFinished(state);
            }
            else
            {
                return Interop.Brotli64.BrotliEncoderIsFinished(state);
            }
        }

        public static void BrotliEncoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                Interop.Brotli86.BrotliEncoderDestroyInstance(state);
            }
            else
            {
                Interop.Brotli64.BrotliEncoderDestroyInstance(state);
            }
        }

        public static UInt32 BrotliEncoderVersion()
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliEncoderVersion();
            }
            else
            {
                return Interop.Brotli64.BrotliEncoderVersion();
            }
        }


        #endregion
        #region Decoder
        public static IntPtr BrotliDecoderCreateInstance()
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
            else
            {
                return Interop.Brotli64.BrotliDecoderCreateInstance(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public static void BrotliDecoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict)
        {
            if (UseX86)
            {
                Interop.Brotli86.BrotliDecoderSetCustomDictionary(state, size, dict);
            }
            else
            {
                Interop.Brotli64.BrotliDecoderSetCustomDictionary(state, size, dict);
            }
        }

        public static BrotliDecoderResult BrotliDecoderDecompressStream(
            IntPtr state, ref UInt32 availableIn,
            ref IntPtr nextIn, ref UInt32 availableOut, ref IntPtr nextOut, out UInt32 totalOut)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliDecoderDecompressStream(state, ref availableIn, ref nextIn, ref availableOut, ref nextOut, out totalOut);
            }
            else
            {
                UInt64 availableInL = availableIn;
                UInt64 availableOutL = availableOut;
                UInt64 totalOutL = 0;
                var r = Interop.Brotli64.BrotliDecoderDecompressStream(state, ref availableInL, ref nextIn, ref availableOutL, ref nextOut, out totalOutL);
                availableIn = (UInt32)availableInL;
                availableOut = (UInt32)availableOutL;
                totalOut = (UInt32)totalOutL;
                return r;
            }
        }

        public static void BrotliDecoderDestroyInstance(IntPtr state)
        {
            if (UseX86)
            {
                Interop.Brotli86.BrotliDecoderDestroyInstance(state);
            }
            else
            {
                Interop.Brotli64.BrotliDecoderDestroyInstance(state);
            }
        }

        public static UInt32 BrotliDecoderVersion()
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliDecoderVersion();
            }
            else
            {
                return Interop.Brotli64.BrotliDecoderVersion();
            }
        }

        public static bool BrotliDecoderIsUsed(IntPtr state)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliDecoderIsUsed(state);
            }
            else
            {
                return Interop.Brotli64.BrotliDecoderIsUsed(state);
            }
        }
        public static bool BrotliDecoderIsFinished(IntPtr state)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliDecoderIsFinished(state);
            }
            else
            {
                return Interop.Brotli64.BrotliDecoderIsFinished(state);
            }

        }
        public static Int32 BrotliDecoderGetErrorCode(IntPtr state)
        {
            if (UseX86)
            {
                return Interop.Brotli86.BrotliDecoderGetErrorCode(state);
            }
            else
            {
                return Interop.Brotli64.BrotliDecoderGetErrorCode(state);
            }
        }

        public static String BrotliDecoderErrorString(Int32 code)
        {
            IntPtr r = IntPtr.Zero;
            if (UseX86)
            {
                r = Interop.Brotli86.BrotliDecoderErrorString(code);
            }
            else
            {
                r = Interop.Brotli64.BrotliDecoderErrorString(code);
            }

            if (r != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(r);
            }
            return String.Empty;


        }


        #endregion
    }
}

