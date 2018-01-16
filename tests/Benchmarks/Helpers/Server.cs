// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Buffers;
using System.Diagnostics;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public static class RawInMemoryHttpServer
    {
        public static void Run(int numberOfRequests, int concurrentConnections, byte[] requestPayload, Action<object, WritableBuffer> writeResponse)
        {
            var memoryPool = new MemoryPool();
            var listener = new FakeListener(memoryPool, concurrentConnections);

            listener.OnConnection(async connection => {
                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    ReadOnlyBuffer<byte> input = result.Buffer;

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
                        // TODO: use the Kestrel parser here
                        object parsedRequest = null;

                        // Writing directly to pooled buffers
                        var output = connection.Output.Alloc();
                        writeResponse(parsedRequest, output);
                        await output.FlushAsync();
                    }
                    catch (Exception e)
                    {
                        var istr = new Utf8Span(input.First.Span).ToString();
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
            memoryPool.Dispose();
        }

    }


}
