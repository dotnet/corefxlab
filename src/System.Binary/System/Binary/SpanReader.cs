// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    // Experiments related to https://github.com/dotnet/corefx/issues/24180
    public ref struct SpanReader
    {
        public SpanReader(ReadOnlySpan<byte> span)
        {
            throw new NotImplementedException();
        }

        public int Length { get; }
        public int Position { get; set; }
        public ReadOnlySpan<byte> Remaining { get; }

        public bool TryReadInt32LittleEndian(out int result)
        {
            throw new NotImplementedException();
        }

        public bool TryReadInt32BigEndian(out int result)
        {
            throw new NotImplementedException();
        }

        public bool TryReadInt64LittleEndian(out long result)
        {
            throw new NotImplementedException();
        }

        public bool TryReadInt64BigEndian(out long result)
        {
            throw new NotImplementedException();
        }

        // TODO: Add APIs similar to above for each primitive type we care about
    }
}
