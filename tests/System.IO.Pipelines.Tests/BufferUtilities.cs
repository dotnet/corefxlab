using System.Buffers;
using System.Linq;
using System.Text;

namespace System.IO.Pipelines.Tests
{
    public class BufferUtilities
    {
        public static ReadableBuffer CreateBuffer(params byte[][] inputs)
        {
            if (inputs == null || !inputs.Any())
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

                // Create a segment that has offset relative to the OwnedMemory and OwnedMemory itself has offset relative to array
                var ownedMemory = new OwnedArray<byte>(new ArraySegment<byte>(chars, memoryOffset, length * 3));
                var current = new BufferSegment(ownedMemory, length, length * 2);
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
            return CreateBuffer(inputs.Select(i => Encoding.ASCII.GetBytes(i)).ToArray());
        }

        public static ReadableBuffer CreateBuffer(params int[] inputs)
        {
            return CreateBuffer(inputs.Select(i => new byte[i]).ToArray());
        }
    }
}