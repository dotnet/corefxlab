using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipelines.Text.Primitives;
using Microsoft.AspNetCore.Hosting.Server;
using System.Text.Formatting;

namespace System.IO.Pipelines.Samples.Http
{
    public partial class HttpConnection<TContext>
    {
        private static readonly byte[] _http11Bytes = Encoding.UTF8.GetBytes("HTTP/1.1 ");
        private static readonly byte[] _chunkedEndBytes = Encoding.UTF8.GetBytes("0\r\n\r\n");
        private static readonly byte[] _endChunkBytes = Encoding.ASCII.GetBytes("\r\n");

        private readonly IPipelineReader _input;
        private readonly IPipelineWriter _output;
        private readonly IHttpApplication<TContext> _application;

        public RequestHeaderDictionary RequestHeaders => _parser.RequestHeaders;
        public ResponseHeaderDictionary ResponseHeaders { get; } = new ResponseHeaderDictionary();

        public ReadableBuffer HttpVersion => _parser.HttpVersion;
        public ReadableBuffer Path => _parser.Path;
        public ReadableBuffer Method => _parser.Method;

        // TODO: Check the http version
        public bool KeepAlive => true; //RequestHeaders.ContainsKey("Connection") && string.Equals(RequestHeaders["Connection"], "keep-alive");

        private bool HasContentLength => ResponseHeaders.ContainsKey("Content-Length");
        private bool HasTransferEncoding => ResponseHeaders.ContainsKey("Transfer-Encoding");

        private HttpRequestStream<TContext> _requestBody;
        private HttpResponseStream<TContext> _responseBody;

        private bool _autoChunk;

        private HttpRequestParser _parser = new HttpRequestParser();

        public HttpConnection(IHttpApplication<TContext> application, IPipelineReader input, IPipelineWriter output)
        {
            _application = application;
            _input = input;
            _output = output;
            _requestBody = new HttpRequestStream<TContext>(this);
            _responseBody = new HttpResponseStream<TContext>(this);
        }

        public IPipelineReader Input => _input;

        public IPipelineWriter Output => _output;

        public HttpRequestStream<TContext> RequestBody { get; set; }

        public HttpResponseStream<TContext> ResponseBody { get; set; }


        public async Task ProcessAllRequests()
        {
            Reset();

            while (true)
            {
                var result = await _input.ReadAsync();
                var buffer = result.Buffer;

                try
                {
                    if (buffer.IsEmpty && result.IsCompleted)
                    {
                        // We're done with this connection
                        return;
                    }

                    var parserResult = _parser.ParseRequest(ref buffer);

                    switch (parserResult)
                    {
                        case HttpRequestParser.ParseResult.Incomplete:
                            if (result.IsCompleted)
                            {
                                // Didn't get the whole request and the connection ended
                                throw new EndOfStreamException();
                            }
                            // Need more data
                            continue;
                        case HttpRequestParser.ParseResult.Complete:
                            // Done
                            break;
                        case HttpRequestParser.ParseResult.BadRequest:
                            // TODO: Don't throw here;
                            throw new Exception();
                        default:
                            break;
                    }

                }
                catch (Exception)
                {
                    StatusCode = 400;

                    await EndResponse();

                    return;
                }
                finally
                {
                    _input.Advance(buffer.Start, buffer.End);
                }

                var context = _application.CreateContext(this);

                try
                {
                    await _application.ProcessRequestAsync(context);
                }
                catch (Exception ex)
                {
                    StatusCode = 500;

                    _application.DisposeContext(context, ex);
                }
                finally
                {
                    await EndResponse();
                }

                if (!KeepAlive)
                {
                    break;
                }

                Reset();
            }
        }

        private Task EndResponse()
        {
            var buffer = _output.Alloc();

            if (!HasStarted)
            {
                WriteBeginResponseHeaders(buffer);
            }

            if (_autoChunk)
            {
                WriteEndResponse(buffer);
            }

            return buffer.FlushAsync();
        }

        private void Reset()
        {
            RequestBody = _requestBody;
            ResponseBody = _responseBody;
            _parser.Reset();
            ResponseHeaders.Reset();
            HasStarted = false;
            StatusCode = 200;
            _autoChunk = false;
            _method = null;
            _path = null;
        }

        public Task WriteAsync(Span<byte> data)
        {
            var buffer = _output.Alloc();

            if (!HasStarted)
            {
                WriteBeginResponseHeaders(buffer);
            }

            if (_autoChunk)
            {
                buffer.Append(data.Length, EncodingData.InvariantUtf8, 'x');
                buffer.Write(_endChunkBytes);
                buffer.Write(data);
                buffer.Write(_endChunkBytes);
            }
            else
            {
                buffer.Write(data);
            }

            return buffer.FlushAsync();
        }

        private void WriteBeginResponseHeaders(WritableBuffer buffer)
        {
            if (HasStarted)
            {
                return;
            }

            HasStarted = true;

            buffer.Write(_http11Bytes);
            var status = ReasonPhrases.ToStatusBytes(StatusCode);
            buffer.Write(status);

            _autoChunk = !HasContentLength && !HasTransferEncoding && KeepAlive;

            ResponseHeaders.CopyTo(_autoChunk, buffer);
        }

        private void WriteEndResponse(WritableBuffer buffer)
        {
            buffer.Write(_chunkedEndBytes);
        }
    }
}
