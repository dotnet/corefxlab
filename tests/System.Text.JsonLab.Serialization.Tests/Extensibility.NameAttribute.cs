// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

namespace System.Text.Json.Serialization.Tests
{
    public partial class ExtensibilityTests
    {
        [JsonCamelCasingConverter]
        public class SimpleTestClassWithCamelCase : SimpleTestClass
        {
            new public static readonly byte[] s_data = Encoding.UTF8.GetBytes(
                @"{" +
                    @"""myInt16"" : 1" +
                @"}");
        }

        [Fact]
        public static void CamelCaseAttributeDesignTimeFail()
        {
            // This fails because the provided data is pascal-cased
            Assert.Throws<InvalidOperationException>(() => JsonConverter.FromJson<SimpleTestClassWithCamelCase>(SimpleTestClass.s_data));
        }

        [Fact]
        public static void CamelCaseAttributeDesignTime()
        {
            SimpleTestClassWithCamelCase obj = JsonConverter.FromJson<SimpleTestClassWithCamelCase>(SimpleTestClassWithCamelCase.s_data);
            Assert.Equal(obj.MyInt16, 1);
        }


        [Fact]
        public static void CamelCaseAttributeRuntime()
        {
            var settings = new JsonConverterSettings();
            settings.AddAttribute(typeof(SimpleTestClass), new JsonCamelCasingConverterAttribute());

            SimpleTestClass obj = JsonConverter.FromJson<SimpleTestClass>(SimpleTestClassWithCamelCase.s_data, settings);
            Assert.Equal(obj.MyInt16, 1);
        }

        [Fact]
        public static void CamelCaseAttributeInheritanceRuntime()
        {
            var settings = new JsonConverterSettings();
            
            // Add attibute to base class
            settings.AddAttribute(typeof(SimpleTestClass), new JsonCamelCasingConverterAttribute());

            SimpleDerivedTestClass obj = JsonConverter.FromJson<SimpleDerivedTestClass>(SimpleTestClassWithCamelCase.s_data, settings);
            Assert.Equal(obj.MyInt16, 1);
        }

        [Fact]
        public static void OverridePropertyNameAtDesignTime()
        {
            OverridePropertyNameDesignTime_TestClass x = JsonConverter.FromJson<OverridePropertyNameDesignTime_TestClass>(OverridePropertyNameDesignTime_TestClass.DataMatchingAttribute);

            Assert.Equal(x.MyInt16, 1);
        }

        [Fact]
        public static void OverridePropertyNameAtRuntime()
        {
            var settings = new JsonConverterSettings();
            settings.AddAttribute(typeof(OverridePropertyNameRuntime_TestClass), new JsonPropertyNameAttribute("blah"));

            OverridePropertyNameRuntime_TestClass x = JsonConverter.FromJson<OverridePropertyNameRuntime_TestClass>(OverridePropertyNameRuntime_TestClass.Data, settings);

            Assert.Equal(x.MyInt16, 1);
        }

        [Fact]
        public static void OverridePropertyNameAndDesignTimeAttributeAtRuntimeFail()
        {
            Assert.Throws<InvalidOperationException>(() => JsonConverter.FromJson<OverridePropertyNameDesignTime_TestClass>(OverridePropertyNameDesignTime_TestClass.DataNotMatchingAttribute));
        }

        [Fact]
        public static void OverridePropertyNameAndDesignTimeAttributeAtRuntime()
        {
            var settings = new JsonConverterSettings();
            settings.AddAttribute(typeof(OverridePropertyNameDesignTime_TestClass).GetProperty("MyInt16"), new JsonPropertyNameAttribute("blah2"));

            OverridePropertyNameDesignTime_TestClass x = JsonConverter.FromJson<OverridePropertyNameDesignTime_TestClass>(OverridePropertyNameDesignTime_TestClass.DataNotMatchingAttribute, settings);

            Assert.Equal(x.MyInt16, 1);
        }
    }
}
