// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Buffers;
using System.IO.Pipelines;
using System.Text.Http.Parser;
using System.Threading.Tasks;

// SocketClient is an experimental low-allocating/low-copy HTTP client API
// TODO (pri 2): need to support cancellations
namespace System.Net.Experimental
{
    public readonly struct PipeHttpClient 
    {
        static readonly HttpParser s_headersParser = new HttpParser();

        readonly IDuplexPipe _pipe;

        public PipeHttpClient(IDuplexPipe pipe)
        {
            _pipe = pipe;
        }

        public async ValueTask<TResponse> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IPipeWritable
            where TResponse : IHttpResponseHandler, new()
        {
            await request.WriteAsync(_pipe.Output).ConfigureAwait(false);

            PipeReader reader = _pipe.Input;
            TResponse response = await ParseResponseAsync<TResponse>(reader).ConfigureAwait(false);
            await response.OnBody(reader);
            return response;
        }

        static async ValueTask<T> ParseResponseAsync<T>(PipeReader reader)
            where T : IHttpResponseHandler, new()
        {
            var handler = new T();
            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;
                // TODO (pri 2): this should not be static, or ParseHeaders should be static
                if (HttpParser.ParseResponseLine(ref handler, ref buffer, out int rlConsumed))
                {
                    reader.AdvanceTo(buffer.GetPosition(rlConsumed));
                    break;
                }
                reader.AdvanceTo(buffer.Start, buffer.End);
            }

            while (true)
            {
                ReadResult result = await reader.ReadAsync();
                ReadOnlySequence<byte> buffer = result.Buffer;
                if (s_headersParser.ParseHeaders(ref handler, buffer, out int hdConsumed))
                {
                    reader.AdvanceTo(buffer.GetPosition(hdConsumed));
                    break;
                }
                reader.AdvanceTo(buffer.Start, buffer.End);
            }

            await handler.OnBody(reader);
            return handler;
        }

        public bool IsConnected => _pipe != null;
    }

    public interface IHttpResponseHandler : IHttpHeadersHandler, IHttpResponseLineHandler
    {
        ValueTask OnBody(PipeReader body);
    }
}



