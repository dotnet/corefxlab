// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    internal abstract class JsonPropertyInfo
    {
        public JsonPropertyNamePolicyAttribute NameConverter;
        private bool? _ignoreNullPropertyValueOnRead;
        private bool? _ignoreNullPropertyValueOnWrite;

        public ClassType ClassType;

        public byte[] Name = default;
        public byte[] EscapedName = default;
        public bool HasEscapedName = false;
        
        // todo: to avoid hashtable lookups, cache this:
        //public JsonClassInfo ClassInfo;

        public JsonPropertyInfo(Type parentClassType, Type propertyType, PropertyInfo propertyInfo, Type elementType, JsonSerializerOptions options)
        { 
            ParentClassType = parentClassType;
            PropertyType = propertyType;
            PropertyInfo = propertyInfo;
            ClassType = JsonClassInfo.GetClassType(propertyType);
            if (elementType != null)
            {
                ElementClassInfo = options.GetOrAddClass(elementType);
            }

            IsNullableType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            CanBeNull = IsNullableType || !propertyType.IsValueType;
        }

        public JsonEnumerableConverter EnumerableConverter { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public Type PropertyType { get; private set; }

        public Type ParentClassType { get; private set; }

        public JsonClassInfo ElementClassInfo { get; private set; }

        public bool IsNullableType { get; private set; }

        public bool CanBeNull { get; private set; }

        public bool IgnoreNullPropertyValueOnRead(JsonSerializerOptions options)
        {
            if (_ignoreNullPropertyValueOnRead.HasValue)
            {
                return _ignoreNullPropertyValueOnRead.Value;
            }

            return options.IgnoreNullPropertyValueOnRead;
        }

        public bool IgnoreNullPropertyValueOnWrite(JsonSerializerOptions options)
        {
            if (_ignoreNullPropertyValueOnWrite.HasValue)
            {
                return _ignoreNullPropertyValueOnWrite.Value;
            }

            return options.IgnoreNullPropertyValueOnWrite;
        }

        public abstract object GetValueAsObject(object obj, JsonSerializerOptions options);
        public abstract void SetValueAsObject(object obj, object value, JsonSerializerOptions options);

        public abstract void Read(JsonTokenType tokenType, JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader);

        protected internal abstract void ReadEnumerable(JsonTokenType tokenType, JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader);

        public abstract void Write(JsonSerializerOptions options, ref WriteObjectState current, ref Utf8JsonWriter writer);

        protected internal abstract void WriteEnumerable(JsonSerializerOptions options, ref WriteObjectState current, ref Utf8JsonWriter writer);

        //public abstract JsonValueConverterAttribute GetValueConverter();

        public virtual void GetPolicies(JsonSerializerOptions options)
        {
            {
                JsonPropertyNamePolicyAttribute attr = DefaultConverters.GetPolicy<JsonPropertyNamePolicyAttribute>(ParentClassType, PropertyInfo, options);
                if (attr != null)
                {
                    NameConverter = attr;
                }
            }

            {
                _ignoreNullPropertyValueOnRead = DefaultConverters.GetPropertyClassAssemblyPolicy(ParentClassType, PropertyInfo, options, attr => attr.IgnoreNullValueOnRead);
                _ignoreNullPropertyValueOnWrite = DefaultConverters.GetPropertyClassAssemblyPolicy(ParentClassType, PropertyInfo, options, attr => attr.IgnoreNullValueOnWrite);
            }

            if (ElementClassInfo != null)
            {
                EnumerableConverter = DefaultConverters.GetEnumerableConverter(ParentClassType, PropertyInfo, PropertyType, options);
            }
        }
    }
}
