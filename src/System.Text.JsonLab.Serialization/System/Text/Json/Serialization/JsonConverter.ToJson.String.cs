// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization
{
    public static partial class JsonConverter
    {
        public static string ToJsonString(object value, JsonConverterSettings settings = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Span<byte> jsonBytes = ToJsonInternal(value, settings);
            string stringJson = JsonReaderHelper.TranscodeHelper(jsonBytes);
            return stringJson;
        }
    }
}
