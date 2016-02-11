using System.Text.Formatting;
using System.Text.Http;
using System.Text.Utf8;

namespace System.Text.Http
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

        public static void Append<T>(this T formatter, Utf8String text) where T : IFormatter
        {
            var bytes = new byte[text.Length];
            int i = 0;
            foreach (var codeUnit in text)
            {
                bytes[i++] = codeUnit.Value;
            }

            var avaliable = formatter.FreeBuffer.Length;
            while (avaliable < bytes.Length)
            {
                avaliable = formatter.FreeBuffer.Length;
                formatter.ResizeBuffer();
            }

            formatter.FreeBuffer.Set(bytes);
            formatter.CommitBytes(bytes.Length);
        }

        public static void FormatWithReserve<T>(
            this T formatter,
            Utf8String text,
            int reserve,
            out Span<byte> bufferWithReserve) where T : IFormatter
        {
            var bytes = new byte[text.Length];
            var i = 0;
            foreach (var codeUnit in text)
            {
                bytes[i++] = codeUnit.Value;
            }

            var avaliable = formatter.FreeBuffer.Length;
            while (avaliable < bytes.Length + reserve)
            {
                avaliable = formatter.FreeBuffer.Length;
                formatter.ResizeBuffer();
            }

            formatter.FreeBuffer.Set(bytes);

            bufferWithReserve = formatter.FreeBuffer.Slice(0, bytes.Length + reserve);
            formatter.CommitBytes(bytes.Length + reserve);
            bufferWithReserve.SetFromRestOfSpanToEmpty(bytes.Length);
        }
    }
}
