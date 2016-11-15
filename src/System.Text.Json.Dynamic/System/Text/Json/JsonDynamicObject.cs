// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Dynamic;
using System.Text.Formatting;
using System.Text.Utf8;

namespace System.Text.Json
{
    public class JsonDynamicObject : DynamicObject, IBufferFormattable
    {
        Dictionary<JsonProperty, JsonValue> _properties;

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

            var reader = new JsonReader(new Utf8String(utf8));
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonReader.JsonTokenType.Property:
                        var name = reader.GetName();
                        var type = reader.GetJsonValueType();
                        var value = reader.GetValue();
                        var current = stack.Peek();
                        var property = new JsonProperty(current, name);
                        switch (type)
                        {
                            case JsonReader.JsonValueType.String:
                                current._properties[property] = new JsonValue(value);
                                break;
                            case JsonReader.JsonValueType.Object: // TODO: could this be lazy? Could this reuse the root JsonObject (which would store non-allocating JsonDom)?
                                var newObj = new JsonDynamicObject(properties);
                                current._properties[property] = new JsonValue(newObj);
                                stack.Push(newObj);
                                break;
                            case JsonReader.JsonValueType.True:
                                current._properties[property] = new JsonValue(type);
                                break;
                            case JsonReader.JsonValueType.False:
                                current._properties[property] = new JsonValue(type);
                                break;
                            case JsonReader.JsonValueType.Null:
                                current._properties[property] = new JsonValue(type);
                                break;
                            case JsonReader.JsonValueType.Number:
                                current._properties[property] = new JsonValue(value, type);
                                break;
                            case JsonReader.JsonValueType.Array:
                                throw new NotImplementedException("array support not implemented yet.");
                            default:
                                throw new NotSupportedException();
                        }
                        break;
                    case JsonReader.JsonTokenType.ObjectStart:
                        break;
                    case JsonReader.JsonTokenType.ObjectEnd:
                        if (stack.Count != 1) { stack.Pop(); }
                        break;
                    case JsonReader.JsonTokenType.ArrayStart:
                        throw new NotImplementedException("array support not implemented yet.");
                    case JsonReader.JsonTokenType.ArrayEnd:
                    case JsonReader.JsonTokenType.Value:
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

            if(jsonValue.Type != JsonReader.JsonValueType.Number)
            {
                throw new InvalidOperationException();
            }
            int consumed;
            return PrimitiveParser.TryParseUInt32(jsonValue.Value, out value, out consumed);
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

            if (jsonValue.Type != JsonReader.JsonValueType.String)
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
                _properties[property] = new JsonValue(JsonReader.JsonValueType.Null);
                return true;
            }
            if(value is string)
            {
                _properties[property] = new JsonValue(new Utf8String((string)value));
                return true;
            }
            return false;
        }

        public bool TryFormat(Span<byte> buffer, out int written, Format.Parsed format, EncodingData formattingData)
        {
            written = 0;
            int justWritten;
            if(!'{'.TryEncode(buffer, out justWritten, formattingData.Encoding)) {
                return false;
            }
            written += justWritten;

            bool firstProperty = true;
            foreach(var property in _properties)
            {
                if (property.Key.Object != this) continue;

                if(firstProperty) { firstProperty = false; }
                else
                {
                    if (!','.TryEncode(buffer.Slice(written), out justWritten, formattingData.Encoding))
                    {
                        return false;
                    }
                    written += justWritten;
                }

                if(!property.Key.TryFormat(buffer.Slice(written), out justWritten, format, formattingData))
                {
                    written = 0; return false;
                }
                written += justWritten;
                if (!':'.TryEncode(buffer.Slice(written), out justWritten, formattingData.Encoding))
                {
                    return false;
                }
                written += justWritten;
                if (!property.Value.TryFormat(buffer.Slice(written), out justWritten, format, formattingData))
                {
                    written = 0; return false;
                }
                written += justWritten;
            }

            if (!'}'.TryEncode(buffer.Slice(written), out justWritten, formattingData.Encoding)) {
                written = 0; return false;
            }
            written += justWritten;
            return true;
        }

        struct JsonValue : IBufferFormattable
        {
            JsonDynamicObject _object;
            Utf8String _value;
            JsonReader.JsonValueType _type;

            public JsonValue(Utf8String value, JsonReader.JsonValueType type = JsonReader.JsonValueType.String)
            {
                _value = value;
                _object = null;
                _type = type;
            }
            public JsonValue(JsonDynamicObject obj)
            {
                _value = default(Utf8String);
                _object = obj;
                _type = JsonReader.JsonValueType.Object;
            }

            public JsonValue(JsonReader.JsonValueType type)
            {
                _type = type;
                _value = default(Utf8String);
                _object = null;
            }

            public JsonDynamicObject Object { get { return _object; } }
            public Utf8String Value { get { return _value; } }
            public JsonReader.JsonValueType Type { get { return _type; } }

            public object ToObject()
            {
                if (_object != null) return _object;
                if (_type == JsonReader.JsonValueType.Null) return null;
                if (_type == JsonReader.JsonValueType.True) return true;
                if (_type == JsonReader.JsonValueType.False) return false;
                if (_type == JsonReader.JsonValueType.String) return _value;
                if (_type == JsonReader.JsonValueType.Number)
                {
                    return double.Parse(_value.ToString());
                }
                else throw new NotImplementedException();
            }

            public bool TryFormat(Span<byte> buffer, out int written, Format.Parsed format, EncodingData formattingData)
            {
                switch (_type)
                {
                    case JsonReader.JsonValueType.String:
                        return _value.TryFormatQuotedString(buffer, format, formattingData, out written);
                    case JsonReader.JsonValueType.Number:
                        return _value.TryFormat(buffer, format, formattingData, out written);
                    case JsonReader.JsonValueType.Object:
                        return _object.TryFormat(buffer, out written, format, formattingData);
                    case JsonReader.JsonValueType.Null:
                        return "null".TryEncode(buffer, out written, formattingData.Encoding);
                    case JsonReader.JsonValueType.True:
                        return "true".TryEncode(buffer, out written, formattingData.Encoding);
                    case JsonReader.JsonValueType.False:
                        return "false".TryEncode(buffer, out written, formattingData.Encoding);
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

            public bool TryFormat(Span<byte> buffer, out int written, Format.Parsed format, EncodingData formattingData)
            {
                return _name.TryFormatQuotedString(buffer, format, formattingData, out written);
            }
        }
    }

    static class Utf8StringExtensions
    {
        // TODO: this should be properly implemented 
        // currently it handles formatting to UTF8 only.
        public static bool TryFormat(this Utf8String str, Span<byte> buffer, Format.Parsed format, EncodingData formattingData, out int written)
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

        public static bool TryFormatQuotedString(this Utf8String str, Span<byte> buffer, Format.Parsed format, EncodingData formattingData, out int written)
        {
            written = 0;
            int justWritten;

            if (!'"'.TryEncode(buffer, out justWritten, formattingData.Encoding))
            {
                return false;
            }
            written += justWritten;

            if (!str.TryFormat(buffer.Slice(written), format, formattingData, out justWritten))
            {
                return false;
            }
            written += justWritten;

            if (!'"'.TryEncode(buffer.Slice(written), out justWritten, formattingData.Encoding))
            {
                return false;
            }
            written += justWritten;

            return true;
        }
    }
}