// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    internal abstract class JsonPropertyInfo
    {
        protected static readonly JsonDateConverterAttribute s_dateConverterAttibute = new JsonDateConverterAttribute();
        protected static readonly JsonEnumConverterAttribute s_enumConverterAttibute = new JsonEnumConverterAttribute();
        protected static readonly JsonArrayConverterAttribute s_arrayConverterAttibute = new JsonArrayConverterAttribute();

        public PropertyNamePolicyAttribute NameConverter;
        public bool IncludeNullValues;
        public byte[] Key = null;

        public JsonPropertyInfo(Type classType, Type propertyType, PropertyInfo propertyInfo, Type elementType, JsonConverterSettings options)
        {
            ClassType = classType;
            PropertyType = propertyType;
            PropertyInfo = propertyInfo;
            ElementType = elementType;
            Options = options;
        }

        public EnumerableConverterAttribute EnumerableConverter { get; private set; }

        public JsonConverterSettings Options { get; private set; }

        public PropertyInfo PropertyInfo { get; private set; }

        public Type PropertyType { get; private set; }

        public Type ClassType { get; private set; }

        public Type ElementType { get; private set; }

        public abstract Type TempListType { get; }

        public abstract object GetValueAsObject(object obj);
        public abstract void SetValueAsObject(object obj, object value);

        public abstract PropertyValueConverterAttribute GetValueConverter();

        public virtual void GetPolicies()
        {
            {
                PropertyNamePolicyAttribute attr = GetPolicy<PropertyNamePolicyAttribute>();
                if (attr != null)
                {
                    NameConverter = attr;
                }
            }

            {
                IncludeNullValues = GetPropertyClassAssemblyPolicy(attr => attr.IncludeNullValues).GetValueOrDefault();
            }

            if (ElementType != null)
            {
                EnumerableConverter = GetEnumerableConverter();
            }
        }

        protected TAttribute GetPolicy<TAttribute>() where TAttribute : Attribute
        {
            TAttribute attr = null;
            if (PropertyInfo != null)
            {
                // Use Property first
                attr = Options.GetAttributes<TAttribute>(PropertyInfo).FirstOrDefault();
            }

            if (attr == null)
            {
                // Then class type
                attr = Options.GetAttributes<TAttribute>(ClassType, inherit: true).FirstOrDefault();

                if (attr == null)
                {
                    // Then declaring assembly
                    attr = Options.GetAttributes<TAttribute>(ClassType.Assembly).FirstOrDefault();
                }
            }

            return attr;
        }

        protected TValue GetPropertyPolicy<TValue>(Func<JsonPropertyAttribute, TValue> selector)
        {
            TValue value;

            JsonPropertyAttribute attr = null;
            if (PropertyInfo != null)
            {
                attr = Options.GetAttributes<JsonPropertyAttribute>(PropertyInfo).FirstOrDefault();
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

        protected TValue GetPropertyClassAssemblyPolicy<TValue>(Func<JsonPropertyAttribute, TValue> selector)
        {
            TValue value;

            JsonPropertyAttribute attr = null;
            if (PropertyInfo != null)
            {
                // Use Property first
                attr = Options.GetAttributes<JsonPropertyAttribute>(PropertyInfo).FirstOrDefault();
            }

            if (attr == null || (value = selector(attr)) == default)
            {
                // Then class type
                attr = Options.GetAttributes<JsonPropertyAttribute>(ClassType, inherit: true).FirstOrDefault();
                if (attr == null || (value = selector(attr)) == default)
                {
                    // Then declaring assembly
                    attr = Options.GetAttributes<JsonPropertyAttribute>(ClassType.Assembly).FirstOrDefault();

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

        protected EnumerableConverterAttribute GetEnumerableConverter()
        {
            Debug.Assert(ElementType != null);

            Type enumerableType;
            if (PropertyType.IsGenericType)
            {
                enumerableType = PropertyType.GetGenericTypeDefinition();
            }
            else if(PropertyType.IsArray)
            {
                enumerableType = typeof(Array);
            }
            else
            {
                enumerableType = PropertyType;
            }

            EnumerableConverterAttribute attr = null;
            if (PropertyInfo != null)
            {
                // Use Property first
                attr = Options.GetAttributes<EnumerableConverterAttribute>(PropertyInfo).Where(a => a.EnumerableType == enumerableType).FirstOrDefault();
            }

            if (attr == null)
            {
                // Then class type
                attr = Options.GetAttributes<EnumerableConverterAttribute>(ClassType, inherit: true).Where(a => a.EnumerableType == enumerableType).FirstOrDefault();
                if (attr == null)
                {
                    // Then declaring assembly
                    attr = Options.GetAttributes<EnumerableConverterAttribute>(ClassType.Assembly).Where(a => a.EnumerableType == enumerableType).FirstOrDefault();

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
    }

    internal class JsonPropertyInfo<TValue> : JsonPropertyInfo
    {
        public delegate TValue GetterDelegate(object obj);
        public delegate void SetterDelegate(object obj, TValue value);
        public PropertyValueConverterAttribute ValueConverter { get; private set; }

        public SetterDelegate Set { get; private set; }
        public GetterDelegate Get { get; private set; }

        public JsonPropertyInfo(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings options, Type elementType = null) :
            base(classType, propertyType, propertyInfo, elementType, options)
        {
            if (propertyInfo != null)
            {
                Set = Options.ClassMaterializerStrategy.CreateSetter<TValue>(propertyInfo);
                Get = Options.ClassMaterializerStrategy.CreateGetter<TValue>(propertyInfo);
            }
            else
            {
                // Used when current.obj contains the return value and this is the property policy.
                Get = delegate (object obj)
                {
                    return (TValue)obj;
                };
            }

            GetPolicies();
        }

        public override Type TempListType
        {
            get
            {
                return typeof(List<TValue>);
            }
        }

        public override object GetValueAsObject(object obj)
        {
            Debug.Assert(Get != null);
            return Get(obj);
        }

        public override void SetValueAsObject(object obj, object value)
        {
            Debug.Assert(Set != null);
            Set(obj, (TValue)value);
        }

        public override PropertyValueConverterAttribute GetValueConverter()
        {
            return ValueConverter;
        }

        public override void GetPolicies()
        {
            // ValueConverter
            ValueConverter = GetPropertyValueConverter<TValue>();

            base.GetPolicies();
        }

        protected PropertyValueConverterAttribute GetPropertyValueConverter<TProperty>()
        {
            Type propertyType = typeof(TProperty);

            PropertyValueConverterAttribute attr = null;
            if (PropertyInfo != null)
            {
                // Use Property first
                attr = Options.GetAttributes<PropertyValueConverterAttribute>(PropertyInfo).Where(a => a.PropertyType == propertyType).FirstOrDefault();
            }

            if (attr == null)
            {
                // Then class type
                attr = Options.GetAttributes<PropertyValueConverterAttribute>(ClassType, inherit: true).Where(a => a.PropertyType == propertyType).FirstOrDefault();
                if (attr == null)
                {
                    // Then declaring assembly
                    attr = Options.GetAttributes<PropertyValueConverterAttribute>(ClassType.Assembly).Where(a => a.PropertyType == propertyType).FirstOrDefault();

                    if (attr == null)
                    {
                        // Then default
                        if (propertyType == typeof(DateTime))
                        {
                            attr = s_dateConverterAttibute;
                        }
                        else if (propertyType == typeof(Enum))
                        {
                            attr = s_enumConverterAttibute;
                        }
                    }
                }
            }

            return attr;
        }
    }
}
