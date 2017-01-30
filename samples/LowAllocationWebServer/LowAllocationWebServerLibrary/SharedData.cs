using System;
using System.Buffers;
using System.Collections.Sequences;
using System.Text;
using System.Text.Formatting;

namespace Microsoft.Net.Http
{
    public class HttpResponse : IDisposable
    {
        ReadWriteBytes _headers;
        ReadWriteBytes _body;
        OwnedArray<byte> _headersMemory;
        OwnedArray<byte> _bodyMemory;
        int _writtenHeaders;
        int _writtenBody;

        public HttpResponse(int size)
        {
            var headersArray = ArrayPool<byte>.Shared.Rent(size / 2);
            _headersMemory = new OwnedArray<byte>(headersArray);
            _headers = new ReadWriteBytes(_headersMemory.Memory);

            var bodyArray = ArrayPool<byte>.Shared.Rent(size / 2);
            _bodyMemory = new OwnedArray<byte>(bodyArray);
            _body = new ReadWriteBytes(_bodyMemory.Memory);
        }

        public ReadWriteBytes Headers => _headers;

        public ReadWriteBytes Body => _body;

        public int BodyLength => _writtenBody;
        public int HeadersLength => _writtenHeaders;

        public Memory<byte> FreeHeaders => _headers.First.Slice(_writtenHeaders);
        public Memory<byte> FreeBody => _body.First.Slice(_writtenBody);

        public void Dispose()
        {
            var headersArray = _headersMemory.Array;
            _headersMemory.Dispose();
            ArrayPool<byte>.Shared.Return(headersArray);
            var bodyArray = _bodyMemory.Array;
            _bodyMemory.Dispose();
            ArrayPool<byte>.Shared.Return(bodyArray);
        }

        internal void AdvanceBody(int bytes)
        {
            _writtenBody += bytes;
        }

        internal void AdvanceHeaders(int bytes)
        {
            _writtenHeaders += bytes;
        }
    }

    public struct ResponseFormatter : ITextOutput
    {
        HttpResponse _response;
        bool _formatBody;

        public ResponseFormatter(HttpResponse response, bool formatBody)
        {
            _response = response;
            _formatBody = formatBody;
        }

        public EncodingData Encoding => EncodingData.InvariantUtf8;

        public Span<byte> Buffer => _formatBody ? _response.FreeBody.Span : _response.FreeHeaders.Span;

        public void Advance(int bytes)
        {
            if (_formatBody) _response.AdvanceBody(bytes);
            else _response.AdvanceHeaders(bytes);
        }

        public void Enlarge(int desiredBufferLength = 0)
        {
            throw new NotImplementedException("resizing not implemented");
        }
    }
}
