// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Parsing;
using System.Text.Utf8;
using System.Text.Internal;

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
            if(!buffer.TryParseUInt32(out value, out consumed))
            {
                throw new InvalidOperationException("could not parse uint");
            }
            return value;
        }

        /// <summary>
        /// Parses a <see cref="ulong"/> from the specified <see cref="ReadableBuffer"/>
        /// </summary>
        /// <param name="buffer">The <see cref="ReadableBuffer"/> to parse</param>
        public unsafe static ulong GetUInt64(this ReadableBuffer buffer)
        {
            byte* addr;
            ulong value;
            int consumed, len = buffer.Length;
            if (buffer.IsSingleSpan)
            {
                // It fits!
                void* pointer;
                ArraySegment<byte> data;
                if (buffer.First.TryGetPointer(out pointer))
                {
                    if (!InternalParser.TryParseUInt64((byte*)pointer, 0, len, EncodingData.InvariantUtf8, TextFormat.HexUppercase, out value, out consumed))
                    {
                        throw new InvalidOperationException();
                    }
                }
                else if (buffer.First.TryGetArray(out data))
                {
                    if (!InternalParser.TryParseUInt64(data.Array, 0, EncodingData.InvariantUtf8, TextFormat.HexUppercase, out value, out consumed))
                    {
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            else if (len < 128) // REVIEW: What's a good number
            {
                var data = stackalloc byte[len];
                buffer.CopyTo(new Span<byte>(data, len));
                addr = data;

                if (!InternalParser.TryParseUInt64(addr, 0, len, EncodingData.InvariantUtf8, TextFormat.HexUppercase, out value, out consumed))
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
                // Heap allocated copy to parse into array (should be rare)
                var arr = buffer.ToArray();
                if (!InternalParser.TryParseUInt64(arr, 0, EncodingData.InvariantUtf8, TextFormat.HexUppercase, out value, out consumed))
                {
                    throw new InvalidOperationException();
                }

                return value;
            }

            return value;
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

            var asciiString = new string('\0', buffer.Length);

            fixed (char* outputStart = asciiString)
            {
                int offset = 0;
                var output = outputStart;

                foreach (var memory in buffer)
                {
                    void* pointer;
                    if (memory.TryGetPointer(out pointer))
                    {
                        if (!AsciiUtilities.TryGetAsciiString((byte*)pointer, output + offset, memory.Length))
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        ArraySegment<byte> data;
                        if (memory.TryGetArray(out data))
                        {
                            fixed (byte* ptr = &data.Array[0])
                            {
                                if (!AsciiUtilities.TryGetAsciiString(ptr + data.Offset, output + offset, memory.Length))
                                {
                                    throw new InvalidOperationException();
                                }
                            }
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
        public static unsafe string GetUtf8String(this ReadableBuffer buffer)
        {
            if (buffer.IsEmpty)
            {
                return null;
            }

            ReadOnlySpan<byte> textSpan;

            if (buffer.IsSingleSpan)
            {
                textSpan = buffer.First.Span;
            }
            else if (buffer.Length < 128) // REVIEW: What's a good number
            {
                var data = stackalloc byte[128];
                var destination = new Span<byte>(data, 128);

                buffer.CopyTo(destination);

                textSpan = destination.Slice(0, buffer.Length);
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
