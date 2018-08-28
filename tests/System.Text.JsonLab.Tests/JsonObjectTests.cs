// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
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
            Utf8Span _id = (Utf8Span)"_id";
            Utf8Span index = (Utf8Span)"index";
            Utf8Span guid = (Utf8Span)"guid";
            Utf8Span isActive = (Utf8Span)"isActive";
            Utf8Span balance = (Utf8Span)"balance";
            Utf8Span picture = (Utf8Span)"picture";
            Utf8Span age = (Utf8Span)"age";
            Utf8Span eyeColor = (Utf8Span)"eyeColor";
            Utf8Span name = (Utf8Span)"name";
            Utf8Span gender = (Utf8Span)"gender";
            Utf8Span company = (Utf8Span)"company";
            Utf8Span email = (Utf8Span)"email";
            Utf8Span phone = (Utf8Span)"phone";
            Utf8Span address = (Utf8Span)"address";
            Utf8Span about = (Utf8Span)"about";
            Utf8Span registered = (Utf8Span)"registered";
            Utf8Span latitude = (Utf8Span)"latitude";
            Utf8Span longitude = (Utf8Span)"longitude";
            Utf8Span tags = (Utf8Span)"tags";
            Utf8Span friends = (Utf8Span)"friends";
            Utf8Span id = (Utf8Span)"id";
            Utf8Span greeting = (Utf8Span)"greeting";
            Utf8Span favoriteFruit = (Utf8Span)"favoriteFruit";


            var sb = new StringBuilder();
            for (int i = 0; i < obj.ArrayLength; i++)
            {
                /*sb.Append((string)obj[i]["_id"]);
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
                sb.Append((string)obj[i]["favoriteFruit"]);*/

                sb.Append((string)obj[i][_id]);
                sb.Append((int)obj[i][index]);
                sb.Append((string)obj[i][guid]);
                sb.Append((bool)obj[i][isActive]);
                sb.Append((string)obj[i][balance]);
                sb.Append((string)obj[i][picture]);
                sb.Append((int)obj[i][age]);
                sb.Append((string)obj[i][eyeColor]);
                sb.Append((string)obj[i][name]);
                sb.Append((string)obj[i][gender]);
                sb.Append((string)obj[i][company]);
                sb.Append((string)obj[i][email]);
                sb.Append((string)obj[i][phone]);
                sb.Append((string)obj[i][address]);
                sb.Append((string)obj[i][about]);
                sb.Append((string)obj[i][registered]);
                sb.Append((double)obj[i][latitude]);
                sb.Append((double)obj[i][longitude]);

                JsonObject tagsObject = obj[i][tags];
                for (int j = 0; j < tagsObject.ArrayLength; j++)
                {
                    sb.Append((string)tagsObject[j]);
                }
                JsonObject friendsObject = obj[i][friends];
                for (int j = 0; j < friendsObject.ArrayLength; j++)
                {
                    sb.Append((int)friendsObject[j][id]);
                    sb.Append((string)friendsObject[j][name]);
                }
                sb.Append((string)obj[i][greeting]);
                sb.Append((string)obj[i][favoriteFruit]);

            }
            return sb.ToString();
        }

        // TestCaseType is only used to give the json strings a descriptive name.
        [Theory]
        [MemberData(nameof(TestCases))]
        public void ParseJson(TestCaseType type, string jsonString)
        {
            // Remove all formatting/indendation
            /*if (true)
            {
                using (JsonTextReader jsonReader = new JsonTextReader(new StringReader(jsonString)))
                {
                    JToken jtoken = JToken.ReadFrom(jsonReader);
                    var stringWriter = new StringWriter();
                    using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter))
                    {
                        jtoken.WriteTo(jsonWriter);
                        jsonString = stringWriter.ToString();
                    }
                }
            }*/

            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            JsonObject obj = JsonObject.Parse(dataUtf8);

            var _stream = new MemoryStream(dataUtf8);
            var _reader = new StreamReader(_stream, Encoding.UTF8, false, 1024, true);

            _stream.Seek(0, SeekOrigin.Begin);
            using (JsonTextReader jsonReader = new JsonTextReader(_reader))
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
            string database1 = obj.PrintDatabase();

            // Change casing to match what JSON.NET does.
            actual = actual.Replace("true", "True").Replace("false", "False");

            TextReader reader = new StringReader(jsonString);
            string expected = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expected, actual);

            if (type == TestCaseType.Json400KB)
            {

                {
                    var lookup1a = new Utf8Span("about");
                    var lookup4a = new Utf8Span("greeting");
                    var lookup2a = new Utf8Span("friends");
                    var lookup3a = new Utf8Span("name");
                    var lookup5a = new Utf8Span("age");

                    string greeting = "";
                    string about = "";
                    string name = "";
                    int age = 0;

                    for (int i = 0; i < obj.ArrayLength; i += 10)
                    {
                        greeting = (string)obj[i][lookup4a];
                        about = (string)obj[i][lookup1a];
                        name = (string)obj[i][lookup2a][1][lookup3a];
                        age = (int)obj[i][lookup5a];
                    }

                    for (int k = 0; k < 2; k++)
                    {
                        for (int i = 0; i < obj.ArrayLength; i += 10)
                        {
                            for (int j = 0; j < 300; j++)
                            {
                                greeting = (string)obj[5][lookup4a];
                            }
                            about = (string)obj[i][lookup1a];
                            name = (string)obj[i][lookup2a][1][lookup3a];
                            age = (int)obj[i][lookup5a];
                        }
                    }

                    Assert.Equal(31, age);
                    Assert.Equal("Lawrence Hewitt", name);
                    Assert.Equal("Nulla qui enim dolor nisi enim occaecat sit ullamco commodo eiusmod proident ipsum eiusmod. Ad incididunt nulla proident ea aute commodo consequat sit esse voluptate nulla laborum ea in. Ipsum laborum dolor consectetur exercitation adipisicing occaecat consectetur excepteur.", about);
                    //Assert.Equal("Hello, Paul Cruz! You have 1 unread messages.", greeting);
                }


                var lookup1 = new Utf8Span("tags");
                var lookup2 = new Utf8Span("friends");
                var lookup3 = new Utf8Span("name");
                var lookup4 = new Utf8Span("name1");
                var lookup5 = new Utf8Span("greeting");

                string t1 = (string)obj[1][lookup1][6];
                string t2 = (string)obj[1][lookup2][2][lookup3];
                
                Assert.Equal("consequat", t1);
                Assert.Equal("Burns Giles", t2);
                string database3 = obj.PrintDatabase();
                try
                {
                    int id = (int)obj[1][lookup4];
                    Assert.True(false);
                }
                catch (KeyNotFoundException)
                {

                }

                string t3 = (string)obj[1][lookup5];
                Assert.Equal("Hello, Faith Cantrell! You have 8 unread messages.", t3);

                /*string t1 = (string)obj[0]["tags"][6];
                string t2 = (string)obj[0]["friends"][2]["name"];
                int id = (int)obj[0]["id"];*/

                for (int i = 0; i < obj.ArrayLength; i++)
                {
                    JsonObject temp = obj[i];
                }

                string database = obj.PrintDatabase();
                var lookup = new Utf8Span("email");
                JsonObject withinArray = obj[5];
                database = withinArray.PrintDatabase();
                database = obj.PrintDatabase();
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
            JsonObject obj = JsonObject.Parse(dataUtf8);

            string actual = obj.PrintJson();
            string database1 = obj.PrintDatabase();

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

            string temp = obj.PrintDatabase();

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
            string temp = parsedObject.PrintDatabase();
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
