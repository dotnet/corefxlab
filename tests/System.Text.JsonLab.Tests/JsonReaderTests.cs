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
            return JsonLabReaderLoop(data, out length, new Utf8JsonReader(data));
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
