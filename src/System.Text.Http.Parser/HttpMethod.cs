// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Http.Parser
{
    public static partial class Http
    {
        public enum Method : byte
        {
            Get,
            Put,
            Delete,
            Post,
            Head,
            Trace,
            Patch,
            Connect,
            Options,

            Custom,
        }
    }
}