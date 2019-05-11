// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace System.Text.JsonLab.Serialization
{
    public static partial class JsonSerializer
    {
        private static object ReadCore(
            Utf8JsonReader reader,
            Type returnType,
            JsonSerializerOptions options)
        {
            if (options == null)
                options = s_defaultSettings;

            List<ReadObjectState> previous = null;
            int arrayIndex = 0;

            ReadObjectState current = default;
            JsonClassInfo classInfo = options.GetOrAddClass(returnType);
            current.ClassInfo = classInfo;
            if (classInfo.ClassType != ClassType.Object)
            {
                current.PropertyInfo = classInfo.GetPolicyProperty();
            }

            ReadCore(options, ref reader, ref current, ref previous, ref arrayIndex);

            return current.ReturnValue;
        }
    }
}
