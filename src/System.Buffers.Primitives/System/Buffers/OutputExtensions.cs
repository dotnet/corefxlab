// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Buffers
{
    public static class OutputExtensions
    {
        public static void Write(this IOutput output, ReadOnlySpan<byte> source)
        {
            var buffer = output.GetMemory();

            // Fast path, try copying to the available memory directly
            if (source.Length <= buffer.Length)
            {
                source.CopyTo(buffer.Span);
                output.Advance(source.Length);
                return;
            }

            var remaining = source.Length;
            var offset = 0;

            while (remaining > 0)
            {
                var writable = Math.Min(remaining, buffer.Length);

                buffer = output.GetMemory(writable);

                if (writable == 0)
                {
                    continue;
                }

                source.Slice(offset, writable).CopyTo(buffer.Span);

                remaining -= writable;
                offset += writable;

                output.Advance(writable);
            }
        }
    }
}
