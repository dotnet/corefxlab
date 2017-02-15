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
        [Fact]
        public void DynamicArrayLazy()
        {
            using (dynamic json = JsonLazyDynamicObject.Parse(new Utf8String("[true, false]"))) {
                Assert.Equal(true, json[0]);
                Assert.Equal(false, json[1]);
            }
        }

        [Fact]
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

        [Fact]
        public void NestedEagerRead()
        {
            dynamic json = JsonDynamicObject.Parse(new Utf8String("{ \"FirstName\": \"John\", \"LastName\": \"Smith\", \"Address\": { \"Street\": \"21 2nd Street\", \"City\": \"New York\", \"State\": \"NY\", \"Zip\": \"10021-3100\" }, \"IsAlive\": true, \"Age\": 25, \"Spouse\":null }"));
            Assert.Equal(new Utf8String("John"), json.FirstName);
            Assert.Equal(new Utf8String("Smith"), json.LastName);
            Assert.Equal(true, json.IsAlive);
            Assert.Equal(25, json.Age);
            Assert.Equal(null, json.Spouse);
            Assert.Equal(6, json.Count);

            dynamic address = json.Address;
            Assert.Equal(new Utf8String("21 2nd Street"), address.Street);
            Assert.Equal(new Utf8String("New York"), address.City);
            Assert.Equal(new Utf8String("NY"), address.State);
            Assert.Equal(new Utf8String("10021-3100"), address.Zip);
            Assert.Equal(4, address.Count);
        }

        [Fact]
        public void NestedEagerWrite()
        {
            var jsonText = new Utf8String("{\"FirstName\":\"John\",\"LastName\":\"Smith\",\"Address\":{\"Street\":\"21 2nd Street\",\"City\":\"New York\",\"State\":\"NY\",\"Zip\":\"10021-3100\"},\"IsAlive\":true,\"Age\":25,\"Spouse\":null}");
            JsonDynamicObject json = JsonDynamicObject.Parse(jsonText, 100);
            var formatter = new ArrayFormatter(1024, TextEncoder.InvariantUtf8);
            formatter.Append(json);
            var formattedText = new Utf8String(formatter.Formatted);

            // The follwoing check only works given the current implmentation of Dictionary.
            // If the implementation changes, the properties might round trip to different places in the JSON text.
            Assert.Equal(jsonText, formattedText); 
        }

        [Fact]
        public void EagerWrite()
        {
            dynamic json = new JsonDynamicObject();
            json.First = "John";

            var formatter = new ArrayFormatter(1024, TextEncoder.InvariantUtf8);
            formatter.Append((JsonDynamicObject)json);
            var formattedText = new Utf8String(formatter.Formatted);
            Assert.Equal(new Utf8String("{\"First\":\"John\"}"), formattedText);
        }

        [Fact]
        public void NonAllocatingRead()
        {
            var jsonText = new Utf8String("{\"First\":\"John\",\"Age\":25}");
            JsonDynamicObject json = JsonDynamicObject.Parse(jsonText);

            Assert.Equal(new Utf8String("John"), json.First());
            Assert.Equal(25U, json.Age());
        }
    }

    static class SchemaExtensions
    {
        static readonly Utf8String s_first = new Utf8String("First");
        static readonly Utf8String s_age = new Utf8String("Age");

        public static Utf8String First(this JsonDynamicObject json)
        {
            Utf8String value;
            if(json.TryGetString(s_first, out value)) {
                return value;
            }
            throw new InvalidOperationException();
        }

        public static uint Age(this JsonDynamicObject json)
        {
            uint value;
            if (json.TryGetUInt32(s_age, out value)) {
                return value;
            }
            throw new InvalidOperationException();
        }
    }
}