// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Text.Json.Serialization
{
    internal struct JsonObjectState
    {
        // The object (POCO or IEnumerable) that is being populated
        public object ReturnValue;
        public JsonClassInfo ClassInfo;

        // Current property values
        public JsonPropertyInfo PropertyInfo;
        public bool PopStackOnEndArray;

        // Support System.Array and other types that don't implement IList
        public List<object> TempEnumerableValues;

        // Cached values used to determine if the current property value or current return value is enumerable.
        private bool _isEnumerable;
        private bool _isPropertyEnumerable;

        // For performance, we order the properties by the first usage and this index helps find the right slot quicker.
        public int PropertyIndex;

        public void Reset()
        {
            ReturnValue = null;
            ClassInfo = null;
            PropertyIndex = 0;
            ResetProperty();
        }

        public void ResetProperty()
        {
            PropertyInfo = null;
            PopStackOnEndArray = false;
            TempEnumerableValues = null;
            _isEnumerable = false;
            _isPropertyEnumerable = false;
        }

        public bool IsEnumerable()
        {
            if (ReturnValue != null)
            {
                return _isEnumerable;
            }

            return (TempEnumerableValues != null);
        }

        public bool IsPropertyEnumerable()
        {
            if (PropertyInfo != null && ReturnValue != null)
            {
                if (TempEnumerableValues != null)
                {
                    return true;
                }

                return _isPropertyEnumerable;
            }

            return false;
        }

        public static Type GetElementType(in JsonObjectState current)
        {
            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                return current.PropertyInfo.ElementType;
            }

            return current.PropertyInfo.PropertyType;
        }

        public static object CreateEnumerableValue(ref JsonObjectState current, Type propType, JsonConverterSettings settings, Type arrayType)
        {
            // If the property has an EnumerableConverter, then we use tempEnumerableValues
            if (current.PropertyInfo.EnumerableConverter != null)
            {
                current.TempEnumerableValues = new List<object>();
                return null;
            }

            if (typeof(IList).IsAssignableFrom(propType))
            {
                // If IList, add the members as we create them.
                JsonClassInfo collectionClassInfo = settings.GetOrAddClass(current.PropertyInfo.PropertyType);
                IList collection = (IList)collectionClassInfo.CreateObject();
                return collection;
            }
            else
            {
                throw new InvalidOperationException($"todo: IEnumerable type {arrayType.ToString()} is not convertable.");
            }
        }

        public static IEnumerable GetEnumerableValue(in JsonObjectState current)
        {
            if (current.IsEnumerable())
            {
                if (current.ReturnValue != null)
                {
                    return (IEnumerable)current.ReturnValue;
                }
            }

            // IEnumerable properties are finished (values added inline) unless they are using tempEnumerableValues.
            return current.TempEnumerableValues;
        }

        public void SetReturnValue(object value, bool isValueEnumerable = false)
        {
            Debug.Assert(ReturnValue == null);
            ReturnValue = value;
            _isEnumerable = isValueEnumerable;
        }

        public static void SetReturnValue(ref JsonObjectState current, object value, bool isPropertyEnumerable, bool ignorePropertyEnumerable = false)
        {
            if (current.IsEnumerable())
            {
                if (current.TempEnumerableValues != null)
                {
                    current.TempEnumerableValues.Add(value);
                }
                else
                {
                    ((IList)current.ReturnValue).Add(value);
                }
            }
            else if (!ignorePropertyEnumerable && current.IsPropertyEnumerable())
            {
                Debug.Assert(current.PropertyInfo != null);
                Debug.Assert(current.ReturnValue != null);
                if (current.TempEnumerableValues != null)
                {
                    current.TempEnumerableValues.Add(value);
                }
                else
                {
                    ((IList)current.PropertyInfo.GetValueAsObject(current.ReturnValue)).Add(value);
                }
            }
            else
            {
                Debug.Assert(current.PropertyInfo != null);
                Debug.Assert(
                    (current.TempEnumerableValues == null && !ignorePropertyEnumerable) || 
                    (current.TempEnumerableValues != null && ignorePropertyEnumerable));
                current.PropertyInfo.SetValueAsObject(current.ReturnValue, value);
                current._isPropertyEnumerable = isPropertyEnumerable;
            }
        }
    }
}
