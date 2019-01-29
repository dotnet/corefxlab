// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace System.Text.Json.Serialization
{
    internal class JsonClassInfo
    {
        // The length of the property name embedded in the key (in bytes).
        private const int PropertyNameKeyLength = 6;

        public delegate object ConstructorDelegate();
        public ConstructorDelegate CreateObject { get; private set; }

        private List<PropertyRef> _property_refs = new List<PropertyRef>();
        private List<PropertyRef> _property_refs_sorted = new List<PropertyRef>();

        public Type ClassType { get; private set; }

        internal JsonClassInfo(Type classType, JsonConverterSettings options)
        {
            ClassType = classType;

            CreateObject = options.ClassMaterializerStrategy.CreateConstructor(classType);

            // Ignore properties on enumerable.
            if (classType.IsPrimitive || typeof(IEnumerable).IsAssignableFrom(classType))
            {
                // Create a property that maps to the classType so we can have policies applied.
                AddProperty(classType, null, classType, options);
            }
            else
            {
                foreach (PropertyInfo propertyInfo in classType.GetProperties())
                {
                    AddProperty(propertyInfo.PropertyType, propertyInfo, classType, options);
                }
            }
        }

        private void AddProperty(Type propertyType, PropertyInfo propertyInfo, Type classType, JsonConverterSettings options)
        {
            JsonPropertyInfo jsonInfo = null;

            if (propertyType.IsValueType)
            {
                // More common types are listed first.
                if (propertyType == typeof(int))
                {
                    jsonInfo = new JsonPropertyInfo<int>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(bool))
                {
                    jsonInfo = new JsonPropertyInfo<bool>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(decimal))
                {
                    jsonInfo = new JsonPropertyInfo<decimal>(classType, propertyType, propertyInfo, options);
                }
                if (propertyType == typeof(DateTime))
                {
                    jsonInfo = new JsonPropertyInfo<DateTime>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(double))
                {
                    jsonInfo = new JsonPropertyInfo<double>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(long))
                {
                    jsonInfo = new JsonPropertyInfo<long>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(short))
                {
                    jsonInfo = new JsonPropertyInfo<short>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(float))
                {
                    jsonInfo = new JsonPropertyInfo<float>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType.IsEnum)
                {
                    jsonInfo = new JsonPropertyInfo<Enum>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(byte))
                {
                    jsonInfo = new JsonPropertyInfo<byte>(classType, propertyType, propertyInfo, options);
                }
                else if (propertyType == typeof(ushort))
                {
                    jsonInfo = new JsonPropertyInfo<ushort>(classType, propertyType, propertyInfo, options);
                }
                else if(propertyType == typeof(uint))
                {
                    jsonInfo = new JsonPropertyInfo<uint>(classType, propertyType, propertyInfo, options);
                }
                else if(propertyType == typeof(ulong))
                {
                    jsonInfo = new JsonPropertyInfo<ulong>(classType, propertyType, propertyInfo, options);
                }
                else
                {
                    Type genericPropertyType = typeof(JsonPropertyInfo<>).MakeGenericType(propertyType);
                    jsonInfo = (JsonPropertyInfo)Activator.CreateInstance(genericPropertyType, new object[] { classType, propertyType, propertyInfo, options, null });
                }
            }
            else
            {
                if (propertyType == typeof(string))
                {
                    jsonInfo = new JsonPropertyInfo<string>(classType, propertyType, propertyInfo, options);
                }
                else
                {
                    Type genericPropertyType = typeof(JsonPropertyInfo<>).MakeGenericType(propertyType);
                    Type collectionElementType = GetElementType(propertyType);
                    jsonInfo = (JsonPropertyInfo)Activator.CreateInstance(genericPropertyType, new object[] { classType, propertyType, propertyInfo, options, collectionElementType });
                }
            }

            if (propertyInfo != null)
            {
                string propertyName = jsonInfo.NameConverter == null ? propertyInfo.Name : jsonInfo.NameConverter.ToJson(propertyInfo.Name);
                var bytes = (ReadOnlySpan<byte>)Encoding.Default.GetBytes(propertyName);
                if (bytes.Length > PropertyNameKeyLength)
                {
                    jsonInfo.Key = bytes.Slice(PropertyNameKeyLength).ToArray();
                }

                _property_refs.Add(new PropertyRef(GetKey(bytes), jsonInfo));
            }
            else
            {
                _property_refs.Add(new PropertyRef(0, jsonInfo));
            }
        }

        public JsonPropertyInfo GetProperty(Type classType, ReadOnlySpan<byte> propertyName, int propertyIndex)
        {
            ulong key = GetKey(propertyName);
            JsonPropertyInfo info = null;

            // First try sorted lookup.
            int count = _property_refs_sorted.Count;
            if (count != 0)
            {
                int iForward = propertyIndex;
                int iBackward = propertyIndex - 1;
                while (iForward < count || (iBackward >= 0 && iBackward < count))
                {
                    if (iForward < count)
                    {
                        if (TryIsPropertyRefEqual(_property_refs_sorted, propertyName, key, iForward, out info))
                        {
                            return info;
                        }
                        ++iForward;
                    }

                    if (iBackward >= 0)
                    {
                        if (TryIsPropertyRefEqual(_property_refs_sorted, propertyName, key, iBackward, out info))
                        {
                            return info;
                        }
                        --iBackward;
                    }
                }
            }

            // Then try fallback
            for (int i = 0; i < _property_refs.Count; i++)
            {
                if (TryIsPropertyRefEqual(_property_refs, propertyName, key, i, out info))
                {
                    break;
                }
            }

            if (info == null)
            {
                string stringPropertyName = JsonConverter.s_utf8Encoding.GetString(propertyName);
                throw new InvalidOperationException($"todo: invalid property {stringPropertyName}");
            }

            _property_refs_sorted.Add(new PropertyRef(key, info));

            return info;
        }

        private static bool TryIsPropertyRefEqual(List<PropertyRef> list, ReadOnlySpan<byte> propertyName, ulong key, int index, out JsonPropertyInfo info)
        {
            if (key == list[index].Key)
            {
                if (propertyName.Length <= PropertyNameKeyLength ||
                    propertyName.Slice(PropertyNameKeyLength).SequenceEqual((ReadOnlySpan<byte>)list[index].Info.Key))
                {
                    info = list[index].Info;
                    return true;
                }
            }

            info = null;
            return false;
        }

        private static ulong GetKey(ReadOnlySpan<byte> propertyName)
        {
            Debug.Assert(propertyName.Length > 0);

            int length = propertyName.Length;

            // Embed the propertyName in the first 6 bytes of the key.
            ulong key = propertyName[0];
            if (length > 1)
            {
                key |= (ulong)propertyName[1] << 8;
                if (length > 2)
                {
                    key |= (ulong)propertyName[2] << 16;
                    if (length > 3)
                    {
                        key |= (ulong)propertyName[3] << 24;
                        if (length > 4)
                        {
                            key |= (ulong)propertyName[4] << 32;
                            if (length > 5)
                            {
                                key |= (ulong)propertyName[5] << 40;
                            }
                        }
                    }
                }
            }

            // Embed the propertyName length in the last two bytes.
            key |= (ulong)propertyName.Length << 48;
            return key;
        }

        public JsonPropertyInfo GetPolicyProperty()
        {
            Debug.Assert(_property_refs.Count == 1);
            return _property_refs[0].Info;
        }

        private static Type GetElementType(Type propertyType)
        {
            Type elementType = null;
            if (typeof(IEnumerable).IsAssignableFrom(propertyType))
            {
                elementType = propertyType.GetElementType();
                if (elementType == null)
                {
                    if (propertyType.IsGenericType)
                    {
                        elementType = propertyType.GetGenericArguments()[0];
                    }
                    else
                    {
                        throw new InvalidOperationException("todo: can't determine base collection type");
                    }
                }
            }

            return elementType;
        }
    }
}
