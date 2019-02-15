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
        public PropertyNamePolicyAttribute NameConverter;
        private bool? _skipNullValuesOnRead;
        private bool? _skipNullValuesOnWrite;
        public byte[] Name = default;
        public ClassType ClassType;
        
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

        public EnumerableConverterAttribute EnumerableConverter { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public Type PropertyType { get; private set; }

        public Type ParentClassType { get; private set; }

        public JsonClassInfo ElementClassInfo { get; private set; }

        public bool IsNullableType { get; private set; }

        public bool CanBeNull { get; private set; }

        public bool SkipNullValuesOnRead(JsonSerializerOptions options)
        {
            if (_skipNullValuesOnRead.HasValue)
            {
                return _skipNullValuesOnRead.Value;
            }

            return options.SkipNullValuesOnRead;
        }

        public bool SkipNullValuesOnWrite(JsonSerializerOptions options)
        {
            if (_skipNullValuesOnWrite.HasValue)
            {
                return _skipNullValuesOnWrite.Value;
            }

            return options.SkipNullValuesOnWrite;
        }

        public abstract object GetValueAsObject(object obj, JsonSerializerOptions options);
        public abstract void SetValueAsObject(object obj, object value, JsonSerializerOptions options);

        public abstract void Read(JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader);

        protected internal abstract void ReadEnumerable(JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader);

        public abstract void Write(JsonSerializerOptions options, ref WriteObjectState current, ref Utf8JsonWriter writer);

        protected internal abstract void WriteEnumerable(JsonSerializerOptions options, ref WriteObjectState current, ref Utf8JsonWriter writer);

        public abstract PropertyValueConverterAttribute GetValueConverter();

        public virtual void GetPolicies(JsonSerializerOptions options)
        {
            {
                PropertyNamePolicyAttribute attr = DefaultConverters.GetPolicy<PropertyNamePolicyAttribute>(ParentClassType, PropertyInfo, options);
                if (attr != null)
                {
                    NameConverter = attr;
                }
            }

            {
                _skipNullValuesOnRead = DefaultConverters.GetPropertyClassAssemblyPolicy(ParentClassType, PropertyInfo, options, attr => attr.SkipNullValuesOnRead);
                _skipNullValuesOnWrite = DefaultConverters.GetPropertyClassAssemblyPolicy(ParentClassType, PropertyInfo, options, attr => attr.SkipNullValuesOnWrite);
            }

            if (ElementClassInfo != null)
            {
                EnumerableConverter = DefaultConverters.GetEnumerableConverter(ParentClassType, PropertyInfo, PropertyType, options);
            }
        }
    }
}
