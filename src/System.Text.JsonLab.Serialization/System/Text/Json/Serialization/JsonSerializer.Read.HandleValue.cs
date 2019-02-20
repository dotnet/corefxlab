// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool HandleValue(JsonTokenType tokenType, JsonSerializerOptions options, ref Utf8JsonReader reader, ref ReadObjectState current)
        {
            if (current.PropertyInfo == null)
                return false;
            
            bool lastCall = (!current.IsEnumerable() && !current.IsPropertyEnumerable() && current.ReturnValue == null);
            current.PropertyInfo.Read(tokenType, options, ref current, ref reader);
            return lastCall;
        }
    }
}
