// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Parsing;
using System.Text.Utf8;

namespace System.IO.Pipelines.Text.Primitives
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ReadableBufferExtensions
    {
        /// <summary>
        /// Trim whitespace starting from the specified <see cref="ReadableBuffer"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to trim</param>
        /// <returns>A new <see cref="ReadableBuffer"/> with the starting whitespace trimmed.</returns>
        public static ReadableBuffer TrimStart(this ReadableBuffer buffer)
        {
            int start = 0;
            foreach (var memory in buffer)
            {
                var span = memory.Span;
                for (int i = 0; i < span.Length; i++)
                {
                    if (!IsWhitespaceChar(span[i]))
                    {
                        break;
                    }

                    start++;
                }
            }

            return buffer.Slice(start);
        }

        /// <summary>
        /// Trim whitespace starting from the specified <see cref="ReadableBuffer"/>.
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to trim</param>
        /// <returns>A new <see cref="ReadableBuffer"/> with the starting whitespace trimmed.</returns>
        public static ReadableBuffer TrimEnd(this ReadableBuffer buffer)
        {
            var end = -1;
            var i = 0;
            foreach (var memory in buffer)
            {
                var span = memory.Span;
                for (int j = 0; j < span.Length; j++)
                {
                    i++;
                    if (IsWhitespaceChar(span[j]))
                    {
                        if (end == -1)
                        {
                            end = i;
                        }
                    }
                    else
                    {
                        end = -1;
                    }
                }
            }

            return end != -1 ? buffer.Slice(0, end - 1) : buffer;
        }

        private static bool IsWhitespaceChar(int ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\n' || ch == '\r';
        }

        /// <summary>
        /// Parses a <see cref="uint"/> from the specified <see cref="ReadableBuffer"/>
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to parse</param>
        public static uint GetUInt32(this ReadableBuffer buffer)
        {
            uint value;
            int consumed;
            if (!buffer.AsSequence().TryParseUInt32(out value, out consumed))
            {
                throw new InvalidOperationException("could not parse uint");
            }
            return value;
        }

        /// <summary>
        /// Parses a <see cref="ulong"/> from the specified <see cref="ReadableBuffer"/>
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to parse</param>
        public static ulong GetUInt64(this ReadableBuffer buffer)
        {
            ulong value;
            if (Utf8Parser.TryParseUInt64(buffer.First.Span, out value))
            {
                return value;
            }

            if (buffer.IsSingleSpan) // no more data to parse
            {
                throw new InvalidOperationException();
            }

            var bufferLength = buffer.Length;

            int toParseLength = 21; // longest invariant UTF8 UInt64 + 1 (so we know there is a delimiter at teh end)
            if (bufferLength < 21) toParseLength = (int)bufferLength;

            Span<byte> toParseBuffer = stackalloc byte[toParseLength];
            buffer.CopyTo(toParseBuffer);

            if (Utf8Parser.TryParseUInt64(toParseBuffer, out value))
            {
                return value;
            }

            throw new InvalidOperationException();
        }

        [Obsolete("Use System.Text.Encoders.Ascii.ToUtf16String method")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe static string GetAsciiString(this Span<byte> span)
        {
            var len = span.Length;
            if (len == 0) {
                return null;
            }

            var asciiString = new string('\0', len);

            fixed (char* destination = asciiString)
            fixed (byte* source = &span.DangerousGetPinnableReference()) {
                if (!AsciiUtilities.TryGetAsciiString(source, destination, len)) {
                    ThrowInvalidOperation();
                }
            }

            return asciiString;
        }

        static void ThrowInvalidOperation()
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Decodes the ASCII encoded bytes in the <see cref="ReadableBuffer"/> into a <see cref="string"/>
        /// </summary>
        /// <param name="buffer">The buffer to decode</param>
        public unsafe static string GetAsciiString(this ReadableBuffer buffer)
        {
            if (buffer.IsEmpty)
            {
                return null;
            }

            var asciiString = new string('\0', (int) Math.Min(int.MaxValue, buffer.Length));

            fixed (char* outputStart = asciiString)
            {
                int offset = 0;
                var output = outputStart;

                foreach (var memory in buffer)
                {
                    fixed (byte* source = &memory.Span.DangerousGetPinnableReference())
                    {
                        if (!AsciiUtilities.TryGetAsciiString(source, output + offset, memory.Length))
                        {
                            ThrowInvalidOperation();
                        }
                    }

                    offset += memory.Length;
                }
            }

            return asciiString;
        }

        /// <summary>
        /// Decodes the utf8 encoded bytes in the <see cref="ReadableBuffer"/> into a <see cref="string"/>
        /// </summary>
        /// <param name="buffer">The buffer to decode</param>
        public static string GetUtf8String(this ReadableBuffer buffer)
        {
            if (buffer.IsEmpty)
            {
                return null;
            }

            // Assign 'textSpan' to something formally stack-referring.
            // The default classification is "returnable, not referring to stack", we want the opposite in this case.
            ReadOnlySpan<byte> textSpan = stackalloc byte[0];

            if (buffer.IsSingleSpan)
            {
                textSpan = buffer.First.Span;
            }
            else if (buffer.Length < 128) // REVIEW: What's a good number
            {
                Span<byte> destination = stackalloc byte[128];
                buffer.CopyTo(destination);

                // We are able to cast because buffer.Length < 128
                textSpan = destination.Slice(0, (int) buffer.Length);
            }
            else
            {
                // Heap allocated copy to parse into array (should be rare)
                textSpan = new ReadOnlySpan<byte>(buffer.ToArray());
            }

            return new Utf8String(textSpan).ToString();
        }

        /// <summary>
        /// Split a buffer into a sequence of tokens using a delimiter
        /// </summary>
        public static SplitEnumerable Split(this ReadableBuffer buffer, byte delimiter)
            => new SplitEnumerable(buffer, delimiter);
    }
}
