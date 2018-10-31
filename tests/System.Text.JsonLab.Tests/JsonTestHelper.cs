// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Tests;
using System.Buffers.Text;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Formatting;

namespace System.Text.JsonLab.Tests
{
    internal static class JsonTestHelper
    {
        public static string NewtonsoftReturnStringHelper(TextReader reader)
        {
            var sb = new StringBuilder();
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

        public static void WriteDepth(ref Utf8JsonWriter<ArrayFormatterWrapper> jsonUtf8, int depth)
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

        public static string ObjectToString(object jsonValues)
        {
            string s = "";
            if (jsonValues is List<object> jsonList)
            {
                s = ListToString(jsonList);
            }
            else
            {
                if (jsonValues is Dictionary<string, object> jsonDictionary)
                {

                    s = DictionaryToString(jsonDictionary);
                }
            }
            return s;
        }

        public static string DictionaryToString(Dictionary<string, object> dictionary)
        {
            var builder = new StringBuilder();
            foreach (KeyValuePair<string, object> entry in dictionary)
            {
                if (entry.Value is Dictionary<string, object> nestedDictionary)
                {
                    builder.Append(entry.Key).Append(", ").Append(DictionaryToString(nestedDictionary));
                }
                else if (entry.Value is List<object> nestedList)
                {
                    builder.Append(entry.Key).Append(", ").Append(ListToString(nestedList));
                }
                else
                {
                    builder.Append(entry.Key).Append(", ").Append(entry.Value).Append(", ");
                }
            }
            return builder.ToString();
        }

        public static string ListToString(List<object> list)
        {
            var builder = new StringBuilder();
            foreach (object entry in list)
            {
                if (entry is Dictionary<string, object> nestedDictionary)
                {
                    builder.Append(DictionaryToString(nestedDictionary));
                }
                else if (entry is List<object> nestedList)
                {
                    builder.Append(ListToString(nestedList));
                }
                else
                {
                    builder.Append(entry).Append(", ");
                }
            }
            return builder.ToString();
        }

        public static void SetKeyValues(ref Utf8Json.Reader json, Dictionary<string, object> dictionary, ref string key, ref object value)
        {
            while (json.Read())
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        key = Encoding.UTF8.GetString(json.Value);
                        dictionary.Add(key, null);
                        break;
                    case JsonTokenType.String:
                        value = Encoding.UTF8.GetString(json.Value);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        value = json.Value[0] == (byte)'t';
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

        public static void SetKeyValues(ref JsonUtf8Reader json, Dictionary<string, object> dictionary, ref string key, ref object value)
        {
            while (json.Read())
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        key = Encoding.UTF8.GetString(json.ValueSpan);
                        dictionary.Add(key, null);
                        break;
                    case JsonTokenType.String:
                        value = Encoding.UTF8.GetString(json.ValueSpan);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        value = json.ValueSpan[0] == (byte)'t';
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

        public static bool TryParseResponseMessageHeapable(ref ReadOnlySequence<byte> buffer)
        {
            if (!TryParseMessage(ref buffer, out var payload))
            {
                return false;
            }

            var reader = new Utf8Json();
            var json = reader.GetReader(payload);

            CheckRead(ref json);
            EnsureObjectStart(ref json);

            int? minorVersion = null;
            string error = null;

            var completed = false;
            while (!completed && CheckRead(ref json))
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        ReadOnlySpan<byte> memberName = json.Value;

                        if (memberName.SequenceEqual(TypePropertyNameUtf8))
                        {

                            // a handshake response does not have a type
                            // check the incoming message was not any other type of message
                            throw new InvalidDataException("Handshake response should not have a 'type' value.");
                        }
                        else if (memberName.SequenceEqual(ErrorPropertyNameUtf8))
                        {
                            error = ReadAsString(ref json, ErrorPropertyName);
                        }
                        else if (memberName.SequenceEqual(MinorVersionPropertyNameUtf8))
                        {
                            minorVersion = ReadAsInt32(ref json, MinorVersionPropertyName);
                        }
                        else
                        {
                            json.Skip();
                        }
                        break;
                    case JsonTokenType.EndObject:
                        completed = true;
                        break;
                    default:
                        throw new InvalidDataException($"Unexpected token '{json.TokenType}' when reading handshake response JSON.");
                }
            };

            json.Dispose();
            return true;
        }

        public static bool TryParseResponseMessage(ref ReadOnlySequence<byte> buffer)
        {
            if (!TryParseMessage(ref buffer, out var payload))
            {
                return false;
            }

            var reader = new JsonUtf8Reader(payload);

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
                        ReadOnlySpan<byte> memberName = reader.IsValueMultiSegment ? reader.ValueSequence.ToArray() : reader.ValueSpan;

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

        public static ReadOnlySequence<byte> GetSequence(byte[] _dataUtf8, int segmentSize)
        {
            int numberOfSegments = _dataUtf8.Length / segmentSize + 1;
            byte[][] buffers = new byte[numberOfSegments][];

            for (int j = 0; j < numberOfSegments - 1; j++)
            {
                buffers[j] = new byte[segmentSize];
                System.Array.Copy(_dataUtf8, j * segmentSize, buffers[j], 0, segmentSize);
            }

            int remaining = _dataUtf8.Length % segmentSize;
            buffers[numberOfSegments - 1] = new byte[remaining];
            System.Array.Copy(_dataUtf8, _dataUtf8.Length - remaining, buffers[numberOfSegments - 1], 0, remaining);

            return BufferFactory.Create(buffers);
        }

        public static readonly byte RecordSeparator = 0x1e;

        public static bool TryParseMessage(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> payload)
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

        public static int? ReadAsInt32(ref JsonUtf8Reader reader, string propertyName)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new InvalidDataException($"Expected '{propertyName}' to be of type Integer.");
            }

            ReadOnlySpan<byte> valueSpan = reader.IsValueMultiSegment ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            if (valueSpan.IsEmpty)
            {
                return null;
            }
            if (!Utf8Parser.TryParse(valueSpan, out int value, out _))
            {
                throw new InvalidDataException($"Expected '{propertyName}' to be of type Integer.");
            }
            return value;
        }

