// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Buffers.Text;

using static System.Runtime.InteropServices.MemoryMarshal;

namespace System.Text.Http.Parser.Internal
{
    internal static class HttpUtilities
    {
        public const string Http10Version = "HTTP/1.0";
        public const string Http11Version = "HTTP/1.1";

        public const string HttpUriScheme = "http://";
        public const string HttpsUriScheme = "https://";

        // readonly primitive statics can be Jit'd to consts https://github.com/dotnet/coreclr/issues/1079

        private readonly static ulong _httpSchemeLong = GetAsciiStringAsLong(HttpUriScheme + "\0");
        private readonly static ulong _httpsSchemeLong = GetAsciiStringAsLong(HttpsUriScheme);
        private readonly static ulong _httpConnectMethodLong = GetAsciiStringAsLong("CONNECT ");
        private readonly static ulong _httpDeleteMethodLong = GetAsciiStringAsLong("DELETE \0");
        private const uint _httpGetMethodInt = 542393671; // retun of GetAsciiStringAsInt("GET "); const results in better codegen
        private readonly static ulong _httpHeadMethodLong = GetAsciiStringAsLong("HEAD \0\0\0");
        private readonly static ulong _httpPatchMethodLong = GetAsciiStringAsLong("PATCH \0\0");
        private readonly static ulong _httpPostMethodLong = GetAsciiStringAsLong("POST \0\0\0");
        private readonly static ulong _httpPutMethodLong = GetAsciiStringAsLong("PUT \0\0\0\0");
        private readonly static ulong _httpOptionsMethodLong = GetAsciiStringAsLong("OPTIONS ");
        private readonly static ulong _httpTraceMethodLong = GetAsciiStringAsLong("TRACE \0\0");

        private const ulong _http10VersionLong = 3471766442030158920; // GetAsciiStringAsLong("HTTP/1.0"); const results in better codegen
        private const ulong _http11VersionLong = 3543824036068086856; // GetAsciiStringAsLong("HTTP/1.1"); const results in better codegen

