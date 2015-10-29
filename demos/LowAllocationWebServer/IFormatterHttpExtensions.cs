using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Net.Http.Buffered
{
    public static class IFormatterHttpExtensions
    {
        //TODO: Issue #387: In the Http extensions of IFormatter, we need to ensure that all the characters follow the basic rules of rfc2616
        private static readonly Utf8String Http = new Utf8String("HTTP/");
        private static readonly Utf8String HttpNewline = new Utf8String(new byte[] { 13, 10 });
        private const int ULongMaxValueNumberOfCharacters = 20;

        public static void WriteHttpStatusLine<TFormatter>(
            this TFormatter formatter,
            Utf8String version,
            Utf8String statusCode,
            Utf8String reasonCode) where TFormatter : IFormatter
        {
            formatter.Append(Http);
            formatter.Append(version);
            formatter.AppendWhiteSpace();
            formatter.Append(statusCode);
            formatter.AppendWhiteSpace();
            formatter.Append(reasonCode);
            formatter.AppendNewLine();
        }

        public static HttpHeaderBuffer WriteHttpHeader<TFormatter>(
            this TFormatter formatter, 
            Utf8String name,
            Utf8String value,
            int reserve = ULongMaxValueNumberOfCharacters)
            where TFormatter : IFormatter
        {
            formatter.Append(name);
            formatter.Append(" : ");
            
            Span<byte> valueBuffer;
            formatter.FormatWithReserve(value, reserve, out valueBuffer);                        

            formatter.AppendNewLine();

            return new HttpHeaderBuffer(valueBuffer, formatter.FormattingData);
        }

        public static void WriteHttpBody<TFormatter>(this TFormatter formatter, Utf8String body) 
            where TFormatter : IFormatter
        {
            formatter.Append(body);
        }

        public static void EndHttpHeaderSection<TFormatter>(this TFormatter formatter) where TFormatter : IFormatter
        {
            formatter.AppendNewLine();
        }

        public static void AppendWhiteSpace<TFormatter>(this TFormatter formatter) where TFormatter : IFormatter
        {
            formatter.Append(" ");
        }

        //TODO: Issue 385: Move this method into System.Text.Formatting
        public static void AppendNewLine<TFormatter>(this TFormatter formatter) where TFormatter : IFormatter
        {
            formatter.Append(HttpNewline);
        }
    }
}
