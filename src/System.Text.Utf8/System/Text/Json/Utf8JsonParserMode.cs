// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.Json
{
    public enum Utf8JsonParserMode
    {
        /// <summary>
        /// Default setting. Requires the input to have strict conformance with RFC 8259.
        /// </summary>
        /// <remarks>
        /// The <see cref="Utf8JsonReader"/> class by itself simply reads tokens from an input
        /// but does not perform validation across tokens. For example, the token reader might
        /// return <see cref="JsonTokenType.ObjectStart"/> immeditately followed by <see cref="JsonTokenType.NullLiteral"/>
        /// if the input reads "{ null ...". It is up to the caller to ensure that the tokens
        /// are valid with respect to each other.
        /// </remarks>
        Rfc8259,

        /// <summary>
        /// Similar to <see cref="Rfc8259"/>, but also allows comments in the input.
        /// Comments are exposed to the caller as <see cref="JsonTokenType.Comment"/> tokens.
        /// </summary>
        Rfc8259ExposeComments,

        /// <summary>
        /// Similar to <see cref="Rfc8259ExposeComments"/>, but silently skips comments
        /// in the input without exposing <see cref="JsonTokenType.Comment"/> tokens to the caller.
        /// </summary>
        Rfc8259SkipComments,
    }
}
