// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization.Converters;
using System.Text.Json.Serialization.Policies;

namespace System.Text.Json.Serialization
{
    internal class JsonPropertyInfo<TValue> : JsonPropertyInfo
    {
        public delegate TValue GetterDelegate(object obj);
        public delegate void SetterDelegate(object obj, TValue value);
        public PropertyValueConverterAttribute ValueConverter { get; private set; }

        public GetterDelegate Get { get; private set; }
        public SetterDelegate Set { get; private set; }

        public JsonPropertyInfo(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonConverterSettings settings, Type elementType = null) :
            base(classType, propertyType, propertyInfo, elementType, settings)
        {
            if (propertyInfo != null)
            {
                Get = Settings.ClassMaterializerStrategy.CreateGetter<TValue>(propertyInfo);
                Set = Settings.ClassMaterializerStrategy.CreateSetter<TValue>(propertyInfo);
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

        public override void FromJson(ref FromJsonObjectState current, ref Utf8JsonReader reader)
        {
            if (ElementClassInfo != null)
            {
                // Forward the setter to the value-based JsonPropertyInfo.
                JsonPropertyInfo propertyInfo = ElementClassInfo.GetPolicyProperty();
                propertyInfo.FromJsonEnumerable(ref current, ref reader);
            }
            else
            {
                if (ValueConverter != null)
                {
                    Type propertyType = PropertyType;
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    object value = ValueConverter.GetFromJson(ref reader, propertyType);
                    SetValueAsObject(current.ReturnValue, value);
                }
                else
                {
                    IJsonConverterInternal<TValue> converter = this as IJsonConverterInternal<TValue>;
                    if (converter != null)
                    {
                        TValue value = converter.FromJson(ref reader);
                        if (current.ReturnValue == null)
                        {
                            current.ReturnValue = value;
                        }
                        else
                        {
                            Set(current.ReturnValue, value);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                    }
                }
            }
        }

        protected internal override void FromJsonEnumerable(ref FromJsonObjectState current, ref Utf8JsonReader reader)
        {
            if (ValueConverter != null)
            {
                Type propertyType = PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }
                object value = ValueConverter.GetFromJson(ref reader, propertyType);
                FromJsonObjectState.SetReturnValue(ref current, value);
            }
            else
            {
                IJsonConverterInternal<TValue> converter = this as IJsonConverterInternal<TValue>;
                if (converter != null)
                {
                    TValue value = converter.FromJson(ref reader);
                    FromJsonObjectState.SetReturnValue(ref current, value);
                }
                else
                {
                    throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                }
            }
        }

        public override void ToJson(ref ToJsonObjectState current, ref Utf8JsonWriter writer)
        {
            if (current.Enumerator != null)
            {
                // Forward the setter to the value-based JsonPropertyInfo.
                JsonPropertyInfo propertyInfo = ElementClassInfo.GetPolicyProperty();
                propertyInfo.ToJsonEnumerable(ref current, ref writer);
            }
            else
            {
                if (ValueConverter != null)
                {
                    TValue value = Get(current.CurrentValue);
                    if (value == null)
                    {
                        if (SerializeNullValues)
                        {
                            writer.WriteNull(Name);
                        }
                    }
                    else
                    {
                        ValueConverter.SetToJson(ref writer, Name, value);
                    }
                }
                else
                {
                    IJsonConverterInternal<TValue> converter = this as IJsonConverterInternal<TValue>;
                    if (converter != null)
                    {
                        TValue value = Get(current.CurrentValue);
                        if (Name == null)
                        {
                            if (value == null)
                            {
                                if (SerializeNullValues)
                                {
                                    writer.WriteNullValue();
                                }
                            }
                            else
                            {
                                converter.ToJson(ref writer, value);
                            }
                        }
                        else
                        {
                            if (value == null)
                            {
                                if (SerializeNullValues)
                                {
                                    writer.WriteNull(Name);
                                }
                            }
                            else
                            {
                                converter.ToJson(ref writer, Name, value);
                            }
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                    }
                }
            }
        }

        protected internal override void ToJsonEnumerable(ref ToJsonObjectState current, ref Utf8JsonWriter writer)
        {
            if (ValueConverter != null)
            {
                Debug.Assert(current.Enumerator != null);

                object value = current.Enumerator.Current;
                if (value == null)
                {
                    if (SerializeNullValues)
                    {
                        writer.WriteNull(Name);
                    }
                }
                else
                {
                    ValueConverter.SetToJson(ref writer, Name, value);
                }
            }
            else
            {
                IJsonConverterInternal<TValue> converter = this as IJsonConverterInternal<TValue>;
                if (converter != null)
                {
                    Debug.Assert(current.Enumerator != null);
                    TValue value = (TValue)current.Enumerator.Current;
                    if (value == null)
                    {
                        if (SerializeNullValues)
                        {
                            writer.WriteNullValue();
                        }
                    }
                    else
                    {
                        converter.ToJson(ref writer, value);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                }
            }
        }

        public override PropertyValueConverterAttribute GetValueConverter()
        {
            return ValueConverter;
        }

        public override void GetPolicies()
        {
            // ValueConverter
            ValueConverter = GetPropertyValueConverter();

            base.GetPolicies();
        }

        protected PropertyValueConverterAttribute GetPropertyValueConverter()
        {
            PropertyValueConverterAttribute attr = DefaultConverters.GetPropertyValueConverter(ParentClassType, PropertyInfo, Settings, PropertyType);
            return attr;
        }
    }
}
