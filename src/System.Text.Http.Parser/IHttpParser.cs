// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Sequences;

namespace System.Text.Http.Parser
{
    public interface IHttpParser
    {
        bool ParseRequestLine<T>(T handler, in ReadOnlyBuffer<byte> buffer, out SequenceIndex consumed, out SequenceIndex examined) where T : IHttpRequestLineHandler;

        bool ParseHeaders<T>(T handler, in ReadOnlyBuffer<byte> buffer, out SequenceIndex consumed, out SequenceIndex examined, out int consumedBytes) where T : IHttpHeadersHandler;

        void Reset();
    }
}
