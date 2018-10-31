// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.Utf8;

namespace System.Text.JsonLab
{
    public class JsonSerializer
    {
        private static Dictionary<Type, Dictionary<int, PropertyInfoLinkedList>> TypeCache = new Dictionary<Type, Dictionary<int, PropertyInfoLinkedList>>();

        public static T Deserialize<T>(ReadOnlySpan<byte> utf8)
        {
            if (!TypeCache.TryGetValue(typeof(T), out Dictionary<int, PropertyInfoLinkedList> dictionary))
            {
                dictionary = GetTypeMap(typeof(T));
                TypeCache.Add(typeof(T), dictionary);
            }

            T instance = Create<T>.CreateInstanceOfType(utf8);

            var reader = new JsonUtf8Reader(utf8);
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.PropertyName:
                        int key = GetHashCode(reader.ValueSpan);
                        reader.Read(); // Move to the value token
                        JsonTokenType type = reader.TokenType;
                        switch (type)
                        {
                            case JsonTokenType.String:
                                PropertyInfo pi = GetPropertyInfo(dictionary, key, reader.ValueSpan);
                                pi.SetValue(instance, new Utf8String(reader.ValueSpan));    // TODO: Use Ref.Emit instead of Reflection
                                break;
                            case JsonTokenType.StartObject: // TODO: could this be lazy? Could this reuse the root JsonObject (which would store non-allocating JsonDom)?
                                throw new NotImplementedException("object support not implemented yet.");
                            case JsonTokenType.True:
                                pi = GetPropertyInfo(dictionary, key, reader.ValueSpan);
                                pi.SetValue(instance, true);
                                break;
                            case JsonTokenType.False:
                                pi = GetPropertyInfo(dictionary, key, reader.ValueSpan);
                                pi.SetValue(instance, false);
                                break;
                            case JsonTokenType.Null:
                                pi = GetPropertyInfo(dictionary, key, reader.ValueSpan);
                                pi.SetValue(instance, null);
                                break;
                            case JsonTokenType.Number:
                                pi = GetPropertyInfo(dictionary, key, reader.ValueSpan);
                                // TODO: Add support for other numeric types like double, long, etc.
                                if (!Utf8Parser.TryParse(reader.ValueSpan, out int result, out _))
                                {
                                    throw new InvalidCastException();
                                }
                                pi.SetValue(instance, result);
                                break;
                            case JsonTokenType.StartArray:
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
                    case JsonTokenType.String:
                    case JsonTokenType.True:
                    case JsonTokenType.False:
                    case JsonTokenType.Null:
                    case JsonTokenType.Number:
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            return instance;
        }

        private static PropertyInfo GetPropertyInfo(Dictionary<int, PropertyInfoLinkedList> dictionary, int key, ReadOnlySpan<byte> span)
        {
            if (!dictionary.TryGetValue(key, out PropertyInfoLinkedList value))
            {
                throw new KeyNotFoundException();
            }
            PropertyInfoNode node = value.Head;
            PropertyInfo pi = node.Value.propertyInfo;

            // This should be a very rare occurrence (only if there is hash collision)
            if (value.Count > 1)
            {
                while (node != null)
                {
                    if (span.SequenceEqual(node.Value.encodedName))
                    {
                        pi = node.Value.propertyInfo;
                        break;
                    }
                    node = node.Next;
                }
            }
            return pi;
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

        // Build a dictionary once per type (and cache it).
        // The (int, PropertyInfoLinkedList) mapping is needed since our JsonReader Value property returns a ReadOnlySpan<byte>
        // which cannot be stored in a collection without allocating.
        // For fast access to the property info with the matching name, we calculate the hashcode of each property within the type
        // and compare it to the hashcode of the ReadOnlySpan<byte> returned by the JsonReader
        // Since it is possible for their to be a collision, we maintain a linked list of property info found.
        private static Dictionary<int, PropertyInfoLinkedList> GetTypeMap(Type type)
        {
            var dictionary = new Dictionary<int, PropertyInfoLinkedList>();
            // TODO: Consider adding properties to the dictionary lazily.
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

        private static class Create<T>
        {
            private static readonly Func<T> _createInstance = CreateDelegate();

            private static Func<T> CreateDelegate()
            {
                var typeInfo = typeof(T).GetTypeInfo();
                if (typeInfo.IsValueType)
                {
                    throw new NotImplementedException(); //TODO: Support struct creation
                }
                DynamicMethod method = new DynamicMethod("CreateInstanceDynamicMethod", typeof(T), null, restrictedSkipVisibility: true);

                // GetConstructors() is only available on netstandard 2.0
                IEnumerable<ConstructorInfo> ctors = typeInfo.DeclaredConstructors;
                ConstructorInfo constructor = default;
                // TODO: Add support for non-default constructors
                foreach (ConstructorInfo ci in ctors)
                {
                    constructor = ci;
                    break;
                }

                ILGenerator generator = method.GetILGenerator();
                generator.Emit(OpCodes.Newobj, constructor);
                generator.Emit(OpCodes.Ret);
                return (Func<T>)method.CreateDelegate(typeof(Func<T>));
            }

            public static T CreateInstanceOfType(ReadOnlySpan<byte> data) => _createInstance();
        }
    }
}
