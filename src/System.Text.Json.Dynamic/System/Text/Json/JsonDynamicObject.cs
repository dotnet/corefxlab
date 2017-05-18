﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Dynamic;
using System.Text.Utf8;

namespace System.Text.Json
{
    public class JsonDynamicObject : DynamicObject, IBufferFormattable
    {
        private readonly Dictionary<JsonProperty, JsonValue> _properties;

        public JsonDynamicObject() : this(new Dictionary<JsonProperty, JsonValue>()) { }

        private JsonDynamicObject(Dictionary<JsonProperty, JsonValue> properties)
        {
            _properties = properties;
        }

        public static JsonDynamicObject Parse(ReadOnlySpan<byte> utf8, int expectedNumberOfProperties = -1)
        {
            Stack<JsonDynamicObject> stack = new Stack<JsonDynamicObject>();
            if(expectedNumberOfProperties == -1) { expectedNumberOfProperties = utf8.Length >> 3; }
            var properties = new Dictionary<JsonProperty, JsonValue>(expectedNumberOfProperties);
            stack.Push(new JsonDynamicObject(properties));

            var reader = new JsonReader(utf8, TextEncoder.Utf8);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        var name = new Utf8String(reader.Value);
                        reader.Read(); // Move to the value token
                        var type = reader.ValueType;
                        var current = stack.Peek();
                        var property = new JsonProperty(current, name);
                            switch (type)
                            {
                            case JsonValueType.String:
                                current._properties[property] = new JsonValue(new Utf8String(reader.Value));
                                break;
                            case JsonValueType.Object: // TODO: could this be lazy? Could this reuse the root JsonObject (which would store non-allocating JsonDom)?
                                var newObj = new JsonDynamicObject(properties);
                                current._properties[property] = new JsonValue(newObj);
                                stack.Push(newObj);
                                    break;
                            case JsonValueType.True:
                                    current._properties[property] = new JsonValue(type);
                                    break;
                            case JsonValueType.False:
                                    current._properties[property] = new JsonValue(type);
                                    break;
                            case JsonValueType.Null:
                                    current._properties[property] = new JsonValue(type);
                                    break;
                            case JsonValueType.Number:
                                current._properties[property] = new JsonValue(new Utf8String(reader.Value), type);
                                    break;
                            case JsonValueType.Array:
                                throw new NotImplementedException("array support not implemented yet.");
                                default:
                                    throw new NotSupportedException();
                            }
                        break;
                    case JsonTokenType.StartObject:
                        break;
                    case JsonTokenType.EndObject:
                        if (stack.Count != 1) { stack.Pop(); }
                        break;
                    case JsonTokenType.StartArray:
                        throw new NotImplementedException("array support not implemented yet.");
                    case JsonTokenType.EndArray:
                    case JsonTokenType.Value:
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            return stack.Peek();
        }

        public bool TryGetUInt32(Utf8String property, out uint value)
        {
            var jsonProperty= new JsonProperty(this, property);
            JsonValue jsonValue;
            if (!_properties.TryGetValue(jsonProperty, out jsonValue))
            {
                value = default(uint);
                return false;
            }

            if(jsonValue.Type != JsonValueType.Number)
            {
                throw new InvalidOperationException();
            }
            return PrimitiveParser.InvariantUtf8.TryParseUInt32(jsonValue.Value.Bytes, out value);
        }

        public bool TryGetString(Utf8String property, out Utf8String value)
        {
            var jsonProperty = new JsonProperty(this, property);
            JsonValue jsonValue;
            if (!_properties.TryGetValue(jsonProperty, out jsonValue))
            {
                value = default(Utf8String);
                return false;
            }

            if (jsonValue.Type != JsonValueType.String)
            {
                throw new InvalidOperationException();
            }

            value = jsonValue.Value;
            return true;
        }

