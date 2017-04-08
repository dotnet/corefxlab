// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        private const int ULongMaxValueNumberOfCharacters = 20;

        public static void AppendHttpStatusLine<TFormatter>(this TFormatter formatter, HttpVersion version, int statusCode, Utf8String reasonCode) where TFormatter : ITextOutput
        {
            switch (version) {
                case HttpVersion.V1_0: formatter.Append(new Utf8String(Http10)); break;
                case HttpVersion.V1_1: formatter.Append(new Utf8String(Http11)); break;
                case HttpVersion.V2_0: formatter.Append(new Utf8String(Http20)); break;
                default: throw new ArgumentException(nameof(version));
            }

            formatter.Append(" ");
            formatter.Append(statusCode);
            formatter.Append(" ");
            formatter.Append(reasonCode);
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
    }
}
