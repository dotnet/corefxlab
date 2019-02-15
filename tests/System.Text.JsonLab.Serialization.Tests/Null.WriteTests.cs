// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class NullTests
    {
        [Fact]
        public static void DefaultWriteOptions()
        {
            var input = new TestClassWithNull();
            string json = JsonSerializer.WriteString(input);
            Assert.Equal(@"{""MyString"":null}", json);
        }

        [Fact]
        public static void OverrideWriteOnOption()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.SkipNullValuesOnWrite = true;

            var input = new TestClassWithNull();
            string json = JsonSerializer.WriteString(input, options);
            Assert.Equal(@"{}", json);
        }

        [Fact]
        public static void OverrideWriteOnAttribute()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            JsonPropertyAttribute attr = new JsonPropertyAttribute();
            attr.SkipNullValuesOnWrite = true;
            options.AddAttribute(typeof(TestClassWithNull), attr);

            var input = new TestClassWithNull();
            string json = JsonSerializer.WriteString(input, options);
            Assert.Equal(@"{}", json);
        }
    }
}
