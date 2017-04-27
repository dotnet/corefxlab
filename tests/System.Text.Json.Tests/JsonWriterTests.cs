// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;
using System.Text.Formatting;

namespace System.Text.Json.Tests
{
    public class CompositeFormattingTests
    {
        [Fact]
        public void WriteJsonUtf8()
        {
            var formatter = new ArrayFormatter(1024, TextEncoder.Utf8);
            var json = new JsonWriter<ArrayFormatter>(formatter, prettyPrint: true);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace("\n", "").Replace(" ", ""));
        }

        [Fact]
        public void WriteJsonUtf16()
        {
            var formatter = new ArrayFormatter(1024, TextEncoder.Utf16);
            var json = new JsonWriter<ArrayFormatter>(formatter, prettyPrint: false);
            Write(ref json);

            var formatted = formatter.Formatted;
            var str = Encoding.Unicode.GetString(formatted.Array, formatted.Offset, formatted.Count);
            Assert.Equal(expected, str.Replace(" ", ""));
        }

        static string expected = "{\"age\":30,\"first\":\"John\",\"last\":\"Smith\",\"phoneNumbers\":[\"425-000-1212\",\"425-000-1213\"],\"address\":{\"street\":\"1MicrosoftWay\",\"city\":\"Redmond\",\"zip\":98052}}";
        static void Write(ref JsonWriter<ArrayFormatter> json)
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
