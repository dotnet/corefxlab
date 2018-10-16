// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    // Choosing the relevant subset of JsonTokenType since we can only support 8 (2 ^ 3)
    // Keep the enum order in sync with JsonTokenType since we cast between the two
    internal enum JsonType
    {
        StartObject,
        StartArray,
        String,
        Number,
        True,
        False,
        Null,
    }
}
