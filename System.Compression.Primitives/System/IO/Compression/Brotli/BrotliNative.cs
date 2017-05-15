using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        private static IntPtr CreateState()
        {
            IntPtr state;
            int hr = Intertop.x86 ? CreateEncoderState86(out state, quality, lgwin) : CreateEncoderState64(out state, quality, lgwin);
            if (hr != 0)
                throw new Win32Exception(hr);

            return state;
        }
    }
}