        private readonly static ulong _mask8Chars = GetMaskAsLong(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff });
        private readonly static ulong _mask7Chars = GetMaskAsLong(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00 });
        private readonly static ulong _mask6Chars = GetMaskAsLong(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00 });
        private readonly static ulong _mask5Chars = GetMaskAsLong(new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00 });
        private readonly static ulong _mask4Chars = GetMaskAsLong(new byte[] { 0xff, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00 });

        private readonly static (ulong Mask, ulong Signature, Http.Method Method, int Length)[] _knownMethods =
        {
            (_mask4Chars, _httpPutMethodLong, Http.Method.Put, 3),
            (_mask5Chars, _httpPostMethodLong, Http.Method.Post, 4),
            (_mask5Chars, _httpHeadMethodLong, Http.Method.Head, 4),
            (_mask6Chars, _httpTraceMethodLong, Http.Method.Trace, 5),
            (_mask6Chars, _httpPatchMethodLong, Http.Method.Patch, 5),
            (_mask7Chars, _httpDeleteMethodLong, Http.Method.Delete, 6),
            (_mask8Chars, _httpConnectMethodLong, Http.Method.Connect, 7),
            (_mask8Chars, _httpOptionsMethodLong, Http.Method.Options, 7),
        };

        private readonly static string[] _methodNames = CreateMethodNames();

        private static string[] CreateMethodNames()
        {
            var methodNames = new string[9];
            methodNames[(byte)Http.Method.Get] = "GET";
            methodNames[(byte)Http.Method.Put] = "PUT";
            methodNames[(byte)Http.Method.Delete] = "DELETE";
            methodNames[(byte)Http.Method.Post] = "POST";
            methodNames[(byte)Http.Method.Head] = "HEAD";
            methodNames[(byte)Http.Method.Trace] = "TRACE";
            methodNames[(byte)Http.Method.Patch] = "PATCH";
            methodNames[(byte)Http.Method.Connect] = "CONNECT";
            methodNames[(byte)Http.Method.Options] = "OPTIONS";
            return methodNames;
        }

        private static ulong GetAsciiStringAsLong(string str)
        {
            Debug.Assert(str.Length == 8, "String must be exactly 8 (ASCII) characters long.");

            Span<byte> span = stackalloc byte[8];
            Encodings.Utf16.ToUtf8(AsBytes(str.AsSpan()), span, out int consumed, out int written);
            return Read<ulong>(span);
        }

        private unsafe static ulong GetMaskAsLong(byte[] bytes)
        {
            Debug.Assert(bytes.Length == 8, "Mask must be exactly 8 bytes long.");

            fixed (byte* ptr = bytes)
            {
                return *(ulong*)ptr;
            }
        }

        public static string GetAsciiStringEscaped(this ReadOnlySpan<byte> span, int maxChars)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < Math.Min(span.Length, maxChars); i++)
            {
                var ch = span[i];
                sb.Append(ch < 0x20 || ch >= 0x7F ? $"\\x{ch:X2}" : ((char)ch).ToString());
            }

            if (span.Length > maxChars)
            {
                sb.Append("...");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Checks that up to 8 bytes from <paramref name="span"/> correspond to a known HTTP method.
        /// </summary>
        /// <remarks>
        /// A "known HTTP method" can be an HTTP method name defined in the HTTP/1.1 RFC.
        /// Since all of those fit in at most 8 bytes, they can be optimally looked up by reading those bytes as a long. Once
        /// in that format, it can be checked against the known method.
        /// The Known Methods (CONNECT, DELETE, GET, HEAD, PATCH, POST, PUT, OPTIONS, TRACE) are all less than 8 bytes
        /// and will be compared with the required space. A mask is used if the Known method is less than 8 bytes.
        /// To optimize performance the GET method will be checked first.
        /// </remarks>
        /// <returns><c>true</c> if the input matches a known string, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetKnownMethod(in this ReadOnlySpan<byte> span, out Http.Method method, out int length)
        {
            Debug.Assert(span.Length > sizeof(ulong));

            length = 0;
            method = Http.Method.Custom;

            if (Unsafe.ReadUnaligned<uint>(ref GetReference(span)) == _httpGetMethodInt)
            {
                length = 3;
                method = Http.Method.Get;
            }
            else
            {
                ulong value = Unsafe.ReadUnaligned<uint>(ref GetReference(span));
                foreach (var (Mask, Signature, Method, Length) in _knownMethods)
                {
                    if ((value & Mask) == Signature)
                    {
                        length = Length;
                        method = Method;
                        break;
                    }
                }
            }

            return method != Http.Method.Custom;
        }

        /// <summary>
        /// Checks 9 bytes from <paramref name="location"/>  correspond to a known HTTP version.
        /// </summary>
        /// <remarks>
        /// A "known HTTP version" Is is either HTTP/1.0 or HTTP/1.1.
        /// Since those fit in 8 bytes, they can be optimally looked up by reading those bytes as a long. Once
        /// in that format, it can be checked against the known versions.
        /// To optimize performance the HTTP/1.1 will be checked first.
        /// </remarks>
        /// <returns><c>true</c> if the input matches a known string, <c>false</c> otherwise.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool GetKnownVersion(in ReadOnlySpan<byte> location, out Http.Version knownVersion)
        {
            knownVersion = Http.Version.Unknown;

            if (location.Length == sizeof(ulong))
            {
                ulong version = Unsafe.ReadUnaligned<ulong>(ref GetReference(location));

                if (version == _http11VersionLong)
                {
                    knownVersion = Http.Version.Http11;
                }
                else if (version == _http10VersionLong)
                {
                    knownVersion = Http.Version.Http10;
                }
            }

            return knownVersion != Http.Version.Unknown;
        }

        /// <summary>
        /// Checks 8 bytes from <paramref name="span"/> that correspond to 'http://' or 'https://'
        /// </summary>
        /// <param name="span">The span</param>
        /// <param name="knownScheme">A reference to the known scheme, if the input matches any</param>
        /// <returns>True when memory starts with known http or https schema</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool GetKnownHttpScheme(this Span<byte> span, out HttpScheme knownScheme)
        {
            if (span.Length >= sizeof(ulong))
            {
                ulong scheme = Unsafe.ReadUnaligned<ulong>(ref GetReference(span));
                if ((scheme & _mask7Chars) == _httpSchemeLong)
                {
                    knownScheme = HttpScheme.Http;
                    return true;
                }

                if (scheme == _httpsSchemeLong)
                {
                    knownScheme = HttpScheme.Https;
                    return true;
                }
            }
            knownScheme = HttpScheme.Unknown;
            return false;
        }

        public static string VersionToString(Http.Version httpVersion)
        {
            switch (httpVersion)
            {
                case Http.Version.Http10:
                    return Http10Version;
                case Http.Version.Http11:
                    return Http11Version;
                default:
                    return null;
            }
        }

        public static string MethodToString(Http.Method method)
        {
            int methodIndex = (int)method;
            if (methodIndex >= 0 && methodIndex <= 8)
            {
                return _methodNames[methodIndex];
            }
            return null;
        }

        public static string SchemeToString(HttpScheme scheme)
        {
            switch (scheme)
            {
                case HttpScheme.Http:
                    return HttpUriScheme;
                case HttpScheme.Https:
                    return HttpsUriScheme;
                default:
                    return null;
            }
        }
    }
}
