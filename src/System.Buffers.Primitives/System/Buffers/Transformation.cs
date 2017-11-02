// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    public interface IBufferOperation
    {
        OperationStatus Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written);
    }
    public interface IBufferTransformation : IBufferOperation
    {
        OperationStatus Transform(Span<byte> buffer, int dataLength, out int written);
    }
}
