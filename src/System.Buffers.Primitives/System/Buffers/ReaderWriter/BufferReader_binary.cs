// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers.Binary;
using System.Collections.Sequences;

namespace System.Buffers
{
    public ref partial struct BufferReader<TSequence> where TSequence : ISequence<ReadOnlyMemory<byte>>
    {
        public bool TryRead(out int value, bool littleEndian = false)
        {
            var unread = UnreadSegment;
            if (littleEndian)
            {
                if (BinaryPrimitives.TryReadInt32LittleEndian(unread, out value))
                {
                    Skip(sizeof(int));
                    return true;
                }
            }
            else if (BinaryPrimitives.TryReadInt32BigEndian(unread, out value))
            {
                Skip(sizeof(int));
                return true;
            }

            return TryReadStraddling(out value, littleEndian);
        }

        private bool TryReadStraddling(out int value, bool littleEndian)
        {
            Span<byte> span = stackalloc byte[4];
            var copied = CopyUnread(this, span);
            if (copied < 4)
            {
                value = default;
                return false;
            }

            if (littleEndian)
            {
                value = BinaryPrimitives.ReadInt32LittleEndian(span);
            }
            else
            {
                value = BinaryPrimitives.ReadInt32BigEndian(span);
            }
            Skip(sizeof(int));
            return true;
        }
    }
}
