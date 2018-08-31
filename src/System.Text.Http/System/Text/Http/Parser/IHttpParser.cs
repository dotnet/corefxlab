// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;

namespace System.Text.Http.Parser
{
    public interface IHttpParser
    {
        bool ParseRequestLine<T>(T handler, in ReadOnlySequence<byte> buffer, out SequencePosition consumed, out SequencePosition examined) where T : IHttpRequestLineHandler;

        bool ParseHeaders<T>(T handler, in ReadOnlySequence<byte> buffer, out SequencePosition consumed, out SequencePosition examined, out int consumedBytes) where T : IHttpHeadersHandler;

        void Reset();
    }
}
