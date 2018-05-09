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
        public void Deserialize()
        {
            //string str = "{\"RememberMe\":true,\"Email\":\"name.familyname@not.com\",\"Password\":\"abcdefgh123456!@\"}";
            //byte[] data = Encoding.UTF8.GetBytes(str);

            string str = "{\"Email\":1,\"Email1\":2,\"Email2\":3,\"Email3\":4,\"Email4\":5,\"Email5\":6,\"Email6\":7,\"Email7\":8,\"Email8\":9,\"Email9\":10,\"Email10\":11,\"Email11\":12,\"Email12\":13,\"Email13\":14,\"Email14\":15,\"Email15\":16,\"Email16\":17,\"Email17\":18,\"Email18\":19,\"Email19\":20,\"Email20\":21,\"Email21\":22,\"RememberMe\":true}";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);

            LoginViewModel2 model = JsonDynamicObject.Deserialize<LoginViewModel2>(data);

            Assert.Equal(1, model.Email);
            Assert.Equal(2, model.Email1);
            Assert.Equal(3, model.Email2);
            Assert.Equal(21, model.Email20);
            Assert.Equal(22, model.Email21);
            Assert.Equal(true, model.RememberMe);

            LoginViewModel2 model2 = JsonDynamicObject.Deserialize<LoginViewModel2>(data);
            Assert.Equal(20, model2.Email19);

            /*LoginViewModel model = JsonDynamicObject.Deserialize<LoginViewModel>(data);

            Assert.Equal("name.familyname@not.com", model.Email);
            Assert.Equal("abcdefgh123456!@", model.Password);
            Assert.Equal(true, model.RememberMe);*/
        }
    }

    [Serializable]
    public class LoginViewModel
    {
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual bool RememberMe { get; set; }
    }

    [Serializable]
    public class LoginViewModel2
    {
        public virtual int Email { get; set; }
        public virtual int Email1 { get; set; }
        public virtual int Email2 { get; set; }
        public virtual int Email3 { get; set; }
        public virtual int Email4 { get; set; }
        public virtual int Email5 { get; set; }
        public virtual int Email6 { get; set; }
        public virtual int Email7 { get; set; }
        public virtual int Email8 { get; set; }
        public virtual int Email9 { get; set; }
        public virtual int Email10 { get; set; }
        public virtual int Email11 { get; set; }
        public virtual int Email12 { get; set; }
        public virtual int Email13 { get; set; }
        public virtual int Email14 { get; set; }
        public virtual int Email15 { get; set; }
        public virtual int Email16 { get; set; }
        public virtual int Email17 { get; set; }
        public virtual int Email18 { get; set; }
        public virtual int Email19 { get; set; }
        public virtual int Email20 { get; set; }
        public virtual int Email21 { get; set; }
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
