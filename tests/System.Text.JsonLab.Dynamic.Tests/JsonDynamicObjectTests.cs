// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Text.Formatting;
using System.Text.Utf8;
using Xunit;

namespace System.Text.JsonLab.Dynamic.Tests
{
    public class JsonDynamicObjectTests
    {
        //[Fact(Skip= "System.TypeLoadException : A value type containing a by-ref instance field, such as Span<T>, cannot be used as the type for a class instance field.")]
        public void DynamicArrayLazy()
        {
            using (dynamic json = JsonLazyDynamicObject.Parse(new Utf8Span("[true, false]"))) {
                Assert.Equal(true, json[0]);
                Assert.Equal(false, json[1]);
            }
        }

        //[Fact(Skip = "System.TypeLoadException : A value type containing a by-ref instance field, such as Span<T>, cannot be used as the type for a class instance field.")]
        public void NestedEagerReadLazy()
        {
            using(dynamic json = JsonLazyDynamicObject.Parse(new Utf8Span("{ \"FirstName\": \"John\", \"LastName\": \"Smith\", \"Address\": { \"Street\": \"21 2nd Street\", \"City\": \"New York\", \"State\": \"NY\", \"Zip\": \"10021-3100\" }, \"IsAlive\": true, \"Age\": 25, \"Spouse\":null }"))){
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
            dynamic json = JsonDynamicObject.Parse(new Utf8Span("{ \"FirstName\": \"John\", \"LastName\": \"Smith\", \"Address\": { \"Street\": \"21 2nd Street\", \"City\": \"New York\", \"State\": \"NY\", \"Zip\": \"10021-3100\" }, \"IsAlive\": true, \"Age\": 25, \"Spouse\":null }"));
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

        [Fact]
        public void NestedEagerWrite()
        {
            var jsonText = new Utf8Span("{\"FirstName\":\"John\",\"LastName\":\"Smith\",\"Address\":{\"Street\":\"21 2nd Street\",\"City\":\"New York\",\"State\":\"NY\",\"Zip\":\"10021-3100\"},\"IsAlive\":true,\"Age\":25,\"Spouse\":null}");
            JsonDynamicObject json = JsonDynamicObject.Parse(jsonText, 100);
            var formatter = new ArrayFormatter(1024, SymbolTable.InvariantUtf8);
            formatter.Append(json);
            var formattedText = new Utf8Span(formatter.Formatted);

            // The follwoing check only works given the current implmentation of Dictionary.
            // If the implementation changes, the properties might round trip to different places in the JSON text.
            Assert.Equal(jsonText.ToString(), formattedText.ToString());
        }

        [Fact]
        public void EagerWrite()
        {
            dynamic json = new JsonDynamicObject();
            json.First = "John";

            var formatter = new ArrayFormatter(1024, SymbolTable.InvariantUtf8);
            formatter.Append((JsonDynamicObject)json);
            var formattedText = new Utf8Span(formatter.Formatted);
            Assert.Equal("{\"First\":\"John\"}", formattedText.ToString());
        }

        [Fact]
        public void NonAllocatingRead()
        {
            JsonDynamicObject json = JsonDynamicObject.Parse(new Utf8Span("{\"First\":\"John\",\"Age\":25}"));

            Assert.Equal("John", json.First().ToString());
            Assert.Equal(25U, json.Age());
        }

        [Fact]
        public void DeserializeWithUtf8Strings()
        {
            string str = "{\"RememberMe\":true,\"Email\":\"name.familyname@not.com\",\"Password\":\"abcdefgh123456!@\"}";
            byte[] data = Encoding.UTF8.GetBytes(str);

            LoginViewModel model = JsonSerializer.Deserialize<LoginViewModel>(data);

            Assert.Equal(new Utf8String("name.familyname@not.com"), model.Email);
            Assert.Equal(new Utf8String("abcdefgh123456!@"), model.Password);
            Assert.Equal(true, model.RememberMe);
        }

        [Fact]
        public void DeserializeWithoutUtf8Strings()
        {
            string str = "{\"Email1\":1,\"Email2\":2,\"Email3\":3,\"RememberMe\":true}";
            byte[] data = Encoding.UTF8.GetBytes(str);

            LoginViewModel_NoUtf8 model = JsonSerializer.Deserialize<LoginViewModel_NoUtf8>(data);

            Assert.Equal(1, model.Email1);
            Assert.Equal(2, model.Email2);
            Assert.Equal(3, model.Email3);
            Assert.Equal(true, model.RememberMe);
        }
    }

    [Serializable]
    public class LoginViewModel
    {
        public virtual Utf8String Email { get; set; }
        public virtual Utf8String Password { get; set; }
        public virtual bool RememberMe { get; set; }
    }

    [Serializable]
    public class LoginViewModel_NoUtf8
    {
        public virtual int Email1 { get; set; }
        public virtual int Email2 { get; set; }
        public virtual int Email3 { get; set; }
        public virtual bool RememberMe { get; set; }
    }

    static class SchemaExtensions
    {
        static readonly string s_first = "First";
        static readonly string s_age = "Age";

        public static Utf8Span First(this JsonDynamicObject json)
        {
            if (json.TryGetString(new Utf8String(s_first), out Utf8Span value))
            {
                return value;
            }
            throw new InvalidOperationException();
        }

        public static uint Age(this JsonDynamicObject json)
        {
            if (json.TryGetUInt32(new Utf8String(s_age), out uint value))
            {
                return value;
            }
            throw new InvalidOperationException();
        }
    }
}
