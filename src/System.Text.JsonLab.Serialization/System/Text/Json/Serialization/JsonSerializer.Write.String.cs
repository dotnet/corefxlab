// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json.Serialization
{
    public static partial class JsonSerializer
    {
        public static string WriteString<TValue>(TValue value, JsonSerializerOptions options = null)
        {
            return WriteStringInternal(value, typeof(TValue), options);
        }

        public static string WriteString(object value, Type type, JsonSerializerOptions options = null)
        {
            VerifyValueAndType(value, type);

            return WriteStringInternal(value, type, options);
        }

        private static string WriteStringInternal(object value, Type type, JsonSerializerOptions options)
        {
            Span<byte> jsonBytes = WriteInternal(value, type, options);
            string stringJson = JsonReaderHelper.TranscodeHelper(jsonBytes);
            return stringJson;
        }
    }
}
