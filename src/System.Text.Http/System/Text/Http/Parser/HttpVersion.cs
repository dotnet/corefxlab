// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Http.Parser
{
    public static partial class Http
    {
        // TODO: this should be renamed to HttpVersion (or something like that). "Version" conflicts with type in System namespace
        public enum Version
        {
            Unknown = -1,
            Http10 = 0,
            Http11 = 1,
            Http20 = 2,
        }
    }
}
