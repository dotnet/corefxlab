// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.Diagnostics;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public static class RawInMemoryHttpServer
    {
        public static void Run(int numberOfRequests, int concurrentConnections, byte[] requestPayload, Action<object, PipeWriter> writeResponse)
        {
            var listener = new FakeListener(MemoryPool<byte>.Shared, concurrentConnections);

            listener.OnConnection(async connection => {
                while (true)
                {
                    // Wait for data
                    var result = await connection.Input.ReadAsync();
                    ReadOnlySequence<byte> input = result.Buffer;

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
                        var output = connection.Output;
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
                        connection.Input.AdvanceTo(input.End, input.End);
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
        }

    }


}
