// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public class EnumTests
    {
        public static readonly string s_jsonStringEnum =
                @"{" +
                @"""MyEnum"" : ""Two""" +
                @"}";

        public static readonly string s_jsonIntEnum =
                @"{" +
                @"""MyEnum"" : 2" +
                @"}";

        [Fact]
        public static void EnumAsStringFail()
        {
            Assert.Throws<InvalidOperationException>(() => JsonSerializer.ReadString<SimpleTestClass>(s_jsonStringEnum));
        }

        [Fact]
        public static void EnumAsString()
        {
            var options = new JsonSerializerOptions();
            options.AddAttribute(typeof(SimpleTestClass), new JsonEnumConverterAttribute(treatAsString: true));

            SimpleTestClass obj = JsonSerializer.ReadString<SimpleTestClass>(s_jsonStringEnum, options);
            Assert.Equal(SampleEnum.Two, obj.MyEnum);
        }

        [Fact]
        public static void EnumAsInt()
        {
            SimpleTestClass obj = JsonSerializer.ReadString<SimpleTestClass>(s_jsonIntEnum);
            Assert.Equal(SampleEnum.Two, obj.MyEnum);
        }
    }
}
