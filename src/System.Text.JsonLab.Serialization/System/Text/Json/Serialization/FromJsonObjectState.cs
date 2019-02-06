// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace System.Text.Json.Serialization
{
    internal struct FromJsonObjectState
    {
        // The object (POCO or IEnumerable) that is being populated
        public object ReturnValue;
        public JsonClassInfo ClassInfo;

        // Current property values
        public JsonPropertyInfo PropertyInfo;
        public bool PopStackOnEndArray;
        public bool EnumerableCreated;

        // Support System.Array and other types that don't implement IList
        public List<object> TempEnumerableValues;

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
            EnumerableCreated = false;
            TempEnumerableValues = null;
        }

        public bool IsEnumerable()
        {
            return ClassInfo.ClassType == ClassType.Enumerable;
        }

        public bool IsPropertyEnumerable()
        {
            if (PropertyInfo != null)
            {
                return PropertyInfo.ClassType == ClassType.Enumerable;
            }

            return false;
        }

        public Type GetElementType()
        {
            if (IsPropertyEnumerable())
            {
                return PropertyInfo.ElementClassInfo.Type;
            }

            if (IsEnumerable())
            {
                return ClassInfo.ElementClassInfo.Type;
            }

            return PropertyInfo.PropertyType;
        }

        public static object CreateEnumerableValue(ref FromJsonObjectState current, JsonConverterSettings settings)//, Type arrayType)
        {
            // If the property has an EnumerableConverter, then we use tempEnumerableValues.
            if (current.PropertyInfo.EnumerableConverter != null)
            {
                current.TempEnumerableValues = new List<object>();
                return null;
            }

            Type propType = current.PropertyInfo.PropertyType;
            if (typeof(IList).IsAssignableFrom(propType))
            {
                // If IList, add the members as we create them.
                JsonClassInfo collectionClassInfo = settings.GetOrAddClass(propType);
                IList collection = (IList)collectionClassInfo.CreateObject();
                return collection;
            }
            else
            {
                throw new InvalidOperationException($"todo: IEnumerable type {propType.ToString()} is not convertable.");
            }
        }

        public static IEnumerable GetEnumerableValue(in FromJsonObjectState current)
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
        }

        public static void SetReturnValue(ref FromJsonObjectState current, object value, bool setPropertyDirectly = false)
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
            else if (!setPropertyDirectly && current.IsPropertyEnumerable())
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
                current.PropertyInfo.SetValueAsObject(current.ReturnValue, value);
            }
        }
    }
}
