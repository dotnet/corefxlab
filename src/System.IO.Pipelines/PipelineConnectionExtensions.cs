// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineConnectionExtensions
    {
        public static Stream GetStream(this IPipeConnection connection)
        {
            return new PipelineConnectionStream(connection);
        }
    }

    public static class PipelineWriterExtensions
    {
        private readonly static Task _completedTask = Task.FromResult(0);

        public static Task WriteAsync(this IPipeWriter output, Span<byte> source)
        {
            var writeBuffer = output.Alloc();
            writeBuffer.Write(source);
            return FlushAsync(writeBuffer);
        }

        private static Task FlushAsync(WritableBuffer writeBuffer)
        {
            var awaitable = writeBuffer.FlushAsync();
            if (awaitable.IsCompleted)
            {
                awaitable.GetResult();
                return _completedTask;
            }

            return FlushAsyncAwaited(awaitable);
        }

        private static async Task FlushAsyncAwaited(WritableBufferAwaitable awaitable)
        {
            await awaitable;
        }
    }

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

                var fin = result.IsCompleted;
                var sliced = inputBuffer.Slice(0, Math.Min(inputBuffer.Length, destination.Length));
                sliced.CopyTo(destination);
                int actual = sliced.Length;
                input.Advance(sliced.End);

                if (actual != 0)
                {
                    return new ValueTask<int>(actual);
                }
                else if (fin)
                {
                    return new ValueTask<int>(0);
                }
            }

            return new ValueTask<int>(input.ReadAsyncAwaited(destination));
        }

        public static Task CopyToEndAsync(this IPipeReader input, Stream stream)
        {
            return input.CopyToEndAsync(stream, 4096, CancellationToken.None);
        }

        public static async Task CopyToEndAsync(this IPipeReader input, Stream stream, int bufferSize, CancellationToken cancellationToken)
        {
            try
            {
                await input.CopyToAsync(stream, bufferSize, cancellationToken);
            }
            catch (Exception ex)
            {
                input.Complete(ex);
                return;
            }
            return;
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

        public static async Task CopyToAsync(this IPipeReader input, IPipeWriter output)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;

                var fin = result.IsCompleted;

                try
                {
                    if (inputBuffer.IsEmpty && fin)
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

                var fin = result.IsCompleted;

                var sliced = inputBuffer.Slice(0, Math.Min(inputBuffer.Length, destination.Length));
                sliced.CopyTo(destination);
                int actual = sliced.Length;
                input.Advance(sliced.End);

                if (actual != 0)
                {
                    return actual;
                }
                else if (fin)
                {
                    return 0;
                }
            }
        }
    }
}
