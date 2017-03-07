// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineReaderExtensions
    {
        public static void Advance(this IPipeReader input, ReadCursor cursor)
        {
            input.Advance(cursor, cursor);
        }

        public static ValueTask<int> ReadAsync(this IPipeReader input, Span<byte> destination)
        {
            while (true)
            {
                var awaiter = input.ReadAsync();

                if (!awaiter.IsCompleted)
                {
                    break;
                }

                var result = awaiter.GetResult();
                var inputBuffer = result.Buffer;
                
                var sliced = inputBuffer.Slice(0, Math.Min(inputBuffer.Length, destination.Length));
                sliced.CopyTo(destination);

                int actual = sliced.Length;
                input.Advance(sliced.End);

                if (actual != 0)
                {
                    return new ValueTask<int>(actual);
                }

                if (result.IsCompleted)
                {
                    return new ValueTask<int>(0);
                }
            }

            return new ValueTask<int>(input.ReadAsyncAwaited(destination));
        }

        public static async Task CopyToAsync(this IPipeReader input, IPipeWriter output)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;
                
                try
                {
                    if (inputBuffer.IsEmpty && result.IsCompleted)
                    {
                        return;
                    }

                    var buffer = output.Alloc();

                    buffer.Append(inputBuffer);

                    await buffer.FlushAsync();
                }
                finally
                {
                    input.Advance(inputBuffer.End);
                }
            }
        }

        private static async Task<int> ReadAsyncAwaited(this IPipeReader input, Span<byte> destination)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;

                var sliced = inputBuffer.Slice(0, Math.Min(inputBuffer.Length, destination.Length));
                sliced.CopyTo(destination);
                int actual = sliced.Length;
                input.Advance(sliced.End);

                if (actual != 0)
                {
                    return actual;
                }
                else if (result.IsCompleted)
                {
                    return 0;
                }
            }
        }
    }
}