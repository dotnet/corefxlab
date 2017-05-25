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
            internal const String LibNameEncoder = @"..\lib\brotlienc.dll";
            internal const String LibNameDecoder = @"..\lib\brotlidec.dll";
            #region Encoder
            [DllImport(LibNameEncoder,CallingConvention = CallingConvention.Cdecl, EntryPoint = "BrotliEncoderCreateInstance ")]
            internal static extern IntPtr BrotliEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

            [DllImport(LibNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderSetParameter(IntPtr state, BrotliNative.BrotliEncoderParameter parameter, UInt32 value);

            [DllImport(LibNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

            [DllImport(LibNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderCompressStream(
                IntPtr state, BrotliNative.BrotliEncoderOperation op, ref IntPtr availableIn,
                ref IntPtr nextIn, ref IntPtr availableOut, ref IntPtr nextOut, out nuint totalOut);

            [DllImport(LibNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderIsFinished(IntPtr state);

            [DllImport(LibNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliEncoderDestroyInstance(IntPtr state);
            [DllImport(LibNameEncoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern UInt32 BrotliEncoderVersion();

            #endregion
            #region Decoder
            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr BrotliDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliDecoderSetCustomDictionary(IntPtr state, nuint size, IntPtr dict);

            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern BrotliNative.BrotliDecoderResult BrotliDecoderDecompressStream(
                IntPtr state, ref IntPtr availableIn, ref IntPtr nextIn,
                ref IntPtr availableOut, ref IntPtr nextOut, out nuint totalOut);

            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern nuint BrotliDecoderVersion();

            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliDecoderIsUsed(IntPtr state);
            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliDecoderIsFinished(IntPtr state);
            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl)]
            internal static extern Int32 BrotliDecoderGetErrorCode(IntPtr state);
            [DllImport(LibNameDecoder, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            internal static extern IntPtr BrotliDecoderErrorString(Int32 code);
            #endregion

        }
    }
}

