using System.Text.Formatting;

namespace System.Net.Http.Buffered
{
    public static class IFormatterHttpExtensions
    {
        private const string Http = "HTTP/";
        private const int ULongMaxValueNumberOfCharacters = 20;

        public static void WriteHttpStatusLine<TFormatter>(
            this TFormatter formatter, 
            string version, 
            string statusCode,
            string reasonCode) where TFormatter : IFormatter
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
            string name, 
            string value,
            int reserve = ULongMaxValueNumberOfCharacters)
            where TFormatter : IFormatter
        {
            formatter.Append(name);
            formatter.Append(" : ");
            
            int bytesWritten;
            while (!value.TryFormat(formatter.FreeBuffer, default(Format.Parsed), formatter.FormattingData, out bytesWritten))
            {                
                formatter.ResizeBuffer();
                bytesWritten = 0;
            }

            var valueBuffer = formatter.FreeBuffer.Slice(0, bytesWritten + reserve);
            formatter.CommitBytes(bytesWritten + reserve);
            valueBuffer.SetFromRestOfSpanToEmpty(bytesWritten);

            formatter.AppendNewLine();

            return new HttpHeaderBuffer(valueBuffer);
        }

        public static void WriteHttpBody<TFormatter>(this TFormatter formatter, string body) 
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

        public static void AppendNewLine<TFormatter>(this TFormatter formatter) where TFormatter : IFormatter
        {
            formatter.Append("\r\n");
        }
    }
}
