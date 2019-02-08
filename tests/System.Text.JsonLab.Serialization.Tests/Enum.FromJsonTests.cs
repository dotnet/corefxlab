// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class FromJsonTests
    {
        public static readonly string s_JsonStringEnum =
                @"{" +
                @"""MyEnum"" : ""Two""" +
                @"}";

        public static readonly string s_JsonIntEnum =
                @"{" +
                @"""MyEnum"" : 2" +
                @"}";

        [Fact]
        public static void EnumAsStringFail()
        {
            Assert.Throws<InvalidOperationException>(() => JsonConverter.FromJson<SimpleTestClass>(s_JsonStringEnum));
        }

        [Fact]
        public static void EnumAsString()
        {
            var settings = new JsonConverterSettings();
            settings.AddAttribute(typeof(SimpleTestClass), new JsonEnumConverterAttribute(treatAsString: true));

            SimpleTestClass obj = JsonConverter.FromJson<SimpleTestClass>(s_JsonStringEnum, settings);
            Assert.Equal(SampleEnum.Two, obj.MyEnum);
        }

        [Fact]
        public static void EnumAsInt()
        {
            SimpleTestClass obj = JsonConverter.FromJson<SimpleTestClass>(s_JsonIntEnum);
            Assert.Equal(SampleEnum.Two, obj.MyEnum);
        }
    }
}
