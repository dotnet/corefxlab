// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Text.Formatting;
using System.Buffers.Text;

namespace System.Text.JsonLab.Tests
{
    public class JsonWriterTests
    {
        [Fact]
        public void WriteJsonUtf8()
        {
            var formatter = new ArrayFormatter(1024, SymbolTable.InvariantUtf8);
            var json = new JsonWriterUtf8(formatter, prettyPrint: false);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace(" ", ""));

            formatter.Clear();
            json = new JsonWriterUtf8(formatter, prettyPrint: true);
            Write(ref json);

            formatted = formatter.Formatted;
            str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace("\r\n", "").Replace(" ", ""));
        }

        [Fact]
        public void WriteJsonUtf16()
        {
            var formatter = new ArrayFormatter(1024, SymbolTable.InvariantUtf16);
            var json = new JsonWriterUtf16(formatter, prettyPrint: false);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.Unicode.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace(" ", ""));

            formatter.Clear();
            json = new JsonWriterUtf16(formatter, prettyPrint: true);
            Write(ref json);

            formatted = formatter.Formatted;
            str = Encoding.Unicode.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace("\r\n", "").Replace(" ", ""));
        }

        static string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052},\"values\":[425121,-425122,425123]}";
        static void Write(ref JsonWriterUtf8 json)
        {
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteArrayStart("values");
            json.WriteValue(425121);
            json.WriteValue(-425122);
            json.WriteValue(425123);
            json.WriteArrayEnd();
            json.WriteObjectEnd();
        }

        static void Write(ref JsonWriterUtf16 json)
        {
            json.WriteObjectStart();
            json.WriteAttribute("age", 30);
            json.WriteAttribute("first", "John");
            json.WriteAttribute("last", "Smith");
            json.WriteArrayStart("phoneNumbers");
            json.WriteValue("425-000-1212");
            json.WriteValue("425-000-1213");
            json.WriteArrayEnd();
            json.WriteObjectStart("address");
            json.WriteAttribute("street", "1 Microsoft Way");
            json.WriteAttribute("city", "Redmond");
            json.WriteAttribute("zip", 98052);
            json.WriteObjectEnd();
            json.WriteArrayStart("values");
            json.WriteValue(425121);
            json.WriteValue(-425122);
            json.WriteValue(425123);
            json.WriteArrayEnd();
            json.WriteObjectEnd();
        }
    }
}
