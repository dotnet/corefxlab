// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace System.Buffers.Testing
{
    public class BufferUtilities
    {
        public static ReadOnlySequence<byte> CreateSplitBuffer(byte[] buffer, int minSize, int maxSize)
        {
            if (buffer == null || buffer.Length == 0 || minSize <= 0 || maxSize <= 0 || minSize > maxSize)
            {
                throw new InvalidOperationException();
            }

            Random r = new Random(0xFEED);

            BufferSegment last = null;
            BufferSegment first = null;
            var ownedBuffer = new OwnedArray<byte>(buffer);

            int remaining = buffer.Length;
            int position = 0;
            while (remaining > 0)
            {
                int take = Math.Min(r.Next(minSize, maxSize), remaining);
                BufferSegment current = new BufferSegment();
                current.SetMemory(ownedBuffer, position, position + take);
                if (first == null)
                {
                    first = current;
                    last = current;
                }
                else
                {
                    last.SetNext(current);
                    last = current;
                }
                remaining -= take;
                position += take;
            }

            return new ReadOnlySequence<byte>(first, 0, last, last.Length);
        }

        public static ReadOnlySequence<byte> CreateBuffer(params byte[][] inputs)
        {
            if (inputs == null || inputs.Length == 0)
            {
                throw new InvalidOperationException();
            }

            BufferSegment last = null;
            BufferSegment first = null;

            for (int i = 0; i < inputs.Length; i++)
            {
                byte[] source = inputs[i];
                int length = source.Length;
                int dataOffset = length;

                // Shift the incoming data for testing
                byte[] chars = new byte[length * 8];
                for (int j = 0; j < length; j++)
                {
                    chars[dataOffset + j] = source[j];
                }

                // Create a segment that has offset relative to the OwnedMemory and OwnedMemory itself has offset relative to array
                var ownedBuffer = new OwnedArray<byte>(chars);
                var current = new BufferSegment();
                current.SetMemory(ownedBuffer, length, length * 2);
                if (first == null)
                {
                    first = current;
                    last = current;
                }
                else
                {
                    last.SetNext(current);
                    last = current;
                }
            }

            return new ReadOnlySequence<byte>(first, 0, last, last.Length);
        }

        public static ReadOnlySequence<byte> CreateBuffer(params string[] inputs)
        {
            var buffers = new byte[inputs.Length][];
            for (int i = 0; i < inputs.Length; i++)
            {
                buffers[i] = Encoding.UTF8.GetBytes(inputs[i]);
            }
            return CreateBuffer(buffers);
        }

        public static ReadOnlySequence<byte> CreateBuffer(params int[] inputs)
        {
            byte[][] buffers;
            if (inputs.Length == 0)
            {
                buffers = new[] { new byte[] { } };
            }
            else
            {
                buffers = new byte[inputs.Length][];
                for (int i = 0; i < inputs.Length; i++)
                {
                    buffers[i] = new byte[inputs[i]];
                }
            }
            return CreateBuffer(buffers);
        }
    }
}
