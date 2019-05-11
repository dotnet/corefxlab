// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.JsonLab.Serialization
{
    public static partial class JsonSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleNull(ref ReadObjectState current, JsonSerializerOptions options)
        {
            Debug.Assert(current.PropertyInfo != null);

            JsonPropertyInfo propertyInfo = current.PropertyInfo;
            if (!propertyInfo.CanBeNull)
            {
                throw new InvalidOperationException($"todo: {propertyInfo.PropertyType} can't be null");
            }

            if (current.IsEnumerable() || current.IsPropertyEnumerable())
            {
                ReadObjectState.SetReturnValue(null, options, ref current);
                return false;
            }

            if (current.ReturnValue == null)
            {
                return true;
            }

            if (!propertyInfo.IgnoreNullPropertyValueOnRead(options))
            {
                current.PropertyInfo.SetValueAsObject(current.ReturnValue, null, options);
            }

            return false;
        }
    }
}
