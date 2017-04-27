// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


namespace System.Text.Http.Parser
{
    public interface IHttpRequestLineHandler
    {
        void OnStartLine(Http.Method method, Http.Version version, ReadOnlySpan<byte> target, ReadOnlySpan<byte> path, ReadOnlySpan<byte> query, ReadOnlySpan<byte> customMethod, bool pathEncoded);
    }
}