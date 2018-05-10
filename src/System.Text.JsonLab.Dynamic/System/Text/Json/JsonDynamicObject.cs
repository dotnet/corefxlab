// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text.Utf8;

namespace System.Text.JsonLab
{
    public partial class JsonDynamicObject : DynamicObject, IBufferFormattable
    {
        private readonly Dictionary<JsonProperty, JsonValue> _properties;

        public JsonDynamicObject() : this(new Dictionary<JsonProperty, JsonValue>()) { }

        private JsonDynamicObject(Dictionary<JsonProperty, JsonValue> properties)
        {
            _properties = properties;
        }

        private static Dictionary<Type, Dictionary<int, PropertyInfoLinkedList>> TypeCache = new Dictionary<Type, Dictionary<int, PropertyInfoLinkedList>>();

        public static T Deserialize<T>(ReadOnlySpan<byte> utf8)
        {
            if (!TypeCache.TryGetValue(typeof(T), out Dictionary<int, PropertyInfoLinkedList> dictionary))
            {
                dictionary = GetTypeMap(typeof(T));
                TypeCache.Add(typeof(T), dictionary);
            }

            T instance = Deserializer<T>.Deserialize(utf8);

            var reader = new JsonReader(utf8, SymbolTable.InvariantUtf8);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        var key = GetHashCode(reader.Value);
                        reader.Read(); // Move to the value token
                        var type = reader.ValueType;
                        switch (type)
                        {
                            case JsonValueType.String:
                                PropertyInfo pi = GetPropertyInfo(dictionary, key, reader.Value);
                                pi.SetValue(instance, new Utf8String(reader.Value));    // TODO: Use Ref.Emit instead of Reflection
                                break;
                            case JsonValueType.Object: // TODO: could this be lazy? Could this reuse the root JsonObject (which would store non-allocating JsonDom)?
                                throw new NotImplementedException("object support not implemented yet.");
                            case JsonValueType.True:
                                pi = GetPropertyInfo(dictionary, key, reader.Value);
                                pi.SetValue(instance, true);
                                break;
                            case JsonValueType.False:
                                pi = GetPropertyInfo(dictionary, key, reader.Value);
                                pi.SetValue(instance, false);
                                break;
                            case JsonValueType.Null:
                                pi = GetPropertyInfo(dictionary, key, reader.Value);
                                pi.SetValue(instance, null);
                                break;
                            case JsonValueType.Number:
                                pi = GetPropertyInfo(dictionary, key, reader.Value);
                                if (!Utf8Parser.TryParse(reader.Value, out int result, out _))
                                {
                                    throw new InvalidCastException();
                                }
                                pi.SetValue(instance, result);
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

            return instance;
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
            if (!_properties.TryGetValue(jsonProperty, out JsonValue jsonValue))
            {
                value = default;
                return false;
            }

            if (jsonValue.Type != JsonValueType.Number)
            {
                throw new InvalidOperationException();
            }
            return Utf8Parser.TryParse(jsonValue.Value.Bytes, out value, out _);
        }

        public bool TryGetString(Utf8String property, out Utf8Span value)
        {
            var jsonProperty = new JsonProperty(this, property);
            if (!_properties.TryGetValue(jsonProperty, out JsonValue jsonValue))
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
            var name = new Utf8String(binder.Name);
            var property = new JsonProperty(this, name);
            if (!_properties.TryGetValue(property, out JsonValue value))
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

        public bool TryFormat(Span<byte> buffer, out int written, StandardFormat format, SymbolTable symbolTable)
        {
            written = 0;
            if (!TryEncodeControlChar(symbolTable, (byte)'{', buffer, out int justWritten))
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

        private static int GetHashCode(ReadOnlySpan<byte> span)
        {
            int hash = 17;
            foreach (byte element in span)
            {
                hash = hash * 31 + element;
            }
            return hash;
        }

        private static Dictionary<int, PropertyInfoLinkedList> GetTypeMap(Type type)
        {
            var dictionary = new Dictionary<int, PropertyInfoLinkedList>();
            IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties();
            foreach (PropertyInfo pi in properties)
            {
                byte[] encoded = Encoding.UTF8.GetBytes(pi.Name);
                int hashcode = GetHashCode(encoded);

                if (dictionary.ContainsKey(hashcode))
                {
                    dictionary.TryGetValue(hashcode, out PropertyInfoLinkedList list);
                    (byte[], PropertyInfo) data = (encoded, pi);
                    list.Add(data);
                }
                else
                {
                    var list = new PropertyInfoLinkedList();
                    (byte[], PropertyInfo) data = (encoded, pi);
                    list.Add(data);
                    dictionary.Add(hashcode, list);
                }
            }
            return dictionary;
        }

        public static PropertyInfo GetPropertyInfo(Dictionary<int, PropertyInfoLinkedList> dictionary, int key, ReadOnlySpan<byte> span)
        {
            if (!dictionary.TryGetValue(key, out PropertyInfoLinkedList value))
            {
                throw new KeyNotFoundException();
            }
            PropertyInfoNode node = value.Head;
            PropertyInfo pi = node.Value.Item2;

            // This should be a very rare occurrence (only if there is hash collision)
            if (value.Count > 1)
            {
                while (true)
                {
                    if (span.SequenceEqual(node.Value.Item1))
                    {
                        pi = node.Value.Item2;
                        break;
                    }
                    node = node.Next;
                }
            }
            return pi;
        }

        struct JsonValue : IBufferFormattable
        {
            static readonly byte[] s_nullBytes = { (byte)'n', (byte)'u', (byte)'l', (byte)'l' };
            static readonly byte[] s_trueBytes = { (byte)'t', (byte)'r', (byte)'u', (byte)'e' };
            static readonly byte[] s_falseBytes = { (byte)'f', (byte)'a', (byte)'l', (byte)'s', (byte)'e' };

            JsonDynamicObject _object;
            Utf8String _value { get; set; }
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
                if (_type == JsonValueType.String) return _value.ToString();
                if (_type == JsonValueType.Number)
                {
                    return double.Parse(_value.ToString());
                }
                else throw new NotImplementedException();
            }

            static readonly byte[] nullValue = { (byte)'n', (byte)'u', (byte)'l', (byte)'l'};

            public bool TryFormat(Span<byte> buffer, out int written, StandardFormat format, SymbolTable symbolTable)
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
            Utf8String _name { get; set; }

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

            public override int GetHashCode() => _name.GetHashCode();

            public bool TryFormat(Span<byte> buffer, out int written, StandardFormat format, SymbolTable symbolTable)
            {
                return _name.TryFormatQuotedString(buffer, out written, format, symbolTable);
            }
        }
    }

    static class Utf8SpanExtensions
    {
        public static bool TryFormat(this Utf8Span str, Span<byte> buffer, out int written, StandardFormat format, SymbolTable symbolTable)
        {
            return symbolTable.TryEncode(str.Bytes, buffer, out var consumed, out written);
        }

        public static bool TryFormatQuotedString(this Utf8Span str, Span<byte> buffer, out int written, StandardFormat format, SymbolTable symbolTable)
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

        public static bool TryFormatQuotedString(this Utf8String str, Span<byte> buffer, out int written, StandardFormat format, SymbolTable symbolTable)
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
