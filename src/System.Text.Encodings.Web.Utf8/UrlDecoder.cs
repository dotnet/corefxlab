// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Encodings.Web.Utf8
{
    public abstract class UrlDecoder
    {
        public static Utf8UriDecoder Utf8 { get; } = new Utf8UriDecoder();
    }
}
