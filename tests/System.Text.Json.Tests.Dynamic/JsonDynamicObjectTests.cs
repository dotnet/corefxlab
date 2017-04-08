// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Formatting;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Dynamic.Tests
{
    public class JsonDynamicObjectTests
    {
        //[Fact(Skip= "System.TypeLoadException : A value type containing a by-ref instance field, such as Span<T>, cannot be used as the type for a class instance field.")]
        public void DynamicArrayLazy()
        {
            using (dynamic json = JsonLazyDynamicObject.Parse(new Utf8String("[true, false]"))) {
                Assert.Equal(true, json[0]);
                Assert.Equal(false, json[1]);
            }
        }

        //[Fact(Skip = "System.TypeLoadException : A value type containing a by-ref instance field, such as Span<T>, cannot be used as the type for a class instance field.")]
        public void NestedEagerReadLazy()
        {
            using(dynamic json = JsonLazyDynamicObject.Parse(new Utf8String("{ \"FirstName\": \"John\", \"LastName\": \"Smith\", \"Address\": { \"Street\": \"21 2nd Street\", \"City\": \"New York\", \"State\": \"NY\", \"Zip\": \"10021-3100\" }, \"IsAlive\": true, \"Age\": 25, \"Spouse\":null }"))){
                Assert.Equal("John", json.FirstName);
                Assert.Equal("Smith", json.LastName);
                Assert.Equal(true, json.IsAlive);
                Assert.Equal(25, json.Age);
                Assert.Equal(null, json.Spouse);

                dynamic address = json.Address;
                Assert.Equal("21 2nd Street", address.Street);
                Assert.Equal("New York", address.City);
                Assert.Equal("NY", address.State);
                Assert.Equal("10021-3100", address.Zip);
            }
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.IEquatable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void NestedEagerRead()
        {
            dynamic json = JsonDynamicObject.Parse(new Utf8String("{ \"FirstName\": \"John\", \"LastName\": \"Smith\", \"Address\": { \"Street\": \"21 2nd Street\", \"City\": \"New York\", \"State\": \"NY\", \"Zip\": \"10021-3100\" }, \"IsAlive\": true, \"Age\": 25, \"Spouse\":null }"));
            Assert.Equal("John", json.FirstName);
            Assert.Equal("Smith", json.LastName);
            Assert.Equal(true, json.IsAlive);
            Assert.Equal(25, json.Age);
            Assert.Equal(null, json.Spouse);
            Assert.Equal(6, json.Count);

            dynamic address = json.Address;
            Assert.Equal("21 2nd Street", address.Street);
            Assert.Equal("New York", address.City);
            Assert.Equal("NY", address.State);
            Assert.Equal("10021-3100", address.Zip);
            Assert.Equal(4, address.Count);
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.IEquatable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void NestedEagerWrite()
        {
            var jsonText = new Utf8String("{\"FirstName\":\"John\",\"LastName\":\"Smith\",\"Address\":{\"Street\":\"21 2nd Street\",\"City\":\"New York\",\"State\":\"NY\",\"Zip\":\"10021-3100\"},\"IsAlive\":true,\"Age\":25,\"Spouse\":null}");
            JsonDynamicObject json = JsonDynamicObject.Parse(jsonText, 100);
            var formatter = new ArrayFormatter(1024, TextEncoder.Utf8);
            formatter.Append(json);
            var formattedText = new Utf8String(formatter.Formatted);

            // The follwoing check only works given the current implmentation of Dictionary.
            // If the implementation changes, the properties might round trip to different places in the JSON text.
            Assert.Equal(jsonText.ToString(), formattedText.ToString()); 
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.IEquatable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void EagerWrite()
        {
            dynamic json = new JsonDynamicObject();
            json.First = "John";

            var formatter = new ArrayFormatter(1024, TextEncoder.Utf8);
            formatter.Append((JsonDynamicObject)json);
            Assert.Equal("{\"First\":\"John\"}", formatter.Formatted.ToString());
        }

        //[Fact(Skip = "System.TypeLoadException : The generic type 'System.IEquatable`1' was used with an invalid instantiation in assembly 'System.Private.CoreLib")]
        public void NonAllocatingRead()
        {
            JsonDynamicObject json = JsonDynamicObject.Parse(new Utf8String("{\"First\":\"John\",\"Age\":25}"));

            Assert.Equal("John", json.First().ToString());
            Assert.Equal(25U, json.Age());
        }
    }

    static class SchemaExtensions
    {
        static readonly string s_first = "First";
        static readonly string s_age = "Age";

        public static Utf8String First(this JsonDynamicObject json)
        {
            Utf8String value;
            if(json.TryGetString(new Utf8String(s_first), out value)) {
                return value;
            }
            throw new InvalidOperationException();
        }

        public static uint Age(this JsonDynamicObject json)
        {
            uint value;
            if (json.TryGetUInt32(new Utf8String(s_age), out value)) {
                return value;
            }
            throw new InvalidOperationException();
        }
    }
}