// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Text.JsonLab
{
    [Flags]
    public enum JsonReaderOptions
    {
        Default = 0b0000,       // Don't allow comments, treat as invalid json if found
        AllowComments = 0b0001, // Allow comments but don't skip them
        SkipComments = 0b0011   // Allow and Skip comments
    }
}
