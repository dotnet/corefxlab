using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;

namespace System.Text.Http
{
    public static class IFormatterHttpExtensions
    {
        //TODO: Issue #387: In the Http extensions of IFormatter, we need to ensure that all the characters follow the basic rules of rfc2616
        private static readonly Utf8String Http10 = new Utf8String("HTTP/1.0");
        private static readonly Utf8String Http11 = new Utf8String("HTTP/1.1");
        private static readonly Utf8String Http20 = new Utf8String("HTTP/2.0");

        private const int ULongMaxValueNumberOfCharacters = 20;

        public static void AppendHttpStatusLine<TFormatter>(this TFormatter formatter, HttpVersion version, int statusCode, Utf8String reasonCode) where TFormatter : IFormatter
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

        public static void AppendHttpNewLine<TFormatter>(this TFormatter formatter) where TFormatter : IFormatter
        {
            var buffer = formatter.FreeBuffer;
            while(buffer.Length < 2) {
                formatter.ResizeBuffer();
                buffer = formatter.FreeBuffer;
            }
            buffer[0] = 13;
            buffer[1] = 10;
            formatter.CommitBytes(2);
        }
    }
}
