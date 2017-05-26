// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Tests.Resources;
using System.Text.Utf8;
using Xunit;

namespace System.Text.Json.Tests
{
    public class JsonObjectTests
    {
        [Fact]
        public void ParseArray()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.SimpleArrayJson, 60);
            using (var parsedObject = JsonObject.Parse(buffer.AsSpan())) {
                Assert.Equal(2, parsedObject.ArrayLength);

                var phoneNumber = (string)parsedObject[0];
                var age = (int)parsedObject[1];

                Assert.Equal("425-214-3151", phoneNumber);
                Assert.Equal(25, age);
            }
        }

        [Fact]
        public void ParseSimpleObject()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.SimpleObjectJson);
            using (var parsedObject = JsonObject.Parse(buffer.AsSpan())) {
                var age = (int)parsedObject["age"];
                var ageStrring = (string)parsedObject["age"];
                var first = (string)parsedObject["first"];
                var last = (string)parsedObject["last"];
                var phoneNumber = (string)parsedObject["phoneNumber"];
                var street = (string)parsedObject["street"];
                var city = (string)parsedObject["city"];
                var zip = (int)parsedObject["zip"];

                JsonObject age2;
                Assert.True(parsedObject.TryGetValue("age", out age2));
                Assert.Equal((int)age2, 30);

                Assert.Equal(age, 30);
                Assert.Equal(ageStrring, "30");
                Assert.Equal(first, "John");
                Assert.Equal(last, "Smith");
                Assert.Equal(phoneNumber, "425-214-3151");
                Assert.Equal(street, "1 Microsoft Way");
                Assert.Equal(city, "Redmond");
                Assert.Equal(zip, 98052);
            }
        }

        [Fact]
        public void ParseNestedJson()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.ParseJson);
            using (var parsedObject = JsonObject.Parse(buffer.AsSpan())) {

                Assert.Equal(1, parsedObject.ArrayLength);
                var person = parsedObject[0];
                var age = (double)person["age"];
                var first = (string)person["first"];
                var last = (string)person["last"];
                var phoneNums = person["phoneNumbers"];
                Assert.Equal(2, phoneNums.ArrayLength);
                var phoneNum1 = (string)phoneNums[0];
                var phoneNum2 = (string)phoneNums[1];
                var address = person["address"];
                var street = (string)address["street"];
                var city = (string)address["city"];
                var zipCode = (double)address["zip"];

                Assert.Equal(30, age);
                Assert.Equal("John", first);
                Assert.Equal("Smith", last);
                Assert.Equal("425-000-1212", phoneNum1);
                Assert.Equal("425-000-1213", phoneNum2);
                Assert.Equal("1 Microsoft Way", street);
                Assert.Equal("Redmond", city);
                Assert.Equal(98052, zipCode);
                try
                {
                    var _ = person.ArrayLength;
                    throw new Exception("Never get here");
                }
                catch (Exception ex)
                {
                    Assert.IsType<InvalidOperationException>(ex);
                }
                try
                {
                    var _ = phoneNums[2];
                    throw new Exception("Never get here");
                }
                catch(Exception ex)
                {
                    Assert.IsType<IndexOutOfRangeException>(ex);
                }
            }

            // Exceptional use case
            //var a = x[1];                             // IndexOutOfRangeException
            //var b = x["age"];                         // NullReferenceException
            //var c = person[0];                        // NullReferenceException
            //var d = address["cit"];                   // KeyNotFoundException
            //var e = address[0];                       // NullReferenceException
            //var f = (double)address["city"];          // InvalidCastException
            //var g = (bool)address["city"];            // InvalidCastException
            //var h = (string)address["zip"];           // Integer converted to string implicitly
            //var i = (string)person["phoneNumbers"];   // InvalidCastException
            //var j = (string)person;                   // InvalidCastException
        }

        [Fact]
        public void ParseBoolean()
        {
            var buffer = StringToUtf8BufferWithEmptySpace("[true,false]", 60);
            using (var parsedObject = JsonObject.Parse(buffer.AsSpan())) {
                var first = (bool)parsedObject[0];
                var second = (bool)parsedObject[1];
                Assert.Equal(true, first);
                Assert.Equal(false, second);
            }
        }

        private static ArraySegment<byte> StringToUtf8BufferWithEmptySpace(string testString, int emptySpaceSize = 2048)
        {
            var utf8Bytes = new Utf8String(testString).Bytes;
            var buffer = new byte[utf8Bytes.Length + emptySpaceSize];
            utf8Bytes.CopyTo(buffer);
            return new ArraySegment<byte>(buffer, 0, utf8Bytes.Length);
        }
    }
}
