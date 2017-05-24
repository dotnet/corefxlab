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
            internal const String LibName = @"C:\Users\t-detsom\Source\Repos\corefxlab\lib\brotli.dll";
            #region Encoder
            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr BrotliEncoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderSetParameter(IntPtr state, BrotliNative.BrotliEncoderParameter parameter, UInt32 value);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliEncoderSetCustomDictionary(IntPtr state, UInt32 size, IntPtr dict);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderCompressStream(
                IntPtr state, BrotliNative.BrotliEncoderOperation op, ref IntPtr availableIn,
                ref IntPtr nextIn, ref IntPtr availableOut, ref IntPtr nextOut, out nuint totalOut);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliEncoderIsFinished(IntPtr state);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliEncoderDestroyInstance(IntPtr state);
            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern UInt32 BrotliEncoderVersion();

            #endregion
            #region Decoder
            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern IntPtr BrotliDecoderCreateInstance(IntPtr allocFunc, IntPtr freeFunc, IntPtr opaque);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliDecoderSetCustomDictionary(IntPtr state, nuint size, IntPtr dict);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern BrotliNative.BrotliDecoderResult BrotliDecoderDecompressStream(
                IntPtr state, ref IntPtr availableIn, ref IntPtr nextIn,
                ref IntPtr availableOut, ref IntPtr nextOut, out nuint totalOut);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern void BrotliDecoderDestroyInstance(IntPtr state);

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern nuint BrotliDecoderVersion();

            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliDecoderIsUsed(IntPtr state);
            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern bool BrotliDecoderIsFinished(IntPtr state);
            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]
            internal static extern Int32 BrotliDecoderGetErrorCode(IntPtr state);
            [DllImport(LibName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
            internal static extern IntPtr BrotliDecoderErrorString(Int32 code);
            #endregion

        }
    }
}

