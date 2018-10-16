// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Text.Formatting;
using System.Text.JsonLab.Tests.Resources;
using Xunit;

using static System.Text.JsonLab.Tests.JsonTestHelper;

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
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expectedStr, actualStr);
            Assert.Equal(expectedStr, actualStrSequence);

            // Json payload contains numbers that are too large for .NET (need BigInteger+)
            if (type != TestCaseType.FullSchema1)
            {
                object jsonValues = JsonLabReturnObjectHelper(dataUtf8);
                string s = ObjectToString(jsonValues);
                Assert.Equal(expectedStr.Substring(0, expectedStr.Length - 2), s.Substring(0, s.Length - 2));
            }

            result = JsonLabReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.SkipComments);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.SkipComments);
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);
            Assert.Equal(expectedStr, actualStrSequence);

            result = JsonLabReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.AllowComments);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.AllowComments);
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);
            Assert.Equal(expectedStr, actualStrSequence);
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
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            // Behavior of E-notation is different between Json.NET and JsonLab
            // Behavior of reading/writing really large number is different as well.
            // TODO: Adjust test accordingly
            //Assert.Equal(expectedStr, actualStr);
            Assert.Equal(actualStr, actualStrSequence);

            // Json payload contains numbers that are too large for .NET (need BigInteger+)
            //object jsonValues = JsonLabReturnObjectHelper(dataUtf8);
            //string s = ObjectToString(jsonValues);
            //Assert.Equal(expectedStr, s);

            result = JsonLabReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.SkipComments);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.SkipComments);
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Assert.Equal(actualStr, actualStrSequence);

            result = JsonLabReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.AllowComments);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, JsonReaderOptions.AllowComments);
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Assert.Equal(actualStr, actualStrSequence);
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
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                byte[] output = JsonLabReaderLoop(outputSpan.Length, out int firstLength, ref json);
                output.AsSpan(0, firstLength).CopyTo(outputSpan);
                int written = firstLength;

                long consumed = json.Consumed;
                JsonReaderState jsonState = json.State;

                // Skipping large JSON since slicing them (O(n^3)) is too slow.
                if (type == TestCaseType.DeepTree || type == TestCaseType.BroadTree || type == TestCaseType.LotsOfNumbers
                    || type == TestCaseType.LotsOfStrings || type == TestCaseType.Json4KB)
                {
                    json = new Utf8JsonReader(dataUtf8.AsSpan((int)consumed), isFinalBlock: true, json.State);
                    output = JsonLabReaderLoop(outputSpan.Length - written, out int length, ref json);
                    output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                    written += length;
                    Assert.Equal(dataUtf8.Length - consumed, json.Consumed);

                    Assert.Equal(outputSpan.Length, written);
                    string actualStr = Encoding.UTF8.GetString(outputSpan);
                    Assert.Equal(expectedStr, actualStr);
                }
                else
                {
                    for (long j = consumed; j < dataUtf8.Length - consumed; j++)
                    {
                        written = firstLength;
                        json = new Utf8JsonReader(dataUtf8.AsSpan((int)consumed, (int)j), isFinalBlock: false, jsonState);
                        output = JsonLabReaderLoop(outputSpan.Length - written, out int length, ref json);
                        output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                        written += length;

                        long consumedInner = json.Consumed;
                        json = new Utf8JsonReader(dataUtf8.AsSpan((int)(consumed + consumedInner)), isFinalBlock: true, json.State);
                        output = JsonLabReaderLoop(outputSpan.Length - written, out length, ref json);
                        output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                        written += length;
                        Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.Consumed);

                        Assert.Equal(outputSpan.Length, written);
                        string actualStr = Encoding.UTF8.GetString(outputSpan);
                        Assert.Equal(expectedStr, actualStr);
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestPartialJsonReaderMultiSegment(bool compactData, TestCaseType type, string jsonString)
        {
            // Skipping really large JSON since slicing them (O(n^2)) is too slow.
            if (type == TestCaseType.Json40KB || type == TestCaseType.Json400KB || type == TestCaseType.ProjectLockJson
                || type == TestCaseType.DeepTree || type == TestCaseType.BroadTree || type == TestCaseType.LotsOfNumbers
                || type == TestCaseType.LotsOfStrings || type == TestCaseType.Json4KB)
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
            ReadOnlyMemory<byte> dataMemory = dataUtf8;

            var sequences = new List<ReadOnlySequence<byte>>();

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);
                sequences.Add(sequence);
            }

            for (int i = 0; i < sequences.Count; i++)
            {
                var json = new Utf8JsonReader(sequences[i]);
                while (json.Read()) ;
                Assert.Equal(sequences[i].Length, json.Consumed);
            }

            for (int i = 0; i < sequences.Count; i++)
            {
                ReadOnlySequence<byte> sequence = sequences[i];
                for (int j = 0; j < dataUtf8.Length; j++)
                {
                    var json = new Utf8JsonReader(sequence.Slice(0, j), isFinalBlock: false);
                    while (json.Read()) ;

                    long consumed = json.Consumed;
                    JsonReaderState jsonState = json.State;
                    json = new Utf8JsonReader(sequence.Slice(consumed), isFinalBlock: true, json.State);
                    while (json.Read()) ;
                    Assert.Equal(dataUtf8.Length - consumed, json.Consumed);
                }
            }
        }

        [Theory]
        [MemberData(nameof(SpecialNumTestCases))]
        public static void TestPartialJsonReaderSpecialNumbers(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                for (int i = 0; i < dataUtf8.Length; i++)
                {
                    var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), isFinalBlock: false)
                    {
                        Options = option
                    };
                    while (json.Read()) ;

                    long consumed = json.Consumed;
                    JsonReaderState jsonState = json.State;
                    for (long j = consumed; j < dataUtf8.Length - consumed; j++)
                    {
                        json = new Utf8JsonReader(dataUtf8.AsSpan((int)consumed, (int)j), isFinalBlock: false, jsonState)
                        {
                            Options = option
                        };
                        while (json.Read()) ;

                        long consumedInner = json.Consumed;
                        json = new Utf8JsonReader(dataUtf8.AsSpan((int)(consumed + consumedInner)), isFinalBlock: true, json.State)
                        {
                            Options = option
                        };
                        while (json.Read()) ;
                        Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.Consumed);
                    }
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
                    if (json.TokenType >= JsonTokenType.String && json.TokenType <= JsonTokenType.Null)
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

        [Theory]
        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", JsonReaderOptions.Default, "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            JsonReaderOptions.Default,
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\\r\\n String\\r\\n, \\tMul\\r\\ntiline String, \\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]

        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", JsonReaderOptions.AllowComments, "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            JsonReaderOptions.AllowComments,
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\\r\\n String\\r\\n, \\tMul\\r\\ntiline String, \\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]

        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", JsonReaderOptions.SkipComments, "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            JsonReaderOptions.SkipComments,
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\\r\\n String\\r\\n, \\tMul\\r\\ntiline String, \\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]
        public static void TestJsonReaderUtf8SpecialString(string jsonString, JsonReaderOptions option, string expectedStr)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int length, option);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);

            result = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, option);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);

            object jsonValues = JsonLabReturnObjectHelper(dataUtf8, option);
            string s = ObjectToString(jsonValues);
            Assert.Equal(expectedStr.Substring(0, expectedStr.Length - 2), s.Substring(0, s.Length - 2));
        }

        [Theory]
        [InlineData("  \"hello\"  ")]
        [InlineData("  \"he\\r\\n\\\"l\\\\\\\"lo\\\\\"  ")]
        [InlineData("  12345  ")]
        [InlineData("  null  ")]
        [InlineData("  true  ")]
        [InlineData("  false  ")]
        public static void SingleJsonValue(string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                for (int i = 0; i < dataUtf8.Length; i++)
                {
                    var json = new Utf8JsonReader(dataUtf8.AsSpan(0, i), false)
                    {
                        Options = option
                    };
                    while (json.Read()) ;

                    long consumed = json.Consumed;
                    json = new Utf8JsonReader(dataUtf8.AsSpan((int)consumed), true, json.State)
                    {
                        Options = option
                    };
                    while (json.Read()) ;
                    Assert.Equal(dataUtf8.Length - consumed, json.Consumed);
                }
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
            
            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, splitLocation), false)
                {
                    Options = option
                };
                while (json.Read()) ;
                Assert.Equal(consumed, json.Consumed);

                json = new Utf8JsonReader(dataUtf8.AsSpan((int)json.Consumed), true, json.State)
                {
                    Options = option
                };
                while (json.Read()) ;
                Assert.Equal(dataUtf8.Length - consumed, json.Consumed);
            }
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

                long consumed = json.Consumed;
                JsonReaderState jsonState = json.State;
                for (long j = consumed; j < dataUtf8.Length - consumed; j++)
                {
                    string key = originalKey;
                    object value = originalValue;
                    var dictionary = new Dictionary<string, object>(originalDictionary);
                    json = new Utf8JsonReader(dataUtf8.AsSpan((int)consumed, (int)j), isFinalBlock: false, jsonState);
                    SetKeyValues(ref json, dictionary, ref key, ref value);

                    long consumedInner = json.Consumed;
                    json = new Utf8JsonReader(dataUtf8.AsSpan((int)(consumed + consumedInner)), isFinalBlock: true, json.State);
                    SetKeyValues(ref json, dictionary, ref key, ref value);
                    Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.Consumed);

                    Assert.True(dictionary.TryGetValue("   is   Active   ", out object value1));
                    Assert.Equal(false.ToString(), value1.ToString());
                    Assert.True(dictionary.TryGetValue("   na   me   ", out object value2));
                    Assert.Equal("   John    Doe   ", value2.ToString());
                }
            }
        }

        [Theory]
        [InlineData("{\r\n\"is\\r\\nActive\": false \"invalid\"\r\n}", 24, 24, 3, 15)]
        [InlineData("{\r\n\"is\\r\\nActive\": false \"invalid\"\r\n}", 25, 25, 3, 15)]
        [InlineData("{\r\n\"is\\r\\nActive\": false, \"invalid\"\r\n}", 24, 24, 4, 0)]
        [InlineData("{\r\n\"is\\r\\nActive\": false, \"invalid\"\r\n}", 25, 24, 4, 0)]
        [InlineData("{\r\n\"is\\r\\nActive\": false, \"invalid\"\r\n}", 26, 24, 4, 0)]
        [InlineData("{\r\n\"is\\r\\nActive\": false, 5\r\n}", 24, 24, 3, 16)]
        [InlineData("{\r\n\"is\\r\\nActive\": false, 5\r\n}", 25, 24, 3, 16)]
        [InlineData("{\r\n\"is\\r\\nActive\": false, 5\r\n}", 26, 24, 3, 16)]
        public static void InvalidJsonSplitRemainsInvalid(string jsonString, int splitLocation, int consumed, int expectedlineNumber, int expectedPosition)
        {
            //TODO: Test multi-segment json payload
            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
                var json = new Utf8JsonReader(dataUtf8.AsSpan(0, splitLocation), false)
                {
                    Options = option
                };
                while (json.Read()) ;
                Assert.Equal(consumed, json.Consumed);

                json = new Utf8JsonReader(dataUtf8.AsSpan((int)json.Consumed), true, json.State)
                {
                    Options = option
                };
                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedPosition, ex.LinePosition);
                }
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
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
        public static void InvalidJsonWhenPartial(string jsonString, int expectedlineNumber, int expectedPosition, int maxDepth = 64)
        {
            //TODO: Test multi-segment json payload
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                var json = new Utf8JsonReader(dataUtf8, false)
                {
                    MaxDepth = maxDepth,
                    Options = option
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedPosition, ex.LinePosition);
                }
            }
        }

        [Theory]
        [InlineData("{\"text\": \"๏ แผ่นดินฮั่นเสื่อมโทรมแสนสังเวช\\uABCZ พระปกเกศกองบู๊กู้ขึ้นใหม่\"}", 1, 48)]
        [InlineData("{\"text\": \"๏ แผ่นดินฮั่นเสื่อมโ\\nทรมแสนสังเวช\\uABCZ พระปกเกศกองบู๊กู้ขึ้นใหม่\"}", 2, 18)]
        public static void PositionInCharacters(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                var json = new Utf8JsonReader(dataUtf8, false)
                {
                    Options = option
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    //Assert.Equal(expectedPosition, ex.LinePosition); //TODO: LinePosition needs to be in UTF-16 characters
                }
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
        [InlineData("1a", 1, 0)]
        [InlineData("-01", 1, 0)]
        [InlineData("10.5e", 1, 0)]
        [InlineData("10.5e-", 1, 0)]
        [InlineData("10.5e-0.2", 1, 0)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5.1e7.3]}", 1, 31)]
        [InlineData("{\"age\":30, \r\n \"num\":-0.e, \r\n \"ints\":[1, 2, 3, 4, 5]}", 2, 7)]
        [InlineData("{{}}", 1, 1)]
        [InlineData("[[{{}}]]", 1, 3)]
        [InlineData("[1, 2, 3, ]", 1, 10)]
        [InlineData("{\"ints\":[1, 2, 3, 4, 5", 1, 21)]
        [InlineData("{\"strings\":[\"abc\", \"def\"", 1, 24)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5}}", 1, 33)]
        [InlineData("{\"age\":30, \"name\":\"test}", 1, 18)]
        [InlineData("{\r\n\"isActive\": false \"\r\n}", 2, 18)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2\":[}]]]]}]]]]", 2, 22)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2:[]}]]]]}]]]]", 2, 13)]
        [InlineData("[[[[{\r\n\"temp1\":[[[[{\"temp2\":[]},[}]]]]}]]]]", 2, 26)]
        [InlineData("{\r\n\t\"isActive\": false,\r\n\t\"array\": [\r\n\t\t[{\r\n\t\t\t\"id\": 1\r\n\t\t}]\r\n\t]\r\n}", 4, 3, 3)]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
        [InlineData("\"hel\rlo\"", 1, 4)]
        [InlineData("\"hel\nlo\"", 1, 4)]
        [InlineData("\"hel\\uABCXlo\"", 1, 9)]
        [InlineData("\"hel\\\tlo\"", 1, 5)]
        [InlineData("\"hel\rlo\\\"\"", 1, 4)]
        [InlineData("\"hel\nlo\\\"\"", 1, 4)]
        [InlineData("\"hel\\uABCXlo\\\"\"", 1, 9)]
        [InlineData("\"hel\\\tlo\\\"\"", 1, 5)]
        [InlineData("\"he\\nl\rlo\\\"\"", 2, 1)]
        [InlineData("\"he\\nl\nlo\\\"\"", 2, 1)]
        [InlineData("\"he\\nl\\uABCXlo\\\"\"", 2, 6)]
        [InlineData("\"he\\nl\\\tlo\\\"\"", 2, 2)]
        [InlineData("\"he\\nl\rlo", 1, 0)]
        [InlineData("\"he\\nl\nlo", 1, 0)]
        [InlineData("\"he\\nl\\uABCXlo", 1, 0)]
        [InlineData("\"he\\nl\\\tlo", 1, 0)]
        public static void InvalidJson(string jsonString, int expectedlineNumber, int expectedPosition, int maxDepth = 64)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions option in Enum.GetValues(typeof(JsonReaderOptions)))
            {
                var json = new Utf8JsonReader(dataUtf8)
                {
                    MaxDepth = maxDepth,
                    Options = option
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedPosition, ex.LinePosition);
                }

                ReadOnlyMemory<byte> dataMemory = dataUtf8;
                for (int i = 0; i < dataMemory.Length; i++)
                {
                    var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                    ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                    BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                    var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                    var jsonMultiSegment = new Utf8JsonReader(sequence)
                    {
                        MaxDepth = maxDepth
                    };

                    try
                    {
                        while (jsonMultiSegment.Read()) ;
                        Assert.True(false, "Expected JsonReaderException was not thrown with multi-segment data.");
                    }
                    catch (JsonReaderException ex)
                    {
                        Assert.Equal(expectedlineNumber, ex.LineNumber);
                        Assert.Equal(expectedPosition, ex.LinePosition);
                    }
                }
            }
        }

        [Theory]
        [InlineData("//", "", 2)]
        [InlineData("//\n", "", 3)]
        [InlineData("/**/", "", 4)]
        [InlineData("/*/*/", "/", 5)]

        [InlineData("//This is a comment before json\n\"hello\"", "This is a comment before json", 32)]
        [InlineData("\"hello\"//This is a comment after json", "This is a comment after json", 37)]
        [InlineData("\"hello\"//This is a comment after json\n", "This is a comment after json", 38)]
        [InlineData("\"alpha\" \r\n//This is a comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a comment after json", 41)]
        [InlineData("\"beta\" \r\n//This is a comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a comment after json", 40)]
        [InlineData("\"gamma\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a comment after json", 41)]
        [InlineData("\"delta\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a comment after json", 41)]
        [InlineData("\"hello\"//This is a comment after json with new line\n", "This is a comment after json with new line", 52)]
        [InlineData("{\"age\" : \n//This is a comment between key-value pairs\n 30}", "This is a comment between key-value pairs", 54)]
        [InlineData("{\"age\" : 30//This is a comment between key-value pairs on the same line\n}", "This is a comment between key-value pairs on the same line", 72)]

        [InlineData("/*This is a multi-line comment before json*/\"hello\"", "This is a multi-line comment before json", 44)]
        [InlineData("\"hello\"/*This is a multi-line comment after json*/", "This is a multi-line comment after json", 50)]
        [InlineData("\"alpha\" \r\n/*This is a multi-line comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a multi-line comment after json", 53)]
        [InlineData("\"beta\" \r\n/*This is a multi-line comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a multi-line comment after json", 52)]
        [InlineData("\"gamma\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a multi-line comment after json", 53)]
        [InlineData("\"delta\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a multi-line comment after json", 53)]
        [InlineData("{\"age\" : \n/*This is a comment between key-value pairs*/ 30}", "This is a comment between key-value pairs", 55)]
        [InlineData("{\"age\" : 30/*This is a comment between key-value pairs on the same line*/}", "This is a comment between key-value pairs on the same line", 73)]

        [InlineData("/*This is a split multi-line \ncomment before json*/\"hello\"", "This is a split multi-line \ncomment before json", 51)]
        [InlineData("\"hello\"/*This is a split multi-line \ncomment after json*/", "This is a split multi-line \ncomment after json", 57)]
        [InlineData("\"alpha\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a split multi-line \ncomment after json", 60)]
        [InlineData("\"beta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a split multi-line \ncomment after json", 59)]
        [InlineData("\"gamma\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a split multi-line \ncomment after json", 60)]
        [InlineData("\"delta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a split multi-line \ncomment after json", 60)]
        [InlineData("{\"age\" : \n/*This is a split multi-line \ncomment between key-value pairs*/ 30}", "This is a split multi-line \ncomment between key-value pairs", 73)]
        [InlineData("{\"age\" : 30/*This is a split multi-line \ncomment between key-value pairs on the same line*/}", "This is a split multi-line \ncomment between key-value pairs on the same line", 91)]
        public static void AllowComments(string jsonString, string expectedComment, int expectedIndex)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8)
            {
                Options = JsonReaderOptions.AllowComments
            };

            bool foundComment = false;
            long indexAfterFirstComment = 0;
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.Comment:
                        if (foundComment)
                            break;
                        foundComment = true;
                        indexAfterFirstComment = json.Consumed;
                        string actualComment = Encoding.UTF8.GetString(json.Value);
                        Assert.Equal(expectedComment, actualComment);
                        break;
                }
            }
            Assert.True(foundComment);
            Assert.Equal(expectedIndex, indexAfterFirstComment);

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new Utf8JsonReader(sequence)
                {
                    Options = JsonReaderOptions.AllowComments
                };

                foundComment = false;
                indexAfterFirstComment = 0;
                while (jsonMultiSegment.Read())
                {
                    JsonTokenType tokenType = jsonMultiSegment.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.Comment:
                            if (foundComment)
                                break;
                            foundComment = true;
                            indexAfterFirstComment = jsonMultiSegment.Consumed;
                            string actualComment = Encoding.UTF8.GetString(jsonMultiSegment.Value);
                            Assert.Equal(expectedComment, actualComment);
                            break;
                    }
                }
                Assert.True(foundComment);
                Assert.Equal(expectedIndex, indexAfterFirstComment);
            }
        }

        [Theory]
        [InlineData("//", 2)]
        [InlineData("//\n", 3)]
        [InlineData("/**/", 4)]
        [InlineData("/*/*/",5)]

        [InlineData("//This is a comment before json\n\"hello\"", 32)]
        [InlineData("\"hello\"//This is a comment after json", 37)]
        [InlineData("\"hello\"//This is a comment after json\n", 38)]
        [InlineData("\"alpha\" \r\n//This is a comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", 41)]
        [InlineData("\"beta\" \r\n//This is a comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 40)]
        [InlineData("\"gamma\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", 41)]
        [InlineData("\"delta\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 41)]
        [InlineData("\"hello\"//This is a comment after json with new line\n", 52)]
        [InlineData("{\"age\" : \n//This is a comment between key-value pairs\n 30}", 54)]
        [InlineData("{\"age\" : 30//This is a comment between key-value pairs on the same line\n}", 72)]

        [InlineData("/*This is a multi-line comment before json*/\"hello\"", 44)]
        [InlineData("\"hello\"/*This is a multi-line comment after json*/", 50)]
        [InlineData("\"alpha\" \r\n/*This is a multi-line comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 53)]
        [InlineData("\"beta\" \r\n/*This is a multi-line comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 52)]
        [InlineData("\"gamma\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 53)]
        [InlineData("\"delta\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 53)]
        [InlineData("{\"age\" : \n/*This is a comment between key-value pairs*/ 30}", 55)]
        [InlineData("{\"age\" : 30/*This is a comment between key-value pairs on the same line*/}", 73)]

        [InlineData("/*This is a split multi-line \ncomment before json*/\"hello\"", 51)]
        [InlineData("\"hello\"/*This is a split multi-line \ncomment after json*/", 57)]
        [InlineData("\"alpha\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 60)]
        [InlineData("\"beta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 59)]
        [InlineData("\"gamma\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 60)]
        [InlineData("\"delta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 60)]
        [InlineData("{\"age\" : \n/*This is a split multi-line \ncomment between key-value pairs*/ 30}", 73)]
        [InlineData("{\"age\" : 30/*This is a split multi-line \ncomment between key-value pairs on the same line*/}", 91)]
        public static void SkipComments(string jsonString, int expectedConsumed)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8)
            {
                Options = JsonReaderOptions.SkipComments
            };

            JsonTokenType prevTokenType = JsonTokenType.None;
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.Comment:
                        Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                        break;
                }
                Assert.NotEqual(tokenType, prevTokenType);
                prevTokenType = tokenType;
            }
            Assert.Equal(dataUtf8.Length, json.Consumed);

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new Utf8JsonReader(sequence)
                {
                    Options = JsonReaderOptions.SkipComments
                };
                prevTokenType = JsonTokenType.None;
                while (jsonMultiSegment.Read())
                {
                    JsonTokenType tokenType = jsonMultiSegment.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.Comment:
                            Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                            break;
                    }
                    Assert.NotEqual(tokenType, prevTokenType);
                    prevTokenType = tokenType;
                }
                Assert.Equal(dataUtf8.Length, json.Consumed);
            }
        }

        [Theory]
        [InlineData("//", 1, 0)]
        [InlineData("//\n", 1, 0)]
        [InlineData("/**/", 1, 0)]
        [InlineData("/*/*/", 1, 0)]

        [InlineData("//This is a comment before json\n\"hello\"", 1, 0)]
        [InlineData("\"hello\"//This is a comment after json", 1, 7)]
        [InlineData("\"hello\"//This is a comment after json\n", 1, 7)]
        [InlineData("\"alpha\" \r\n//This is a comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"beta\" \r\n//This is a comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"gamma\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"delta\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"hello\"//This is a comment after json with new line\n", 1, 7)]
        [InlineData("{\"age\" : \n//This is a comment between key-value pairs\n 30}", 2, 0)]
        [InlineData("{\"age\" : 30//This is a comment between key-value pairs on the same line\n}", 1, 11)]

        [InlineData("/*This is a multi-line comment before json*/\"hello\"", 1, 0)]
        [InlineData("\"hello\"/*This is a multi-line comment after json*/", 1, 7)]
        [InlineData("\"alpha\" \r\n/*This is a multi-line comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"beta\" \r\n/*This is a multi-line comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"gamma\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"delta\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("{\"age\" : \n/*This is a comment between key-value pairs*/ 30}", 2, 0)]
        [InlineData("{\"age\" : 30/*This is a comment between key-value pairs on the same line*/}", 1, 11)]

        [InlineData("/*This is a split multi-line \ncomment before json*/\"hello\"", 1, 0)]
        [InlineData("\"hello\"/*This is a split multi-line \ncomment after json*/", 1, 7)]
        [InlineData("\"alpha\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"beta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"gamma\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"delta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("{\"age\" : \n/*This is a split multi-line \ncomment between key-value pairs*/ 30}", 2, 0)]
        [InlineData("{\"age\" : 30/*This is a split multi-line \ncomment between key-value pairs on the same line*/}", 1, 11)]
        public static void CommentsAreInvalidByDefault(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8);

            try
            {
                while (json.Read())
                {
                    JsonTokenType tokenType = json.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.Comment:
                            Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                            break;
                    }
                }
                Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.LinePosition);
            }

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new Utf8JsonReader(sequence);
                try
                {
                    while (jsonMultiSegment.Read())
                    {
                        JsonTokenType tokenType = jsonMultiSegment.TokenType;
                        switch (tokenType)
                        {
                            case JsonTokenType.Comment:
                                Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                                break;
                        }
                    }
                    Assert.True(false, "Expected JsonReaderException was not thrown with multi-segment data.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedPosition, ex.LinePosition);
                }
            }
        }

        [Theory]
        [InlineData("//\n}", 2, 0)]
        [InlineData("//comment\n}", 2, 0)]
        [InlineData("/**/}", 1, 4)]
        [InlineData("/*\n*/}", 2, 2)]
        [InlineData("/*comment\n*/}", 2, 2)]
        [InlineData("/*/*/}", 1, 5)]
        [InlineData("//This is a comment before json\n\"hello\"{", 2, 7)]
        [InlineData("\"hello\"//This is a comment after json\n{", 2, 0)]
        [InlineData("\"gamma\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*/{//Another single-line comment", 4, 28)]
        [InlineData("\"delta\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/{", 5, 18)]
        [InlineData("\"hello\"//This is a comment after json with new line\n{", 2, 0)]
        [InlineData("{\"age\" : \n//This is a comment between key-value pairs\n 30}{", 3, 4)]
        [InlineData("{\"age\" : 30//This is a comment between key-value pairs on the same line\n}{", 2, 1)]
        [InlineData("/*This is a multi-line comment before json*/\"hello\"{", 1, 51)]
        [InlineData("\"hello\"/*This is a multi-line comment after json*/{", 1, 50)]
        [InlineData("\"gamma\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*/{//Another single-line comment", 3, 28)]
        [InlineData("\"delta\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/{", 4, 18)]
        [InlineData("{\"age\" : \n/*This is a comment between key-value pairs*/ 30}{", 2, 49)]
        [InlineData("{\"age\" : 30/*This is a comment between key-value pairs on the same line*/}{", 1, 74)]
        [InlineData("/*This is a split multi-line \ncomment before json*/\"hello\"{", 2, 28)]
        [InlineData("\"hello\"/*This is a split multi-line \ncomment after json*/{", 2, 20)]
        [InlineData("\"gamma\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*/{//Another single-line comment", 4, 28)]
        [InlineData("\"delta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/{", 5, 18)]
        [InlineData("{\"age\" : \n/*This is a split multi-line \ncomment between key-value pairs*/ 30}{", 3, 37)]
        [InlineData("{\"age\" : 30/*This is a split multi-line \ncomment between key-value pairs on the same line*/}{", 2, 51)]
        public static void InvalidJsonWithComments(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new Utf8JsonReader(dataUtf8)
            {
                Options = JsonReaderOptions.AllowComments
            };

            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.LinePosition);
            }

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new Utf8JsonReader(sequence)
                {
                    Options = JsonReaderOptions.AllowComments
                };

                try
                {
                    while (jsonMultiSegment.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown with multi-segment data.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedPosition, ex.LinePosition);
                }
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

        [Fact]
        public void TestingNumbers()
        {
            var random = new Random(42);

            // Make sure we have 1_005 values in each numeric list.
            var ints = new List<int>
            {
                0,
                12345,
                -12345,
                int.MaxValue,
                int.MinValue
            };
            for (int i = 0; i < 1_000; i++)
            {
                int value = random.Next(int.MinValue, int.MaxValue);
                ints.Add(value);
            }

            var longs = new List<long>
            {
                0,
                12345678901,
                -12345678901,
                long.MaxValue,
                long.MinValue
            };
            for (int i = 0; i < 1_000; i++)
            {
                long value = random.Next(int.MinValue, int.MaxValue);
                if (value < 0)
                    value += int.MinValue;
                else
                    value += int.MaxValue;
                longs.Add(value);
            }

            var doubles = new List<double>
            {
                0.000,
                1.1234e1,
                -1.1234e1,
                1.79769313486231E+308,  // double.MaxValue doesn't round trip
                -1.79769313486231E+308  // double.MinValue doesn't round trip
            };
            for (int i = 0; i < 500; i++)
            {
                double value = NextDouble(random, double.MinValue / 10, double.MaxValue / 10);
                doubles.Add(value);
            }
            for (int i = 0; i < 500; i++)
            {
                double value = NextDouble(random, 1_000_000, -1_000_000);
                doubles.Add(value);
            }

            var floats = new List<float>
            {
                0.000f,
                1.1234e1f,
                -1.1234e1f,
                float.MaxValue,
                float.MinValue
            };
            for (int i = 0; i < 1_000; i++)
            {
                float value = NextFloat(random);
                floats.Add(value);
            }

            var decimals = new List<decimal>
            {
                (decimal)0.000,
                (decimal)1.1234e1,
                (decimal)-1.1234e1,
                decimal.MaxValue,
                decimal.MinValue
            };
            for (int i = 0; i < 500; i++)
            {
                decimal value = NextDecimal(random, 78E14, -78E14);
                decimals.Add(value);
            }
            for (int i = 0; i < 500; i++)
            {
                decimal value = NextDecimal(random, 1_000_000, -1_000_000);
                decimals.Add(value);
            }

            var builder = new StringBuilder();
            builder.Append("{");

            for (int i = 0; i < ints.Count; i++)
            {
                builder.Append("\"int").Append(i).Append("\": ");
                builder.Append(ints[i]).Append(", ");
            }
            for (int i = 0; i < longs.Count; i++)
            {
                builder.Append("\"long").Append(i).Append("\": ");
                builder.Append(longs[i]).Append(", ");
            }
            for (int i = 0; i < doubles.Count; i++)
            {
                builder.Append("\"double").Append(i).Append("\": ");
                builder.Append(doubles[i]).Append(", ");
            }
            for (int i = 0; i < floats.Count; i++)
            {
                builder.Append("\"float").Append(i).Append("\": ");
                builder.Append(floats[i]).Append(", ");
            }
            for (int i = 0; i < decimals.Count; i++)
            {
                builder.Append("\"decimal").Append(i).Append("\": ");
                builder.Append(decimals[i]).Append(", ");
            }

            builder.Append("\"intEnd\": 0}");

            string jsonString = builder.ToString();
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            var json = new Utf8JsonReader(dataUtf8);
            string key = "";
            int count = 0;
            while (json.Read())
            {
                if (json.TokenType == JsonTokenType.PropertyName)
                {
                    key = json.GetValueAsString();
                }
                if (json.TokenType == JsonTokenType.Number)
                {
                    if (key.StartsWith("int"))
                    {
                        int numberInt = json.GetValueAsInt32();
                        if (count >= ints.Count)
                            count = 0;
                        Assert.Equal(ints[count], numberInt);
                        count++;
                    }
                    else if (key.StartsWith("long"))
                    {
                        long numberLong = json.GetValueAsInt64();
                        if (count >= longs.Count)
                            count = 0;
                        Assert.Equal(longs[count], numberLong);
                        count++;
                    }
                    else if (key.StartsWith("float"))
                    {
                        float numberFloat = json.GetValueAsSingle();
                        if (count >= floats.Count)
                            count = 0;

                        float expected = float.Parse(floats[count].ToString());

                        Assert.Equal(expected, numberFloat);
                        count++;
                    }
                    else if (key.StartsWith("double"))
                    {
                        double numberDouble = json.GetValueAsDouble();
                        if (count >= doubles.Count)
                            count = 0;

                        double expected = double.Parse(doubles[count].ToString());

                        Assert.Equal(expected, numberDouble);
                        count++;
                    }
                    else if (key.StartsWith("decimal"))
                    {
                        decimal numberDecimal = json.GetValueAsDecimal();
                        if (count >= decimals.Count)
                            count = 0;
                        Assert.Equal(decimals[count], numberDecimal);
                        count++;
                    }
                }
            }

            Assert.Equal(dataUtf8.Length, json.Consumed);
        }

        [Theory]
        [InlineData("0.000", 0)]
        [InlineData("1e1", 10)]
        [InlineData("1.1e2", 110)]
        [InlineData("12345.1", 12345.1)]
        [InlineData("12345678901", 12345678901)]
        [InlineData("123456789012345678901", 123456789012345678901d)]
        public void TestingNumbersInvalidCastToInt32(string jsonString, double expected)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            var json = new Utf8JsonReader(dataUtf8);
            while (json.Read())
            {
                if (json.TokenType == JsonTokenType.Number)
                {
                    try
                    {
                        int value = json.GetValueAsInt32();
                        Assert.True(false, "Expected InvalidCastException when trying to read invalid int32.");
                    }
                    catch (InvalidCastException)
                    {
                        double value = json.GetValueAsDouble();
                        Assert.Equal(expected, value);
                    }
                }
            }

            Assert.Equal(dataUtf8.Length, json.Consumed);
        }

        [Theory]
        [InlineData("0.000", 0)]
        [InlineData("1e1", 10)]
        [InlineData("1.1e2", 110)]
        [InlineData("12345.1", 12345.1)]
        [InlineData("123456789012345678901", 123456789012345678901d)]
        public void TestingNumbersInvalidCastToInt64(string jsonString, double expected)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            var json = new Utf8JsonReader(dataUtf8);
            while (json.Read())
            {
                if (json.TokenType == JsonTokenType.Number)
                {
                    try
                    {
                        long value = json.GetValueAsInt64();
                        Assert.True(false, "Expected InvalidCastException when trying to read invalid int64.");
                    }
                    catch (InvalidCastException)
                    {
                        double value = json.GetValueAsDouble();
                        Assert.Equal(expected, value);
                    }
                }
            }

            Assert.Equal(dataUtf8.Length, json.Consumed);
        }
    }
}
