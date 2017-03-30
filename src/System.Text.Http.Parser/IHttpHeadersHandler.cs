// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace System.Text.Http.Parser
{
    public interface IHttpHeadersHandler
    {
        void OnHeader(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value);
    }
}