// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Http.Formatter
{
    public static class IFormatterHttpExtensions
    {
        //TODO: Issue #387: In the Http extensions of IFormatter, we need to ensure that all the characters follow the basic rules of rfc2616
        private static readonly string Http10 = "HTTP/1.0";
        private static readonly string Http11 = "HTTP/1.1";

        private static readonly byte[] s_Http10Utf8 = Encoding.UTF8.GetBytes(" HTTP/1.0\r\n");
        private static readonly byte[] s_Http11Utf8 = Encoding.UTF8.GetBytes(" HTTP/1.1\r\n");
        private static readonly byte[] s_Http20Utf8 = Encoding.UTF8.GetBytes(" HTTP/2.0\r\n");

        private static readonly byte[] s_GetUtf8 = Encoding.UTF8.GetBytes("GET ");
        private static readonly byte[] s_PostUtf8 = Encoding.UTF8.GetBytes("POST ");
        private static readonly byte[] s_PutUtf8 = Encoding.UTF8.GetBytes("PUT ");
        private static readonly byte[] s_DeleteUtf8 = Encoding.UTF8.GetBytes("DELETE ");

        public static void AppendHttpStatusLine<TFormatter>(this TFormatter formatter, Parser.Http.Version version, int statusCode, Utf8Span reasonCode) where TFormatter : ITextBufferWriter
        {
            switch (version)
            {
                case Parser.Http.Version.Http10: formatter.Append(Http10); break;
                case Parser.Http.Version.Http11: formatter.Append(Http11); break;
                //case HttpVersion.V2_0: formatter.Append(Http20); break;
                default: throw new ArgumentException(nameof(version));
            }

            formatter.Append(" ");
            formatter.Append(statusCode);
            formatter.Append(" ");
            formatter.Append(reasonCode);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpRequestLine<TFormatter>(this TFormatter formatter, Parser.Http.Method method, Parser.Http.Version version, string path) where TFormatter : ITextBufferWriter
        {
            if (formatter.SymbolTable == SymbolTable.InvariantUtf8)
            {
                switch (method)
                {
                    case Parser.Http.Method.Get: formatter.AppendBytes(s_GetUtf8); break;
                    case Parser.Http.Method.Post: formatter.AppendBytes(s_PostUtf8); break;
                    case Parser.Http.Method.Put: formatter.AppendBytes(s_PutUtf8); break;
                    case Parser.Http.Method.Delete: formatter.AppendBytes(s_DeleteUtf8); break;
                    default: formatter.Append(method.ToString()); formatter.Append(' '); break;
                }

                formatter.Append(path);

                switch (version)
                {
                    case Parser.Http.Version.Http10: formatter.AppendBytes(s_Http10Utf8); break;
                    case Parser.Http.Version.Http11: formatter.AppendBytes(s_Http11Utf8); break;
                    //case HttpVersion.V2_0: formatter.AppendBytes(s_Http20Utf8); break;
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
                    case Parser.Http.Version.Http10: formatter.Append(Http10); break;
                    case Parser.Http.Version.Http11: formatter.Append(Http11); break;
                    //case HttpVersion.V2_0: formatter.Append(Http20); break;
                    default: throw new ArgumentException(nameof(version));
                }
                formatter.AppendHttpNewLine();
            }
        }

        public static void AppendHttpHeader<TFormatter, T>(this TFormatter formatter, string name, T value, StandardFormat valueFormat = default) where TFormatter : ITextBufferWriter where T : IBufferFormattable
        {
            formatter.Append(name);
            formatter.Append(value, formatter.SymbolTable, valueFormat);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpHeader<TFormatter>(this TFormatter formatter, string name, string value) where TFormatter : ITextBufferWriter
        {
            formatter.Append(name);
            formatter.Append(value);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpHeader<TFormatter>(this TFormatter formatter, string name, int value) where TFormatter : ITextBufferWriter
        {
            formatter.Append(name);
            formatter.Append(value);
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpHeader<TFormatter>(this TFormatter formatter, string name, DateTime value) where TFormatter : ITextBufferWriter
        {
            formatter.Append(name);
            formatter.Append(value, 'R');
            formatter.AppendHttpNewLine();
        }

        public static void AppendHttpNewLine<TFormatter>(this TFormatter formatter) where TFormatter : ITextBufferWriter
        {
            var buffer = formatter.GetSpan();
            while (buffer.Length < 2)
            {
                buffer = formatter.GetSpan(2);
            }
            buffer[0] = 13;
            buffer[1] = 10;
            formatter.Advance(2);
        }

        static void AppendBytes<TFormatter>(this TFormatter formatter, byte[] bytes) where TFormatter : ITextBufferWriter
        {
            while (true)
            {
                var buffer = formatter.GetSpan();
                if (bytes.Length > buffer.Length)
                {
                    formatter.GetSpan(bytes.Length);
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
