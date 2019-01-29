// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;

namespace System.Text.Json.Serialization.Policies
{
    internal static class DateConverter
    {
        public static bool TryGetFromJson(ReadOnlySpan<byte> span, Type type, out object value)
        {
            DateTime temp;
            bool success = Utf8Parser.TryParse(span, out temp, out int bytesConsumed, 'O') && span.Length == bytesConsumed;
            value = temp;
            return success;
        }

        public static bool TrySetToJson(object value, out Span<byte> span)
        {
            span = JsonConverter.s_utf8Encoding.GetBytes(((Enum)value).ToString("O"));
            return true;
        }
    }
}
