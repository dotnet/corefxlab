// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    internal abstract class JsonPropertyInfo
    {
        public PropertyNamePolicyAttribute NameConverter;
        public bool DeserializeNullValues;
        public bool SerializeNullValues;
        public byte[] Name = default;
        public ClassType ClassType;
        
        // todo: to avoid hashtable lookups, cache this:
        //public JsonClassInfo ClassInfo;

        public JsonPropertyInfo(Type parentClassType, Type propertyType, PropertyInfo propertyInfo, Type elementType, JsonConverterSettings settings)
        { 
            ParentClassType = parentClassType;
            PropertyType = propertyType;
            PropertyInfo = propertyInfo;
            Settings = settings;
            ClassType = JsonClassInfo.GetClassType(propertyType);
            if (elementType != null)
            {
                ElementClassInfo = settings.GetOrAddClass(elementType);
            }

            IsNullableType = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            CanBeNull = IsNullableType || !propertyType.IsValueType;
        }

        public EnumerableConverterAttribute EnumerableConverter { get; private set; }

        public JsonConverterSettings Settings { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public Type PropertyType { get; private set; }

        public Type ParentClassType { get; private set; }

        public JsonClassInfo ElementClassInfo { get; private set; }

        public bool IsNullableType { get; private set; }

        public bool CanBeNull { get; private set; }

        public abstract object GetValueAsObject(object obj);
        public abstract void SetValueAsObject(object obj, object value);

        public abstract void FromJson(ref FromJsonObjectState current, ref Utf8JsonReader reader);

        protected internal abstract void FromJsonEnumerable(ref FromJsonObjectState current, ref Utf8JsonReader reader);

        public abstract void ToJson(ref ToJsonObjectState current, ref Utf8JsonWriter writer);

        protected internal abstract void ToJsonEnumerable(ref ToJsonObjectState current, ref Utf8JsonWriter writer);

        public abstract PropertyValueConverterAttribute GetValueConverter();

        public virtual void GetPolicies()
        {
            {
                PropertyNamePolicyAttribute attr = DefaultConverters.GetPolicy<PropertyNamePolicyAttribute>(ParentClassType, PropertyInfo, Settings);
                if (attr != null)
                {
                    NameConverter = attr;
                }
            }

            {
                DeserializeNullValues = DefaultConverters.GetPropertyClassAssemblyPolicy(ParentClassType, PropertyInfo, Settings, attr => attr.DeserializeNullValues).GetValueOrDefault();
                SerializeNullValues = DefaultConverters.GetPropertyClassAssemblyPolicy(ParentClassType, PropertyInfo, Settings, attr => attr.SerializeNullValues).GetValueOrDefault();
            }

            if (ElementClassInfo != null)
            {
                EnumerableConverter = DefaultConverters.GetEnumerableConverter(ParentClassType, PropertyInfo, Settings, PropertyType);
            }
        }
    }
}
