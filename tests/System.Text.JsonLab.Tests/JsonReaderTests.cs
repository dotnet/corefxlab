// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.IO;
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
        public static void TestJsonReaderUtf8(TestCaseType type, string jsonString)
        {
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int length);
            byte[] resultSequence = JsonLabSequenceReturnBytesHelper(dataUtf8, out length);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));
            string actualStrSequence = Encoding.UTF8.GetString(resultSequence.AsSpan(0, length));

            Stream stream = new MemoryStream(dataUtf8);
            TextReader reader = new StreamReader(stream, Encoding.UTF8, false, 1024, true);
            string expectedStr = NewtonsoftReturnStringHelper(reader);

            Assert.Equal(expectedStr, actualStr);
            Assert.Equal(expectedStr, actualStrSequence);

            long memoryBefore = GC.GetAllocatedBytesForCurrentThread();
            JsonLabEmptyLoopHelper(dataUtf8);
            long memoryAfter = GC.GetAllocatedBytesForCurrentThread();
            Assert.Equal(0, memoryAfter - memoryBefore);
        }

        [Fact]
        public static void TestJsonReaderUtf8SpecialString()
        {
            string jsonString = "{\"nam\\\"e\":\"ah\\\"son\"}";
            string expectedStr = "nam\\\"e, ah\\\"son, ";
            
            byte[] dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            byte[] result = JsonLabReturnBytesHelper(dataUtf8, out int length);
            string actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);

            jsonString = "{\"Here is a string: \\\"\\\"\":\"Here is a\",\"Here is a back slash\\\\\":[\"Multiline\r\n String\r\n\",\"	Mul\r\ntiline String\",\"\\\"somequote\\\"\tMu\\\"\\\"l\r\ntiline\\\"another\\\" String\\\\\"],\"str\":\"\\\"\\\"\"}";
            expectedStr = "Here is a string: \\\"\\\", Here is a, Here is a back slash\\\\, Multiline\r\n String\r\n, \tMul\r\ntiline String, \\\"somequote\\\"	Mu\\\"\\\"l\r\ntiline\\\"another\\\" String\\\\, str, \\\"\\\", ";

            dataUtf8 = Encoding.UTF8.GetBytes(jsonString);
            result = JsonLabReturnBytesHelper(dataUtf8, out length);
            actualStr = Encoding.UTF8.GetString(result.AsSpan(0, length));

            Assert.Equal(expectedStr, actualStr);
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

        private static byte[] JsonLabSequenceReturnBytesHelper(byte[] data, out int length)
        {
            ReadOnlyMemory<byte> dataMemory = data;

            var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, data.Length / 2));
            ReadOnlyMemory<byte> secondMem = dataMemory.Slice(data.Length / 2);
            BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);

            var sequence = new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);

            return JsonLabReaderLoop(data, out length, new Utf8JsonReader(sequence));
        }

        private static byte[] JsonLabReaderLoop(byte[] data, out int length, Utf8JsonReader json)
        {
            byte[] outputArray = new byte[data.Length];
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
                                // Special casing  Null so that it matches what JSON.NET does
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
            return JsonLabReaderLoop(data, out length, new Utf8JsonReader(data));
        }

        private static string NewtonsoftReturnStringHelper(TextReader reader)
        {
            StringBuilder sb = new StringBuilder();
            var json = new Newtonsoft.Json.JsonTextReader(reader);
            while (json.Read())
            {
                if (json.Value != null)
                {
                    sb.Append(json.Value).Append(", ");
                }
            }

            return sb.ToString();
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