        public static int? ReadAsInt32(ref Utf8Json.Reader reader, string propertyName)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.Number)
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

        public static bool CheckRead(ref JsonUtf8Reader reader)
        {
            if (!reader.Read())
            {
                throw new InvalidDataException("Unexpected end when reading JSON.");
            }

            return true;
        }

        public static bool CheckRead(ref Utf8Json.Reader reader)
        {
            if (!reader.Read())
            {
                throw new InvalidDataException("Unexpected end when reading JSON.");
            }

            return true;
        }

        public static string GetTokenString(JsonTokenType tokenType)
        {
            switch (tokenType)
            {
                case JsonTokenType.Number:
                    return "Integer";
                case JsonTokenType.StartArray:
                    return "Array";
                case JsonTokenType.StartObject:
                    return "Object";
                default:
                    break;
            }
            return tokenType.ToString();
        }

        public static void EnsureObjectStart(ref JsonUtf8Reader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidDataException($"Unexpected JSON Token Type '{GetTokenString(reader.TokenType)}'. Expected a JSON Object.");
            }
        }

        public static void EnsureObjectStart(ref Utf8Json.Reader reader)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new InvalidDataException($"Unexpected JSON Token Type '{GetTokenString(reader.TokenType)}'. Expected a JSON Object.");
            }
        }

        private const string ProtocolPropertyName = "protocol";
        private const string ProtocolVersionPropertyName = "version";
        private const string MinorVersionPropertyName = "minorVersion";
        private const string ErrorPropertyName = "error";
        private const string TypePropertyName = "type";

        public static readonly byte[] ProtocolPropertyNameUtf8 = Encoding.UTF8.GetBytes("protocol");
        public static readonly byte[] ProtocolVersionPropertyNameUtf8 = Encoding.UTF8.GetBytes("version");
        public static readonly byte[] MinorVersionPropertyNameUtf8 = Encoding.UTF8.GetBytes("minorVersion");
        public static readonly byte[] ErrorPropertyNameUtf8 = Encoding.UTF8.GetBytes("error");
        public static readonly byte[] TypePropertyNameUtf8 = Encoding.UTF8.GetBytes("type");

        public static bool TryParseRequestMessage(ref ReadOnlySequence<byte> buffer)
        {
            if (!TryParseMessage(ref buffer, out var payload))
            {
                return false;
            }

            var reader = new JsonUtf8Reader(payload);
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
                        ReadOnlySpan<byte> memberName = reader.IsValueMultiSegment ? reader.ValueSequence.ToArray() : reader.ValueSpan;

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

        public static bool TryParseRequestMessageHeapable(ref ReadOnlySequence<byte> buffer)
        {
            if (!TryParseMessage(ref buffer, out var payload))
            {
                return false;
            }

            var reader = new Utf8Json();
            Utf8Json.Reader json = reader.GetReader(payload);
            CheckRead(ref json);
            EnsureObjectStart(ref json);

            string protocol = null;
            int? protocolVersion = null;

            var completed = false;
            while (!completed && CheckRead(ref json))
            {
                switch (json.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        ReadOnlySpan<byte> memberName = json.Value;

                        if (memberName.SequenceEqual(ProtocolPropertyNameUtf8))
                        {
                            protocol = ReadAsString(ref json, ProtocolPropertyName);
                        }
                        else if (memberName.SequenceEqual(ProtocolVersionPropertyNameUtf8))
                        {
                            protocolVersion = ReadAsInt32(ref json, ProtocolVersionPropertyName);
                        }
                        else
                        {
                            json.Skip();
                        }
                        break;
                    case JsonTokenType.EndObject:
                        completed = true;
                        break;
                    default:
                        throw new InvalidDataException($"Unexpected token '{json.TokenType}' when reading handshake request JSON.");
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
            json.Dispose();
            return true;
        }

        public static unsafe string ReadAsString(ref JsonUtf8Reader reader, string propertyName)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.String)
            {
                throw new InvalidDataException($"Expected '{propertyName}' to be of type String.");
            }

            ReadOnlySpan<byte> value = reader.IsValueMultiSegment ? reader.ValueSequence.ToArray() : reader.ValueSpan;

            if (value.IsEmpty) return "";

#if NETCOREAPP2_2
            return Encoding.UTF8.GetString(reader.Value);
#else
            fixed (byte* bytes = &MemoryMarshal.GetReference(value))
            {
                return Encoding.UTF8.GetString(bytes, value.Length);
            }
#endif
        }

        public static unsafe string ReadAsString(ref Utf8Json.Reader reader, string propertyName)
        {
            reader.Read();

            if (reader.TokenType != JsonTokenType.String)
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

        public static void JsonLabEmptyLoopHelper(byte[] data)
        {
            var json = new JsonUtf8Reader(data)
            {
                MaxDepth = 32,
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
            };

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                        break;
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.String:
                        break;
                    case JsonTokenType.Number:
                        break;
                    case JsonTokenType.True:
                        break;
                    case JsonTokenType.False:
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public static void HeapableJsonLabEmptyLoopHelper(byte[] data)
        {
            var reader = new Utf8Json()
            {
                MaxDepth = 32,
                Options = new JsonReaderOptions(JsonReaderOptions.CommentHandling.SkipComments)
            };

            Utf8Json.Reader json = reader.GetReader(data);
            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                    case JsonTokenType.EndObject:
                        break;
                    case JsonTokenType.StartArray:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.PropertyName:
                        break;
                    case JsonTokenType.String:
                        break;
                    case JsonTokenType.Number:
                        break;
                    case JsonTokenType.True:
                        break;
                    case JsonTokenType.False:
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        throw new ArgumentException();
                }
            }
        }

        public static ReadOnlySequence<byte> CreateSegments(byte[] data)
        {
            ReadOnlyMemory<byte> dataMemory = data;

            var firstSegment = new BufferSegment<byte>(dataMemory.Slice(0, data.Length / 2));
            ReadOnlyMemory<byte> secondMem = dataMemory.Slice(data.Length / 2);
            BufferSegment<byte> secondSegment = firstSegment.Append(secondMem);

            return new ReadOnlySequence<byte>(firstSegment, 0, secondSegment, secondMem.Length);
        }

        public static byte[] HeapableJsonLabSequenceReturnBytesHelper(byte[] data, out int length, JsonReaderOptions options = default)
        {
            ReadOnlySequence<byte> sequence = CreateSegments(data);
            var json = new Utf8Json()
            {
                Options = options
            };
            Utf8Json.Reader reader = json.GetReader(sequence);
            byte[] result = JsonLabReaderLoop(data.Length, out length, ref reader);
            reader.Dispose();

            // TODO: Should we reset the value and valuetype once we are done?
            //Assert.True(reader.Value.IsEmpty);
            //Assert.Equal(JsonValueType.Unknown, reader.ValueType);
            return result;
        }

        public static byte[] JsonLabSequenceReturnBytesHelper(byte[] data, out int length, JsonReaderOptions options = default)
        {
            ReadOnlySequence<byte> sequence = CreateSegments(data);
            var reader = new JsonUtf8Reader(sequence)
            {
                Options = options
            };
            byte[] result = JsonLabReaderLoop(data.Length, out length, ref reader);

            // TODO: Should we reset the value and valuetype once we are done?
            //Assert.True(reader.Value.IsEmpty);
            //Assert.Equal(JsonValueType.Unknown, reader.ValueType);
            return result;
        }

        public static byte[] JsonLabReaderLoop(int inpuDataLength, out int length, ref Utf8JsonReaderStream json)
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
                    case JsonTokenType.Number:
                    case JsonTokenType.String:
                    case JsonTokenType.Comment:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.True:
                        // Special casing True/False so that the casing matches with Json.NET
                        destination[0] = (byte)'T';
                        destination[1] = (byte)'r';
                        destination[2] = (byte)'u';
                        destination[3] = (byte)'e';
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.False:
                        destination[0] = (byte)'F';
                        destination[1] = (byte)'a';
                        destination[2] = (byte)'l';
                        destination[3] = (byte)'s';
                        destination[4] = (byte)'e';
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.Null:
                        // Special casing Null so that it matches what JSON.NET does
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        public static byte[] JsonLabReaderLoop(int inpuDataLength, out int length, ref JsonUtf8Reader json)
        {
            byte[] outputArray = new byte[inpuDataLength];
            Span<byte> destination = outputArray;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.IsValueMultiSegment ? json.ValueSequence.ToArray() : json.ValueSpan;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.Number:
                    case JsonTokenType.String:
                    case JsonTokenType.Comment:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.True:
                        // Special casing True/False so that the casing matches with Json.NET
                        destination[0] = (byte)'T';
                        destination[1] = (byte)'r';
                        destination[2] = (byte)'u';
                        destination[3] = (byte)'e';
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.False:
                        destination[0] = (byte)'F';
                        destination[1] = (byte)'a';
                        destination[2] = (byte)'l';
                        destination[3] = (byte)'s';
                        destination[4] = (byte)'e';
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.Null:
                        // Special casing Null so that it matches what JSON.NET does
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        public static byte[] JsonLabReaderLoop(int inpuDataLength, out int length, ref Utf8Json.Reader json)
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
                    case JsonTokenType.Number:
                    case JsonTokenType.String:
                    case JsonTokenType.Comment:
                        valueSpan.CopyTo(destination);
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.True:
                        // Special casing True/False so that the casing matches with Json.NET
                        destination[0] = (byte)'T';
                        destination[1] = (byte)'r';
                        destination[2] = (byte)'u';
                        destination[3] = (byte)'e';
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.False:
                        destination[0] = (byte)'F';
                        destination[1] = (byte)'a';
                        destination[2] = (byte)'l';
                        destination[3] = (byte)'s';
                        destination[4] = (byte)'e';
                        destination[valueSpan.Length] = (byte)',';
                        destination[valueSpan.Length + 1] = (byte)' ';
                        destination = destination.Slice(valueSpan.Length + 2);
                        break;
                    case JsonTokenType.Null:
                        // Special casing Null so that it matches what JSON.NET does
                        break;
                    default:
                        break;
                }
            }
            length = outputArray.Length - destination.Length;
            return outputArray;
        }

        public static object JsonLabReaderLoop(ref JsonUtf8Reader json)
        {
            object root = null;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.ValueSpan;
                switch (tokenType)
                {
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        root = valueSpan[0] == 't';
                        break;
                    case JsonTokenType.Number:
                        json.TryGetValueAsDouble(out double valueDouble);
                        root = valueDouble;
                        break;
                    case JsonTokenType.String:
                        json.TryGetValueAsString(out string valueString);
                        root = valueString;
                        break;
                    case JsonTokenType.Null:
                        break;
                    case JsonTokenType.StartObject:
                        root = JsonLabReaderDictionaryLoop(ref json);
                        break;
                    case JsonTokenType.StartArray:
                        root = JsonLabReaderListLoop(ref json);
                        break;
                    case JsonTokenType.EndObject:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.None:
                    case JsonTokenType.Comment:
                    default:
                        break;
                }
            }
            return root;
        }

        public static object JsonLabReaderLoop(ref Utf8Json.Reader json)
        {
            object root = null;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        root = valueSpan[0] == 't';
                        break;
                    case JsonTokenType.Number:
                        root = json.GetValueAsNumber();
                        break;
                    case JsonTokenType.String:
                        root = json.GetValueAsString();
                        break;
                    case JsonTokenType.Null:
                        break;
                    case JsonTokenType.StartObject:
                        root = JsonLabReaderDictionaryLoop(ref json);
                        break;
                    case JsonTokenType.StartArray:
                        root = JsonLabReaderListLoop(ref json);
                        break;
                    case JsonTokenType.EndObject:
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.None:
                    case JsonTokenType.Comment:
                    default:
                        break;
                }
            }
            return root;
        }

        public static Dictionary<string, object> JsonLabReaderDictionaryLoop(ref JsonUtf8Reader json)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            string key = "";
            object value = null;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.ValueSpan;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        json.TryGetValueAsString(out key);
                        dictionary.Add(key, null);
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        value = valueSpan[0] == 't';
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.Number:
                        json.TryGetValueAsDouble(out double valueDouble);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = valueDouble;
                        }
                        else
                        {
                            dictionary.Add(key, valueDouble);
                        }
                        break;
                    case JsonTokenType.String:
                        json.TryGetValueAsString(out string valueString);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = valueString;
                        }
                        else
                        {
                            dictionary.Add(key, valueString);
                        }
                        break;
                    case JsonTokenType.Null:
                        value = null;
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.StartObject:
                        value = JsonLabReaderDictionaryLoop(ref json);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonLabReaderListLoop(ref json);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.EndObject:
                        return dictionary;
                    case JsonTokenType.None:
                    case JsonTokenType.Comment:
                    default:
                        break;
                }
            }
            return dictionary;
        }

        public static Dictionary<string, object> JsonLabReaderDictionaryLoop(ref Utf8Json.Reader json)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();

            string key = "";
            object value = null;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.PropertyName:
                        key = json.GetValueAsString();
                        dictionary.Add(key, null);
                        break;
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        value = valueSpan[0] == 't';
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.Number:
                        value = json.GetValueAsNumber();
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.String:
                        value = json.GetValueAsString();
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.Null:
                        value = null;
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.StartObject:
                        value = JsonLabReaderDictionaryLoop(ref json);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonLabReaderListLoop(ref json);
                        if (dictionary.TryGetValue(key, out _))
                        {
                            dictionary[key] = value;
                        }
                        else
                        {
                            dictionary.Add(key, value);
                        }
                        break;
                    case JsonTokenType.EndObject:
                        return dictionary;
                    case JsonTokenType.None:
                    case JsonTokenType.Comment:
                    default:
                        break;
                }
            }
            return dictionary;
        }

        public static List<object> JsonLabReaderListLoop(ref JsonUtf8Reader json)
        {
            List<object> arrayList = new List<object>();

            object value = null;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.ValueSpan;
                switch (tokenType)
                {
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        value = valueSpan[0] == 't';
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.Number:
                        json.TryGetValueAsDouble(out double doubleValue);
                        arrayList.Add(doubleValue);
                        break;
                    case JsonTokenType.String:
                        json.TryGetValueAsString(out string valueString);
                        arrayList.Add(valueString);
                        break;
                    case JsonTokenType.Null:
                        value = null;
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.StartObject:
                        value = JsonLabReaderDictionaryLoop(ref json);
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonLabReaderListLoop(ref json);
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.EndArray:
                        return arrayList;
                    case JsonTokenType.None:
                    case JsonTokenType.Comment:
                    default:
                        break;
                }
            }
            return arrayList;
        }

        public static List<object> JsonLabReaderListLoop(ref Utf8Json.Reader json)
        {
            List<object> arrayList = new List<object>();

            object value = null;

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;
                ReadOnlySpan<byte> valueSpan = json.Value;
                switch (tokenType)
                {
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                        value = valueSpan[0] == 't';
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.Number:
                        value = json.GetValueAsNumber();
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.String:
                        value = json.GetValueAsString();
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.Null:
                        value = null;
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.StartObject:
                        value = JsonLabReaderDictionaryLoop(ref json);
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.StartArray:
                        value = JsonLabReaderListLoop(ref json);
                        arrayList.Add(value);
                        break;
                    case JsonTokenType.EndArray:
                        return arrayList;
                    case JsonTokenType.None:
                    case JsonTokenType.Comment:
                    default:
                        break;
                }
            }
            return arrayList;
        }

        public static byte[] HeapableJsonLabReturnBytesHelper(byte[] data, out int length, JsonReaderOptions options = default)
        {
            var json = new Utf8Json()
            {
                Options = options
            };
            Utf8Json.Reader reader = json.GetReader(data);
            return JsonLabReaderLoop(data.Length, out length, ref reader);
        }

        public static byte[] JsonLabReturnBytesHelper(byte[] data, out int length, JsonReaderOptions options = default)
        {
            var reader = new JsonUtf8Reader(data)
            {
                Options = options
            };
            return JsonLabReaderLoop(data.Length, out length, ref reader);
        }

        public static byte[] JsonLabStreamReturnBytesHelper(byte[] data, out int length)
        {
            Stream stream = new MemoryStream(data);
            var reader = new Utf8JsonReaderStream(stream);

            byte[] result = JsonLabReaderLoop(data.Length, out length, ref reader);
            reader.Dispose();
            return result;
        }

        public static object JsonLabReturnObjectHelper(byte[] data, JsonReaderOptions options = default)
        {
            var reader = new JsonUtf8Reader(data)
            {
                Options = options
            };
            return JsonLabReaderLoop(ref reader);
        }

        public static object JsonLabReturnObjectHelperHeapable(byte[] data, JsonReaderOptions options = default)
        {
            var reader = new Utf8Json()
            {
                Options = options
            };
            Utf8Json.Reader json = reader.GetReader(data);
            return JsonLabReaderLoop(ref json);
        }

        public static float NextFloat(Random random)
        {
            double mantissa = (random.NextDouble() * 2.0) - 1.0;
            // choose -149 instead of -126 to also generate subnormal floats (*)
            double exponent = Math.Pow(2.0, random.Next(-126, 128));
            return (float)(mantissa * exponent);
        }

        public static double NextDouble(Random random, double minValue, double maxValue)
        {
            double value = random.NextDouble() * (maxValue - minValue) + minValue;
            return value;
        }

        public static decimal NextDecimal(Random random, double minValue, double maxValue)
        {
            double value = random.NextDouble() * (maxValue - minValue) + minValue;
            return (decimal)value;
        }
    }
}
