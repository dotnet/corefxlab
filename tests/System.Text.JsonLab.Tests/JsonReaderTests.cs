// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Formatting;
using System.Text.JsonLab.Tests.Resources;
using Xunit;

namespace System.Text.JsonLab.Tests
{
    public class JsonReaderTests
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
                    new object[] { true, TestCaseType.HelloWorld, TestJson.HelloWorld},
                    new object[] { true, TestCaseType.LotsOfNumbers, TestJson.LotsOfNumbers},
                    new object[] { true, TestCaseType.LotsOfStrings, TestJson.LotsOfStrings},
                    new object[] { true, TestCaseType.ProjectLockJson, TestJson.ProjectLockJson},
                    //new object[] { true, TestCaseType.SpecialStrings, TestJson.JsonWithSpecialStrings},    // Behavior of escaping is different between Json.NET and JsonLab
                    new object[] { true, TestCaseType.Json400B, TestJson.Json400B},
                    new object[] { true, TestCaseType.Json4KB, TestJson.Json4KB},
                    new object[] { true, TestCaseType.Json40KB, TestJson.Json40KB},
                    new object[] { true, TestCaseType.Json400KB, TestJson.Json400KB},

                    new object[] { false, TestCaseType.Basic, TestJson.BasicJson},
                    new object[] { false, TestCaseType.BasicLargeNum, TestJson.BasicJsonWithLargeNum}, // Json.NET treats numbers starting with 0 as octal (0425 becomes 277)
                    new object[] { false, TestCaseType.BroadTree, TestJson.BroadTree}, // \r\n behavior is different between Json.NET and JsonLab
                    new object[] { false, TestCaseType.DeepTree, TestJson.DeepTree},
                    new object[] { false, TestCaseType.FullSchema1, TestJson.FullJsonSchema1},
                    new object[] { false, TestCaseType.HelloWorld, TestJson.HelloWorld},
                    new object[] { false, TestCaseType.LotsOfNumbers, TestJson.LotsOfNumbers},
                    new object[] { false, TestCaseType.LotsOfStrings, TestJson.LotsOfStrings},
                    new object[] { false, TestCaseType.ProjectLockJson, TestJson.ProjectLockJson},
                    //new object[] { false, TestCaseType.SpecialStrings, TestJson.JsonWithSpecialStrings},    // Behavior of escaping is different between Json.NET and JsonLab
                    new object[] { false, TestCaseType.Json400B, TestJson.Json400B},
                    new object[] { false, TestCaseType.Json4KB, TestJson.Json4KB},
                    new object[] { false, TestCaseType.Json40KB, TestJson.Json40KB},
                    new object[] { false, TestCaseType.Json400KB, TestJson.Json400KB}
                };
            }
        }

        public static IEnumerable<object[]> SpecialNumTestCases
        {
            get
            {
                return new List<object[]>
                {
                    new object[] { TestCaseType.FullSchema2, TestJson.FullJsonSchema2},
                    new object[] { TestCaseType.SpecialNumForm, TestJson.JsonWithSpecialNumFormat},
                };
            }
        }

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
        public static void TestJsonReaderUtf8(bool compactData, TestCaseType type, string jsonString)
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
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int length);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            byte[] resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length);
            string actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expectedStr, actualStr);
            Assert.Equal(expectedStr, actualStrSequence);

            long memoryBefore = GC.GetAllocatedBytesForCurrentThread();
            JsonLabEmptyLoopHelper(dataUtf8);
            long memoryAfter = GC.GetAllocatedBytesForCurrentThread();
            Assert.Equal(0, memoryAfter - memoryBefore);
        }

        [Theory]
        [MemberData(nameof(SpecialNumTestCases))]
        public static void TestJsonReaderUtf8SpecialNumbers(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int length);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            byte[] resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length);
            string actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            // Behavior of E-notation is different between Json.NET and JsonLab
            // Behavior of reading/writing really large number is different as well.
            // TODO: Adjust test accordingly
            //Assert.Equal(expectedStr, actualStr);
            Assert.Equal(actualStr, actualStrSequence);

            long memoryBefore = GC.GetAllocatedBytesForCurrentThread();
            JsonLabEmptyLoopHelper(dataUtf8);
            long memoryAfter = GC.GetAllocatedBytesForCurrentThread();
            Assert.Equal(0, memoryAfter - memoryBefore);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestPartialJsonReader(bool compactData, TestCaseType type, string jsonString)
        {
            // Skipping really large JSON since slicing them (O(n^2)) is too slow.
            if (type == TestCaseType.Json40KB || type == TestCaseType.Json400KB || type == TestCaseType.ProjectLockJson)
            {
                return;
            }

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

            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int outputLength);
            Span<byte> outputSpan = new byte[outputLength];

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = JsonTestHelper.NewtonsoftReturnStringHelper(reader);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                byte[] output = JsonLabReaderLoop(outputSpan.Length, out int firstLength, ref json);
                output.AsSpan(0, firstLength).CopyTo(outputSpan);
                int written = firstLength;

                int consumed = json.CurrentIndex;
                JsonReaderState jsonState = json.State;

                // Skipping large JSON since slicing them (O(n^3)) is too slow.
                if (type == TestCaseType.DeepTree || type == TestCaseType.BroadTree || type == TestCaseType.LotsOfNumbers
                    || type == TestCaseType.LotsOfStrings || type == TestCaseType.Json4KB)
                {
                    json = new Utf8JsonReader(dataUtf8.AsSpan(consumed), isFinalBlock: true, json.State);
                    output = JsonLabReaderLoop(outputSpan.Length - written, out int length, ref json);
                    output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                    written += length;
                    Assert.Equal(dataUtf8.Length - consumed, json.CurrentIndex);

                    Assert.Equal(outputSpan.Length, written);
                    string actualStr = Encoding.UTF8.GetString(outputSpan);
                    Assert.Equal(expectedStr, actualStr);
                }
                else
                {
                    for (int j = consumed; j < dataUtf8.Length - consumed; j++)
                    {
                        written = firstLength;
                        json = new Utf8JsonReader(dataUtf8.AsSpan(consumed, j), isFinalBlock: false, jsonState);
                        output = JsonLabReaderLoop(outputSpan.Length - written, out int length, ref json);
                        output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                        written += length;

                        int consumedInner = json.CurrentIndex;
                        json = new Utf8JsonReader(dataUtf8.AsSpan(consumed + consumedInner), isFinalBlock: true, json.State);
                        output = JsonLabReaderLoop(outputSpan.Length - written, out length, ref json);
                        output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                        written += length;
                        Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.CurrentIndex);

                        Assert.Equal(outputSpan.Length, written);
                        string actualStr = Encoding.UTF8.GetString(outputSpan);
                        Assert.Equal(expectedStr, actualStr);
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(SpecialNumTestCases))]
        public static void TestPartialJsonReaderSpecialNumbers(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                while (json.Read()) ;

                int consumed = json.CurrentIndex;
                JsonReaderState jsonState = json.State;
                for (int j = consumed; j < dataUtf8.Length - consumed; j++)
                {
                    json = new Utf8JsonReader(dataUtf8.AsSpan(consumed, j), isFinalBlock: false, jsonState);
                    while (json.Read()) ;

                    int consumedInner = json.CurrentIndex;
                    json = new Utf8JsonReader(dataUtf8.AsSpan(consumed + consumedInner), isFinalBlock: true, json.State);
                    while (json.Read()) ;
                    Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.CurrentIndex);
                }
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(62)]
        [InlineData(63)]
        [InlineData(64)]
        [InlineData(65)]
        [InlineData(66)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        public static void TestDepth(int depth)
        {
            for (int i = 0; i < depth; i++)
            {
                var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
                var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output);

                WriteDepth(ref jsonUtf8, i);

                ArraySegment<byte> formatted = output.Formatted;
                string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

                Span<byte> data = formatted.Array.AsSpan(formatted.Offset, formatted.Count);
                var json = new Utf8JsonReader(data)
                {
                    MaxDepth = depth
                };

                int actualDepth = 0;
                while (json.Read())
                {
                    if (json.TokenType == JsonTokenType.Value)
                        actualDepth = json.Depth;
                }

                Stream stream = new MemoryStream(data.ToArray());
                TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
                int expectedDepth = 0;
                var newtonJson = new JsonTextReader(reader)
                {
                    MaxDepth = depth
                };
                while (newtonJson.Read())
                {
                    if (newtonJson.TokenType == JsonToken.String)
                    {
                        expectedDepth = newtonJson.Depth;
                    }
                }

                Assert.Equal(expectedDepth, actualDepth);
                Assert.Equal(i + 1, actualDepth);
            }
        }

        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(62)]
        [InlineData(63)]
        [InlineData(64)]
        [InlineData(65)]
        [InlineData(66)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        public static void TestDepthBeyondLimit(int depth)
        {
            var output = new ArrayFormatterWrapper(1024, SymbolTable.InvariantUtf8);
            var jsonUtf8 = new Utf8JsonWriter<ArrayFormatterWrapper>(output);

            WriteDepth(ref jsonUtf8, depth - 1);

            ArraySegment<byte> formatted = output.Formatted;
            string actualStr = Encoding.UTF8.GetString(formatted.Array, formatted.Offset, formatted.Count);

            Span<byte> data = formatted.Array.AsSpan(formatted.Offset, formatted.Count);
            var json = new Utf8JsonReader(data)
            {
                MaxDepth = depth - 1
            };

            try
            {
                int maxDepth = 0;
                while (json.Read())
                {
                    if (maxDepth < json.Depth)
                        maxDepth = json.Depth;
                }
                Assert.True(false, $"Expected JsonReaderException was not thrown. Max depth allowed = {json.MaxDepth} | Max depth reached = {maxDepth}");
            }
            catch (JsonReaderException)
            { }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public static void TestDepthInvalid(int depth)
        {
            Span<byte> data = Span<byte>.Empty;
            var json = new Utf8JsonReader(data);
            try
            {
                json.MaxDepth = depth;
                Assert.True(false, "Expected ArgumentException was not thrown. Max depth must be set to greater than 0.");
            }
            catch (ArgumentException)
            { }
        }

        private static void WriteDepth(ref Utf8JsonWriter<ArrayFormatterWrapper> jsonUtf8, int depth)
        {
            jsonUtf8.WriteObjectStart();
            for (int i = 0; i < depth; i++)
            {
                jsonUtf8.WriteObjectStart("message" + i);
            }
            jsonUtf8.WriteAttribute("message" + depth, "Hello, World!");
            for (int i = 0; i < depth; i++)
            {
                jsonUtf8.WriteObjectEnd();
            }
            jsonUtf8.WriteObjectEnd();
            jsonUtf8.Flush();
        }

        [Theory]
        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\r\n String\r\n\",\"	Mul\r\ntiline String\",\"\\\"somequote\\\"\tMu\\\"\\\"l\r\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\r\n String\r\n, \tMul\r\ntiline String, \\\"somequote\\\"	Mu\\\"\\\"l\r\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]
        public static void TestJsonReaderUtf8SpecialString(string jsonString, string expectedStr)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int length);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);

            result = JsonLabSequenceReturnBytesHelper(dataUtf8, out length);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [InlineData("  \"hello\"  ")]
        [InlineData("  \"he\r\n\\\"l\\\\\\\"lo\\\\\"  ")]
        [InlineData("  12345  ")]
        [InlineData("  null  ")]
        [InlineData("  true  ")]
        [InlineData("  false  ")]
        public static void SingleJsonValue(string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), false);
                while (json.Read()) ;

                int consumed = json.CurrentIndex;
                json = new Utf8JsonReader(dataUtf8.AsSpan(consumed), true, json.State);
                while (json.Read()) ;
                Assert.Equal(dataUtf8.Length - consumed, json.CurrentIndex);
            }
        }

        [Theory]
        [InlineData("\"hello\"", 1, 0)] // "\""
        [InlineData("12345", 3, 0)]   // "123"
        [InlineData("null", 3, 0)]   // "nul"
        [InlineData("true", 3, 0)]   // "tru"
        [InlineData("false", 4, 0)]  // "fals"
        [InlineData("   {\"age\":30}   ", 10, 10)] // "   {\"age\":"
        [InlineData("{\"name\":\"Ahson\"}", 9, 8)]  // "{\"name\":\"Ahso"
        [InlineData("-123456789", 1, 0)] // "-"
        [InlineData("0.5", 2, 0)]    // "0."
        [InlineData("10.5e+3", 5, 0)] // "10.5e"
        [InlineData("10.5e-1", 6, 0)]    // "10.5e-"
        [InlineData("{\"ints\":[1, 2, 3, 4, 5]}", 21, 19)]    // "{\"ints\":[1, 2, 3, 4, "
        [InlineData("{\"strings\":[\"abc\", \"def\"], \"ints\":[1, 2, 3, 4, 5]}", 24, 24)]  // "{\"strings\":[\"abc\", \"def\""
        [InlineData("{\"age\":30, \"name\":\"test}:[]\", \"another string\" : \"tests\"}", 19, 18)]   // "{\"age\":30, \"name\":\"test}"
        [InlineData("   [[[[{\r\n\"temp1\":[[[[{\"temp2:[]}]]]]}]]]]\":[]}]]]]}]]]]   ", 42, 23)] // "   [[[[{\r\n\"temp1\":[[[[{\"temp2:[]}]]]]}]]]]"
        [InlineData("{\r\n\"isActive\": false, \"invalid\"\r\n : \"now its valid\"}", 20, 20)]  // "{\r\n\"isActive\": false, \"invalid\"\r\n}"
        public static void PartialJson(string jsonString, int splitLocation, int consumed)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            var json = new Utf8JsonReader(dataUtf8.AsSpan(0, splitLocation), false);
            while (json.Read()) ;
            Assert.Equal(consumed, json.CurrentIndex);

            json = new Utf8JsonReader(dataUtf8.AsSpan(json.CurrentIndex), true, json.State);
            while (json.Read()) ;
            Assert.Equal(dataUtf8.Length - consumed, json.CurrentIndex);
        }

        [Fact]
        public static void PartialJsonWithWhiteSpace()
        {
            string jsonString = "   {   \r   \n   \"   is   Active   \"   :    false   ,    \"   na   me   \"   \r   \n    :    \"   John    Doe   \"   }   ";
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                var originalDictionary = new Dictionary<string, object>();
                string originalKey = "";
                object originalValue = null;
                SetKeyValues(ref json, originalDictionary, ref originalKey, ref originalValue);

                int consumed = json.CurrentIndex;
                JsonReaderState jsonState = json.State;
                for (int j = consumed; j < dataUtf8.Length - consumed; j++)
                {
                    string key = originalKey;
                    object value = originalValue;
                    var dictionary = new Dictionary<string, object>(originalDictionary);
                    json = new Utf8JsonReader(dataUtf8.AsSpan(consumed, j), isFinalBlock: false, jsonState);
                    SetKeyValues(ref json, dictionary, ref key, ref value);

                    int consumedInner = json.CurrentIndex;
                    json = new Utf8JsonReader(dataUtf8.AsSpan(consumed + consumedInner), isFinalBlock: true, json.State);
                    SetKeyValues(ref json, dictionary, ref key, ref value);
                    Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.CurrentIndex);

                    Assert.True(dictionary.TryGetValue("   is   Active   ", out object value1));
                    Assert.Equal(false.ToString(), value1.ToString());
                    Assert.True(dictionary.TryGetValue("   na   me   ", out object value2));
                    Assert.Equal("   John    Doe   ", value2.ToString());
                }
            }
        }

        private static void SetKeyValues(ref Utf8JsonReader json, Dictionary<string, object> dictionary, ref string key, ref object value)
        {
            while (json.Read())
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        key = Encoding.UTF8.GetString(json.Value);
                        dictionary.Add(key, null);
                        break;
                    case JsonTokenType.Value:
                        switch (json.ValueType)
                        {
                            case JsonValueType.String:
                                value = Encoding.UTF8.GetString(json.Value);
                                break;
                            case JsonValueType.False:
                                value = false;
                                break;
                        }
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        [Theory]
        [InlineData("{\r\n\"is\r\nActive\": false \"invalid\"\r\n}", 22, 22, 3, 15)]
        [InlineData("{\r\n\"is\r\nActive\": false \"invalid\"\r\n}", 23, 23, 3, 15)]
        [InlineData("{\r\n\"is\r\nActive\": false, \"invalid\"\r\n}", 22, 22, 4, 0)]
        [InlineData("{\r\n\"is\r\nActive\": false, \"invalid\"\r\n}", 23, 22, 4, 0)]
        [InlineData("{\r\n\"is\r\nActive\": false, \"invalid\"\r\n}", 24, 22, 4, 0)]
        [InlineData("{\r\n\"is\r\nActive\": false, 5\r\n}", 22, 22, 3, 16)]
        [InlineData("{\r\n\"is\r\nActive\": false, 5\r\n}", 23, 22, 3, 16)]
        [InlineData("{\r\n\"is\r\nActive\": false, 5\r\n}", 24, 22, 3, 16)]
        public static void InvalidJsonSplitRemainsInvalid(string jsonString, int splitLocation, int consumed, int expectedlineNumber, int expectedPosition)
        {
            //TODO: Test multi-segment json payload
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8.AsSpan(0, splitLocation), false)
            {
                Instrument = true
            };
            while (json.Read()) ;
            Assert.Equal(consumed, json.CurrentIndex);

            json = new Utf8JsonReader(dataUtf8.AsSpan(json.CurrentIndex), true, json.State)
            {
                Instrument = true
            };
            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.Position);
            }

            json = new Utf8JsonReader(dataUtf8.AsSpan(json.CurrentIndex), true, json.State);
            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(-1, ex.LineNumber);
                Assert.Equal(-1, ex.Position);
            }
        }

        [Theory]
        [InlineData("{]", 1, 1)]
        [InlineData("[}", 1, 1)]
        [InlineData("nulz", 1, 0)]
        [InlineData("truz", 1, 0)]
        [InlineData("falsz", 1, 0)]
        [InlineData("\"age\":", 1, 5)]
        [InlineData("12345.1.", 1, 0)]
        [InlineData("-f", 1, 0)]
        [InlineData("1.f", 1, 0)]
        [InlineData("0.1f", 1, 0)]
        [InlineData("0.1e1f", 1, 0)]
        [InlineData("123,", 1, 3)]
        [InlineData("01", 1, 0)]
        [InlineData("-01", 1, 0)]
        [InlineData("10.5e-0.2", 1, 0)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5.1e7.3]}", 1, 31)]
        [InlineData("{\"age\":30, \r\n \"num\":-0.e, \r\n \"ints\":[1, 2, 3, 4, 5]}", 2, 7)]
        [InlineData("{{}}", 1, 1)]
        [InlineData("[[{{}}]]", 1, 3)]
        [InlineData("[1, 2, 3, ]", 1, 10)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5}}", 1, 33)]
        [InlineData("{\r\n\"isActive\": false \"\r\n}", 2, 18)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2\":[}]]]]}]]]]", 2, 22)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2\":[]},[}]]]]}]]]]", 2, 26)]
        [InlineData("{\r\n\t\"isActive\": false,\r\n\t\"array\": [\r\n\t\t[{\r\n\t\t\t\"id\": 1\r\n\t\t}]\r\n\t]\r\n}", 4, 3, 3)]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\r\n String\r\n\",\"	Mul\r\ntiline String\",\"\\\"somequote\\\"\tMu\\\"\\\"l\r\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
        public static void InvalidJsonWhenPartial(string jsonString, int expectedlineNumber, int expectedPosition, int maxDepth = 64)
        {
            //TODO: Test multi-segment json payload
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8, false)
            {
                MaxDepth = maxDepth,
                Instrument = true
            };

            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.Position);
            }

            json = new Utf8JsonReader(dataUtf8, false)
            {
                MaxDepth = maxDepth
            };

            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(-1, ex.LineNumber);
                Assert.Equal(-1, ex.Position);
            }
        }

        [Theory]
        [InlineData("\"", 1, 0)]
        [InlineData("{]", 1, 1)]
        [InlineData("[}", 1, 1)]
        [InlineData("nul", 1, 0)]
        [InlineData("tru", 1, 0)]
        [InlineData("fals", 1, 0)]
        [InlineData("\"age\":", 1, 5)]
        [InlineData("{\"age\":", 1, 7)]
        [InlineData("{\"name\":\"Ahso", 1, 8)]
        [InlineData("12345.1.", 1, 0)]
        [InlineData("-", 1, 0)]
        [InlineData("-f", 1, 0)]
        [InlineData("1.f", 1, 0)]
        [InlineData("0.", 1, 0)]
        [InlineData("0.1f", 1, 0)]
        [InlineData("0.1e1f", 1, 0)]
        [InlineData("123,", 1, 3)]
        [InlineData("false,", 1, 5)]
        [InlineData("true,", 1, 4)]
        [InlineData("null,", 1, 4)]
        [InlineData("\"hello\",", 1, 7)]
        [InlineData("01", 1, 0)]
        [InlineData("-01", 1, 0)]
        [InlineData("10.5e", 1, 0)]
        [InlineData("10.5e-", 1, 0)]
        [InlineData("10.5e-0.2", 1, 0)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5.1e7.3]}", 1, 31)]
        [InlineData("{\"age\":30, \r\n \"num\":-0.e, \r\n \"ints\":[1, 2, 3, 4, 5]}", 2, 7)]
        [InlineData("{{}}", 1, 1)]
        [InlineData("[[{{}}]]", 1, 3)]
        [InlineData("[1, 2, 3, ]", 1, 10)]
        [InlineData("{\"ints\":[1, 2, 3, 4, 5", 1, 22)]
        [InlineData("{\"strings\":[\"abc\", \"def\"", 1, 24)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5}}", 1, 33)]
        [InlineData("{\"age\":30, \"name\":\"test}", 1, 18)]
        [InlineData("{\r\n\"isActive\": false \"\r\n}", 2, 18)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2\":[}]]]]}]]]]", 2, 22)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2:[]}]]]]}]]]]", 2, 13)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2\":[]},[}]]]]}]]]]", 2, 26)]
        [InlineData("{\r\n\t\"isActive\": false,\r\n\t\"array\": [\r\n\t\t[{\r\n\t\t\t\"id\": 1\r\n\t\t}]\r\n\t]\r\n}", 4, 3, 3)]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\r\n String\r\n\",\"	Mul\r\ntiline String\",\"\\\"somequote\\\"\tMu\\\"\\\"l\r\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
        public static void InvalidJson(string jsonString, int expectedlineNumber, int expectedPosition, int maxDepth = 64)
        {
            //TODO: Test multi-segment json payload
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8)
            {
                MaxDepth = maxDepth,
                Instrument = true
            };

            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.Position);
            }

            json = new Utf8JsonReader(dataUtf8)
            {
                MaxDepth = maxDepth
            };
            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(-1, ex.LineNumber);
                Assert.Equal(-1, ex.Position);
            }
        }

        [Theory]
        [InlineData("{\"protocol\":\"dummy\",\"version\":1}\u001e", "dummy", 1)]
        [InlineData("{\"protocol\":\"\",\"version\":10}\u001e", "", 10)]
        [InlineData("{\"protocol\":\"\",\"version\":10,\"unknown\":null}\u001e", "", 10)]
        public void ParsingHandshakeRequestMessageSuccessForValidMessages(string json, string protocol, int version)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(json);
            var message = new ReadOnlySequence<byte>(dataUtf8);

            Assert.True(TryParseRequestMessage(ref message));

            message = CreateSegments(dataUtf8);
            Assert.True(TryParseRequestMessage(ref message));
        }

        [Theory]
        [InlineData("42\u001e", "Unexpected JSON Token Type 'Integer'. Expected a JSON Object.")]
        [InlineData("\"42\"\u001e", "Unexpected JSON Token Type 'String'. Expected a JSON Object.")]
        [InlineData("null\u001e", "Unexpected JSON Token Type 'Null'. Expected a JSON Object.")]
        [InlineData("{}\u001e", "Missing required property 'protocol'.")]
        [InlineData("[]\u001e", "Unexpected JSON Token Type 'Array'. Expected a JSON Object.")]
        [InlineData("{\"protocol\":\"json\"}\u001e", "Missing required property 'version'.")]
        [InlineData("{\"version\":1}\u001e", "Missing required property 'protocol'.")]
        [InlineData("{\"version\":\"123\"}\u001e", "Expected 'version' to be of type Integer.")]
        [InlineData("{\"protocol\":null,\"version\":123}\u001e", "Expected 'protocol' to be of type String.")]
        public void ParsingHandshakeRequestMessageThrowsForInvalidMessages(string payload, string expectedMessage)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(payload);
            var message = new ReadOnlySequence<byte>(dataUtf8);

            var exception = Assert.Throws<InvalidDataException>(() =>
                Assert.True(TryParseRequestMessage(ref message)));

            Assert.Equal(expectedMessage, exception.Message);

            message = CreateSegments(dataUtf8);

            exception = Assert.Throws<InvalidDataException>(() =>
                Assert.True(TryParseRequestMessage(ref message)));

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData("42\u001e", "Unexpected JSON Token Type 'Integer'. Expected a JSON Object.")]
        [InlineData("\"42\"\u001e", "Unexpected JSON Token Type 'String'. Expected a JSON Object.")]
        [InlineData("null\u001e", "Unexpected JSON Token Type 'Null'. Expected a JSON Object.")]
        [InlineData("[]\u001e", "Unexpected JSON Token Type 'Array'. Expected a JSON Object.")]
        [InlineData("{\"error\":null}\u001e", "Expected 'error' to be of type String.")]
        public void ParsingHandshakeResponseMessageThrowsForInvalidMessages(string payload, string expectedMessage)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(payload);
            var message = new ReadOnlySequence<byte>(dataUtf8);

            var exception = Assert.Throws<InvalidDataException>(() =>
                TryParseResponseMessage(ref message));

            Assert.Equal(expectedMessage, exception.Message);

            message = CreateSegments(dataUtf8);

            exception = Assert.Throws<InvalidDataException>(() =>
                TryParseResponseMessage(ref message));

            Assert.Equal(expectedMessage, exception.Message);
        }

        private static bool TryParseResponseMessage(ref ReadOnlySequence<byte> buffer)
        {
            if (!TryParseMessage(ref buffer, out var payload))
            {
                return false;
            }

            var reader = new Utf8JsonReader(payload);

            CheckRead(ref reader);
            EnsureObjectStart(ref reader);

            int? minorVersion = null;
            string error = null;

            var completed = false;
            while (!completed && CheckRead(ref reader))
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        ReadOnlySpan<byte> memberName = reader.Value;

                        if (memberName.SequenceEqual(TypePropertyNameUtf8))
                        {

                            // a handshake response does not have a type
                            // check the incoming message was not any other type of message
                            throw new InvalidDataException("Handshake response should not have a 'type' value.");
                        }
                        else if (memberName.SequenceEqual(ErrorPropertyNameUtf8))
                        {
                            error = ReadAsString(ref reader, ErrorPropertyName);
                        }
                        else if (memberName.SequenceEqual(MinorVersionPropertyNameUtf8))
                        {
                            minorVersion = ReadAsInt32(ref reader, MinorVersionPropertyName);
                        }
                        else
                        {
                            reader.Skip();
                        }
                        break;
                    case JsonTokenType.EndObject:
                        completed = true;
                        break;
                    default:
                        throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading handshake response JSON.");
                }
            };

            return true;
        }

        private static readonly byte RecordSeparator = 0x1e;

        private static bool TryParseMessage(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> payload)
        {
            var position = buffer.PositionOf(RecordSeparator);
            if (position == null)
            {
                payload = default;
                return false;
            }

            payload = buffer.Slice(0, position.Value);

            // Skip record separator
            buffer = buffer.Slice(buffer.GetPosition(1, position.Value));

            return true;
        }

        private static int? ReadAsInt32(ref Utf8JsonReader reader, string propertyName)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.Value || reader.ValueType != JsonValueType.Number)
            {
                throw new InvalidDataException($"Expected '{propertyName}' to be of type Integer.");
            }

            if (reader.Value.IsEmpty)
            {
                return null;
            }
            if (!Utf8Parser.TryParse(reader.Value, out int value, out _))
            {
                throw new InvalidDataException($"Expected '{propertyName}' to be of type Integer.");
            }
            return value;
        }

        private static bool CheckRead(ref Utf8JsonReader reader)
        {
            if (!reader.Read())
            {
                throw new InvalidDataException("Unexpected end when reading JSON.");
            }

            return true;
        }

        private static string GetTokenString(JsonValueType valueType, JsonTokenType tokenType)
        {
            switch (valueType)
            {
                case JsonValueType.Number:
                    return "Integer";
                case JsonValueType.Unknown:
                    if (tokenType == JsonTokenType.StartArray)
                    {
                        return JsonValueType.Array.ToString();
                    }
                    if (tokenType == JsonTokenType.StartObject)
                    {
                        return JsonValueType.Object.ToString();
                    }
                    return tokenType.ToString();
                default:
                    break;
            }
            return valueType.ToString();
        }

        private static void EnsureObjectStart(ref Utf8JsonReader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidDataException($"Unexpected JSON Token Type '{GetTokenString(reader.ValueType, reader.TokenType)}'. Expected a JSON Object.");
            }
        }

        private const string ProtocolPropertyName = "protocol";
        private const string ProtocolVersionPropertyName = "version";
        private const string MinorVersionPropertyName = "minorVersion";
        private const string ErrorPropertyName = "error";
        private const string TypePropertyName = "type";

        private static readonly byte[] ProtocolPropertyNameUtf8 = Encoding.UTF8.GetBytes("protocol");
        private static readonly byte[] ProtocolVersionPropertyNameUtf8 = Encoding.UTF8.GetBytes("version");
        private static readonly byte[] MinorVersionPropertyNameUtf8 = Encoding.UTF8.GetBytes("minorVersion");
        private static readonly byte[] ErrorPropertyNameUtf8 = Encoding.UTF8.GetBytes("error");
        private static readonly byte[] TypePropertyNameUtf8 = Encoding.UTF8.GetBytes("type");

        private static bool TryParseRequestMessage(ref ReadOnlySequence<byte> buffer)
        {
            if (!TryParseMessage(ref buffer, out var payload))
            {
                return false;
            }

            var reader = new Utf8JsonReader(payload);
            CheckRead(ref reader);
            EnsureObjectStart(ref reader);

            string protocol = null;
            int? protocolVersion = null;

            var completed = false;
            while (!completed && CheckRead(ref reader))
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        ReadOnlySpan<byte> memberName = reader.Value;

                        if (memberName.SequenceEqual(ProtocolPropertyNameUtf8))
                        {
                            protocol = ReadAsString(ref reader, ProtocolPropertyName);
                        }
                        else if (memberName.SequenceEqual(ProtocolVersionPropertyNameUtf8))
                        {
                            protocolVersion = ReadAsInt32(ref reader, ProtocolVersionPropertyName);
                        }
                        else
                        {
                            reader.Skip();
                        }
                        break;
                    case JsonTokenType.EndObject:
                        completed = true;
                        break;
                    default:
                        throw new InvalidDataException($"Unexpected token '{reader.TokenType}' when reading handshake request JSON.");
                }
            }

            if (protocol == null)
            {
                throw new InvalidDataException($"Missing required property '{ProtocolPropertyName}'.");
            }
            if (protocolVersion == null)
            {
                throw new InvalidDataException($"Missing required property '{ProtocolVersionPropertyName}'.");
            }

            return true;
        }

        private static unsafe string ReadAsString(ref Utf8JsonReader reader, string propertyName)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.Value || reader.ValueType != JsonValueType.String)
            {
                throw new InvalidDataException($"Expected '{propertyName}' to be of type String.");
            }

            if (reader.Value.IsEmpty) return "";

