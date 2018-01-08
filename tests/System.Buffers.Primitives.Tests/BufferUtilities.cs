// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Collections.Sequences;
using System.Text;

namespace System.IO.Pipelines.Testing
{
    public class BufferUtilities
    {
        public static ReadOnlyBuffer CreateBuffer(params byte[][] inputs)
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
                var dataOffset = length;
                var chars = new byte[length * 8];

                for (int j = 0; j < length; j++)
                {
                    chars[dataOffset + j] = s[j];
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
                i++;
            } while (i < inputs.Length);

            return new ReadOnlyBuffer(first, 0, last, last.Length);
        }

        public static ReadOnlyBuffer CreateBuffer(params string[] inputs)
        {
            var buffers = new byte[inputs.Length][];
            for (int i = 0; i < inputs.Length; i++)
            {
                buffers[i] = Encoding.UTF8.GetBytes(inputs[i]);
            }
            return CreateBuffer(buffers);
        }

        public static ReadOnlyBuffer CreateBuffer(params int[] inputs)
        {
            byte[][] buffers;
            if (inputs.Length == 0)
            {
                buffers = new[] { new byte[] {}};
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
