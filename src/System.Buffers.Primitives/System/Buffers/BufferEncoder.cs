// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

namespace System.Buffers
{
    public abstract class BufferEncoder : IBufferOperation
    {
        public abstract OperationStatus Encode(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written);

        public virtual OperationStatus Transform(Span<byte> buffer, int inputLength, out int written)
        {
            throw new NotSupportedException();
        }

        public virtual bool IsEncodeInPlaceSupported { get; } = false;

        OperationStatus IBufferOperation.Execute(ReadOnlySpan<byte> input, Span<byte> output, out int consumed, out int written) =>
            Encode(input, output, out consumed, out written);

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToString()
        {
            return base.ToString();
        }
    }
}

