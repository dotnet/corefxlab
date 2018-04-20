// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json
{
    public struct Utf8JsonString
    {
        public Utf8StringSegment DangerousGetRawUnvalidatedValue() => throw null;

        public bool TryGetValue(out Utf8StringSegment value) => throw null;
    }
}
