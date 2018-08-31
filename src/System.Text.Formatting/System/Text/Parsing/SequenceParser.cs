// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Buffers.Text;
using System.Collections.Sequences;

namespace System.Text.Parsing
{
    public static class TextSequenceExtensions
    {
        const int StackBufferSize = 128;

        public static bool TryParseUInt64(this ReadOnlySequence<byte> bufferSequence, out ulong value, out int consumed)
        {
            value = default;
            consumed = default;
            SequencePosition position = default;

            // Fetch the first segment
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> first))
            {
                return false;
            }

            // Attempt to parse the first segment. If it works (and it should in most cases), then return success.
            bool parsed = Utf8Parser.TryParse(first.Span, out value, out consumed);
            if (parsed && consumed < first.Length)
            {
                return true;
            }

            // Apparently the we need data from the second segment to succesfully parse, and so fetch the second segment.
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> second))
            {
                // if there is no second segment and the first parsed succesfully, return the result of the parsing.
                if (parsed) return true;
                return false;
            }

            // Combine the first, the second, and potentially more segments into a stack allocated buffer
            if (first.Length < StackBufferSize)
            {
                Span<byte> destination = stackalloc byte[StackBufferSize];

                first.Span.CopyTo(destination);
                var free = destination.Slice(first.Length);

                if (second.Length > free.Length) second = second.Slice(0, free.Length);
                second.Span.CopyTo(free);
                free = free.Slice(second.Length);

                while (free.Length > 0)
                {
                    if (bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> next))
                    {
                        if (next.Length > free.Length) next = next.Slice(0, free.Length);
                        next.Span.CopyTo(free);
                        free = free.Slice(next.Length);
                    }
                    else
                    {
                        break;
                    }
                }

                var combinedStackSpan = destination.Slice(0, StackBufferSize - free.Length);

                // if the stack allocated buffer parsed succesfully (and for uint it should always do), then return success. 
                if (Utf8Parser.TryParse(combinedStackSpan, out value, out consumed))
                {
                    if (consumed < combinedStackSpan.Length || combinedStackSpan.Length < StackBufferSize)
                    {
                        return true;
                    }
                }
            }

            // for invariant culture, we should never reach this point, as invariant uint text is never longer than 127 bytes. 
            // I left this code here, as we will need it for custom cultures and possibly when we shrink the stack allocated buffer.
            var combinedSpan = bufferSequence.ToSpan();
            if (!Utf8Parser.TryParse(combinedSpan, out value, out consumed))
            {
                return false;
            }
            return true;
        }

        public static bool TryParseUInt32(this ReadOnlySequence<byte> bufferSequence, out uint value, out int consumed)
        {
            value = default;
            consumed = default;
            SequencePosition position = default;

            // Fetch the first segment
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> first))
            {
                return false;
            }

            // Attempt to parse the first segment. If it works (and it should in most cases), then return success.
            bool parsed = Utf8Parser.TryParse(first.Span, out value, out consumed);
            if (parsed && consumed < first.Length)
            {
                return true;
            }

            // Apparently the we need data from the second segment to succesfully parse, and so fetch the second segment.
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> second))
            {
                // if there is no second segment and the first parsed succesfully, return the result of the parsing.
                if (parsed) return true;
                return false;
            }

            // Combine the first, the second, and potentially more segments into a stack allocated buffer
            if (first.Length < StackBufferSize)
            {

                Span<byte> destination = stackalloc byte[StackBufferSize];

                first.Span.CopyTo(destination);
                var free = destination.Slice(first.Length);

                if (second.Length > free.Length) second = second.Slice(0, free.Length);
                second.Span.CopyTo(free);
                free = free.Slice(second.Length);

                while (free.Length > 0)
                {
                    if (bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> next))
                    {
                        if (next.Length > free.Length) next = next.Slice(0, free.Length);
                        next.Span.CopyTo(free);
                        free = free.Slice(next.Length);
                    }
                    else
                    {
                        break;
                    }
                }

                var combinedStackSpan = destination.Slice(0, StackBufferSize - free.Length);

                // if the stack allocated buffer parsed succesfully (and for uint it should always do), then return success. 
                if (Utf8Parser.TryParse(combinedStackSpan, out value, out consumed))
                {
                    if (consumed < combinedStackSpan.Length || combinedStackSpan.Length < StackBufferSize)
                    {
                        return true;
                    }
                }
            }

            // for invariant culture, we should never reach this point, as invariant uint text is never longer than 127 bytes. 
            // I left this code here, as we will need it for custom cultures and possibly when we shrink the stack allocated buffer.
            var combinedSpan = bufferSequence.ToSpan();
            if (!Utf8Parser.TryParse(combinedSpan, out value, out consumed))
            {
                return false;
            }
            return true;
        }

        public static bool TryParseUInt64<T>(this T bufferSequence, out ulong value, out int consumed) where T : ISequence<ReadOnlyMemory<byte>>
        {
            value = default;
            consumed = default;
            SequencePosition position = default;

            // Fetch the first segment
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> first))
            {
                return false;
            }

            // Attempt to parse the first segment. If it works (and it should in most cases), then return success.
            bool parsed = Utf8Parser.TryParse(first.Span, out value, out consumed);
            if (parsed && consumed < first.Length)
            {
                return true;
            }

            // Apparently the we need data from the second segment to succesfully parse, and so fetch the second segment.
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> second))
            {
                // if there is no second segment and the first parsed succesfully, return the result of the parsing.
                if (parsed) return true;
                return false;
            }

            // Combine the first, the second, and potentially more segments into a stack allocated buffer
            if (first.Length < StackBufferSize)
            {
                Span<byte> destination = stackalloc byte[StackBufferSize];

                first.Span.CopyTo(destination);
                var free = destination.Slice(first.Length);

                if (second.Length > free.Length) second = second.Slice(0, free.Length);
                second.Span.CopyTo(free);
                free = free.Slice(second.Length);

                while (free.Length > 0)
                {
                    if (bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> next))
                    {
                        if (next.Length > free.Length) next = next.Slice(0, free.Length);
                        next.Span.CopyTo(free);
                        free = free.Slice(next.Length);
                    }
                    else
                    {
                        break;
                    }
                }

                var combinedStackSpan = destination.Slice(0, StackBufferSize - free.Length);

                // if the stack allocated buffer parsed succesfully (and for uint it should always do), then return success. 
                if (Utf8Parser.TryParse(combinedStackSpan, out value, out consumed))
                {
                    if (consumed < combinedStackSpan.Length || combinedStackSpan.Length < StackBufferSize)
                    {
                        return true;
                    }
                }
            }

            // for invariant culture, we should never reach this point, as invariant uint text is never longer than 127 bytes. 
            // I left this code here, as we will need it for custom cultures and possibly when we shrink the stack allocated buffer.
            var combinedSpan = bufferSequence.ToSpan();
            if (!Utf8Parser.TryParse(combinedSpan, out value, out consumed))
            {
                return false;
            }
            return true;
        }

        public static bool TryParseUInt32<T>(this T bufferSequence, out uint value, out int consumed) where T : ISequence<ReadOnlyMemory<byte>>
        {
            value = default;
            consumed = default;
            SequencePosition position = default;

            // Fetch the first segment
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> first))
            {
                return false;
            }

            // Attempt to parse the first segment. If it works (and it should in most cases), then return success.
            bool parsed = Utf8Parser.TryParse(first.Span, out value, out consumed);
            if (parsed && consumed < first.Length)
            {
                return true;
            }

            // Apparently the we need data from the second segment to succesfully parse, and so fetch the second segment.
            if (!bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> second))
            {
                // if there is no second segment and the first parsed succesfully, return the result of the parsing.
                if (parsed) return true;
                return false;
            }

            // Combine the first, the second, and potentially more segments into a stack allocated buffer
            if (first.Length < StackBufferSize)
            {

                Span<byte> destination = stackalloc byte[StackBufferSize];

                first.Span.CopyTo(destination);
                var free = destination.Slice(first.Length);

                if (second.Length > free.Length) second = second.Slice(0, free.Length);
                second.Span.CopyTo(free);
                free = free.Slice(second.Length);

                while (free.Length > 0)
                {
                    if (bufferSequence.TryGet(ref position, out ReadOnlyMemory<byte> next))
                    {
                        if (next.Length > free.Length) next = next.Slice(0, free.Length);
                        next.Span.CopyTo(free);
                        free = free.Slice(next.Length);
                    }
                    else
                    {
                        break;
                    }
                }

                var combinedStackSpan = destination.Slice(0, StackBufferSize - free.Length);

                // if the stack allocated buffer parsed succesfully (and for uint it should always do), then return success. 
                if (Utf8Parser.TryParse(combinedStackSpan, out value, out consumed))
                {
                    if (consumed < combinedStackSpan.Length || combinedStackSpan.Length < StackBufferSize)
                    {
                        return true;
                    }
                }
            }

            // for invariant culture, we should never reach this point, as invariant uint text is never longer than 127 bytes. 
            // I left this code here, as we will need it for custom cultures and possibly when we shrink the stack allocated buffer.
            var combinedSpan = bufferSequence.ToSpan();
            if (!Utf8Parser.TryParse(combinedSpan, out value, out consumed))
            {
                return false;
            }
            return true;
        }
    }
}
