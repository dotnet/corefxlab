// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Buffers;
using System.Buffers.Tests;
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

            //TODO: Fix Utf8JsonReaderStream based on changes to Utf8JsonReader
            //byte[] resultStream = JsonLabStreamReturnBytesHelper(dataUtf8, out length);
            //string actualStrStream = Encoding.UTF8.GetString(resultStream.AsSpan(0, length));

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expectedStr, actualStr);
            Assert.Equal(expectedStr, actualStrSequence);
            //TODO: Fix Utf8JsonReaderStream based on changes to Utf8JsonReader
            //Assert.Equal(expectedStr, actualStrStream);

            // Json payload contains numbers that are too large for .NET (need BigInteger+)
            if (type != TestCaseType.FullSchema1 && type != TestCaseType.BasicLargeNum)
            {
                object jsonValues = JsonLabReturnObjectHelper(dataUtf8);
                string s = ObjectToString(jsonValues);
                Assert.Equal(expectedStr.Substring(0, expectedStr.Length - 2), s.Substring(0, s.Length - 2));
            }

            result = JsonLabReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments));
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments));
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);
            //Assert.Equal(expectedStr, actualStrSequence);

            result = JsonLabReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments));
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments));
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

            result = JsonLabReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments));
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments));
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            //Assert.Equal(actualStr, actualStrSequence);

            result = JsonLabReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments));
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length, new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments));
            actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Assert.Equal(actualStr, actualStrSequence);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestJsonReaderUtf8SegmentSizeOne(bool compactData, TestCaseType type, string jsonString)
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

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            ReadOnlySequence<byte> sequence = GetSequence(dataUtf8, 1);

            // Skipping really large JSON since slicing them (O(n^2)) is too slow.
            if (type == TestCaseType.Json40KB || type == TestCaseType.Json400KB || type == TestCaseType.ProjectLockJson)
            {
                var utf8JsonReader = new JsonUtf8Reader(sequence);
                byte[] resultSequence = JsonLabReaderLoop(dataUtf8.Length, out int length, ref utf8JsonReader);
                string actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));
                Assert.Equal(expectedStr, actualStrSequence);
                return;
            }

            for (int j = 0; j < dataUtf8.Length; j++)
            {
                var utf8JsonReader = new JsonUtf8Reader(sequence.Slice(0, j), isFinalBlock: false);
                byte[] resultSequence = JsonLabReaderLoop(dataUtf8.Length, out int length, ref utf8JsonReader);
                string actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

                long consumed = utf8JsonReader.BytesConsumed;
                Assert.Equal(consumed, utf8JsonReader.CurrentState.BytesConsumed);
                utf8JsonReader = new JsonUtf8Reader(sequence.Slice(consumed), isFinalBlock: true, utf8JsonReader.CurrentState);
                resultSequence = JsonLabReaderLoop(dataUtf8.Length, out length, ref utf8JsonReader);
                actualStrSequence += Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));
                string message = $"Expected consumed: {dataUtf8.Length - consumed}, Actual consumed: {utf8JsonReader.BytesConsumed}, Index: {j}";
                Assert.Equal(utf8JsonReader.BytesConsumed, utf8JsonReader.CurrentState.BytesConsumed);
                Assert.True(dataUtf8.Length - consumed == utf8JsonReader.BytesConsumed, message);
                Assert.Equal(expectedStr, actualStrSequence);
            }
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public static void TestJsonReaderUtf8NBytesAtaTime(bool compactData, TestCaseType type, string jsonString)
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

            int[] numBytes = new int[] { 1, 10, 100, 1_000 };

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            for (int i = 0; i < numBytes.Length; i++)
            {
                JsonReaderState jsonState = default;
                int consumed = 0;

                if (numBytes[i] >= dataUtf8.Length)
                    numBytes[i] = dataUtf8.Length - 1;

                int numberOfBytes = numBytes[i];
                bool isFinalBlock = false;
                string actualStr = "";
                while (consumed != dataUtf8.Length)
                {
                    ReadOnlySpan<byte> data = dataUtf8.AsSpan();

                    if (isFinalBlock)
                    {
                        data = data.Slice(consumed);
                    }
                    else
                    {
                        data = data.Slice(consumed, numberOfBytes);
                    }

                    var utf8JsonReader = new JsonUtf8Reader(data, isFinalBlock, jsonState);

                    byte[] result = JsonLabReaderLoop((numberOfBytes * 2) + 128, out int length, ref utf8JsonReader);
                    actualStr += Encoding.UTF8.GetString(result.AsSpan(0, length));

                    if (utf8JsonReader.BytesConsumed == 0)
                        numberOfBytes++;
                    else
                        numberOfBytes = numBytes[i];

                    consumed += (int)utf8JsonReader.BytesConsumed;
                    Assert.Equal(utf8JsonReader.BytesConsumed, utf8JsonReader.CurrentState.BytesConsumed);
                    jsonState = utf8JsonReader.CurrentState;
                    if (consumed >= dataUtf8.Length - numBytes[i])
                        isFinalBlock = true;
                }

                Assert.Equal(expectedStr, actualStr);
            }
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
                var json = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                byte[] output = JsonLabReaderLoop(outputSpan.Length, out int firstLength, ref json);
                output.AsSpan(0, firstLength).CopyTo(outputSpan);
                int written = firstLength;

                long consumed = json.BytesConsumed;
                Assert.Equal(consumed, json.CurrentState.BytesConsumed);
                JsonReaderState jsonState = json.CurrentState;

                // Skipping large JSON since slicing them (O(n^3)) is too slow.
                if (type == TestCaseType.DeepTree || type == TestCaseType.BroadTree || type == TestCaseType.LotsOfNumbers
                    || type == TestCaseType.LotsOfStrings || type == TestCaseType.Json4KB)
                {
                    json = new JsonUtf8Reader(dataUtf8.AsSpan((int)consumed), isFinalBlock: true, jsonState);
                    output = JsonLabReaderLoop(outputSpan.Length - written, out int length, ref json);
                    output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                    written += length;
                    Assert.Equal(dataUtf8.Length - consumed, json.BytesConsumed);
                    Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);

                    Assert.Equal(outputSpan.Length, written);
                    string actualStr = Encoding.UTF8.GetString(outputSpan);
                    Assert.Equal(expectedStr, actualStr);
                }
                else
                {
                    for (long j = consumed; j < dataUtf8.Length - consumed; j++)
                    {
                        written = firstLength;
                        json = new JsonUtf8Reader(dataUtf8.AsSpan((int)consumed, (int)j), isFinalBlock: false, jsonState);
                        output = JsonLabReaderLoop(outputSpan.Length - written, out int length, ref json);
                        output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                        written += length;

                        long consumedInner = json.BytesConsumed;
                        Assert.Equal(consumedInner, json.CurrentState.BytesConsumed);
                        json = new JsonUtf8Reader(dataUtf8.AsSpan((int)(consumed + consumedInner)), isFinalBlock: true, json.CurrentState);
                        output = JsonLabReaderLoop(outputSpan.Length - written, out length, ref json);
                        output.AsSpan(0, length).CopyTo(outputSpan.Slice(written));
                        written += length;
                        Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.BytesConsumed);
                        Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);

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
                var json = new JsonUtf8Reader(sequences[i]);
                while (json.Read()) ;
                Assert.Equal(sequences[i].Length, json.BytesConsumed);
                Assert.Equal(sequences[i].Length, json.CurrentState.BytesConsumed);
            }

            for (int i = 0; i < sequences.Count; i++)
            {
                ReadOnlySequence<byte> sequence = sequences[i];
                for (int j = 0; j < dataUtf8.Length; j++)
                {
                    var json = new JsonUtf8Reader(sequence.Slice(0, j), isFinalBlock: false);
                    while (json.Read()) ;

                    long consumed = json.BytesConsumed;
                    JsonReaderState jsonState = json.CurrentState;
                    json = new JsonUtf8Reader(sequence.Slice(consumed), isFinalBlock: true, json.CurrentState);
                    while (json.Read()) ;
                    Assert.Equal(dataUtf8.Length - consumed, json.BytesConsumed);
                    Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
                }
            }
        }

        [Theory]
        [MemberData(nameof(SpecialNumTestCases))]
        public static void TestPartialJsonReaderSpecialNumbers(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                for (int i = 0; i < dataUtf8.Length; i++)
                {
                    var json = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false)
                    {
                        Options = new JsonReaderOptions(commentHandling)
                    };
                    while (json.Read()) ;

                    long consumed = json.BytesConsumed;
                    Assert.Equal(consumed, json.CurrentState.BytesConsumed);
                    JsonReaderState jsonState = json.CurrentState;
                    for (long j = consumed; j < dataUtf8.Length - consumed; j++)
                    {
                        json = new JsonUtf8Reader(dataUtf8.AsSpan((int)consumed, (int)j), isFinalBlock: false, jsonState)
                        {
                            Options = new JsonReaderOptions(commentHandling)
                        };
                        while (json.Read()) ;

                        long consumedInner = json.BytesConsumed;
                        Assert.Equal(consumedInner, json.CurrentState.BytesConsumed);
                        json = new JsonUtf8Reader(dataUtf8.AsSpan((int)(consumed + consumedInner)), isFinalBlock: true, json.CurrentState)
                        {
                            Options = new JsonReaderOptions(commentHandling)
                        };
                        while (json.Read()) ;
                        Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.BytesConsumed);
                        Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
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
                var json = new JsonUtf8Reader(data)
                {
                    MaxDepth = depth
                };

                int actualDepth = 0;
                while (json.Read())
                {
                    if (json.TokenType >= JsonTokenType.String && json.TokenType <= JsonTokenType.Null)
                        actualDepth = json.CurrentDepth;
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
            var json = new JsonUtf8Reader(data)
            {
                MaxDepth = depth - 1
            };

            try
            {
                int maxDepth = 0;
                while (json.Read())
                {
                    if (maxDepth < json.CurrentDepth)
                        maxDepth = json.CurrentDepth;
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
            var json = new JsonUtf8Reader(data);
            try
            {
                json.MaxDepth = depth;
                Assert.True(false, "Expected ArgumentException was not thrown. Max depth must be set to greater than 0.");
            }
            catch (ArgumentException)
            { }
        }

        [Theory]
        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", JsonReaderOptions.CommentHandling.Default, "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            JsonReaderOptions.CommentHandling.Default,
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\\r\\n String\\r\\n, \\tMul\\r\\ntiline String, \\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]

        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", JsonReaderOptions.CommentHandling.AllowComments, "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            JsonReaderOptions.CommentHandling.AllowComments,
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\\r\\n String\\r\\n, \\tMul\\r\\ntiline String, \\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]

        [InlineData("{\"nam\\\"e\":\"ah\\\"son\"}", JsonReaderOptions.CommentHandling.SkipComments, "nam\\\"e, ah\\\"son, ")]
        [InlineData("{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}",
            JsonReaderOptions.CommentHandling.SkipComments,
            "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\\r\\n String\\r\\n, \\tMul\\r\\ntiline String, \\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ")]
        public static void TestJsonReaderUtf8SpecialString(string jsonString, JsonReaderOptions.CommentHandling commentHandling, string expectedStr)
        {
            JsonReaderOptions option = new JsonReaderOptions(commentHandling);
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
        [InlineData("  \"h漢字ello\"  ")]
        [InlineData("  \"he\\r\\n\\\"l\\\\\\\"lo\\\\\"  ")]
        [InlineData("  12345  ")]
        [InlineData("  null  ")]
        [InlineData("  true  ")]
        [InlineData("  false  ")]
        public static void SingleJsonValue(string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                for (int i = 0; i < dataUtf8.Length; i++)
                {
                    var json = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), false)
                    {
                        Options = new JsonReaderOptions(commentHandling)
                    };
                    while (json.Read()) ;

                    long consumed = json.BytesConsumed;
                    Assert.Equal(consumed, json.CurrentState.BytesConsumed);
                    json = new JsonUtf8Reader(dataUtf8.AsSpan((int)consumed), true, json.CurrentState)
                    {
                        Options = new JsonReaderOptions(commentHandling)
                    };
                    while (json.Read()) ;
                    Assert.Equal(dataUtf8.Length - consumed, json.BytesConsumed);
                    Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
                }
            }
        }

        [Theory]
        [InlineData("\"h漢字ello\"", 1, 0)] // "\""
        [InlineData("12345", 3, 0)]   // "123"
        [InlineData("null", 3, 0)]   // "nul"
        [InlineData("true", 3, 0)]   // "tru"
        [InlineData("false", 4, 0)]  // "fals"
        [InlineData("   {\"a漢字ge\":30}   ", 16, 16)] // "   {\"a漢字ge\":"
        [InlineData("{\"n漢字ame\":\"A漢字hson\"}", 15, 14)]  // "{\"n漢字ame\":\"A漢字hso"
        [InlineData("-123456789", 1, 0)] // "-"
        [InlineData("0.5", 2, 0)]    // "0."
        [InlineData("10.5e+3", 5, 0)] // "10.5e"
        [InlineData("10.5e-1", 6, 0)]    // "10.5e-"
        [InlineData("{\"i漢字nts\":[1, 2, 3, 4, 5]}", 27, 25)]    // "{\"i漢字nts\":[1, 2, 3, 4, "
        [InlineData("{\"s漢字trings\":[\"a漢字bc\", \"def\"], \"ints\":[1, 2, 3, 4, 5]}", 36, 36)]  // "{\"s漢字trings\":[\"a漢字bc\", \"def\""
        [InlineData("{\"a漢字ge\":30, \"name\":\"test}:[]\", \"another 漢字string\" : \"tests\"}", 25, 24)]   // "{\"a漢字ge\":30, \"name\":\"test}"
        [InlineData("   [[[[{\r\n\"t漢字emp1\":[[[[{\"t漢字emp2:[]}]]]]}]]]]\":[]}]]]]}]]]]   ", 54, 29)] // "   [[[[{\r\n\"t漢字emp1\":[[[[{\"t漢字emp2:[]}]]]]}]]]]"
        [InlineData("{\r\n\"is漢字Active\": false, \"in漢字valid\"\r\n : \"now its 漢字valid\"}", 26, 26)]  // "{\r\n\"is漢字Active\": false, \"in漢字valid\"\r\n}"
        public static void PartialJson(string jsonString, int splitLocation, int consumed)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                var json = new JsonUtf8Reader(dataUtf8.AsSpan(0, splitLocation), false)
                {
                    Options = new JsonReaderOptions(commentHandling)
                };
                while (json.Read()) ;
                Assert.Equal(consumed, json.BytesConsumed);
                Assert.Equal(consumed, json.CurrentState.BytesConsumed);

                json = new JsonUtf8Reader(dataUtf8.AsSpan((int)json.BytesConsumed), true, json.CurrentState)
                {
                    Options = new JsonReaderOptions(commentHandling)
                };
                while (json.Read()) ;
                Assert.Equal(dataUtf8.Length - consumed, json.BytesConsumed);
                Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
            }
        }

        [Fact]
        public static void PartialJsonWithWhiteSpace()
        {
            string jsonString = "   {   \r   \n   \"   is   Active   \"   :    false   ,    \"   na   me   \"   \r   \n    :    \"   John    Doe   \"   }   ";
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var json = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                var originalDictionary = new Dictionary<string, object>();
                string originalKey = "";
                object originalValue = null;
                SetKeyValues(ref json, originalDictionary, ref originalKey, ref originalValue);

                long consumed = json.BytesConsumed;
                Assert.Equal(consumed, json.CurrentState.BytesConsumed);
                JsonReaderState jsonState = json.CurrentState;
                for (long j = consumed; j < dataUtf8.Length - consumed; j++)
                {
                    string key = originalKey;
                    object value = originalValue;
                    var dictionary = new Dictionary<string, object>(originalDictionary);
                    json = new JsonUtf8Reader(dataUtf8.AsSpan((int)consumed, (int)j), isFinalBlock: false, jsonState);
                    SetKeyValues(ref json, dictionary, ref key, ref value);

                    long consumedInner = json.BytesConsumed;
                    Assert.Equal(consumedInner, json.CurrentState.BytesConsumed);
                    json = new JsonUtf8Reader(dataUtf8.AsSpan((int)(consumed + consumedInner)), isFinalBlock: true, json.CurrentState);
                    SetKeyValues(ref json, dictionary, ref key, ref value);
                    Assert.Equal(dataUtf8.Length - consumedInner - consumed, json.BytesConsumed);
                    Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);

                    Assert.True(dictionary.TryGetValue("   is   Active   ", out object value1));
                    Assert.Equal(false.ToString(), value1.ToString());
                    Assert.True(dictionary.TryGetValue("   na   me   ", out object value2));
                    Assert.Equal("   John    Doe   ", value2.ToString());
                }
            }
        }

        [Theory]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false \"in漢字valid\"\r\n}", 30, 30, 3, 21)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false \"in漢字valid\"\r\n}", 31, 31, 3, 21)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false, \"in漢字valid\"\r\n}", 30, 30, 4, 0)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false, \"in漢字valid\"\r\n}", 31, 30, 4, 0)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false, \"in漢字valid\"\r\n}", 32, 30, 4, 0)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false, 5\r\n}", 30, 30, 3, 22)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false, 5\r\n}", 31, 30, 3, 22)]
        [InlineData("{\r\n\"is\\r\\nAct漢字ive\": false, 5\r\n}", 32, 30, 3, 22)]
        public static void InvalidJsonSplitRemainsInvalid(string jsonString, int splitLocation, int consumed, int expectedlineNumber, int expectedBytePosition)
        {
            //TODO: Test multi-segment json payload
            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
                var json = new JsonUtf8Reader(dataUtf8.AsSpan(0, splitLocation), false)
                {
                    Options = new JsonReaderOptions(commentHandling)
                };
                while (json.Read()) ;
                Assert.Equal(consumed, json.BytesConsumed);
                Assert.Equal(consumed, json.CurrentState.BytesConsumed);

                json = new JsonUtf8Reader(dataUtf8.AsSpan((int)json.BytesConsumed), true, json.CurrentState)
                {
                    Options = new JsonReaderOptions(commentHandling)
                };
                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedBytePosition, ex.LineBytePosition);
                }
            }
        }

        [Theory]
        [InlineData("{]", 1, 1)]
        [InlineData("[}", 1, 1)]
        [InlineData("nulz", 1, 3)]
        [InlineData("truz", 1, 3)]
        [InlineData("falsz", 1, 4)]
        [InlineData("\"a漢字ge\":", 1, 11)]
        [InlineData("12345.1.", 1, 7)]
        [InlineData("-f", 1, 1)]
        [InlineData("1.f", 1, 2)]
        [InlineData("0.1f", 1, 3)]
        [InlineData("0.1e1f", 1, 5)]
        [InlineData("123,", 1, 3)]
        [InlineData("01", 1, 1)]
        [InlineData("-01", 1, 2)]
        [InlineData("10.5e-0.2", 1, 7)]
        [InlineData("{\"a漢字ge\":30, \"ints\":[1, 2, 3, 4, 5.1e7.3]}", 1, 42)]
        [InlineData("{\"a漢字ge\":30, \r\n \"num\":-0.e, \r\n \"ints\":[1, 2, 3, 4, 5]}", 2, 10)]
        [InlineData("{{}}", 1, 1)]
        [InlineData("[[{{}}]]", 1, 3)]
        [InlineData("[1, 2, 3, ]", 1, 10)]
        [InlineData("{\"a漢字ge\":30, \"ints\":[1, 2, 3, 4, 5}}", 1, 38)]
        [InlineData("{\r\n\"isActive\": false \"\r\n}", 2, 18)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2\":[}]]]]}]]]]", 2, 28)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2\":[]},[}]]]]}]]]]", 2, 32)]
        [InlineData("{\r\n\t\"isActive\": false,\r\n\t\"array\": [\r\n\t\t[{\r\n\t\t\t\"id\": 1\r\n\t\t}]\r\n\t]\r\n}", 4, 3, 3)]
        [InlineData("{\"Here is a 漢字string: \\\"\\\"\":\"Here is 漢字a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
        public static void InvalidJsonWhenPartial(string jsonString, int expectedlineNumber, int expectedBytePosition, int maxDepth = 64)
        {
            //TODO: Test multi-segment json payload
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                var json = new JsonUtf8Reader(dataUtf8, false)
                {
                    MaxDepth = maxDepth,
                    Options = new JsonReaderOptions(commentHandling)
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedBytePosition, ex.LineBytePosition);
                }
            }
        }

        [Theory]
        [InlineData("{\"text\": \"๏ แผ่นดินฮั่นเสื่อมโทรมแสนสังเวช\\uABCZ พระปกเกศกองบู๊กู้ขึ้นใหม่\"}", 1, 109)]
        [InlineData("{\"text\": \"๏ แผ่นดินฮั่นเสื่อมโ\\nทรมแสนสังเวช\\uABCZ พระปกเกศกองบู๊กู้ขึ้นใหม่\"}", 2, 41)]
        public static void PositionInCodeUnits(string jsonString, int expectedlineNumber, int expectedBytePosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                var json = new JsonUtf8Reader(dataUtf8, false)
                {
                    Options = new JsonReaderOptions(commentHandling)
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedBytePosition, ex.LineBytePosition);
                }
            }
        }

        [Theory]
        [InlineData("\"", 1, 0)]
        [InlineData("{]", 1, 1)]
        [InlineData("[}", 1, 1)]
        [InlineData("nul", 1, 3)]
        [InlineData("tru", 1, 3)]
        [InlineData("fals", 1, 4)]
        [InlineData("\"a漢字ge\":", 1, 11)]
        [InlineData("{\"a漢字ge\":", 1, 13)]
        [InlineData("{\"name\":\"A漢字hso", 1, 8)]
        [InlineData("12345.1.", 1, 7)]
        [InlineData("-", 1, 1)]
        [InlineData("-f", 1, 1)]
        [InlineData("1.f", 1, 2)]
        [InlineData("0.", 1, 2)]
        [InlineData("0.1f", 1, 3)]
        [InlineData("0.1e1f", 1, 5)]
        [InlineData("123,", 1, 3)]
        [InlineData("false,", 1, 5)]
        [InlineData("true,", 1, 4)]
        [InlineData("null,", 1, 4)]
        [InlineData("\"h漢字ello\",", 1, 13)]
        [InlineData("01", 1, 1)]
        [InlineData("1a", 1, 1)]
        [InlineData("-01", 1, 2)]
        [InlineData("10.5e", 1, 5)]
        [InlineData("10.5e-", 1, 6)]
        [InlineData("10.5e-0.2", 1, 7)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5.1e7.3]}", 1, 36)]
        [InlineData("{\"age\":30, \r\n \"num\":-0.e, \r\n \"ints\":[1, 2, 3, 4, 5]}", 2, 10)]
        [InlineData("{{}}", 1, 1)]
        [InlineData("[[{{}}]]", 1, 3)]
        [InlineData("[1, 2, 3, ]", 1, 10)]
        [InlineData("{\"ints\":[1, 2, 3, 4, 5", 1, 22)]
        [InlineData("{\"s漢字trings\":[\"a漢字bc\", \"def\"", 1, 36)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5}}", 1, 32)]
        [InlineData("{\"age\":30, \"name\":\"test}", 1, 18)]
        [InlineData("{\r\n\"isActive\": false \"\r\n}", 2, 18)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2\":[}]]]]}]]]]", 2, 28)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2:[]}]]]]}]]]]", 2, 19)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2\":[]},[}]]]]}]]]]", 2, 32)]
        [InlineData("{\r\n\t\"isActive\": false,\r\n\t\"array\": [\r\n\t\t[{\r\n\t\t\t\"id\": 1\r\n\t\t}]\r\n\t]\r\n}", 4, 3, 3)]
        [InlineData("{\"Here is a 漢字string: \\\"\\\"\":\"Here is 漢字a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
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
        public static void InvalidJson(string jsonString, int expectedlineNumber, int expectedBytePosition, int maxDepth = 64)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                var json = new JsonUtf8Reader(dataUtf8)
                {
                    MaxDepth = maxDepth,
                    Options = new JsonReaderOptions(commentHandling)
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedBytePosition, ex.LineBytePosition);
                }

                ReadOnlyMemory<byte> dataMemory = dataUtf8;
                for (int i = 0; i < dataMemory.Length; i++)
                {
                    var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                    ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                    BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                    var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                    var jsonMultiSegment = new JsonUtf8Reader(sequence)
                    {
                        MaxDepth = maxDepth,
                        Options = new JsonReaderOptions(commentHandling)
                    };

                    try
                    {
                        while (jsonMultiSegment.Read()) ;
                        Assert.True(false, $"Expected JsonReaderException was not thrown with multi-segment data. Index: {i}");
                    }
                    catch (JsonReaderException ex)
                    {
                        string errorMessage = $"expectedLineNumber: {expectedlineNumber} | actual: {ex.LineNumber} | index: {i} | option: {commentHandling}";
                        string firstSegmentString = Buffers.Text.Encodings.Utf8.ToString(dataMemory.Slice(0, i).Span);
                        string secondSegmentString = Buffers.Text.Encodings.Utf8.ToString(secondMem.Span);
                        errorMessage += " | " + firstSegmentString + " | " + secondSegmentString;
                        Assert.True(expectedlineNumber == ex.LineNumber, errorMessage);
                        errorMessage = $"expectedBytePosition: {expectedBytePosition} | actual: {ex.LineBytePosition} | index: {i} | option: {commentHandling}";
                        errorMessage += " | " + firstSegmentString + " | " + secondSegmentString;
                        Assert.True(expectedBytePosition == ex.LineBytePosition, errorMessage);
                    }
                }
            }
        }

        [Theory]
        [InlineData("\"", 1, 0)]
        [InlineData("{]", 1, 1)]
        [InlineData("[}", 1, 1)]
        [InlineData("nul", 1, 3)]
        [InlineData("tru", 1, 3)]
        [InlineData("fals", 1, 4)]
        [InlineData("\"a漢字ge\":", 1, 11)]
        [InlineData("{\"a漢字ge\":", 1, 13)]
        [InlineData("{\"name\":\"A漢字hso", 1, 8)]
        [InlineData("12345.1.", 1, 7)]
        [InlineData("-", 1, 1)]
        [InlineData("-f", 1, 1)]
        [InlineData("1.f", 1, 2)]
        [InlineData("0.", 1, 2)]
        [InlineData("0.1f", 1, 3)]
        [InlineData("0.1e1f", 1, 5)]
        [InlineData("123,", 1, 3)]
        [InlineData("false,", 1, 5)]
        [InlineData("true,", 1, 4)]
        [InlineData("null,", 1, 4)]
        [InlineData("\"h漢字ello\",", 1, 13)]
        [InlineData("01", 1, 1)]
        [InlineData("1a", 1, 1)]
        [InlineData("-01", 1, 2)]
        [InlineData("10.5e", 1, 5)]
        [InlineData("10.5e-", 1, 6)]
        [InlineData("10.5e-0.2", 1, 7)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5.1e7.3]}", 1, 36)]
        [InlineData("{\"age\":30, \r\n \"num\":-0.e, \r\n \"ints\":[1, 2, 3, 4, 5]}", 2, 10)]
        [InlineData("{{}}", 1, 1, 1)]
        [InlineData("[[{{}}]]", 1, 3)]
        [InlineData("[1, 2, 3, ]", 1, 10)]
        [InlineData("{\"ints\":[1, 2, 3, 4, 5", 1, 22)]
        [InlineData("{\"s漢字trings\":[\"a漢字bc\", \"def\"", 1, 36)]
        [InlineData("{\"age\":30, \"ints\":[1, 2, 3, 4, 5}}", 1, 32)]
        [InlineData("{\"age\":30, \"name\":\"test}", 1, 18)]
        [InlineData("{\r\n\"isActive\": false \"\r\n}", 2, 18)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2\":[}]]]]}]]]]", 2, 28)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2:[]}]]]]}]]]]", 2, 19)]
        [InlineData("[[[[{\r\n\"t漢字emp1\":[[[[{\"temp2\":[]},[}]]]]}]]]]", 2, 32)]
        [InlineData("{\r\n\t\"isActive\": false,\r\n\t\"array\": [\r\n\t\t[{\r\n\t\t\t\"id\": 1\r\n\t\t}]\r\n\t]\r\n}", 4, 3, 3)]
        [InlineData("{\"Here is a 漢字string: \\\"\\\"\":\"Here is 漢字a\",\"Here is a back slash\\\\\":[\"Multiline\\r\\n String\\r\\n\",\"\\tMul\\r\\ntiline String\",\"\\\"somequote\\\"\\tMu\\\"\\\"l\\r\\ntiline\\\"another\\\" String\\\\\"],\"str:\"\\\"\\\"\"}", 5, 35)]
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
        public static void InvalidJsonSingleSegment(string jsonString, int expectedlineNumber, int expectedBytePosition, int maxDepth = 64)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);

            foreach (JsonReaderOptions.CommentHandling commentHandling in Enum.GetValues(typeof(JsonReaderOptions.CommentHandling)))
            {
                var json = new JsonUtf8Reader(dataUtf8)
                {
                    MaxDepth = maxDepth,
                    Options = new JsonReaderOptions(commentHandling)
                };

                try
                {
                    while (json.Read()) ;
                    Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedBytePosition, ex.LineBytePosition);
                }

                for (int i = 0; i < dataUtf8.Length; i++)
                {
                    try
                    {
                        var jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false)
                        {
                            MaxDepth = maxDepth,
                            Options = new JsonReaderOptions(commentHandling)
                        };
                        while (jsonSlice.Read()) ;

                        long consumed = jsonSlice.BytesConsumed;
                        Assert.Equal(consumed, jsonSlice.CurrentState.BytesConsumed);
                        JsonReaderState jsonState = jsonSlice.CurrentState;

                        jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan((int)consumed), isFinalBlock: true, jsonState)
                        {
                            MaxDepth = maxDepth,
                            Options = new JsonReaderOptions(commentHandling)
                        };
                        while (jsonSlice.Read()) ;

                        Assert.True(false, "Expected JsonReaderException was not thrown with multi-segment data.");
                    }
                    catch (JsonReaderException ex)
                    {
                        string errorMessage = $"expectedLineNumber: {expectedlineNumber} | actual: {ex.LineNumber} | index: {i} | option: {commentHandling}";
                        string firstSegmentString = Buffers.Text.Encodings.Utf8.ToString(dataUtf8.AsSpan(0, i));
                        string secondSegmentString = Buffers.Text.Encodings.Utf8.ToString(dataUtf8.AsSpan(i));
                        errorMessage += " | " + firstSegmentString + " | " + secondSegmentString;
                        Assert.True(expectedlineNumber == ex.LineNumber, errorMessage);
                        errorMessage = $"expectedBytePosition: {expectedBytePosition} | actual: {ex.LineBytePosition} | index: {i} | option: {commentHandling}";
                        errorMessage += " | " + firstSegmentString + " | " + secondSegmentString;
                        Assert.True(expectedBytePosition == ex.LineBytePosition, errorMessage);
                    }
                }
            }
        }

        [Theory]
        [InlineData("//", "", 2)]
        [InlineData("//\n", "", 3)]
        [InlineData("/**/", "", 4)]
        [InlineData("/*/*/", "/", 5)]

        [InlineData("//T漢字his is a 漢字comment before json\n\"hello\"", "T漢字his is a 漢字comment before json", 44)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json", "This is a 漢字comment after json", 49)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json\n", "This is a 漢字comment after json", 50)]
        [InlineData("\"a漢字lpha\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a 漢字comment after json", 53)]
        [InlineData("\"b漢字eta\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a 漢字comment after json", 52)]
        [InlineData("\"g漢字amma\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a 漢字comment after json", 53)]
        [InlineData("\"d漢字elta\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a 漢字comment after json", 53)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json with new line\n", "This is a 漢字comment after json with new line", 64)]
        [InlineData("{\"a漢字ge\" : \n//This is a 漢字comment between key-value pairs\n 30}", "This is a 漢字comment between key-value pairs", 66)]
        [InlineData("{\"a漢字ge\" : 30//This is a 漢字comment between key-value pairs on the same line\n}", "This is a 漢字comment between key-value pairs on the same line", 84)]

        [InlineData("/*T漢字his is a multi-line 漢字comment before json*/\"hello\"", "T漢字his is a multi-line 漢字comment before json", 56)]
        [InlineData("\"h漢字ello\"/*This is a multi-line 漢字comment after json*/", "This is a multi-line 漢字comment after json", 62)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a multi-line 漢字comment after json", 65)]
        [InlineData("\"b漢字eta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a multi-line 漢字comment after json", 64)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a multi-line 漢字comment after json", 65)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a multi-line 漢字comment after json", 65)]
        [InlineData("{\"a漢字ge\" : \n/*This is a 漢字comment between key-value pairs*/ 30}", "This is a 漢字comment between key-value pairs", 67)]
        [InlineData("{\"a漢字ge\" : 30/*This is a 漢字comment between key-value pairs on the same line*/}", "This is a 漢字comment between key-value pairs on the same line", 85)]

        [InlineData("/*T漢字his is a split multi-line \n漢字comment before json*/\"hello\"", "T漢字his is a split multi-line \n漢字comment before json", 63)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \n漢字comment after json*/", "This is a split multi-line \n漢字comment after json", 69)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a split multi-line \n漢字comment after json", 72)]
        [InlineData("\"b漢字eta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a split multi-line \n漢字comment after json", 71)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a split multi-line \n漢字comment after json", 72)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a split multi-line \n漢字comment after json", 72)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \n漢字comment between key-value pairs*/ 30}", "This is a split multi-line \n漢字comment between key-value pairs", 85)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \n漢字comment between key-value pairs on the same line*/}", "This is a split multi-line \n漢字comment between key-value pairs on the same line", 103)]
        public static void AllowComments(string jsonString, string expectedComment, int expectedIndex)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8)
            {
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
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
                        indexAfterFirstComment = json.BytesConsumed;
                        Assert.Equal(indexAfterFirstComment, json.CurrentState.BytesConsumed);
                        string actualComment = Encoding.UTF8.GetString(json.ValueSpan);
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

                var jsonMultiSegment = new JsonUtf8Reader(sequence)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
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
                            indexAfterFirstComment = jsonMultiSegment.BytesConsumed;
                            Assert.Equal(indexAfterFirstComment, jsonMultiSegment.CurrentState.BytesConsumed);
                            ReadOnlySpan<byte> value = jsonMultiSegment.IsValueMultiSegment ? jsonMultiSegment.ValueSequence.ToArray() : jsonMultiSegment.ValueSpan;
                            string actualComment = Encoding.UTF8.GetString(value);
                            Assert.Equal(expectedComment, actualComment);
                            break;
                    }
                }
                Assert.True(foundComment);
                Assert.Equal(expectedIndex, indexAfterFirstComment);
            }
        }

        [Theory]
        [InlineData("//", "", 2)]
        [InlineData("//\n", "", 3)]
        [InlineData("/**/", "", 4)]
        [InlineData("/*/*/", "/", 5)]

        [InlineData("//T漢字his is a 漢字comment before json\n\"hello\"", "T漢字his is a 漢字comment before json", 44)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json", "This is a 漢字comment after json", 49)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json\n", "This is a 漢字comment after json", 50)]
        [InlineData("\"a漢字lpha\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a 漢字comment after json", 53)]
        [InlineData("\"b漢字eta\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a 漢字comment after json", 52)]
        [InlineData("\"g漢字amma\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a 漢字comment after json", 53)]
        [InlineData("\"d漢字elta\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a 漢字comment after json", 53)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json with new line\n", "This is a 漢字comment after json with new line", 64)]
        [InlineData("{\"a漢字ge\" : \n//This is a 漢字comment between key-value pairs\n 30}", "This is a 漢字comment between key-value pairs", 66)]
        [InlineData("{\"a漢字ge\" : 30//This is a 漢字comment between key-value pairs on the same line\n}", "This is a 漢字comment between key-value pairs on the same line", 84)]

        [InlineData("/*T漢字his is a multi-line 漢字comment before json*/\"hello\"", "T漢字his is a multi-line 漢字comment before json", 56)]
        [InlineData("\"h漢字ello\"/*This is a multi-line 漢字comment after json*/", "This is a multi-line 漢字comment after json", 62)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a multi-line 漢字comment after json", 65)]
        [InlineData("\"b漢字eta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a multi-line 漢字comment after json", 64)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a multi-line 漢字comment after json", 65)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a multi-line 漢字comment after json", 65)]
        [InlineData("{\"a漢字ge\" : \n/*This is a 漢字comment between key-value pairs*/ 30}", "This is a 漢字comment between key-value pairs", 67)]
        [InlineData("{\"a漢字ge\" : 30/*This is a 漢字comment between key-value pairs on the same line*/}", "This is a 漢字comment between key-value pairs on the same line", 85)]

        [InlineData("/*T漢字his is a split multi-line \n漢字comment before json*/\"hello\"", "T漢字his is a split multi-line \n漢字comment before json", 63)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \n漢字comment after json*/", "This is a split multi-line \n漢字comment after json", 69)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", "This is a split multi-line \n漢字comment after json", 72)]
        [InlineData("\"b漢字eta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a split multi-line \n漢字comment after json", 71)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", "This is a split multi-line \n漢字comment after json", 72)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", "This is a split multi-line \n漢字comment after json", 72)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \n漢字comment between key-value pairs*/ 30}", "This is a split multi-line \n漢字comment between key-value pairs", 85)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \n漢字comment between key-value pairs on the same line*/}", "This is a split multi-line \n漢字comment between key-value pairs on the same line", 103)]
        public static void AllowCommentsSingleSegment(string jsonString, string expectedComment, int expectedIndex)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8)
            {
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
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
                        indexAfterFirstComment = json.BytesConsumed;
                        Assert.Equal(indexAfterFirstComment, json.CurrentState.BytesConsumed);
                        string actualComment = Encoding.UTF8.GetString(json.ValueSpan);
                        Assert.Equal(expectedComment, actualComment);
                        break;
                }
            }
            Assert.True(foundComment);
            Assert.Equal(expectedIndex, indexAfterFirstComment);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
                };

                foundComment = false;
                indexAfterFirstComment = 0;
                while (jsonSlice.Read())
                {
                    JsonTokenType tokenType = jsonSlice.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.Comment:
                            if (foundComment)
                                break;
                            foundComment = true;
                            indexAfterFirstComment = jsonSlice.BytesConsumed;
                            Assert.Equal(indexAfterFirstComment, jsonSlice.CurrentState.BytesConsumed);
                            string actualComment = Encoding.UTF8.GetString(jsonSlice.ValueSpan);
                            Assert.Equal(expectedComment, actualComment);
                            break;
                    }
                }

                int consumed = (int)jsonSlice.BytesConsumed;
                Assert.Equal(consumed, jsonSlice.CurrentState.BytesConsumed);
                jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(consumed), isFinalBlock: true, jsonSlice.CurrentState)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
                };

                if (!foundComment)
                {
                    while (jsonSlice.Read())
                    {
                        JsonTokenType tokenType = jsonSlice.TokenType;
                        switch (tokenType)
                        {
                            case JsonTokenType.Comment:
                                if (foundComment)
                                    break;
                                foundComment = true;
                                indexAfterFirstComment = jsonSlice.BytesConsumed;
                                Assert.Equal(indexAfterFirstComment, jsonSlice.CurrentState.BytesConsumed);
                                string actualComment = Encoding.UTF8.GetString(jsonSlice.ValueSpan);
                                Assert.Equal(expectedComment, actualComment);
                                break;
                        }
                    }
                    indexAfterFirstComment += consumed;
                }

                Assert.True(foundComment);
                Assert.Equal(expectedIndex, indexAfterFirstComment);
            }
        }

        [Theory]
        [InlineData("//", 2)]
        [InlineData("//\n", 3)]
        [InlineData("/**/", 4)]
        [InlineData("/*/*/", 5)]

        [InlineData("//T漢字his is a 漢字comment before json\n\"hello\"", 32)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json", 37)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json\n", 38)]
        [InlineData("\"a漢字lpha\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", 41)]
        [InlineData("\"b漢字eta\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 40)]
        [InlineData("\"g漢字amma\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", 41)]
        [InlineData("\"d漢字elta\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 41)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json with new line\n", 52)]
        [InlineData("{\"a漢字ge\" : \n//This is a 漢字comment between key-value pairs\n 30}", 54)]
        [InlineData("{\"a漢字ge\" : 30//This is a 漢字comment between key-value pairs on the same line\n}", 72)]

        [InlineData("/*T漢字his is a multi-line 漢字comment before json*/\"hello\"", 44)]
        [InlineData("\"h漢字ello\"/*This is a multi-line 漢字comment after json*/", 50)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 53)]
        [InlineData("\"b漢字eta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 52)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 53)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 53)]
        [InlineData("{\"a漢字ge\" : \n/*This is a 漢字comment between key-value pairs*/ 30}", 55)]
        [InlineData("{\"a漢字ge\" : 30/*This is a 漢字comment between key-value pairs on the same line*/}", 73)]

        [InlineData("/*T漢字his is a split multi-line \n漢字comment before json*/\"hello\"", 51)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \n漢字comment after json*/", 57)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 60)]
        [InlineData("\"b漢字eta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 59)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 60)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 60)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \n漢字comment between key-value pairs*/ 30}", 73)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \n漢字comment between key-value pairs on the same line*/}", 91)]
        public static void SkipComments(string jsonString, int expectedConsumed)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8)
            {
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
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
            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(dataUtf8.Length, json.CurrentState.BytesConsumed);

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new JsonUtf8Reader(sequence)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
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
                Assert.Equal(dataUtf8.Length, jsonMultiSegment.BytesConsumed);
                Assert.Equal(jsonMultiSegment.BytesConsumed, json.CurrentState.BytesConsumed);
            }
        }

        [Theory]
        [InlineData("//", 2)]
        [InlineData("//\n", 3)]
        [InlineData("/**/", 4)]
        [InlineData("/*/*/", 5)]

        [InlineData("//T漢字his is a 漢字comment before json\n\"hello\"", 32)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json", 37)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json\n", 38)]
        [InlineData("\"a漢字lpha\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", 41)]
        [InlineData("\"b漢字eta\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 40)]
        [InlineData("\"g漢字amma\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", 41)]
        [InlineData("\"d漢字elta\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 41)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json with new line\n", 52)]
        [InlineData("{\"a漢字ge\" : \n//This is a 漢字comment between key-value pairs\n 30}", 54)]
        [InlineData("{\"a漢字ge\" : 30//This is a 漢字comment between key-value pairs on the same line\n}", 72)]

        [InlineData("/*T漢字his is a multi-line 漢字comment before json*/\"hello\"", 44)]
        [InlineData("\"h漢字ello\"/*This is a multi-line 漢字comment after json*/", 50)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 53)]
        [InlineData("\"b漢字eta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 52)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 53)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 53)]
        [InlineData("{\"a漢字ge\" : \n/*This is a 漢字comment between key-value pairs*/ 30}", 55)]
        [InlineData("{\"a漢字ge\" : 30/*This is a 漢字comment between key-value pairs on the same line*/}", 73)]

        [InlineData("/*T漢字his is a split multi-line \n漢字comment before json*/\"hello\"", 51)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \n漢字comment after json*/", 57)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 60)]
        [InlineData("\"b漢字eta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 59)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 60)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 60)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \n漢字comment between key-value pairs*/ 30}", 73)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \n漢字comment between key-value pairs on the same line*/}", 91)]
        public static void SkipCommentsSingleSegment(string jsonString, int expectedConsumed)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8)
            {
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
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
            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(dataUtf8.Length, json.CurrentState.BytesConsumed);

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
                };

                prevTokenType = JsonTokenType.None;
                while (jsonSlice.Read())
                {
                    JsonTokenType tokenType = jsonSlice.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.Comment:
                            Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                            break;
                    }
                    Assert.NotEqual(tokenType, prevTokenType);
                    prevTokenType = tokenType;
                }

                int prevConsumed = (int)jsonSlice.BytesConsumed;
                Assert.Equal(prevConsumed, jsonSlice.CurrentState.BytesConsumed);
                jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(prevConsumed), isFinalBlock: true, jsonSlice.CurrentState)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
                };

                while (jsonSlice.Read())
                {
                    JsonTokenType tokenType = jsonSlice.TokenType;
                    switch (tokenType)
                    {
                        case JsonTokenType.Comment:
                            Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                            break;
                    }
                    Assert.NotEqual(tokenType, prevTokenType);
                    prevTokenType = tokenType;
                }

                Assert.Equal(dataUtf8.Length - prevConsumed, jsonSlice.BytesConsumed);
                Assert.Equal(jsonSlice.BytesConsumed, jsonSlice.CurrentState.BytesConsumed);
            }
        }

        [Theory]
        [InlineData("//", 1, 0)]
        [InlineData("//\n", 1, 0)]
        [InlineData("/**/", 1, 0)]
        [InlineData("/*/*/", 1, 0)]

        [InlineData("//T漢字his is a 漢字comment before json\n\"hello\"", 1, 0)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json", 1, 13)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json\n", 1, 13)]
        [InlineData("\"a漢字lpha\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"b漢字eta\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"d漢字elta\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json with new line\n", 1, 13)]
        [InlineData("{\"a漢字ge\" : \n//This is a 漢字comment between key-value pairs\n 30}", 2, 0)]
        [InlineData("{\"a漢字ge\" : 30//This is a 漢字comment between key-value pairs on the same line\n}", 1, 17)]

        [InlineData("/*T漢字his is a multi-line 漢字comment before json*/\"hello\"", 1, 0)]
        [InlineData("\"h漢字ello\"/*This is a multi-line 漢字comment after json*/", 1, 13)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"b漢字eta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("{\"a漢字ge\" : \n/*This is a 漢字comment between key-value pairs*/ 30}", 2, 0)]
        [InlineData("{\"a漢字ge\" : 30/*This is a 漢字comment between key-value pairs on the same line*/}", 1, 17)]

        [InlineData("/*T漢字his is a split multi-line \n漢字comment before json*/\"hello\"", 1, 0)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \n漢字comment after json*/", 1, 13)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"b漢字eta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \n漢字comment between key-value pairs*/ 30}", 2, 0)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \n漢字comment between key-value pairs on the same line*/}", 1, 17)]
        public static void CommentsAreInvalidByDefault(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8);

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
                Assert.Equal(expectedPosition, ex.LineBytePosition);
            }

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new JsonUtf8Reader(sequence);
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
                    Assert.Equal(expectedPosition, ex.LineBytePosition);
                }
            }
        }

        [Theory]
        [InlineData("//", 1, 0)]
        [InlineData("//\n", 1, 0)]
        [InlineData("/**/", 1, 0)]
        [InlineData("/*/*/", 1, 0)]

        [InlineData("//T漢字his is a 漢字comment before json\n\"hello\"", 1, 0)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json", 1, 13)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json\n", 1, 13)]
        [InlineData("\"a漢字lpha\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"b漢字eta\" \r\n//This is a 漢字comment after json\n//Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"d漢字elta\" \r\n//This is a 漢字comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"h漢字ello\"//This is a 漢字comment after json with new line\n", 1, 13)]
        [InlineData("{\"a漢字ge\" : \n//This is a 漢字comment between key-value pairs\n 30}", 2, 0)]
        [InlineData("{\"a漢字ge\" : 30//This is a 漢字comment between key-value pairs on the same line\n}", 1, 17)]

        [InlineData("/*T漢字his is a multi-line 漢字comment before json*/\"hello\"", 1, 0)]
        [InlineData("\"h漢字ello\"/*This is a multi-line 漢字comment after json*/", 1, 13)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"b漢字eta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line 漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("{\"a漢字ge\" : \n/*This is a 漢字comment between key-value pairs*/ 30}", 2, 0)]
        [InlineData("{\"a漢字ge\" : 30/*This is a 漢字comment between key-value pairs on the same line*/}", 1, 17)]

        [InlineData("/*T漢字his is a split multi-line \n漢字comment before json*/\"hello\"", 1, 0)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \n漢字comment after json*/", 1, 13)]
        [InlineData("\"a漢字lpha\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"b漢字eta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment", 2, 0)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \n漢字comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/", 2, 0)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \n漢字comment between key-value pairs*/ 30}", 2, 0)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \n漢字comment between key-value pairs on the same line*/}", 1, 17)]
        public static void CommentsAreInvalidByDefaultSingleSegment(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8);

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
                Assert.Equal(expectedPosition, ex.LineBytePosition);
            }

            for (int i = 0; i < dataUtf8.Length; i++)
            {
                var jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false);
                try
                {
                    while (jsonSlice.Read())
                    {
                        JsonTokenType tokenType = jsonSlice.TokenType;
                        switch (tokenType)
                        {
                            case JsonTokenType.Comment:
                                Assert.True(false, "TokenType should never be Comment when we are skipping them.");
                                break;
                        }
                    }

                    Assert.Equal(jsonSlice.BytesConsumed, jsonSlice.CurrentState.BytesConsumed);
                    jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan((int)jsonSlice.BytesConsumed), isFinalBlock: true, jsonSlice.CurrentState);
                    while (jsonSlice.Read())
                    {
                        JsonTokenType tokenType = jsonSlice.TokenType;
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
                    Assert.Equal(expectedPosition, ex.LineBytePosition);
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

        [InlineData("//\n漢字}", 2, 0)]
        [InlineData("//c漢字omment\n漢字}", 2, 0)]
        [InlineData("/**/漢字}", 1, 4)]
        [InlineData("/*\n*/漢字}", 2, 2)]
        [InlineData("/*c漢字omment\n*/漢字}", 2, 2)]
        [InlineData("/*/*/漢字}", 1, 5)]
        [InlineData("//T漢字his is a comment before json\n\"hello\"漢字{", 2, 7)]
        [InlineData("\"h漢字ello\"//This is a comment after json\n漢字{", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*/漢字{//Another single-line comment", 4, 28)]
        [InlineData("\"d漢字elta\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/漢字{", 5, 18)]
        [InlineData("\"h漢字ello\"//This is a comment after json with new line\n漢字{", 2, 0)]
        [InlineData("{\"a漢字ge\" : \n//This is a comment between key-value pairs\n 30}漢字{", 3, 4)]
        [InlineData("{\"a漢字ge\" : 30//This is a comment between key-value pairs on the same line\n}漢字{", 2, 1)]
        [InlineData("/*T漢字his is a multi-line comment before json*/\"hello\"漢字{", 1, 57)]
        [InlineData("\"h漢字ello\"/*This is a multi-line comment after json*/漢字{", 1, 56)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*/漢字{//Another single-line comment", 3, 28)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/漢字{", 4, 18)]
        [InlineData("{\"a漢字ge\" : \n/*This is a comment between key-value pairs*/ 30}漢字{", 2, 49)]
        [InlineData("{\"a漢字ge\" : 30/*This is a comment between key-value pairs on the same line*/}漢字{", 1, 80)]
        [InlineData("/*T漢字his is a split multi-line \ncomment before json*/\"hello\"漢字{", 2, 28)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \ncomment after json*/漢字{", 2, 20)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*/漢字{//Another single-line comment", 4, 28)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/漢字{", 5, 18)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \ncomment between key-value pairs*/ 30}漢字{", 3, 37)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \ncomment between key-value pairs on the same line*/}漢字{", 2, 51)]
        public static void InvalidJsonWithComments(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8)
            {
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
            };

            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.LineBytePosition);
            }

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, i));
                ReadOnlyMemory<byte> secondMem = dataMemory.Slice(i);
                BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);
                var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

                var jsonMultiSegment = new JsonUtf8Reader(sequence)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
                };

                try
                {
                    while (jsonMultiSegment.Read()) ;
                    Assert.True(false, $"Expected JsonReaderException was not thrown with multi-segment data. Index: {i}");
                }
                catch (JsonReaderException ex)
                {
                    string errorMessage = $"expectedLineNumber: {expectedlineNumber} | actual: {ex.LineNumber} | index: {i} | option: {jsonMultiSegment.Options}";
                    string firstSegmentString = Buffers.Text.Encodings.Utf8.ToString(dataUtf8.AsSpan(0, i));
                    string secondSegmentString = Buffers.Text.Encodings.Utf8.ToString(dataUtf8.AsSpan(i));
                    errorMessage += " | " + firstSegmentString + " | " + secondSegmentString;
                    Assert.True(expectedlineNumber == ex.LineNumber, errorMessage);
                    errorMessage = $"expectedBytePosition: {expectedPosition} | actual: {ex.LineBytePosition} | index: {i} | option: {jsonMultiSegment.Options}";
                    errorMessage += " | " + firstSegmentString + " | " + secondSegmentString;
                    Assert.True(expectedPosition == ex.LineBytePosition, errorMessage);
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

        [InlineData("//\n漢字}", 2, 0)]
        [InlineData("//c漢字omment\n漢字}", 2, 0)]
        [InlineData("/**/漢字}", 1, 4)]
        [InlineData("/*\n*/漢字}", 2, 2)]
        [InlineData("/*c漢字omment\n*/漢字}", 2, 2)]
        [InlineData("/*/*/漢字}", 1, 5)]
        [InlineData("//T漢字his is a comment before json\n\"hello\"漢字{", 2, 7)]
        [InlineData("\"h漢字ello\"//This is a comment after json\n漢字{", 2, 0)]
        [InlineData("\"g漢字amma\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*/漢字{//Another single-line comment", 4, 28)]
        [InlineData("\"d漢字elta\" \r\n//This is a comment after json\n//Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/漢字{", 5, 18)]
        [InlineData("\"h漢字ello\"//This is a comment after json with new line\n漢字{", 2, 0)]
        [InlineData("{\"a漢字ge\" : \n//This is a comment between key-value pairs\n 30}漢字{", 3, 4)]
        [InlineData("{\"a漢字ge\" : 30//This is a comment between key-value pairs on the same line\n}漢字{", 2, 1)]
        [InlineData("/*T漢字his is a multi-line comment before json*/\"hello\"漢字{", 1, 57)]
        [InlineData("\"h漢字ello\"/*This is a multi-line comment after json*/漢字{", 1, 56)]
        [InlineData("\"g漢字amma\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*/漢字{//Another single-line comment", 3, 28)]
        [InlineData("\"d漢字elta\" \r\n/*This is a multi-line comment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/漢字{", 4, 18)]
        [InlineData("{\"a漢字ge\" : \n/*This is a comment between key-value pairs*/ 30}漢字{", 2, 49)]
        [InlineData("{\"a漢字ge\" : 30/*This is a comment between key-value pairs on the same line*/}漢字{", 1, 80)]
        [InlineData("/*T漢字his is a split multi-line \ncomment before json*/\"hello\"漢字{", 2, 28)]
        [InlineData("\"h漢字ello\"/*This is a split multi-line \ncomment after json*/漢字{", 2, 20)]
        [InlineData("\"g漢字amma\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*/漢字{//Another single-line comment", 4, 28)]
        [InlineData("\"d漢字elta\" \r\n/*This is a split multi-line \ncomment after json*///Here is another comment\n/*and a multi-line comment*///Another single-line comment\n\t  /*blah * blah*/漢字{", 5, 18)]
        [InlineData("{\"a漢字ge\" : \n/*This is a split multi-line \ncomment between key-value pairs*/ 30}漢字{", 3, 37)]
        [InlineData("{\"a漢字ge\" : 30/*This is a split multi-line \ncomment between key-value pairs on the same line*/}漢字{", 2, 51)]
        public static void InvalidJsonWithCommentsSingleSegment(string jsonString, int expectedlineNumber, int expectedPosition)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            var json = new JsonUtf8Reader(dataUtf8)
            {
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
            };

            try
            {
                while (json.Read()) ;
                Assert.True(false, "Expected JsonReaderException was not thrown with single-segment data.");
            }
            catch (JsonReaderException ex)
            {
                Assert.Equal(expectedlineNumber, ex.LineNumber);
                Assert.Equal(expectedPosition, ex.LineBytePosition);
            }

            ReadOnlyMemory<byte> dataMemory = dataUtf8;
            for (int i = 0; i < dataMemory.Length; i++)
            {
                var jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan(0, i), isFinalBlock: false)
                {
                    Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
                };

                try
                {
                    while (jsonSlice.Read()) ;

                    Assert.Equal(jsonSlice.BytesConsumed, jsonSlice.CurrentState.BytesConsumed);
                    jsonSlice = new JsonUtf8Reader(dataUtf8.AsSpan((int)jsonSlice.BytesConsumed), isFinalBlock: true, jsonSlice.CurrentState)
                    {
                        Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.AllowComments)
                    };

                    while (jsonSlice.Read()) ;

                    Assert.True(false, "Expected JsonReaderException was not thrown with multi-segment data.");
                }
                catch (JsonReaderException ex)
                {
                    Assert.Equal(expectedlineNumber, ex.LineNumber);
                    Assert.Equal(expectedPosition, ex.LineBytePosition);
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

            var json = new JsonUtf8Reader(dataUtf8);
            string key = "";
            int count = 0;
            while (json.Read())
            {
                if (json.TokenType == JsonTokenType.PropertyName)
                {
                    json.TryGetValueAsString(out key);
                }
                if (json.TokenType == JsonTokenType.Number)
                {
                    if (key.StartsWith("int"))
                    {
                        json.TryGetValueAsInt32(out int numberInt);
                        if (count >= ints.Count)
                            count = 0;
                        Assert.Equal(ints[count], numberInt);
                        count++;
                    }
                    else if (key.StartsWith("long"))
                    {
                        json.TryGetValueAsInt64(out long numberLong);
                        if (count >= longs.Count)
                            count = 0;
                        Assert.Equal(longs[count], numberLong);
                        count++;
                    }
                    else if (key.StartsWith("float"))
                    {
                        json.TryGetValueAsSingle(out float numberFloat);
                        if (count >= floats.Count)
                            count = 0;

                        float expected = float.Parse(floats[count].ToString());

                        Assert.Equal(expected, numberFloat);
                        count++;
                    }
                    else if (key.StartsWith("double"))
                    {
                        json.TryGetValueAsDouble(out double numberDouble);
                        if (count >= doubles.Count)
                            count = 0;

                        double expected = double.Parse(doubles[count].ToString());

                        Assert.Equal(expected, numberDouble);
                        count++;
                    }
                    else if (key.StartsWith("decimal"))
                    {
                        json.TryGetValueAsDecimal(out decimal numberDecimal);
                        if (count >= decimals.Count)
                            count = 0;
                        Assert.Equal(decimals[count], numberDecimal);
                        count++;
                    }
                }
            }

            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
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

            var json = new JsonUtf8Reader(dataUtf8);
            while (json.Read())
            {
                if (json.TokenType == JsonTokenType.Number)
                {
                    Assert.False(json.TryGetValueAsInt32(out int value));
                    Assert.True(json.TryGetValueAsDouble(out double doubleValue));
                    Assert.Equal(expected, doubleValue);
                }
            }

            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
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

            var json = new JsonUtf8Reader(dataUtf8);
            while (json.Read())
            {
                if (json.TokenType == JsonTokenType.Number)
                {
                    Assert.False(json.TryGetValueAsInt64(out long value));
                    Assert.True(json.TryGetValueAsDouble(out double doubleValue));
                    Assert.Equal(expected, doubleValue);
                }
            }

            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
        }

        [Fact]
        public void SingleSegmentSequence()
        {
            string jsonString = TestJson.Json400KB;
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            ReadOnlySequence<byte> sequenceSingle = new ReadOnlySequence<byte>(dataUtf8);
            var json = new JsonUtf8Reader(sequenceSingle);
            while (json.Read()) ;
            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
        }

        [Fact]
        public void MultiSegmentSequence()
        {
            string jsonString = TestJson.Json400KB;
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            ReadOnlySequence<byte> sequenceMultiple = GetSequence(dataUtf8, 4_000);
            var json = new JsonUtf8Reader(sequenceMultiple);
            while (json.Read()) ;
            Assert.Equal(dataUtf8.Length, json.BytesConsumed);
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
        }

        [Fact]
        public void MultiSegmentSequenceUsingSpan()
        {
            string jsonString = TestJson.Json400KB;
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            ReadOnlySequence<byte> sequenceMultiple = GetSequence(dataUtf8, 4_000);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(8_000);
            JsonReaderState state = default;
            int previous = 0;

            byte[] outputArray = new byte[dataUtf8.Length];
            Span<byte> destination = outputArray;
            int currentLength = 0;

            int numberOfSegments = dataUtf8.Length / 4_000 + 1;
            int counter = 0;
            foreach (ReadOnlyMemory<byte> memory in sequenceMultiple)
            {
                ReadOnlySpan<byte> span = memory.Span;
                Span<byte> bufferSpan = buffer;
                span.CopyTo(bufferSpan.Slice(previous));
                bufferSpan = bufferSpan.Slice(0, span.Length + previous);

                bool isFinalBlock = bufferSpan.Length == 0;

                var json = new JsonUtf8Reader(bufferSpan, isFinalBlock, state);

                Span<byte> copy = destination.Slice(currentLength);

                while (json.Read())
                {
                    JsonTokenType tokenType = json.TokenType;
                    ReadOnlySpan<byte> valueSpan = json.ValueSpan;
                    switch (tokenType)
                    {
                        case JsonTokenType.PropertyName:
                            valueSpan.CopyTo(copy);
                            copy[valueSpan.Length] = (byte)',';
                            copy[valueSpan.Length + 1] = (byte)' ';
                            copy = copy.Slice(valueSpan.Length + 2);
                            break;
                        case JsonTokenType.Number:
                        case JsonTokenType.String:
                        case JsonTokenType.Comment:
                            valueSpan.CopyTo(copy);
                            copy[valueSpan.Length] = (byte)',';
                            copy[valueSpan.Length + 1] = (byte)' ';
                            copy = copy.Slice(valueSpan.Length + 2);
                            break;
                        case JsonTokenType.True:
                            // Special casing True/False so that the casing matches with Json.NET
                            copy[0] = (byte)'T';
                            copy[1] = (byte)'r';
                            copy[2] = (byte)'u';
                            copy[3] = (byte)'e';
                            copy[valueSpan.Length] = (byte)',';
                            copy[valueSpan.Length + 1] = (byte)' ';
                            copy = copy.Slice(valueSpan.Length + 2);
                            break;
                        case JsonTokenType.False:
                            copy[0] = (byte)'F';
                            copy[1] = (byte)'a';
                            copy[2] = (byte)'l';
                            copy[3] = (byte)'s';
                            copy[4] = (byte)'e';
                            copy[valueSpan.Length] = (byte)',';
                            copy[valueSpan.Length + 1] = (byte)' ';
                            copy = copy.Slice(valueSpan.Length + 2);
                            break;
                        case JsonTokenType.Null:
                            // Special casing Null so that it matches what JSON.NET does
                            break;
                        default:
                            break;
                    }
                }
                currentLength = outputArray.Length - copy.Length;

                if (isFinalBlock)
                    break;

                state = json.CurrentState;
                Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
                if (json.BytesConsumed != bufferSpan.Length)
                {
                    ReadOnlySpan<byte> leftover = bufferSpan.Slice((int)json.BytesConsumed);
                    previous = leftover.Length;
                    leftover.CopyTo(buffer);
                }
                else
                {
                    previous = 0;
                }
                counter++;
            }
            ArrayPool<byte>.Shared.Return(buffer);
            Assert.Equal(numberOfSegments, counter);

            string actualStr = Encoding.UTF8.GetString(outputArray.AsSpan(0, currentLength));

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [InlineData(10_000, 1)]
        [InlineData(100_000, 1)]
        [InlineData(1_000_000, 1)]
        [InlineData(10_000, 7)]
        [InlineData(100_000, 7)]
        [InlineData(1_000_000, 7)]
        [InlineData(10_000_000, 7)]
        [InlineData(10_000, 19)]
        [InlineData(100_000, 19)]
        [InlineData(1_000_000, 19)]
        [InlineData(10_000_000, 19)]
        [InlineData(10_000, 4_000)]
        [InlineData(100_000, 4_000)]
        [InlineData(1_000_000, 4_000)]
        [InlineData(10_000_000, 4_000)]
        [InlineData(10_000, 1_000_000)]
        [InlineData(100_000, 1_000_000)]
        [InlineData(1_000_000, 1_000_000)]
        [InlineData(10_000_000, 1_000_000)]
        //[InlineData(1_000_000_000, 1_000_000)] // Too large, causes intermittent OOM, reserved for outerloop
        public void MultiSegmentSequenceMaxTokenSize(int tokenSize, int segmentSize)
        {
            var random = new Random(42);
            byte[] dataUtf8 = new byte[tokenSize];
            byte[] expected = new byte[tokenSize];

            for (int i = 0; i < 5; i++)
            {
                int splitIndex = random.Next(3, 1_000);
                System.Array.Fill<byte>(dataUtf8, 97);  // 'a'
                System.Array.Clear(expected, 0, expected.Length);

                dataUtf8[0] = 91;   // '['
                dataUtf8[1] = 34;   // '"'
                dataUtf8[splitIndex - 1] = 34;   // '"'
                dataUtf8[splitIndex] = 44;   // ', '
                dataUtf8[splitIndex + 1] = 34;   // '"'
                dataUtf8[tokenSize - 2] = 34; // '"'
                dataUtf8[tokenSize - 1] = 93; // ']'

                Span<byte> expectedFirstValue = expected.AsSpan(0, splitIndex - 3);
                expectedFirstValue.Fill(97);  // 'a'

                Span<byte> expectedSecondValue = expected.AsSpan(splitIndex - 4, tokenSize - (7 + expectedFirstValue.Length));  // adding 7 since we have 7 format characters: ["...","..."]
                expectedSecondValue.Fill(97);  // 'a'

                bool first = true;

                ReadOnlySequence<byte> sequenceMultiple = GetSequence(dataUtf8, segmentSize);
                var json = new JsonUtf8Reader(sequenceMultiple);
                while (json.Read())
                {
                    Assert.True(json.TokenType == JsonTokenType.StartArray || json.TokenType == JsonTokenType.String || json.TokenType == JsonTokenType.EndArray);

                    if (json.TokenType == JsonTokenType.String)
                    {
                        ReadOnlySpan<byte> value = json.IsValueMultiSegment ? json.ValueSequence.ToArray() : json.ValueSpan;
                        if (first)
                        {
                            Assert.True(value.SequenceEqual(expectedFirstValue));
                            first = false;
                        }
                        else
                        {
                            Assert.True(value.SequenceEqual(expectedSecondValue));
                        }
                    }
                }
                Assert.Equal(dataUtf8.Length, json.BytesConsumed);
                Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);
            }
        }

        [Theory]
        [InlineData(250)]   // 1 MB
        //[InlineData(250_000)]    // 1 GB, allocating 1 GB for a test is too high for inner loop (reserved for outerloop)
        //[InlineData(2_500_000)] // 10 GB, takes too long to run (~7 minutes)
        public void MultiSegmentSequenceLarge(int numberOfSegments)
        {
            const int segmentSize = 4_000;
            byte[][] buffers = new byte[numberOfSegments][];

            for (int j = 0; j < numberOfSegments; j++)
            {
                byte[] arr = new byte[segmentSize];

                for (int i = 0; i < segmentSize - 7; i += 7)
                {
                    arr[i] = (byte)'"';
                    arr[i + 1] = (byte)'a';
                    arr[i + 2] = (byte)'a';
                    arr[i + 3] = (byte)'a';
                    arr[i + 4] = (byte)'"';
                    arr[i + 5] = (byte)',';
                    arr[i + 6] = (byte)' ';
                }
                arr[3_997] = (byte)' ';
                arr[3_998] = (byte)' ';
                arr[3_999] = (byte)' ';

                buffers[j] = arr;
            }

            buffers[0][0] = (byte)'[';
            buffers[0][1] = (byte)' ';
            buffers[0][2] = (byte)' ';
            buffers[0][3] = (byte)' ';
            buffers[0][4] = (byte)' ';
            buffers[0][5] = (byte)' ';

            buffers[numberOfSegments - 1][segmentSize - 5] = (byte)']';
            buffers[numberOfSegments - 1][segmentSize - 4] = (byte)' ';
            buffers[numberOfSegments - 1][segmentSize - 3] = (byte)' ';
            buffers[numberOfSegments - 1][segmentSize - 2] = (byte)' ';
            buffers[numberOfSegments - 1][segmentSize - 1] = (byte)' ';

            ReadOnlySequence<byte> sequenceMultiple = BufferFactory.Create(buffers);
            var json = new JsonUtf8Reader(sequenceMultiple);
            while (json.Read()) ;
            Assert.Equal(sequenceMultiple.Length, json.BytesConsumed);
            Assert.Equal(json.BytesConsumed, json.CurrentState.BytesConsumed);

            //TODO: Fix Utf8JsonReaderStream based on changes to Utf8JsonReader
            //var stream = new MemoryStream(sequenceMultiple.ToArray());
            //var jsonStream = new Utf8JsonReaderStream(stream);
            //while (jsonStream.Read()) ;
            //Assert.Equal(sequenceMultiple.Length, jsonStream.Consumed);
        }
    }
}
