// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Formatting;
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
                    new object[] { true, TestCaseType.Basic, TestJson.BasicJson},
                    new object[] { true, TestCaseType.BasicLargeNum, TestJson.BasicJsonWithLargeNum}, // Json.NET treats numbers starting with 0 as octal (0425 becomes 277)
                    new object[] { true, TestCaseType.BroadTree, TestJson.BroadTree}, // \r\n behavior is different between Json.NET and JsonLab
                    new object[] { true, TestCaseType.DeepTree, TestJson.DeepTree},
                    new object[] { true, TestCaseType.FullSchema1, TestJson.FullJsonSchema1},
                    //new object[] { true, TestCaseType.FullSchema2, TestJson.FullJsonSchema2},   // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { true, TestCaseType.HelloWorld, TestJson.HelloWorld},
                    new object[] { true, TestCaseType.LotsOfNumbers, TestJson.LotsOfNumbers},
                    new object[] { true, TestCaseType.LotsOfStrings, TestJson.LotsOfStrings},
                    new object[] { true, TestCaseType.ProjectLockJson, TestJson.ProjectLockJson},
                    //new object[] { true, TestCaseType.SpecialStrings, TestJson.JsonWithSpecialStrings},    // Behavior of escaping is different between Json.NET and JsonLab
                    //new object[] { true, TestCaseType.SpecialNumForm, TestJson.JsonWithSpecialNumFormat},    // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { true, TestCaseType.Json400B, TestJson.Json400B},
                    new object[] { true, TestCaseType.Json4KB, TestJson.Json4KB},
                    new object[] { true, TestCaseType.Json40KB, TestJson.Json40KB},
                    new object[] { true, TestCaseType.Json400KB, TestJson.Json400KB},

                    new object[] { false, TestCaseType.Basic, TestJson.BasicJson},
                    new object[] { false, TestCaseType.BasicLargeNum, TestJson.BasicJsonWithLargeNum}, // Json.NET treats numbers starting with 0 as octal (0425 becomes 277)
                    new object[] { false, TestCaseType.BroadTree, TestJson.BroadTree}, // \r\n behavior is different between Json.NET and JsonLab
                    new object[] { false, TestCaseType.DeepTree, TestJson.DeepTree},
                    new object[] { false, TestCaseType.FullSchema1, TestJson.FullJsonSchema1},
                    //new object[] { false, TestCaseType.FullSchema2, TestJson.FullJsonSchema2},   // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { false, TestCaseType.HelloWorld, TestJson.HelloWorld},
                    new object[] { false, TestCaseType.LotsOfNumbers, TestJson.LotsOfNumbers},
                    new object[] { false, TestCaseType.LotsOfStrings, TestJson.LotsOfStrings},
                    new object[] { false, TestCaseType.ProjectLockJson, TestJson.ProjectLockJson},
                    //new object[] { false, TestCaseType.SpecialStrings, TestJson.JsonWithSpecialStrings},    // Behavior of escaping is different between Json.NET and JsonLab
                    //new object[] { false, TestCaseType.SpecialNumForm, TestJson.JsonWithSpecialNumFormat},    // Behavior of E-notation is different between Json.NET and JsonLab
                    new object[] { false, TestCaseType.Json400B, TestJson.Json400B},
                    new object[] { false, TestCaseType.Json4KB, TestJson.Json4KB},
                    new object[] { false, TestCaseType.Json40KB, TestJson.Json40KB},
                    new object[] { false, TestCaseType.Json400KB, TestJson.Json400KB}
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

        private string ReadHelloWorld(JToken obj)
        {
            string message = (string)obj["message"];
            return message;
        }

        private string ReadJson400KB(JToken obj)
        {
            var sb = new StringBuilder();
            foreach (JToken token in obj)
            {
                sb.Append((string)token["_id"]);
                sb.Append((int)token["index"]);
                sb.Append((string)token["guid"]);
                sb.Append((bool)token["isActive"]);
                sb.Append((string)token["balance"]);
                sb.Append((string)token["picture"]);
                sb.Append((int)token["age"]);
                sb.Append((string)token["eyeColor"]);
                sb.Append((string)token["name"]);
                sb.Append((string)token["gender"]);
                sb.Append((string)token["company"]);
                sb.Append((string)token["email"]);
                sb.Append((string)token["phone"]);
                sb.Append((string)token["address"]);
                sb.Append((string)token["about"]);
                sb.Append((string)token["registered"]);
                sb.Append((double)token["latitude"]);
                sb.Append((double)token["longitude"]);

                JToken tags = token["tags"];
                foreach (JToken tag in tags)
                {
                    sb.Append((string)tag);
                }
                JToken friends = token["friends"];
                foreach (JToken friend in friends)
                {
                    sb.Append((int)friend["id"]);
                    sb.Append((string)friend["name"]);
                }
                sb.Append((string)token["greeting"]);
                sb.Append((string)token["favoriteFruit"]);

            }
            return sb.ToString();
        }

        private string ReadHelloWorld(JsonObject obj)
        {
            string message = (string)obj["message"];
            return message;
        }

        private string ReadJson400KB(JsonObject obj)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < obj.ArrayLength; i++)
            {
                sb.Append((string)obj[i]["_id"]);
                sb.Append((int)obj[i]["index"]);
                sb.Append((string)obj[i]["guid"]);
                sb.Append((bool)obj[i]["isActive"]);
                sb.Append((string)obj[i]["balance"]);
                sb.Append((string)obj[i]["picture"]);
                sb.Append((int)obj[i]["age"]);
                sb.Append((string)obj[i]["eyeColor"]);
                sb.Append((string)obj[i]["name"]);
                sb.Append((string)obj[i]["gender"]);
                sb.Append((string)obj[i]["company"]);
                sb.Append((string)obj[i]["email"]);
                sb.Append((string)obj[i]["phone"]);
                sb.Append((string)obj[i]["address"]);
                sb.Append((string)obj[i]["about"]);
                sb.Append((string)obj[i]["registered"]);
                sb.Append((double)obj[i]["latitude"]);
                sb.Append((double)obj[i]["longitude"]);

                JsonObject tags = obj[i]["tags"];
                for (int j = 0; j < tags.ArrayLength; j++)
                {
                    sb.Append((string)tags[j]);
                }
                JsonObject friends = obj[i]["friends"];
                for (int j = 0; j < friends.ArrayLength; j++)
                {
                    sb.Append((int)friends[j]["id"]);
                    sb.Append((string)friends[j]["name"]);
                }
                sb.Append((string)obj[i]["greeting"]);
                sb.Append((string)obj[i]["favoriteFruit"]);
            }
            return sb.ToString();
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public void ParseJson(bool compactData, TestCaseType type, string jsonString)
        {
            // Remove all formatting/indendation
            if (compactData)
            {
                using (JsonTextReader jsonReader = new JsonTextReader(new StringReader(jsonString)))
                {
                    jsonReader.FloatParseHandling = FloatParseHandling.Decimal;
                    JToken jtoken = JToken.ReadFrom(jsonReader);
                    var stringWriter = new StringWriter();
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jtoken.WriteTo(jsonWriter);
                        jsonString = stringWriter.ToString();
                    }
                }
            }

            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            JsonObject obj = JsonObject.Parse(dataUtf8);

            var stream = new MemoryStream(dataUtf8);
            var streamReader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            using (JsonTextReader jsonReader = new JsonTextReader(streamReader))
            {
                JToken jtoken = JToken.ReadFrom(jsonReader);

                string expectedString = "";
                string actualString = "";

                if (type == TestCaseType.Json400KB)
                {
                    expectedString = ReadJson400KB(jtoken);
                    actualString = ReadJson400KB(obj);
                }
                else if (type == TestCaseType.HelloWorld)
                {
                    expectedString = ReadHelloWorld(jtoken);
                    actualString = ReadHelloWorld(obj);
                }
                Assert.Equal(expectedString, actualString);
            }

            string actual = obj.PrintJson();

            // Change casing to match what JSON.NET does.
            actual = actual.Replace("true", "True").Replace("false", "False");

            TextReader reader = new StringReader(jsonString);
            string expected = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expected, actual);

            if (compactData)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output);

                jsonUtf8.Write(obj);
                jsonUtf8.Flush();

                ArraySegment<byte> formatted = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

                Assert.Equal(jsonString, actualStr);
            }

            obj.Dispose();
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
            JsonObject obj = JsonObject.Parse(dataUtf8);

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
            JsonObject obj = JsonObject.Parse(dataUtf8);

            var targetsString = new Utf8Span("targets");
            var librariesString = new Utf8Span("libraries");

            JsonObject targets = obj[targetsString];

            foreach (JsonObject target in targets)
            {
                Assert.True(target.TryGetChild(out JsonObject firstChild));
                obj.Remove(firstChild);
            }

            JsonObject libraries = obj[librariesString];
            Assert.True(libraries.TryGetChild(out JsonObject child));
            obj.Remove(child);

            string expected = ChangeEntryPointLibraryNameExpected();

            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output);

            jsonUtf8.Write(obj);
            jsonUtf8.Flush();

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            Assert.Equal(expected, actualStr);
        }

        [Fact]
        public void ParseArray()
        {
            var buffer = StringToUtf8BufferWithEmptySpace(TestJson.SimpleArrayJson, 60);

            JsonObject parsedObject = JsonObject.Parse(buffer);
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
            JsonObject parsedObject = JsonObject.Parse(buffer);
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
            JsonObject parsedObject = JsonObject.Parse(buffer);
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
            JsonObject parsedObject = JsonObject.Parse(buffer);
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

            using (var textWriter = new StringWriter())
            using (var writer = new JsonTextWriter(textWriter))
            {
                deps.WriteTo(writer);
                return textWriter.ToString();
            }
        }
    }
}
