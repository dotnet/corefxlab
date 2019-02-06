// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class FromJsonTests
    {
        [Fact]
        public static void VerifyNull()
        {
            {
                TestClassWithNull obj = JsonConverter.FromJson<TestClassWithNull>(TestClassWithNull.s_json);
                obj.Verify();
            }

            {
                JsonConverterSettings settings = new JsonConverterSettings();
                JsonPropertyAttribute attr = new JsonPropertyAttribute();
                attr.DeserializeNullValues = false;
                settings.AddAttribute(typeof(TestClassWithNull), attr);

                TestClassWithNull obj = JsonConverter.FromJson<TestClassWithNull>(TestClassWithNull.s_json, settings);
                obj.Verify();
            }

            {
                JsonConverterSettings settings = new JsonConverterSettings();
                JsonPropertyAttribute attr = new JsonPropertyAttribute();
                attr.DeserializeNullValues = true;
                settings.AddAttribute(typeof(TestClassWithNull), attr);

                TestClassWithNull obj = JsonConverter.FromJson<TestClassWithNull>(TestClassWithNull.s_json, settings);
                obj.Verify();
            }

            // Now try class with a default value but don't overwrite it.
            {
                TestClassWithNullButInitialized obj = JsonConverter.FromJson<TestClassWithNullButInitialized>(TestClassWithNullButInitialized.s_json);
                Assert.Equal("Hello", obj.MyString);
                Assert.Equal(1, obj.MyInt);
            }

            // Same but use attr.DeserializeNullValues = false
            {
                JsonConverterSettings settings = new JsonConverterSettings();
                JsonPropertyAttribute attr = new JsonPropertyAttribute();
                attr.DeserializeNullValues = false;
                settings.AddAttribute(typeof(TestClassWithNullButInitialized), attr);

                // Should skip the setting since the json value was null.
                TestClassWithNullButInitialized obj = JsonConverter.FromJson<TestClassWithNullButInitialized>(TestClassWithNullButInitialized.s_json, settings);
                Assert.Equal("Hello", obj.MyString);
                Assert.Equal(1, obj.MyInt);
            }

            // Now try class with a default value and overrite it (the default)
            {
                JsonConverterSettings settings = new JsonConverterSettings();
                JsonPropertyAttribute attr = new JsonPropertyAttribute();
                attr.DeserializeNullValues = true;
                settings.AddAttribute(typeof(TestClassWithNullButInitialized), attr);

                // Should skip the setting since the json value was null.
                TestClassWithNullButInitialized obj = JsonConverter.FromJson<TestClassWithNullButInitialized>(TestClassWithNullButInitialized.s_json, settings);
                Assert.Equal(null, obj.MyString);
                Assert.Equal(null, obj.MyInt);
            }
        }
    }
}
