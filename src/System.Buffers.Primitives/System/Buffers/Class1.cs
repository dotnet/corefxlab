// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace System.Buffers
{
    public static class ReadOnlyBufferReaderExtensions
    {
        public static ReadOnlyBuffer SliceBefore(this BufferReader<ReadOnlyBuffer> reader)
        {
            return reader.Sequence.Slice(reader.Sequence.Start, reader.Position);
        }

        public static ReadOnlyBuffer SliceAfter(this BufferReader<ReadOnlyBuffer> reader)
        {
            return reader.Sequence.Slice(reader.Position);
        }
    }
}
