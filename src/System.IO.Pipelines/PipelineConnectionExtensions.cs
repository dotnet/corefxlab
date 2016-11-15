// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Pipelines
{
    public static class PipelineConnectionExtensions
    {
        public static Stream GetStream(this IPipelineConnection connection)
        {
            return new PipelineConnectionStream(connection);
        }
    }

    public static class PipelineWriterExtensions
    {
        public static Task WriteAsync(this IPipelineWriter output, Span<byte> source)
        {
            var writeBuffer = output.Alloc();
            writeBuffer.Write(source);
            return writeBuffer.FlushAsync();
        }
    }

    public static class PipelineReaderExtensions
    {
        public static void Advance(this IPipelineReader input, ReadCursor cursor)
        {
            input.Advance(cursor, cursor);
        }

        public static ValueTask<int> ReadAsync(this IPipelineReader input, Span<byte> destination)
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
                var sliced = inputBuffer.Slice(0, destination.Length);
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

        public static Task CopyToAsync(this IPipelineReader input, Stream stream)
        {
            return input.CopyToAsync(stream, 4096, CancellationToken.None);
        }

        public static async Task CopyToAsync(this IPipelineReader input, Stream stream, int bufferSize, CancellationToken cancellationToken)
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

        public static async Task CopyToAsync(this IPipelineReader input, IPipelineWriter output)
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

        private static async Task<int> ReadAsyncAwaited(this IPipelineReader input, Span<byte> destination)
        {
            while (true)
            {
                var result = await input.ReadAsync();
                var inputBuffer = result.Buffer;

                var fin = result.IsCompleted;

                var sliced = inputBuffer.Slice(0, destination.Length);
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
