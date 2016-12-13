using System.Diagnostics;
using System.Text;
using System.Text.Formatting;
using System.Text.Utf8;
using System.Threading.Tasks;

namespace System.IO.Pipelines.Samples
{
    public static class RawInMemoryHttpServer
    {
        private static readonly byte[] _request = Encoding.UTF8.GetBytes(@"GET /developer/documentation/data-insertion/r-sample-http-get HTTP/1.1
Host: marketing.adobe.com
Connection: keep-alive
Cache-Control: max-age=0
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.98 Safari/537.36
Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8
Accept-Encoding: gzip, deflate, sdch, br
Accept-Language: en-US,en;q=0.8,it;q=0.6,ms;q=0.4

");

        public static void Run(int numberOfRequests, int concurrentConnections)
        {
            var factory = new PipelineFactory();
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

                        var requestBytes = input.First.Span;

                        if (requestBytes.Length != 492)
                        {
                            continue;
                        }
                        // Parse the input http request
                        var parseResult = System.Text.Http.HttpRequest.Parse(requestBytes);

                        // Writing directly to pooled buffers
                        var output = connection.Output.Alloc();
                        var formatter = new OutputFormatter<WritableBuffer>(output, EncodingData.InvariantUtf8);
                        formatter.Append("HTTP/1.1 200 OK");
                        formatter.Append("\r\nContent-Length: 13");
                        formatter.Append("\r\nContent-Type: text/plain");
                        formatter.Append("\r\n\r\n");
                        formatter.Append("Hello, World!");
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
                tasks[i] = listener.ExecuteRequestAsync(_request);
            }

            Task.WaitAll(tasks);

            listener.Dispose();
            factory.Dispose();
        }
    }
}