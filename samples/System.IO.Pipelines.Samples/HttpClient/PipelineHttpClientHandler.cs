// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using System.Text;
using System.Text.Formatting;

namespace System.IO.Pipelines.Samples
{
    public abstract class PipelineHttpClientHandler : HttpClientHandler
    {

        private ConcurrentDictionary<string, ConnectionState> _connectionPool = new ConcurrentDictionary<string, ConnectionState>();

        public PipelineHttpClientHandler()
        {

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var key = request.RequestUri.GetComponents(UriComponents.HostAndPort, UriFormat.SafeUnescaped);
            var path = request.RequestUri.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

            var state = _connectionPool.GetOrAdd(key, k => GetConnection(request));
            var connection = await state.ConnectionTask;

            var requestBuffer = connection.Output.Alloc();
            var output = requestBuffer.AsOutput();
            output.Append($"{request.Method} {path} HTTP/1.1", SymbolTable.InvariantUtf8);
            WriteHeaders(request.Headers, ref output);

            // End of the headers
            output.Append("\r\n\r\n", SymbolTable.InvariantUtf8);

            if (request.Method != HttpMethod.Get && request.Method != HttpMethod.Head)
            {
                WriteHeaders(request.Content.Headers, ref output);

                await requestBuffer.FlushAsync();

                // Copy the body to the input
                var body = await request.Content.ReadAsStreamAsync();

                await body.CopyToAsync(connection.Output);
            }
            else
            {
                await requestBuffer.FlushAsync();
            }

            var response = new HttpResponseMessage();
            response.Content = new PipelineHttpContent(connection.Input);

            await ProduceResponse(state, connection, response);

            // Get off the libuv thread
            await Task.Yield();

            return response;
        }

