// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
