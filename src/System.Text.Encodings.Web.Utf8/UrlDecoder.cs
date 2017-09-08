// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Text.Encodings.Web.Internal;

namespace System.Text.Encodings.Web.Utf8
{
    public abstract class UrlDecoder
    {
        public static BufferDecoder Utf8 { get; } = new Utf8UriDecoder();
    }
}
