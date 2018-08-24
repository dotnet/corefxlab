// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.JsonLab.Tests.Resources;
using System.Text.Utf8;
using Xunit;

namespace System.Text.JsonLab.Tests
{
    public class JsonObjectTests
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { TestCaseType.Basic, TestJson.BasicJson},
                    new object[] { TestCaseType.BasicLargeNum, TestJson.BasicJsonWithLargeNum}, // Json.NET treats numbers starting with 0 as octal (0425 becomes 277)
                    new object[] { TestCaseType.BroadTree, TestJson.BroadTree}, // \r\n behavior is different between Json.NET and JsonLab
                    new object[] { TestCaseType.DeepTree, TestJson.DeepTree},
                    new object[] { TestCaseType.FullSchema1, TestJson.FullJsonSchema1},
                    //new object[] { TestCaseType.FullSchema2, TestJson.FullJsonSchema2},   // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { TestCaseType.HelloWorld, TestJson.HelloWorld},
                    new object[] { TestCaseType.LotsOfNumbers, TestJson.LotsOfNumbers},
                    new object[] { TestCaseType.LotsOfStrings, TestJson.LotsOfStrings},
                    new object[] { TestCaseType.ProjectLockJson, TestJson.ProjectLockJson},
                    //new object[] { TestCaseType.SpecialStrings, TestJson.JsonWithSpecialStrings},    // Behavior of escaping is different between Json.NET and JsonLab
                    //new object[] { TestCaseType.SpecialNumForm, TestJson.JsonWithSpecialNumFormat},    // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { TestCaseType.Json400B, TestJson.Json400B},
                    new object[] { TestCaseType.Json4KB, TestJson.Json4KB},
                    new object[] { TestCaseType.Json40KB, TestJson.Json40KB},
                    new object[] { TestCaseType.Json400KB, TestJson.Json400KB}
                };
            }
        }

        // TestCaseType is only used to give the json strings a descriptive name within the unit tests.
        public enum TestCaseType
        {
            HelloWorld,
            Basic,
            BasicLargeNum,
            SpecialNumForm,
            SpecialStrings,
            ProjectLockJson,
            FullSchema1,
            FullSchema2,
            DeepTree,
            BroadTree,
            LotsOfNumbers,
            LotsOfStrings,
            Json400B,
            Json4KB,
            Json40KB,
            Json400KB,
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public void ParseJson(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var parser = new JsonParser(dataUtf8);
            JsonObject obj = parser.Parse();

            string actual = obj.PrintJson();
            string database1 = obj.PrintDatabase();

            // Change casing to match what JSON.NET does.
            actual = actual.Replace("true", "True").Replace("false", "False");

            TextReader reader = new StringReader(jsonString);
            string expected = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expected, actual);

            if (type == TestCaseType.Json400KB)
            {
                string database = obj.PrintDatabase();
                var lookup = new Utf8Span("email");
                JsonObject withinArray = obj[5];
                database = withinArray.PrintDatabase();
                JsonObject withinObject = withinArray[lookup];
                database = withinObject.PrintDatabase();
                Utf8Span email = (Utf8Span)withinObject;
                Assert.Equal("kentlester@solgan.com", email.ToString());
            }
        }

        [Theory]
        [InlineData("[{\"arrayWithObjects\":[\"text\",14,[],null,false,{},{\"time\":24},[\"1\",\"2\",\"3\"]]}]")]
        [InlineData("[{},{},{},{},{},{},{},{},{},{},{},{},{},{},{},{},{},{}]")]
        [InlineData("[{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}},{\"a\":{}}]")]
        [InlineData("{\"a\":\"b\"}")]
        [InlineData("{}")]
        [InlineData("[]")]
        public void CustomParseJson(string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var parser = new JsonParser(dataUtf8);
            JsonObject obj = parser.Parse();

            string actual = obj.PrintJson();

            // Change casing to match what JSON.NET does.
            actual = actual.Replace("true", "True").Replace("false", "False");

            TextReader reader = new StringReader(jsonString);
            string expected = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ChangeEntryPointLibraryName()
        {
            string depsJson = TestJson.DepsJsonSignalR;
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(depsJson);
            var parser = new JsonParser(dataUtf8);
            JsonObject obj = parser.Parse();

            var targetsString = new Utf8Span("targets");
            var librariesString = new Utf8Span("libraries");

            JsonObject targets = obj[targetsString];

            Assert.True(targets.TryGetChild(out JsonObject child));
            Assert.True(child.TryGetChild(out child));
            obj.Remove(child);

            JsonObject libraries = obj[librariesString];
            Assert.True(libraries.TryGetChild(out child));
            obj.Remove(child);

            string actual = obj.PrintJson();

            // Change casing to match what JSON.NET does.
            actual = actual.Replace("true", "True").Replace("false", "False");

            string expected = ChangeEntryPointLibraryNameExpected();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ParseArray()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.SimpleArrayJson, 60);

            var parser = new JsonParser(buffer);
            JsonObject parsedObject = parser.Parse();
            try
            {
                Assert.Equal(2, parsedObject.ArrayLength);

                var phoneNumber = (string)parsedObject[0];
                var age = (int)parsedObject[1];

                Assert.Equal("425-214-3151", phoneNumber);
                Assert.Equal(25, age);
            }
            finally
            {
                parsedObject.Dispose();
            }
        }

        [Fact]
        public void ParseSimpleObject()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.SimpleObjectJson);
            var parser = new JsonParser(buffer);
            JsonObject parsedObject = parser.Parse();
            try
            {
                var age = (int)parsedObject["age"];
                var ageStrring = (string)parsedObject["age"];
                var first = (string)parsedObject["first"];
                var last = (string)parsedObject["last"];
                var phoneNumber = (string)parsedObject["phoneNumber"];
                var street = (string)parsedObject["street"];
                var city = (string)parsedObject["city"];
                var zip = (int)parsedObject["zip"];

                Assert.True(parsedObject.TryGetValue("age", out JsonObject age2));
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
            finally
            {
                parsedObject.Dispose();
            }
        }

        [Fact]
        public void ParseNestedJson()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.ParseJson);
            var parser = new JsonParser(buffer);
            JsonObject parsedObject = parser.Parse();
            try
            {
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
                catch (Exception ex)
                {
                    Assert.IsType<IndexOutOfRangeException>(ex);
                }
            }
            finally
            {
                parsedObject.Dispose();
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
            var parser = new JsonParser(buffer);
            JsonObject parsedObject = parser.Parse();
            try
            {
                var first = (bool)parsedObject[0];
                var second = (bool)parsedObject[1];
                Assert.Equal(true, first);
                Assert.Equal(false, second);
            }
            finally
            {
                parsedObject.Dispose();
            }
        }

        private static ArraySegment<byte> StringToUtf8BufferWithEmptySpace(string testString, int emptySpaceSize = 2048)
        {
            var utf8Bytes = new Utf8Span(testString).Bytes;
            var buffer = new byte[utf8Bytes.Length + emptySpaceSize];
            utf8Bytes.CopyTo(buffer);
            return new ArraySegment<byte>(buffer, 0, utf8Bytes.Length);
        }

        private static string ChangeEntryPointLibraryNameExpected()
        {
            JToken deps = JObject.Parse(TestJson.DepsJsonSignalR);

            foreach (JProperty target in deps["targets"])
            {
                JProperty targetLibrary = target.Value.Children<JProperty>().FirstOrDefault();
                if (targetLibrary == null)
                {
                    continue;
                }
                targetLibrary.Remove();
            }

            JProperty library = deps["libraries"].Children<JProperty>().First();
            library.Remove();

            string json = deps.ToString(Newtonsoft.Json.Formatting.None);

            TextReader reader = new StringReader(json);

            return JsonTestHelper.NewtonsoftReturnStringHelper(reader);
        }
    }
}
