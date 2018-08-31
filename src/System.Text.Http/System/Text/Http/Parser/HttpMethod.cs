// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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