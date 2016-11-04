// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Collections.Sequences;
using System.Text.Utf8;

namespace System.Text.Parsing
{
    public static class TextSequenceExtensions
    {
        /// <summary>
        /// Parses a uint from a sequence of buffers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memorySequence"></param>
        /// <returns></returns>
        public static bool TryParseUInt32<T>(this T memorySequence, out uint value, out int consumed) where T : ISequence<ReadOnlyMemory<byte>>
        {
            value = default(uint);
            consumed = default(int);
            Position position = Position.First;

            ReadOnlyMemory<byte> first;
            if (!memorySequence.TryGet(ref position, out first, advance: true)) {
                return false;
            }

            // attempt to parse
            bool parsed = PrimitiveParser.TryParseUInt32(new Utf8String(first.Span), out value, out consumed);
            if (parsed && consumed < first.Length) {
                return true;
            }

            ReadOnlyMemory<byte> second;
            if (!memorySequence.TryGet(ref position, out second, advance: true)) {
                if (parsed) return true;
                return false;
            }

            ReadOnlySpan<byte> combinedSpan;
            unsafe
            {
                // TODO: this block could be optimized even more. Currently, it only attenpts to combine two spans.
                // It should do it in a loop till it completly filles 128 bytes, at which point
                // the method should never allocate on the GC heap as I am not aware of uints textual representation
                // that is longer than 128 bytes.
                if (first.Length < 128) {
                    var data = stackalloc byte[128];
                    var destination = new Span<byte>(data, 128);
                    first.CopyTo(destination);
                    var remaining = 128 - first.Length;
                    if (remaining > second.Length) remaining = second.Length;
                    second.Slice(0, remaining).CopyTo(destination.Slice(first.Length));
                    combinedSpan = destination.Slice(0, first.Length + remaining);

                    if (PrimitiveParser.TryParseUInt32(new Utf8String(combinedSpan), out value, out consumed)) {
                        if (consumed < combinedSpan.Length) {
                            return true;
                        }
                    }
                }
            }

            combinedSpan = memorySequence.ToSingleSpan();
            if (!PrimitiveParser.TryParseUInt32(new Utf8String(combinedSpan), out value, out consumed)) {
                return false;
            }
            return true;
        }
    }
}