// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleNull(ref FromJsonObjectState current)
        {
            Debug.Assert(current.PropertyInfo != null);

            JsonPropertyInfo propertyInfo = current.PropertyInfo;
            if (!propertyInfo.CanBeNull)
            {
                throw new InvalidOperationException($"todo: {propertyInfo.PropertyType} can't be null");
            }

            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                if (propertyInfo.DeserializeNullValues)
                {
                    FromJsonObjectState.SetReturnValue(ref current, null);
                }
                return false;
            }

            if (current.ReturnValue == null)
            {
                return true;
            }

            if (propertyInfo.DeserializeNullValues)
            {
                current.PropertyInfo.SetValueAsObject(current.ReturnValue, null);
            }

            return false;
        }
    }
}
