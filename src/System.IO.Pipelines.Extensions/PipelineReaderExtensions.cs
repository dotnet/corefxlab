// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineReaderExtensions
    {
        public static ValueTask<int> ReadAsync(this IPipeReader input, ArraySegment<byte> destination)
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

                var length = (int) Math.Min(inputBuffer.Length, destination.Count);

                var sliced = inputBuffer.Slice(0, length);
                sliced.CopyTo(destination);

                input.Advance(sliced.End);

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

        private static async Task<int> ReadAsyncAwaited(this IPipeReader input, ArraySegment<byte> destination)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;

                var length = (int)Math.Min(inputBuffer.Length, destination.Count);
                var sliced = inputBuffer.Slice(0, length);
                sliced.CopyTo(destination);
                input.Advance(sliced.End);

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

        public static Task CopyToAsync(this IPipeReader input, Stream stream)
        {
            return input.CopyToAsync(stream, 4096, CancellationToken.None);
        }

        public static async Task CopyToAsync(this IPipeReader input, Stream stream, int bufferSize, CancellationToken cancellationToken)
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
                    input.Advance(inputBuffer.End);
                }
            }
        }
    }
}
