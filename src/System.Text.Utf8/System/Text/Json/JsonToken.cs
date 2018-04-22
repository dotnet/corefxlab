// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json
{
    public struct JsonToken
    {
        public Utf8StringSegment DangerousGetRawUnvalidatedValue() => throw null;

        public bool TryGetValueAsDouble(out double value) => throw null;

        public bool TryGetValueAsInt32(out int value) => throw null;

        public bool TryGetValueAsInt64(out long value) => throw null;

        public bool TryGetValueAsString(out Utf8StringSegment value) => throw null;

        public bool TryGetValueAsUInt32(out uint value) => throw null;

        public bool TryGetValueAsUInt64(out ulong value) => throw null;
    }
}
