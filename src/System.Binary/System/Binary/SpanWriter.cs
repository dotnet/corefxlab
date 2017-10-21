// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    // Experiments related to https://github.com/dotnet/corefx/issues/24180
    public ref struct SpanWriter
    {
        public SpanWriter(Span<byte> span)
        {
            throw new NotImplementedException();
        }

        public int Length { get; }
        public int Position { get; set; }
        public Span<byte> Remaining { get; }

        public bool TryWriteInt32LittleEndian(int result)
        {
            throw new NotImplementedException();
        }

        public bool TryWriteInt32BigEndian(int result)
        {
            throw new NotImplementedException();
        }

        public bool TryWriteInt64LittleEndian(long result)
        {
            throw new NotImplementedException();
        }

        public bool TryWriteInt64BigEndian(long result)
        {
            throw new NotImplementedException();
        }

        // TODO: Add APIs similar to above for each primitive type we care about
    }
}
