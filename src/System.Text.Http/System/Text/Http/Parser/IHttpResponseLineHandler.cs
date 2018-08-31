// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Http.Parser
{
    public interface IHttpResponseLineHandler
    {
        void OnStatusLine(Http.Version version, ushort status, ReadOnlySpan<byte> reason);
    }
}
