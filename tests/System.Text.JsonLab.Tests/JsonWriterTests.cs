// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Text.Formatting;
using System.Buffers.Text;
using System.IO;

namespace System.Text.JsonLab.Tests
{
    public class JsonWriterTests
    {
        private const int ExtraArraySize = 500;

        /*[Fact]
        public void WriteJsonUtf8()
        {
            var formatter = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var json = new Utf8JsonWriter<ArrayFormatterWrapper>(formatter, prettyPrint: false);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace(" ", ""));

            formatter.Clear();
            json = new Utf8JsonWriter<ArrayFormatterWrapper>(formatter, prettyPrint: true);
            Write(ref json);

            formatted = formatter.Formatted;
            str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace("\r\n", "").Replace("\n", "").Replace(" ", ""));
        }

        static readonly string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\",null],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"values\":[425121,-425122,425123]}";

        static void Write(ref Utf8JsonWriter<ArrayFormatterWrapper> json)
        {
            int[] values = { 425121, -425122, 425123 };
            byte[] valueString = Encoding.UTF8.GetBytes("values");
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteNull();
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteArrayUtf8(valueString, values);
            json.WriteObjectEnd();
            json.Flush();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteHelloWorldJsonUtf8(bool prettyPrint)
        {
            string expectedStr = GetHelloWorldExpectedString(prettyPrint);

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output, prettyPrint);

            jsonUtf8.WriteObjectStart();
            jsonUtf8.WriteAttribute("message", "Hello, World!");
            jsonUtf8.WriteObjectEnd();
            jsonUtf8.Flush();

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            Assert.Equal(expectedStr, actualStr);
        }*/

        /*[Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WriteBasicJsonUtf8(bool prettyPrint)
        {
            int[] data = GetData(ExtraArraySize, 42, -10000, 10000);
            byte[] ExtraArray = Encoding.UTF8.GetBytes("ExtraArray");
            string expectedStr = GetExpectedString(prettyPrint, isUtf8: true, data);

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output, prettyPrint);

            jsonUtf8.WriteObjectStart();
            jsonUtf8.WriteAttribute("age", 42);
            jsonUtf8.WriteAttribute("first", null);
            jsonUtf8.WriteAttribute("last", "Smith");
            jsonUtf8.WriteArrayStart("phoneNumbers");
            jsonUtf8.WriteValue("425-000-1212");
            jsonUtf8.WriteValue("425-000-1213");
            jsonUtf8.WriteArrayEnd();
            jsonUtf8.WriteObjectStart("address");
            jsonUtf8.WriteAttribute("street", "1 Microsoft Way");
            jsonUtf8.WriteAttribute("city", "Redmond");
            jsonUtf8.WriteAttribute("zip", 98052);
            jsonUtf8.WriteObjectEnd();

            // Add a large array of values
            jsonUtf8.WriteArrayUtf8(ExtraArray, data);

            jsonUtf8.WriteObjectEnd();
            jsonUtf8.Flush();

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            Assert.Equal(expectedStr, actualStr);
        }

        private static int[] GetData(int size, int seed, int minValue, int maxValue)
        {
            int[] data = new int[size];
            Random rand = new Random(seed);

            for (int i = 0; i < ExtraArraySize; i++)
            {
                data[i] = rand.Next(minValue, maxValue);
            }

            return data;
        }*/

        /*private static string GetExpectedString(bool prettyPrint, bool isUtf8, int[] data)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(ms, new UTF8Encoding(false), 1024, true);

            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);

            TextWriter writer = isUtf8 ? streamWriter : (TextWriter)stringWriter;

            var json = new Newtonsoft.Json.JsonTextWriter(writer)
            {
                Formatting = prettyPrint ? Newtonsoft.Json.Formatting.Indented : Newtonsoft.Json.Formatting.None
            };

            json.WriteStartObject();
            json.WritePropertyName("age");
            json.WriteValue(42);
            json.WritePropertyName("first");
            json.WriteValue((string)null);
            json.WritePropertyName("last");
            json.WriteValue("Smith");
            json.WritePropertyName("phoneNumbers");
            json.WriteStartArray();
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteEnd();
            json.WritePropertyName("address");
            json.WriteStartObject();
            json.WritePropertyName("street");
            json.WriteValue("1 Microsoft Way");
            json.WritePropertyName("city");
            json.WriteValue("Redmond");
            json.WritePropertyName("zip");
            json.WriteValue(98052);
            json.WriteEnd();

            // Add a large array of values
            json.WritePropertyName("ExtraArray");
            json.WriteStartArray();
            for (var i = 0; i < ExtraArraySize; i++)
            {
                json.WriteValue(data[i]);
            }
            json.WriteEnd();

            json.WriteEnd();

            json.Flush();

            return isUtf8 ? Encoding.UTF8.GetString(ms.ToArray()) : sb.ToString();
        }*/
    }
}
