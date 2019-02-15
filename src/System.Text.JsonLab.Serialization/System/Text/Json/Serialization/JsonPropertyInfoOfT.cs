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

        public JsonPropertyInfo(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options, Type elementType = null) :
            base(classType, propertyType, propertyInfo, elementType, options)
        {
            if (propertyInfo != null)
            {
                Get = options.ClassMaterializerStrategy.CreateGetter<TValue>(propertyInfo);
                Set = options.ClassMaterializerStrategy.CreateSetter<TValue>(propertyInfo);
            }
            else
            {
                // Used when current.obj contains the return value and this is the property policy.
                Get = delegate (object obj)
                {
                    return (TValue)obj;
                };
            }

            GetPolicies(options);
        }

        public override object GetValueAsObject(object obj, JsonSerializerOptions options)
        {
            Debug.Assert(Get != null);
            return Get(obj);
        }

        public override void SetValueAsObject(object obj, object value, JsonSerializerOptions options)
        {
            Debug.Assert(Set != null);
            TValue typedValue = (TValue)value;

            if (typedValue != null || !SkipNullValuesOnWrite(options))
            {
                Set(obj, (TValue)value);
            }
        }

        public override void Read(JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader)
        {
            if (ElementClassInfo != null)
            {
                // Forward the setter to the value-based JsonPropertyInfo.
                JsonPropertyInfo propertyInfo = ElementClassInfo.GetPolicyProperty();
                propertyInfo.ReadEnumerable(options, ref current, ref reader);
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

                    object value = ValueConverter.GetRead(ref reader, propertyType);
                    if (value != null || !SkipNullValuesOnRead(options))
                    {
                        SetValueAsObject(current.ReturnValue, value, options);
                    }
                }
                else
                {
                    if (this is IJsonSerializerInternal<TValue> converter)
                    {
                        TValue value = converter.Read(ref reader);
                        if (current.ReturnValue == null)
                        {
                            current.ReturnValue = value;
                        }
                        else
                        {
                            if (value != null || !SkipNullValuesOnRead(options))
                            {
                                Set(current.ReturnValue, value);
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

        protected internal override void ReadEnumerable(JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader)
        {
            if (ValueConverter != null)
            {
                Type propertyType = PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }
                object value = ValueConverter.GetRead(ref reader, propertyType);
                ReadObjectState.SetReturnValue(value, options, ref current);
            }
            else
            {
                if (this is IJsonSerializerInternal<TValue> converter)
                {
                    TValue value = converter.Read(ref reader);
                    ReadObjectState.SetReturnValue(value, options, ref current);
                }
                else
                {
                    throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                }
            }
        }

        // todo: have the caller check if current.Enumerator != null and call WriteEnumerable of the underlying property directly to avoid an extra virtual call.
        public override void Write(JsonSerializerOptions options, ref WriteObjectState current, ref Utf8JsonWriter writer)
        {
            if (current.Enumerator != null)
            {
                // Forward the setter to the value-based JsonPropertyInfo.
                JsonPropertyInfo propertyInfo = ElementClassInfo.GetPolicyProperty();
                propertyInfo.WriteEnumerable(options, ref current, ref writer);
            }
            else
            {
                if (ValueConverter != null)
                {
                    TValue value = Get(current.CurrentValue);
                    if (value == null)
                    {
                        if (Name == null)
                        {
                            writer.WriteNullValue();
                        }
                        else if (!SkipNullValuesOnWrite(options))
                        {
                            writer.WriteNull(Name);
                        }
                    }
                    else
                    {
                        ValueConverter.SetWrite(ref writer, Name, value);
                    }
                }
                else
                {
                    TValue value = Get(current.CurrentValue);

                    if (value == null)
                    {
                        if (Name == null)
                        {
                            writer.WriteNullValue();
                        }
                        else if (!SkipNullValuesOnWrite(options))
                        {
                            writer.WriteNull(Name);
                        }
                    }
                    else
                    {
                        if (this is IJsonSerializerInternal<TValue> converter)
                        {
                            if (Name == null)
                            {
                                converter.Write(ref writer, value);
                            }
                            else if (!SkipNullValuesOnWrite(options))
                            {
                                converter.Write(ref writer, Name, value);
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                        }
                    }
                }
            }
        }

        protected internal override void WriteEnumerable(JsonSerializerOptions options, ref WriteObjectState current, ref Utf8JsonWriter writer)
        {
            if (ValueConverter != null)
            {
                Debug.Assert(current.Enumerator != null);

                object value = current.Enumerator.Current;
                if (value == null)
                {
                    writer.WriteNull(Name);
                }
                else
                {
                    ValueConverter.SetWrite(ref writer, Name, value);
                }
            }
            else
            {
                if (this is IJsonSerializerInternal<TValue> converter)
                {
                    Debug.Assert(current.Enumerator != null);
                    TValue value = (TValue)current.Enumerator.Current;
                    if (value == null)
                    {
                        writer.WriteNullValue();
                    }
                    else
                    {
                        converter.Write(ref writer, value);
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

        public override void GetPolicies(JsonSerializerOptions options)
        {
            // ValueConverter
            ValueConverter = GetPropertyValueConverter(options);

            base.GetPolicies(options);
        }

        protected PropertyValueConverterAttribute GetPropertyValueConverter(JsonSerializerOptions options)
        {
            PropertyValueConverterAttribute attr = DefaultConverters.GetPropertyValueConverter(ParentClassType, PropertyInfo, PropertyType, options);
            return attr;
        }
    }
}
