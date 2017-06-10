// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Compression;

#if BIT64
    using nuint = System.UInt64;
#else
    using nuint = System.UInt32;
#endif 

namespace System.IO.Compression
{
    internal static partial class Interop
    {
        internal static partial class Brotli
        {
            internal const string _libNameEncoder = Library._brotliEncoder;
            internal const string _libNameDecoder = Library._brotliDecoder;

            #region Encoder

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderCompress(int quality, int lgwin, BrotliEncoderMode mode, nuint input_size,
                IntPtr input_buffer, ref nuint encoded_size, IntPtr encoded_buffer);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr BrotliEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderSetParameter(IntPtr state, BrotliEncoderParameter parameter, UInt32 value);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliEncoderSetCustomDictionary(IntPtr state, nuint size, IntPtr dict);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderCompressStream(
                IntPtr state, BrotliEncoderOperation op, ref nuint availableIn,
                ref IntPtr nextIn, ref nuint availableOut, ref IntPtr nextOut, out nuint totalOut);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderIsFinished(IntPtr state);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliEncoderDestroyInstance(IntPtr state);

            [DllImport(_libNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern UInt32 BrotliEncoderVersion();

            #endregion

            #region Decoder

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern BrotliDecoderResult BrotliDecoderDecompress(ref nuint availableIn, IntPtr nextIn,
                ref nuint availableOut, IntPtr nextOut);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr BrotliDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliDecoderSetCustomDictionary(IntPtr state, nuint size, IntPtr dict);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern BrotliDecoderResult BrotliDecoderDecompressStream(
                IntPtr state, ref nuint availableIn, ref IntPtr nextIn,
                ref nuint availableOut, ref IntPtr nextOut, out nuint totalOut);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern nuint BrotliDecoderVersion();

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliDecoderIsUsed(IntPtr state);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliDecoderIsFinished(IntPtr state);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern Int32 BrotliDecoderGetErrorCode(IntPtr state);

            [DllImport(_libNameDecoder, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            internal static extern IntPtr BrotliDecoderErrorString(Int32 code);

            #endregion

        }
    }
}

