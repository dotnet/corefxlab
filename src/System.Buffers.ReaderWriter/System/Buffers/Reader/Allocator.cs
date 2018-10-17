// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    /// <summary>
    /// Allocate a new Span<typeparamref name="T"/> of the given length.
    /// </summary>
    public delegate Span<T> Allocator<T>(int length);
}
