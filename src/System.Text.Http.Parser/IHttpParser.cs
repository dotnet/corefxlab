// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO.Pipelines;

namespace System.Text.Http.Parser
{
    public interface IHttpParser
    {
        bool ParseRequestLine<T>(T handler, in ReadableBuffer buffer, out ReadCursor consumed, out ReadCursor examined) where T : IHttpRequestLineHandler;

        bool ParseHeaders<T>(T handler, in ReadableBuffer buffer, out ReadCursor consumed, out ReadCursor examined, out int consumedBytes) where T : IHttpHeadersHandler;

        void Reset();
    }
}
