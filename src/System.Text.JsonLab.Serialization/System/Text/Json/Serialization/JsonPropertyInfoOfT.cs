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
        public JsonValueConverter<TValue> ValueConverter { get; private set; }

        public bool HasGetter { get; private set; }
        public bool HasSetter { get; private set; }

        public GetterDelegate Get { get; private set; }
        public SetterDelegate Set { get; private set; }

        public JsonPropertyInfo(Type classType, Type propertyType, PropertyInfo propertyInfo, JsonSerializerOptions options, Type elementType = null) :
            base(classType, propertyType, propertyInfo, elementType, options)
        {
            if (propertyInfo != null)
            {
                if (propertyInfo.GetMethod?.IsPublic == true)
                {
                    HasGetter = true;
                    Get = options.ClassMaterializerStrategy.CreateGetter<TValue>(propertyInfo);
                }

                if (propertyInfo.SetMethod?.IsPublic == true)
                {
                    HasSetter = true;
                    Set = options.ClassMaterializerStrategy.CreateSetter<TValue>(propertyInfo);
                }
            }
            else
            {
                HasGetter = true;
                HasSetter = true;

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

            if (typedValue != null || !IgnoreNullPropertyValueOnWrite(options))
            {
                Set(obj, (TValue)value);
            }
        }

        public override void Read(JsonTokenType tokenType, JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader)
        {
            if (ElementClassInfo != null)
            {
                // Forward the setter to the value-based JsonPropertyInfo.
                JsonPropertyInfo propertyInfo = ElementClassInfo.GetPolicyProperty();
                propertyInfo.ReadEnumerable(tokenType, options, ref current, ref reader);
            }
            else if (HasSetter)
            {
                if (ValueConverter != null)
                {
                    Type propertyType = PropertyType;
                    if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propertyType = Nullable.GetUnderlyingType(propertyType);
                    }

                    if (!ValueConverter.TryRead(propertyType, ref reader, out TValue value))
                    {
                        throw new JsonReaderException("todo: unable to read value (propertypath)", 0,0);
                    }

                    if (value != null || !IgnoreNullPropertyValueOnRead(options))
                    {
                        SetValueAsObject(current.ReturnValue, value, options);
                    }
                }
                else
                {
                    if (this is IJsonValueConverter<TValue> converter)
                    {
                        if (!converter.TryRead(PropertyType, ref reader, out TValue value))
                        {
                            throw new JsonReaderException("todo: unable to read value (propertypath)", 0,0);
                        }

                        if (current.ReturnValue == null)
                        {
                            current.ReturnValue = value;
                        }
                        else
                        {
                            if (value != null || !IgnoreNullPropertyValueOnRead(options))
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

        protected internal override void ReadEnumerable(JsonTokenType tokenType, JsonSerializerOptions options, ref ReadObjectState current, ref Utf8JsonReader reader)
        {
            if (ValueConverter != null)
            {
                Type propertyType = PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = Nullable.GetUnderlyingType(propertyType);
                }

                if (!ValueConverter.TryRead(propertyType, ref reader, out TValue value))
                {
                    throw new JsonReaderException("todo: unable to read value (propertypath)", 0,0);
                }

                ReadObjectState.SetReturnValue(value, options, ref current);
            }
            else
            {
                if (this is IJsonValueConverter<TValue> converter)
                {
                    if (!converter.TryRead(PropertyType, ref reader, out TValue value))
                    {
                        throw new JsonReaderException("todo: unable to read value (propertypath)", 0,0);
                    }

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
            else if (HasGetter)
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
                        else if (!IgnoreNullPropertyValueOnWrite(options))
                        {
                            writer.WriteNull(Name);
                        }
                    }
                    else
                    {
                        if (Name != null)
                        {
                            if (HasEscapedName)
                            {
                                ValueConverter.Write(EscapedName, value, ref writer);
                            }
                            else
                            {
                                ValueConverter.Write(Name, value, ref writer);
                            }
                        }
                        else
                        {
                            ValueConverter.Write(value, ref writer);
                        }
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
                        else if (!IgnoreNullPropertyValueOnWrite(options))
                        {
                            if (HasEscapedName)
                            {
                                writer.WriteNull(EscapedName);
                            }
                            else
                            {
                                writer.WriteNull(Name);
                            }
                        }
                    }
                    else
                    {
                        if (this is IJsonValueConverter<TValue> converter)
                        {
                            if (Name != null)
                            {
                                if (HasEscapedName)
                                {
                                    converter.Write(EscapedName, value, ref writer);
                                }
                                else
                                {
                                    converter.Write(Name, value, ref writer);
                                }
                            }
                            else
                            {
                                converter.Write(value, ref writer);
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
                    if (Name != null)
                    {
                        if (HasEscapedName)
                        {
                            ValueConverter.Write(EscapedName, (TValue)value, ref writer);
                        }
                        else
                        {
                            ValueConverter.Write(Name, (TValue)value, ref writer);
                        }
                    }
                    else
                    {
                        ValueConverter.Write((TValue)value, ref writer);
                    }
                }
            }
            else
            {
                if (this is IJsonValueConverter<TValue> converter)
                {
                    Debug.Assert(current.Enumerator != null);
                    TValue value = (TValue)current.Enumerator.Current;
                    if (value == null)
                    {
                        writer.WriteNullValue();
                    }
                    else
                    {
                        converter.Write(value, ref writer);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"todo: there is no converter for {PropertyType}");
                }
            }
        }

        //public override PropertyValueConverter<TValue> GetValueConverter()
        //{
        //    return ValueConverter;
        //}

        public override void GetPolicies(JsonSerializerOptions options)
        {
            // ValueConverter
            ValueConverter = GetPropertyValueConverter(options);

            base.GetPolicies(options);
        }

        protected JsonValueConverter<TValue> GetPropertyValueConverter(JsonSerializerOptions options)
        {
            JsonValueConverterAttribute attr = DefaultConverters.GetPropertyValueConverter(ParentClassType, PropertyInfo, PropertyType, options);
            if (attr != null)
            {
                return attr.GetConverter<TValue>();
            }

            return null;
        }
    }
}
