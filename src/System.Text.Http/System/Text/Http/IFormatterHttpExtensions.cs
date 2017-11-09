// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Http
{
    public static class IFormatterHttpExtensions
    {
        //TODO: Issue #387: In the Http extensions of IFormatter, we need to ensure that all the characters follow the basic rules of rfc2616
        private static readonly string Http10 = "HTTP/1.0";
        private static readonly string Http11 = "HTTP/1.1";
        private static readonly string Http20 = "HTTP/2.0";

        private static readonly byte[] s_Http10Utf8 = Encoding.UTF8.GetBytes(" HTTP/1.0\r\n");
        private static readonly byte[] s_Http11Utf8 = Encoding.UTF8.GetBytes(" HTTP/1.1\r\n");
        private static readonly byte[] s_Http20Utf8 = Encoding.UTF8.GetBytes(" HTTP/2.0\r\n");

        private static readonly byte[] s_GetUtf8 = Encoding.UTF8.GetBytes("GET ");
        private static readonly byte[] s_PostUtf8 = Encoding.UTF8.GetBytes("POST ");
        private static readonly byte[] s_PutUtf8 = Encoding.UTF8.GetBytes("PUT ");
        private static readonly byte[] s_DeleteUtf8 = Encoding.UTF8.GetBytes("DELETE ");

        public static void AppendHttpStatusLine<TFormatter>(this TFormatter formatter, HttpVersion version, int statusCode, Utf8Span reasonCode) where TFormatter : ITextOutput
        {
            switch (version) {
                case HttpVersion.V1_0: formatter.Append(Http10); break;
                case HttpVersion.V1_1: formatter.Append(Http11); break;
                case HttpVersion.V2_0: formatter.Append(Http20); break;
                default: throw new ArgumentException(nameof(version));
            }

            formatter.Append(" ");
            formatter.Append(statusCode);
            formatter.Append(" ");
            formatter.Append(reasonCode);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpRequestLine<TFormatter>(this TFormatter formatter, HttpMethod method, HttpVersion version, string path) where TFormatter : ITextOutput
        {
            if (formatter.SymbolTable == SymbolTable.InvariantUtf8)
            {
                switch (method)
                {
                    case HttpMethod.Get: formatter.AppendBytes(s_GetUtf8); break;
                    case HttpMethod.Post: formatter.AppendBytes(s_PostUtf8); break;
                    case HttpMethod.Put: formatter.AppendBytes(s_PutUtf8); break;
                    case HttpMethod.Delete: formatter.AppendBytes(s_DeleteUtf8); break;
                    default: formatter.Append(method.ToString()); formatter.Append(' '); break;
                }

                formatter.Append(path);

                switch (version)
                {
                    case HttpVersion.V1_0: formatter.AppendBytes(s_Http10Utf8); break;
                    case HttpVersion.V1_1: formatter.AppendBytes(s_Http11Utf8); break;
                    case HttpVersion.V2_0: formatter.AppendBytes(s_Http20Utf8); break;
                    default: throw new ArgumentException(nameof(version));
                }
            }
            else
            {
                formatter.Append(method.ToString());
                formatter.Append(' ');
                formatter.Append(path);
                formatter.Append(' ');
                switch (version)
                {
                    case HttpVersion.V1_0: formatter.Append(Http10); break;
                    case HttpVersion.V1_1: formatter.Append(Http11); break;
                    case HttpVersion.V2_0: formatter.Append(Http20); break;
                    default: throw new ArgumentException(nameof(version));
                }
                formatter.AppendHttpNewLine();
            }
        }

        public static void AppendHttpHeader<TFormatter, T>(this TFormatter formatter, string name, T value, StandardFormat valueFormat = default) where TFormatter : ITextOutput where T:IBufferFormattable
        {
            formatter.Append(name);
            formatter.Append(value, formatter.SymbolTable, valueFormat);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpHeader<TFormatter>(this TFormatter formatter, string name, string value) where TFormatter : ITextOutput
        {
            formatter.Append(name);
            formatter.Append(value);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpHeader<TFormatter>(this TFormatter formatter, string name, int value) where TFormatter : ITextOutput
        {
            formatter.Append(name);
            formatter.Append(value);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpHeader<TFormatter>(this TFormatter formatter, string name, DateTime value) where TFormatter : ITextOutput
        {
            formatter.Append(name);
            formatter.Append(value, 'R');
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpNewLine<TFormatter>(this TFormatter formatter) where TFormatter : ITextOutput
        {
            var buffer = formatter.Buffer;
            while(buffer.Length < 2) {
                formatter.Enlarge(2);
                buffer = formatter.Buffer;
            }
            buffer[0] = 13;
            buffer[1] = 10;
            formatter.Advance(2);
        }

        static void AppendBytes<TFormatter>(this TFormatter formatter, byte[] bytes) where TFormatter : ITextOutput
        {
            while (true)
            {
                var buffer = formatter.Buffer;
                if (bytes.Length > buffer.Length)
                {
                    formatter.Enlarge(bytes.Length);
                }
                else
                {
                    bytes.AsSpan().CopyTo(buffer);
                    break;
                }
            }
            formatter.Advance(bytes.Length);
        }
    }
}
