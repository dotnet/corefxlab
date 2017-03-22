// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Text;

namespace System.IO.Pipelines.Testing
{
    public class BufferUtilities
    {
        public static ReadableBuffer CreateBuffer(params byte[][] inputs)
        {
            if (inputs == null || inputs.Length == 0)
            {
                throw new InvalidOperationException();
            }

            var i = 0;

            BufferSegment last = null;
            BufferSegment first = null;

            do
            {
                var s = inputs[i];
                var length = s.Length;
                var memoryOffset = length;
                var dataOffset = length * 2;
                var chars = new byte[length * 8];

                for (int j = 0; j < length; j++)
                {
                    chars[dataOffset + j] = s[j];
                }

                // Create a segment that has offset relative to the OwnedBuffer and OwnedBuffer itself has offset relative to array
                var ownedBuffer = new SegmentBuffer(new ArraySegment<byte>(chars, memoryOffset, length * 3));
                var current = new BufferSegment(ownedBuffer, length, length * 2);
                if (first == null)
                {
                    first = current;
                    last = current;
                }
                else
                {
                    last.Next = current;
                    last = current;
                }
                i++;
            } while (i < inputs.Length);

            return new ReadableBuffer(new ReadCursor(first, first.Start), new ReadCursor(last, last.Start + last.ReadableBytes));
        }

        public static ReadableBuffer CreateBuffer(params string[] inputs)
        {
            var buffers = new byte[inputs.Length][];
            for (int i = 0; i < inputs.Length; i++)
            {
                buffers[i] = Encoding.UTF8.GetBytes(inputs[i]);
            }
            return CreateBuffer(buffers);
        }

        public static ReadableBuffer CreateBuffer(params int[] inputs)
        {
            var buffers = new byte[inputs.Length][];
            for (int i = 0; i < inputs.Length; i++)
            {
                buffers[i] = new byte[inputs[i]];
            }
            return CreateBuffer(buffers);
        }

        class SegmentBuffer : OwnedBuffer<byte>
        {
            private ArraySegment<byte> _segment;

            public SegmentBuffer(ArraySegment<byte> arraySegment)
            {
                _segment = arraySegment;
            }

            public override int Length => _segment.Count;

            public override Span<byte> Span => new Span<byte>(_segment.Array, _segment.Offset, _segment.Count);

            public override Span<byte> GetSpan(int index, int length)
            {
                if (IsDisposed) ThrowObjectDisposed();
                return Span.Slice(index, length);
            }

            protected override bool TryGetArrayInternal(out ArraySegment<byte> buffer)
            {
                buffer = _segment;
                return true;
            }

            protected override unsafe bool TryGetPointerInternal(out void* pointer)
            {
                pointer = null;
                return false;
            }
        }
    }
}