// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    internal partial class JsonClassInfo
    {
        // The length of the property name embedded in the key (in bytes).
        private const int PropertyNameKeyLength = 6;

        public delegate object ConstructorDelegate();
        public ConstructorDelegate CreateObject { get; private set; }

        private List<PropertyRef> _property_refs = new List<PropertyRef>();
        private List<PropertyRef> _property_refs_sorted = new List<PropertyRef>();

        public Type Type { get; private set; }
        public ClassType ClassType { get; private set; }

        public EnumerableConverterAttribute EnumerableConverter { get; private set; }

        // If enumerable, the info for the element type.
        public JsonClassInfo ElementClassInfo { get; set; }

        internal JsonClassInfo(Type type, JsonConverterSettings settings)
        {
            Type = type;
            ClassType = GetClassType(type);

            CreateObject = settings.ClassMaterializerStrategy.CreateConstructor(type);

            // Ignore properties on enumerable.
            if (ClassType == ClassType.Object)
            {
                foreach (PropertyInfo propertyInfo in type.GetProperties())
                {
                    AddProperty(propertyInfo.PropertyType, propertyInfo, type, settings);
                }
            }
            else if (ClassType == ClassType.Enumerable)
            {
                // Add a single property that maps to the class type so we can have policies applied.
                AddProperty(type, propertyInfo : null, type, settings);

                // Create a ClassInfo that maps to the element type which is used for (de)serialization and policies.
                Type elementType = GetElementType(type);
                ElementClassInfo = settings.GetOrAddClass(elementType);

                GetPolicies(settings);
            }
            else
            {
                Debug.Assert(ClassType == ClassType.Value);

                // Add a single property that maps to the class type so we can have policies applied.
                AddProperty(type, null, type, settings);
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
#if BUILDING_INBOX_LIBRARY
                string stringPropertyName = JsonConverter.s_utf8Encoding.GetString(propertyName);
#else
                string stringPropertyName;
                if (propertyName.IsEmpty)
                {
                    stringPropertyName = string.Empty;
                }
                unsafe
                {
                    fixed (byte* bytePtr = propertyName)
                    {
                        stringPropertyName = JsonConverter.s_utf8Encoding.GetString(bytePtr, propertyName.Length);
                    }
                }
#endif
                throw new InvalidOperationException($"todo: invalid property {stringPropertyName}");
            }

            _property_refs_sorted.Add(new PropertyRef(key, info));

            return info;
        }

        public JsonPropertyInfo GetPolicyProperty()
        {
            Debug.Assert(_property_refs.Count == 1);
            return _property_refs[0].Info;
        }

        public JsonPropertyInfo GetProperty(int index)
        {
            Debug.Assert(index < _property_refs.Count);
            return _property_refs[index].Info;
        }

        public ReadOnlySpan<byte> GetPropertyName(int index)
        {
            Debug.Assert(index < _property_refs.Count);
            return _property_refs[index].Info.Name;
        }

        public int PropertyCount
        {
            get
            {
                return _property_refs.Count;
            }
        }

        private static bool TryIsPropertyRefEqual(List<PropertyRef> list, ReadOnlySpan<byte> propertyName, ulong key, int index, out JsonPropertyInfo info)
        {
            if (key == list[index].Key)
            {
                if (propertyName.Length <= PropertyNameKeyLength ||
                    // todo: is it really any faster to do slices here to avoid comparing the first 6 bytes?
                    propertyName.Slice(PropertyNameKeyLength).SequenceEqual(((ReadOnlySpan<byte>)list[index].Info.Name).Slice(PropertyNameKeyLength)))
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

            ulong key;
            int length = propertyName.Length;

            // Embed the propertyName in the first 6 bytes of the key.
            if (length > 3)
            {
                key = MemoryMarshal.Read<uint>(propertyName);
                if (length > 4)
                {
                    key |= (ulong)propertyName[4] << 32;
                }
                if (length > 5)
                {
                    key |= (ulong)propertyName[5] << 40;
                }
            }
            else if (length > 1)
            {
                key = MemoryMarshal.Read<ushort>(propertyName);
                if (length > 2)
                {
                    key |= (ulong)propertyName[2] << 16;
                }
            }
            else
            {
                key = propertyName[0];
            }

            // Embed the propertyName length in the last two bytes.
            key |= (ulong)propertyName.Length << 48;
            return key;

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

        internal static ClassType GetClassType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            // A Type is considered a value if it implements IConvertible.
            if (typeof(IConvertible).IsAssignableFrom(type))
                return ClassType.Value;

            if (typeof(IEnumerable).IsAssignableFrom(type))
                return ClassType.Enumerable;

            return ClassType.Object;
        }

        public void GetPolicies(JsonConverterSettings settings)
        {
            if (ClassType == ClassType.Enumerable)
            {
                EnumerableConverter = DefaultConverters.GetEnumerableConverter(Type, propertyInfo: null, settings, Type);
            }
        }
    }
}
