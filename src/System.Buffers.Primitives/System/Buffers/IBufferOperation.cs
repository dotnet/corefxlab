// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers.Operations
{
    public interface IBufferOperation
    {
        OperationStatus Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written);
    }
}
