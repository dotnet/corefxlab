using System.IO;
using System.Text.Json;
using Xunit;

namespace System.Text.Formatting.Tests
{
    public class CompositeFormattingTests
    {
        [Fact]
        public void WriteJsonUtf8()
        {
            var buffer = new byte[1024];
            var stream = new MemoryStream(buffer);
            var json = new JsonWriter(stream, FormattingData.Encoding.Utf8, prettyPrint: true);
            Write(json);
            var str = Encoding.UTF8.GetString(buffer, 0, (int)stream.Position);
            Assert.Equal(expected, str.Replace("\n", "").Replace(" ", ""));
        }

        [Fact]
        public void WriteJsonUtf16()
        {
            var buffer = new byte[1024];
            var stream = new MemoryStream(buffer);
            var json = new JsonWriter(stream, FormattingData.Encoding.Utf16);
            Write(json);
            var str = Encoding.Unicode.GetString(buffer, 0, (int)stream.Position);
            Assert.Equal(expected, str.Replace(" ", ""));
        }

        static string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052}}";
        static void Write(JsonWriter json)
        {
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteMember("phoneNumbers");
            json.WriteArrayStart();
            json.WriteString("425-000-1212");
            json.WriteString("425-000-1213");
            json.WriteArrayEnd();
            json.WriteMember("address");
            json.WriteObjectStart();
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteObjectEnd();
        }
    }
}
