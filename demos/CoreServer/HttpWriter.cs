using System.Diagnostics;
using System.IO;
using System.IO.Buffers;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading;

namespace System.Net.Http.Buffered
{
    public struct HttpWriter
    {
        public static Utf8String HttpNewline = new Utf8String(13, 10);

        public static void WriteCommonHeaders(BufferFormatter formatter, string responseLine)
        {
            var currentTime = DateTime.UtcNow;
            formatter.Append(responseLine);
            formatter.Append(HttpNewline);
            formatter.Append("Date: ");
            formatter.Append(currentTime, 'R');
            formatter.Append(HttpNewline);
            formatter.Append("Server: .NET Core Sample Server");
            formatter.Append(HttpNewline);
            formatter.Append("Last-Modified: ");
            formatter.Append(currentTime, 'R');
            formatter.Append(HttpNewline);
            formatter.Append("Content-Type: text/html; charset=UTF-8");
            formatter.Append(HttpNewline);
            formatter.Append("Connection: close");
            formatter.Append(HttpNewline);
            formatter.Append(HttpNewline);
        }
    }
}
