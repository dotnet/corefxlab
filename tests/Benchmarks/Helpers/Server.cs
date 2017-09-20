// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Text.Http;
using System.Text.Http.SingleSegment;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public static class RawInMemoryHttpServer
    {
        public delegate void WriteResponseDelegate(HttpRequestSingleSegment request, WritableBuffer buffer);

        public static void RunSingleSegmentParser(int numberOfRequests, int concurrentConnections, byte[] requestPayload, WriteResponseDelegate writeResponse)
        {
            var factory = new MemoryPool();
            var listener = new FakeListener(new MemoryPool(), concurrentConnections);

            listener.OnConnection(async connection =>
            {
                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    ReadableBuffer input = result.Buffer;

                    try
                    {
                        if (input.IsEmpty && result.IsCompleted)
                        {
                            // No more data
                            break;
                        }

                        var requestBuffer = input.First;

                        if (requestBuffer.Length != 492)
                        {
                            continue;
                        }
                        // Parse the input http request
                        WritableBuffer output = WriteResponse(writeResponse, connection, requestBuffer);
                        await output.FlushAsync();
                    }
                    catch (Exception e)
                    {
                        var istr = new Utf8String(input.First.Span).ToString();
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        // Consume the input
                        connection.Input.Advance(input.End, input.End);
                    }
                }
            });

            var tasks = new Task[numberOfRequests];
            for (int i = 0; i < numberOfRequests; i++)
            {
                tasks[i] = listener.ExecuteRequestAsync(requestPayload);
            }

            Task.WaitAll(tasks);

            listener.Dispose();
            factory.Dispose();
        }

        private static WritableBuffer WriteResponse(WriteResponseDelegate writeResponse, IPipeConnection connection, Memory<byte> requestBuffer)
        {
            HttpRequestSingleSegment parsedRequest = HttpRequestSingleSegment.Parse(requestBuffer.Span);

            // Writing directly to pooled buffers
            var output = connection.Output.Alloc();
            writeResponse(parsedRequest, output);
            return output;
        }

        public static void Run(int numberOfRequests, int concurrentConnections, byte[] requestPayload, Action<HttpRequest, WritableBuffer> writeResponse)
        {
            var factory = new MemoryPool();
            var listener = new FakeListener(factory, concurrentConnections);

            listener.OnConnection(async connection => {
                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    ReadableBuffer input = result.Buffer;

                    try
                    {
                        if (input.IsEmpty && result.IsCompleted)
                        {
                            // No more data
                            break;
                        }

                        var requestBytes = input.First;

                        if (requestBytes.Length != 492)
                        {
                            continue;
                        }
                        // Parse the input http request
                        HttpRequest parsedRequest = HttpRequest.Parse(new ReadOnlyBytes(requestBytes));

                        // Writing directly to pooled buffers
                        var output = connection.Output.Alloc();
                        writeResponse(parsedRequest, output);
                        await output.FlushAsync();
                    }
                    catch (Exception e)
                    {
                        var istr = new Utf8String(input.First.Span).ToString();
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        // Consume the input
                        connection.Input.Advance(input.End, input.End);
                    }
                }
            });

            var tasks = new Task[numberOfRequests];
            for (int i = 0; i < numberOfRequests; i++)
            {
                tasks[i] = listener.ExecuteRequestAsync(requestPayload);
            }

            Task.WaitAll(tasks);

            listener.Dispose();
            factory.Dispose();
        }

    }


}
