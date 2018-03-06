// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineReaderExtensions
    {
        public static ValueTask<int> ReadAsync(this PipeReader input, ArraySegment<byte> destination)
        {
            while (true)
            {
                var awaiter = input.ReadAsync();

                if (!awaiter.IsCompleted)
                {
                    break;
                }

                var result = awaiter.GetAwaiter().GetResult();
                var inputBuffer = result.Buffer;

                var length = (int) Math.Min(inputBuffer.Length, destination.Count);

                var sliced = inputBuffer.Slice(0, length);
                sliced.CopyTo(destination);

                input.AdvanceTo(sliced.End);

                if (length != 0)
                {
                    return new ValueTask<int>(length);
                }

                if (result.IsCompleted)
                {
                    return new ValueTask<int>(0);
                }
            }

            return new ValueTask<int>(input.ReadAsyncAwaited(destination));
        }

        public static async Task CopyToAsync(this PipeReader input, PipeWriter output)
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

                    var buffer = output;

                    foreach (var memory in inputBuffer)
                    {
                        buffer.Write(memory.Span);
                    }


                    await buffer.FlushAsync();
                }
                finally
                {
                    input.AdvanceTo(inputBuffer.End);
                }
            }
        }

        private static async Task<int> ReadAsyncAwaited(this PipeReader input, ArraySegment<byte> destination)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;

                var length = (int)Math.Min(inputBuffer.Length, destination.Count);
                var sliced = inputBuffer.Slice(0, length);
                sliced.CopyTo(destination);
                input.AdvanceTo(sliced.End);

                if (length != 0)
                {
                    return length;
                }

                if (result.IsCompleted)
                {
                    return 0;
                }
            }
        }

        public static Task CopyToAsync(this PipeReader input, Stream stream)
        {
            return input.CopyToAsync(stream, 4096, CancellationToken.None);
        }

        public static async Task CopyToAsync(this PipeReader input, Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            // TODO: Use bufferSize argument
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;
                try
                {
                    if (inputBuffer.IsEmpty && result.IsCompleted)
                    {
                        return;
                    }

                    await inputBuffer.CopyToAsync(stream);
                }
                finally
                {
                    input.AdvanceTo(inputBuffer.End);
                }
            }
        }
    }
}
