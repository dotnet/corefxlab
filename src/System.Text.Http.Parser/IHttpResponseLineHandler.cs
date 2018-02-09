// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Text.Http.Parser
{
    public interface IHttpResponseLineHandler
    {
        void OnStatusLine(Http.Version version, ushort status, ReadOnlySpan<byte> reason);
    }
}