        private static async Task ProduceResponse(ConnectionState state, IPipeConnection connection, HttpResponseMessage response)
        {
            // TODO: pipelining support!
            while (true)
            {
                var result = await connection.Input.ReadAsync();
                var responseBuffer = result.Buffer;

                var consumed = responseBuffer.Start;

                var needMoreData = true;

                try
                {
                    if (consumed == state.Consumed)
                    {
                        var oldBody = responseBuffer.Slice(0, state.PreviousContentLength);

                        if (oldBody.Length != state.PreviousContentLength)
                        {
                            // Not enough data
                            continue;
                        }

                        // The caller didn't read the body
                        responseBuffer = responseBuffer.Slice(state.PreviousContentLength);
                        consumed = responseBuffer.Start;

                        state.Consumed = default;
                    }

                    if (responseBuffer.IsEmpty && result.IsCompleted)
                    {
                        break;
                    }

                    ReadCursor delim;
                    ReadableBuffer responseLine;
                    if (!responseBuffer.TrySliceTo((byte)'\r', (byte)'\n', out responseLine, out delim))
                    {
                        continue;
                    }

                    responseBuffer = responseBuffer.Slice(delim).Slice(2);

                    ReadableBuffer httpVersion;
                    if (!responseLine.TrySliceTo((byte)' ', out httpVersion, out delim))
                    {
                        // Bad request
                        throw new InvalidOperationException();
                    }

                    consumed = responseBuffer.Start;

                    responseLine = responseLine.Slice(delim).Slice(1);

                    ReadableBuffer statusCode;
                    if (!responseLine.TrySliceTo((byte)' ', out statusCode, out delim))
                    {
                        // Bad request
                        throw new InvalidOperationException();
                    }

                    response.StatusCode = (HttpStatusCode)statusCode.GetUInt32();
                    responseLine = responseLine.Slice(delim).Slice(1);

                    ReadableBuffer remaining;
                    if (!responseLine.TrySliceTo((byte)' ', out remaining, out delim))
                    {
                        // Bad request
                        throw new InvalidOperationException();
                    }

                    while (!responseBuffer.IsEmpty)
                    {
                        if (responseBuffer.Length == 0)
                        {
                            break;
                        }

                        int ch = responseBuffer.First.Span[0];

                        if (ch == '\r')
                        {
                            // Check for final CRLF.
                            responseBuffer = responseBuffer.Slice(1);
                            if (responseBuffer.Length == 0)
                            {
                                break;
                            }
                            ch = responseBuffer.First.Span[0];
                            responseBuffer = responseBuffer.Slice(1);

                            if (ch == '\n')
                            {
                                consumed = responseBuffer.Start;
                                needMoreData = false;
                                break;
                            }

                            // Headers don't end in CRLF line.
                            throw new Exception();
                        }

                        var headerName = default(ReadableBuffer);
                        var headerValue = default(ReadableBuffer);

                        // End of the header
                        // \n
                        ReadableBuffer headerPair;
                        if (!responseBuffer.TrySliceTo((byte)'\n', out headerPair, out delim))
                        {
                            break;
                        }

                        responseBuffer = responseBuffer.Slice(delim).Slice(1);

                        // :
                        if (!headerPair.TrySliceTo((byte)':', out headerName, out delim))
                        {
                            throw new Exception();
                        }

                        headerName = headerName.TrimStart();
                        headerPair = headerPair.Slice(headerName.End).Slice(1);

                        // \r
                        if (!headerPair.TrySliceTo((byte)'\r', out headerValue, out delim))
                        {
                            // Bad request
                            throw new Exception();
                        }

                        headerValue = headerValue.TrimStart();
                        var hKey = headerName.GetAsciiString();
                        var hValue = headerValue.GetAsciiString();

                        if (!response.Content.Headers.TryAddWithoutValidation(hKey, hValue))
                        {
                            response.Headers.TryAddWithoutValidation(hKey, hValue);
                        }

                        // Move the consumed
                        consumed = responseBuffer.Start;
                    }
                }
                catch (Exception ex)
                {
                    // Close the connection
                    connection.Output.Complete(ex);
                    break;
                }
                finally
                {
                    connection.Input.Advance(consumed);
                }

                if (needMoreData)
                {
                    continue;
                }

                // Only handle content length for now
                var length = response.Content.Headers.ContentLength;

                if (!length.HasValue)
                {
                    throw new NotSupportedException();
                }

                checked
                {
                    // BAD but it's a proof of concept ok?
                    state.PreviousContentLength = (int)length.Value;
                    ((PipelineHttpContent)response.Content).ContentLength = (int)length;
                    state.Consumed = consumed;
                }

                break;
            }
        }

        private static void WriteHeaders(HttpHeaders headers, ref PipeOutput buffer)
        {
            foreach (var header in headers)
            {
                buffer.Append($"{header.Key}:{string.Join(",", header.Value)}\r\n", SymbolTable.InvariantUtf8);
            }
        }

        private ConnectionState GetConnection(HttpRequestMessage request)
        {
            var state = new ConnectionState
            {
                ConnectionTask = ConnectAsync(request)
            };

            return state;
        }

        private async Task<IPipeConnection> ConnectAsync(HttpRequestMessage request)
        {
            var addresses = await Dns.GetHostAddressesAsync(request.RequestUri.Host);
            var port = request.RequestUri.Port;
            var address = addresses.First(a => a.AddressFamily == AddressFamily.InterNetwork);
            return await ConnectAsync(new IPEndPoint(address, port));
        }

        protected abstract Task<IPipeConnection> ConnectAsync(IPEndPoint ipEndpoint);

        protected override void Dispose(bool disposing)
        {
            foreach (var state in _connectionPool)
            {
                state.Value.ConnectionTask.GetAwaiter().GetResult().Output.Complete();
            }

            base.Dispose(disposing);
        }

        private class ConnectionState
        {
            public Task<IPipeConnection> ConnectionTask { get; set; }
            public int PreviousContentLength { get; set; }
            public ReadCursor Consumed { get; set; }
        }
    }
}
