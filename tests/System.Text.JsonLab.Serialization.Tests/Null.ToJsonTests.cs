// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ToJsonTests
    {
        [Fact]
        public static void DontSerializeNull()
        {
            var input = new TestClassWithNull();
            string json = JsonConverter.ToJsonString(input);
            Assert.Equal("{}", json);
        }

        [Fact]
        public static void SerializeNull()
        {
            JsonConverterSettings settings = new JsonConverterSettings();
            JsonPropertyAttribute attr = new JsonPropertyAttribute();
            attr.SerializeNullValues = true;
            settings.AddAttribute(typeof(TestClassWithNull), attr);

            var input = new TestClassWithNull();
            string json = JsonConverter.ToJsonString(input, settings);
            Assert.Equal(@"{""MyString"":null}", json);
        }
    }
}
