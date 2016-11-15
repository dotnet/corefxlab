// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime;
using System.Text;

namespace System.IO.Pipelines.Text.Primitives
{
    // These APIs suck since you can't pass structs by ref to extension methods and they are mutable structs...
    public static class WritableBufferExtensions
    {
        private static readonly byte[] HexChars = new byte[] {
                (byte)'0', (byte)'1', (byte)'2', (byte)'3', (byte)'4', (byte)'5', (byte)'6',
                (byte)'7', (byte)'8', (byte)'9', (byte)'a', (byte)'b', (byte)'c', (byte)'d',
                (byte)'e', (byte)'f'
            };

        private static readonly Encoding Utf8Encoding = Encoding.UTF8;
        private static readonly Encoding ASCIIEncoding = Encoding.ASCII;

        public static void WriteAsciiString(this WritableBuffer buffer, string value)
            => WriteString(buffer, value, ASCIIEncoding);

        public static void WriteUtf8String(this WritableBuffer buffer, string value)
            => WriteString(buffer, value, Utf8Encoding);

        // review: make public?
        private static unsafe void WriteString(this WritableBuffer buffer, string value, Encoding encoding)
        {
            int bytesPerChar = encoding.GetMaxByteCount(1);
            fixed (char* s = value)
            {
                int remainingChars = value.Length, charOffset = 0;
                while (remainingChars != 0)
                {
                    buffer.Ensure(bytesPerChar);

                    var memory = buffer.Memory;
                    var charsThisBatch = Math.Min(remainingChars, memory.Length / bytesPerChar);
                    int bytesWritten = 0;

                    void* pointer;
                    ArraySegment<byte> data;
                    if (memory.TryGetPointer(out pointer))
                    {
                        bytesWritten = encoding.GetBytes(s + charOffset, charsThisBatch,
                        (byte*)pointer, memory.Length);
                    }
                    else if (memory.TryGetArray(out data))
                    {
                        bytesWritten = encoding.GetBytes(value, charOffset, charsThisBatch, data.Array, data.Offset);
                    }

                    charOffset += charsThisBatch;
                    remainingChars -= charsThisBatch;
                    buffer.Advance(bytesWritten);
                }
            }
        }

        public unsafe static void WriteHex(this WritableBuffer buffer, int value)
        {
            if (value < 16)
            {
                buffer.Write(new Span<byte>(HexChars, value, 1));
                return;
            }

            // TODO: Don't use 2 passes
            int length = 0;
            var val = value;
            while (val > 0)
            {
                val >>= 4;
                length++;
            }

            // Allocate space for writing the hex number
            byte* digits = stackalloc byte[length];
            var span = new Span<byte>(digits, length);
            int index = span.Length - 1;

            while (value > 0)
            {
                span[index--] = HexChars[value & 0x0f];
                value >>= 4;
            }

            // Write the span to the buffer
            buffer.Write(span);
        }

        // REVIEW: See if we can use IFormatter here
        public static void WriteUInt32(this WritableBuffer buffer, uint value) => WriteUInt64(buffer, value);

        public static void WriteUInt64(this WritableBuffer buffer, ulong value)
        {
            // optimized versions for 0-1000
            int len;
            if (value < 10)
            {
                buffer.Ensure(len = 1);
                var span = buffer.Memory.Span;
                span[0] = (byte)('0' + value);
            }
            else if (value < 100)
            {
                buffer.Ensure(len = 2);
                var span = buffer.Memory.Span;
                span[0] = (byte)('0' + value / 10);
                span[1] = (byte)('0' + value % 10);
            }
            else if (value < 1000)
            {
                buffer.Ensure(len = 3);
                var span = buffer.Memory.Span;
                span[2] = (byte)('0' + value % 10);
                value /= 10;
                span[0] = (byte)('0' + value / 10);
                span[1] = (byte)('0' + value % 10);
            }
            else
            {

                // more generic version for all other numbers; first find the number of digits;
                // lost of ways to do this, but: http://stackoverflow.com/a/6655759/23354
                ulong remaining = value;
                len = 1;
                if (remaining >= 10000000000000000) { remaining /= 10000000000000000; len += 16; }
                if (remaining >= 100000000) { remaining /= 100000000; len += 8; }
                if (remaining >= 10000) { remaining /= 10000; len += 4; }
                if (remaining >= 100) { remaining /= 100; len += 2; }
                if (remaining >= 10) { remaining /= 10; len += 1; }
                buffer.Ensure(len);

                // now we'll walk *backwards* from the last character, adding the digit each time
                // and dividing by 10
                int index = len - 1;
                var span = buffer.Memory.Span;

                do
                {
                    span[index--] = (byte)('0' + value % 10);
                    value /= 10;
                } while (value != 0);
            }
            buffer.Advance(len);
        }
    }
}
