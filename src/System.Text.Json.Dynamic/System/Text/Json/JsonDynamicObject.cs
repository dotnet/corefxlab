// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
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

            var reader = new JsonReader(utf8, SymbolTable.InvariantUtf8);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        var name = new Utf8Span(reader.Value);
                        reader.Read(); // Move to the value token
                        var type = reader.ValueType;
                        var current = stack.Peek();
                        var property = new JsonProperty(current, name);
                            switch (type)
                            {
                            case JsonValueType.String:
                                current._properties[property] = new JsonValue(new Utf8Span(reader.Value));
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
                                current._properties[property] = new JsonValue(new Utf8Span(reader.Value), type);
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

        public bool TryGetUInt32(Utf8Span property, out uint value)
        {
            var jsonProperty= new JsonProperty(this, property);
            JsonValue jsonValue;
            if (!_properties.TryGetValue(jsonProperty, out jsonValue))
            {
                value = default;
                return false;
            }

            if(jsonValue.Type != JsonValueType.Number)
            {
                throw new InvalidOperationException();
            }
            return Utf8Parser.TryParseUInt32(jsonValue.Value.Bytes, out value);
        }

        public bool TryGetString(Utf8Span property, out Utf8Span value)
        {
            var jsonProperty = new JsonProperty(this, property);
            JsonValue jsonValue;
            if (!_properties.TryGetValue(jsonProperty, out jsonValue))
            {
                value = default;
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
            var name = new Utf8Span(binder.Name);
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
            var name = new Utf8Span(binder.Name);
            var property = new JsonProperty(this, name);
            if(value == null)
            {
                _properties[property] = new JsonValue(JsonValueType.Null);
                return true;
            }
            if(value is string)
            {
                _properties[property] = new JsonValue(new Utf8Span((string)value));
                return true;
            }
            return false;
        }

        public bool TryFormat(Span<byte> buffer, out int written, ParsedFormat format, SymbolTable symbolTable)
        {
            written = 0;
            int justWritten;
            if(!TryEncodeControlChar(symbolTable, (byte)'{', buffer, out justWritten))
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
                    if (!TryEncodeControlChar(symbolTable, (byte)',', buffer.Slice(written), out justWritten))
                    {
                        return false;
                    }
                    written += justWritten;
                }

                if(!property.Key.TryFormat(buffer.Slice(written), out justWritten, format, symbolTable))
                {
                    written = 0; return false;
                }
                written += justWritten;
                if (!TryEncodeControlChar(symbolTable, (byte)':', buffer.Slice(written), out justWritten))
                {
                    return false;
                }
                written += justWritten;
                if (!property.Value.TryFormat(buffer.Slice(written), out justWritten, format, symbolTable))
                {
                    written = 0; return false;
                }
                written += justWritten;
            }

            if (!TryEncodeControlChar(symbolTable, (byte)'}', buffer.Slice(written), out justWritten)) {
                written = 0; return false;
            }
            written += justWritten;
            return true;
        }

        private static unsafe bool TryEncodeControlChar(SymbolTable symbolTable, byte value, Span<byte> buffer, out int written)
        {
            return symbolTable.TryEncode(value, buffer, out written);
        }

        struct JsonValue : IBufferFormattable
        {
            static readonly byte[] s_nullBytes = { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
            static readonly byte[] s_trueBytes = { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
            static readonly byte[] s_falseBytes = { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };

            JsonDynamicObject _object;
            //TODO: no spans on the heap
            Utf8Span _value => default;
            JsonValueType _type;

            public JsonValue(Utf8Span value, JsonValueType type = JsonValueType.String)
            {
                //TODO: no spans on the heap
                //_value = value;
                _object = null;
                _type = type;
            }
            public JsonValue(JsonDynamicObject obj)
            {
                //TODO: no spans on the heap
                //_value = default(Utf8Span);
                _object = obj;
                _type = JsonValueType.Object;
            }

            public JsonValue(JsonValueType type)
            {
                _type = type;
                //TODO: no spans on the heap
                //_value = default(Utf8Span);
                _object = null;
            }

            public JsonDynamicObject Object { get { return _object; } }
            public Utf8Span Value { get { return _value; } }
            public JsonValueType Type { get { return _type; } }

            public object ToObject()
            {
                if (_object != null) return _object;
                if (_type == JsonValueType.Null) return null;
                if (_type == JsonValueType.True) return true;
                if (_type == JsonValueType.False) return false;
                //TODO: no spans on the heap
                //if (_type == JsonValueType.String) return _value;
                if (_type == JsonValueType.Number)
                {
                    return double.Parse(_value.ToString());
                }
                else throw new NotImplementedException();
            }

            static readonly byte[] nullValue = { (byte)'n', (byte)'u', (byte)'l', (byte)'l'};

            public bool TryFormat(Span<byte> buffer, out int written, ParsedFormat format, SymbolTable symbolTable)
            {
                int consumed;

                switch (_type)
                {
                    case JsonValueType.String:
                        return _value.TryFormatQuotedString(buffer, out written, format, symbolTable: symbolTable);
                    case JsonValueType.Number:
                        return _value.TryFormat(buffer, out written, format, symbolTable: symbolTable);
                    case JsonValueType.Object:
                        return _object.TryFormat(buffer, out written, format, symbolTable);
                    case JsonValueType.Null:
                        return symbolTable.TryEncode(s_nullBytes, buffer, out consumed, out written);
                    case JsonValueType.True:
                        return symbolTable.TryEncode(s_trueBytes, buffer, out consumed, out written);
                    case JsonValueType.False:
                        return symbolTable.TryEncode(s_falseBytes, buffer, out consumed, out written);
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        // this type allows all JsonObject instances to share one hashtable
        struct JsonProperty : IEquatable<JsonProperty>, IBufferFormattable
        {
            JsonDynamicObject _object;
            //TODO: no spans on the heap
            Utf8Span _name => default;

            public JsonDynamicObject Object
            {
                get
                {
                    return _object;
                }
            }

            public JsonProperty(JsonDynamicObject obj, Utf8Span name)
            {
                _object = obj;
                //TODO: no spans on the heap
                //_name = name;
            }

            public bool Equals(JsonProperty other)
            {
                if (Object != other.Object) return false;
                if (_name.Equals(other._name)) return true;
                return false;
            }

            public override int GetHashCode() => _name.GetHashCode();

            public bool TryFormat(Span<byte> buffer, out int written, ParsedFormat format, SymbolTable symbolTable)
            {
                return _name.TryFormatQuotedString(buffer, out written, format, symbolTable);
            }
        }
    }

    static class Utf8SpanExtensions
    {
        public static bool TryFormat(this Utf8Span str, Span<byte> buffer, out int written, ParsedFormat format, SymbolTable symbolTable)
        {
            return symbolTable.TryEncode(str.Bytes, buffer, out var consumed, out written);
        }

        public static bool TryFormatQuotedString(this Utf8Span str, Span<byte> buffer, out int written, ParsedFormat format, SymbolTable symbolTable)
        {
            written = 0;
            int justWritten;

            unsafe
            {
                if (!symbolTable.TryEncode((byte)'"', buffer, out justWritten))
                {
                    return false;
                }
                written += justWritten;

                if (!str.TryFormat(buffer.Slice(written), out justWritten, format, symbolTable))
                {
                    return false;
                }
                written += justWritten;

                if (!symbolTable.TryEncode((byte)'"', buffer.Slice(written), out justWritten))
                {
                    return false;
                }
            }

            written += justWritten;

            return true;
        }
    }
}
