// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    public struct JsonReaderOptions
    {
        public JsonReaderOptions(CommentHandling commentHandling)
        {
            _commentHandling = commentHandling;
        }

        internal CommentHandling _commentHandling;

        public enum CommentHandling
        {
            Default = 0,       // Don't allow comments, treat as invalid json if found
            AllowComments = 1, // Allow comments but don't skip them
            SkipComments = 2,  // Allow and Skip comments
        }
    }
}