#if NETCOREAPP2_2
            return Encoding.UTF8.GetString(reader.Value);
#else
            fixed (byte* bytes = &MemoryMarshal.GetReference(reader.Value))
            {
                return Encoding.UTF8.GetString(bytes, reader.Value.Length);
            }
#endif
        }

        private static void JsonLabEmptyLoopHelper(byte[] data)
        {
            var json = new Utf8JsonReader(data);
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.Value:
                        JsonValueType valueType = json.ValueType;
                        switch (valueType)
                        {
                            case JsonValueType.Unknown:
                                break;
                            case JsonValueType.Object:
                                break;
                            case JsonValueType.Array:
                                break;
                            case JsonValueType.Number:
                                break;
                            case JsonValueType.String:
                                break;
                            case JsonValueType.True:
                                break;
                            case JsonValueType.False:
                                break;
                            case JsonValueType.Null:
                                break;
                        }
                        break;
                    case JsonTokenType.None:
                        break;
                    case JsonTokenType.Comment:
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        private static ReadOnlySequence<byte> CreateSegments(byte[] data)
        {
            ReadOnlyMemory<byte> dataMemory = data;

            var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, data.Length / 2));
            ReadOnlyMemory<byte> secondMem = dataMemory.Slice(data.Length / 2);
            BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);

            return new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);
        }

        private static byte[] JsonLabSequenceReturnBytesHelper(byte[] data, out int length)
        {
            ReadOnlySequence<byte> sequence = CreateSegments(data);
            var reader = new Utf8JsonReader(sequence);
            byte[] result = JsonLabReaderLoop(data.Length, out length, ref reader);
            // TODO: Should we reset the value and valuetype once we are done?
            //Assert.True(reader.Value.IsEmpty);
            //Assert.Equal(JsonValueType.Unknown, reader.ValueType);
            return result;
        }

        private static byte[] JsonLabReaderLoop(int inpuDataLength, out int length, ref Utf8JsonReader json)
        {
            byte[] outputArray = new byte[inpuDataLength];
            Span<byte> destination = outputArray;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.Value:
                        JsonValueType valueType = json.ValueType;

                        switch (valueType)
                        {
                            // Special casing True/False so that the casing matches with Json.NET
                            case JsonValueType.True:
                                destination[0] = (byte)'T';
                                destination[1] = (byte)'r';
                                destination[2] = (byte)'u';
                                destination[3] = (byte)'e';
                                destination[valueSpan.Length] = (byte)',';
                                destination[valueSpan.Length + 1] = (byte)' ';
                                destination = destination.Slice(valueSpan.Length + 2);
                                break;
                            case JsonValueType.False:
                                destination[0] = (byte)'F';
                                destination[1] = (byte)'a';
                                destination[2] = (byte)'l';
                                destination[3] = (byte)'s';
                                destination[4] = (byte)'e';
                                destination[valueSpan.Length] = (byte)',';
                                destination[valueSpan.Length + 1] = (byte)' ';
                                destination = destination.Slice(valueSpan.Length + 2);
                                break;
                            case JsonValueType.Number:
                            case JsonValueType.String:
                                valueSpan.CopyTo(destination);
                                destination[valueSpan.Length] = (byte)',';
                                destination[valueSpan.Length + 1] = (byte)' ';
                                destination = destination.Slice(valueSpan.Length + 2);
                                break;
                            case JsonValueType.Null:
                                // Special casing Null so that it matches what JSON.NET does
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        private static byte[] JsonLabReturnBytesHelper(byte[] data, out int length)
        {
            var reader = new Utf8JsonReader(data);
            return JsonLabReaderLoop(data.Length, out length, ref reader);
        }
    }

    internal class BufferSegment<T> : ReadOnlySequenceSegment<T>
    {
        public BufferSegment(ReadOnlyMemory<T> memory)
        {
            Memory = memory;
        }

        public BufferSegment<T> Append(ReadOnlyMemory<T> memory)
        {
            var segment = new BufferSegment<T>(memory)
            {
                RunningIndex = RunningIndex + Memory.Length
            };
            Next = segment;
            return segment;
        }
    }
}
