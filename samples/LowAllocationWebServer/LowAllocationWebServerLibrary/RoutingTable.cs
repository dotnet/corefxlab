// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Text;
using System.Text.Http;
using System.Text.Utf8;

namespace Microsoft.Net.Http
{
    public class ApiRoutingTable<TRequestId>
    {
        const int tablecapacity = 20;
        byte[][] _uris = new byte[tablecapacity][];
        TRequestId[] _requestIds = new TRequestId[tablecapacity];
        HttpMethod[] _verbs = new HttpMethod[tablecapacity];
        Action<HttpRequest, TcpConnectionFormatter>[] _handlers = new Action<HttpRequest, TcpConnectionFormatter>[tablecapacity];
        int _count;

        public TRequestId GetRequestId(HttpRequestLine requestLine)
        {
            for(int i=0; i<_count; i++) {
                if (requestLine.RequestUri.Span.SequenceEqual(_uris[i]) && requestLine.Method == _verbs[i]) return _requestIds[i];
            }
            return default;
        }

        public bool TryHandle(HttpRequest request, TcpConnectionFormatter response)
        {
            // TODO: this should not allocate new string
            Utf8String requestUtf8 = request.Path.ToUtf8String(TextEncoder.Utf8);
            for (int i = 0; i < _count; i++)
            {
                // TODO: this should check the verb too
                if (requestUtf8.Equals(new Utf8String(_uris[i])))
                {
                    _handlers[i](request, response);
                    return true;
                }
            }
            return false;
        }

        public void Add(HttpMethod method, string requestUri, TRequestId requestId, Action<HttpRequest, TcpConnectionFormatter> handler = null)
        {
            if (_count == tablecapacity) throw new NotImplementedException("ApiReoutingTable does not resize yet.");
            _uris[_count] = new Utf8String(requestUri).Bytes.ToArray();
            _requestIds[_count] = requestId;
            _verbs[_count] = method;
            _handlers[_count] = handler;
            _count++;
        }
    }
}
