using System;
using System.Text.Http;
using System.Text.Utf8;

namespace Microsoft.Net.Http
{
    public class ApiRoutingTable<TRequestId>
    {
        const int tablecapacity = 20;
        Utf8String[] _uris = new Utf8String[tablecapacity];
        TRequestId[] _requestIds = new TRequestId[tablecapacity];
        HttpMethod[] _verbs = new HttpMethod[tablecapacity];
        Action<HttpRequest, HttpResponse>[] _handlers = new Action<HttpRequest, HttpResponse>[tablecapacity];
        int _count;

        public TRequestId GetRequestId(HttpRequestLine requestLine)
        {
            for(int i=0; i<_count; i++) {
                if (requestLine.RequestUri.Equals(_uris[i]) && requestLine.Method == _verbs[i]) return _requestIds[i];
            }
            return default(TRequestId);
        }

        public bool TryHandle(HttpRequest request, HttpResponse response)
        {
            for (int i = 0; i < _count; i++) {
                if (request.RequestLine.RequestUri.Equals(_uris[i]) && request.RequestLine.Method == _verbs[i]) {
                    _handlers[i](request, response);
                    return true;
                }
            }
            return false;
        }

        public void Add(HttpMethod method, Utf8String requestUri, TRequestId requestId, Action<HttpRequest, HttpResponse> handler = null)
        {
            if (_count == tablecapacity) throw new NotImplementedException("ApiReoutingTable does not resize yet.");
            _uris[_count] = requestUri;
            _requestIds[_count] = requestId;
            _verbs[_count] = method;
            _handlers[_count] = handler;
            _count++;
        }

        public void Add(HttpMethod method, string requestUri, TRequestId requestId, Action<HttpRequest, HttpResponse> handler = null)
        {
            Add(method, new Utf8String(requestUri), requestId, handler);
        }
    }
}
