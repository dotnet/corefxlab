// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers.Text;

namespace System.Text.Json.Serialization.Policies
{
    internal class DateConverter : IUtf8ValueConverter<DateTime>
    {
        public DateConverter() { }

        public bool TryGetFromJson(ReadOnlySpan<byte> span, Type type, out DateTime value)
        {
            return Utf8Parser.TryParse(span, out value, out int bytesConsumed, 'O') && span.Length == bytesConsumed;
        }

        public bool TrySetToJson(DateTime value, out Span<byte> span)
        {
            span = Encoding.UTF8.GetBytes(value.ToString("O"));
            return true;
        }
    }
}
