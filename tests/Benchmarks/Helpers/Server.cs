// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Text;
using System.Text.Formatting;
using System.Text.Http;
using System.Text.Http.SingleSegment;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public static class RawInMemoryHttpServer
    {
        public static void RunSingleSegmentParser(int numberOfRequests, int concurrentConnections, byte[] requestPayload, Action<HttpRequestSingleSegment, WritableBuffer> writeResponse)
        {
            var factory = new PipeFactory();
            var listener = new FakeListener(factory, concurrentConnections);

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

                        var requestBytes = input.First.Span;

                        if (requestBytes.Length != 492)
                        {
                            continue;
                        }
                        // Parse the input http request
                        HttpRequestSingleSegment parsedRequest = HttpRequestSingleSegment.Parse(requestBytes);

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

        public static void Run(int numberOfRequests, int concurrentConnections, byte[] requestPayload, Action<HttpRequest, WritableBuffer> writeResponse)
        {
            var factory = new PipeFactory();
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