        public int Count
        {
            get
            {
                int sum = 0;
                foreach(var pair in _properties)
                {
                    if(pair.Key.Object == this) { sum++; }
                }
                return sum;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = new Utf8String(binder.Name);
            var property = new JsonProperty(this, name);
            JsonValue value;
            if(!_properties.TryGetValue(property, out value))
            {
                result = null;
                return false;
            }

            result = value.ToObject();
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var name = new Utf8String(binder.Name);
            var property = new JsonProperty(this, name);
            if(value == null)
            {
                _properties[property] = new JsonValue(JsonValueType.Null);
                return true;
            }
            if(value is string)
            {
                _properties[property] = new JsonValue(new Utf8String((string)value));
                return true;
            }
            return false;
        }

        public bool TryFormat(Span<byte> buffer, out int written, TextFormat format, TextEncoder encoder)
        {
            written = 0;
            int justWritten;
            if(!TryEncodeChar(encoder, '{', buffer, out justWritten))
            {
                return false;
            }
            written += justWritten;

            bool firstProperty = true;
            foreach(var property in _properties)
            {
                if (property.Key.Object != this) continue;

                if (firstProperty)
                {
                    firstProperty = false;
                }
                else
                {
                    if (!TryEncodeChar(encoder, ',', buffer.Slice(written), out justWritten))
                    {
                        return false;
                    }
                    written += justWritten;
                }

                if(!property.Key.TryFormat(buffer.Slice(written), out justWritten, format, encoder))
                {
                    written = 0; return false;
                }
                written += justWritten;
                if (!TryEncodeChar(encoder, ':', buffer.Slice(written), out justWritten))
                {
                    return false;
                }
                written += justWritten;
                if (!property.Value.TryFormat(buffer.Slice(written), out justWritten, format, encoder))
                {
                    written = 0; return false;
                }
                written += justWritten;
            }

            if (!TryEncodeChar(encoder, '}', buffer.Slice(written), out justWritten)) {
                written = 0; return false;
            }
            written += justWritten;
            return true;
        }

        private static unsafe bool TryEncodeChar(TextEncoder formattingData, char value, Span<byte> buffer, out int written)
        {
            ReadOnlySpan<char> charSpan = new ReadOnlySpan<char>(&value, 1);
            
            int consumed;
            return formattingData.TryEncode(charSpan, buffer, out consumed, out written);
        }

        struct JsonValue : IBufferFormattable
        {
            JsonDynamicObject _object;
            Utf8String _value;
            JsonValueType _type;

            public JsonValue(Utf8String value, JsonValueType type = JsonValueType.String)
            {
                _value = value;
                _object = null;
                _type = type;
            }
            public JsonValue(JsonDynamicObject obj)
            {
                _value = default(Utf8String);
                _object = obj;
                _type = JsonValueType.Object;
            }

            public JsonValue(JsonValueType type)
            {
                _type = type;
                _value = default(Utf8String);
                _object = null;
            }

            public JsonDynamicObject Object { get { return _object; } }
            public Utf8String Value { get { return _value; } }
            public JsonValueType Type { get { return _type; } }

            public object ToObject()
            {
                if (_object != null) return _object;
                if (_type == JsonValueType.Null) return null;
                if (_type == JsonValueType.True) return true;
                if (_type == JsonValueType.False) return false;
                if (_type == JsonValueType.String) return _value;
                if (_type == JsonValueType.Number)
                {
                    return double.Parse(_value.ToString());
                }
                else throw new NotImplementedException();
            }

            public bool TryFormat(Span<byte> buffer, out int written, TextFormat format, TextEncoder encoder)
            {
                switch (_type)
                {
                    case JsonValueType.String:
                        return _value.TryFormatQuotedString(buffer, out written, format, encoder: encoder);
                    case JsonValueType.Number:
                        return _value.TryFormat(buffer, out written, format, encoder: encoder);
                    case JsonValueType.Object:
                        return _object.TryFormat(buffer, out written, format, encoder);
                    case JsonValueType.Null:
                        return encoder.TryEncode("null", buffer, out written);
                    case JsonValueType.True:
                        return encoder.TryEncode("true", buffer, out written);
                    case JsonValueType.False:
                        return encoder.TryEncode("false", buffer, out written);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        // this type allows all JsonObject instances to share one hashtable
        struct JsonProperty : IEquatable<JsonProperty>, IBufferFormattable
        {
            JsonDynamicObject _object;
            Utf8String _name;

            public JsonDynamicObject Object
            {
                get
                {
                    return _object;
                }
            }

            public JsonProperty(JsonDynamicObject obj, Utf8String name)
            {
                _object = obj;
                _name = name;
            }

            public bool Equals(JsonProperty other)
            {
                if (Object != other.Object) return false;
                if (_name.Equals(other._name)) return true;
                return false;
            }

            public override int GetHashCode()
            {
                int result = Object.GetHashCode();
                result = result * 19 + (byte)_name[0];
                result = result * 19 + (byte)_name[_name.Length - 1];
                result = result * 19 + (byte)_name[_name.Length>>2];
                return result;
            }

            public bool TryFormat(Span<byte> buffer, out int written, TextFormat format, TextEncoder encoder)
            {
                return _name.TryFormatQuotedString(buffer, out written, format, encoder);
            }
        }
    }

    static class Utf8StringExtensions
    {
        // TODO: this should be properly implemented 
        // currently it handles formatting to UTF8 only.
        public static bool TryFormat(this Utf8String str, Span<byte> buffer, out int written, TextFormat format, TextEncoder encoder)
        {
            written = 0;
            if (buffer.Length < str.Length)
            {
                return false;
            }

            foreach (var cp in str)
            {
                var b = cp;
                buffer[written++] = cp;
            }

            return true;
        }

        public static bool TryFormatQuotedString(this Utf8String str, Span<byte> buffer, out int written, TextFormat format, TextEncoder encoder)
        {
            written = 0;
            int consumed;
            int justWritten;

            unsafe
            {
                char quoteChar = '"';
                ReadOnlySpan<char> quoteSpan = new ReadOnlySpan<char>(&quoteChar, 1);

                if (!encoder.TryEncode(quoteSpan, buffer, out consumed, out justWritten))
                {
                    return false;
                }
                written += justWritten;

                if (!str.TryFormat(buffer.Slice(written), out justWritten, format, encoder))
                {
                    return false;
                }
                written += justWritten;

                if (!encoder.TryEncode(quoteSpan, buffer.Slice(written), out consumed, out justWritten))
                {
                    return false;
                }
            }

            written += justWritten;

            return true;
        }
    }
}
