// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization.Converters
{
    internal static class DefaultConverters
    {
        public static readonly JsonDateConverterAttribute s_dateConverterAttibute = new JsonDateConverterAttribute();
        public static readonly JsonEnumConverterAttribute s_enumConverterAttibute = new JsonEnumConverterAttribute();
        public static readonly JsonArrayConverterAttribute s_arrayConverterAttibute = new JsonArrayConverterAttribute();

        public static TAttribute GetPolicy<TAttribute>(Type parentClassType, PropertyInfo propertyInfo, JsonConverterSettings settings) where TAttribute : Attribute
        {
            TAttribute attr = null;
            if (propertyInfo != null)
            {
                // Use Property first
                attr = settings.GetAttributes<TAttribute>(propertyInfo).FirstOrDefault();
            }

            if (attr == null)
            {
                // Then class type
                attr = settings.GetAttributes<TAttribute>(parentClassType, inherit: true).FirstOrDefault();

                if (attr == null)
                {
                    // Then declaring assembly
                    attr = settings.GetAttributes<TAttribute>(parentClassType.Assembly).FirstOrDefault();
                }
            }

            return attr;
        }

        public static TValue GetPropertyPolicy<TValue>(PropertyInfo propertyInfo, JsonConverterSettings settings, Func<JsonPropertyAttribute, TValue> selector)
        {
            TValue value;

            JsonPropertyAttribute attr = null;
            if (propertyInfo != null)
            {
                attr = settings.GetAttributes<JsonPropertyAttribute>(propertyInfo).FirstOrDefault();
            }

            if (attr == null)
            {
                value = default;
            }
            else
            {
                value = selector(attr);
            }

            return value;
        }

        public static TValue GetPropertyClassAssemblyPolicy<TValue>(Type parentClassType, PropertyInfo propertyInfo, JsonConverterSettings settings, Func<JsonPropertyAttribute, TValue> selector)
        {
            TValue value;

            JsonPropertyAttribute attr = null;
            if (propertyInfo != null)
            {
                // Use Property first
                attr = settings.GetAttributes<JsonPropertyAttribute>(propertyInfo).FirstOrDefault();
            }

            if (attr == null || (value = selector(attr)) == default)
            {
                // Then class type
                attr = settings.GetAttributes<JsonPropertyAttribute>(parentClassType, inherit: true).FirstOrDefault();
                if (attr == null || (value = selector(attr)) == default)
                {
                    // Then declaring assembly
                    attr = settings.GetAttributes<JsonPropertyAttribute>(parentClassType.Assembly).FirstOrDefault();

                    if (attr == null)
                    {
                        value = default;
                    }
                    else
                    {
                        value = selector(attr);
                    }
                }
            }

            return value;
        }

        public static EnumerableConverterAttribute GetEnumerableConverter(
            Type parentClassType,
            PropertyInfo propertyInfo,
            JsonConverterSettings settings,
            Type propertyType)
        {
            Type enumerableType;
            if (propertyType.IsGenericType)
            {
                enumerableType = propertyType.GetGenericTypeDefinition();
            }
            else if (propertyType.IsArray)
            {
                enumerableType = typeof(Array);
            }
            else
            {
                enumerableType = propertyType;
            }

            EnumerableConverterAttribute attr = null;
            if (propertyInfo != null)
            {
                // Use Property first
                attr = settings.GetAttributes<EnumerableConverterAttribute>(propertyInfo).Where(a => a.EnumerableType == enumerableType).FirstOrDefault();
            }

            if (attr == null)
            {
                // Then class type
                attr = settings.GetAttributes<EnumerableConverterAttribute>(parentClassType, inherit: true).Where(a => a.EnumerableType == enumerableType).FirstOrDefault();
                if (attr == null)
                {
                    // Then declaring assembly
                    attr = settings.GetAttributes<EnumerableConverterAttribute>(parentClassType.Assembly).Where(a => a.EnumerableType == enumerableType).FirstOrDefault();

                    if (attr == null)
                    {
                        // Then default
                        if (enumerableType == typeof(Array))
                        {
                            attr = s_arrayConverterAttibute;
                        }
                    }
                }
            }

            return attr;
        }

        public static PropertyValueConverterAttribute GetPropertyValueConverter(
            Type parentClassType,
            PropertyInfo propertyInfo,
            JsonConverterSettings settings,
            Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            PropertyValueConverterAttribute attr = GetPropertyValueConverterInternal(parentClassType, propertyInfo, settings, propertyType);

            // For Enums, support both the type Enum plus strongly-typed Enums.
            if (attr == null && (propertyType.IsEnum || propertyType == typeof(Enum)))
            {
                attr = DefaultConverters.GetPropertyValueConverterInternal(parentClassType, propertyInfo, settings, typeof(Enum));
                if (attr == null)
                {
                    attr = DefaultConverters.s_enumConverterAttibute;
                }
            }

            return attr;
        }

        private static PropertyValueConverterAttribute GetPropertyValueConverterInternal(
            Type parentClassType,
            PropertyInfo propertyInfo,
            JsonConverterSettings settings,
            Type propertyType)
        {
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propertyType = Nullable.GetUnderlyingType(propertyType);
            }

            PropertyValueConverterAttribute attr = null;
            if (propertyInfo != null)
            {
                // Use Property first
                attr = settings.GetAttributes<PropertyValueConverterAttribute>(propertyInfo).Where(a => a.PropertyType == propertyType).FirstOrDefault();
            }

            if (attr == null)
            {
                // Then class type
                attr = settings.GetAttributes<PropertyValueConverterAttribute>(parentClassType, inherit: true).Where(a => a.PropertyType == propertyType).FirstOrDefault();
                if (attr == null)
                {
                    // Then declaring assembly
                    attr = settings.GetAttributes<PropertyValueConverterAttribute>(parentClassType.Assembly).Where(a => a.PropertyType == propertyType).FirstOrDefault();

                    if (attr == null)
                    {
                        // Then default
                        if (propertyType == typeof(DateTime))
                        {
                            attr = DefaultConverters.s_dateConverterAttibute;
                        }
                    }
                }
            }

            return attr;
        }
    }
}

